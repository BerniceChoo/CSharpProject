using ardonagh.Data;
using ardonagh.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ardonagh.Controllers
{
    
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;

        public CustomersController(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var customers = await applicationDbContext.Customer.ToListAsync();
            return View(customers);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCustomerViewModel addCustomerRequest)
        {
            var customer = new Customer()
            {
                //Id = Guid(),
                Name = addCustomerRequest.Name,
                Age = addCustomerRequest.Age,
                Postcode = addCustomerRequest.Postcode,
                Height = addCustomerRequest.Height
            };

            await applicationDbContext.AddAsync(customer);
            await applicationDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> View(int id)
        {
            var customer = await applicationDbContext.Customer.FirstOrDefaultAsync(x => x.Id == id);

            if (customer != null)
            {
                var viewModel = new UpdateCustomerViewModel()
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Age = customer.Age,
                    Postcode = customer.Postcode,
                    Height = customer.Height
                };

                return await Task.Run(() => View("View", viewModel));
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateCustomerViewModel model)
        {
            var customer = await applicationDbContext.Customer.FindAsync(model.Id);

            if (customer != null)
            {
                customer.Name = model.Name;
                customer.Age = model.Age;
                customer.Postcode = model.Postcode;
                customer.Height = model.Height;

                await applicationDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}

