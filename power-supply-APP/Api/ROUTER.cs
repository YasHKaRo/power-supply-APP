using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace power_supply_APP
{
    //абстрактный класс для всех тестирований
    abstract class BPTestProcess
    {
        // абстрактный метод для получения информации
        public abstract double GetInfo();
        public abstract void StartTest();
    }

    //наследуем тестирование 
    internal class TestHeat : BPTestProcess
    {

        //поле для параметра
        public float parametr { get; set; }
        //переопределение метода коллбэк
        public override double GetInfo()
        {
            throw new NotImplementedException();//todo
        }
        //переопределение метода для теста
        public override void StartTest()
        {

            Console.WriteLine("начинаем тестирование 1");
        }
        //перепишем метод ту стринг на будущее
        public override string ToString()
        {
            return $"";
        }
    }
    internal class TestEnergyCyclic : BPTestProcess
    {
        //поле для параметра
        public float parametr { get; set; }
        //переопределение метода коллбэк
        public override double GetInfo()
        {
            throw new NotImplementedException();//todo
        }
        //переопределение метода для теста
        public override void StartTest()
        {

            Console.WriteLine("начинаем тестирование 2");
        }
        //перепишем метод ту стринг на будущее
        public override string ToString()
        {
            return $"";
        }
    }
    internal class TestIxx : BPTestProcess
    {
        //поле для параметра
        public float parametr { get; set; }
        //переопределение метода коллбэк
        public override double GetInfo()
        {
            throw new NotImplementedException();//todo
        }
        //переопределение метода для теста
        public override void StartTest()
        {

            Console.WriteLine("начинаем тестирование 2");
        }
        //перепишем метод ту стринг на будущее
        public override string ToString()
        {
            return $"";
        }
    }
}
