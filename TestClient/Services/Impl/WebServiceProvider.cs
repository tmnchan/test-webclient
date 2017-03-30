using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestClient.Data;

namespace TestClient.Services.Impl
{
    public class WebServiceProvider
    {
        private DatabaseContext _databaseContext;

        public WebServiceProvider()
        {
            _databaseContext = new DatabaseContext();
        }

        public string GetUrl()
        {
            return _databaseContext.WebService.FirstOrDefault()?.Url;
        }

        public void AddOrUpdate(string url)
        {
            var webService = _databaseContext.WebService.FirstOrDefault();
            if (webService == null)
            {
                webService = new Data.Model.WebService()
                {
                    Id = 1,
                    Url = url
                };
                _databaseContext.WebService.Add(webService);
            }
            else
            {
                webService.Url = url;
                _databaseContext.WebService.Update(webService);
            }

            _databaseContext.SaveChanges();
        }
    }
}
