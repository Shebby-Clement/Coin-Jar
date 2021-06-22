using System;
using System.Collections.Generic;
using System.Text;

namespace GlobalKinetic.CoinJar.Core.Models
{
    public interface ICoin
    {
        public double Amount { get; set; }
        public double Volume { get; set; }
    }
}
