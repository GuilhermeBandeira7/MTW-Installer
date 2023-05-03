using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiMtwServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FolderController : ControllerBase
    {
        // GET api/<FolderController>/5
        [HttpGet("{path}")]
        public string Get(string path)
        {

            string json = GetDirectory(new DirectoryInfo(path.Replace("%2F","/"))).ToString();
            return json;
        }

        JToken GetDirectory(DirectoryInfo directory)
        {
            return JToken.FromObject(new
            {
                directory = directory.EnumerateDirectories()
                    .ToDictionary(x => x.Name, x => GetDirectory(x)),
                file = directory.EnumerateFiles().Select(x => x.Name).ToList()
            });
        }
    }
}
