using Microsoft.AspNetCore.Mvc;
using Solutions.Dotnet.API.Errors;

namespace Solutions.Dotnet.API.Controllers;

[Route("errors/{errorCode}")]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : BaseApiController
{
  public IActionResult Error(int errorCode)
  {
    return new ObjectResult(new ApiResponse(errorCode));
  }
}
