#pragma warning disable CS8618

namespace TestLibrary.Members.Properties
{
  public class ClassPropertiesParent
  {
    public string PropertyTop { get; set; }

    public class ClassPropertiesNested
    {
      public string PropertyNested { get; set; }

      public class ClassPropertiesNestedNested
      {
        public string PropertyNestedNested { get; set; }
      }
    }
  }
}
