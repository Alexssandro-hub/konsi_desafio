using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearch.Provider
{
    public static class ElasticSearchExtension
    {
        public static void AddElasticSearch(this IServiceCollection services, IConfiguration configuration)
        {
            var baseUrl = configuration["ElasticSettings:baseUrl"];
            var index = configuration["ElasticSettings:defaultIndex"];
            var settings = new ConnectionSettings(new Uri(baseUrl ?? "")).PrettyJson().CertificateFingerprint("7248420dc3fa11126f9fa15ef33547f3b4fcaac225c52c6e98122131166a244e").BasicAuthentication("superuser", "JbNb_unwrJy3W0OaZ07n").DefaultIndex(index);
            settings.EnableApiVersioningHeader();
             
            var client = new ElasticClient(settings);
            services.AddSingleton<IElasticClient>(client);
            CreateIndex(client, index);
        }
          
        private static void CreateIndex(IElasticClient client, string indexName)
        {
            var createIndexResponse = client.Indices.Create(indexName, index => index.Map<object>(x => x.AutoMap()));
        }
    }
}
