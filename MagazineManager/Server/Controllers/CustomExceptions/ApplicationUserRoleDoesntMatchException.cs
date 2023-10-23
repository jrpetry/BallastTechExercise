using MagazineManager.Server.Controllers.BLL;

namespace MagazineManager.Server.Controllers.CustomExceptions
{
    public class ApplicationUserRoleDoesntMatchException : CustomException
    {
        public ApplicationUserRoleDoesntMatchException() : base(string.Format("This application user's role doesn't match. Need to be: {0}", ApplicationUserRoles.RolesString))
        {
        }
    }
}
