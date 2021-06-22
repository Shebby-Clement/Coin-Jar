using GlobalKinetic.CoinJar.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GlobalKinetic.CoinJar.Services.Interfaces
{
    public interface ICoinService
    {
        #region GENERAL CRUD
        Coin GetCoinById(Guid coinId);
        void UpdateCoin(Coin coin);
        void CreateCoin(Coin coin);
        void DeleteCoin(Guid coinId);

        #endregion

        IEnumerable<Coin> GetAll();
        IEnumerable<Coin> SearchCoin(double query);
        decimal GetTotalAmount();
        void Reset();
        void CoinCommit();
    }
}
