using api.Models;

namespace api.Interfaces
{
    public interface IInsertCustomer
    {
         public void InsertCustomer(Customer value);
         public void UpdateCustomer(int id, Customer value);
         public void InsertMessage(CustomerMessage value); 
    }
}