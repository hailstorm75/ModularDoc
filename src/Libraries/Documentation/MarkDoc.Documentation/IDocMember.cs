namespace MarkDoc.Documentation
{
  public interface IDocMember
  {
    public enum MemberType
    {
      Method = 'M',
      Property = 'P',
      Field = 'F'
    }

    string Name { get; }
    MemberType Type { get; }
    IDocumentation Documentation { get; }
  }
}
