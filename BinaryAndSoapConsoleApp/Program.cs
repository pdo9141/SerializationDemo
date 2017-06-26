using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;

namespace BinaryAndSoapConsoleApp
{
    class Program
    {
        public static object SoapFormatter { get; private set; }

        static void Main(string[] args)
        {
            var emp1 = new Employee("Joe", "Pro", 7800M);
            SerializeWithBinaryFormatter(emp1, "Employee.binary");
            var emp2 = (Employee)DeserializeWithBinaryFormatter("Employee.binary");
            Console.WriteLine(emp2.FirstName);
            Console.WriteLine(emp2.LastName);
            Console.WriteLine(emp2.GetSalary());

            SerializeWithSoapFormatter(emp1, "Employee.soap");
            emp2 = (Employee)DeserializeWithSoapFormatter("Employee.soap");
            Console.WriteLine(emp2.FirstName);
            Console.WriteLine(emp2.LastName);
            Console.WriteLine(emp2.GetSalary());

            var loc1 = new Location(26, 80, "Miami");
            SerializeWithBinaryFormatter(loc1, "Location.binary");
            var loc2 = (Location)DeserializeWithBinaryFormatter("Location.binary");
            Console.WriteLine(loc2.x);
            Console.WriteLine(loc2.y);
            Console.WriteLine(loc2.name);

            SerializeWithSoapFormatter(loc1, "Location.soap");
            loc2 = (Location)DeserializeWithSoapFormatter("Location.soap");
            Console.WriteLine(loc2.x);
            Console.WriteLine(loc2.y);
            Console.WriteLine(loc2.name);

            Console.ReadLine();
        }

        private static void SerializeWithBinaryFormatter(object obj, string fileName)
        {
            /* Fast, both sender and receiver must be .NET
             * Not firewall friendly
             * All members (public and private) get serialized unless specified to ignore
             */

            Stream streamOut = File.OpenWrite(fileName);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(streamOut, obj);
            streamOut.Close();
        }

        private static object DeserializeWithBinaryFormatter(string fileName)
        {
            Stream streamIn = File.OpenRead(fileName);
            BinaryFormatter formatter = new BinaryFormatter();
            object obj = formatter.Deserialize(streamIn);
            streamIn.Close();
            return obj;
        }

        private static void SerializeWithSoapFormatter(object obj, string fileName)
        {
            /* Slow because more verbose but firewall friendly
             * Good to interoperability with .NET and Java for example
             * All members (public and private) get serialized unless specified to ignore
             */

            Stream streamOut = File.OpenWrite(fileName);
            SoapFormatter formatter = new SoapFormatter();
            formatter.Serialize(streamOut, obj);
            streamOut.Close();
        }

        private static object DeserializeWithSoapFormatter(string fileName)
        {
            Stream streamIn = File.OpenRead(fileName);
            SoapFormatter formatter = new SoapFormatter();
            object obj = formatter.Deserialize(streamIn);
            streamIn.Close();
            return obj;
        }
    }

    [Serializable]
    public class Employee
    {
        public Employee(string firstName, string lastName, decimal salary)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this._salary = salary;
        }

        public string FirstName;
        public string LastName;

        [NonSerialized]
        private decimal _salary;

        public decimal GetSalary() { return _salary; }
    }

    [Serializable]
    public class Location : ISerializable
    {
        public int x;
        public int y;
        public string name;

        public Location(int x, int y, string name)
        {
            this.x = x;
            this.y = y;
            this.name = name;
        }

        protected Location(SerializationInfo info, StreamingContext context)
        {
            x = info.GetInt32("i");
            y = info.GetInt32("j");
            name = info.GetString("k");
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("i", x);
            info.AddValue("j", y);
            info.AddValue("k", name);
        }
    }
}
