using Microsoft.AspNetCore.Mvc;

namespace PoZiomkaApi.Controllers
{
	public class AdminController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
