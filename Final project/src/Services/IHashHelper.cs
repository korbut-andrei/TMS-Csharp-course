namespace AndreiKorbut.CareerChoiceBackend.Services
{
    public interface IHashHelper
    {
        int ComputeHash<T>(T input);

    }
}
