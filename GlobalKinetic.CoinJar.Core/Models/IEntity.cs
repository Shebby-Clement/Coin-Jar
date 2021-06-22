using System;
using System.Collections.Generic;
using System.Text;

namespace GlobalKinetic.CoinJar.Core.Models
{
    public interface IEntity<Key>
    {
        //Key ID { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime ModifiedAt { get; set; }
    }
}
