using MagazineManager.Server.Data.Repositories.Abstraction;
using System.Data.Common;

namespace MagazineManager.Server.Data.DAL
{
    public abstract class BaseDAL<T> where T : class
    {
        public abstract bool Exists(DbCommand command);
        public abstract IEnumerable<T> Get(DbCommand command, BaseRepository<T> repo);
        public abstract IEnumerable<T> GetAll(DbCommand command, BaseRepository<T> repo);
        public abstract void Add(DbCommand command);
        public abstract void Update(DbCommand command);
        public abstract void Delete(DbCommand command);
        public abstract DbCommand BuildCommand(string commandText);
        public abstract DbParameter[] CreateParameters(params KeyValuePair<string, object>[] paramArray);
        public abstract DbParameter[] CreateLikeParameters(params KeyValuePair<string, object>[] paramArray);
        public abstract Dictionary<string, object> GetFieldsFromReader(DbDataReader reader);
    }
}
