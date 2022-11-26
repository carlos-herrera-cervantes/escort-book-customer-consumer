using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Globalization;
using EscortBookCustomer.Consumer.Repositories;
using EscortBookCustomer.Consumer.Contexts;
using EscortBookCustomer.Consumer.Models;

namespace EscortBookCustomer.Tests.Repositories;

[Collection(nameof(ProfileRepository))]
public class ProfileRepositoryTests
{
    #region snippet_Properties

    private readonly DbContextOptions<EscortBookCustomerConsumerContext> _contextOptions;

    #endregion

    #region snippet_Constructors

    public ProfileRepositoryTests()
    {
        _contextOptions = new DbContextOptionsBuilder<EscortBookCustomerConsumerContext>()
            .UseNpgsql(Environment.GetEnvironmentVariable("PG_DB_CONNECTION"))
            .Options;
    }

    #endregion

    #region snippet_Tests

    [Fact(DisplayName = "Should create a new profile")]
    public async Task CreateAsyncShouldCreateProfile()
    {
        using var context = new EscortBookCustomerConsumerContext(_contextOptions);
        var profileRepository = new ProfileRepository(context);
        var profile = new Profile
        {
            CustomerID = "638137b268e75dbc858192da",
            Email = "test.user@example.com",
            Birthdate = DateTime.ParseExact("1990-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture)
        };

        await profileRepository.CreateAsync(profile);

        int counter = await profileRepository.CountAsync(p => p.Email != string.Empty);
        Assert.True(counter > 0);

        await profileRepository.DeleteAsync(p => p.Email == "test.user@example.com");
    }

    #endregion
}
