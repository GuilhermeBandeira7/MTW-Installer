using EntityMtwServer.Entities;
using MTWServerApiClient;
using Newtonsoft.Json;
using System.Text;

namespace ApiClientMtwServer
{
    public class DvcClient<T> : IClient<T> where T : DVC
    {
        public HttpClient Client { get; set; } = new HttpClient();
        public string Host { get; set; } = Configuration.DvcHost;

        public async Task<T> Create(T element)
        {
            return (T)new DVC();
        }
        public async Task<List<T>> Read()
        {
            var list = JsonConvert.DeserializeObject<List<T>>(await Client.GetStringAsync(Host));
            return list;
        }
        public async Task<T> Read(long id)
        {
            return JsonConvert.DeserializeObject<T>(await Client.GetStringAsync(Host + "/" + id));
        }

        public async Task<T> Update(T element)
        {
            var content = new StringContent(JsonConvert.SerializeObject(element), Encoding.UTF8, "application/json");
            var response = await Client.PutAsync(Host + "/" + element.Id.ToString(), content);
            var responseString = await response.Content.ReadAsStringAsync();
            return element;
        }

        public void Delete(long id)
        {

        }
    }
}
