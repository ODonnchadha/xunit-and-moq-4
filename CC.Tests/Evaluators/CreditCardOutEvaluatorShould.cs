using Application.Evaluators;
using Application.Interfaces.Validators;
using Application.Models;
using Moq;

namespace CC.Tests.Evaluators
{
    public class CreditCardOutEvaluatorShould
    {
        private Mock<IFrequentFlyerNumberValidator> validator;
        public CreditCardOutEvaluatorShould() =>
            this.validator = new Mock<IFrequentFlyerNumberValidator>();

        [Fact()]
        public void AcceptHighGrossAnnualIncome()
        {
            var isValid = true;

            validator.Setup(v => v.IsValid(
                It.IsAny<string>(), out isValid));

            var sut = new CreditCardOutEvaluator(validator.Object);

            Assert.Equal(CreditCardStatus.AutoAccepted, sut.Evaluate(
                new CreditCard
                {
                    GrossAnnualIncome = 100_000
                }));
        }

        [Fact()]
        public void ReferYoungCreditCardApplication()
        {
            var isValid = true;

            validator.Setup(v => v.IsValid(
                It.IsAny<string>(), out isValid));

            var sut = new CreditCardOutEvaluator(validator.Object);

            Assert.Equal(CreditCardStatus.ReferredToHuman, sut.Evaluate(
                new CreditCard
                {
                    Age = 19
                }));
        }

        [Fact()]
        public void DeclineLowIncomeAPplications()
        {
            var isValid = true;

            validator.Setup(v => v.IsValid(
                It.IsAny<string>(), out isValid));

            var sut = new CreditCardOutEvaluator(validator.Object);

            var status = sut.Evaluate(
                new CreditCard
                {
                    GrossAnnualIncome = 19_999,
                    Age = 42,
                    FrequentFlyerNumber = "x"
                });

            Assert.Equal(CreditCardStatus.AutoDeclined, status);
        }

        [Fact()]
        public void ReferInvalidFrequestFlyerApplications()
        {
            var isValid = false;

            validator.Setup(v => v.IsValid(
                It.IsAny<string>(), out isValid));

            var sut = new CreditCardOutEvaluator(validator.Object);

            Assert.Equal(CreditCardStatus.ReferredToHuman,
                sut.Evaluate(
                    new CreditCard
                    {
                    }));
        }
    }
}
