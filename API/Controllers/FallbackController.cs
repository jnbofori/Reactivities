using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
  // note: here we use Controller instead of BaseApiController/ControllerBase
  // because we need view support for our client side app
  [AllowAnonymous]
  public class FallbackController : Controller
  {
    public IActionResult Index()
    {
      return PhysicalFile(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html"),
        "text/html"
      );
    }
  }
}