using NModbus.Device;
using System;

namespace power_supply_APP.Api.Modules
{
    public class TestHeat : ITest
    {
        private readonly TestParameters _parameters;
        private readonly ModbusService _modbusService;
        private readonly SerialPortService _serialPortService;

        public TestHeat(TestParameters parameters)
        {
            _parameters = parameters;
            _modbusService = new ModbusService("COM3", 9600);
            _serialPortService = new SerialPortService("COM3", 9600);
        }

        public bool Run()
        {
            try
            {
                // Пример логики теста нагрева

                // 1. Настроить регистры через Modbus
                _modbusService.WriteSingleRegister(1, 100, (int)_parameters.Temperature);

                // 2. Послать команду старт через SerialPort
                _serialPortService.Open();
                _serialPortService.WriteData("START_HEAT_TEST");

                // 3. Допустим: ждать завершения теста (эмуляция ожидания)
                System.Threading.Thread.Sleep(_parameters.DurationSeconds * 1000);

                // 4. Остановить тест
                _serialPortService.WriteData("STOP_HEAT_TEST");

                // Всё прошло успешно
                return true;
            }
            catch (Exception ex)
            {
                // Логировать ошибку можно прямо здесь или выбросить наверх
                Console.WriteLine($"Ошибка выполнения TestHeat: {ex.Message}");
                return false;
            }
            finally
            {
                _serialPortService.Close();
                _modbusService.close();
            }
        }
    }
}
