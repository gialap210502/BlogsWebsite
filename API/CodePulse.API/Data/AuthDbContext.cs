using CodePulse.API.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace CodePulse.API.Data;

public class AuthDbContext : IdentityDbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        var readerRoleId = "57b281f1-f1eb-4048-ba96-a745f5345edf";
        var writerRoleId = "05e7704b-cb8e-493c-b593-a3b403f367d9";

        // create reader and writer role
        var roles = new List<IdentityRole>
        {
            new IdentityRole()
            {
                Id = readerRoleId,
                Name = "Reader",
                NormalizedName = "Reader".ToUpper(),
                ConcurrencyStamp = readerRoleId
            },
            new IdentityRole()
            {
                Id = writerRoleId,
                Name = "Writer",
                NormalizedName = "Writer".ToUpper(),
                ConcurrencyStamp = writerRoleId
            }
        };

        //seed the roles
        builder.Entity<IdentityRole>().HasData(roles);

        // Create an admin user
        var adminUserId = "c2e7eaea-41e7-4f37-8ce7-fcb9c9f70ba3";
        var admin = new IdentityUser()
        {
            Id = adminUserId,
            UserName = "admin@gmail.com",
            Email = "admin@gmail.com",
            NormalizedEmail = "admin@gmail.com".ToUpper(),
            NormalizedUserName = "admin@gmail.com".ToUpper()
        };

        admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin@123");

        builder.Entity<IdentityUser>().HasData(admin);

        // Gives role to Admin  
        var adminRoles = new List<IdentityUserRole<string>>()
        {
            new(){
                UserId = adminUserId,
                RoleId = readerRoleId
            },
            new(){
                UserId = adminUserId,
                RoleId = writerRoleId
            }

        };

        builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);


    }
}