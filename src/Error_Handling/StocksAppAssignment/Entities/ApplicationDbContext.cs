using System.Text.Json;
using Microsoft.EntityFrameworkCore;
namespace Entities
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public virtual DbSet<BuyOrder> BuyOrders { get; set; }
        public virtual DbSet<SellOrder> SellOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BuyOrder>().ToTable("BuyOrders");
            modelBuilder.Entity<SellOrder>().ToTable("SellOrders");

            //Seed to BuyOrders 
            string buyOrdersJson = File.ReadAllText("BuyOrders.json");
            Console.WriteLine(buyOrdersJson);
            List<BuyOrder>? buyOrders = JsonSerializer.Deserialize<List<BuyOrder>>(buyOrdersJson);

            if (buyOrders != null)
            {
                foreach (BuyOrder buyOrder in buyOrders)
                {
                    modelBuilder.Entity<BuyOrder>().HasData(buyOrder);
                }
            }

            //Seed to SellOrders 
            string sellOrdersJson = File.ReadAllText("SellOrders.json");
            List<SellOrder>? sellOrders = JsonSerializer.Deserialize<List<SellOrder>>(sellOrdersJson);

            if (sellOrders != null)
            {
                foreach (SellOrder sellOrder in sellOrders)
                {
                    modelBuilder.Entity<SellOrder>().HasData(sellOrder);
                }
            }


        }
    }
}
