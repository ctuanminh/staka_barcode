using Microsoft.AspNetCore.Http;

namespace Be.Common.Responses
{
	public abstract class ServiceResponse
	{
		public virtual ApiResponse Ok(object data = default, string code = "", string message = "") => ApiResponse.Succeed(StatusCodes.Status200OK, data, code, message);
		public virtual ApiResponse BadRequest(string code = "", string message = "") => ApiResponse.Fail(StatusCodes.Status400BadRequest,"", code, message);
	}
}
