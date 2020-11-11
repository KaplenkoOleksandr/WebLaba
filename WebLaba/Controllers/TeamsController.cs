using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebLaba.Models;

namespace WebLaba.Controllers
{
    public class TeamsController : Controller
    {
        private readonly WebLabaContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public TeamsController(WebLabaContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: Teams
        public async Task<IActionResult> Index(int? corpId)
        {
            if (corpId == null) { return View(await _context.Teams.Include(t => t.Corporation).ToListAsync()); }

            ViewBag.corpId = corpId;
            var teamsByGame = _context.Teams
                .Where(b => b.CorporationId == corpId)
                .Include(b => b.Corporation);

            return View(await teamsByGame.ToListAsync());
        }

        // GET: Teams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teams = await _context.Teams
                .Include(t => t.Corporation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teams == null)
            {
                return NotFound();
            }

            return View(teams);
        }

        // GET: Teams/Create
        public IActionResult Create(int corpId)
        {
            ViewBag.corpId = corpId;
            return View();
        }

        // POST: Teams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ImageFile,CorporationId")] Teams teams)
        {
                if (teams.ImageFile != null)
                {
                    string wwwrootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(teams.ImageFile.FileName);
                    string extension = Path.GetExtension(teams.ImageFile.FileName);
                    teams.Icon = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwrootPath + "/Icon/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await teams.ImageFile.CopyToAsync(fileStream);
                    }
                }

                _context.Add(teams);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { corpId = teams.CorporationId });
        }

        // GET: Teams/Edit/5
        public async Task<IActionResult> Edit(int? id, int corpId)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.corpId = corpId;
            var teams = await _context.Teams.FindAsync(id);
            if (teams == null)
            {
                return NotFound();
            }
            return View(teams);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Icon,CorporationId,ImageFile")] Teams teams)
        {
            if (id != teams.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (teams.ImageFile != null)
                    {
                        var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "Icon", teams.Icon);
                        if (System.IO.File.Exists(imagePath))
                            System.IO.File.Delete(imagePath);

                        string wwwrootPath = _hostEnvironment.WebRootPath;
                        string fileName = Path.GetFileNameWithoutExtension(teams.ImageFile.FileName);
                        string extension = Path.GetExtension(teams.ImageFile.FileName);
                        teams.Icon = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        string path = Path.Combine(wwwrootPath + "/Icon/", fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await teams.ImageFile.CopyToAsync(fileStream);
                        }
                    }

                    _context.Update(teams);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamsExists(teams.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new { corpId = teams.CorporationId });
            }
            ViewData["CorporationId"] = new SelectList(_context.Corporations, "Id", "Name", teams.CorporationId);
            return View(teams);
        }

        // GET: Teams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teams = await _context.Teams
                .Include(t => t.Corporation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teams == null)
            {
                return NotFound();
            }

            return View(teams);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teams = await _context.Teams.FindAsync(id);

            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "Icon", teams.Icon);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);

            var players = from p in _context.Players
                          where p.TeamId == id
                          select p;
            foreach (var p in players)
            {
                var imagePathP = Path.Combine(_hostEnvironment.WebRootPath, "photo", p.Photo);
                if (System.IO.File.Exists(imagePathP))
                    System.IO.File.Delete(imagePathP);

                _context.Players.Remove(p);
            }

            _context.Teams.Remove(teams);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { corpId = teams.CorporationId });
        }

        private bool TeamsExists(int id)
        {
            return _context.Teams.Any(e => e.Id == id);
        }
    }
}
