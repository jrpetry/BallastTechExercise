using System.Data;

namespace MagazineManager.Models
{
    public class ApplicationUser
    {
        public ApplicationUser() { }

        //Used by Activator Create Instance
        public ApplicationUser(int id, string userName, string pwd, string role) 
        {
            Id = id;
            UserName = userName;
            Pwd = pwd;
            Role = role;
        }

        public int Id { get; set; }
        public string UserName { get; set; } = "";
        public string Pwd { get; set; } = "";
        public string Role { get; set; } = "";
    }
}
