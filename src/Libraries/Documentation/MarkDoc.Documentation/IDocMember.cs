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

    MemberType Type { get; }
    IDocumentation Documentation { get; }
  }
}
