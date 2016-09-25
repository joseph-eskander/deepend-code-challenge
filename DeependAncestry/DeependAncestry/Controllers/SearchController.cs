using System.Configuration;
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
                var data = new DataRepository(Server.MapPath($@"~\app_data\{ConfigurationManager.AppSettings["dataFilename"]}"));

                string genderSearch = null;
                if (searchViewModel.Male != searchViewModel.Female)
                {
                    genderSearch = searchViewModel.Male ? "M" : "F";
                }

                searchViewModel.Result = data.FindPerson(searchViewModel.Name, genderSearch)
                    .Select(p => new PersonSearchResult()
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Gender = p.Gender == "M" ? "Male" : "Female",
                        BirthPlace = data.Places[p.PlaceId].Name
                    }).ToList();

                return View(searchViewModel);
            }

            return View(searchViewModel);
        }

        public ActionResult Advanced()
        {
            return View(new SearchViewModel());
        }

        [HttpPost]
        public ActionResult Advanced(SearchViewModel searchViewModel)
        {
            if (ModelState.IsValid)
            {
                var data = new DataRepository(Server.MapPath($@"~\app_data\{ConfigurationManager.AppSettings["dataFilename"]}"));

                string genderSearch = null;
                if (searchViewModel.Male != searchViewModel.Female)
                {
                    genderSearch = searchViewModel.Male ? "M" : "F";
                }

                bool ancestorSearch = searchViewModel.Direction == SearchDirection.Ancestors;

                searchViewModel.Result = data.FindPersonHierarchy(searchViewModel.Name, genderSearch, ancestorSearch)
                    .Select(p => new PersonSearchResult()
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Gender = p.Gender == "M" ? "Male" : "Female",
                        BirthPlace = data.Places[p.PlaceId].Name
                    }).ToList();

                return View(searchViewModel);
            }

            return View(new SearchViewModel());
        }
    }
}