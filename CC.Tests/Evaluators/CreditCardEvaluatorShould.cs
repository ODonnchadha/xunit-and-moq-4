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
            // validator.VerifyNoOtherCalls();
        }

        [Fact()]
        public void ReferWhenFrequentFlyerValidationError()
        {
            validator.Setup(v => v.IsValid(
                It.IsAny<string>())).Throws<Exception>();
            validator.Setup(v => v.LicenseKey).Returns(GetLicenseKey);

            var sut = new CreditCardEvaluator(validator.Object);

            Assert.Equal(CreditCardStatus.ReferredToHuman, sut.Evaluate(
                new CreditCard
                    {
                        Age = 42
                    })
                );
        }

        [Fact()]
        public void ReferWhenFrequentFlyerValidationCustomExceptionError()
        {
            validator.Setup(v => v.IsValid(
                It.IsAny<string>())).Throws(
                    new Exception("IsValid Exception"));
            validator.Setup(v => v.LicenseKey).Returns(GetLicenseKey);

            var sut = new CreditCardEvaluator(validator.Object);

            Assert.Equal(CreditCardStatus.ReferredToHuman, sut.Evaluate(
                new CreditCard
                {
                    Age = 42
                })
            );
        }

        [Fact()]
        public void IncrementCountViaManualEvent()
        {
            validator.Setup(v => v.IsValid(
                It.IsAny<string>())).Returns(true);
            validator.Setup(v => v.LicenseKey).Returns(GetLicenseKey);

            var sut = new CreditCardEvaluator(validator.Object);

            sut.Evaluate(new CreditCard
            {
                FrequentFlyerNumber = "x",
                Age = 25
            });

            validator.Raise(v => v.CountPerformed += null, EventArgs.Empty);

            Assert.Equal(1, sut.Count);
        }

        [Fact()]
        public void IncrementCountViaAutomaticEvent()
        {
            validator.Setup(v => v.IsValid(
                It.IsAny<string>()))
                .Returns(true)
                .Raises(v => v.CountPerformed += null, EventArgs.Empty);

            validator.Setup(v => v.LicenseKey).Returns(GetLicenseKey);

            var sut = new CreditCardEvaluator(validator.Object);

            sut.Evaluate(new CreditCard
            {
                FrequentFlyerNumber = "x",
                Age = 25
            });

            Assert.Equal(1, sut.Count);
        }

        [Fact()]
        public void ReferInvalidFrequentFlyerApplications_ReturnValuesSequence()
        {
            validator.SetupSequence(v => v.IsValid(
                It.IsAny<string>()))
                .Returns(false).Returns(true);

            validator.Setup(v => v.LicenseKey).Returns(GetLicenseKey);

            var sut = new CreditCardEvaluator(validator.Object);

            var application = new CreditCard
            {
                FrequentFlyerNumber = "x",
                Age = 25
            };

            var decision1 = sut.Evaluate(application);
            Assert.Equal(CreditCardStatus.ReferredToHuman, decision1);

            var decision2 = sut.Evaluate(application);
            Assert.Equal(CreditCardStatus.AutoDeclined, decision2);
        }

        [Fact()]
        public void ReferInvalidFrequentFlyerApplications_MultipleCallsSequence()
        {
            var numbers = new List<string>();

            validator.Setup(v => v.IsValid(Capture.In(numbers)));
            validator.Setup(v => v.LicenseKey).Returns(GetLicenseKey);

            var sut = new CreditCardEvaluator(validator.Object);

            var application1 = new CreditCard
            {
                FrequentFlyerNumber = "xx",
                Age = 25
            };
            var application2 = new CreditCard
            {
                FrequentFlyerNumber = "yy",
                Age = 25
            };
            var application3 = new CreditCard
            {
                FrequentFlyerNumber = "zz",
                Age = 25
            };

            sut.Evaluate(application1);
            sut.Evaluate(application2);
            sut.Evaluate(application3);

            Assert.Equal(
                new List<string>
                    {
                        "xx", "yy", "zz"
                    },
                numbers);
        }

        [Fact()]
        public void LinqToMocks()
        {
            var validator = Mock.Of<IFrequentFlyerNumberValidator>
                (
                    v => 
                    v.LicenseKey == GetLicenseKey() &&
                    v.IsValid(It.IsAny<string>()) == true
                );

            var sut = new CreditCardEvaluator(validator);

            Assert.Equal(CreditCardStatus.AutoDeclined, sut.Evaluate(
                new CreditCard
                {
                    Age = 25
                })
            );
        }
    }
}
