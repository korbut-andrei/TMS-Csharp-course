using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Lesson_Async_Programming.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NumbersController : ControllerBase
    {
        private readonly ILogger<NumbersController> _logger;

        private readonly INumbersService _numberService;

        public NumbersController(ILogger<NumbersController> logger, INumbersService numberService)
        {
            _logger = logger;
            _numberService = numberService;

        }

        [HttpPost(Name = "GetEvenNumbers")]
        public async Task<IActionResult>  GetEvenNumbers([FromBody] IEnumerable<int> numbers)
        {

            using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            CancellationToken cancellationToken = cancellationTokenSource.Token;

            try
            {
                await Task.Delay(TimeSpan.FromSeconds(20), cancellationToken);

                // Start the asynchronous operation with the cancellation token
                return Ok(await _numberService.FindEvenNumbers(numbers, cancellationToken));

            }
            catch (OperationCanceledException)
            {
                // Handle the cancellation of the operation
                return StatusCode(499, "Request has timed out, server hasn't processed your request in time.");
            }
            catch (Exception)
            {
                // Handle other exceptions
                return BadRequest();
            }
        }
    }
}