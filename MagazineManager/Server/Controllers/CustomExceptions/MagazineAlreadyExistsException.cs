namespace MagazineManager.Server.Controllers.CustomExceptions
{
    public class MagazineAlreadyExistsForApplicationUserException : CustomException
    {
        public MagazineAlreadyExistsForApplicationUserException() : base("Magazine already exists for the application user")
        {
        }
    }
}
