using System;
using System.Text.Json;
using AutoMapper;
using FluentAssertions;
using POCAutomapperPolymorphic.Dtos;
using POCAutomapperPolymorphic.Models;
using POCAutomapperPolymorphic.Profiles;
using Xunit;

namespace POCAutomapperPolymorphic.Tests;

public class SerializerTests
{
    private readonly IMapper _mapper;

    private readonly JsonSerializerOptions _options = new() { WriteIndented = true };

    public SerializerTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MapperProfile>());
        config.AssertConfigurationIsValid();
        _mapper = config.CreateMapper();
    }

    /// <summary>
    /// Tests the serialization of a base class model to JSON format.
    /// </summary>
    /// <remarks>
    /// This unit test verifies that the properties of the <see cref="BaseModel"/> are correctly mapped to a <see cref="BaseDto"/>
    /// and serialized into the expected JSON structure. It sets up a base model with various properties, including an inner
    /// model and a derived property. The test checks that the mapped DTO is not null and that all properties match the
    /// original model's values. Additionally, it ensures that the serialized JSON output matches the expected JSON string.
    /// The test uses FluentAssertions for assertions to improve readability and maintainability.
    /// </remarks>
    [Fact]
    public void SerializeBaseClass()
    {
        // Arrange
        const string expectedJson =
            @"{
  ""IntProperty"": 1,
  ""StringProperty"": ""base"",
  ""InnerProperty"": {
    ""GuidProperty"": ""00000000-0000-0000-0000-000000000000"",
    ""StringProperty"": ""inner"",
    ""DateOnlyProperty"": ""0001-01-01"",
    ""DateTimeOffsetProperty"": ""1970-01-01T00:00:00+00:00""
  },
  ""DerivedProperty"": {
    ""$type"": ""base"",
    ""GuidProperty"": ""00000000-0000-0000-0000-000000000000""
  }
}";
        var baseModel = new BaseModel
        {
            IntProperty = 1,
            StringProperty = "base",
            InnerProperty = new InnerModel
            {
                GuidProperty = Guid.Empty,
                StringProperty = "inner",
                DateOnlyProperty = DateOnly.FromDayNumber(0),
                DateTimeOffsetProperty = DateTimeOffset.UnixEpoch,
            },
            DerivedProperty = new DerivedBaseModel { GuidProperty = Guid.Empty },
        };

        // Act
        var dto = _mapper.Map<BaseDto>(baseModel);
        var json = JsonSerializer.Serialize(dto, _options);

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

        json.Should().NotBeNullOrEmpty();
        json.Should().Be(expectedJson);
    }

    /// <summary>
    /// Tests the mapping of a derived model to a Data Transfer Object (DTO).
    /// </summary>
    /// <remarks>
    /// This unit test verifies that the mapping from a <see cref="BaseModel"/> to a <see cref="BaseDto"/>
    /// is performed correctly by the mapper. It creates an instance of <see cref="BaseModel"/> with
    /// nested properties, including a derived property of type <see cref="DerivedAModel"/>.
    /// The test checks that all properties are mapped accurately, including nested properties,
    /// and that the resulting JSON representation matches the expected format.
    /// The assertions ensure that the mapped DTO is not null and that its properties
    /// correspond to those of the original model. Additionally, it verifies that the derived property
    /// is of the correct type and contains the expected values.
    /// </remarks>
    /// <exception cref="System.Exception">Thrown when the mapping does not produce the expected results.</exception>
    [Fact]
    public void MapperShouldMapDerivedA()
    {
        const string expectedJson =
            @"{
  ""IntProperty"": 1,
  ""StringProperty"": ""base"",
  ""InnerProperty"": {
    ""GuidProperty"": ""00000000-0000-0000-0000-000000000000"",
    ""StringProperty"": ""inner"",
    ""DateOnlyProperty"": ""0001-01-01"",
    ""DateTimeOffsetProperty"": ""1970-01-01T00:00:00+00:00""
  },
  ""DerivedProperty"": {
    ""$type"": ""derivedA"",
    ""AProperty"": ""A"",
    ""GuidProperty"": ""00000000-0000-0000-0000-000000000000""
  }
}";
        var derivedAModel = new DerivedAModel { GuidProperty = Guid.Empty, AProperty = "A" };
        var baseModel = new BaseModel
        {
            IntProperty = 1,
            StringProperty = "base",
            InnerProperty = new InnerModel
            {
                GuidProperty = Guid.Empty,
                StringProperty = "inner",
                DateOnlyProperty = DateOnly.FromDayNumber(0),
                DateTimeOffsetProperty = DateTimeOffset.UnixEpoch,
            },
            DerivedProperty = derivedAModel,
        };

        // Act
        var dto = _mapper.Map<BaseDto>(baseModel);
        var json = JsonSerializer.Serialize(dto, _options);

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

        json.Should().NotBeNullOrEmpty();
        json.Should().Be(expectedJson);
    }

    /// <summary>
    /// Tests the mapping of a BaseModel to a BaseDto, ensuring that all properties are correctly mapped.
    /// </summary>
    /// <remarks>
    /// This unit test verifies the functionality of the mapping process between the BaseModel and BaseDto classes.
    /// It creates instances of these models, populates them with sample data, and then uses an object mapper to convert
    /// the BaseModel into a BaseDto. The test checks that all properties in the BaseDto match the expected values
    /// from the BaseModel, including nested properties and derived types. Additionally, it serializes the resulting
    /// BaseDto into JSON format and compares it against an expected JSON string to ensure that the serialization
    /// process is also functioning correctly. The test uses FluentAssertions for clear and expressive assertions.
    /// </remarks>
    [Fact]
    public void MapperShouldMapDerivedB()
    {
        const string expectedJson =
            @"{
  ""IntProperty"": 1,
  ""StringProperty"": ""base"",
  ""InnerProperty"": {
    ""GuidProperty"": ""00000000-0000-0000-0000-000000000000"",
    ""StringProperty"": ""inner"",
    ""DateOnlyProperty"": ""0001-01-01"",
    ""DateTimeOffsetProperty"": ""1970-01-01T00:00:00+00:00""
  },
  ""DerivedProperty"": {
    ""$type"": ""derivedB"",
    ""BProperty"": ""B"",
    ""BGuidProperty"": ""00000000-0000-0000-0000-000000000000"",
    ""GuidProperty"": ""00000000-0000-0000-0000-000000000000""
  }
}";
        var derivedBModel = new DerivedBModel
        {
            GuidProperty = Guid.Empty,
            BProperty = "B",
            BGuidProperty = Guid.Empty,
        };
        var baseModel = new BaseModel
        {
            IntProperty = 1,
            StringProperty = "base",
            InnerProperty = new InnerModel
            {
                GuidProperty = Guid.Empty,
                StringProperty = "inner",
                DateOnlyProperty = DateOnly.FromDayNumber(0),
                DateTimeOffsetProperty = DateTimeOffset.UnixEpoch,
            },
            DerivedProperty = derivedBModel,
        };

        // Act
        var dto = _mapper.Map<BaseDto>(baseModel);
        var json = JsonSerializer.Serialize(dto, _options);

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

        json.Should().NotBeNullOrEmpty();
        json.Should().Be(expectedJson);
    }
}
