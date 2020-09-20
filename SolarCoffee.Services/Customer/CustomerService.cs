using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SolarCoffee.Data;

namespace SolarCoffee.Services.Customer
{
    public class CustomerService : ICustomerService
    {
        private readonly SolarDbContext _db;

        public CustomerService(SolarDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Return a List of Customers
        /// </summary>
        /// <returns>return customers</returns>
        public List<Data.Models.Customer> GetAllCustomers()
        {
            var customers = _db.Customers
                .Include(customer => customer.PrimaryAddress)
                .OrderBy(customer => customer.LastName)
                .ToList();
            return customers;
        }

        /// <summary>
        /// Create a new customer
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public ServiceResponse<Data.Models.Customer> CreateCustomer(Data.Models.Customer customer)
        {
            try
            {
                _db.Customers.Add(customer);
                _db.SaveChanges();
                return new ServiceResponse<Data.Models.Customer>
                {
                    IsSuccess = true,
                    Message = "Add New Customer",
                    Data = customer,
                    Time = DateTime.UtcNow,
                };
            }
            catch (Exception e)
            {
                return new ServiceResponse<Data.Models.Customer>
                {
                    IsSuccess = false,
                    Message = e.Message,
                    Data = null,
                    Time = DateTime.UtcNow,
                };
            }
        }

        /// <summary>
        /// Delete a customer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ServiceResponse<bool> DeleteCustomer(int id)
        {
            try
            {
                var customer = _db.Customers.Find(id);
                if (customer == null)
                {
                    return new ServiceResponse<bool>
                    {
                        IsSuccess = false,
                        Message = "Customer not found!",
                        Data = false,
                        Time = DateTime.UtcNow,
                    };
                }

                _db.Customers.Remove(customer);
                _db.SaveChanges();

                return new ServiceResponse<bool>
                {
                    IsSuccess = true,
                    Message = "Delete customer!",
                    Data = true,
                    Time = DateTime.UtcNow,
                };
            }
            catch (Exception e)
            {
                return new ServiceResponse<bool>
                {
                    IsSuccess = false,
                    Message = e.Message,
                    Data = false,
                    Time = DateTime.UtcNow,
                };
            }
        }

        public Data.Models.Customer GetById(int id)
        {
            return _db.Customers.Find(id);
        }
    }
}