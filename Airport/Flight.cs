using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport
{
    internal class Flight
    {
        public Flight(
            Airplane plane,
            DateTime arrivalDateTime,
            DateTime departureDateTime,
            DateTime arrivedToAirport,
            string arrivalAirport,
            string departureAirport,
            TimeSpan durationOfFlight,
            int numberOfAvailableTickets,
            int gateArrival,
            int gateDeparture)
        {
            ArrivalDateTime = arrivalDateTime;
            DepartureDateTime = departureDateTime;
            ArrivedToAirport = arrivedToAirport;
            ArrivalAirportCode = arrivalAirport;
            DepartureAirportCode = departureAirport;
            DurationOfFlight = durationOfFlight;
            Plane = plane;
            NumberOfAvailableTickets = numberOfAvailableTickets;
            GateArrival = gateArrival;
            GateDeparture = gateDeparture;
        }

        public int Id { get; set; }

        public Airplane Plane { get; set; }

        public DateTime ArrivalDateTime { get; set; }

        public DateTime DepartureDateTime { get; set; }

        public DateTime ArrivedToAirport { get; set; }

        public string ArrivalAirportCode { get; set; }

        public string DepartureAirportCode { get; set; }

        public TimeSpan DurationOfFlight { get; set; }

        public int NumberOfAvailableTickets { get; set; }

        public int GateArrival { get; set; }

        public int GateDeparture { get; set; }

        public override string ToString()
        {
            return $"FS{Id:0000}. Departure: {DepartureAirportCode} {DepartureDateTime}. Arrival: {ArrivalAirportCode} {ArrivalDateTime}. Duration: {DurationOfFlight}. Tickets available: {NumberOfAvailableTickets}. Gates: departure: {GateDeparture}, arrival: {GateArrival}";
        }
    }
}
