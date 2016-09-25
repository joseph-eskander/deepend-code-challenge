using System;
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

        public List<Person> FindPerson(string name, string gender)
        {
            var result = People.Values.Where(p => p.Name.ToLower().Contains(name.ToLower()));

            if (!string.IsNullOrEmpty(gender))
            {
                result = result.Where(p => p.Gender == gender);
            }

            return result.Take(10).ToList();
        }

        public List<Person> FindPersonHierarchy(string name, string gender, bool ancestors)
        {
            var people = People.Values.Where(p => p.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

            if (!string.IsNullOrEmpty(gender))
            {
                people = people.Where(p => p.Gender == gender);
            }

            var person = people.FirstOrDefault();

            var result = new List<Person>();
            if (person != null)
            {
                result.AddRange(ancestors ? GetAncestors(person) : GetDescendants(person));
            }

            return result.Take(10).ToList();
        }

        private IList<Person> GetDescendants(Person person, List<Person> descendants = null)
        {
            if (descendants == null)
                descendants = new List<Person>();

            //quit search if required number of descendants is already reached
            if (descendants.Count >= 10)
                return descendants;

            var directDescendants =
                People.Values.Where(p => p.FatherId == person.Id || p.MotherId == person.Id).ToList();
            descendants.AddRange(directDescendants);
            foreach (var directDescendant in directDescendants)
            {
                GetDescendants(directDescendant, descendants);
            }
            return descendants.OrderBy(p => p.Level).ToList();
        }

        private IList<Person> GetAncestors(Person person, List<Person> ancestors=null )
        {
            if (ancestors == null)
                ancestors = new List<Person>();

            //quit search if required number of ancestors is already reached
            if (ancestors.Count >= 10)
                return ancestors;

            if (person.FatherId.HasValue)
            {
                var father = People[person.FatherId.Value];
                ancestors.Add(father);
                GetAncestors(father, ancestors);
            }
            if (person.MotherId.HasValue)
            {
                var mother = People[person.MotherId.Value];
                ancestors.Add(mother);
                GetAncestors(mother, ancestors);
            }
            return ancestors.OrderBy(p => p.Level).ToList();
        }
    }
}