using EntityMtwServer.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTWServerApiClient
{
    public class LprClient
    {
        public HttpClient Client { get; set; } = new HttpClient();
        public string Host { get; set; } = Configuration.LprHost;

        public async Task<object> Create(object o)
        {
            return new object();
        }
        public async Task<List<object>> Read()
        {
            var list = JsonConvert.DeserializeObject<List<Lprs>>(await Client.GetStringAsync(Host));
            return list.Cast<object>().ToList();
        }
        public async Task<object> Read(long id)
        {
            try
            {
                string response = await Client.GetStringAsync(Host + "/" + id);
                return JsonConvert.DeserializeObject<Lprs>(response);
            }
            catch (Exception ex)
            {

            }

            return null;
    
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
