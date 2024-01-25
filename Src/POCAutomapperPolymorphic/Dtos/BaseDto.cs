namespace POCAutomapperPolymorphic.Dtos;

public class BaseDto
{
    public int IntProperty { get; set; }

    public string StringProperty { get; set; }

    public InnerDto InnerProperty { get; set; }

    public DerivedBaseDto DerivedProperty { get; set; }
}
