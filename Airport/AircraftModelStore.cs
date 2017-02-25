using System;
using System.Collections.Generic;
using System.Net.Configuration;

namespace Airport
{
    public class AircraftModelStore
    {
        private Dictionary<int, AircraftModel> models = new Dictionary<int, AircraftModel>();
        private int numberOfModels;

        public IEnumerable<AircraftModel> Models
        {
            get { return models.Values; }
        }

        public void AddModel(AircraftModel model)
        {
            if (model.Id != 0)
            {
                throw new Exception($"Cannot add model with non-zero id {model.Id}.");
            }

            model.Id = GenerateId();
            models.Add(model.Id, model);
        }

        public void UpdateModel(AircraftModel model)
        {
            if (model.Id == 0)
            {
                throw new Exception($"Model {model} does not exist.");
            }

            models[model.Id] = model;
        }

        public void DeleteModel(int id)
        {
            if (models.ContainsKey(id))
            {
                models.Remove(id);
            }
        }

        public AircraftModel GetModel(int id)
        {
            AircraftModel result;

            if (models.TryGetValue(id, out result))
            {
                return result;
            }

            return null;
        }

        public int ModelsCount()
        {
            return models.Count;
        }

        private int GenerateId()
        {
            numberOfModels++;
            return numberOfModels;
        }
    }
}