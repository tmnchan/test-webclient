using System.Linq;
using TestClient.Data;
using TestClient.Data.Model;

namespace TestClient.Services.Impl
{
    public class SettingQueryResultProvider
    {
        private DatabaseContext _databaseContext;

        public SettingQueryResultProvider()
        {
            _databaseContext = new DatabaseContext();
        }

        public SettingQueryResult Get()
        {
            return _databaseContext.SettingQueryResults.FirstOrDefault();
        }

        public void Add(SettingQueryResult entry)
        {
            _databaseContext.SettingQueryResults.Add(entry);
            _databaseContext.SaveChanges();
        }

        public void Update(SettingQueryResult entry)
        {
            _databaseContext.SettingQueryResults.Update(entry);
            _databaseContext.SaveChanges();
        }
    }
}
