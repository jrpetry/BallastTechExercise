using MagazineManager.Models;
using MagazineManager.Server.Data.DAL;
using MagazineManager.Server.Data.Repositories.Abstraction;

namespace MagazineManager.Server.Data.Repositories.Implementation
{
    public class MagazineRepository : BaseRepository<Magazine>
    {
        const string TABLE_NAME = "MAGAZINES";
        const string PARAMS_COMMA_SEPARATED = "@Name, @ReleaseDate, @ApplicationUserId";
        const string FIELDS_COMMA_SEPARATED = "Name, ReleaseDate, ApplicationUserId";
        const string SET_STATEMENT = "Name = @Name, ReleaseDate = @ReleaseDate, ApplicationUserId = @ApplicationUserId";
        const string LIKE_FIELD_NAME = "Name";
        const string FK_FIELD_NAME = "ApplicationUserId";
        public MagazineRepository(string connectionString)
        {
            base._DAL = new SqlServerDAL<Magazine>(connectionString);
        }

        public Magazine? Get(int id)
        {
            return base.Get(id, TABLE_NAME);
        }

        public bool Exists(Magazine model)
        {
            return base.Exists(model.Id, TABLE_NAME);
        }

        public IEnumerable<Magazine>? GetByName(string magazineName)
        {
            return base.GetByName(TABLE_NAME, LIKE_FIELD_NAME, magazineName);
        }

        public IEnumerable<Magazine>? GetByApplicationUserId(int applicationUserId)
        {
            return base.GetByForeignKey(TABLE_NAME, FK_FIELD_NAME, applicationUserId);
        }

        public IEnumerable<Magazine>? GetAll()
        {
            return base.GetAll(TABLE_NAME);
        }

        public void Post(Magazine model)
        {
            base.Post(model, TABLE_NAME, FIELDS_COMMA_SEPARATED, PARAMS_COMMA_SEPARATED);
        }

        public void Put(Magazine model)
        {
            base.Put(model, TABLE_NAME, SET_STATEMENT);
        }

        public void Delete(int id)
        {
            base.Delete(id, TABLE_NAME);
        }
    }
}
