using System;
using System.IO.Ports;
using EasyModbus;

namespace power_supply_APP.Api.Modules
{
    public interface Imodbusservice
    {
        int[] readholdingregisters(int slaveid, int startaddress, int numberofpoints);
        void WriteSingleRegister(int slaveid, int registeraddress, int value);
    }

    public class ModbusService : Imodbusservice
    {
        private readonly ModbusClient _modbusclient;

        public ModbusService(string portname, int baudrate, Parity parity = Parity.None, int databits = 8, StopBits stopbits = StopBits.None)
        {
           _modbusclient = new ModbusClient(portname)
           {
                Baudrate = baudrate,
                /*
                Parity = (byte)parity,//need to fix
                StopBits = (byte)StopBits,//need to fix */
                ConnectionTimeout = 2000 // таймаут в мс (можно настроить)
            };
            _modbusclient.Connect();
        }

        public int[] readholdingregisters(int slaveid, int startaddress, int numberofpoints)
        {
            _modbusclient.UnitIdentifier = (byte)slaveid; // устанавливаем идентификатор ведомого устройства
            return _modbusclient.ReadHoldingRegisters(startaddress, numberofpoints);
        }

        public void WriteSingleRegister(int slaveid, int registeraddress, int value)
        {
            _modbusclient.UnitIdentifier = (byte)slaveid;
            _modbusclient.WriteSingleRegister(registeraddress, value);
        }

        public void close()
        {
            _modbusclient.Disconnect();
        }
    }
   

}