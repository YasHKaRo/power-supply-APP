using power_supply_APP.Api.Modules;
using System;
using System.IO;

public class TestExecutorService
{
    private readonly string _portName;
    private readonly int _baudRate;

    public TestExecutorService(string portName = "COM3", int baudRate = 9600)
    {
        _portName = portName;
        _baudRate = baudRate;
    }

    public bool ExecuteTest(string sectionName, TestParameters parameters)
    {
        try
        {
            //var test = new (parameters);

           // using (var portService = new SerialPortService(_portName, _baudRate))
           // {
            //    portService.Open();
            //    test.StartTest();
            //    portService.WriteData($"Начат тест: {parameters.TestName ?? "Без названия"}");
           // }

            return true;
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
        File.AppendAllText(logPath, fullMessage);
    }
}
