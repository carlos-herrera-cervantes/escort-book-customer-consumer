using Xunit;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EscortBookCustomer.Consumer.Contexts;
using EscortBookCustomer.Consumer.Repositories;
using EscortBookCustomer.Consumer.Models;
using EscortBookCustomer.Consumer.Constants;

namespace EscortBookCustomer.Tests.Repositories;

[Collection(nameof(ProfileStatusCategoryRepository))]
public class ProfileStatusCategoryRepositoryTests
{
    #region snippet_Properties

    private readonly DbContextOptions<EscortBookCustomerConsumerContext> _contextOptions;

    #endregion

    #region snippet_Constructors

    public ProfileStatusCategoryRepositoryTests()
    {
        _contextOptions = new DbContextOptionsBuilder<EscortBookCustomerConsumerContext>()
            .UseNpgsql(PostgresClient.CustomerProfile)
            .Options;
    }

    #endregion

    #region snippet_Tests

    [Fact(DisplayName = "Should return null when record does not exists")]
    public async Task GetByNameShouldReturnNull()
    {
        using var context = new EscortBookCustomerConsumerContext(_contextOptions);
        var profileStatusCategoryRepository = new ProfileStatusCategoryRepository(context);

        ProfileStatusCategory category = await profileStatusCategoryRepository.GetByName("test");
        Assert.Null(category);
    }

    #endregion
}
