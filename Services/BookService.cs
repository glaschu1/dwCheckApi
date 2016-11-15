using dwCheckApi.Models;
using dwCheckApi.DatabaseContexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

// Explicit joins of entities is taken from here:
// https://weblogs.asp.net/jeff/ef7-rc-navigation-properties-and-lazy-loading
// At the time of committing 5da65e093a64d7165178ef47d5c21e8eeb9ae1fc, Entity
// Framework Core had no built in support for Lazy Loading, so the above was
// used on all DbSet queries.

namespace dwCheckApi.Services 
{
    public class BookService : IBookService
    {
        private DwContext _dwContext;

        public BookService (DwContext dwContext)
        {
            _dwContext = dwContext;
        }

        public Book FindById(int id)
        {
            return _dwContext.Books
                .AsNoTracking()
                // Explicitly join entities
                .Include(book => book.Characters)
                .FirstOrDefault(book => book.BookId == id);
        }

        public Book FindByOrdinal (int id)
        {
            return _dwContext.Books
                .AsNoTracking()
                .Include(book => book.Characters)
                .FirstOrDefault(book => book.BookOrdinal == id);
        }

        public IEnumerable<Book> Search(string searchKey)
        {
            return _dwContext.Books
                       .AsNoTracking()
                       .Include(book => book.Characters)
                       .Where(book => book.BookName.Contains(searchKey)
                           || book.BookDescription.Contains(searchKey)
                           || book.BookIsbn10.Contains(searchKey)
                           || book.BookIsbn13.Contains(searchKey));
        }
        public IEnumerable<Book> GetAll()
        {
            return _dwContext.Books
                .AsNoTracking()
                .Include(book => book.Characters)
                .OrderBy(book => book.BookOrdinal);
        }
    }
}
