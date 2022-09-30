using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WCF.Services.ServiceContract
{

    [Table("Book")]
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public string? Title { get; set; } = "";
        public string? Author { get; set; } = "";
		public string? Year { get; set; } = "";
		public string? ISBN { get; set; } = "";
		public string? Description { get; set; } = "";
		public System.Byte[]? FrontPage { get; set; }
		public string? FileName { get; set; } = "";


		public Book()
        {
			ISBN = "";
			Title = "";
			Author = "";
			Year = "";
			Description = "";
			FileName = "";
		}
        public Book(string isbn = "", string author = "", string title = "",  string fileName = "",  string уear = "", string description ="")
        {
            #region Check parametrs
   //         if (string.IsNullOrEmpty(isbn))  throw new ArgumentNullException(nameof(isbn));
   //         if (string.IsNullOrEmpty(title)) throw new ArgumentNullException(nameof(title));
			//if (string.IsNullOrEmpty(author)) throw new ArgumentNullException(nameof(author));
			//if (string.IsNullOrEmpty(description) == false)
   //         {
   //             if (description.Length > 500)   throw new ArgumentOutOfRangeException(nameof(description));
   //         }
            #endregion

            ISBN = isbn;
            Title = title;
			Author = author;
            Year = уear;
			Description = description;
			FileName = fileName;


		}
    }

}


// https://stackoverflow.com/questions/4852558/how-to-save-image-to-a-database