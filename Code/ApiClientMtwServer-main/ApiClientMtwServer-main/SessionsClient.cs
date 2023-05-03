
using EntityMtwServer.Entities;
using MTWServerApiClient;
using Newtonsoft.Json;
using System.Text;

namespace ApiClientMtwServer
{
    public class SessionClient<T> : IClient<T> where T : Session
    {
        public HttpClient Client { get; set; } = new HttpClient();
        public string Host { get; set; } = Configuration.SessionHost;

        public async Task<T> Create(T element)
        {
            return (T)new Session();
        }
        public async Task<List<T>> Read()
        {
            var list = JsonConvert.DeserializeObject<List<T>>(await Client.GetStringAsync(Host));
            return list;
        }

        public async Task<List<T>> ReadByUser(long id)
        {
            string query = Host + "/instructorId/" + id;
            return JsonConvert.DeserializeObject<List<T>>(await Client.GetStringAsync(query));
        }

        public async Task<T> Read(long sessionId)
        {
            return JsonConvert.DeserializeObject<T>(await Client.GetStringAsync(Host + "/" + sessionId));
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
