using EntityMtwServer.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTWServerApiClient
{
    public class LprRecordClient
    {
        public HttpClient Client { get; set; } = new HttpClient();
        public string Host { get; set; } = Configuration.LprRecordHost;

        public async Task<object> Create(object o)
        {
            var content = new StringContent(JsonConvert.SerializeObject(o), Encoding.UTF8, "application/json");
            var response = await Client.PostAsync(Host, content);
            var responseString = await response.Content.ReadAsStringAsync();
            return true;
        }
        public async Task<List<object>> Read()
        {
            var list = JsonConvert.DeserializeObject<List<LprRecord>>(await Client.GetStringAsync(Host));
            if (list != null)
                return list.Cast<object>().ToList();

            return new List<object>();
        }
        public async Task<object> Read(long id)
        {
            return JsonConvert.DeserializeObject<LprRecord>(await Client.GetStringAsync(Host + "/" + id));
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
