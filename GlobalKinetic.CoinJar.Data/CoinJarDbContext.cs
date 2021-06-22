using GlobalKinetic.CoinJar.Core.Models;
using GlobalKinetic.CoinJar.Data.ModelConfig;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GlobalKinetic.CoinJar.Data
{
    public class CoinJarDbContext : DbContext
    {
        #region Constructors

        public CoinJarDbContext(DbContextOptions<CoinJarDbContext> options) : base(options)
        {

        }

        #endregion

        #region Properties

        public DbSet<Coin> Coin { get; set; }


        #endregion


        #region Methods

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new CoinConfiguration());

            base.OnModelCreating(builder);
        }

        public virtual void Commit()
        {
            base.SaveChanges();
        }

        #endregion
    }

}
