using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport
{
    internal class FlightStore
    {
        private Dictionary<int, Flight> flights = new Dictionary<int, Flight>();
        private int numberOfFlights;

        public IEnumerable<Flight> Flights
        {
            get { return flights.Values; }
        }

        public void AddFlight(Flight flight)
        {
            if (flight.Id != 0)
            {
                throw new Exception($"Cannot add flight with non-zero id {flight.Id}.");
            }

            flight.Id = GenerateId();
            flights.Add(flight.Id, flight);
        }

        public void UpdateFlight(Flight flight)
        {
            if (flight.Id == 0)
            {
                throw new Exception($"Flight {flight} does not exist.");
            }

            flights[flight.Id] = flight;
        }

        public void DeleteFlight(int id)
        {
            if (flights.ContainsKey(id))
            {
                flights.Remove(id);
            }
        }

        public Flight GetFlight(int number)
        {
            Flight result;

            if (flights.TryGetValue(number, out result))
            {
                return result;
            }

            return null;
        }

        private int GenerateId()
        {
            numberOfFlights++;
            return numberOfFlights;
        }
    }
}
