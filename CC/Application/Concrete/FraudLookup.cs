using Application.Models;

namespace Application.Concrete
{
    public class FraudLookup
    {
        public virtual bool IsRiskVirtual(CreditCard card)
        {
            if (card.LastName == "Smith")
            {
                return true;
            }

            return false;
        }

        public bool IsRiskProtected(CreditCard card) => Check(card);
        protected virtual bool Check(CreditCard card)
        {
            if (card.LastName == "Smith")
            {
                return true;
            }

            return false;
        }

    }
}
