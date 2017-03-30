using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TestClient.Data;
using TestClient.Data.Model;
using TestClient.Services.Contracts;
using TestClient.TestService;

namespace TestClient.Services.Impl
{
    public class SettingProvider : ISettingProvider
    {
        #region Properties

        private const bool _saveQueryResultToDb = true;

        private SettingQueryResultProvider _settingQueryResultService;
        private WebServiceProvider _webServiceProvider;

        private string responseFormat = "json";

        private readonly string _defaultTestServiceEndpoint = "rest";
        private readonly string _defaultTestServiceMethod = "GetItems";
        private readonly string _defaultTestServiceParameter = "SearchField";

        #endregion

        public SettingProvider()
        {
            _settingQueryResultService = new SettingQueryResultProvider();
            _webServiceProvider = new WebServiceProvider();
        }

        public async Task<SettingList> GetSettingList(string searchField = null)
        {
            var response = await GetResponse(
                _webServiceProvider.GetUrl(),
                _defaultTestServiceEndpoint,
                _defaultTestServiceMethod, 
                _defaultTestServiceParameter, 
                searchField);

            using (var dataStream = response.GetResponseStream())
            {
                Stream copiedStream = new MemoryStream();
                dataStream.CopyTo(copiedStream);

                if (_saveQueryResultToDb)
                {
                    SaveQueryResultToDb(copiedStream);
                }
                return Deserialize(copiedStream);
            }
        }

        public SettingList GetSettingListFromDb()
        {
            var queryResult = _settingQueryResultService.Get();
            SettingList result = new SettingList();
            if (!string.IsNullOrEmpty(queryResult?.LastSearchResult))
            {
                result = Deserialize(queryResult.LastSearchResult);
            }

            return result;
        }

        public string GetWebServerUrl()
        {
            return _webServiceProvider.GetUrl();
        }
        
        public void AddOrUpdateWebServerUrl(string url)
        {
            _webServiceProvider.AddOrUpdate(url);
        }

        private async Task<HttpWebResponse> GetResponse(
            string url, 
            string endPoint, 
            string method, 
            string parameter, 
            string parameterValue = null)
        {
            var request = WebRequest.Create($"{url}/{endPoint}/{method}?{parameter}={parameterValue}");
            var response = await request.GetResponseAsync();
            return (HttpWebResponse) response;
        }

        private SettingList Deserialize(string content)
        {
            byte[] byteArray = Encoding.ASCII.GetBytes(content);
            MemoryStream stream = new MemoryStream(byteArray);
            return Deserialize(stream);
        }

        private SettingList Deserialize(Stream stream)
        {
            stream.Position = 0;
            if (responseFormat == "json")
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(SettingList));
                return (SettingList) ser.ReadObject(stream);
            }
            else
            {
                StreamReader strReader = new StreamReader(stream);
                string response = strReader.ReadToEnd();

                stream.Position = 0;
                XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(stream, new XmlDictionaryReaderQuotas());
                DataContractSerializer ser = new DataContractSerializer(typeof(SettingList));

                return (SettingList)ser.ReadObject(reader, true);
            }
        }

        private void SaveQueryResultToDb(Stream stream)
        {
            stream.Position = 0;
            StreamReader reader = new StreamReader(stream);
            string response = reader.ReadToEnd();

            var settingQueryResult = _settingQueryResultService.Get();
            if (settingQueryResult == null)
            {
                settingQueryResult = new SettingQueryResult
                {
                    Id = 1,
                    LastSearchResult = response,
                    LastSearchFilter = ""
                };
                _settingQueryResultService.Add(settingQueryResult);
            }
            else
            {
                settingQueryResult.LastSearchResult = response;
                _settingQueryResultService.Update(settingQueryResult);
            }
            
        }
    }
}
