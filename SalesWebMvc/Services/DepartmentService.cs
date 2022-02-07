using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Services
{
    public class DepartmentService
    {
        private readonly SalesWebMvcContext _context;

        public DepartmentService(SalesWebMvcContext context)
        {
            _context = context;
        }
        //passando metodo FindAll de sincrono para assincrono
        public async Task<List<Department>> FindAllAsync() //metodo para retornar os departamentos ordenados
        {
            return await _context.Department.OrderBy(x => x.Name).ToListAsync();
        }

        //public List<Department> FindAll() //metodo para retornar os departamentos ordenados
        //{
        //    return _context.Department.OrderBy(x => x.Name).ToList();
        //}
    }
}
