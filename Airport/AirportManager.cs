using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport
{
    public class AirportManager
    {
        private readonly ICommandLine commandLine;
        private readonly AirportStore airportStore;
        private readonly AirplaneManager airplaneManager;

        private Dictionary<string, Dictionary<int, Airplane>> airplanes = new Dictionary<string, Dictionary<int, Airplane>>();

        public AirportManager(ICommandLine commandLine, AirportStore airportStore, AirplaneManager airplaneManager)
        {
            this.commandLine = commandLine;
            this.airportStore = airportStore;
            this.airplaneManager = airplaneManager;
        }

        public void ListAirports()
        {
            commandLine.Write("List of Airports:");
            foreach (var airport in airportStore.Airports)
            {
                commandLine.Write(airport.ToString());
            }
        }

        public void AddAirport()
        {
            commandLine.Write("Add new Airport.");
            commandLine.Write("Enter City:");
            string city = commandLine.Read();
            while (AnalyzeString(city) == null)
            {
                city = commandLine.Read();
            }

            commandLine.Write("Enter Country:");
            string country = commandLine.Read();
            while (AnalyzeString(country) == null)
            {
                country = commandLine.Read();
            }

            commandLine.Write("Enter Name:");
            string name = ReadAirportName();
            while (AnalyzeString(name) == null)
            {
                name = commandLine.Read();
            }

            commandLine.Write("Enter Code:");
            string code = ReadAirportCode();
            while (AnalyzeString(code) == null)
            {
                code = commandLine.Read();
            }

            int freeGates = commandLine.ReadInt("Enter number of free gates:");

            Airport airport = new Airport(code, name, city, country, freeGates);
            airportStore.AddAirport(airport);
            commandLine.Write($"Successfully added airport: {airport}");
        }

        public void EditAirport()
        {
            commandLine.Write("Edit Airport.");
            commandLine.Write("Enter Airport Code:");
            string code = commandLine.Read();
            Airport airport = airportStore.GetCode(code);

            if (airport == null)
            {
                commandLine.Write($"Airport with code {code} was not found.");
                return;
            }

            commandLine.Write("Enter City:");
            string city = commandLine.Read();

            commandLine.Write("Enter Country:");
            string country = commandLine.Read();

            commandLine.Write("Enter Name:");
            string name = ReadAirportName();

            int totalGates = commandLine.ReadInt("Enter Number of gates:", allowEmpty: true);

            if (!string.IsNullOrWhiteSpace(city))
            {
                airport.City = city;
            }

            if (!string.IsNullOrWhiteSpace(country))
            {
                airport.Country = country;
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                airport.Name = name;
            }

            if (totalGates != 0)
            {
                airport.FreeGates = totalGates;
            }

            airportStore.UpdateAirport(airport);
            commandLine.Write($"Successfully updated airport {airport}");
        }

        public void DeleteAirport()
        {
            commandLine.Write("Delete Airport.");
            commandLine.Write("Enter Airport Code:");
            string code = commandLine.Read();
            Airport airport = airportStore.GetCode(code);

            if (airport == null)
            {
                commandLine.Write($"Airport with code {code} was not found.");
            }
            else
            {
                airportStore.DeleteAirport(code);
                commandLine.Write($"Successfully deleted airpot {code}.");
            }
        }

        public string GetAirportCode()
        {
            string code = commandLine.Read().ToUpper();
            Airport airport = airportStore.GetCode(code);

            if (airport == null)
            {
                commandLine.Write($"Airport with code {code} was not found.");
                return null;
            }

            return code;
        }

        public string GetAirportAddress(string code)
        {
            string address = airportStore.GetAddress(code);

            if (address == null)
            {
                commandLine.Write($"Airport with code {code} at address {address} was not found.");
                return null;
            }

            return address;
        }

        public int GetNumberOfFreeGates(string code)
        {
            Airport airport = airportStore.GetCode(code);
            return airport.FreeGates;
        }

        public void AddOrRemoveOneGate(string value, string code)
        {
            Airport airport = airportStore.GetCode(code);

            if (value == "add")
            {
                airport.FreeGates += 1;
            }

            if (value == "delete")
            {
                airport.FreeGates -= 1;
            }
        }

        public int AirportsCount()
        {
            return airportStore.AirportsCount();
        }

        private string ReadAirportName()
        {
            string name = commandLine.Read();

            foreach (var item in airportStore.Airports)
            {
                while (name.Equals(item.Name, StringComparison.OrdinalIgnoreCase))
                {
                    commandLine.Write("Airport already exists. Add another one.");
                    name = commandLine.Read();
                }
            }

            return name;
        }

        private string ReadAirportCode()
        {
            string code = commandLine.Read();

            foreach (var item in airportStore.Airports)
            {
                while (code.Equals(item.Code, StringComparison.OrdinalIgnoreCase))
                {
                    commandLine.Write("Airport code already exists. Enter another one.");
                    code = commandLine.Read();
                }
            }

            return code;
        }

        private string AnalyzeString(string text)
        {
            string temp = text.Replace(" ", string.Empty);
            temp = temp.Replace("-", string.Empty);

            if (!temp.All(char.IsLetter))
            {
                commandLine.Write($"Only letters, spaces and dashes are accepted.");
                return null;
            }

            return text;
        }
    }
}