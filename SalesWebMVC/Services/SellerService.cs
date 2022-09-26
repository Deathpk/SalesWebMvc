using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Data;
using SalesWebMVC.Models;
using SalesWebMVC.Services.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace SalesWebMVC.Services
{
    public class SellerService
    {
        private readonly SalesWebMVCContext _context;

        public SellerService(SalesWebMVCContext context)
        {
            _context = context;
        }

        public List<Seller> FindAll()
        {
            return _context.Seller.ToList();
        }

        public List<Seller> FindAllWithRelations()
        {
            return _context.Seller.Include("Department").ToList();
        }

        public void Insert(Seller seller)
        {
            _context.Add(seller);
            _context.SaveChanges();
        }

        public Seller FindById(int sellerId)
        {
            //TODO LANÇAR EXCEÇÃO DEPOIS...
            return _context.Seller.Include("Department")
                .FirstOrDefault(seller => seller.Id == sellerId);
        }

        public void Remove(int sellerId)
        {
            Seller seller = FindById(sellerId);

            List<SalesRecord> sales = _context.SalesRecord.ToList();
            _context.SalesRecord.RemoveRange(sales);

            _context.Seller.Remove(seller);
            _context.SaveChanges();
        }

        public void Update(Seller seller)
        {
            bool sellerExists = _context.Seller.Any(obj => obj.Id == seller.Id);

            if (!sellerExists)
            {
                throw new NotFoundException("The seller was not found in our database");
            }

            try
            {
                _context.Update(seller);
                _context.SaveChanges();
            } catch(DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }

        }
    }
}
