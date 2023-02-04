﻿using Application.Evaluators;
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

            var sut = new CreditCardEvaluator(validator.Object);

            Assert.Equal(CreditCardStatus.ReferredToHuman, 
                sut.Evaluate(
                    new CreditCard
                    {
                    }));
        }
    }
}
