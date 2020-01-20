using System;

namespace MoneyManager.Models.Domain
{
    public class Currency : IEquatable<Currency>
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string IsoCode { get; set; } = null!;
        public string Symbol { get; set; } = null!;


        public override bool Equals(object obj)
        {

            if ((obj == null) || !GetType().Equals(obj.GetType()))
            {
                return false;
            }
            return Equals((Currency)obj);
        }

        public bool Equals(Currency other)
        {
            return other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
