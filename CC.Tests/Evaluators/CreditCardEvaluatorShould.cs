using Application.Evaluators;
using Application.Interfaces.Validators;
using Application.Models;
using Moq;

namespace CC.Tests.Evaluators
{
    public class CreditCardEvaluatorShould
    {
        private Mock<IFrequentFlyerNumberValidator> validator;
        public CreditCardEvaluatorShould() => 
            this.validator = new Mock<IFrequentFlyerNumberValidator>(
            MockBehavior.Strict);

        [Fact()]
        public void AcceptHighGrossAnnualIncome()
        {
            validator.Setup(v => v.IsValid(
                It.IsAny<string>())).Returns(true);

            var sut = new CreditCardEvaluator(validator.Object);

            Assert.Equal(CreditCardStatus.AutoAccepted, sut.Evaluate(
                new CreditCard
                {
                    GrossAnnualIncome = 100_000
                }));
        }

        [Fact()]
        public void ReferYoungCreditCardApplication()
        {
            validator.Setup(v => v.IsValid(
                It.IsAny<string>())).Returns(true);
            validator.Setup(v => v.LicenseKey).Returns(GetLicenseKey);

            var sut = new CreditCardEvaluator(validator.Object);

            Assert.Equal(CreditCardStatus.ReferredToHuman, sut.Evaluate(
                new CreditCard
                {
                    Age = 19
                }));
        }

        [Fact()]
        public void DeclineLowIncomeAPplications()
        {
            validator.Setup(v => v.IsValid(
                It.IsAny<string>())).Returns(true);
            validator.Setup(v => v.LicenseKey).Returns(GetLicenseKey);

            var sut = new CreditCardEvaluator(validator.Object);

            var status = sut.Evaluate(
                new CreditCard
                {
                    GrossAnnualIncome = 19_999,
                    Age= 42,
                    FrequentFlyerNumber = "x"
                });

            Assert.Equal(CreditCardStatus.AutoDeclined, status);
        }

        [Fact()]
        public void ReferInvalidFrequestFlyerApplications()
        {
            validator.Setup(v => v.IsValid(
                It.IsAny<string>())).Returns(false);
            validator.Setup(v => v.LicenseKey).Returns(GetLicenseKey);

            var sut = new CreditCardEvaluator(validator.Object);

            Assert.Equal(CreditCardStatus.ReferredToHuman, 
                sut.Evaluate(
                    new CreditCard
                    {
                    }));
        }

        [Fact()]
        public void ReferWhenLicenseKeyExpiredLiteral()
        {
            validator.Setup(v => v.IsValid(
                It.IsAny<string>())).Returns(true);
            validator.Setup(v => v.LicenseKey).Returns("EXPIRED");

            var sut = new CreditCardEvaluator(validator.Object);

            Assert.Equal(CreditCardStatus.ReferredToHuman,
                sut.Evaluate(
                    new CreditCard
                    {
                        GrossAnnualIncome = 19_999
                    }));
        }

        [Fact()]
        public void ReferWhenLicenseKeyExpiredFunction()
        {
            validator.Setup(v => v.IsValid(
                It.IsAny<string>())).Returns(true);
            // Note: Execution deferred.
            validator.Setup(v => v.LicenseKey).Returns(GetLicenseKey);

            var sut = new CreditCardEvaluator(validator.Object);

            Assert.Equal(CreditCardStatus.AutoDeclined,
                sut.Evaluate(
                    new CreditCard
                    {
                        GrossAnnualIncome = 19_999,
                        Age = 42,
                        FrequentFlyerNumber = "x"
                    }));
        }

        private string GetLicenseKey() => "VALID";

        [Fact()]
        public void ValidateFrequentFlyerNumberForLowIncomepApplications()
        {
            string SPECIFIC_VALUE = "x";

            validator.Setup(v => v.IsValid(
                It.IsAny<string>())).Returns(true);
            validator.Setup(v => v.LicenseKey).Returns(GetLicenseKey);

            var sut = new CreditCardEvaluator(validator.Object);

            sut.Evaluate(new CreditCard 
            { 
                FrequentFlyerNumber = SPECIFIC_VALUE 
            });

            validator.Verify(v => v.IsValid(SPECIFIC_VALUE), 
                $"Invalid FrequentFlyerNumber: {SPECIFIC_VALUE}");
        }

        [Fact()]
        public void NotValidateFrequentFlyerNumberForHighIncomepApplications()
        {
            var sut = new CreditCardEvaluator(validator.Object);

            sut.Evaluate(new CreditCard
            {
                GrossAnnualIncome = 100_000
            });

            validator.Verify(v => v.IsValid(It.IsAny<string>()), Times.Never);
        }

        [Fact()]
        public void CheckLicenseKeyForLowIncomepApplications()
        {
            validator.Setup(v => v.IsValid(
                It.IsAny<string>())).Returns(true);
            validator.Setup(v => v.LicenseKey).Returns(GetLicenseKey);

            var sut = new CreditCardEvaluator(validator.Object);

            sut.Evaluate(new CreditCard
            {
                GrossAnnualIncome = 100_001
            });

            validator.VerifyGet(v => v.LicenseKey, Times.Never);
            validator.VerifyNoOtherCalls();
        }
    }
}
