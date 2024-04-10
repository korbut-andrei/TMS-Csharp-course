using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Lesson_Async_Programming
{
    public class NumberService : INumbersService
    {
        public async Task<IEnumerable<int>> FindEvenNumbers(IEnumerable<int> numbers, CancellationToken cancellationToken)
        {
            
            var response = new List<int>();
            
            await Task.Run(() =>
            {
                Parallel.ForEach(numbers, new ParallelOptions { CancellationToken = cancellationToken }, item =>
                {
                    // Process each item
                    if (cancellationToken.IsCancellationRequested)
                        cancellationToken.ThrowIfCancellationRequested();

                    // Check if the number is even
                    if (item % 2 == 0)
                    {
                        // Add even numbers to the response list
                        lock (response) // Ensure thread-safe access to the response list
                        {
                            response.Add(item);
                        }
                    }
                });
            }, cancellationToken);

            return response;
        }
    }
}
