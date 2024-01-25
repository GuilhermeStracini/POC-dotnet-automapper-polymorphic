namespace POCAutomapperPolymorphic.Models;

public class InnerModel
{
    public Guid GuidProperty { get; set; }

    public string StringProperty { get; set; }

    public DateOnly DateOnlyProperty { get; set; }

    public DateTimeOffset DateTimeOffsetProperty { get; set; }
}
