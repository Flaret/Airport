namespace Airport
{
    public class AircraftModel
    {
        public AircraftModel(string make, string model, int speed, int numberOfSeats)
        {
            Make = make;
            Model = model;
            Speed = speed;
            NumberOfSeats = numberOfSeats;
        }

        public int Id { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public int Speed { get; set; }

        public int NumberOfSeats { get; set; }

        public override string ToString()
        {
            return $"{Id:0000}. Aircraft {Make} {Model}. Number of seats: {NumberOfSeats}";
        }
    }
}