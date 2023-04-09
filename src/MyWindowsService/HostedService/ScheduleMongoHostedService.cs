using MongoDB.Bson;
using MongoDB.Driver;
using MyWindowsService.MongoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWindowsService.HostedService
{
    public class ScheduleMongoHostedService : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IServiceProvider _services;
        public ScheduleMongoHostedService ( 
            IServiceProvider serviceProvider,
            ILogger<ScheduleMongoHostedService> logger)
        {
            _services = serviceProvider;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Factory.StartNew(async () =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        _logger.LogInformation("start to accessing mongodb.");
                        using (var scope = _services.CreateScope())
                        { 
                            var mongoContext = scope.ServiceProvider.GetService<IMyMongodbContext>();
                            using (var session = await mongoContext.MongoClient.StartSessionAsync())
                            {
                                // Begin transaction
                                session.StartTransaction();

                                try
                                {
                                    //Update the Order collection
                                    var update = Builders<Library>.Update.Set("OrderId", order.orderId);
                                    var updateBuilder = Builders<Library>.Update;
                                    await _orderCollection.UpdateOneAsync(filter, update);

                                    //Update Books collection nestet array
                                    await _booksCollection.UpdateOneAsync(x => x.userId == order.inputUserId,
                                    Builders<Library>.Update.Set("books.$[g].config.$[c].value", order.inputValue),
                                    new UpdateOptions
                                    {
                                        ArrayFilters = new List<ArrayFilterDefinition>
                                        {
                  new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument("g.book_name", order.inputBookName)),
                  new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument("c.key", order.key))
                                        }
                                    });

                                    //All good , lets commit the transaction 
                                    await session.CommitTransactionAsync();

                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("Error in MongoDB Transacation: " + e.Message);
                                    await session.AbortTransactionAsync();
                                }

                            }
                    }
                    catch (Exception)
                    { 
                        throw;
                    }
                    await Task.Delay(TimeSpan.FromSeconds(2));
                }
            }
            , stoppingToken, TaskCreationOptions.LongRunning, TaskScheduler.Default
            );
        }
    }
}
