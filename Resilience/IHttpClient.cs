using System.Threading.Tasks;
using System.Net.Http;

namespace Resilience
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> PostAsync<T>(
            string url,
            T item,
            string authorizationToken,
            string requestId = null,
            string authorizationMethod = "Bearer");
    }
}