using System.IO;
using System.Linq;
using System.Web.Mvc;
using Data;
using DeependAncestry.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DeependAncestry.Controllers
{
    public class SearchController : Controller
    {
        private const string _jsonFile = @"data_small.json";

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
                var data = new DataRepository(Server.MapPath($@"~\app_data\{_jsonFile}"));

                var result =
                    data.People.Values.Where(p => p.Name.ToLower().Contains(searchViewModel.Name.ToLower()))
                        .Select(p => new PersonSearchResult()
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Gender = p.Gender == "M" ? "Male" : "Female",
                            BirthPlace = data.Places[p.PlaceId].Name
                        });

                    if (searchViewModel.Male != searchViewModel.Female)
                    {
                        result = searchViewModel.Male ? result.Where(p => p.Gender == "Male") : result.Where(p => p.Gender == "Female");
                    }

                    searchViewModel.Result = result.Take(10).ToList();
                    return View(searchViewModel);
            }

            return View(searchViewModel);
        }

        public ActionResult Advanced()
        {
            throw new System.NotImplementedException();
        }
    }
}