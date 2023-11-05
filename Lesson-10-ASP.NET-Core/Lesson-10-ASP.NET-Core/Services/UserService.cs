using Lesson_10_ASP.NET_Core.Models;
using Lesson_10_ASP.NET_Core.Services;

public class UserService : IUserService
{
    private readonly List<NewUserModel> _users = new List<NewUserModel>();

    public bool AddUser(NewUserModel model)
    {
        if (string.IsNullOrWhiteSpace(model.UserName) ||
                string.IsNullOrWhiteSpace(model.Email) ||
               string.IsNullOrWhiteSpace(model.Password))
        {
            return false;
        }

        _users.Add(model);
        return true;
    }

    public UserModel[] GetAllUsers()
    {
        return _users.Select(x => new UserModel { Email = x.Email, UserName = x.UserName, Password = x.Password }).ToArray();
    }
}