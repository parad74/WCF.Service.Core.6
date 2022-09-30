using System.Runtime.CompilerServices;
using WCF.Services.ServiceContract;

namespace WCF.Services
{
    public static class DbInitializer
	{
       
		public static void Initialize(CatalogSqliteDBContext context, IWebHostEnvironment env)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.BookDatas.Any())
            {
                return;   // DB has been seeded
            }

            Book[] books = new Book[]
            {
                new Book ("1TT1U2","Мария Покусаева","Отражения","1TT1U2.jpg", "2022"),
                new Book ("54HUY","Дем Михайлов","Инфер","54HUY.jpg","2022"),
                new Book ("TYEE1","Борис Романовский","Арчи","TYEE1.jpg","2016"),
                new Book ("4RTHJ","Ирина Котова","Королевская кровь","4RTHJ.jpg","2018"),
                new Book ("TUI152","Николай Метельский","Меняя маски","TUI152.jpg","2020"),
                new Book ("1ROTI4","Владимир Ильин","Напряжение","1ROTI4.jpg","2020")
             };

            var contentRootPath = env.ContentRootPath;
	

			foreach (Book book in books)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(book.FileName) == false && contentRootPath != null)
                    {
                        string filePath = Path.Combine(contentRootPath.ToString(), "FrontPage", book.FileName);
                        book.FrontPage = File.ReadAllBytes(filePath);
                    }

                    context.BookDatas.Add(book);
                }
                catch (Exception ex)
                {
                    ;
                }
            }
            context.SaveChanges();

        }
    }
}