using GlobalKinetic.CoinJar.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GlobalKinetic.CoinJar.Data.ModelConfig
{
    public class CoinConfiguration : IEntityTypeConfiguration<Coin>
    {
        public void Configure(EntityTypeBuilder<Coin> entity)
        {
            entity.ToTable("Coin");
            entity.HasKey(e => e.CoinID);
            entity.Property(p => p.Amount).IsRequired();
            entity.Property(p => p.Volume).IsRequired();
            // etc.
        }
    }
}
