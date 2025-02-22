using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modbus.Device;
using System.IO.Ports;

namespace power_supply_APP.Api
{

    public interface IModbusService
    {
        ushort[] ReadHoldingRegisters(byte slaveId, ushort startAddress, ushort numberOfPoints);
        void WriteSingleRegister(byte slaveId, ushort registerAddress, ushort value);
    }

    public class ModbusService : IModbusService
    {
        private readonly IModbusSerialMaster _modbusMaster;
        private readonly SerialPort _serialPort;

        public ModbusService(string portName, int baudRate)
        {
            _serialPort = new SerialPort(portName, baudRate);
            _serialPort.Open();
            _modbusMaster = ModbusSerialMaster.CreateRtu(_serialPort);
        }

        public ushort[] ReadHoldingRegisters(byte slaveId, ushort startAddress, ushort numberOfPoints)
        {
            return _modbusMaster.ReadHoldingRegisters(slaveId, startAddress, numberOfPoints);
        }

        public void WriteSingleRegister(byte slaveId, ushort registerAddress, ushort value)
        {
            _modbusMaster.WriteSingleRegister(slaveId, registerAddress, value);
        }
    }
}
