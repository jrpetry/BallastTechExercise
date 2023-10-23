namespace MagazineManager.Server.Controllers.CustomExceptions
{
    public abstract class CustomException : Exception
    {
        public CustomException(string name) : base(" " + name){ }
    }
}
