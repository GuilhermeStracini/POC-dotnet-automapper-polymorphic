namespace POCAutomapperPolymorphic.Models;

public class BaseModel
{
    public int IntProperty { get; set; }

    public string StringProperty { get; set; }

    public InnerModel InnerProperty { get; set; }

    public DerivedBaseModel DerivedProperty { get; set; }
}
