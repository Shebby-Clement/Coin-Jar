using System;
using System.Collections.Generic;
using System.Text;

namespace GlobalKinetic.CoinJar.Core.Models
{
    public class Coin : BaseEntity<Guid>, ICoin
    {
        #region Constructors

        public Coin()
        {
            CoinID = Guid.NewGuid();
        }

        #endregion

        #region Properties
        public Guid CoinID { get; set; }
        public double Amount { get; set; }
        public double Volume { get; set; }

        #endregion
    }
}
