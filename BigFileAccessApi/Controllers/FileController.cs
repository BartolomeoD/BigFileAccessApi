using Microsoft.AspNetCore.Mvc;

namespace BigFileAccessApi.Controllers
{
    [ApiController]
    public class FileController : Controller
    {
        [HttpGet("")]
        public string index()
        {
            return "it`s ok";
        }
    }
}
