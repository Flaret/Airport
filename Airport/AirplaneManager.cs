using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport
{
    public class AirplaneManager
    {
        private readonly ICommandLine commandLine;
        private readonly AirplaneStore airplaneStore;
        private readonly AircraftModelManager aircraftModelManager;

        public AirplaneManager(ICommandLine commandLine, AirplaneStore airplaneStore, AircraftModelManager aircraftModelManager)
        {
            this.commandLine = commandLine;
            this.airplaneStore = airplaneStore;
            this.aircraftModelManager = aircraftModelManager;
        }

        public void ListAirplanes()
        {
            commandLine.Write("Availible airplanes:");

            foreach (var airplane in airplaneStore.Airplanes)
            {
                commandLine.Write(airplane.ToString());
            }
        }

        public void AddAirplane()
        {
            commandLine.Write("Add new airplane.");

            if (aircraftModelManager.ModelsCount().Equals(0))
            {
                commandLine.Write("Add at least one aircraft model.");
                return;
            }

            Airplane airplane = new Airplane(GetAircraftModel());
            airplane.MaintenanceTime = TimeSpan.FromMinutes(120);
            commandLine.Write("Enter airport of origin.");
            airplane.AirportCode = commandLine.Read().ToUpper();
            airplaneStore.AddAirplane(airplane);
            commandLine.Write($"Successfully added airplane: {airplane}");
        }

        public void EditAirplane()
        {
            commandLine.Write("Edit airplane.");
            commandLine.Write("Enter number in format LL00:");
            string number = commandLine.Read().ToUpper();
            Airplane airplane = airplaneStore.GetNumber(number);

            if (airplane == null)
            {
                commandLine.Write($"Airplane with number {number} was not found.");
                return;
            }

            AircraftModel aircraftModel = GetAircraftModel();

            if (!string.IsNullOrWhiteSpace(aircraftModel.ToString()))
            {
                airplane.Model = aircraftModel;
            }

            commandLine.Write("Enter airport of origin.");
            airplane.AirportCode = commandLine.Read().ToUpper();
            airplaneStore.UpdateAirplane(airplane);
            commandLine.Write($"Successfully updated airplane {airplane}.");
        }

        public void DeleteAirplane()
        {
            commandLine.Write("Delete airplane.");
            commandLine.Write("Enter number in format LL00:");
            string number = commandLine.Read().ToUpper();
            Airplane airplane = airplaneStore.GetNumber(number);

            if (airplane == null)
            {
                commandLine.Write($"Airplane with number {number} was not found.");
                return;
            }
            else
            {
                airplaneStore.DeleteAirplane(number);
                commandLine.Write($"Successfully deleted airplane {number}.");
            }
        }

        public Airplane GetAirplane()
        {
            commandLine.Write("Enter airplane number in format LL00:");
            string number = commandLine.Read().ToUpper();
            Airplane airplane = airplaneStore.GetNumber(number);

            if (airplane == null)
            {
                commandLine.Write($"Airplane with number {number} was not found.");
                return null;
            }

            return airplane;
        }

        public bool GetAirportCodeOfAirplane(string airportCode)
        {
            foreach (var airplane in airplaneStore.Airplanes)
            {
                if (airportCode.Equals(airplane.AirportCode, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        public int AirplanesCount()
        {
            return airplaneStore.AirplanesCount();
        }

        private AircraftModel GetAircraftModel()
        {
            int id;
            AircraftModel aircraftModel = null;

            while (aircraftModel == null)
            {
                id = commandLine.ReadInt("Enter aircraft model id:");
                aircraftModel = aircraftModelManager.GetAircraftModel(id);
            }

            return aircraftModel;
        }
    }
}
