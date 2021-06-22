using AutoMapper;
using GlobalKinetic.CoinJar.Core.Dtos;
using GlobalKinetic.CoinJar.Core.Helpers;
using GlobalKinetic.CoinJar.Core.Models;
using GlobalKinetic.CoinJar.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
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

        [Route("")]
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

        [Route("AddCoin")]
        [HttpPost]
        [ResponseType(typeof(CoinDto))]
        public async Task<IActionResult> AddCoin(CoinDto coinDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
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

                        _coinService.CreateCoin(coin);
                        _coinService.CoinCommit();

                        return Task.FromResult(new ResponseResult<CoinDto>(coinDto, new ResponseStatus($"New coin with Amount of ${coinDto.Amount} and Volume of {coinDto.Volume}g has been added into the jar", (int)HttpStatusCode.Created)));
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

        #endregion

    }
}
