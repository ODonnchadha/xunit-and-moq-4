using Application.Concrete;
using Application.Evaluators;
using Application.Interfaces.Validators;
using Application.Models;
using Moq;
using Moq.Protected;

namespace CC.Tests.Evaluators
{
    public class CreditCardFraudEvaluatorShould
    {
        private Mock<FraudLookup> fraud;
        private Mock<IFrequentFlyerNumberValidator> validator;
        public CreditCardFraudEvaluatorShould()
        {
            this.fraud  = new Mock<FraudLookup>();
            this.validator = new Mock<IFrequentFlyerNumberValidator>(
                MockBehavior.Strict);
        }

        [Fact()]
        public void ReferFraudRiskProtected()
        {
            fraud.Protected().Setup<bool>(
                "Check", ItExpr.IsAny<CreditCard>()).Returns(true);

            validator.Setup(v => v.IsValid(
                It.IsAny<string>())).Returns(true);

            var sut = new CreditCardFraudEvaluator(
                fraud.Object, validator.Object);

            Assert.Equal(CreditCardStatus.ReferredToHumanFraudRisk,
                sut.EvaluateProtected(new CreditCard
                {
                    LastName = "Smith",
                    GrossAnnualIncome = 8_000_000
                }));
        }


        [Fact()]
        public void ReferFraudRiskVirtual()
        {
            fraud.Setup(f => f.IsRiskVirtual(
                It.IsAny<CreditCard>())).Returns(true);
            validator.Setup(v => v.IsValid(
                It.IsAny<string>())).Returns(true);

            var sut = new CreditCardFraudEvaluator(
                fraud.Object, validator.Object);

            Assert.Equal(CreditCardStatus.ReferredToHumanFraudRisk, 
                sut.EvaluateVirtual(new CreditCard
                {
                    LastName = "Smith",
                    GrossAnnualIncome = 8_000_000
                }));
        }
    }
}
