using AutoMapper;
using HealthConnect.Application.Dtos;
using HealthConnect.Application.Mappers;
using HealthConnect.Domain.Models;
using Xunit;

namespace HealthConnect.Application.Tests;

public class UserMapperTests
{
    private readonly IMapper _mapper;
    private readonly IConfigurationProvider _configuration;

    public UserMapperTests()
    {
        _configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<UserMapper>();
        });

        _mapper = _configuration.CreateMapper();
    }

    [Fact]
    public void Configuration_ShouldBeValid()
    {
        _configuration.AssertConfigurationIsValid();
    }

    [Fact]
    public void Should_Map_User_To_UserSummaryDto_Correctly()
    {
        var userEntity = new User
        {
            Id = Guid.NewGuid(),
            Name = "Test User",
            Email = "test@example.com",
            CPF = "12345678901",
            Phone = "1234567890",
            HashedPassword = "hashedpassword",
            Salt = "salt",
            BirthDate = new DateOnly(1990, 1, 1)
        };

        var userDto = _mapper.Map<UserSummaryDto>(userEntity);

        Assert.NotNull(userDto);
        Assert.Equal(userEntity.Name, userDto.Name);
        Assert.Equal(userEntity.Email, userDto.Email);
    }
}