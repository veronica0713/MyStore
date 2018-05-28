using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyStore.Models;


namespace MyStore.Controllers
{
    public class YachtsAdminController : Controller
    {
        private readonly BoatChartersDbContext _context;
        private readonly IHostingEnvironment _env;

        public YachtsAdminController(BoatChartersDbContext context, IHostingEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: YachtsAdmin
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        // GET: YachtsAdmin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var yacht = await _context.Products
                .SingleOrDefaultAsync(m => m.ID == id);
            if (yacht == null)
            {
                return NotFound();
            }

            return View(yacht);
        }

        // GET: YachtsAdmin/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: YachtsAdmin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Description,PriceHighSeason,PriceOffSeason,Image,Size,Year,Cabins,AirCond")] Yacht yacht)
        {
            if (ModelState.IsValid)
            {
                _context.Add(yacht);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(yacht);
        }

        // GET: YachtsAdmin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var yacht = await _context.Products.SingleOrDefaultAsync(m => m.ID == id);
            if (yacht == null)
            {
                return NotFound();
            }
            return View(yacht);
        }

        // POST: YachtsAdmin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Yacht yacht , Microsoft.AspNetCore.Http.IFormFile imageFile)
        {
            if (id != yacht.ID)
            {
                return NotFound();
            }
            string newFile = System.IO.Path.Combine(_env.WebRootPath,
                "images", imageFile.FileName);

            System.IO.FileInfo newFileInfo = new System.IO.FileInfo(newFile);

            using (System.IO.FileStream fs = newFileInfo.Create())
            {
                 await imageFile.CopyToAsync(fs);
                fs.Close();
            }

            yacht.Image = "/images/" + imageFile.FileName;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(yacht);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!YachtExists(yacht.ID))
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
            return View(yacht);
        }

        // GET: YachtsAdmin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var yacht = await _context.Products
                .SingleOrDefaultAsync(m => m.ID == id);
            if (yacht == null)
            {
                return NotFound();
            }

            return View(yacht);
        }

        // POST: YachtsAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var yacht = await _context.Products.SingleOrDefaultAsync(m => m.ID == id);
            _context.Products.Remove(yacht);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool YachtExists(int id)
        {
            return _context.Products.Any(e => e.ID == id);
        }
    }
}
