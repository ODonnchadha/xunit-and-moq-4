using Application.Interfaces.ChainedValidators;
using Application.Interfaces.Validators;
using Application.Models;

namespace Application.Validators
{
    public class ChainedFrequentFlyerNumberValidator : IChainedFrequentFlyerNumberValidator
    {
        public IChainedServiceInformation ServiceInformation => 
            throw new NotImplementedException();

        public ValidationMode ValidationMode 
        { 
            get => throw new NotImplementedException(); 
            set => throw new NotImplementedException(); 
        }

        public bool IsValid(string number) =>
            throw new NotImplementedException();
    }
}
