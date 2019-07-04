using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace WebStore.Clients.Base
{
    public abstract class BaseClient
    {
        public readonly HttpClient _client;

        protected readonly string _ServiceAddress;

        protected BaseClient(IConfiguration Configuration, string ServiceAdress)
        {
            _ServiceAddress = ServiceAdress;
            _client = new HttpClient {BaseAddress = new Uri(Configuration["ClientAdress"])};
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        protected async Task<T> GetAsync<T>(string url, CancellationToken Cancel = default) where T : new()
        {
            var response = await _client.GetAsync(url, Cancel);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<T>(Cancel);
            return new T();
        }
        protected T Get<T>(string url) where T: new()
        {
            return GetAsync<T>(url).Result;
        }
        protected async Task<HttpResponseMessage> PostAsync<T>(string url, T item, CancellationToken Cancel = default)
        {
            return (await _client.PostAsJsonAsync(url, item, Cancel)).EnsureSuccessStatusCode();
        }
        protected async Task<HttpResponseMessage> PutAsync<T>(string url, T item, CancellationToken Cancel = default)
        {
            return (await _client.PutAsJsonAsync(url, item, Cancel)).EnsureSuccessStatusCode();
        }
        protected HttpResponseMessage Post<T>(string url, T item) => PostAsync(url, item).Result;
        protected HttpResponseMessage Put<T>(string url, T item) => PutAsync(url, item).Result;
        protected async Task<HttpResponseMessage> DeleteAsync(string url, CancellationToken Cancel = default)
        {
            return await _client.DeleteAsync(url, Cancel);
        }
        protected HttpResponseMessage Delete(string url) => DeleteAsync(url).Result;//
    }
}
