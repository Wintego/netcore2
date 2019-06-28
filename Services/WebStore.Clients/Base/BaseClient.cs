using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Configuration;

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
    }
}
