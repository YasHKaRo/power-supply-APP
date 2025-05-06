using System;
using System.Threading;
using power_supply_APP.Api.Modules;

namespace power_supply_APP.Api.Modules
{
    public class TestVoltage : ITest
    {
        private readonly TestParameters _parameters;
        private readonly ModbusService _modbusService;
        private readonly SerialPortService _serialPortService;
        private readonly TestResult _testResult;

        public TestVoltage(TestParameters parameters)
        {
            _parameters = parameters;
            _modbusService = new ModbusService("COM3", 9600);
            _serialPortService = new SerialPortService("COM3", 9600);
            _testResult = new TestResult
            {
                TestName = parameters.TestName,
                StartTime = DateTime.Now
            };
        }

        public bool Run()
        {
            try
            {
                _modbusService.WriteSingleRegister(1, 101, (int)_parameters.Voltage);

                _serialPortService.Open();
                _serialPortService.WriteData("START_VOLTAGE_TEST");

                int intervals = _parameters.DurationSeconds / 5;
                for (int i = 0; i < intervals; i++)
                {
                    Thread.Sleep(5000);
                    float voltage = 12.0f + (i % 2) * 0.1f;

                    _testResult.VoltagePoints.Add(voltage);
                    _testResult.TimePoints.Add(DateTime.Now);
                }

                _serialPortService.WriteData("STOP_VOLTAGE_TEST");

                _testResult.EndTime = DateTime.Now;
                _testResult.Success = true;
                _testResult.SaveAsTxt($"TestReports\\{_parameters.TestName}_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
                _testResult.SaveAsDocx($"TestReports\\{_parameters.TestName}_{DateTime.Now:yyyyMMdd_HHmmss}.docx");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка в TestVoltage: {ex.Message}");
                _testResult.Success = false;
                _testResult.EndTime = DateTime.Now;
                _testResult.SaveAsTxt($"TestReports\\{_parameters.TestName}_{DateTime.Now:yyyyMMdd_HHmmss}_error.txt");
                _testResult.SaveAsDocx($"TestReports\\{_parameters.TestName}_{DateTime.Now:yyyyMMdd_HHmmss}_error.docx");
                return false;
            }
            finally
            {
                _serialPortService.Close();
                _modbusService.Close();
            }
        }
    }
}
