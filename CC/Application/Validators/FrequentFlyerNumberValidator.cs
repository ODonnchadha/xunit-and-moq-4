using Application.Interfaces.Validators;

namespace Application.Validators
{
    public class FrequentFlyerNumberValidator : IFrequentFlyerNumberValidator
    {
        public bool IsValid(string number)
        {
            throw new NotImplementedException();
        }

        public bool IsValid(string number, out bool isValid)
        {
            throw new NotImplementedException();
        }
    }
}
