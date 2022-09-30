using CoreWCF;
using WCF.Services.ServiceContract;
using WCF.Services.ServiceContracts;

namespace WCF.Services
{
	[ServiceContract(Name = "IBookService", Namespace = "http://wpf.book.core")]
	public interface IBookService
	{
		[OperationContract]
		PingOutput Ping();

		[OperationContract]
		Result<IEnumerable<Book>> GetAllBook();

		[OperationContract]
		Result<Book> GetBookByISBN(string ISBN);

		[OperationContract]
		Result<Book> GetBookById(long id);

		[OperationContract]
		Result AddBook(Book book);

		[OperationContract]
		Result DeleteBook(long id);

		[OperationContract]
		Result DeleteAll();
	}
	public class BookService : IBookService
	{
		private readonly ILogger _logger;
		private readonly IWebHostEnvironment _env;
		private readonly CatalogSqliteDBContext _context;
		public BookService(ILogger<BookService> logger, IWebHostEnvironment env,
			CatalogSqliteDBContext context)
		{
			_logger = logger;
			_env = env;
			_context = context;
		}

		public PingOutput Ping()
		{
			return new PingOutput() { Result = true };
		}

		public Result AddBook(Book book)
		{
			Result result = new Result();
			var contentRootPath = _env.ContentRootPath;
			try
			{
				if (string.IsNullOrWhiteSpace(book.FileName) == false && contentRootPath != null)
				{
					string filePath = Path.Combine(contentRootPath.ToString(), "FrontPage", book.FileName);
					book.FrontPage = File.ReadAllBytes(filePath);
				}

				_context.BookDatas.Add(book);
				_context.SaveChanges();
				result.IsCompleted = true;
			}
			catch (Exception ex)
			{
				result.IsCompleted = false;
				result.Message = ex.Message;
			}
			return result;
		}

		public Result DeleteBook(long id)
		{
			Result result = new Result();
			try
			{
				var entity = _context.BookDatas.Where(e => (e.ID == id)).FirstOrDefault();
				if (entity != null)
				{
					_context.BookDatas.Remove(entity);
					_context.SaveChanges();
				}
				result.IsCompleted = true;
			}
			catch (Exception ex)
			{
				result.IsCompleted = false;
				result.Message = ex.Message;
			}
			return result;
		}

		public Result DeleteAll()
		{
			Result result = new Result();
			try
			{
				_context.BookDatas.ToList().ForEach(e => _context.BookDatas.Remove(e));
				_context.SaveChanges();
				result.IsCompleted = true;
			}
			catch (Exception ex)
			{
				result.IsCompleted = false;
				result.Message = ex.Message;
			}
			return result;
 		}


		public Result<IEnumerable<Book>> GetAllBook()
		{
			Result<IEnumerable<Book>> result = new Result<IEnumerable<Book>>();
			try
			{
				var entertiList = this._context.BookDatas.ToList();
				//result.Data = entertiList.Select(x=>x.ToSimpleObject());
				result.Data = entertiList;
				result.IsCompleted = true;
			}
			catch (Exception ex)
			{
				result.IsCompleted = false;
				result.Message = ex.Message;
			}
			return result;
		}


		public Result<Book> GetBookByISBN(string ISBN)
		{
			Result<Book> result = new Result<Book>() ;
			try
			{
				var entity = _context.BookDatas.Where(e => (e.ISBN == ISBN)).FirstOrDefault();
				if (entity != null)
				{
					result.Data = entity;
				}
				result.IsCompleted = true;
			}
			catch (Exception ex)
			{
				result.IsCompleted = false;
				result.Message = ex.Message;
			}
			return result;
		}

		public Result<Book> GetBookById(long id)
		{
			Result<Book> result = new Result<Book>();
			try
			{
				var entity = _context.BookDatas.Where(e => (e.ID == id)).FirstOrDefault();
				if (entity != null)
				{
					result.Data = entity;
				}
				result.IsCompleted = true;
			}
			catch (Exception ex)
			{
				result.IsCompleted = false;
				result.Message = ex.Message;
			}
			return result;
		}

	}
}
