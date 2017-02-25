namespace Airport
{
    public class AircraftModelManager
    {
        private readonly ICommandLine commandLine;
        private readonly AircraftModelStore store;

        public AircraftModelManager(ICommandLine commandLine, AircraftModelStore store)
        {
            this.commandLine = commandLine;
            this.store = store;
        }

        public void ListModels()
        {
            commandLine.Write("Available aircraft models:");
            foreach (var aircraftModel in store.Models)
            {
                commandLine.Write(aircraftModel.ToString());
            }
        }

        public void AddModel()
        {
            commandLine.Write("Add new aircraft model.");
            commandLine.Write("Enter Make:");
            string make = commandLine.Read();
            commandLine.Write("Enter Model:");
            string model = commandLine.Read();
            int speed = commandLine.ReadInt("Enter Model speed in km/h. (Average speed is 828 km/h):");
            int numberOfSeats = commandLine.ReadInt("Enter Number of seats:");
            AircraftModel aircraftModel = new AircraftModel(make, model, speed, numberOfSeats);
            store.AddModel(aircraftModel);
            commandLine.Write($"Successfully added model: {aircraftModel}");
        }

        public void EditModel()
        {
            commandLine.Write("Edit aircraft model.");
            int id = commandLine.ReadInt("Enter model id:");
            AircraftModel aircraftModel = store.GetModel(id);

            if (aircraftModel == null)
            {
                commandLine.Write($"Model with id {id} was not found.");
                return;
            }

            commandLine.Write("Enter Make:");
            string make = commandLine.Read();
            commandLine.Write("Enter Model:");
            string model = commandLine.Read();
            int speed = commandLine.ReadInt("Enter Model speed in km/h. (Average speed is 828 km/h):");
            int numberOfSeats = commandLine.ReadInt("Enter Number of seats:", allowEmpty: true);

            if (!string.IsNullOrWhiteSpace(make))
            {
                aircraftModel.Make = make;
            }

            if (!string.IsNullOrWhiteSpace(model))
            {
                aircraftModel.Model = model;
            }

            if (speed != 0)
            {
                aircraftModel.Speed = speed;
            }

            if (numberOfSeats != 0)
            {
                aircraftModel.NumberOfSeats = numberOfSeats;
            }

            store.UpdateModel(aircraftModel);
            commandLine.Write($"Successfully updated model {aircraftModel}.");
        }

        public void DeleteModel()
        {
            commandLine.Write("Delete aircraft model.");
            int id = commandLine.ReadInt("Enter model id:");
            AircraftModel aircraftModel = store.GetModel(id);

            if (aircraftModel == null)
            {
                commandLine.Write($"Model with id {id} was not found.");
            }
            else
            {
                store.DeleteModel(id);
                commandLine.Write($"Successfully deleted model {id}.");
            }
        }

        public AircraftModel GetAircraftModel(int id)
        {
            AircraftModel aircraftModel = store.GetModel(id);

            if (aircraftModel == null)
            {
                commandLine.Write($"Model with id {id} was not found.");
                return null;
            }
            else
            {
                return aircraftModel;
            }
        }

        public int ModelsCount()
        {
            return store.ModelsCount();
        }
    }
}