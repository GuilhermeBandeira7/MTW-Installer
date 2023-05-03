using EntityMtwServer.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTWServerApiClient
{
    public class ProfileClient 
    {
        public HttpClient Client { get; set; } = new HttpClient();
        public string Host { get; set; } = Configuration.ProfileHost;
        public async Task<object> Create(object o)
        {
            return new object();
        }
        public async Task<List<object>> Read(long userId = 0)
        {
            var list = JsonConvert.DeserializeObject<List<Profile>>(await Client.GetStringAsync(Host));
            return list.Cast<object>().ToList();
        }
        public async Task<object> Read(long id, long userId = 0)
        {
            return JsonConvert.DeserializeObject<object>(await Client.GetStringAsync(Host + "/" + id));
        }
        public async Task<object> Update(object o)
        {
            return new object();
        }
        public void Delete(long userId, object o)
        {

        }
    }
}
