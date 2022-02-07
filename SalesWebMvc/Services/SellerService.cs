using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Services.Exceptions;

namespace SalesWebMvc.Services
{
    public class SellerService
    {
        private readonly SalesWebMvcContext _context;

        public SellerService(SalesWebMvcContext context)
        {
            _context = context;
        }

        //Passando metodo de síncrono para assincrono
        public async Task<List<Seller>> FindAllAsync() //metodo que vai retornar do DB uma lista de vendedores 
        {
            return await _context.Seller.ToListAsync();
        }
        //public List<Seller> FindAll() //metodo que vai retornar do DB uma lista de vendedores 
        //{
        //    return _context.Seller.ToList();
        //}

        //Passando metodo de síncrono para assincrono
        public async Task InsertAsync(Seller obj) //metodo para salva no DB os dados do form em /Sellers/Create
        {
            //obj.Department = _context.Department.First(); //paleativo que apenas associa o primeiro depto ao vendedor par anao dar erro na compilacao //comentado apos a criacao da prop DepartmentId
            _context.Add(obj); //insere as infos do objeto inseridas no form
            await _context.SaveChangesAsync(); //salva as infos do objeto no DB
        }

        /*public void Insert(Seller obj) //metodo para salva no DB os dados do form em /Sellers/Create
        {
            //obj.Department = _context.Department.First(); //paleativo que apenas associa o primeiro depto ao vendedor par anao dar erro na compilacao //comentado apos a criacao da prop DepartmentId
            _context.Add(obj); //insere as infos do objeto inseridas no form
            _context.SaveChanges(); //salva as infos do objeto no DB
        }*/

        public async Task<Seller> FindByIdAsync(int id)
        {
            return await _context.Seller.Include(obj => obj.Department).FirstOrDefaultAsync(obj => obj.Id == id); //eager loading - carregar outros objetos associado ao objeto principal
        }

        //passado para assincrono
        public async Task RemoveAsync(int id)
        {
            var obj = await _context.Seller.FindAsync(id);
            _context.Seller.Remove(obj);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Seller obj)
        {
            bool hasAny = await _context.Seller.AnyAsync(x => x.Id == obj.Id);

            if (!hasAny)
            {
                throw new NotFoundException("Id not found");
            }
            try
            {
                _context.Update(obj);
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }
    }
}