using EntityMtwServer.Entities;
using Newtonsoft.Json;

namespace MTWServerApiClient
{

    public class EquipmentClient 
    {
        public HttpClient Client { get; set; } = new HttpClient();
        public string Host { get; set; } = Configuration.EquipmentHost;
        public async Task<object> Create(object o)
        {
            return new object();
        }
        public async Task<List<object>> Read(long userId)
        {
            var list = JsonConvert.DeserializeObject<List<Equipment>>(await Client.GetStringAsync(Host + "/" + userId));
            return list.Cast<object>().ToList();
        }
        public async Task<object> Read(long userId, long id)
        {
            return JsonConvert.DeserializeObject<Equipment>(await Client.GetStringAsync(Host + "/" + id + "/" + userId));
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