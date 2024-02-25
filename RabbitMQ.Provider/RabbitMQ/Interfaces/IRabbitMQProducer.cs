namespace RabbitMQ.Provider.RabbitMQ.Interfaces;

public interface IRabbitMQProducer
{ 
    public Task CreateQueueMQ(string rabbitMqHost, string rabbitMqUsername, string rabbitMqPassword, List<string> cpfs);
    public Task GetQueueRabbitMQ(string rabbitMqHost, string rabbitMqUsername, string rabbitMqPassword);
    public Task CheckBenefits(string apiUrl, List<string> cpfs);
}