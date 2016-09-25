using System.IO;
using System.Linq;
using System.Web.Mvc;
using DeependAncestry.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DeependAncestry.Controllers
{
    public class SearchController : Controller
    {
        private const string _jsonFile = @"data_large.json";

        // GET: Search
        public ActionResult Index()
        {
            return View(new SearchViewModel());
        }

        [HttpPost]
        public ActionResult Index(SearchViewModel searchViewModel)
        {
            if (ModelState.IsValid)
            {
                using (StreamReader reader = System.IO.File.OpenText(Server.MapPath($@"~\app_data\{_jsonFile}")))
                {
                    JObject data = (JObject) JToken.ReadFrom(new JsonTextReader(reader));

                    var allPlaces = data["places"].Select(p => new
                    {
                        Id = (int) p["id"],
                        Name = (string) p["name"]
                    }).ToDictionary(p => p.Id);

                    var result = data["people"].Select(p => new Person()
                        {
                            Id = (int) p["id"],
                            Name = (string) p["name"],
                            Gender = (string) p["gender"] == "M" ? "Male" : "Female",
                            BirthPlace = allPlaces[(int) p["place_id"]].Name
                        })
                        .Where(p => p.Name.ToLower().Contains(searchViewModel.Name.ToLower()));

                    if (searchViewModel.Male != searchViewModel.Female)
                    {
                        result = searchViewModel.Male ? result.Where(p => p.Gender == "Male") : result.Where(p => p.Gender == "Female");
                    }

                    searchViewModel.Result = result.Take(10).ToList();
                    return View(searchViewModel);
                }
            }

            return View(searchViewModel);
        }

        public ActionResult Advanced()
        {
            throw new System.NotImplementedException();
        }
    }
}