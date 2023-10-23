using Castle.Core.Resource;
using MagazineManager.Models;
using MagazineManager.Server.Data.Repositories.Implementation;
using Moq;

namespace MagazineManagerTest
{
    public class MagazineControllerTests
    {
        //[Fact]
        //public void CreatesMagazine()
        //{
        //    // Arrange

        //    var magazine = new Magazine() { Id = 1, Name = "Test Magazine", ApplicationUserId = 1, ReleaseDate = DateTime.Now };

            
        //    var connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=BallastExercise;Trusted_Connection=True;";
        //    var repo = new MagazineRepository(connectionString);

        //    repo.Post(magazine);

        //    var m = repo.GetByName(magazine.Name);

        //    Assert.NotNull(m);
        //}


        MagazineRepository magazineRepository;
        ApplicationUserRepository applicationUserRepository;
        Magazine magazineMock;
        ApplicationUser applicationUserMock;

        public void Setup()
        {
            var connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=BallastExercise;Trusted_Connection=True;";
            magazineRepository = new MagazineRepository(connectionString);
            applicationUserRepository = new ApplicationUserRepository(connectionString);
        }

        public void CleanUpDB(Magazine magazine = null, ApplicationUser applicationUser = null)
        {
            if (magazine != null)
            {
                magazineRepository.Delete(magazine.Id);
            }

            if (applicationUser != null)
            {
                applicationUserRepository.Delete(applicationUser.Id);
            }
        }

        [Fact]
        public void Test_Insert_New_Magazine()
        {
            var Magazine = Insert_New_Magazine();
            Assert.IsType<Magazine>(Magazine);
            CleanUpDB(Magazine);
        }

        public Magazine Insert_New_Magazine()
        {
            Setup();
            var magazineMock = new Magazine() { Id = 1, Name = "Test Magazine", ApplicationUserId = 1, ReleaseDate = DateTime.Now };

            magazineRepository.Post(magazineMock);

            var magazine = magazineRepository.GetByName("MAGAZINES", "Name", magazineMock.Name);
            return magazine.FirstOrDefault();
        }

        [Fact]
        public void Test_Insert_New_ApplicationUser()
        {
            var ApplicationUser = Insert_New_ApplicationUser();
            Assert.IsType<ApplicationUser>(ApplicationUser);
            CleanUpDB(null, ApplicationUser);
        }
        public ApplicationUser Insert_New_ApplicationUser()
        {
            Setup();
            var applicationUserMock = new ApplicationUser() { Id = 1, UserName = "Test ApplicationUser", Role = "Teacher" };

            applicationUserRepository.Post(applicationUserMock);

            var applicationUsers = applicationUserRepository.GetByName("APPLICATION_USERS", "UserName", applicationUserMock.UserName);
            return applicationUsers.FirstOrDefault();
        }

    }
}