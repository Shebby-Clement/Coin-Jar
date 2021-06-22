using GlobalKinetic.CoinJar.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GlobalKinetic.CoinJar.Data.Repositories.Interfaces
{

    public interface ICoinRepository : IRepository<Coin>
    {
        decimal GetTotalAmount();
        void Reset();
    }
}
