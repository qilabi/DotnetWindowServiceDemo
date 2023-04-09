using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MyWindowsService.Configurations.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWindowsService.MongoDb
{
    public class MyMongodbContext : IMyMongodbContext
    {
        private IMongoDatabase _db { get; set; }
        private MongoClient _mongoClient { get; set; }
        public IClientSessionHandle Session { get; set; }

        public MongoClient MongoClient => _mongoClient;

        public MyMongodbContext(IOptions<Mongosettings> configuration)
        {
            _mongoClient = new MongoClient(configuration.Value.Connection);
            _db = _mongoClient.GetDatabase(configuration.Value.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _db.GetCollection<T>(name);
        }
    }
}
