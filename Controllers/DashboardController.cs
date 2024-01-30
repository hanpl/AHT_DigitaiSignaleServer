using DigitalSignageSevice.Models;
using DigitalSignageSevice.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DigitalSignageSevice.Controllers
{
    public class DashboardController : Controller
    {
        DigitalSignarlRepository digitalSignarlRepository;
        public DashboardController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            digitalSignarlRepository = new DigitalSignarlRepository(connectionString);
        }
        public IActionResult Index()
        {
            List<AHT_DigitalSignage> mode = new List<AHT_DigitalSignage>();
            mode = digitalSignarlRepository.GetDataDigital();
            return View(mode);
        }

    }
}
