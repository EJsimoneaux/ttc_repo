using api.DBConnect;
using api.Interfaces;
using MySql.Data.MySqlClient;

namespace api.Models
{
    public class SaveCustomer : IInsertCustomer
    {
        public void InsertCustomer(Customer value)
        {
            DBConnection db = new DBConnection(); 
            bool isOpen = db.OpenConnection(); 
            MySqlConnection conn = db.GetConn(); 
            string stm1 = @"INSERT INTO customer(FName, LName, Company, Phone, Email) VALUES(@FName, @LName, @Company, @Phone, @Email)"; 
            string stm2 = @"INSERT INTO customer_message (Cust_ID, message) VALUES (LAST_INSERT_ID(), @Message)"; 
            if (isOpen)
            {
                using (MySqlCommand cmd1 = new MySqlCommand(stm1, conn))
                {
                    cmd1.Parameters.AddWithValue("@FName", value.FName);
                    cmd1.Parameters.AddWithValue("@LName", value.LName);
                    cmd1.Parameters.AddWithValue("@Company", value.Company);
                    cmd1.Parameters.AddWithValue("@Phone", value.Phone);
                    cmd1.Parameters.AddWithValue("@Email", value.Email);
                    cmd1.Prepare(); 
                    cmd1.ExecuteNonQuery(); 
                }
                if (value.Message.Message != null)
                {
                    using (MySqlCommand cmd2 = new MySqlCommand(stm2, conn))
                    {
                        cmd2.Parameters.AddWithValue("@message", value.Message.Message);
                        cmd2.Prepare(); 
                        cmd2.ExecuteNonQuery(); 
                    }
                }
                db.CloseConnection(); 
            }
        }

        public void InsertMessage(CustomerMessage value)
        {
            DBConnection db = new DBConnection(); 
            bool isOpen = db.OpenConnection(); 
            if (isOpen)
            {
                MySqlConnection conn = db.GetConn(); 
                string stm = @"INSERT INTO customer_message(Cust_ID, message) VALUES(@Cust_ID, @message)"; 
                MySqlCommand cmd = new MySqlCommand(stm, conn); 
                
                cmd.Parameters.AddWithValue("@Cust_ID", value.CustID);
                cmd.Parameters.AddWithValue("@message", value.Message);
                cmd.Prepare(); 
                cmd.ExecuteNonQuery(); 
                db.CloseConnection(); 
            }
        }

        public void UpdateCustomer(int id, Customer value)
        {
            throw new System.NotImplementedException();
        }
    }
}