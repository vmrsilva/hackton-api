//using Hackton.Infrastructure.Context;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;
//using Microsoft.Extensions.Configuration;
//using System.IO;



//namespace Hackton.Infrastructure
//{
//    public class HacktonContextFactory : IDesignTimeDbContextFactory<HacktonContext>
//    {
//        public HacktonContext CreateDbContext(string[] args)
//        {
//            // Lê a configuração de appsettings.json
//            var configuration = new ConfigurationBuilder()
//                .SetBasePath(Directory.GetCurrentDirectory()) // importante para o EF localizar o appsettings corretamente
//                .AddJsonFile("appsettings.json", optional: false)
//                .Build();

//            var optionsBuilder = new DbContextOptionsBuilder<HacktonContext>();

//            var connectionString = configuration.GetConnectionString("Database");

//            optionsBuilder.UseSqlServer(connectionString);

//            return new HacktonContext(optionsBuilder.Options);
//        }
//    }
//}
