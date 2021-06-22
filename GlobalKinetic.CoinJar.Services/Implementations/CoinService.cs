using GlobalKinetic.CoinJar.Core.Models;
using GlobalKinetic.CoinJar.Data.Interfaces;
using GlobalKinetic.CoinJar.Data.Repositories.Interfaces;
using GlobalKinetic.CoinJar.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GlobalKinetic.CoinJar.Services.Implementations
{

    public class CoinService : ICoinService
    {
        #region Fields

        private readonly IUnitOfWork unitOfWork;
        private readonly ICoinRepository coinRepository;

        #endregion

        #region Constructors

        public CoinService(ICoinRepository coinRepository, IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.coinRepository = coinRepository;
        }

        #endregion

        #region Methods Implementation

        public void CreateCoin(Coin coin)
        {
            coinRepository.Add(coin);
        }

        public void DeleteCoin(Guid coinId)
        {
            coinRepository.Delete(coinId);
        }

        public IEnumerable<Coin> GetAll()
        {
            return coinRepository.GetAll();
        }

        public Coin GetCoinById(Guid coinId)
        {
            return coinRepository.GetById(coinId);
        }


        public IEnumerable<Coin> SearchCoin(double query)
        {
            var coins = coinRepository.GetMany(x => x.Volume == query || x.Amount == query).ToList();
            return coins?.OrderByDescending(t => t.CreatedAt);
        }


        public void UpdateCoin(Coin coin)
        {
            coinRepository.Update(coin);
        }

        public decimal GetTotalAmount()
        {
            return coinRepository.GetTotalAmount();
        }

        public void Reset()
        {
            coinRepository.Reset();
        }

        public void CoinCommit()
        {
            unitOfWork.Commit();
        }

        #endregion
    }
}
