namespace Application.Interfaces.Validators
{
    public interface IFrequentFlyerNumberValidator
    {
        event EventHandler CountPerformed;
        bool IsValid(string number);
        bool IsValid(string number, out bool isValid);
        string LicenseKey { get; }
    }
}
