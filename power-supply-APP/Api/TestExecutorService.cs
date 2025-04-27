using System;
using power_supply_APP.Api.Modules;

namespace power_supply_APP.Api
{
    public interface ITest
    {
        bool Run();
    }
    public class TestExecutorService
    {
        public bool ExecuteTest(string testName, TestParameters parameters)
        {
            try
            {
                ITest test = testName switch
                {
                    "HeatTest" => new TestHeat(parameters),
                    //"VoltageTest" => new TestVoltage(parameters),
                    //"CurrentTest" => new TestCurrent(parameters),
                    _ => throw new ArgumentException($"Неизвестный тест: {testName}")
                };

                return test.Run();
            }
            catch (Exception ex)
            {
                LogError("Ошибка при выполнении теста", ex);
                return false;
            }
        }

        private void LogError(string message, Exception ex)
        {
            string logPath = "log.txt";
            string fullMessage = $"{DateTime.Now}: {message} - {ex.Message}\n{ex.StackTrace}\n";
            System.IO.File.AppendAllText(logPath, fullMessage);
        }
    }
}
