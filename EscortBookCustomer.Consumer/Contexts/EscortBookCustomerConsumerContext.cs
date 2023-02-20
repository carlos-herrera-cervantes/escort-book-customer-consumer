using Microsoft.EntityFrameworkCore;
using EscortBookCustomer.Consumer.Models;

namespace EscortBookCustomer.Consumer.Contexts;

public class EscortBookCustomerConsumerContext : DbContext
{
    public EscortBookCustomerConsumerContext
    (
        DbContextOptions<EscortBookCustomerConsumerContext> options
    ) : base(options) { }

    public DbSet<Profile> Profiles { get; set; }

    public DbSet<ProfileStatus> ProfileStatus { get; set; }

    public DbSet<ProfileStatusCategory> ProfileStatusCategories { get; set; }
}
