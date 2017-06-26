using System;
using System.IO;
using System.Xml.Serialization;

namespace XmlConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var aircraft1 = new Aircraft();
            aircraft1.aircraftType = "cargo";
            aircraft1.model = "Super Guppy";
            aircraft1.serial = 1972;
            aircraft1.miles = 2315000;
            aircraft1.condition = Aircraft.ConditionType.HighCondition;

            var xmlSerializer = new XmlSerializer(aircraft1.GetType());
            var steamOut = File.OpenWrite("Aircraft.xml");
            xmlSerializer.Serialize(steamOut, aircraft1);
            steamOut.Close();

            var streamIn = File.OpenRead("Aircraft.xml");
            var aircraft2 = (Aircraft)xmlSerializer.Deserialize(streamIn);
            Console.WriteLine(aircraft2.aircraftType);
            Console.WriteLine(aircraft2.model);
            Console.WriteLine(aircraft2.serial);
            Console.WriteLine(aircraft2.miles);
            Console.WriteLine(aircraft2.condition);

            Console.ReadLine();
        }
    }

    public class Aircraft
    {
        [XmlAttribute(AttributeName = "model")]
        public string aircraftType;
        public string model;
        [XmlIgnore]
        public int serial;
        [XmlElement(ElementName = "mileage")]
        public int miles;
        public ConditionType condition;

        public enum ConditionType
        {
            [XmlEnum("Rotten")]
            LowCondition,
            [XmlEnum("OK")]
            MediumCondition,
            [XmlEnum("Perfect")]
            HighCondition
        }
    }
}
