using System;
using System.Collections.Generic;
using System.Text;

namespace GlobalKinetic.CoinJar.Core.Models
{
    public class ResponseResult<T>
    {
        public ResponseResult(T data, ResponseStatus status)
        {
            Data = data;
            ResponseStatus = status;
        }

        public T Data { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    public class ResponseStatus
    {
        public ResponseStatus(string message , int code)
        {
            Message = message;
            Code = code;
        }
        public string Message { get; set; }
        public int Code { get; set; }
    }
}
