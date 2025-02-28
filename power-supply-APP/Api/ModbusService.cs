using System;
using System.IO.Ports;
using EasyModbus;

namespace power_supply_APP.Api
{
    public interface IModbusService
    {
        int[] ReadHoldingRegisters(int slaveId, int startAddress, int numberOfPoints);
        void WriteSingleRegister(int slaveId, int registerAddress, int value);
    }

    public class ModbusService : IModbusService
    {
        private readonly ModbusClient _modbusClient;

        public ModbusService(string portName, int baudRate, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One)
        {
            _modbusClient = new ModbusClient(portName)
            {
                Baudrate = baudRate,
                Parity = (byte)parity,//need to fix
                StopBits = (byte)stopBits,//need to fix 
                ConnectionTimeout = 2000 // Таймаут в мс (можно настроить)
            };
            _modbusClient.Connect();
        }

        public int[] ReadHoldingRegisters(int slaveId, int startAddress, int numberOfPoints)
        {
            _modbusClient.UnitIdentifier = (byte)slaveId; // Устанавливаем идентификатор ведомого устройства
            return _modbusClient.ReadHoldingRegisters(startAddress, numberOfPoints);
        }

        public void WriteSingleRegister(int slaveId, int registerAddress, int value)
        {
            _modbusClient.UnitIdentifier = (byte)slaveId;
            _modbusClient.WriteSingleRegister(registerAddress, value);
        }

        public void Close()
        {
            _modbusClient.Disconnect();
        }
    }
}