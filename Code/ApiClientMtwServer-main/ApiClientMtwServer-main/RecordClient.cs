using EntityMtwServer.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTWServerApiClient
{
    public class RecordClient
    {
        public HttpClient Client { get; set; } = new HttpClient();
        public string Host { get; set; } = Configuration.RecordHost;

        public async Task<object> Create(object o)
        {
            return new object();
        }
        public async Task<List<object>> Read()
        {
            var list = JsonConvert.DeserializeObject<List<Record>>(await Client.GetStringAsync(Host));
            if (list == null)
                return new List<object>();

            return list.Cast<object>().ToList();
        }
        public async Task<object> Read(long id)
        {
            try
            {
                var record = JsonConvert.DeserializeObject<Record>(await Client.GetStringAsync(Host + "/" + id));
                if (record == null)
                    return new Record();
                return record;
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
