using Application.Concrete;
using Application.Interfaces.Validators;
using Application.Models;

namespace Application.Evaluators
{
    public class CreditCardFraudEvaluator
    {
        private readonly IFrequentFlyerNumberValidator validator;
        private readonly FraudLookup fraud;

        private const int AutoReferralMaxAge = 20;
        private const int HighIncomeThreshold = 100_000;
        private const int LowIncomeThreshold = 20_000;
        public int Count { get; private set; }
        public CreditCardFraudEvaluator(
            FraudLookup fraud,
            IFrequentFlyerNumberValidator validator)
        {
            this.fraud = fraud;

            this.validator = validator;
            this.validator.CountPerformed += CountPerformed;
        }

        private void CountPerformed(object? sender, EventArgs e) => Count++;

        public CreditCardStatus EvaluateVirtual(CreditCard card)
        {
            if (fraud != null && fraud.IsRiskVirtual(card))
            {
                return CreditCardStatus.ReferredToHumanFraudRisk;
            }

            return CreditCardStatus.ReferredToHuman;
        }

        public CreditCardStatus EvaluateProtected(CreditCard card)
        {
            if (fraud != null && fraud.IsRiskProtected(card))
            {
                return CreditCardStatus.ReferredToHumanFraudRisk;
            }

            return CreditCardStatus.ReferredToHuman;
        }
    }
}