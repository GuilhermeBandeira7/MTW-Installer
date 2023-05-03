using EntityMtwServer.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTWServerApiClient
{
    public class UserClient 
    {
        public string Host { get; set; } = Configuration.UserHost;
        public HttpClient Client { get; set; } = new HttpClient();
 
        public async Task<object> Create(object o)
        {
            string json = JsonConvert.SerializeObject(o);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Client.PostAsync(new Uri(Host), content);
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<object>(responseString);
        }
        public async Task<List<object>> Read(long userId)
        {
            return JsonConvert.DeserializeObject<List<object>>(await Client.GetStringAsync(Host + "/" + userId));
        }
        public async Task<object> Read(long userId, long id)
        {
            return JsonConvert.DeserializeObject<object>(await Client.GetStringAsync(Host + "/" + id + "/" + userId));
        }
        public async Task<object> Read(string userName, string password)
        {
            string query = Host + "/" + userName + "/" + password;
            var result = await Client.GetStringAsync(query);
            return JsonConvert.DeserializeObject<User>(result);
        }
        public async Task<object> Update(object o)
        {
            return new object();
        }

    }
}
