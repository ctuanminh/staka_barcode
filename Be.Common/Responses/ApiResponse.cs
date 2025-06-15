using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Be.Common.Responses
{
	public class ApiResponse
	{
        public object Data { get; private set; }
        public int StatusCode { get; private set; }
        public bool Success { get; private set; }
        public string Code { get; private set; }
        public string Message { get; private set; }

        private ApiResponse()
        {
            
        }

		public static ApiResponse Succeed(int statusCode, object data = default, string code = "", string message = "")
			=> new()
			{
                StatusCode =statusCode,
                Data = data,
                Code = code,
                Message = message,
                Success = true
            };
		public static ApiResponse Fail(int statusCode, object data = default, string code = "", string message = "")
			=> new()
			{
				StatusCode = statusCode,
				Data = data,
				Code = code,
				Message = message,
				Success = false
			};
	}
}
