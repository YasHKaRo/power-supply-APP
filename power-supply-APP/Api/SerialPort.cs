using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace power_supply_APP.Api
{
    using System;
    using System.IO.Ports;

    public interface IPortService
    {
        void Open();
        void Close();
        void WriteData(string data);
    }

    public class SerialPortService : IPortService
    {
        private readonly SerialPort _serialPort;

        public SerialPortService(string portName, int baudRate)
        {
            _serialPort = new SerialPort(portName, baudRate);
        }

        public void Open() => _serialPort.Open();
        public void Close() => _serialPort.Close();
        public void WriteData(string data)
        {
            if (_serialPort.IsOpen)
                _serialPort.WriteLine(data);
        }
    }

}
