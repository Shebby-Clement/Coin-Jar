using System;
using System.Collections.Generic;
using System.Text;

namespace GlobalKinetic.CoinJar.Data.Interfaces
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}
