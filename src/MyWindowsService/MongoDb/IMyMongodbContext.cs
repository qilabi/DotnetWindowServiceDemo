using MongoDB.Driver;
using MyWindowsService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWindowsService.MongoDb
{
    public interface IMyMongodbContext
    {
        IMongoCollection<Book> GetCollection<Book>(string name);

        MongoClient MongoClient { get;   }

    }
}
