using System;
using System.Threading;
using power_supply_APP.Api.Modules;

namespace power_supply_APP.Api.Modules
{
    public class TestHeat : ITest
    {
        private readonly TestParameters _parameters;
        private readonly ModbusService _modbusService;
        private readonly SerialPortService _serialPortService;
        private readonly TestResult _testResult;

        public TestHeat(TestParameters parameters)
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
                // Настройка Modbus
                _modbusService.WriteSingleRegister(1, 100, (int)_parameters.Temperature);

                // Открываем порт и отправляем стартовый сигнал
                _serialPortService.Open();
                _serialPortService.WriteData("START_HEAT_TEST");

                // Эмулируем сбор данных по времени (например, каждые 5 секунд)
                int intervals = _parameters.DurationSeconds / 5;
                for (int i = 0; i < intervals; i++)
                {
                    Thread.Sleep(5000); // Ждём 5 секунд

                    // Имитируем получение данных
                    float temperature = 75.0f + i * 0.5f; // Заглушка для температуры
                    float voltage = 12.0f + (i % 3) * 0.1f; // Заглушка для напряжения

                    _testResult.TemperaturePoints.Add(temperature);
                    _testResult.VoltagePoints.Add(voltage);
                    _testResult.TimePoints.Add(DateTime.Now);
                }

                // Завершаем тест
                _serialPortService.WriteData("STOP_HEAT_TEST");

                _testResult.EndTime = DateTime.Now;
                _testResult.Success = true;

                // Сохраняем результаты
                _testResult.SaveAsTxt($"TestReports\\{_parameters.TestName}_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
                _testResult.SaveAsDocx($"TestReports\\{_parameters.TestName}_{DateTime.Now:yyyyMMdd_HHmmss}.docx");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка выполнения TestHeat: {ex.Message}");
                _testResult.Success = false;
                _testResult.EndTime = DateTime.Now;

                // Сохраняем даже неудачный результат
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
