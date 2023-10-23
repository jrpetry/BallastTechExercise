namespace MagazineManager.Models
{
    public class TokenModel
    {
        public string UserName { get; set; } = "";
        public string Role { get; set; } = "";
        public int Id { get; set; }//ApplicationUserId
        public string? Token { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
