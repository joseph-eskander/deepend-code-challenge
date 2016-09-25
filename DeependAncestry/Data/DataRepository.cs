using System.Collections.Generic;
using System.IO;
using System.Linq;
using Data.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Data
{
    public class DataRepository
    {
        public Dictionary<int, Place> Places { get; set; }
        public Dictionary<int, Person> People { get; set; }

        public DataRepository(string fileName)
        {
            using (StreamReader reader = File.OpenText(fileName))
            {
                JObject data = (JObject) JToken.ReadFrom(new JsonTextReader(reader));

                Places = data["places"].Select(p => new Place()
                {
                    Id = (int) p["id"],
                    Name = (string) p["name"]
                }).ToDictionary(p => p.Id);

                People = data["people"].Select(p => new Person()
                {
                    Id = (int) p["id"],
                    Name = (string) p["name"],
                    Gender = (string) p["gender"],
                    FatherId = (int?) p["father_id"],
                    MotherId = (int?) p["mother_id"],
                    PlaceId = (int) p["place_id"],
                    Level = (int) p["level"]
                }).ToDictionary(p => p.Id);
            }
        }
    }
}