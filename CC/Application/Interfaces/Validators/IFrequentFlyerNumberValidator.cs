namespace Application.Interfaces.Validators
{
    public interface IFrequentFlyerNumberValidator
    {
        bool IsValid(string number);
        bool IsValid(string number, out bool isValid);
        string LicenseKey { get; }
    }
}
