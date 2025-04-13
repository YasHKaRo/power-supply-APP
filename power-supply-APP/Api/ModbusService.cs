using system;
using system.io.ports;
using easymodbus;

namespace power_supply_app.api
{
    public interface imodbusservice
    {
        int[] readholdingregisters(int slaveid, int startaddress, int numberofpoints);
        void writesingleregister(int slaveid, int registeraddress, int value);
    }

    public class modbusservice : imodbusservice
    {
        private readonly modbusclient _modbusclient;

        public modbusservice(string portname, int baudrate, parity parity = parity.none, int databits = 8, stopbits stopbits = stopbits.one)
        {
           _modbusclient = new modbusclient(portname)
           {
                baudrate = baudrate,
               parity = (byte)parity,//need to fix
                stopbits = (byte)stopbits,//need to fix 
                connectiontimeout = 2000 // таймаут в мс (можно настроить)
            };
            _modbusclient.connect();
        }

        public int[] readholdingregisters(int slaveid, int startaddress, int numberofpoints)
        {
            _modbusclient.unitidentifier = (byte)slaveid; // устанавливаем идентификатор ведомого устройства
            return _modbusclient.readholdingregisters(startaddress, numberofpoints);
        }

        public void writesingleregister(int slaveid, int registeraddress, int value)
        {
            _modbusclient.unitidentifier = (byte)slaveid;
            _modbusclient.writesingleregister(registeraddress, value);
        }

        public void close()
        {
            _modbusclient.disconnect();
        }
    }
}