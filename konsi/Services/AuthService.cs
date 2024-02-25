using System.Net;
using konsi.Constants;
using konsi.Models;
using RabbitMQ.Provider.RabbitMQ.Interfaces;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;

namespace konsi.Services
{
    public class AuthService
    {
        private readonly IRabbitMQProducer _producer;
        private readonly ILogger<AuthService> _logger;
        private readonly HttpClient _client;

        public AuthService(
            IRabbitMQProducer producer,
            ILogger<AuthService> logger,
            HttpClient client
        )
        {
            _producer = producer;
            _logger = logger;
            _client = client;
        }

        public async Task Authentication(UserRequest user)
        {
            var requestUrl = $"{new Url().url}api/v1";

            if (await GetAccessToken(requestUrl, user.Username, user.Password))
            { 
                await _producer.CheckBenefits(requestUrl, new QueriesCPFs().CPFs);
            }
        }

        static async Task<bool> GetAccessToken(string apiUrl, string username, string password)
        {
            using (HttpClient client = new HttpClient())
            { 
                var requestBody = new
                {
                    username,
                    password
                };
                 
                string jsonBody = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);
                 
                HttpResponseMessage response = await client.PostAsync($"{apiUrl}/token",
                    new StringContent(jsonBody, Encoding.UTF8, "application/json"));
                 
                response.EnsureSuccessStatusCode();
                  
                return response.StatusCode.Equals((HttpStatusCode)200);
            }
        }
    }
}
