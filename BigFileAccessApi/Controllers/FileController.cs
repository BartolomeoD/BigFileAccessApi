using BigFileAccessApi.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BigFileAccessApi.Controllers
{
    [ApiController]
    public class FileController : Controller
    {
        [HttpGet("read/{lineNumber}")]
        public string ReadLine(int lineNumber)
        {
            return $"LineNumber {lineNumber}";
        }

        [HttpPost("append")]
        public string Append(AppendRequest appendRequest)
        {
            return $"Value = {appendRequest.Value}";
        }

        [HttpGet("delete/{lineNumber}")]
        public string Delete(int lineNumber)
        {
            return $"lineNumber = {lineNumber}";
        }

        [HttpPost("add")]
        public string Add(AddRequest addRequest)
        {
            return $"LineNumber = {addRequest.LineNumber}, Value = {addRequest.Value}";
        }
    }
}
