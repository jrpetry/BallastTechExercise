namespace MagazineManager.Server.Controllers.CustomExceptions
{
    public class UserNotFoundException : CustomException
    {
        public UserNotFoundException() : base("Username or password doesn't match")
        {
        }
    }
}
