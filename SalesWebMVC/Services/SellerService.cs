using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Data;
using SalesWebMVC.Models;
using SalesWebMVC.Services.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMVC.Services
{
    public class SellerService
    {
        private readonly SalesWebMVCContext _context;

        public SellerService(SalesWebMVCContext context)
        {
            _context = context;
        }

        public async Task<List<Seller>> FindAllAsync()
        {
            return await _context.Seller.ToListAsync();
        }

        public async Task<List<Seller>> FindAllWithRelationsAsync()
        {
            return await _context.Seller.Include("Department").ToListAsync();
        }

        public async Task InsertAsync(Seller seller)
        {
            _context.Add(seller);
            await _context.SaveChangesAsync();
        }

        public async Task<Seller> FindByIdAsync(int sellerId)
        {
            //TODO LANÇAR EXCEÇÃO DEPOIS...
            return await _context.Seller.Include("Department")
                .FirstOrDefaultAsync(seller => seller.Id == sellerId);
        }

        public async Task RemoveAsync(int sellerId)
        {
            try
            {
                Seller seller = await FindByIdAsync(sellerId);

                List<SalesRecord> sales = _context.SalesRecord.ToList();
                _context.SalesRecord.RemoveRange(sales);

                _context.Seller.Remove(seller);
                await _context.SaveChangesAsync();
            } catch(DbUpdateException)
            {
                throw new IntegrityException("An error ocurred while deleting the seller.");
            }
        }

        public async Task UpdateAsync(Seller seller)
        {
            bool sellerExists = await _context.Seller.AnyAsync(obj => obj.Id == seller.Id);

            if (!sellerExists)
            {
                throw new NotFoundException("The seller was not found in our database");
            }

            try
            {
                _context.Update(seller);
                await _context.SaveChangesAsync();
            } catch(DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }

        }
    }
}
