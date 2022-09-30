using Microsoft.EntityFrameworkCore;
using WCF.Services.ServiceContract;

namespace WCF.Services
{
 	public class CatalogSqliteDBContext : DbContext
	{
		public CatalogSqliteDBContext(DbContextOptions<CatalogSqliteDBContext> options) : base(options)
		{
        }

		//protected override void OnConfiguring(DbContextOptionsBuilder builder)
		//{
		//	builder.UseSqlite(@"Data Source=catalog.db");
 	//	}

		public DbSet<Book> BookDatas { get; set; }

		protected override void OnModelCreating(
						ModelBuilder modelBuilder)
		{
		   base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Book>().ToTable("Book");
		}

	}
}
