using MagazineManager.Server.Data.Repositories.Abstraction;
using Microsoft.Data.SqlClient;
using System.Data.Common;
using System.Text;

namespace MagazineManager.Server.Data.DAL
{
    public class SqlServerDAL<T> : BaseDAL<T> where T : class
    {
        protected string _connectionString { get; set; }
        public SqlServerDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        public override bool Exists(DbCommand dbCommand)
        {
            var command = (SqlCommand)dbCommand;
            bool result = false;
            using (SqlConnection connection = command.Connection)
            {
                SqlDataReader reader;
                connection.Open();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    result = true;
                }
                connection.Close();
            }
            return result;
        }

        public override IEnumerable<T> Get(DbCommand dbCommand, BaseRepository<T> repo)
        {
            var command = (SqlCommand)dbCommand;
            List<T> list = new List<T>();
            T obj = null;
            using (SqlConnection connection = command.Connection)
            {
                SqlDataReader reader;
                connection.Open();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Dictionary<string, object> fields = GetFieldsFromReader(reader);
                    obj = repo.CreateInstance<T>(fields);
                    if (obj != null) list.Add(obj);
                }
                connection.Close();
            }
            return list;
        }

        public override IEnumerable<T> GetAll(DbCommand dbCommand, BaseRepository<T> repo)
        {
            var command = (SqlCommand)dbCommand;
            var list = new List<T>();
            T obj = null;
            using (SqlConnection connection = command.Connection)
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Dictionary<string, object> fields = GetFieldsFromReader(reader);
                        obj = repo.CreateInstance<T>(fields);
                        if (obj != null) list.Add(obj);
                    }
                }
                connection.Close();
            }
            return list;

        }

        public override void Add(DbCommand dbCommand)
        {
            var command = (SqlCommand)dbCommand;
            using (SqlConnection connection = command.Connection)
            {
                connection.Open();

                int result = command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public override void Update(DbCommand dbCommand)
        {
            var command = (SqlCommand)dbCommand;
            using (SqlConnection connection = command.Connection)
            {
                connection.Open();

                int result = command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public override void Delete(DbCommand dbCommand)
        {
            var command = (SqlCommand)dbCommand;
            using (SqlConnection connection = command.Connection)
            {
                connection.Open();

                int result = command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public override DbCommand BuildCommand(string commandText)
        {
            var command = new SqlCommand(commandText, new SqlConnection(_connectionString));
            return command;
        }

        public override DbParameter[] CreateParameters(params KeyValuePair<string, object>[] paramArray)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            foreach (KeyValuePair<string, object> param in paramArray)
            {
                object paramValue;
                if (param.Key == "Pwd")
                    paramValue = Encoding.ASCII.GetBytes(param.Value.ToString());
                else
                    paramValue = param.Value;

                parameters.Add(new SqlParameter(string.Format("@{0}", param.Key), paramValue));
            }
            return parameters.ToArray();
        }

        public override DbParameter[] CreateLikeParameters(params KeyValuePair<string, object>[] paramArray)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            foreach (KeyValuePair<string, object> param in paramArray)
            {
                parameters.Add(new SqlParameter(string.Format("@{0}", param.Key), param.Value));
            }
            return parameters.ToArray();
        }

        public override Dictionary<string, object> GetFieldsFromReader(DbDataReader dbDataReader)
        {
            SqlDataReader reader = (SqlDataReader)dbDataReader;
            var columns = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
            var values = Enumerable.Range(0, reader.FieldCount).Select(reader.GetValue).ToList();

            Dictionary<string, object> fields = new Dictionary<string, object>();
            for (int i = 0; i < columns.Count; i++)
            {
                var value = (values[i].GetType() == typeof(byte[]))
                    ? Encoding.Default.GetString((byte[])values[i])
                    : values[i];

                fields.Add(columns[i], value);
            }

            return fields;
        }        
    }
}
