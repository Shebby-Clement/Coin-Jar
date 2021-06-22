using GlobalKinetic.CoinJar.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GlobalKinetic.CoinJar.Data.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbFactory dbFactory;
        private readonly CoinJarDbContext dbContext;

        public UnitOfWork(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public CoinJarDbContext DbContext
        {
            get { return dbContext ?? dbFactory.Init(); }
        }

        public void Commit()
        {
            DbContext.Commit();
        }
    }
}
