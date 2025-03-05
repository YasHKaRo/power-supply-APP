using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Xml;

namespace power_supply_APP.Model
{
    public class ConfigManager
    {
        private static readonly string ConfigFilePath = Path.GetFullPath(Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, @"..\..\Files\Other\conf.json"));
        public Dictionary<string, string> Users { get; private set; }

        public ConfigManager()
        {
            LoadConfig();
        }

        private void LoadConfig()
        {
            if (File.Exists(ConfigFilePath))
            {
                string json = File.ReadAllText(ConfigFilePath);
                Console.WriteLine($"Загруженный JSON: {json}");

                try
                {
                    Users = JsonConvert.DeserializeObject<Dictionary<string, string>>(json)
                            ?? new Dictionary<string, string>();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка десериализации: {ex.Message}");
                    Users = new Dictionary<string, string>();
                }
            }
            else
            {
                Console.WriteLine("Файл конфигурации не найден!");
                Console.WriteLine($"Текущая директория: {Directory.GetCurrentDirectory()}");
                Console.WriteLine($"Полный путь к файлу: {ConfigFilePath}");
                Console.WriteLine($"Файл существует: {File.Exists(ConfigFilePath)}");
                Users = new Dictionary<string, string>();
            }
        }

        public void SaveConfig()
        {
            string json = JsonConvert.SerializeObject(Users, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(ConfigFilePath, json);
        }

        public bool ChangePassword(string login, string newPassword)
        {
            Console.WriteLine($"Пытаемся изменить пароль для пользователя: {login}");

            if (Users.ContainsKey(login))
            {
                Console.WriteLine($"Пользователь {login} найден. Смена пароля...");
                Users[login] = newPassword;
                SaveConfig();
                return true;
            }

            Console.WriteLine($"Пользователь {login} не найден!");

            return false;
        }
    }
}
