using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Syring1.Controllers;
using Syring1.Models;

namespace Syring1.DAL
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<HomeMainSlider> HomeMainSliders { get; set; }
        public DbSet<HomeMainSliderPhoto> HomeMainSliderPhotos { get; set; }

        public DbSet<OurVision> OurVisions { get; set; }
        public DbSet<HomeDepartment> HomeDepartments { get; set; }
        public DbSet<LastestNew> LastestNews { get; set; }
        public DbSet<ProDoctor> ProDoctors { get; set; }

        public DbSet<HomeMedicalCenter> HomeMedicalCenter { get; set; }
        public DbSet<HomeCoverVideo> HomeCoverVideo { get; set; }

        public DbSet<About> Abouts { get; set; }
        public DbSet<AboutPhoto> AboutPhotos { get; set; }




        public DbSet<MedicalDepartment> MedicalDepartments { get; set; }



        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<PricingPlan> PricingPlans { get; set; }

        //public DbSet<FaqCoverPhoto> FaqCoverPhoto { get; set; }


        public DbSet<FaqQuestion> FaqQuestions { get; set; }
        public DbSet<FaqCategory> FaqCategories { get; set; }


        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }


        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketProduct> BasketProducts { get; set; }
    }
}
