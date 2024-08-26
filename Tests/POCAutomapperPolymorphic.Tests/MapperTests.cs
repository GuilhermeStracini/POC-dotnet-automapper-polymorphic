using System;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using POCAutomapperPolymorphic.Dtos;
using POCAutomapperPolymorphic.Models;
using POCAutomapperPolymorphic.Profiles;
using Xunit;

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

    /// <summary>
    /// Tests the mapping functionality from a base model to a data transfer object (DTO).
    /// </summary>
    /// <remarks>
    /// This unit test verifies that the properties of the <see cref="BaseModel"/> are correctly mapped to the
    /// corresponding properties of the <see cref="BaseDto"/>. It creates an instance of <see cref="BaseModel"/>
    /// with various properties, including nested objects, and then uses an object mapper to convert it into a
    /// <see cref="BaseDto"/>. The test checks that all properties are mapped accurately, ensuring that the
    /// mapping logic is functioning as expected. Additionally, it verifies that the derived properties are
    /// correctly assigned and that they adhere to the expected types.
    /// </remarks>
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
            DerivedProperty = new DerivedBaseModel { GuidProperty = _fixture.Create<Guid>() },
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
        dto.InnerProperty.DateTimeOffsetProperty.Should()
            .Be(baseModel.InnerProperty.DateTimeOffsetProperty);
        dto.DerivedProperty.Should().NotBeNull();
        dto.DerivedProperty.Should().BeAssignableTo<DerivedBaseDto>();
        dto.DerivedProperty.Should().NotBeAssignableTo<DerivedADto>();
        dto.DerivedProperty.Should().NotBeAssignableTo<DerivedBDto>();
        dto.DerivedProperty.GuidProperty.Should().Be(baseModel.DerivedProperty.GuidProperty);
    }

    /// <summary>
    /// Tests the mapping of a derived model to a Data Transfer Object (DTO).
    /// </summary>
    /// <remarks>
    /// This unit test verifies that the mapping from a <see cref="BaseModel"/> instance to a <see cref="BaseDto"/>
    /// instance is performed correctly. It sets up a <see cref="BaseModel"/> with various properties, including
    /// a derived model <see cref="DerivedAModel"/>. The test then invokes the mapping function and asserts that
    /// the resulting DTO has the expected properties and values. It checks that the inner properties are mapped
    /// correctly and that the derived property is of the correct type and contains the expected values.
    /// The test ensures that the mapping logic handles derived types appropriately, confirming that the
    /// <see cref="DerivedADto"/> is correctly populated from the <see cref="DerivedAModel"/>.
    /// </remarks>
    [Fact]
    public void MapperShouldMapDerivedA()
    {
        // Arrange
        var derivedAModel = new DerivedAModel
        {
            GuidProperty = _fixture.Create<Guid>(),
            AProperty = _fixture.Create<string>(),
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
            DerivedProperty = derivedAModel,
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
        dto.InnerProperty.DateTimeOffsetProperty.Should()
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

    /// <summary>
    /// Tests the mapping functionality from a <see cref="BaseModel"/> to a <see cref="BaseDto"/>
    /// specifically for the derived type <see cref="DerivedBModel"/>.
    /// </summary>
    /// <remarks>
    /// This unit test method sets up a <see cref="BaseModel"/> instance containing properties
    /// including a derived model of type <see cref="DerivedBModel"/>. It uses AutoMapper to map
    /// the base model to a data transfer object (DTO) of type <see cref="BaseDto"/>.
    /// The test then asserts that the resulting DTO is not null and that all properties are correctly
    /// mapped from the original model to the DTO, including nested properties.
    /// It also verifies that the derived property in the DTO is of the expected type and contains
    /// the correct values from the derived model.
    /// </remarks>
    [Fact]
    public void MapperShouldMapDerivedB()
    {
        // Arrange
        var derivedBModel = new DerivedBModel
        {
            GuidProperty = _fixture.Create<Guid>(),
            BProperty = _fixture.Create<string>(),
            BGuidProperty = _fixture.Create<Guid>(),
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
            DerivedProperty = derivedBModel,
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
        dto.InnerProperty.DateTimeOffsetProperty.Should()
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
