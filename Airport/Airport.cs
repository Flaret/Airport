using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport
{
    public class Airport
    {
        public Airport(string code, string name, string city, string country, int freeGates)
        {
            Code = code;
            Name = name;
            City = city;
            Country = country;
            FreeGates = freeGates;
        }

        public string Code { get; set; }

        public string Name { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public int FreeGates { get; set; }

        public int Gate { get; set; }

        public override string ToString()
        {
            return $"Airport {Code} {Name}. City: {City}. Number of free gates: {FreeGates}.";
        }
    }
}
