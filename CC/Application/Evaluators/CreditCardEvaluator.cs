using Application.Interfaces.Validators;
using Application.Models;

namespace Application.Evaluators
{
    public class CreditCardEvaluator
    {
        private readonly IFrequentFlyerNumberValidator validator;

        private const int AutoReferralMaxAge = 20;
        private const int HighIncomeThreshold = 100_000;
        private const int LowIncomeThreshold = 20_000;

        public CreditCardEvaluator(IFrequentFlyerNumberValidator validator) =>
            this.validator = validator;

        public CreditCardStatus Evaluate(CreditCard card)
        {
            if (card.GrossAnnualIncome >= HighIncomeThreshold)
            {
                return CreditCardStatus.AutoAccepted;
            }

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