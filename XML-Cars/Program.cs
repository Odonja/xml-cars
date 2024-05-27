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
    private static readonly string NO_CARS_FILE = @".\XML-Cars\xml-files\no-cars.xml";
    private static readonly string CAR_SCHEMA = @".\XML-Cars\xsd-files\cars.xsd";
    private static readonly string ROOT_ATTRIBUTE = "cars";

    static void Main(string[] args)
    {
        CreateValidCarsXML();
        CreateNoCarsXML();
        XMLToJsonHelper.PrintXMLAsJson<Car>(VALID_CARS_FILE, CAR_SCHEMA, ROOT_ATTRIBUTE);
        XMLToJsonHelper.PrintXMLAsJson<Car>(INVALID_CARS_FILE, CAR_SCHEMA, ROOT_ATTRIBUTE);
        XMLToJsonHelper.PrintXMLAsJson<Car>(NO_CARS_FILE, CAR_SCHEMA, ROOT_ATTRIBUTE);
    }

    private static void CreateValidCarsXML()
    {
        var kiaCar = new Car { Name = "kia", Weight = 700 };
        var toyotaCar = new Car { Name = "toyota", Weight = 1200 };
        var tank = new Car { Name = "tank", Weight = 8000 };
        var cars = new List<Car> { kiaCar, toyotaCar, tank };
        using var writer = new StreamWriter(VALID_CARS_FILE);
        var serializer = new XmlSerializer(cars.GetType(), new XmlRootAttribute(ROOT_ATTRIBUTE));
        serializer.Serialize(writer.BaseStream, cars);
    }

    private static void CreateNoCarsXML()
    {
        var cars = new List<Car>();
        using var writer = new StreamWriter(NO_CARS_FILE);
        var serializer = new XmlSerializer(cars.GetType(), new XmlRootAttribute(ROOT_ATTRIBUTE));
        serializer.Serialize(writer.BaseStream, cars);
    }
}
