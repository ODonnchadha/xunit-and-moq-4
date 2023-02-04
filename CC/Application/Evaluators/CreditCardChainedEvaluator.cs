using Application.Interfaces.Validators;
using Application.Models;

namespace Application.Evaluators
{
    public class CreditCardChainedEvaluator
    {
        private readonly IChainedFrequentFlyerNumberValidator validator;

        private const int AutoReferralMaxAge = 20;
        private const int HighIncomeThreshold = 100_000;
        private const int LowIncomeThreshold = 20_000;

        public CreditCardChainedEvaluator(
            IChainedFrequentFlyerNumberValidator validator) =>
            this.validator = validator;

        public CreditCardStatus Evaluate(CreditCard card)
        {
            if (card.GrossAnnualIncome >= HighIncomeThreshold)
            {
                return CreditCardStatus.AutoAccepted;
            }

            if (validator.ServiceInformation.License.Equals("EXPIRED"))
            {
                return CreditCardStatus.ReferredToHuman;
            }

            validator.ValidationMode = 
                card.Age >= 30 ? 
                ValidationMode.Detailed : 
                ValidationMode.Quick;

            if (!validator.IsValid(card.FrequentFlyerNumber))
            {
                return CreditCardStatus.ReferredToHuman;
            }

            if (card.Age <= AutoReferralMaxAge)
            {
                return CreditCardStatus.ReferredToHuman;
            }

            if (card.GrossAnnualIncome < LowIncomeThreshold)
            {
                return CreditCardStatus.AutoDeclined;
            }

            return CreditCardStatus.ReferredToHuman;
        }
    }
}