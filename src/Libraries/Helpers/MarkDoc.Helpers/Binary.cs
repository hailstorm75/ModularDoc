using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MarkDoc.Helpers
{
  public static class Binary
  {
    /// <summary>
    /// Convert an object to a Byte Array.
    /// </summary>
    public static byte[] ObjectToByteArray(object objData)
      => Encoding.UTF8.GetBytes(JsonSerializer.Serialize(objData, GetJsonSerializerOptions()));

    /// <summary>
    /// Convert a byte array to an Object of T.
    /// </summary>
    public static T? ByteArrayToObject<T>(byte[] byteArray)
      => !byteArray.Any()
        ? default
        : JsonSerializer.Deserialize<T>(byteArray, GetJsonSerializerOptions());

    private static JsonSerializerOptions GetJsonSerializerOptions()
      => new()
      {
        PropertyNamingPolicy = null,
        WriteIndented = true,
        AllowTrailingCommas = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
      };
  }
}