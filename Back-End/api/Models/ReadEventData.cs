using System.Collections.Generic;
using api.DBConnect;
using api.Interfaces;
using MySql.Data.MySqlClient;

namespace api.Models
{
    public class ReadEventData : IGetAllEvents, IGetEvent
    {
        public List<Event> GetAllEvents()
        {
            ConnectionString myConnection = new ConnectionString(); 
            string cs = myConnection.cs;
            using var con = new MySqlConnection(cs);
            con.Open(); 


            string stm = "SELECT * FROM event LEFT JOIN customer ON event.Cust_ID = customer.Cust_ID";
            using var cmd = new MySqlCommand(stm,con); 

            using MySqlDataReader rdr = cmd.ExecuteReader();

            List<Customer> customersEvent = new List<Customer>(); 
            List<Event> allEvent = new List<Event>(); 

            while (rdr.Read())
            {
                allEvent.Add(new Event(){EventID = rdr.GetInt32(0),
                    EventName = rdr.GetString(1),
                    EventDate = rdr.GetDateTime(2),
                    EventTime = rdr.GetString(3),
                    Cost = rdr.GetDouble(4),
                    Customer = new Customer(){
                    CustID = rdr.GetInt32(6),
                    AccountNo = rdr.GetString(7), 
                    FName = rdr.GetString(8),
                    LName = rdr.GetString(9),
                    Company = rdr.GetString(10),
                    Phone = rdr.GetString(11),
                    Email = rdr.GetString(12)}});

            }
            return allEvent; 
        }

        public List<Event> GetAllUpComingEvents()
        {
            ConnectionString myConnection = new ConnectionString(); 
            string cs = myConnection.cs;
            using var con = new MySqlConnection(cs);
            con.Open(); 


            string stm = "SELECT * FROM event WHERE DATEDIFF(Event_Date, NOW()) >= 1";
            using var cmd = new MySqlCommand(stm,con); 

            using MySqlDataReader rdr = cmd.ExecuteReader();

            List<Customer> customersEvent = new List<Customer>(); 
            List<Event> allEvent = new List<Event>(); 

            while (rdr.Read())
            {
                allEvent.Add(new Event(){EventID = rdr.GetInt32(0),
                    EventName = rdr.GetString(1),
                    EventDate = rdr.GetDateTime(2),
                    EventTime = rdr.GetString(3),
                    Cost = rdr.GetDouble(4)});

            }
            return allEvent; 
        }


        public Event GetEvent(int id)
        {
            ConnectionString myConnection = new ConnectionString(); 
            string cs = myConnection.cs;
            using var con = new MySqlConnection(cs);
            con.Open(); 


            string stm = "SELECT * FROM event LEFT JOIN customer ON event.Cust_ID = customer.Cust_ID WHERE event.Cust_ID = @id";
            using var cmd = new MySqlCommand(stm,con); 
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare(); 
            using MySqlDataReader rdr = cmd.ExecuteReader();

            rdr.Read(); 
            return new Event(){EventID = rdr.GetInt32(0),
                    EventName = rdr.GetString(1),
                    EventDate = rdr.GetDateTime(2),
                    EventTime = rdr.GetString(3),
                    Cost = rdr.GetDouble(4),
                Customer = new Customer(){
                    CustID = rdr.GetInt32(6),
                    AccountNo = rdr.GetString(7), 
                    FName = rdr.GetString(8),
                    LName = rdr.GetString(9),
                    Company = rdr.GetString(10),
                    Phone = rdr.GetString(11),
                    Email = rdr.GetString(12)}};
        }

        public Event NextEvent()
        {
            ConnectionString myConnection = new ConnectionString(); 
            string cs = myConnection.cs;
            using var con = new MySqlConnection(cs);
            con.Open(); 


            string stm = "SELECT Event_ID, Event_Date, Event_Time, Cost, Event.Cust_ID, Customer.FName, Customer.LName, DATEDIFF(Event_Date,NOW()) AS TimeDifference FROM event LEFT JOIN customer ON event.Cust_ID = customer.Cust_ID WHERE event.Cust_ID = customer.Cust_ID AND Event_Date > NOW() ORDER BY Event_Date LIMIT 1";
            using var cmd = new MySqlCommand(stm,con); 
            using MySqlDataReader rdr = cmd.ExecuteReader();

            rdr.Read(); 
            return new Event(){EventID = rdr.GetInt32(0),
                    EventDate = rdr.GetDateTime(1),
                    EventTime = rdr.GetString(2),
                    Cost = rdr.GetDouble(3),
                Customer = new Customer(){
                    CustID = rdr.GetInt32(4),
                    FName = rdr.GetString(5),
                    LName = rdr.GetString(6),
                    TimeDifference = rdr.GetInt32(7)}};
        }
        public Event GetByInvoice(int id)
        {
            DBConnection db = new DBConnection(); 
            Event invoice = new Event(){};
            bool isOpen = db.OpenConnection(); 
            if (isOpen){
                MySqlConnection conn = db.GetConn(); 
                string stm = "SELECT customer.Cust_ID, customer.FName, customer.LName, customer.Email, Invoice_ID, InvoiceNo, AmountDue, invoice.Event_ID, customer.Account_No FROM invoice JOIN event ON invoice.Event_ID = event.Event_ID JOIN customer ON event.Event_ID = customer.Cust_ID WHERE invoice.Invoice_ID = @id";
                MySqlCommand cmd = new MySqlCommand(stm, conn); 
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Prepare(); 
                using(var rdr = cmd.ExecuteReader()){
                    while (rdr.Read())
                    {
                        invoice.EventID = rdr.GetInt32(7);
                        invoice.Customer = new Customer(){
                        CustID = rdr.GetInt32(0),
                        AccountNo = rdr.GetString(8), 
                        FName = rdr.GetString(1),        
                        LName = rdr.GetString(2),
                        Email = rdr.GetString(3)};
                        invoice.Invoice = new Invoice(){

                            InvoiceID = rdr.GetInt32(4),
                            InvoiceNo = rdr.GetString(5),
                            AmountDue = rdr.GetDouble(6),
                            EventID = rdr.GetInt32(7)
                        }; 
                        
                    }
                }
                db.CloseConnection(); 
                return invoice; 
            }else{
                return new Event(){}; 
            }   
        }
    }
}