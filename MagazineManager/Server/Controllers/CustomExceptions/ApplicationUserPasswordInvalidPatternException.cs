namespace MagazineManager.Server.Controllers.CustomExceptions
{
    public class ApplicationUserPasswordInvalidPatternException : CustomException
    {
        public ApplicationUserPasswordInvalidPatternException() : base("Password invalid pattern")
        {
        }
    }
}
