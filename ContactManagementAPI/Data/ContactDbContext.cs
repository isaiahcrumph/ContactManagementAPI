using Microsoft.EntityFrameworkCore;
using ContactManagementAPI.Models;
using System.Collections.Generic;

namespace ContactManagementAPI.Data
{
    public class ContactDbContext : DbContext
    {
        public ContactDbContext(DbContextOptions<ContactDbContext> options)
            : base(options)
        {
        }

        public DbSet<Contact> Contacts { get; set; }
    }
}