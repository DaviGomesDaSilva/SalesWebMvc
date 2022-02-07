using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;
using SalesWebMvc.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService; //para chamar o metodo FindAll deve-se declarar essa injeção de dependencia (variavel e construtor)
        private readonly DepartmentService _departmentService; //para chamar o metodo FindAll deve-se declarar essa injeção de dependencia (variavel e construtor)
        public SellersController(SellerService sellerService, DepartmentService departmentService)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }

        //passado para assincrona
        public async Task<IActionResult> Index()
        {
            var list = await _sellerService.FindAllAsync();
            return View(list);
        }

        //passado para assincrona
        public async Task<IActionResult> Create() //metodo que abre o formulário para cadastrar um vendedor
        {
            var departments = await _departmentService.FindAllAsync(); //busca no DB todos os departments (ver DepartmentService.cs)
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }

        //passado para assincrona
        [HttpPost] //declara que Create é um método Post
        [ValidateAntiForgeryToken] //Evita brechas para ataque externo
        public async Task<IActionResult> Create(Seller seller) //nao é necessario editar pois o compilador consegue identificar a classe SellerController
        {
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel);
            }

            await _sellerService.InsertAsync(seller);
            return RedirectToAction(nameof(Index));
        }

        //passado para assincrona
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "ID not provided" });
            }

            var obj = await _sellerService.FindByIdAsync(id.Value);

            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "ID not found" });
            }
            return View(obj);
        }

        //passado para assincrona
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _sellerService.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }

        //passado para assincrona
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "ID not provided" });
            }

            var obj = await _sellerService.FindByIdAsync(id.Value);

            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "ID not found" });
            }
            return View(obj);
        }

        //passado para assincrona
        public async Task<IActionResult> Edit(int? id)  //esse metodo serve para abrir a tela de editar 
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "ID not provided" });
            }

            var obj = await _sellerService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "ID not found" });
            }

            List<Department> departments = await _departmentService.FindAllAsync();
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments };
            return View(viewModel);
        }

        //passado para assincrona
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel);
            }

            if (id != seller.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "ID mismatch" });
            }

            try
            {
                await _sellerService.UpdateAsync(seller);
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException e) //Usar ApplicationException para cobrir NotFoundException e DbConcurrencyException em um só metodo
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
            //catch (NotFoundException e)
            //{
            //    return RedirectToAction(nameof(Error), new { message = e.Message });
            //}
            //catch (DbConcurrencyException e)
            //{
            //    return RedirectToAction(nameof(Error), new { message = e.Message });
            //}
        }
            public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
        }
    }
}

