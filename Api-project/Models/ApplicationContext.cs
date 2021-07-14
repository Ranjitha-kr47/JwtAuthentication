using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;



namespace Api_project.Models
{
    public class ApplicationContext:IdentityDbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        { }

        
        public DbSet<Register> register { get; set; }
        public DbSet<Department> department { get; set; }
        public DbSet<Designation> designation { get; set; }
        public DbSet<Login> login{get;set;}

        protected override void OnModelCreating(ModelBuilder builder)  
        {  
            
            base.OnModelCreating(builder);  
            // builder.Entity<Register>()
            //     .Property(b => b.Password)
            //     .IsRequired();
            //base.OnModelCreating(builder);
            builder.Entity<IdentityUserRole<string>>().ToTable("aspnetuserroles");
            builder.Ignore <IdentityUserLogin<string>>();
            //builder.Ignore <IdentityUserRole<string>>();
            //builder.Ignore<IdentityUserClaim<string>>();
            builder.Ignore<IdentityUserToken<string>>();
           // builder.Ignore<IdentityUser<string>>();
            //builder.Ignore<ApplicationUser>();
            
        } 
    }
}