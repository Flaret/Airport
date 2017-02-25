using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Airport
{
    public class AirportStore
    {
        private Dictionary<string, Airport> airports = new Dictionary<string, Airport>();

        public IEnumerable<Airport> Airports
        {
            get { return airports.Values; }
        }

        public void AddAirport(Airport airport)
        {
            if (airport.Code == null)
            {
                throw new Exception($"Cannot add airport with empty code {airport.Code}.");
            }

            airports.Add(airport.Code, airport);
        }

        public void UpdateAirport(Airport airport)
        {
            if (airport.Code == null)
            {
                throw new Exception($"Airport {airport} doesn't exist.");
            }

            airports[airport.Code] = airport;
        }

        public void DeleteAirport(string code)
        {
            if (airports.ContainsKey(code))
            {
                airports.Remove(code);
            }
        }

        public Airport GetCode(string code)
        {
            Airport result;

            if (airports.TryGetValue(code, out result))
            {
                return result;
            }

            return null;
        }

        public string GetAddress(string code)
        {
            Airport result;

            if (airports.TryGetValue(code, out result))
            {
                return result.Name + " " + result.City + ", " + result.Country;
            }

            return null;
        }

        public int AirportsCount()
        {
            return airports.Count;
        }
    }
}
