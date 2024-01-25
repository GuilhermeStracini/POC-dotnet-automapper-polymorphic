using System.Text.Json.Serialization;

namespace POCAutomapperPolymorphic.Dtos;

[JsonDerivedType(typeof(DerivedBaseDto), "base")]
[JsonDerivedType(typeof(DerivedADto), "derivedA")]
[JsonDerivedType(typeof(DerivedBDto), "derivedB")]
public class DerivedBaseDto
{
    public Guid GuidProperty { get; set; }
}
