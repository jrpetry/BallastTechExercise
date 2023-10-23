namespace MagazineManager.Server.Controllers.CustomExceptions
{
    public class ApplicationUserAlreadyExistsException : CustomException
    {
        public ApplicationUserAlreadyExistsException() : base("Application User already exists")
        {
        }
    }
}
