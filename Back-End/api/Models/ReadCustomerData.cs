using System.Collections.Generic;
using api.DBConnect;
using api.Interfaces;
using MySql.Data.MySqlClient;

namespace api.Models
{
    public class ReadCustomerData : IGetAllCustomers, IGetCustomer
    {
        public List<Customer> GetAllCustomers()
        {
            DBConnection db = new DBConnection(); 
            bool isOpen = db.OpenConnection(); 
            if (isOpen){
                MySqlConnection conn = db.GetConn(); 
                string stm = "SELECT * FROM customer";
                MySqlCommand cmd = new MySqlCommand(stm, conn); 
                List<Customer> allCustomers = new List<Customer>(); 
                List<Event> allEvent = new List<Event>(); 

                using(var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        allCustomers.Add(new Customer(){CustID=rdr.GetInt32(0), AccountNo = rdr.GetString(1), FName = rdr.GetString(2), LName = rdr.GetString(3), Company = rdr.GetString(4), Phone = rdr.GetString(5), Email = rdr.GetString(6)}); 
                    }
                   
                }
                db.CloseConnection(); 
                return allCustomers; 
            } else{
                return new List<Customer>(); 
            }

            
        }
        public List<Customer> GetCustomerDueAmount()
        {
            DBConnection db = new DBConnection(); 
            bool isOpen = db.OpenConnection(); 
            if (isOpen){
                MySqlConnection conn = db.GetConn(); 
                string stm = "SELECT customer.cust_id, CONCAT(customer.FName, ' ', customer.LName) AS FullName, SUM(AmountDue), COUNT(Invoice_ID) FROM customer JOIN event ON customer.Cust_ID = event.Cust_ID JOIN invoice ON event.Event_ID = invoice.Event_ID GROUP BY customer.Cust_ID HAVING SUM(AmountDue) > 0";
                MySqlCommand cmd = new MySqlCommand(stm, conn); 
                List<Customer> allCustomers = new List<Customer>();
                using(var rdr = cmd.ExecuteReader()){
                    while (rdr.Read())
                    {
                        allCustomers.Add(new Customer(){CustID = rdr.GetInt32(0), FName = rdr.GetString(1), InvoiceSummary = new Invoice(){AmountDue = rdr.GetDouble(2), NumberOfEvents= rdr.GetInt32(3)}});
                        
                    }
                }
                db.CloseConnection(); 
                return allCustomers;
            }else{
                return new List<Customer>(); 
            }

        }

        public List<Customer> GetCustomerInquiries()
        {
            DBConnection db = new DBConnection(); 
            bool isOpen = db.OpenConnection(); 
            if (isOpen){
                MySqlConnection conn = db.GetConn(); 
                string stm = "SELECT * FROM customer_message LEFT JOIN customer ON customer_message.Cust_ID = customer.Cust_ID ";
                MySqlCommand cmd = new MySqlCommand(stm, conn); 
                List<Customer> allCustomers = new List<Customer>();
                using(var rdr = cmd.ExecuteReader()){
                    while (rdr.Read())
                    {
                        
                        allCustomers.Add(new Customer(){CustID = rdr.GetInt32(3), AccountNo = rdr.GetString(4), FName = rdr.GetString(5), LName = rdr.GetString(6), Company = rdr.GetString(7), Phone = rdr.GetString(8), Email = rdr.GetString(9), Message = new CustomerMessage(){Message = rdr.GetString(1), Status = rdr.GetInt16(2)}}); 
            
                    }
                }
                db.CloseConnection(); 
                return allCustomers;
            }else{
                return new List<Customer>(); 
            }
        }
        public Customer GetCustomer(int id)
        {
            DBConnection db = new DBConnection(); 
            Customer getCustomer = new Customer(){};
            bool isOpen = db.OpenConnection(); 
            if (isOpen){
                MySqlConnection conn = db.GetConn(); 
                string stm = "SELECT * FROM customer LEFT JOIN event ON customer.Cust_ID = event.Cust_ID WHERE customer.Cust_ID = @id";
                MySqlCommand cmd = new MySqlCommand(stm, conn); 
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Prepare(); 
                using(var rdr = cmd.ExecuteReader()){
                    while (rdr.Read())
                    {
                        getCustomer.CustID=rdr.GetInt32(0);
                        getCustomer.AccountNo = rdr.GetString(1);
                        getCustomer.FName = rdr.GetString(2);
                        getCustomer.LName = rdr.GetString(3);
                        getCustomer.Company = rdr.GetString(4); 
                        getCustomer.Phone = rdr.GetString(5); 
                        getCustomer.Email = rdr.GetString(6); 
                        
                    }
                }
                db.CloseConnection(); 
                return getCustomer; 
            }else{
                return new Customer(){}; 
            }   
        }

        public Customer GetCustomerInvoices(int id)
        {
            List <Invoice> custInvoices = new List<Invoice>();
            Customer customer = new Customer(){}; 
            string stm1 = "select * FROM customer WHERE Cust_ID = @id ";
            string stm2 = "select Invoice_ID, InvoiceNo, AmountDue, Invoice.Event_ID, Event.Name FROM customer JOIN event ON customer.Cust_ID = event.Cust_ID JOIN invoice ON event.Event_ID = invoice.Event_ID WHERE Customer.Cust_ID = @id HAVING AmountDue > 0"; 
            DBConnection db = new DBConnection(); 
            MySqlConnection conn = db.GetConn();
            bool isOpen = db.OpenConnection(); 
            if (isOpen)
            {
                using (MySqlCommand cmd1 = new MySqlCommand(stm1, conn))
                {
                    cmd1.Parameters.AddWithValue("@id", id);
                    cmd1.Prepare(); 
                    using MySqlDataReader rdr = cmd1.ExecuteReader(); 
                    rdr.Read();
                    customer.CustID = rdr.GetInt32(0); 
                    customer.AccountNo = rdr.GetString(1); customer.FName = rdr.GetString(2); customer.LName = rdr.GetString(3); customer.Company = rdr.GetString(4); customer.Phone = rdr.GetString(5); customer.Email = rdr.GetString(6);
                    customer.InvoiceList = custInvoices; 
                }
                using (MySqlCommand cmd2 = new MySqlCommand(stm2, conn))
                {
                    cmd2.Parameters.AddWithValue("@id", id);
                    cmd2.Prepare(); 
                    using MySqlDataReader rdr = cmd2.ExecuteReader(); 
                    while(rdr.Read())
                    {
                        custInvoices.Add(new Invoice(){InvoiceID = rdr.GetInt32(0), InvoiceNo = rdr.GetString(1), AmountDue = rdr.GetDouble(2), EventID = rdr.GetInt32(3), EventName = rdr.GetString(4)});
                    }
                }
                db.CloseConnection(); 
                return customer;
            } else {
                return new Customer(); 
            }
           
                
            
        }
    }
}