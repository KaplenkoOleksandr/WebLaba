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
    public class PlayersController : Controller
    {
        private readonly WebLabaContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public PlayersController(WebLabaContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: Players
        public async Task<IActionResult> Index(int? teamId)
        {
            if (teamId == null) { return View(await _context.Players.Include(p => p.Team).ToListAsync()); }

            ViewBag.teamId = teamId;
            var payersByTeam = _context.Players
                .Where(b => b.TeamId == teamId)
                .Include(b => b.Team);

            return View(await payersByTeam.ToListAsync());
        }

        // GET: Players/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var players = await _context.Players
                .Include(p => p.Team)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (players == null)
            {
                return NotFound();
            }

            return View(players);
        }

        // GET: Players/Create
        public IActionResult Create(int teamId)
        {
            ViewBag.teamId = teamId;
            return View();
        }

        // POST: Players/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Nickname,ImageFile,TeamId")] Players players)
        {
                if (players.ImageFile != null)
                {
                    string wwwrootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(players.ImageFile.FileName);
                    string extension = Path.GetExtension(players.ImageFile.FileName);
                    players.Photo = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwrootPath + "/Photo/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await players.ImageFile.CopyToAsync(fileStream);
                    }
                }
                _context.Add(players);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { teamId = players.TeamId });
        }

        // GET: Players/Edit/5
        public async Task<IActionResult> Edit(int? id, int teamId)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.teamId = teamId;
            var players = await _context.Players.FindAsync(id);
            if (players == null)
            {
                return NotFound();
            }
            return View(players);
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, enable the  specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Nickname,Photo,TeamId,ImageFile")] Players players)
        {
            if (id != players.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (players.ImageFile != null)
                    {
                        var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "Photo", players.Photo);
                        if (System.IO.File.Exists(imagePath))
                            System.IO.File.Delete(imagePath);

                        string wwwrootPath = _hostEnvironment.WebRootPath;
                        string fileName = Path.GetFileNameWithoutExtension(players.ImageFile.FileName);
                        string extension = Path.GetExtension(players.ImageFile.FileName);
                        players.Photo = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        string path = Path.Combine(wwwrootPath + "/Photo/", fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await players.ImageFile.CopyToAsync(fileStream);
                        }
                    }

                    _context.Update(players);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayersExists(players.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new { teamId = players.TeamId });
            }
            return View(players);
        }

        // GET: Players/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var players = await _context.Players
                .Include(p => p.Team)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (players == null)
            {
                return NotFound();
            }

            return View(players);
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var players = await _context.Players.FindAsync(id);

            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "Photo", players.Photo);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);

            _context.Players.Remove(players);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { teamId = players.TeamId });
        }

        private bool PlayersExists(int id)
        {
            return _context.Players.Any(e => e.Id == id);
        }
    }
}
