using System;
using System.Collections.Generic;
using System.Text;

namespace GlobalKinetic.CoinJar.Data.Interfaces
{
    public interface IDbFactory
    {
        CoinJarDbContext Init();
    }
}
