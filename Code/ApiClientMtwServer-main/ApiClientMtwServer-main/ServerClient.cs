using EntityMtwServer.Entities;
using MTWServerApiClient;
using Newtonsoft.Json;

namespace ApiClientMtwServer
{
    public class ServerClient
    {
        public HttpClient Client { get; set; } = new HttpClient();
        public string Host { get; set; } = Configuration.ServerHost;
        public async Task<object> Create(object o)
        {
            return new object();
        }
        public async Task<List<object>> Read()
        {
            var list = JsonConvert.DeserializeObject<List<Server>>(await Client.GetStringAsync(Host));
            return list.Cast<object>().ToList();
        }
        public async Task<object> Read(long id)
        {
            return JsonConvert.DeserializeObject<Server>(await Client.GetStringAsync(Host + "/" + id ));
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
