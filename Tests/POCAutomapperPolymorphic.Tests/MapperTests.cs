using AutoFixture;
using AutoMapper;
using FluentAssertions;
using POCAutomapperPolymorphic.Dtos;
using POCAutomapperPolymorphic.Models;
using POCAutomapperPolymorphic.Profiles;

namespace POCAutomapperPolymorphic.Tests;

public class MapperTests
{
    private readonly Fixture _fixture = new();
    private readonly IMapper _mapper;

    public MapperTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MapperProfile>());
        config.AssertConfigurationIsValid();
        _mapper = config.CreateMapper();
    }

    [Fact]
    public void MapperShouldMapBaseClass()
    {
        // Arrange
        var baseModel = new BaseModel
        {
            IntProperty = _fixture.Create<int>(),
            StringProperty = _fixture.Create<string>(),
            InnerProperty = new InnerModel
            {
                GuidProperty = _fixture.Create<Guid>(),
                StringProperty = _fixture.Create<string>(),
                DateOnlyProperty = DateOnly.FromDateTime(_fixture.Create<DateTime>()),
                DateTimeOffsetProperty = _fixture.Create<DateTimeOffset>(),
            },
            DerivedProperty = new DerivedBaseModel { GuidProperty = _fixture.Create<Guid>() }
        };

        // Act
        var dto = _mapper.Map<BaseDto>(baseModel);

        // Assert
        dto.Should().NotBeNull();
        dto.IntProperty.Should().Be(baseModel.IntProperty);
        dto.StringProperty.Should().Be(baseModel.StringProperty);
        dto.InnerProperty.Should().NotBeNull();
        dto.InnerProperty.GuidProperty.Should().Be(baseModel.InnerProperty.GuidProperty);
        dto.InnerProperty.StringProperty.Should().Be(baseModel.InnerProperty.StringProperty);
        dto.InnerProperty.DateOnlyProperty.Should().Be(baseModel.InnerProperty.DateOnlyProperty);
        dto
            .InnerProperty.DateTimeOffsetProperty.Should()
            .Be(baseModel.InnerProperty.DateTimeOffsetProperty);
        dto.DerivedProperty.Should().NotBeNull();
        dto.DerivedProperty.Should().BeAssignableTo<DerivedBaseDto>();
        dto.DerivedProperty.Should().NotBeAssignableTo<DerivedADto>();
        dto.DerivedProperty.Should().NotBeAssignableTo<DerivedBDto>();
        dto.DerivedProperty.GuidProperty.Should().Be(baseModel.DerivedProperty.GuidProperty);
    }

    [Fact]
    public void MapperShouldMapDerivedA()
    {
        // Arrange
        var derivedAModel = new DerivedAModel
        {
            GuidProperty = _fixture.Create<Guid>(),
            AProperty = _fixture.Create<string>()
        };
        var baseModel = new BaseModel
        {
            IntProperty = _fixture.Create<int>(),
            StringProperty = _fixture.Create<string>(),
            InnerProperty = new InnerModel
            {
                GuidProperty = _fixture.Create<Guid>(),
                StringProperty = _fixture.Create<string>(),
                DateOnlyProperty = DateOnly.FromDateTime(_fixture.Create<DateTime>()),
                DateTimeOffsetProperty = _fixture.Create<DateTimeOffset>(),
            },
            DerivedProperty = derivedAModel
        };

        // Act
        var dto = _mapper.Map<BaseDto>(baseModel);

        // Assert
        dto.Should().NotBeNull();
        dto.IntProperty.Should().Be(baseModel.IntProperty);
        dto.StringProperty.Should().Be(baseModel.StringProperty);
        dto.InnerProperty.Should().NotBeNull();
        dto.InnerProperty.GuidProperty.Should().Be(baseModel.InnerProperty.GuidProperty);
        dto.InnerProperty.StringProperty.Should().Be(baseModel.InnerProperty.StringProperty);
        dto.InnerProperty.DateOnlyProperty.Should().Be(baseModel.InnerProperty.DateOnlyProperty);
        dto
            .InnerProperty.DateTimeOffsetProperty.Should()
            .Be(baseModel.InnerProperty.DateTimeOffsetProperty);
        dto.DerivedProperty.Should().NotBeNull();
        dto.DerivedProperty.Should().BeAssignableTo<DerivedBaseDto>();
        dto.DerivedProperty.Should().NotBeAssignableTo<DerivedBDto>();
        dto.DerivedProperty.GuidProperty.Should().Be(derivedAModel.GuidProperty);

        var derivedADto = dto.DerivedProperty.Should().BeAssignableTo<DerivedADto>().Subject;
        derivedADto.Should().NotBeNull();
        derivedADto.GuidProperty.Should().Be(derivedAModel.GuidProperty);
        derivedADto.AProperty.Should().Be(derivedAModel.AProperty);
    }

    [Fact]
    public void MapperShouldMapDerivedB()
    {
        // Arrange
        var derivedBModel = new DerivedBModel
        {
            GuidProperty = _fixture.Create<Guid>(),
            BProperty = _fixture.Create<string>(),
            BGuidProperty = _fixture.Create<Guid>()
        };
        var baseModel = new BaseModel
        {
            IntProperty = _fixture.Create<int>(),
            StringProperty = _fixture.Create<string>(),
            InnerProperty = new InnerModel
            {
                GuidProperty = _fixture.Create<Guid>(),
                StringProperty = _fixture.Create<string>(),
                DateOnlyProperty = DateOnly.FromDateTime(_fixture.Create<DateTime>()),
                DateTimeOffsetProperty = _fixture.Create<DateTimeOffset>(),
            },
            DerivedProperty = derivedBModel
        };

        // Act
        var dto = _mapper.Map<BaseDto>(baseModel);

        // Assert
        dto.Should().NotBeNull();
        dto.IntProperty.Should().Be(baseModel.IntProperty);
        dto.StringProperty.Should().Be(baseModel.StringProperty);
        dto.InnerProperty.Should().NotBeNull();
        dto.InnerProperty.GuidProperty.Should().Be(baseModel.InnerProperty.GuidProperty);
        dto.InnerProperty.StringProperty.Should().Be(baseModel.InnerProperty.StringProperty);
        dto.InnerProperty.DateOnlyProperty.Should().Be(baseModel.InnerProperty.DateOnlyProperty);
        dto
            .InnerProperty.DateTimeOffsetProperty.Should()
            .Be(baseModel.InnerProperty.DateTimeOffsetProperty);
        dto.DerivedProperty.Should().NotBeNull();
        dto.DerivedProperty.Should().BeAssignableTo<DerivedBaseDto>();
        dto.DerivedProperty.Should().NotBeAssignableTo<DerivedADto>();
        dto.DerivedProperty.GuidProperty.Should().Be(derivedBModel.GuidProperty);

        var derivedBDto = dto.DerivedProperty.Should().BeAssignableTo<DerivedBDto>().Subject;
        derivedBDto.Should().NotBeNull();
        derivedBDto.GuidProperty.Should().Be(derivedBModel.GuidProperty);
        derivedBDto.BProperty.Should().Be(derivedBModel.BProperty);
        derivedBDto.BGuidProperty.Should().Be(derivedBModel.BGuidProperty);
    }
}
