using MagazineManager.Server.Data.DAL;

namespace MagazineManager.Server.Data.Repositories.Abstraction
{
    public class BaseRepository<T> where T : class
    {
        public BaseRepository(): base(){
        }

        public BaseDAL<T> _DAL { get; set; }

        public T CreateInstance<T>(Dictionary<string, object> items)
        {
            var properties = typeof(T).GetProperties();
            object[] objectsArray = new object[properties.Length];

            int i = 0;
            foreach (var property in properties)
            {
                var name = property.Name;
                var value = items[name];                

                objectsArray[i] = value;
                i++;
            }

            var result = Activator.CreateInstance(typeof(T), objectsArray);
            
            return (T)result;
        }

        public T? Get(int id, string tableName)
        {
            string commandText = string.Format("SELECT * FROM {0} WHERE Id = @Id", tableName);
            var command = _DAL.BuildCommand(commandText);

            var parameterItems = FillParameter<int>("Id", id);
            var paramaters = _DAL.CreateParameters(parameterItems.ToArray());
            command.Parameters.AddRange(paramaters);

            var items = _DAL.Get(command, this);

            return items.FirstOrDefault();
        }

        public bool Exists(int id, string tableName)
        {
            string commandText = string.Format("SELECT 1 FROM {0} WHERE Id = @Id", tableName);
            var command = _DAL.BuildCommand(commandText);

            var items = FillParameter<int>("Id", id);
            var paramaters = _DAL.CreateParameters(items.ToArray());
            command.Parameters.AddRange(paramaters);

            return _DAL.Exists(command);
        }

        public IEnumerable<T>? GetAll(string tableName)
        {
            string commandText = string.Format("SELECT * FROM {0}", tableName);
            var command = _DAL.BuildCommand(commandText);
            return _DAL.GetAll(command, this);
        }

        public IEnumerable<T>? GetByName(string tableName, string fieldName, string fieldValue)
        {
            string commandText = string.Format("SELECT * FROM {0} WHERE {1} LIKE @{2}", tableName, fieldName, fieldName);
            var command = _DAL.BuildCommand(commandText);
            var items = FillParameter<string>(fieldName, fieldValue);
            var paramaters = _DAL.CreateParameters(items.ToArray());
            command.Parameters.AddRange(paramaters);

            var user = _DAL.Get(command, this);
            return user;
        }

        public IEnumerable<T>? GetByForeignKey(string tableName, string fkName, int fkValue)
        {
            string commandText = string.Format("SELECT * FROM {0} WHERE {1} = @{2}", tableName, fkName, fkName);
            var command = _DAL.BuildCommand(commandText);
            var items = FillParameter<int>(fkName, fkValue);
            var paramaters = _DAL.CreateParameters(items.ToArray());
            command.Parameters.AddRange(paramaters);

            var user = _DAL.Get(command, this);
            return user;
        }

        public void Post(T model, string tableName, string fields, string parameters)
        {
            string commandText = string.Format("INSERT INTO {0} ({1}) VALUES ({2})", tableName, fields, parameters);
            var command = _DAL.BuildCommand(commandText);

            var items = FillAllParameters(model);
            var parmaters = _DAL.CreateParameters(items.ToArray());
            command.Parameters.AddRange(parmaters);

            _DAL.Add(command);
        }

        public void Put(T model, string tableName, string fields)
        {
            string commandText = string.Format("UPDATE {0} SET {1} WHERE Id = @Id", tableName, fields);
            var command = _DAL.BuildCommand(commandText);
            var items = FillAllParameters(model, true);
            var parmaters = _DAL.CreateParameters(items.ToArray());
            command.Parameters.AddRange(parmaters);
            _DAL.Update(command);
        }

        public void Delete(int id, string tableName)
        {
            string commandText = string.Format("DELETE {0} WHERE Id = @Id", tableName);
            var command = _DAL.BuildCommand(commandText);

            var items = FillParameter<int>("Id", id);
            var parmaters = _DAL.CreateParameters(items.ToArray());
            command.Parameters.AddRange(parmaters);

            _DAL.Delete(command);
        }

        public List<KeyValuePair<string, object>> FillParameter<T>(string paramName, object value)
        {
            List<KeyValuePair<string, object>> items = new List<KeyValuePair<string, object>>()
            {
                new KeyValuePair<string, object>(paramName, (T)value)
            };

            return items;
        }

        public List<KeyValuePair<string, object>> FillAllParameters<T>(T obj, bool doIdParameter= false)
        {
            List<KeyValuePair<string, object>> items = new List<KeyValuePair<string, object>>();
            KeyValuePair<string, object> item;

            var properties = obj.GetType().GetProperties();

            foreach (var property in properties)
            {
                if (property.Name == "Id")
                    if (!doIdParameter)
                        continue;

                var value = GetPropValue(obj, property.Name);

                item = new KeyValuePair<string, object>(property.Name, value);
                items.Add(item);
            }

            return items;
        }

        object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
    }
}
