using Lesson_10_ASP.NET_Core.Models;

namespace Lesson_10_ASP.NET_Core.Services
{
    public interface IUserService
    {
        public bool AddUser(NewUserModel model);
        public UserModel[] GetAllUsers(); 
    }
}
