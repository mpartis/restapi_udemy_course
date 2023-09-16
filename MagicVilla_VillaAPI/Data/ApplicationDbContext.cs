using MagicVilla_VillaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {

        }



        // db set gia kathe model
        public DbSet<Villa> Villas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Villa>().HasData(
                new Villa()
                {
                    Id = 1,
                    Name = "Prosili Villa",
                    Details = "Big pool, big parking, Lamia tou Gkinani",
                    ImageUrl = "",
                    Occupancy = 5,
                    Rate = 200,
                    Sqft = 550,
                    Amenity = "",
                    CreatedDate = DateTime.Now
                },
                new Villa()
                {
                    Id = 2,
                    Name = "Paralia Villa",
                    Details = "Small pool, Small parking, Hot Pefko nearby",
                    ImageUrl = "",
                    Occupancy = 4,
                    Rate = 170,
                    Sqft = 300,
                    Amenity = "",
                    CreatedDate = DateTime.Now
                },
                new Villa()
                {
                    Id = 3,
                    Name = "Kampos Villa",
                    Details = "Pool, Parking, Dogs, BBQ",
                    ImageUrl = "",
                    Occupancy = 8,
                    Rate = 250,
                    Sqft = 400,
                    Amenity = "",
                    CreatedDate = DateTime.Now
                },
                new Villa()
                {
                    Id = 4,
                    Name = "Zarmpala Villa",
                    Details = "Huge Pool, Parking, Bar, Fireplace, Hoes",
                    ImageUrl = "",
                    Occupancy = 10,
                    Rate = 400,
                    Sqft = 350,
                    Amenity = "",
                    CreatedDate = DateTime.Now
                },
                new Villa()
                {
                    Id = 5,
                    Name = "Koulouri Villa",
                    Details = "No Pool, No Parking, No Bar, No hoes",
                    ImageUrl = "",
                    Occupancy = 2,
                    Rate = 50,
                    Sqft = 90,
                    Amenity = "",
                    CreatedDate = DateTime.Now
                }
                );
        }
    }
}
