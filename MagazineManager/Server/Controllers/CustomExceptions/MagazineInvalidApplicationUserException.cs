namespace MagazineManager.Server.Controllers.CustomExceptions
{
    public class MagazineInvalidApplicationUserException : CustomException
    {
        public MagazineInvalidApplicationUserException() : base("Invalid Application User linked to this Magazine")
        {
        }
    }
}
