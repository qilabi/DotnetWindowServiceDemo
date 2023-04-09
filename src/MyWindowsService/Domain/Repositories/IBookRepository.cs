using MyWindowsService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWindowsService.Domain.Repositories
{
    public interface IBookRepository: IBaseRepository<Book> 
    {
    }
}
