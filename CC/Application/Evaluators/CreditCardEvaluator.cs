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
        public int Count { get; private set; }
        public CreditCardEvaluator(IFrequentFlyerNumberValidator validator)
        {
            this.validator = validator;
            this.validator.CountPerformed += CountPerformed;
        }

        private void CountPerformed(object? sender, EventArgs e) => Count++;

        public CreditCardStatus Evaluate(CreditCard card)
        {
            if (card.GrossAnnualIncome >= HighIncomeThreshold)
            {
                return CreditCardStatus.AutoAccepted;
            }

            if (validator.LicenseKey.Equals("EXPIRED"))
            {
                return CreditCardStatus.ReferredToHuman;
            }

            bool isValid;

            try
            {
                isValid = validator.IsValid(card.FrequentFlyerNumber);
            }
            catch
            {
                return CreditCardStatus.ReferredToHuman;
            }

            if (!isValid)
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