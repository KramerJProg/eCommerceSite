using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceSite.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Add(int id) // This is id if the product to add
        {
            // Get the product from the database
            // Add product to cart cookie

            // Redirect back to previous page
            return View();
        }

        public IActionResult Summary()
        {
            // Display all products in Shopping cart cookie
            return View();
        }
    }
}
