using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Sisyphus.Data;
using Sisyphus.Models;
using Sisyphus.Services;
using Sisyphus.Utilities;
using Sisyphus.ViewModels;

namespace Sisyphus.Controllers
{
    public class OperationsController : Controller
    {
        private readonly DataContext dataContext;
        private readonly IOperationsService operationsService;
        public OperationsController( DataContext dc, IOperationsService opService )
        {
            dataContext = dc;
            operationsService = opService;
        }

        [Authorize( ClaimsPolicies.Administrator )]
        public IActionResult Index()
        {
            return View( dataContext.Operations.ToList() );
        }

        [Authorize( ClaimsPolicies.Administrator )]
        public IActionResult Edit( Guid id )
        {
            var operation = dataContext.Operations.Where( o => o.Id == id ).FirstOrDefault();

            var operationVM = new OperationViewModel();

            if ( operation != null )
            {
                operationVM.Id = operation.Id;
                operationVM.ProviderId = operation.ProviderId;
                operationVM.Name = operation.Name;
                operationVM.SettingsObject = operation.SettingsObject;
            }

            var providers = dataContext.Providers.ToList();
            if ( operation == null )
            {
                providers.Insert( 0, new Provider { Id = Guid.Empty, Name = "" } );
            }
            ViewBag.Providers = providers;

            return View( operationVM );
        }

        [Authorize( ClaimsPolicies.Administrator )]
        public IActionResult Export( Guid id )
        {
            var op = dataContext.Operations.Where( o => o.Id == id ).FirstOrDefault();
            var export = OperationExport.GetExport( op );
            var output = JsonConvert.SerializeObject( export );

            var stream = new MemoryStream();
            var writer = new StreamWriter( stream );
            writer.Write( output );
            writer.Flush();
            stream.Position = 0;
            return File( stream, "application/json", op.Name.Replace( " ", "_" ) + ".json" );
        }

        [Authorize( ClaimsPolicies.Administrator )]
        public IActionResult Settings( Guid id )
        {
            var provider = dataContext.Providers.Where( p => p.Id == id ).FirstOrDefault();
            var operation = new Operation
            {
                Provider = provider
            };

            return PartialView( "Settings", operation );
        }

        [HttpPost]
        [Authorize( ClaimsPolicies.Administrator )]
        public IActionResult Save( OperationViewModel operationViewModel )
        {
            operationsService.UpdateFromViewModel( operationViewModel );

            return Redirect( "/Operations" );
        }

        [Authorize( ClaimsPolicies.Administrator )]
        public async Task<IActionResult> Delete( Guid? id )
        {
            if ( id == null )
            {
                return NotFound();
            }

            var operation = await dataContext.Operations
                .FirstOrDefaultAsync( m => m.Id == id );
            if ( operation == null )
            {
                return NotFound();
            }

            ViewBag.Bundles = dataContext.BundleOpperations
                .Where( bo => bo.OperationId == id )
                .Select( bo => bo.Bundle )
                .ToList();

            return View( operation );
        }

        [HttpPost, ActionName( "Delete" )]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed( Guid id )
        {
            var bundle = await dataContext.Operations.FindAsync( id );
            dataContext.Operations.Remove( bundle );
            await dataContext.SaveChangesAsync();
            return RedirectToAction( nameof( Index ) );
        }

        [Authorize( ClaimsPolicies.Administrator )]
        public IActionResult Import()
        {
            return View();
        }

        [Authorize( ClaimsPolicies.Administrator )]
        [HttpPost]
        public IActionResult Import( FileUpload fileUpload )
        {
            var file = fileUpload.FormFile;
            using ( var stream = file.OpenReadStream() )
            {
                StreamReader reader = new StreamReader( stream );
                string export = reader.ReadToEnd();
                var operation = OperationExport.Import( export );
                return RedirectToAction( "Edit", new { id = operation.Id } );
            }
        }


        [Authorize( ClaimsPolicies.Administrator )]
        public IActionResult ExportAll()
        {

            return Json( dataContext.Operations.ToList() );
        }
    }
}
