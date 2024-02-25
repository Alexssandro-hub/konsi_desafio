using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Provider.RabbitMQ.Interfaces;
using Redis.Provider.Redis.Services;

namespace RabbitMQ.Provider.RabbitMQ.Provider
{
    public class RabbitMQProducer : IRabbitMQProducer
    {
        private readonly VerifyExistsDataService _verifyExistsDataService;
        public RabbitMQProducer(VerifyExistsDataService verifyExistsDataService)
        {
            _verifyExistsDataService = verifyExistsDataService;
        }

        public async Task CheckBenefits(string apiUrl, List<string> cpfs)
        {
            using (HttpClient client = new HttpClient())
            { 
                foreach (var cpf in cpfs)
                {  
                    HttpResponseMessage response = await client.GetAsync($"{apiUrl}/inss/consulta-beneficios?cpf={cpf}");
                     
                    if (response.IsSuccessStatusCode)
                    { 
                        string content = await response.Content.ReadAsStringAsync();
                    }
                }
            }
        }

        public async Task CreateQueueMQ(string rabbitMqHost, string rabbitMqUsername, string rabbitMqPassword, List<string> cpfs)
        {
            var factory = new ConnectionFactory
            {
                HostName = rabbitMqHost,
                UserName = rabbitMqUsername,
                Password = rabbitMqPassword
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "cpf_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

                foreach (var cpf in cpfs)
                {
                    var body = Encoding.UTF8.GetBytes(cpf);
                    channel.BasicPublish(exchange: "", routingKey: "cpf_queue", basicProperties: null, body: body); 
                }
            }
        }

        public async Task GetQueueRabbitMQ(string rabbitMqHost, string rabbitMqUsername, string rabbitMqPassword)
        {
            var factory = new ConnectionFactory
            {
                HostName = rabbitMqHost,
                UserName = rabbitMqUsername,
                Password = rabbitMqPassword
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "cpf_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var cpf = Encoding.UTF8.GetString(body);
                     
                    _verifyExistsDataService.VerifyRedis(cpf); 
                    //CreateIndexesElasticsearch(cpf);
                };

                channel.BasicConsume(queue: "cpf_queue", autoAck: true, consumer: consumer); 
            }
        }
    }
}
