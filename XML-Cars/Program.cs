using System.Text.Json;
using System.Xml.Serialization;
using System.Xml.Schema;
using DataTypesApp;
using System.Xml.Linq;

namespace XML_Cars;

class Program
{
    private static readonly string INVALID_CARS_FILE = @".\XML-Cars\xml-files\invalid-cars.xml";
    private static readonly string VALID_CARS_FILE = @".\XML-Cars\xml-files\valid-cars.xml";
    private static readonly string CAR_SCHEMA = @".\XML-Cars\xsd-files\cars.xsd";

    static void Main(string[] args)
    {
        createValidCarsXML();

        if (isValidXML(VALID_CARS_FILE))
        {
            printXMLFileAsJson(VALID_CARS_FILE);
        }

        if (isValidXML(INVALID_CARS_FILE))
        {
            printXMLFileAsJson(INVALID_CARS_FILE);
        }
    }

    private static bool isValidXML(string fileName)
    {
        Console.WriteLine($"testing {fileName}");
        XmlSchemaSet schemas = new XmlSchemaSet();
        schemas.Add(null, CAR_SCHEMA);
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

    private static void printXMLFileAsJson(string fileName)
    {
        var serializer = new XmlSerializer(typeof(List<Car>), new XmlRootAttribute("cars"));
        var cars = new List<Car>();
        using (var reader = new FileStream(fileName, FileMode.Open))
        {
            var deserializedContent = serializer.Deserialize(reader);
            if (deserializedContent != null)
            {
                cars = (List<Car>)deserializedContent;
            }
        }
        var options = new JsonSerializerOptions { WriteIndented = true };
        cars?.Select(car => JsonSerializer.Serialize(car, options)).ToList().ForEach(Console.WriteLine);
    }

    private static void createValidCarsXML()
    {
        var kiaCar = new Car { Name = "kia", Weight = 700 };
        var toyotaCar = new Car { Name = "toyota", Weight = 1200 };
        var tank = new Car { Name = "tank", Weight = 8000 };
        var cars = new List<Car> { kiaCar, toyotaCar, tank };
        using var writer = new StreamWriter(VALID_CARS_FILE);
        var serializer = new XmlSerializer(cars.GetType(), new XmlRootAttribute("cars"));
        serializer.Serialize(writer.BaseStream, cars);
    }
}
