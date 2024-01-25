using System;

namespace POCAutomapperPolymorphic.Models;

public class DerivedBModel : DerivedBaseModel
{
    public string BProperty { get; set; }

    public Guid BGuidProperty { get; set; }
}
