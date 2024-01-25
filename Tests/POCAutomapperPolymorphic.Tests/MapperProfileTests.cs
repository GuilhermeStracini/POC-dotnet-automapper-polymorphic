using AutoMapper;
using POCAutomapperPolymorphic.Profiles;
using Xunit;

namespace POCAutomapperPolymorphic.Tests;

public class MapperProfileTests
{
    [Fact]
    public void MapperProfileIsValid()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MapperProfile>());
        config.AssertConfigurationIsValid();
    }
}
