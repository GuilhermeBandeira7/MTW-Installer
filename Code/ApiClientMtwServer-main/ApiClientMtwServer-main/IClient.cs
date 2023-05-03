using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClientMtwServer
{
    internal interface IClient<T>
    {
        HttpClient Client { get; set; }
        string Host { get; set; }

        Task<T> Create(T element);
        Task<List<T>> Read();
        Task<T> Read(long id);
        Task<T> Update(T element);
        void Delete(long id);
    }
}
