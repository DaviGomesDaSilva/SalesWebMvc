using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;
using System;
using System.Collections.Generic;
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
        public IActionResult Index()
        {
            var list = _sellerService.FindAll();

            return View(list);
        }

        public IActionResult Create() //metodo que abre o formulário para cadastrar um vendedor
        {
            var departments = _departmentService.FindAll(); //busca no DB todos os departments (ver DepartmentService.cs)
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }

        [HttpPost] //declara que Create é um método Post
        [ValidateAntiForgeryToken] //Evita brechas para ataque externo
        public IActionResult Create(Seller seller) //nao é necessario editar pois o compilador consegue identificar a classe SellerController
        {
            _sellerService.Insert(seller);
            return RedirectToAction(nameof(Index));
        }
    }
}
