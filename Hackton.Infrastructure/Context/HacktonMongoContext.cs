using Hackton.Domain.VideoResult.Entity;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace Hackton.Infrastructure.Context
{
    public class HacktonMongoContext
    {
        protected IMongoDatabase MongoDatabase => LazyStore.Value;

        private Lazy<IMongoDatabase> LazyStore;

        public HacktonMongoContext(IConfiguration configuration)
        {
            LazyStore = new Lazy<IMongoDatabase>(() =>
            {
                var connectionString = configuration.GetConnectionString("Mongodb");

                var settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));

                settings.SslSettings =
                  new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };

                var databaseName = configuration.GetConnectionString("MongoDbDatabase");

                var mongoClient = new MongoClient(settings);


                return mongoClient.GetDatabase(databaseName); ;
            });
        }

        public IMongoCollection<VideoResultEntity> VideoResults =>
            MongoDatabase.GetCollection<VideoResultEntity>("VideoResults");
    }
}
