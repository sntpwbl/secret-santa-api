using Microsoft.EntityFrameworkCore;
using SecretSanta.Entities;

namespace SecretSanta.Context
{
    public class SecretSantaContext : DbContext
    {
        public SecretSantaContext(DbContextOptions<SecretSantaContext> options)
            : base(options)
        { }

        public DbSet<Person> People { get; set; }
        public DbSet<Group> Groups { get; set; }
    }
}