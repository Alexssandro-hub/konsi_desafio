using konsi.Services;
using RabbitMQ.Provider.RabbitMQ.Interfaces;
using RabbitMQ.Provider.RabbitMQ.Provider;
using Redis.Provider.Redis.Interfaces;
using Redis.Provider.Redis.Services;

var builder = WebApplication.CreateBuilder(args);
 
builder.Services.AddScoped(typeof(HttpClient));
builder.Services.AddScoped<IRabbitMQProducer, RabbitMQProducer>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped(typeof(AuthService));

builder.Services.AddControllers(); 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
