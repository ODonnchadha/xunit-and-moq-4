using Application.Evaluators;
using Application.Interfaces.ChainedValidators;
using Application.Interfaces.Validators;
using Application.Models;
using Moq;
using System.ComponentModel.DataAnnotations;

namespace CC.Tests.Evaluators
{
    public class CreditCardChainedEvaluatorShould
    {
        private Mock<IChainedFrequentFlyerNumberValidator> validator;
        public CreditCardChainedEvaluatorShould() =>
            this.validator = new Mock<IChainedFrequentFlyerNumberValidator>();

        [Fact()]
        public void AcceptHighGrossAnnualIncome()
        {
            validator.Setup(v => v.IsValid(
                It.IsAny<string>())).Returns(true);

            var sut = new CreditCardChainedEvaluator(validator.Object);

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
            validator.Setup(v => 
            v.ServiceInformation.License.LicenseKey).Returns(
                GetLicenseKey);

            var sut = new CreditCardChainedEvaluator(validator.Object);

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
            validator.Setup(v =>
            v.ServiceInformation.License.LicenseKey).Returns(
                GetLicenseKey);

            var sut = new CreditCardChainedEvaluator(validator.Object);

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
            validator.Setup(v => v.IsValid(
                It.IsAny<string>())).Returns(false);
            validator.Setup(v =>
            v.ServiceInformation.License.LicenseKey).Returns(
                GetLicenseKey);

            var sut = new CreditCardChainedEvaluator(validator.Object);

            Assert.Equal(CreditCardStatus.ReferredToHuman,
                sut.Evaluate(
                    new CreditCard
                    {
                    }));
        }

        [Fact()]
        public void UseDetailedLookupForOlderApplications()
        {
            validator.Setup(v => v.IsValid(
                It.IsAny<string>())).Returns(true);
            validator.Setup(v =>
            v.ServiceInformation.License.LicenseKey).Returns(
                GetLicenseKey);
            validator.SetupProperty(v => v.ValidationMode);

            var sut = new CreditCardChainedEvaluator(validator.Object);

            sut.Evaluate(new CreditCard{ Age = 30 });

            Assert.Equal(
                ValidationMode.Detailed, validator.Object.ValidationMode);
        }

        [Fact()]
        public void ReferWhenLicenseKeyExpiredChain()
        {
            // Create a hierarchy.
            var licenseData = new Mock<IChainedLicenseData>();
            licenseData.Setup(l => l.LicenseKey).Returns("EXPIRED");

            validator.Setup(v => v.IsValid(
                It.IsAny<string>())).Returns(true);
            validator.Setup(v =>
            v.ServiceInformation.License).Returns(licenseData.Object);

            var sut = new CreditCardChainedEvaluator(validator.Object);

            Assert.Equal(CreditCardStatus.ReferredToHuman,
                sut.Evaluate(
                    new CreditCard
                    {
                        GrossAnnualIncome = 19_999,
                    }));
        }

        [Fact()]
        public void ReferWhenLicenseKeyExpiredLiteral()
        {
            validator.Setup(v => v.IsValid(
                It.IsAny<string>())).Returns(true);
            validator.Setup(v =>
            v.ServiceInformation.License.LicenseKey).Returns(
                "EXPIRED");

            var sut = new CreditCardChainedEvaluator(validator.Object);

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
            validator.Setup(v =>
            v.ServiceInformation.License.LicenseKey).Returns(
                GetLicenseKey);

            var sut = new CreditCardChainedEvaluator(validator.Object);

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
    }
}
