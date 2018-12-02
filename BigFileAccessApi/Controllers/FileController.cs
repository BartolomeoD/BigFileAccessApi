using System.Threading.Tasks;
using BigFileAccessApi.Abstractions;
using BigFileAccessApi.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BigFileAccessApi.Controllers
{
    [ApiController]
    public class FileController : Controller
    {
        private readonly IIndexerService _indexerService;
        private readonly IBigFileWriter _bigFileWriter;
        private readonly IBigFileReader _bigFileReader;

        public FileController(
            IIndexerService indexerService,
            IBigFileWriter bigFileWriter,
            IBigFileReader bigFileReader)
        {
            _indexerService = indexerService;
            _bigFileWriter = bigFileWriter;
            _bigFileReader = bigFileReader;
        }

        [HttpGet("read/{lineNumber}")]
        public async Task<ReadLIneResponse> ReadLine(int lineNumber)
        {
            var line = _indexerService.GetLine(lineNumber);
            var stringLine = await _bigFileReader.ReadLineAsync(line);

            return new ReadLIneResponse
            {
                Value = stringLine
            };
        }

        [HttpPost("append")]
        public async Task<ActionResult> Append(AppendRequest appendRequest)
        {
            _indexerService.AppendLine(appendRequest.Value.Length);
            await _bigFileWriter.AppendLineAsync(appendRequest.Value);
            return Ok();
        }

        [HttpGet("delete/{lineNumber}")]
        public ActionResult Delete(int lineNumber)
        {
            var line = _indexerService.GetLine(lineNumber);
            _bigFileWriter.DeleteLineAsync(line.StartOffset, line.Length);
            _indexerService.DeleteLine(lineNumber, line.Length);
            return Ok();
        }

        [HttpPost("add")]
        public async Task<ActionResult> Add(AddRequest addRequest)
        {
            var currentLine = _indexerService.GetLine(addRequest.LineNumber);
            await _bigFileWriter.AddLineAsync(currentLine.StartOffset, addRequest.Value);
            _indexerService.AddLine(addRequest.LineNumber, addRequest.Value.Length);
            return Ok();
        }
    }
}