using WCF.Services.ServiceContract;

namespace WCF.Services.ServiceContract
{
	public static class BookMapper
	{
		public static Book ToSimpleObject(this Book entity)
		{
			if (entity == null)
				return null;
			return new Book()
			{
				ISBN = entity.ISBN,
				Title = entity.Title,
				Author = entity.Author,
				Year = entity.Year,
				Description = entity.Description,
				FileName = entity.FileName,
			};

		}
	}

}
