// ReSharper disable All
namespace TestLibrary.Members.Properties
{
  public class ClassProperties
  {
    public static string PropertyStatic { get; set; } = null!;

    public string PropertyGetSet { get; set; } = null!;
    public string PropertyGet { get; } = null!;
    public string PropertySet { private get; set; } = null!;

    public string PropertyPublic { get; set; } = null!;
    protected string PropertyProtected { get; set; } = null!;
    internal string PropertyInternal { get; set; } = null!;
    private string PropertyPrivate { get; set; } = null!;

    public string PropertyPublicSetProtected { get; protected set; } = null!;
    public string PropertyPublicSetInternal { get; internal set; } = null!;
    public string PropertyPublicSetPrivate { get; private set; } = null!;

    public string PropertyPublicGetProtected { protected get; set; } = null!;
    public string PropertyPublicGetInternal { internal get; set; } = null!;
    public string PropertyPublicGetPrivate { private get; set; } = null!;

    protected string PropertyProtectedSetPrivate { get; private set; } = null!;
    protected string PropertyProtectedGetPrivate { private get; set; } = null!;

    internal string PropertyInternalSetPrivate { get; private set; } = null!;
    internal string PropertyInternalGetPrivate { private get; set; } = null!;
  }
}