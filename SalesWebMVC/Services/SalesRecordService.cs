using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Data;
using SalesWebMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMVC.Services
{
    public class SalesRecordService
    {
        private readonly SalesWebMVCContext _context;

        public SalesRecordService(SalesWebMVCContext context)
        {
            _context = context;
        }

        public async Task< List<SalesRecord> > FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = from obj in _context.SalesRecord select obj; // OBJETO DE CONSULTA NO BANCO , TIPO UM QUERY BUILDER DO LARAVEL.

            if (minDate.HasValue)
            {
                result = result.Where(query => query.Date >= minDate.Value);
            }

            if (maxDate.HasValue)
            {
                result = result.Where(query => query.Date <= maxDate.Value);
            }

            return await result
                .Include(x => x.Seller)
                .Include(x => x.Seller.Department)
                .OrderByDescending(x => x.Date)
                .ToListAsync();
        }

        public async Task< List< IGrouping<Department, SalesRecord> > > FindByDateGroupingAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = from obj in _context.SalesRecord select obj; // OBJETO DE CONSULTA NO BANCO , TIPO UM QUERY BUILDER DO LARAVEL.

            if (minDate.HasValue)
            {
                result = result.Where(query => query.Date >= minDate.Value);
            }

            if (maxDate.HasValue)
            {
                result = result.Where(query => query.Date <= maxDate.Value);
            }

            return await result
                .Include(x => x.Seller)
                .Include(x => x.Seller.Department)
                .OrderByDescending(x => x.Date)
                .GroupBy(x => x.Seller.Department)
                .ToListAsync();
        }
    }
}
