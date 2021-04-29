using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        // GET: api/customer
        [EnableCors("AnotherPolicy")]
        [HttpGet]
        public List<Customer> Get()
        {
            IGetAllCustomers myObject = new ReadCustomerData();
            return myObject.GetAllCustomers(); 
            
        }
        [EnableCors("AnotherPolicy")]
        [Route("getinquries")]
        public List<Customer> GetInquries()
        {
            IGetAllCustomers myObject = new ReadCustomerData();
            return myObject.GetCustomerInquiries(); 
           
        }
        [EnableCors("AnotherPolicy")]
        [Route("getbalance")]
        public List<Customer> GetBalance()
        {
            IGetAllCustomers myObject = new ReadCustomerData();
            return myObject.GetCustomerDueAmount(); 
           
        }
        [EnableCors("AnotherPolicy")]
        [Route("getinvoices/{id}")]
        public Customer GetInvoice(int id)
        {
            IGetCustomer myObject = new ReadCustomerData();
            return myObject.GetCustomerInvoices(id); 
           
        }

        [EnableCors("AnotherPolicy")]
        [HttpGet("{id}", Name = "Get")]
        public Customer Get(int id)
        {
            IGetCustomer myObject = new ReadCustomerData(); 
            return myObject.GetCustomer(id);
        }

        // POST: api/ttc
        [EnableCors("AnotherPolicy")]
        [HttpPost]
        public void Post([FromBody] Customer value)
        {
            IInsertCustomer insertObject = new SaveCustomer(); 
            insertObject.InsertCustomer(value);
        }

        // PUT: api/ttc/5
        [EnableCors("AnotherPolicy")]
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ttc/5
        [EnableCors("AnotherPolicy")]
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
