using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redis.Provider.Redis.Services
{
    public class VerifyExistsDataService
    {
        public bool VerifyRedis(string cpf)
        { 
            var redisConnection = ConnectionMultiplexer.Connect("localhost");
             
            IDatabase redisDb = redisConnection.GetDatabase();
            string jsonData = redisDb.StringGet(cpf);

            return !string.IsNullOrEmpty(jsonData);
        }
    }
}
