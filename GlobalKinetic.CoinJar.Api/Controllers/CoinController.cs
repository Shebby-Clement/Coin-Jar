using AutoMapper;
using GlobalKinetic.CoinJar.Core.Dtos;
using GlobalKinetic.CoinJar.Core.Helpers;
using GlobalKinetic.CoinJar.Core.Models;
using GlobalKinetic.CoinJar.Core.Models.Enums;
using GlobalKinetic.CoinJar.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http.Description;

namespace GlobalKinetic.CoinJar.Api.Controllers
{
    [Route("api/Coins")]
    [ApiController]
    public class CoinController : BaseController
    {
        #region Fields

        private readonly IMapper _mapper;
        private readonly ICoinService _coinService;
        private readonly double Max_Coin_Jar_Size = 1242.0882; // converted 42 fluid ounces to grams.

        #endregion

        #region Constructors

        public CoinController(IMapper mapper, ICoinService coinService)
        {
            _mapper = mapper;
            _coinService = coinService;
        }

        #endregion

        #region Methods

        [SwaggerOperation(Summary = "Get all coin jar records from the database.")]
        [Route("GetAllCoins")]
        [HttpGet]
        [ResponseType(typeof(List<CoinDto>))]
        public IActionResult GetAll()
        {
            try
            {
                var coins = _coinService.GetAll();

                if (coins != null)
                {
                    var usersDto = _mapper.Map<List<CoinDto>>(coins);

                    return StatusCode((int)HttpStatusCode.OK, usersDto);
                }
                return StatusCode((int)HttpStatusCode.NotFound, $"No Coins were found");
            }
            catch (Exception er)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, $"{er.Message}");
            }
        }

        [SwaggerOperation(Summary = "Adds new coin to the Coin Jar. You can optionally pass/remove CreatedAt, ModifiedAt and CoinID properties and will be initialised automatically.")]
        [Route("AddCoin")]
        [HttpPost]
        [ResponseType(typeof(CoinDto))]
        public async Task<IActionResult> AddCoin(CoinDto coinDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var isAlreadyExisting = _coinService.GetCoinById(coinDto.CoinID);

            if (isAlreadyExisting != null)
            {
                return BadRequest($"A record with Coin ID : {coinDto.CoinID} already exist. Please pass unique ID or Remove CoinID property to auto generate new value");
            }


            try
            {
                var results = await RunSave(() =>
                {
                    var coinsInJar = _coinService.GetAll();
                    var currentJarSize = coinsInJar.Sum(c => c.Volume);

                    if (currentJarSize < Max_Coin_Jar_Size)
                    {
                        var coin = _mapper.Map<Coin>(coinDto);

                        var newJarSize = currentJarSize + coin.Volume;

                        if (newJarSize < Max_Coin_Jar_Size)
                        {
                            _coinService.CreateCoin(coin);
                            _coinService.CoinCommit();

                            return Task.FromResult(new ResponseResult<CoinDto>(_mapper.Map<CoinDto>(coin), new ResponseStatus($"New coin with Amount of ${coin.Amount} and Volume of {coin.Volume}g has been added into the jar", (int)HttpStatusCode.Created)));
                        }

                        return Task.FromResult(new ResponseResult<CoinDto>(_mapper.Map<CoinDto>(coin), new ResponseStatus($"Please note that coin jar is full to accept any coin with Volume of {coin.Volume}g and above , try to insert small coin.", (int)HttpStatusCode.MethodNotAllowed)));
                    }

                    return Task.FromResult(new ResponseResult<CoinDto>(coinDto, new ResponseStatus("Please note that coin jar is full and can not accept any coins", (int)HttpStatusCode.MethodNotAllowed)));

                }, throwExpection: true);

                return Ok(results);
            }
            catch (Exception er)
            {
                er.ExceptionHelper();

                return StatusCode((int)HttpStatusCode.InternalServerError, new ResponseResult<CoinDto>(coinDto, new ResponseStatus($"{er.Message}", er.HResult)));
            }
        }

        [Route("GetTotalAmount")]
        [HttpGet]
        [ResponseType(typeof(decimal))]
        public async Task<IActionResult> GetTotalAmount()
        {

            try
            {
                var results = await RunSave(() =>
                {
                    var totalAmount = _coinService.GetTotalAmount();

                    return Task.FromResult(new ResponseResult<decimal>(totalAmount, new ResponseStatus($"Current Total Amount in the Jar is ${totalAmount}", (int)HttpStatusCode.OK)));

                }, throwExpection: true);

                return Ok(results);
            }
            catch (Exception er)
            {
                er.ExceptionHelper();

                return StatusCode((int)HttpStatusCode.InternalServerError, new ResponseResult<object>(null, new ResponseStatus($"{er.Message}", er.HResult)));
            }
        }

        [Route("Reset")]
        [HttpGet]
        public async Task<IActionResult> Reset()
        {

            try
            {
                var results = await RunSave(() =>
                {
                    _coinService.Reset();
                    _coinService.CoinCommit();

                    return Task.FromResult(new ResponseResult<object>(null, new ResponseStatus($"Coin Jar has been resert to $0.00", (int)HttpStatusCode.OK)));

                }, throwExpection: true);

                return Ok(results);
            }
            catch (Exception er)
            {
                er.ExceptionHelper();

                return StatusCode((int)HttpStatusCode.InternalServerError, new ResponseResult<object>(null, new ResponseStatus($"{er.Message}", er.HResult)));
            }
        }




        /// This is another approach in which the same solution can be implemented with less error since it is based on set of 
        /// pre-defined values of coins like ( PENNY = 1c, NICKEL = 5c, DIMME = 10c, QUATTER = 25c , HALF_DOLAR = 50c & DOLAR = $1) which can be pased as parameter when calling AddNewCoin method
        /// And their respective masses in grams ( PENNY = 2.500g, NICKEL = 5.000g, DIMME = 2.268g, QUATTER = 5.670g , HALF_DOLAR = 11.34g & DOLAR = 8.10g)

        [SwaggerOperation(Summary = "Another AddCoin Method to add Coins to the Jar. You just pass any coin type value ( 1 = PENNY, 5 = NICKEL, 10 = DIMME, 25 = QUATTER , 50 = HALF_DOLAR & 100 = DOLAR) as coinTypeDto parameter")]
        [Route("Additional_AddCoin_Method")]
        [HttpPost]
        [ResponseType(typeof(CoinDto))]
        public async Task<IActionResult> AddNewCoin(int coinTypeDto)
        {
            var currentCoins = Enum.GetName(typeof(CoinType), coinTypeDto);

            if (string.IsNullOrEmpty(currentCoins))
            {
                return BadRequest($"This coin :{coinTypeDto} is invalid.");
            }

            var coin = new Coin();

            try
            {
                var results = await RunSave(() =>
                {

                    coin.Amount = CoinHelpers.GetCoinAmountInDollar((CoinType)coinTypeDto);
                    coin.Volume = CoinHelpers.GetCoinVolumeInGrams((CoinType)coinTypeDto);

                    var coinsInJar = _coinService.GetAll();
                    var currentJarSize = coinsInJar.Sum(c => c.Volume);

                    if (currentJarSize < Max_Coin_Jar_Size)
                    {
                        var newJarSize = currentJarSize + coin.Volume;

                        if (newJarSize < Max_Coin_Jar_Size)
                        {
                            _coinService.CreateCoin(coin);
                            _coinService.CoinCommit();

                            return Task.FromResult(new ResponseResult<CoinDto>(_mapper.Map<CoinDto>(coin), new ResponseStatus($"New coin with Amount of ${coin.Amount} and Volume of {coin.Volume}g has been added into the jar", (int)HttpStatusCode.Created)));
                        }

                        return Task.FromResult(new ResponseResult<CoinDto>(_mapper.Map<CoinDto>(coin), new ResponseStatus($"Please note that coin jar is full to accept any coin with Volume of {coin.Volume}g and above , try to insert small coin.", (int)HttpStatusCode.MethodNotAllowed)));
                    }

                    return Task.FromResult(new ResponseResult<CoinDto>(_mapper.Map<CoinDto>(coin), new ResponseStatus("Please note that coin jar is full and can not accept any coins", (int)HttpStatusCode.MethodNotAllowed)));

                }, throwExpection: true);

                return Ok(results);
            }
            catch (Exception er)
            {
                er.ExceptionHelper();

                return StatusCode((int)HttpStatusCode.InternalServerError, new ResponseResult<CoinDto>(_mapper.Map<CoinDto>(coin), new ResponseStatus($"{er.Message}", er.HResult)));
            }
        }

        #endregion

    }
}
