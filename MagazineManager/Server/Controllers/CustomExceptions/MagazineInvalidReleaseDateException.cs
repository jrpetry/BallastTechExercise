namespace MagazineManager.Server.Controllers.CustomExceptions
{
    public class MagazineInvalidReleaseDateException : CustomException
    {
        public MagazineInvalidReleaseDateException() : base("Invalid Release Date")
        {
        }
    }
}
