﻿using eCommerceSite.Data;
using eCommerceSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceSite.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductContext _context;

        public ProductController(ProductContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Displays a view that lists all products
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            // Get all products from database
            // List<Product> products = _context.Products.ToList();
            List<Product> products =
                await (from p in _context.Products
                       select p).ToListAsync();

            // Send list of products to view to be displayed
            return View(products);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Product p)
        {
            if (ModelState.IsValid)
            {
                // Add to DB
                await _context.Products.AddAsync(p);
                await _context.SaveChangesAsync();

                TempData["Message"] = $"{p.Title} was added successfully!";

                // Redirect back to catalog page
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            // Get product with corresponding id
            // Query Syntax
            Product p =
                await (from prod in _context.Products
                       where prod.ProductId == id
                       select prod).SingleAsync();
            // Method Syntax
            //Product p2 =
            //    _context.Products.Where(prod => prod.ProductId == id).SingleAsync();

            // pass product to view
            return View(p);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product p)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(p).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                ViewData["Message"] = "Product updated successfully";
            }
            return View(p);
        }

        public async Task<IActionResult> Delete(int id)
        {
            Product p = await (from prod in _context.Products
                         where prod.ProductId == id
                         select prod).SingleAsync();

            return View(p);
        }
    }
}
