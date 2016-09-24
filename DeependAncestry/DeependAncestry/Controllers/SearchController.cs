using System.Web.Mvc;

namespace DeependAncestry.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Advanced()
        {
            throw new System.NotImplementedException();
        }
    }
}