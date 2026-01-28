using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceIdentityCMS.Core.Models
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public string? Error { get; }
        public T? Data { get; }

        protected Result(bool isSuccess, T? data, string? error)
        {
            IsSuccess = isSuccess;
            Data = data;
            Error = error;
        }
        public static Result<T> Success(T data, string mess)
       => new(true, data, mess);

        public static Result<T> Failure(string error)
            => new(false, default, error);
    }
}
