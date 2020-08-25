using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace EcommUIService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FallbackController : Controller
    {
      
        [HttpGet]
        public IActionResult Index()
        {
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), 
                "wwwroot", "index.html"), "text/HTML");
        }
    }
}