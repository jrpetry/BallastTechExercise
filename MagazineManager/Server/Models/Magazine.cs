namespace MagazineManager.Models
{
    public class Magazine
    {
        public Magazine() { }

        //Used by Activator Create Instance
        public Magazine(int id, string name, DateTime releaseDate, int applicationUserId)
        {
            this.Id = id;
            this.Name = name;
            this.ReleaseDate = releaseDate;
            this.ApplicationUserId = applicationUserId;
        }

        public int Id { get; set; }
        public string Name { get; set; } = "";
        public DateTime ReleaseDate { get; set; }
        public int ApplicationUserId { get; set; }
    }
}
