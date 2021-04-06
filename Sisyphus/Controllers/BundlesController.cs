using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sisyphus.Data;
using Sisyphus.Models;
using Sisyphus.Utilities;

namespace Sisyphus.Controllers
{
    public class BundlesController : Controller
    {
        private readonly DataContext _context;

        public BundlesController( DataContext context )
        {
            _context = context;
        }

        // GET: Bundles
        [Authorize( ClaimsPolicies.Administrator )]
        public IActionResult Index()
        {
            var bundles =  _context.Bundles.ToList();
            return View( bundles );
        }

        // GET: Bundles/Details/5
        public async Task<IActionResult> Manage( Guid? id )
        {
            if ( id == null )
            {
                return NotFound();
            }

            var bundle = await _context.Bundles
                .FirstOrDefaultAsync( m => m.Id == id );
            if ( bundle == null )
            {
                return NotFound();
            }

            var operations = _context.Operations.OrderBy( o => o.Name ).ToList().ToDictionary( o => o.Id, o => o.Name + " (" + o.Provider.Name + ")" );
            ViewBag.Operations = operations;

            return View( bundle );
        }

        [HttpPost]
        public async Task<IActionResult> AddOperation( Guid id, Guid operationId )
        {
            var bundle = await _context.Bundles
                .FirstOrDefaultAsync( m => m.Id == id );
            var operation = await _context.Operations
                .FirstOrDefaultAsync( o => o.Id == operationId );

            if ( bundle == null || operation == null )
            {
                return NotFound();
            }

            var bundleOp = new BundleOperation
            {
                BundleId = id,
                OperationId = operationId,
                Order = bundle.GetNextOperationId()
            };
            _context.BundleOpperations.Add( bundleOp );
            await _context.SaveChangesAsync();

            return RedirectToAction( "Manage", new { id = id } );
        }

        [HttpPost]
        public async Task<IActionResult> RemoveOperation( Guid id )
        {
            var bundleOp = await _context.BundleOpperations
                .FirstOrDefaultAsync( o => o.Id == id );

            if ( bundleOp == null )
            {
                return NoContent();
            }

            var bundle = bundleOp.Bundle;

            _context.BundleOpperations.Remove( bundleOp );
            await _context.SaveChangesAsync();

            bundle.SquashOrder();

            return Content( "Success" );
        }

        [HttpPost]
        public async Task<IActionResult> MoveOperation( Guid id, Guid? siblingId )
        {
            var bundleOperation = await _context.BundleOpperations
                .FirstOrDefaultAsync( bo => bo.Id == id );

            if ( bundleOperation == null )
            {
                return NotFound();
            }

            var bundle = bundleOperation.Bundle;

            var sibling = await _context.BundleOpperations
                .FirstOrDefaultAsync( bo => bo.Id == siblingId );

            if ( sibling != null )
            {
                var newOrder = sibling.Order;
                var lowerBundleOps = bundle.BundleOperations.Where( bo => bo.Order >= newOrder ).ToList();

                foreach ( var bOp in lowerBundleOps )
                {
                    bOp.Order++;
                }
                bundleOperation.Order = newOrder;
            }
            else
            {
                bundleOperation.Order = bundle.GetNextOperationId();
            }

            await _context.SaveChangesAsync();

            bundle.SquashOrder();

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: Bundles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Bundles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( [Bind( "Id,Name,Description" )] Bundle bundle )
        {
            if ( ModelState.IsValid )
            {
                bundle.Id = Guid.NewGuid();
                _context.Add( bundle );
                await _context.SaveChangesAsync();
                return RedirectToAction( nameof( Index ) );
            }
            return View( bundle );
        }

        // GET: Bundles/Edit/5
        public async Task<IActionResult> Edit( Guid? id )
        {
            if ( id == null )
            {
                return NotFound();
            }

            var bundle = await _context.Bundles.FindAsync( id );
            if ( bundle == null )
            {
                return NotFound();
            }
            return View( bundle );
        }

        // POST: Bundles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( Guid id, [Bind( "Id,Name,Description" )] Bundle bundle )
        {
            if ( id != bundle.Id )
            {
                return NotFound();
            }

            if ( ModelState.IsValid )
            {
                try
                {
                    _context.Update( bundle );
                    await _context.SaveChangesAsync();
                }
                catch ( DbUpdateConcurrencyException )
                {
                    if ( !BundleExists( bundle.Id ) )
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction( nameof( Index ) );
            }
            return View( bundle );
        }

        // GET: Bundles/Delete/5
        public async Task<IActionResult> Delete( Guid? id )
        {
            if ( id == null )
            {
                return NotFound();
            }

            var bundle = await _context.Bundles
                .FirstOrDefaultAsync( m => m.Id == id );
            if ( bundle == null )
            {
                return NotFound();
            }

            return View( bundle );
        }

        // POST: Bundles/Delete/5
        [HttpPost, ActionName( "Delete" )]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed( Guid id )
        {
            var bundle = await _context.Bundles.FindAsync( id );
            _context.Bundles.Remove( bundle );
            await _context.SaveChangesAsync();
            return RedirectToAction( nameof( Index ) );
        }

        public async Task<IActionResult> Run( Guid? id )
        {
            if ( id == null )
            {
                return NotFound();
            }

            var bundle = await _context.Bundles
                .FirstOrDefaultAsync( m => m.Id == id );
            if ( bundle == null )
            {
                return NotFound();
            }

            return View( bundle );
        }

        // POST: Bundles/Delete/5
        [HttpPost, ActionName( "Run" )]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RunConfirmed( Guid id )
        {
            var bundle = await _context.Bundles.FindAsync( id );
            TaskRunner.RunBundle( bundle );

            return RedirectToAction( nameof( Index ) );
        }


        private bool BundleExists( Guid id )
        {
            return _context.Bundles.Any( e => e.Id == id );
        }
    }
}
