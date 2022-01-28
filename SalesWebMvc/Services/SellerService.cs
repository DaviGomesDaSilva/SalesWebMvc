using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Services
{
    public class SellerService
    {
        private readonly SalesWebMvcContext _context;

        public SellerService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public List<Seller> FindAll() //metodo que vai retornar do DB uma lista de vendedores 
        {
            return _context.Seller.ToList();
        }

        public void Insert(Seller obj) //metodo para salva no DB os dados do form em /Sellers/Create
        {
            //obj.Department = _context.Department.First(); //paleativo que apenas associa o primeiro depto ao vendedor par anao dar erro na compilacao //comentado apos a criacao da prop DepartmentId
            _context.Add(obj); //insere as infos do objeto inseridas no form
            _context.SaveChanges(); //salva as infos do objeto no DB
        }
    }
}
