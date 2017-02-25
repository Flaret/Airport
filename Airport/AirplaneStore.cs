using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport
{
    public class AirplaneStore
    {
        private Dictionary<string, Airplane> airplanes = new Dictionary<string, Airplane>();
        private int numberOfAirplanes;

        public IEnumerable<Airplane> Airplanes
        {
            get { return airplanes.Values; }
        }

        public void AddAirplane(Airplane airplane)
        {
            if (airplane.Number != null)
            {
                throw new Exception($"Cannot add airplane with non-empty number {airplane.Number}.");
            }

            if (airplane.Model.Id == 0)
            {
                throw new Exception($"Cannot add model with zero id {airplane.Model.Id}.");
            }

            airplane.Number = GenerateNumber();
            airplanes.Add(airplane.Number, airplane);
        }

        public void UpdateAirplane(Airplane airplane)
        {
            if (airplane.Number == null)
            {
                throw new Exception($"Airplane {airplane} does not exist.");
            }

            airplanes[airplane.Number] = airplane;
        }

        public void DeleteAirplane(string number)
        {
            if (airplanes.ContainsKey(number))
            {
                airplanes.Remove(number);
            }
        }

        public Airplane GetNumber(string number)
        {
            Airplane result;

            if (airplanes.TryGetValue(number, out result))
            {
                return result;
            }

            return null;
        }

        public int AirplanesCount()
        {
            return airplanes.Count;
        }

        private string GenerateNumber()
        {
            numberOfAirplanes++;
            return "UA" + numberOfAirplanes.ToString();
        }
    }
}
