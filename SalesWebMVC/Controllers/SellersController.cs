using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesWebMVC.Models;
using SalesWebMVC.Models.ViewModels;
using SalesWebMVC.Services;
using SalesWebMVC.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SalesWebMVC.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _service;
        private readonly DepartmentService _departmentService;

        public SellersController(SellerService service, DepartmentService departmentService)
        {
            _service = service;
            _departmentService = departmentService;
        }

        public async Task<IActionResult> Index()
        {
            List<Seller> sellers = await _service.FindAllWithRelationsAsync();
            return View(sellers);
        }

        public async Task<IActionResult> Create()
        {
            List<Department> departments = await _departmentService.FindAllAsync();
            SellerFormViewModel viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel);
            }

            await _service.InsertAsync(seller);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id Not Provided" });
            }

            Seller seller = await _service.FindByIdAsync(id.Value);

            if (seller == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Seller Not Found" });
            }

            return View(seller);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id Not Provided" });
            }

            Seller seller = await _service.FindByIdAsync(id.Value);

            if (seller == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Seller Not Found" });
            }

            return View(seller);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id Not Provided" });
            }

            Seller seller = await _service.FindByIdAsync(id.Value);

            if (seller == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Seller Not Found" });
            }

            List<Department> departments = await _departmentService.FindAllAsync();
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id , Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel);
            }

            if (id != seller.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Seller Id Mismatch" });
            }

            try
            {
                await _service.UpdateAsync(seller);
            } 
            catch(ApplicationException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Error(string message)
        {
            ErrorViewModel viewModel = new ErrorViewModel 
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier 
            };

            return View(viewModel);
        }
    }
}
