using Sales.Api.Middlewares;
using Microsoft.AspNetCore.Mvc;

namespace Sales.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        protected IActionResult OkResponse<T>(T data, string message = "")
        {
            var response = new ResponseApi<T>
            {
                IsSuccess = true,
                Message = message,
                Data = data
            };
            return Ok(response);
        }

        protected IActionResult SuccessResponse(string message = "")
        {
            var response = new ResponseApi<object>
            {
                IsSuccess = true,
                Message = message,
                Data = null
            };
            return Ok(response);
        }
    }
}

