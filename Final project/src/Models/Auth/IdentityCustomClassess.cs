using Microsoft.AspNetCore.Identity;

namespace AndreiKorbut.CareerChoiceBackend.Models.Auth
{
    public class ApplicationUserClaim : IdentityUserClaim<int> { }
    public class ApplicationUserRole : IdentityUserRole<int> { }
    public class ApplicationUserLogin : IdentityUserLogin<int> { }
    public class ApplicationRoleClaim : IdentityRoleClaim<int> { }
    public class ApplicationUserToken : IdentityUserToken<int> { }
}
