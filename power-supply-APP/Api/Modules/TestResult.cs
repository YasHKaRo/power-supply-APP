using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using Xceed.Words.NET; // Для DocX библиотеки

namespace power_supply_APP.Api.Modules
{
    public class TestResult
    {
        public List<float> TemperaturePoints { get; set; } = new List<float>();
        public List<float> VoltagePoints { get; set; } = new List<float>();
        public List<DateTime> TimePoints { get; set; } = new List<DateTime>();
        public string TestName { get; set; }
        public DateTime StartTime { get; set; } = DateTime.Now;
        public DateTime EndTime { get; set; }
        public bool Success { get; set; }

        public string GenerateReport()
        {
            var report = $"Отчёт по тесту: {TestName}\n";
            report += $"Начало: {StartTime}\n";
            report += $"Окончание: {EndTime}\n";
            report += $"Результат: {(Success ? "Успешно" : "Ошибка")}\n";
            report += "\nЗамеры:\n";

            for (int i = 0; i < TimePoints.Count; i++)
            {
                report += $"[{TimePoints[i]}] Температура: {TemperaturePoints[i]}°C, Напряжение: {VoltagePoints[i]}V\n";
            }

            return report;
        }

        public void SaveAsTxt(string path)
        {
            var report = GenerateReport();
            File.WriteAllText(path, report);
        }

        public void SaveAsDocx(string path)
        {
            var report = GenerateReport();
            using var doc = DocX.Create(path);
            doc.InsertParagraph(report);
            doc.Save();
        }
    }
}
