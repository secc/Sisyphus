    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sisyphus.Data;
using Sisyphus.Models;

namespace Sisyphus.Controllers
{
    public class AppSettingsController : Controller
    {
        private readonly DataContext _context;

        public AppSettingsController(DataContext context)
        {
            _context = context;
        }

        // GET: AppSettings
        public async Task<IActionResult> Index()
        {
            return View(await _context.AppSettings.ToListAsync());
        }

        // GET: AppSettings/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appSetting = await _context.AppSettings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appSetting == null)
            {
                return NotFound();
            }

            return View(appSetting);
        }

        // GET: AppSettings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AppSettings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Key,Value")] AppSetting appSetting)
        {
            if (ModelState.IsValid)
            {
                appSetting.Id = Guid.NewGuid();
                _context.Add(appSetting);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(appSetting);
        }

        // GET: AppSettings/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appSetting = await _context.AppSettings.FindAsync(id);
            if (appSetting == null)
            {
                return NotFound();
            }
            return View(appSetting);
        }

        // POST: AppSettings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Key,Value")] AppSetting appSetting)
        {
            if (id != appSetting.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appSetting);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppSettingExists(appSetting.Id))
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
            return View(appSetting);
        }

        // GET: AppSettings/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appSetting = await _context.AppSettings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appSetting == null)
            {
                return NotFound();
            }

            return View(appSetting);
        }

        // POST: AppSettings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var appSetting = await _context.AppSettings.FindAsync(id);
            _context.AppSettings.Remove(appSetting);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppSettingExists(Guid id)
        {
            return _context.AppSettings.Any(e => e.Id == id);
        }
    }
}
