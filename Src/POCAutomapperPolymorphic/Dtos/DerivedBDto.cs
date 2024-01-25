using System;

namespace POCAutomapperPolymorphic.Dtos;

public class DerivedBDto : DerivedBaseDto
{
    public string BProperty { get; set; }

    public Guid BGuidProperty { get; set; }
}
