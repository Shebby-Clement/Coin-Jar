using GlobalKinetic.CoinJar.Core.Helpers;
using GlobalKinetic.CoinJar.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GlobalKinetic.CoinJar.Data.Implementations
{
    public class DbFactory : Disposable, IDbFactory
    {
        CoinJarDbContext _dbContext;

        public DbFactory(CoinJarDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public CoinJarDbContext Init()
        {

            if (_dbContext == null)
            {
                throw new Exception($"Please initialize your DbContext");
            }
            return _dbContext;
        }

        protected override void DisposeCore()
        {
            if (_dbContext != null)
                _dbContext.Dispose();
        }
    }
}
