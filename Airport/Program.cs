using System;
using System.Collections.Generic;

namespace Airport
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new Program().Run();
        }

        private readonly ICommandLine commandLine;
        private readonly CommandLineInterface cli;

        private readonly AircraftModelStore modelStore;
        private readonly AircraftModelManager modelManager;

        private readonly AirplaneStore airplaneStore;
        private readonly AirplaneManager airplaneManager;

        private readonly FlightStore flightStore;
        private readonly FlightManager flightManager;

        private readonly AirportStore airportStore;
        private readonly AirportManager airportManager;

        public Program()
        {
            commandLine = new ConsoleWrapper();
            cli = new CommandLineInterface(commandLine);
            modelStore = new AircraftModelStore();
            modelManager = new AircraftModelManager(commandLine, modelStore);
            airplaneStore = new AirplaneStore();
            airplaneManager = new AirplaneManager(commandLine, airplaneStore, modelManager);
            airportStore = new AirportStore();
            airportManager = new AirportManager(commandLine, airportStore, airplaneManager);
            flightStore = new FlightStore();
            flightManager = new FlightManager(commandLine, flightStore, airplaneManager, modelManager, airportManager);
        }

        public void Run()
        {
            RegisterCommands();
            cli.DisplayApplicationName();
            while (true)
            {
                commandLine.Write("Available sections: model, airplane, airport, flight, exit");
                commandLine.Write("What do you want to do next:");
                string help = commandLine.Read();

                if (help.ToLower() == "exit")
                {
                    Exit();
                }
                else
                {
                    if (cli.ReturnCommand(help.ToLower()))
                    {
                        cli.HandleCommand(help.ToLower());
                    }
                    else
                    {
                        cli.DisplayHelp(help);
                        cli.HandleCommand();
                    }
                }
            }
        }

        private void RegisterCommands()
        {
            cli.AddCommand("exit", Exit);

            cli.AddCommand("listmodels", modelManager.ListModels);
            cli.AddCommand("addmodel", modelManager.AddModel);
            cli.AddCommand("editmodel", modelManager.EditModel);
            cli.AddCommand("deletemodel", modelManager.DeleteModel);

            cli.AddCommand("addairplane", airplaneManager.AddAirplane);
            cli.AddCommand("listairplanes", airplaneManager.ListAirplanes);
            cli.AddCommand("editairplane", airplaneManager.EditAirplane);
            cli.AddCommand("deleteairplane", airplaneManager.DeleteAirplane);

            cli.AddCommand("listflights", flightManager.ListFlights);
            cli.AddCommand("addflight", flightManager.AddFlight);
            cli.AddCommand("editflight", flightManager.EditFlight);
            cli.AddCommand("deleteflight", flightManager.DeleteFlight);

            cli.AddCommand("listairports", airportManager.ListAirports);
            cli.AddCommand("addairport", airportManager.AddAirport);
            cli.AddCommand("editairport", airportManager.EditAirport);
            cli.AddCommand("deleteairport", airportManager.DeleteAirport);
            //cli.AddCommand("addairplanetoairport", airportManager.AddAirplaneToAirport);
            //cli.AddCommand("deleteairplanefromairport", airportManager.DeleteAirplaneFromAirport);
        }

        private void Exit()
        {
            cli.SayGoodBye();
            Environment.Exit(0);
        }
    }
}