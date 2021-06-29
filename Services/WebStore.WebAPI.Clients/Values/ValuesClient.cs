using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;

using WebStore.Interfaces.TestAPI;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Values
{
    public class ValuesClient : BaseClient, IValuesService
    {
        public ValuesClient(HttpClient client) : base(client, "api/values") { }

        public IEnumerable<string> GetAll()
        {
            var response = Http.GetAsync(Address).Result;
            
            if (response.IsSuccessStatusCode)
                return response.Content.ReadFromJsonAsync<IEnumerable<string>>().Result;

            return Enumerable.Empty<string>();
        }

        public string GetByIndex(int index)
        {
            var response = Http.GetAsync($"{Address}/{index}").Result;

            if (response.IsSuccessStatusCode)
                return response.Content.ReadFromJsonAsync<string>().Result;

            return string.Empty;
        }

        public void Add(string value)
        {
            var response = Http.PostAsJsonAsync(Address, value).Result;

            response.EnsureSuccessStatusCode();
        }

        public void Update(int index, string value)
        {
            var response = Http.PutAsJsonAsync($"{Address}/update/{index}", value).Result;

            response.EnsureSuccessStatusCode();
        }

        public bool Delete(int index)
        {
            var response = Http.DeleteAsync($"{Address}/remove/{index}").Result;

            return response.IsSuccessStatusCode;
        }
    }
}
