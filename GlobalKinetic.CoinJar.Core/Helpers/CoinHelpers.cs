using GlobalKinetic.CoinJar.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GlobalKinetic.CoinJar.Core.Helpers
{
    public static class CoinHelpers
    {
        public static double GetCoinVolumeInGrams(CoinType coinType)
        {
            switch (coinType)
            {
                case CoinType.Penny:
                    return 2.500;
                case CoinType.Nickel:
                    return 5.000;
                case CoinType.Dime:
                    return 2.268;
                case CoinType.Quater:
                    return 5.670;
                case CoinType.Half_Dolar:
                    return 11.34;
                case CoinType.Dolar:
                    return 8.10;
                default:
                    return 0.0;
            }
        }

        public static double GetCoinAmountInDollar(CoinType coinType)
        {
            switch (coinType)
            {
                case CoinType.Penny:
                    return 0.01;
                case CoinType.Nickel:
                    return 0.05;
                case CoinType.Dime:
                    return 0.10;
                case CoinType.Quater:
                    return 0.25;
                case CoinType.Half_Dolar:
                    return 0.50;
                case CoinType.Dolar:
                    return 1;
                default:
                    return 0.0;
            }
        }
    }
}
