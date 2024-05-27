using System.Text.Json;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml.Linq;

namespace XML_Cars;

public static class XMLToJsonHelper
{
    public static void PrintXMLAsJson<T>(string fileName, string validSchema, string rootAttribute)
    {
        if (IsValidXML(fileName, validSchema))
        {
            PrintXMLFileAsJson<T>(fileName, rootAttribute);
        }
    }

    private static bool IsValidXML(string fileName, string validSchema)
    {
        Console.WriteLine($"testing {fileName}");
        XmlSchemaSet schemas = new XmlSchemaSet();
        schemas.Add(null, validSchema);
        XDocument xml = XDocument.Load(fileName);
        bool errorFree = true;
        string conclusion = "valid";
        xml.Validate(schemas, (o, e) =>
                     {
                         Console.WriteLine("message {0}", e.Message);
                         errorFree = false;
                         conclusion = "not valid";
                     });

        Console.WriteLine($"Conclusion: {fileName} is {conclusion}");
        return errorFree;
    }

    private static void PrintXMLFileAsJson<T>(string fileName, string rootAttribute)
    {
        var serializer = new XmlSerializer(typeof(List<T>), new XmlRootAttribute(rootAttribute));
        var objects = new List<T>();
        using (var reader = new FileStream(fileName, FileMode.Open))
        {
            var deserializedContent = serializer.Deserialize(reader);
            if (deserializedContent != null)
            {
                objects = (List<T>)deserializedContent;
            }
        }
        var options = new JsonSerializerOptions { WriteIndented = true };
        objects.Select(t => JsonSerializer.Serialize(t, options)).ToList().ForEach(Console.WriteLine);
    }
}