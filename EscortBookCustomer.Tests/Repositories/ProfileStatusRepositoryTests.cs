using System;
using System.Threading.Tasks;
using System.Globalization;
using Xunit;
using Microsoft.EntityFrameworkCore;
using EscortBookCustomer.Consumer.Contexts;
using EscortBookCustomer.Consumer.Repositories;
using EscortBookCustomer.Consumer.Models;
using EscortBookCustomer.Consumer.Constants;

namespace EscortBookCustomer.Tests.Repositories;

[Collection(nameof(ProfileStatusRepository))]
public class ProfileStatusRepositoryTests
{
    #region snippet_Properties

    private readonly DbContextOptions<EscortBookCustomerConsumerContext> _contextOptions;

    #endregion

    #region snippet_Constructors

    public ProfileStatusRepositoryTests()
    {
        _contextOptions = new DbContextOptionsBuilder<EscortBookCustomerConsumerContext>()
            .UseNpgsql(PostgresClient.CustomerProfile)
            .Options;
    }

    #endregion

    #region snippet_Tests

    [Fact(DisplayName = "Should return null when record does not exist")]
    public async Task GetAsyncShouldReturnNull()
    {
        using var context = new EscortBookCustomerConsumerContext(_contextOptions);
        var profileStatusRepository = new ProfileStatusRepository(context);
        var profileStatus = await profileStatusRepository.GetAsync(ps => ps.CustomerID == "6381458f0d758a07a4334e71");
        Assert.Null(profileStatus);
    }

    [Fact(DisplayName = "Should create a new profile status")]
    public async Task CreateAsync()
    {
        var profile = new Profile
        {
            CustomerID = "638151b282dea38826eab954",
            Email = "test.user2@example.com",
            Birthdate = DateTime.ParseExact("1990-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture)
        };
        var category1 = new ProfileStatusCategory
        {
            Name = "Test Category 1"
        };
        var category2 = new ProfileStatusCategory
        {
            Name = "Test Category 2"
        };

        using var context = new EscortBookCustomerConsumerContext(_contextOptions);

        var profileRepository = new ProfileRepository(context);
        var profileStatusCategoryRepository = new ProfileStatusCategoryRepository(context);

        await profileRepository.CreateAsync(profile);
        await profileStatusCategoryRepository.CreateAsync(category1);
        await profileStatusCategoryRepository.CreateAsync(category2);

        var profileStatus = new ProfileStatus
        {
            ID = "a8da088a-0fad-4a57-9fb0-118f1e638e5a",
            CustomerID = "638151b282dea38826eab954",
            ProfileStatusCategoryID = category1.ID
        };

        var profileStatusRepository = new ProfileStatusRepository(context);
        await profileStatusRepository.CreateAsync(profileStatus);

        int counterBeforeDelete = await profileStatusRepository.CountAsync(ps => ps.CustomerID == "638151b282dea38826eab954");
        Assert.True(counterBeforeDelete > 0);

        profileStatus.ProfileStatusCategoryID = category2.ID;
        await profileStatusRepository.UpdateAsync(profileStatus);

        ProfileStatus getResult = await profileStatusRepository.GetAsync(ps => ps.CustomerID == "638151b282dea38826eab954");
        Assert.Equal(category2.ID, getResult.ProfileStatusCategoryID);

        await profileRepository.DeleteAsync(p => p.CustomerID != string.Empty);
        await profileStatusCategoryRepository.DeleteAsync(c => c.Name != string.Empty);

        int counterAfterDelete = await profileStatusRepository.CountAsync(ps => ps.CustomerID == "638151b282dea38826eab954");
        Assert.True(counterAfterDelete == 0);
    }

    #endregion
}
