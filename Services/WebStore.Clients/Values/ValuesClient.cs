using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WebStore.Clients.Base;
using WebStore.Interfaces.Api;

namespace WebStore.Clients.Values
{
    public class ValuesClient: BaseClient, IValuesService
    {
        public ValuesClient(IConfiguration Configuration) : base(Configuration, "api/values")
        {
        }

        public IEnumerable<string> Get()
        {
            return GetAsync().Result;
        }

        public async Task<IEnumerable<string>> GetAsync()
        {
            var response = await _client.GetAsync(_ServiceAddress);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<List<string>>();
            return new string[0];
        }

        public string Get(int id)
        {
            return GetAsync(id).Result;
        }

        public async Task<string> GetAsync(int id)
        {
            var response = await _client.GetAsync($"{_ServiceAddress}/{id}");
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<string>();
            return string.Empty;
        }

        public Uri Post(string value)
        {
            return PostAsync(value).Result;
        }

        public async Task<Uri> PostAsync(string value)
        {
            var response = await _client.PostAsJsonAsync($"{_ServiceAddress}/post", value);
            response.EnsureSuccessStatusCode();
            return response.Headers.Location;
        }

        public HttpStatusCode Put(int id, string value)
        {
            throw new NotImplementedException();
        }

        public async Task<HttpStatusCode> PutAsync(int id, string value)
        {
            var response = await _client.PutAsJsonAsync($"{_ServiceAddress}/put/{id}", value);
            response.EnsureSuccessStatusCode();
            return response.StatusCode;

        }

        public HttpStatusCode Delete(int id)
        {
            return DeleteAsync(id).Result;
        }

        public async Task<HttpStatusCode> DeleteAsync(int id)
        {
            var response = await _client.DeleteAsync($"{_ServiceAddress}/delete/{id}");
            return response.StatusCode;
        }
    }
}
