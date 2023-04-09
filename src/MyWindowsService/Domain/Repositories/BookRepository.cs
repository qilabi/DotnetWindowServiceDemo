using MyWindowsService.Domain.Models;
using MyWindowsService.MongoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWindowsService.Domain.Repositories
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        public BookRepository(IMyMongodbContext context) : base(context)
        {
        }
    }
}
