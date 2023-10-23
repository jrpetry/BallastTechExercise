using MagazineManager.Models;
using MagazineManager.Server.Data.DAL;
using MagazineManager.Server.Data.Repositories.Abstraction;

namespace MagazineManager.Server.Data.Repositories.Implementation
{
    public class ApplicationUserRepository : BaseRepository<ApplicationUser>
    {
        const string TABLE_NAME = "APPLICATION_USERS";
        const string PARAMS_COMMA_SEPARATED = "@UserName , @Pwd, @Role";
        const string FIELDS_COMMA_SEPARATED = "UserName , Pwd, Role";
        const string SET_STATEMENT = "UserName = @UserName, Pwd = @Pwd, Role = @Role";
        const string LIKE_FIELD_NAME = "UserName";

        public ApplicationUserRepository(string connectionString)
        {
            base._DAL = new SqlServerDAL<ApplicationUser>(connectionString);
        }

        public ApplicationUser? Get(int id)
        {            
            return base.Get(id, TABLE_NAME);
        }

        public bool Exists(ApplicationUser model)
        {
            return base.Exists(model.Id, TABLE_NAME);
        }

        public IEnumerable<ApplicationUser>? GetByName(string userName)
        {
            return base.GetByName(TABLE_NAME, LIKE_FIELD_NAME, userName);
        }

        public IEnumerable<ApplicationUser>? GetAll()
        {
            return base.GetAll(TABLE_NAME);
        }

        public void Post(ApplicationUser model)
        {
            base.Post(model, TABLE_NAME, FIELDS_COMMA_SEPARATED, PARAMS_COMMA_SEPARATED);
        }

        public void Put(ApplicationUser model)
        {
            base.Put(model, TABLE_NAME, SET_STATEMENT);
        }

        public void Delete(int id)
        {
            base.Delete(id, TABLE_NAME);
        }
    }
}
