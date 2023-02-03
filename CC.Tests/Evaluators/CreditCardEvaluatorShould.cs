using Application.Evaluators;
using Application.Interfaces.Validators;
using Application.Models;
using Moq;

namespace CC.Tests.Evaluators
{
    public class CreditCardEvaluatorShould
    {
        CreditCardEvaluator sut;

        public CreditCardEvaluatorShould()
        {
            var validator = new Mock<IFrequentFlyerNumberValidator>();
            validator.Setup(v => v.IsValid(It.IsAny<string>())).Returns(true);

            this.sut = new CreditCardEvaluator(validator.Object);
        }

        [Fact()]
        public void AcceptHighGrossAnnualIncome()
        {
            CreditCardStatus status = sut.Evaluate(
                new CreditCard 
                { 
                    GrossAnnualIncome = 100_000
                });

            Assert.Equal(CreditCardStatus.AutoAccepted, status);
        }

        [Fact()]
        public void ReferYoungCreditCard()
        {
            CreditCardStatus status = sut.Evaluate(
                new CreditCard
                {
                    Age = 19
                });

            Assert.Equal(CreditCardStatus.ReferredToHuman, status);
        }
    }
}
