using Application.Interfaces.ChainedValidators;
using Application.Models;

namespace Application.Interfaces.Validators
{
    public interface IChainedFrequentFlyerNumberValidator
    {
        bool IsValid(string number);
        IChainedServiceInformation ServiceInformation { get; }
        ValidationMode ValidationMode { get; set; }
    }
}
