namespace Lesson_Async_Programming
{
    public interface INumbersService
    {
        public Task<IEnumerable<int>> FindEvenNumbers(IEnumerable<int> numbers, CancellationToken cancellationToken);

    }
}
