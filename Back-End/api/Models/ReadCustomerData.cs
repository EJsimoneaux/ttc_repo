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
            ConnectionString myConnection = new ConnectionString(); 
            string cs = myConnection.cs;
            using var con = new MySqlConnection(cs);
            con.Open(); 


            string stm = "SELECT * FROM customer LEFT JOIN event ON customer.Cust_ID = event.Cust_ID GROUP BY customer.Cust_ID";
            using var cmd = new MySqlCommand(stm,con); 

            using MySqlDataReader rdr = cmd.ExecuteReader();

            List<Customer> allCustomers = new List<Customer>(); 
            List<Event> allEvent = new List<Event>(); 

            while (rdr.Read())
            {
                allCustomers.Add(new Customer(){CustID=rdr.GetInt32(0), AccountNo = rdr.GetString(1), FName = rdr.GetString(2), LName = rdr.GetString(3), Company = rdr.GetString(4), Phone = rdr.GetString(5), Event = new Event(){EventID = rdr.GetInt32(6), EventDate = rdr.GetDateTime(7), EventTime = rdr.GetString(8), Cost = rdr.GetDouble(9)}}); 

            }
            return allCustomers; 
        }

        public Customer GetCustomer(int id)
        {
            ConnectionString myConnection = new ConnectionString(); 
            string cs = myConnection.cs;
            using var con = new MySqlConnection(cs);
            con.Open(); 


            string stm = "SELECT * FROM customer LEFT JOIN event ON customer.Cust_ID = event.Cust_ID WHERE customer.Cust_ID = @id";
            using var cmd = new MySqlCommand(stm,con); 
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare(); 
            using MySqlDataReader rdr = cmd.ExecuteReader();

            rdr.Read(); 
            return new Customer(){CustID=rdr.GetInt32(0), AccountNo = rdr.GetString(1), FName = rdr.GetString(2), LName = rdr.GetString(3), Company = rdr.GetString(4), Phone = rdr.GetString(5)}; 
        }

        public List<Customer> GetCustomerInquiries()
        {
            ConnectionString myConnection = new ConnectionString(); 
            string cs = myConnection.cs;
            using var con = new MySqlConnection(cs);
            con.Open(); 


            string stm = "SELECT * FROM customer_message LEFT JOIN customer ON customer_message.Cust_ID = customer.Cust_ID ";
            using var cmd = new MySqlCommand(stm,con); 

            using MySqlDataReader rdr = cmd.ExecuteReader();

            List<Customer> allCustomers = new List<Customer>(); 

            while (rdr.Read())
            {
                allCustomers.Add(new Customer(){CustID = rdr.GetInt32(3), AccountNo = rdr.GetString(4), FName = rdr.GetString(5), LName = rdr.GetString(6), Company = rdr.GetString(7), Phone = rdr.GetString(8), Message = new CustomerMessage(){Message = rdr.GetString(1), Status = rdr.GetInt16(2)}}); 

            }
            return allCustomers; 
        }
    }
}