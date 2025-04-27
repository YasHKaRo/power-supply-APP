using power_supply_APP.Api.Modules;
public class TestParameters
{
    public string TestName { get; set; }
    public float Temperature { get; set; }
    public float Voltage { get; set; }
    public int DurationSeconds { get; set; }
    public float CurrentLimit { get; set; }

    public TestParameters()
    {
        TestName = "Тест без названия";
    }
}
