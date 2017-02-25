using System;

namespace Airport
{
    public class Airplane
    {
        public Airplane(AircraftModel model)
        {
            Model = model;
        }

        public string Number { get; set; }

        public AircraftModel Model { get; set; }

        public TimeSpan MaintenanceTime { get; set; }

        public string AirportCode { get; set; }

        public override string ToString()
        {
            return $"{Number}. Aircraft: {Model.Make} {Model.Model}. Number of seats: {Model.NumberOfSeats}. Airport: {AirportCode}";
        }
    }
}