using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SalesWebMVC.Models
{
    public class Seller
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage ="{0} Is Required")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "{0} Size should be between {2} and {1}")]
        public string Name { get; set; }
        [DataType(DataType.EmailAddress)]
        
        [Required(ErrorMessage = "{0} Is Required")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public string Email { get; set; }
        
        [Display(Name = "Birthday")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "{0} Is Required")]
        [Range(1000.00, 50000.00, ErrorMessage = "Base Salary should be between {1} and {2}")]
        [Display(Name = "Base Salary")]
        [DisplayFormat(DataFormatString = "{0:F2}")] //0 é para acessar o atributo
        public double BaseSalary { get; set; }
        
        public Department Department { get; set; }
        
        public int DepartmentId { get; set; }
        
        public ICollection<SalesRecord> Sales { get; set; } = new List<SalesRecord>();

        public Seller() { }

        public Seller(int id, string name, string email, DateTime birthDate, double baseSalary, Department department)
        {
            Id = id;
            Name = name;
            Email = email;
            BirthDate = birthDate;
            BaseSalary = baseSalary;
            Department = department;
        }

        public void AddSales(SalesRecord saleRecord)
        {
            Sales.Add(saleRecord);
        }

        public void Remove(SalesRecord saleRecord)
        {
            Sales.Remove(saleRecord);
        }

        public double TotalSales(DateTime initial, DateTime final)
        {
            return Sales.Where(saleRecord => saleRecord.Date >= initial && saleRecord.Date <= final)
                .Sum(saleRecord => saleRecord.Amount);
        }
    }
}
