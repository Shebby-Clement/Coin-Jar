using GlobalKinetic.CoinJar.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GlobalKinetic.CoinJar.Core.Dtos
{
    public class CoinDto : BaseEntity<Guid>, ICoin
    {
        #region Constructors

        public CoinDto()
        {
            CoinID = Guid.NewGuid();
        }

        #endregion


        #region Properties
        public Guid CoinID { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public double Volume { get; set; }

        #endregion
    }
}
