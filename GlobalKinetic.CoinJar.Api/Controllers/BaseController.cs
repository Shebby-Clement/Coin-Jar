using GlobalKinetic.CoinJar.Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalKinetic.CoinJar.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        public Task<T> RunSave<T>(Func<Task<T>> execute, T defaultData = default, bool throwExpection = false)
        {
            var task = new Task<T>(() => default);

            try
            { task = Policy
                        .Handle<Exception>()
                        .WaitAndRetryAsync
                        (
                            3,
                            retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                        )
                        .ExecuteAsync(async () => await execute());

                return task;
            }
            catch (Exception ex)
            {
                ex.ExceptionHelper();
                if (throwExpection)
                    throw;              
            }

            return task;
        }
    }
}
