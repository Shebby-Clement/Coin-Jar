using GlobalKinetic.CoinJar.Core.Models;
using GlobalKinetic.CoinJar.Data.Interfaces;
using GlobalKinetic.CoinJar.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GlobalKinetic.CoinJar.Data.Repositories.Implementation
{
    public class CoinRepository : BaseRepository<Coin>, ICoinRepository
    {
        #region Constructors

        public CoinRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        #endregion

        #region Methods Implementation

        public decimal GetTotalAmount()
        {
            var coinsInJar = GetAll();

            return Convert.ToDecimal(coinsInJar.Sum(c => c.Amount));
        }

        public void Reset()
        {
            var coins = GetAll();

            foreach (var coin in coins)
            {
                Delete(coin.CoinID);
            }
        }

        #endregion
    }
}
