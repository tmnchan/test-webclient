using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestClient.TestService;

namespace TestClient.Services.Contracts
{
    public interface ISettingProvider
    {
        Task<SettingList> GetSettingList(string searchField = null);
    }
}
