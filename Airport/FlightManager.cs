using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport
{
    internal class FlightManager
    {
        private readonly ICommandLine commandLine;
        private readonly FlightStore flightStore;
        private readonly AirplaneManager airplaneManager;
        private readonly AircraftModelManager aircraftModelManager;
        private readonly AirportManager airportManager;

        public FlightManager(
            ICommandLine commandLine,
            FlightStore flightStore,
            AirplaneManager airplaneManager,
            AircraftModelManager aircraftModelManager,
            AirportManager airportManager)
        {
            this.commandLine = commandLine;
            this.flightStore = flightStore;
            this.airplaneManager = airplaneManager;
            this.aircraftModelManager = aircraftModelManager;
            this.airportManager = airportManager;
        }

        public void ListFlights()
        {
            commandLine.Write("Available flights:");

            foreach (var flight in flightStore.Flights)
            {
                commandLine.Write(flight.ToString());
            }
        }

        public void AddFlight()
        {
            commandLine.Write("Add new flight.");

            if (airplaneManager.AirplanesCount().Equals(0))
            {
                commandLine.Write("Add at least one airplane.");
                return;
            }

            if (airportManager.AirportsCount() < 2)
            {
                commandLine.Write("Add at least two airports.");
                return;
            }

            commandLine.Write("Enter departure airport code:");
            string departureAirportCode = ReadDepartureAirportCode();
            commandLine.Write("Enter departure date:");
            DateTime departureDateTime = ReadDate();

            commandLine.Write("Enter destination airport code:");
            string arrivalAirportCode = ReadArrivalAirportCode(departureAirportCode);
            commandLine.Write("Enter departure gate.");
            int gateDeparture = GetGate(departureAirportCode);

            if (gateDeparture == 0)
            {
                commandLine.Write($"Gates in airport with code {departureAirportCode} are not available.");
                return;
            }

            airportManager.AddOrRemoveOneGate("add", departureAirportCode);

            commandLine.Write("Add airplane:");
            Airplane airplane = airplaneManager.GetAirplane();

            while (airplane == null)
            {
                commandLine.Write($"Airplane doesn't exist. Please enter existing one.");
                airplane = airplaneManager.GetAirplane();
            }

            TimeSpan durationOfFlight = CalculateFlightDuration(departureAirportCode, arrivalAirportCode, airplane.Model.Speed);
            DateTime arrivalDateTime = CalculateArrivalDate(departureDateTime, durationOfFlight);
            commandLine.Write("Enter date when flight arrived to the airport:");
            DateTime flightArrivedToAirport = ReadDate();

            if (departureDateTime - flightArrivedToAirport < airplane.MaintenanceTime)
            {
                commandLine.Write("Flight is not available. Plane is not ready yet. Maintenance time 2 hours.");
                return;
            }

            commandLine.Write("Enter arrival gate.");
            int gateArrival = GetGate(arrivalAirportCode);

            if (gateArrival == 0)
            {
                commandLine.Write($"Gates in airport with code {arrivalAirportCode} are not available.");
                return;
            }

            airportManager.AddOrRemoveOneGate("delete", arrivalAirportCode);

            int numberOfAvailableTickets = airplane.Model.NumberOfSeats;
            airplane.AirportCode = arrivalAirportCode;
            Flight flight = new Flight(
                airplane,
                arrivalDateTime,
                departureDateTime,
                flightArrivedToAirport,
                arrivalAirportCode,
                departureAirportCode,
                durationOfFlight,
                numberOfAvailableTickets,
                gateArrival,
                gateDeparture);
            flightStore.AddFlight(flight);
            commandLine.Write($"Successfully added flight {flight}");
        }

        public void EditFlight()
        {
            commandLine.Write("Edit flight.");
            int id = commandLine.ReadInt("Enter flight number:");
            Flight flight = flightStore.GetFlight(id);

            if (flight.Id == 0)
            {
                commandLine.Write($"Flight with number {flight.Id} was not found.");
                return;
            }

            commandLine.Write("Enter departure airport code:");
            flight.DepartureAirportCode = ReadDepartureAirportCode();
            commandLine.Write("Enter departure date:");
            flight.DepartureDateTime = ReadDate();
            commandLine.Write("Enter destination airport:");
            flight.ArrivalAirportCode = ReadArrivalAirportCode(flight.DepartureAirportCode);
            commandLine.Write("Enter departure gate.");
            flight.GateDeparture = GetGate(flight.DepartureAirportCode);

            if (flight.GateDeparture == 0)
            {
                commandLine.Write($"Gates in airport with code {flight.DepartureAirportCode} are not available.");
                return;
            }

            airportManager.AddOrRemoveOneGate("add", flight.DepartureAirportCode);
            commandLine.Write("Add airplane:");

            do
            {
                flight.Plane = airplaneManager.GetAirplane();
                if (flight.Plane == null)
                {
                    commandLine.Write("Airplane doesn't exist. Add another airplane:");
                }
            }
            while (flight.Plane == null);

            commandLine.Write("Enter date when flight arrived to the airport:");
            flight.ArrivedToAirport = ReadDate();

            if (flight.DepartureDateTime - flight.ArrivedToAirport < flight.Plane.MaintenanceTime)
            {
                commandLine.Write("Flight is not available. Plane is not ready yet.");
                return;
            }

            TimeSpan durationOfFlight = CalculateFlightDuration(flight.DepartureAirportCode, flight.ArrivalAirportCode, flight.Plane.Model.Speed);
            flight.ArrivalDateTime = CalculateArrivalDate(flight.DepartureDateTime, durationOfFlight);
            commandLine.Write("Enter arrival gate.");
            flight.GateArrival = GetGate(flight.ArrivalAirportCode);

            if (flight.GateArrival == 0)
            {
                commandLine.Write($"Gates in airport with code {flight.ArrivalAirportCode} are not available.");
                return;
            }

            airportManager.AddOrRemoveOneGate("delete", flight.ArrivalAirportCode);
            flight.NumberOfAvailableTickets = flight.Plane.Model.NumberOfSeats;
            flight.Plane.AirportCode = flight.ArrivalAirportCode;

            flightStore.UpdateFlight(flight);
            commandLine.Write($"Successfully updated flight {flight}");
        }

        public void DeleteFlight()
        {
            commandLine.Write("Delete flight.");
            commandLine.Write("Enter flgith number:");

            int number = commandLine.ReadInt("Enter model id:");
            Flight flight = flightStore.GetFlight(number);

            if (flight == null)
            {
                commandLine.Write($"Flight with number {number} was not found.");
                return;
            }
            else
            {
                flightStore.DeleteFlight(number);
                commandLine.Write($"Successfully deleted flight {number}.");
            }
        }

        private DateTime ReadDate()
        {
            DateTime dateValue;
            string dateString;

            do
            {
                commandLine.Write("Format: DD/MM/YYYY HH:MM:");
                dateString = commandLine.Read();
            }
            while (!DateTime.TryParseExact(dateString, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue));

            return dateValue;
        }

        private TimeSpan CalculateFlightDuration(string departureAirportCode, string arrivalAirportCode, int speed)
        {
            TravelTimeCalculator ttc = new TravelTimeCalculator(commandLine);
            string departureAirportAddress = airportManager.GetAirportAddress(departureAirportCode);
            string arrivalAirportAddress = airportManager.GetAirportAddress(arrivalAirportCode);

            return ttc.GetTravelTime(departureAirportAddress, arrivalAirportAddress, speed);
        }

        private DateTime CalculateArrivalDate(DateTime departureDateTime, TimeSpan durationOfFlight)
        {
            return departureDateTime + durationOfFlight;
        }

        private int GetGate(string code)
        {
            if (airportManager.GetNumberOfFreeGates(code) <= 0)
            {
                return 0;
            }

            return commandLine.ReadInt("Enter gate number:");
        }

        private string ReadAirportCode()
        {
            string airportCode = airportManager.GetAirportCode();

            while (airportCode == null)
            {
                commandLine.Write($"Please enter code of existing airport.");
                airportCode = airportManager.GetAirportCode();
            }

            return airportCode;
        }

        private string ReadDepartureAirportCode()
        {
            string departureAirportCode = ReadAirportCode();

            while (!airplaneManager.GetAirportCodeOfAirplane(departureAirportCode))
            {
                commandLine.Write($"Airport with code {departureAirportCode} doesn't contain airplanes ready to departure.");
                commandLine.Write("Enter another departure airport code:");
                departureAirportCode = ReadAirportCode();
            }

            return departureAirportCode;
        }

        private string ReadArrivalAirportCode(string departureAirportCode)
        {
            string arrivalAirportCode = ReadAirportCode();

            while (arrivalAirportCode.Equals(departureAirportCode, StringComparison.OrdinalIgnoreCase))
            {
                commandLine.Write("Airport codes of departure and arrival cannot be equal.");
                arrivalAirportCode = ReadAirportCode();
            }

            return arrivalAirportCode;
        }
    }
}