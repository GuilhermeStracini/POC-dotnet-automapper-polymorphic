using System.Diagnostics;
using System.Text.Json;
using AutoMapper;
using POCAutomapperPolymorphic.Dtos;
using POCAutomapperPolymorphic.Models;
using POCAutomapperPolymorphic.Profiles;

namespace POCAutomapperPolymorphic;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var baseModelWithA = new BaseModel
        {
            IntProperty = 1,
            StringProperty = "Base",
            InnerProperty = new InnerModel
            {
                GuidProperty = Guid.NewGuid(),
                StringProperty = "Inner",
                DateOnlyProperty = DateOnly.FromDateTime(DateTime.Now),
                DateTimeOffsetProperty = DateTimeOffset.Now,
            },
            DerivedProperty = new DerivedAModel
            {
                GuidProperty = Guid.NewGuid(),
                AProperty = "DerivedA"
            }
        };

        var baseModelWithB = new BaseModel
        {
            IntProperty = 1,
            StringProperty = "Base",
            InnerProperty = new InnerModel
            {
                GuidProperty = Guid.NewGuid(),
                StringProperty = "Inner",
                DateOnlyProperty = DateOnly.FromDateTime(DateTime.Now),
                DateTimeOffsetProperty = DateTimeOffset.Now,
            },
            DerivedProperty = new DerivedBModel
            {
                GuidProperty = Guid.NewGuid(),
                BProperty = "DerivedB",
                BGuidProperty = Guid.NewGuid()
            }
        };

        var config = new MapperConfiguration(cfg => cfg.AddProfile<MapperProfile>());
        config.AssertConfigurationIsValid();
        var mapper = config.CreateMapper();

        var dtoWithA = mapper.Map<BaseDto>(baseModelWithA);
        var dtoWithB = mapper.Map<BaseDto>(baseModelWithB);

        var derivedA = mapper.Map<DerivedADto>(baseModelWithA.DerivedProperty);
        var derivedB = mapper.Map<DerivedBDto>(baseModelWithB.DerivedProperty);

        var jsonWithA = JsonSerializer.Serialize(
            dtoWithA,
            new JsonSerializerOptions { WriteIndented = true }
        );
        var jsonWithB = JsonSerializer.Serialize(
            dtoWithB,
            new JsonSerializerOptions { WriteIndented = true }
        );

        var jsonDerivedA = JsonSerializer.Serialize(
            derivedA,
            new JsonSerializerOptions { WriteIndented = true }
        );

        var jsonDerivedB = JsonSerializer.Serialize(
            derivedB,
            new JsonSerializerOptions { WriteIndented = true }
        );

        Console.WriteLine(jsonWithA);
        Console.WriteLine(jsonWithB);
        Console.WriteLine(jsonDerivedA);
        Console.WriteLine(jsonDerivedB);
    }
}
