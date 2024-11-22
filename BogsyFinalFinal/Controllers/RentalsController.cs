using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BogsyFinalFinal.Data;
using BogsyFinalFinal.Models;

namespace BogsyFinalFinal.Controllers
{
    public class RentalsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RentalsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Rentals
        public async Task<IActionResult> Index()
        {
            var rentals = await (from r in _context.Rentals
                                 join v in _context.Videos on r.VideoID equals v.VideoID
                                 join c in _context.Customers on r.CustomerID equals c.CustomerId  // Join on CustomerID
                                 select new RentalViewModel
                                 {
                                     RentalID = r.RentalID,
                                     CustomerName = c.CustomerName,  // Fetch the CustomerName
                                     VideoTitle = v.Title,  // Fetch the Video Title
                                     RentalDate = r.RentalDate,
                                     ReturnDate = r.ReturnDate,
                                     IsReturned = r.IsReturned ? "Yes" : "No",
                                     TotalRentalPrice = r.DaysRented * v.RentalPrice
                                 }).ToListAsync();

            return View(rentals);  // Pass the list of rentals to the view
        }






        // GET: Rentals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentals = await _context.Rentals
                .Include(r => r.Customer)
                .Include(r => r.Video)
                .FirstOrDefaultAsync(m => m.RentalID == id);
            if (rentals == null)
            {
                return NotFound();
            }

            return View(rentals);
        }

        // GET: Rentals/Create
        public IActionResult Create()
        {
            // Check the number of customers and videos to ensure there is data
            var customers = _context.Customers.ToList();
            var videos = _context.Videos.ToList();

            // Output the counts to the console for debugging
            Console.WriteLine($"Number of customers: {customers.Count}");
            Console.WriteLine($"Number of videos: {videos.Count}");

            // Ensure the ViewBag is populated with data for dropdowns
            ViewBag.CustomerID = new SelectList(customers, "CustomerId", "CustomerName");
            ViewBag.VideoID = new SelectList(videos, "VideoID", "Title");

            return View();
        }

        // POST: Rentals/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Rentals rentals)
        {
            // Ensure the model is valid
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine("Validation Error: " + error.ErrorMessage);
                }

                // Repopulate ViewBag to ensure dropdowns are rendered with the correct options
                var customers = _context.Customers.ToList();
                var videos = _context.Videos.ToList();
                ViewBag.CustomerID = new SelectList(customers, "CustomerId", "CustomerName");
                ViewBag.VideoID = new SelectList(videos, "VideoID", "Title");

                return View(rentals);
            }

            // Check if the CustomerID exists
            if (!_context.Customers.Any(c => c.CustomerId == rentals.CustomerID))
            {
                ModelState.AddModelError("", "Invalid Customer selection.");
                return View(rentals);
            }

            // Check if the VideoID exists
            if (!_context.Videos.Any(v => v.VideoID == rentals.VideoID))
            {
                ModelState.AddModelError("", "Invalid Video selection.");
                return View(rentals);
            }

            // Add the rental record
            _context.Rentals.Add(rentals);

            try
            {
                int affectedRows = _context.SaveChanges(); // Save changes to the database

                // Log the result
                if (affectedRows > 0)
                {
                    Console.WriteLine("Rental saved successfully.");
                }
                else
                {
                    Console.WriteLine("No rows were affected during SaveChanges.");
                }
            }
            catch (Exception ex)
            {
                // Log any errors that occur during SaveChanges
                Console.WriteLine($"Error saving rental: {ex.Message}");
                ModelState.AddModelError("", "Error occurred while saving the rental.");
                return View(rentals);
            }

            return RedirectToAction("Index");
        }






        // GET: Rentals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentals = await _context.Rentals.FindAsync(id);
            if (rentals == null)
            {
                return NotFound();
            }
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerId", "CustomerName", rentals.CustomerID);
            ViewData["VideoID"] = new SelectList(_context.Set<Videos>(), "VideoID", "Category", rentals.VideoID);
            return View(rentals);
        }

        // POST: Rentals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RentalID,CustomerID,VideoID,RentalDate,ReturnDate,IsReturned,TotalRentalPrice")] Rentals rentals)
        {
            if (id != rentals.RentalID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rentals);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentalsExists(rentals.RentalID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerId", "CustomerName", rentals.CustomerID);
            ViewData["VideoID"] = new SelectList(_context.Set<Videos>(), "VideoID", "Category", rentals.VideoID);
            return View(rentals);
        }

        // GET: Rentals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentals = await _context.Rentals
                .Include(r => r.Customer)
                .Include(r => r.Video)
                .FirstOrDefaultAsync(m => m.RentalID == id);
            if (rentals == null)
            {
                return NotFound();
            }

            return View(rentals);
        }

        // POST: Rentals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rentals = await _context.Rentals.FindAsync(id);
            if (rentals != null)
            {
                _context.Rentals.Remove(rentals);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentalsExists(int id)
        {
            return _context.Rentals.Any(e => e.RentalID == id);
        }
    }
}
