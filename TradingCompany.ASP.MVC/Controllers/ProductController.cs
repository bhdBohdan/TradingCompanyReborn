using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using TradingCompany.ASP.MVC.Models;
using TradingCompany.BLL.Interfaces;
using TradingCompany.DTO;

namespace TradingCompany.ASP.MVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductManager _manager;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductManager manager, IMapper mapper, ILogger<ProductController> logger)
        {
            _manager = manager;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: ProductController

        [AllowAnonymous]
        public ActionResult Index()
        {
            _logger.LogInformation("Visiting the Index page."); // This should log
            _logger.LogDebug("This is a debug message.");
            var products = _manager.GetProducts();
            Console.WriteLine("Products retrieved: " + products.Count);
            return View(products);
        }

        // GET: ProductController/Details/5
        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            _logger.LogInformation($"Visiting the Details page of item {id}"); // This should log
            _logger.LogDebug("This is a debug message.");
            var product = _manager.GetProductById(id);
            return View(product);
        }

        // GET: ProductController/Create

        private void AddDictionaries(EditProductModel model)
        {
            model.Users = _mapper.Map<List<SelectListItem>>(_manager.GetSomeUsers());
        }


        [Authorize(Roles = $"{nameof(RoleType.USER)},{nameof(RoleType.ADMIN)}")]
        public ActionResult Create()
        {
            var editProductModel = new EditProductModel();
            AddDictionaries(editProductModel);
            return View(editProductModel);
        }

        // POST: ProductController/Create
        [Authorize(Roles = $"{nameof(RoleType.USER)},{nameof(RoleType.ADMIN)}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EditProductModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .SelectMany(kvp => kvp.Value.Errors.Select(e => new { Key = kvp.Key, Error = e.ErrorMessage, Exception = e.Exception?.Message }))
                    .ToList();

                // quick console log (replace with ILogger in production)
                Console.WriteLine("ModelState is invalid. Errors:");
                foreach (var err in errors)
                {
                    Console.WriteLine($" - {err.Key}: {err.Error} {(err.Exception != null ? $"(ex: {err.Exception})" : "")}");
                }

                // re-populate any dictionaries used in the view and return same view
                AddDictionaries(model);
                return View(model);
            }
            try
            {
                if (ModelState.IsValid)
                {
                    //throw new ArgumentOutOfRangeException("test");

                    var product = _manager.AddProduct(_mapper.Map<Product>(model));
                    return RedirectToAction("Index");
                }
                else
                {
                    AddDictionaries(model);
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("An exception has occurred when creating a movie object. Exception: {ex}", ex);
                ModelState.AddModelError(string.Empty, $"An exception has occurred: {ex}");
                AddDictionaries(model);
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    Message = ex.Message
                });
            }
        }

        // GET: ProductController/Edit/5

        [Authorize(Roles = $"{nameof(RoleType.ADMIN)}")]

        public ActionResult Edit(int id)
        {
            var editProductModel = _manager.GetProductById(id) is Product product
               ? _mapper.Map<EditProductModel>(product)
               : new EditProductModel();
            AddDictionaries(editProductModel);
            return View(editProductModel);
        }

        // POST: ProductController/Edit/5
        [Authorize(Roles = $"{nameof(RoleType.ADMIN)}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EditProductModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .SelectMany(kvp => kvp.Value.Errors.Select(e => new { Key = kvp.Key, Error = e.ErrorMessage, Exception = e.Exception?.Message }))
                    .ToList();

                // quick console log (replace with ILogger in production)
                Console.WriteLine("ModelState is invalid. Errors:");
                foreach (var err in errors)
                {
                    Console.WriteLine($" - {err.Key}: {err.Error} {(err.Exception != null ? $"(ex: {err.Exception})" : "")}");
                }

                // re-populate any dictionaries used in the view and return same view
                AddDictionaries(model);
                return View(model);
            }

            try
            {
                var updated = _manager.UpdateProduct(_mapper.Map<Product>(model));
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An exception has occurred: {ex}");
                _logger.LogError(ex, "An error occurred in the Create action.");
                AddDictionaries(model);
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    Message = ex.Message
                });
            }
        }

        // GET: ProductController/Delete/5
        // Shows a confirmation page (optional)
        [Authorize(Roles = $"{nameof(RoleType.ADMIN)}")]

        public ActionResult Delete(int id)
        {
            var product = _manager.GetProductById(id);
            if (product == null) return NotFound();
            return View(product);
        }

        // POST: ProductController/Delete/5
        // Use ActionName to keep the route /Product/Delete{id} for POST
        [Authorize(Roles = $"{nameof(RoleType.ADMIN)}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var result = _manager.DeleteProduct(id);
                if (result) return RedirectToAction(nameof(Index));

                // deletion failed for some reason
                ModelState.AddModelError(string.Empty, "Unable to delete the product.");
                var product = _manager.GetProductById(id);
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    Message = "Unable to delete product"
                });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An exception has occurred: {ex.Message}");
                var product = _manager.GetProductById(id);
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    Message = ex.Message
                });
            }
        }

        // GET: ProductController/Delete/5 (existing delete-by-link pattern)
        public ActionResult DeleteById(int id) => Delete(id);
    }
}
