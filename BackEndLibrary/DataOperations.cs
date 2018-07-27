using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEndLibrary
{
    public class DataOperations : BaseSqlServerConnections
    {
        public DataOperations()
        {
            DefaultCatalog = "NorthWindAzure";
        }
        /// <summary>
        /// Conventional method to return data from a database table into a DataTable.
        /// </summary>
        /// <returns></returns>
        public DataTable Customers1()
        {
            mHasException = false;

            var dt = new DataTable();
            const string selectStatement = "SELECT cust.CustomerIdentifier,cust.CompanyName,cust.ContactName,ct.ContactTitle, " +  
                                           "cust.[Address] AS street,cust.City,cust.PostalCode,cust.Country,cust.Phone, " + 
                                           "cust.ContactTypeIdentifier FROM dbo.Customers AS cust " + 
                                           "INNER JOIN ContactType AS ct ON cust.ContactTypeIdentifier = ct.ContactTypeIdentifier;";


            using (var cn = new SqlConnection() { ConnectionString = ConnectionString })
            {
                using (var cmd = new SqlCommand() { Connection = cn, CommandText = selectStatement })
                {
                    try
                    {
                        cn.Open();
                        dt.Load(cmd.ExecuteReader());
                    }
                    catch (Exception e)
                    {
                        mHasException = true;
                        mLastException = e;
                    }
                }
            }

            return dt;
        }
        /// <summary>
        /// Using C#7 Out parameter to return data from a database table into a DataTable.
        /// </summary>
        /// <param name="dtCustomers"></param>
        /// <returns></returns>
        public bool Customers2(out DataTable dtCustomers)
        {
            mHasException = false;

            dtCustomers = new DataTable();

            const string selectStatement = "SELECT cust.CustomerIdentifier,cust.CompanyName,cust.ContactName,ct.ContactTitle, " +
                                           "cust.[Address] AS street,cust.City,cust.PostalCode,cust.Country,cust.Phone, " +
                                           "cust.ContactTypeIdentifier FROM dbo.Customers AS cust " +
                                           "INNER JOIN ContactType AS ct ON cust.ContactTypeIdentifier = ct.ContactTypeIdentifier;";


            using (var cn = new SqlConnection() { ConnectionString = ConnectionString })
            {
                using (var cmd = new SqlCommand() { Connection = cn, CommandText = selectStatement })
                {
                    try
                    {
                        cn.Open();
                        dtCustomers.Load(cmd.ExecuteReader());
                    }
                    catch (Exception e)
                    {
                        mHasException = true;
                        mLastException = e;
                    }
                }
            }

            return IsSuccessFul;
        }
        public bool CustomersWithError2(out DataTable dtCustomers)
        {
            mHasException = false;

            dtCustomers = new DataTable();

            // using a invalid fieldname
            const string selectStatement = "SELECT cust.CustomerIdentifer,cust.CompanyName,cust.ContactName,ct.ContactTitle, " +
                                           "cust.[Address] AS street,cust.City,cust.PostalCode,cust.Country,cust.Phone, " +
                                           "cust.ContactTypeIdentifier FROM dbo.Customers AS cust " +
                                           "INNER JOIN ContactType AS ct ON cust.ContactTypeIdentifier = ct.ContactTypeIdentifier;";


            using (var cn = new SqlConnection() { ConnectionString = ConnectionString })
            {
                using (var cmd = new SqlCommand() { Connection = cn, CommandText = selectStatement })
                {
                    try
                    {
                        cn.Open();
                        dtCustomers.Load(cmd.ExecuteReader());
                    }
                    catch (Exception e)
                    {
                        mHasException = true;
                        mLastException = e;
                    }
                }
            }

            return IsSuccessFul;
        }
        /// <summary>
        /// Using C#7 Out parameter to return data from a database table into a list.
        /// </summary>
        /// <param name="Customers"></param>
        /// <returns></returns>
        public bool Customers3(out List<Customers> Customers)
        {
            mHasException = false;

            Customers = new List<Customers>();

            var selectStatement = "SELECT cust.CustomerIdentifier,cust.CompanyName,cust.ContactName,ct.ContactTitle, " +
                                  "cust.[Address] AS street,cust.City,cust.PostalCode,cust.Country,cust.Phone, " +
                                  "cust.ContactTypeIdentifier FROM dbo.Customers AS cust " +
                                  "INNER JOIN ContactType AS ct ON cust.ContactTypeIdentifier = ct.ContactTypeIdentifier;";


            using (var cn = new SqlConnection() { ConnectionString = ConnectionString })
            {
                using (var cmd = new SqlCommand() { Connection = cn, CommandText = selectStatement })
                {
                    try
                    {
                        cn.Open();
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Customers.Add(new Customers()
                            {
                                CustomerIdentifier = reader.GetInt32(0),
                                CompanyName = reader.GetString(1),
                                ContactName = reader.GetString(2),
                                ContactTitle = reader.GetString(3),
                                Street = reader.GetString(4),
                                City = reader.GetString(5),
                                PostalCode = reader.GetString(6),
                                Country = reader.GetString(7),
                                Phone = reader.GetString(8)
                            });
                        }

                    }
                    catch (Exception e)
                    {
                        mHasException = true;
                        mLastException = e;
                    }
                }
            }

            return IsSuccessFul;
        }
        public bool CustomersSingleByOutParameter(int pId, out Customers Customer)
        {
            mHasException = false;

            Customer = new Customers();

            var selectStatement = "SELECT cust.CustomerIdentifier,cust.CompanyName,cust.ContactName,ct.ContactTitle, " +
                                  "cust.[Address] AS street,cust.City,cust.PostalCode,cust.Country,cust.Phone, " +
                                  "cust.ContactTypeIdentifier FROM dbo.Customers AS cust " +
                                  "INNER JOIN ContactType AS ct ON cust.ContactTypeIdentifier = ct.ContactTypeIdentifier " +
                                  "WHERE cust.CustomerIdentifier = @Id";


            using (var cn = new SqlConnection() { ConnectionString = ConnectionString })
            {
                using (var cmd = new SqlCommand() { Connection = cn, CommandText = selectStatement })
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("@Id", pId);
                        cn.Open();
                        var reader = cmd.ExecuteReader();
                        reader.Read();
                        if (reader.HasRows)
                        {
                            Customer.CustomerIdentifier = reader.GetInt32(0);
                            Customer.CompanyName = reader.GetString(1);
                            Customer.ContactName = reader.GetString(2);
                            Customer.ContactTitle = reader.GetString(3);
                            Customer.Street = reader.GetString(4);
                            Customer.City = reader.GetString(5);
                            Customer.PostalCode = reader.GetString(6);
                            Customer.Country = reader.GetString(7);
                            Customer.Phone = reader.GetString(8);                            
                        }
                        else
                        {
                            return false;                            
                        }

                    }
                    catch (Exception e)
                    {
                        mHasException = true;
                        mLastException = e;
                    }
                }
            }

            return IsSuccessFul;
        }
        public bool CustomersSingleByOutParameterNoCustomer(int pId, out Customers Customer)
        {
            mHasException = false;
            Customer = new Customers();


            var selectStatement = "SELECT cust.CustomerIdentifier,cust.CompanyName,cust.ContactName,ct.ContactTitle, " +
                                  "cust.[Address] AS street,cust.City,cust.PostalCode,cust.Country,cust.Phone, " +
                                  "cust.ContactTypeIdentifier FROM dbo.Customers AS cust " +
                                  "INNER JOIN ContactType AS ct ON cust.ContactTypeIdentifier = ct.ContactTypeIdentifier " +
                                  "WHERE cust.CustomerIdentifier = @Id";


            using (var cn = new SqlConnection() { ConnectionString = ConnectionString })
            {
                using (var cmd = new SqlCommand() { Connection = cn, CommandText = selectStatement })
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("@Id", pId);
                        cn.Open();
                        var reader = cmd.ExecuteReader();
                        reader.Read();
                        if (reader.HasRows)
                        {                           
                            Customer.CustomerIdentifier = reader.GetInt32(0);
                            Customer.CompanyName = reader.GetString(1);
                            Customer.ContactName = reader.GetString(2);
                            Customer.ContactTitle = reader.GetString(3);
                            Customer.Street = reader.GetString(4);
                            Customer.City = reader.GetString(5);
                            Customer.PostalCode = reader.GetString(6);
                            Customer.Country = reader.GetString(7);
                            Customer.Phone = reader.GetString(8);
                        }
                        else
                        {
                            Customer = null;
                            return false;
                        }

                    }
                    catch (Exception e)
                    {
                        mHasException = true;
                        mLastException = e;
                    }
                }
            }

            return IsSuccessFul;
        }
        public bool CustomersContactNameAndTitleByOutParameterDiscard(int pId, out string ContactName, out string ContactTitle)
        {
            mHasException = false;

            ContactName = "";
            ContactTitle = "";

            var selectStatement = "SELECT cust.CustomerIdentifier,cust.CompanyName,cust.ContactName,ct.ContactTitle, " +
                                  "cust.[Address] AS street,cust.City,cust.PostalCode,cust.Country,cust.Phone, " +
                                  "cust.ContactTypeIdentifier FROM dbo.Customers AS cust " +
                                  "INNER JOIN ContactType AS ct ON cust.ContactTypeIdentifier = ct.ContactTypeIdentifier " +
                                  "WHERE cust.CustomerIdentifier = @Id";


            using (var cn = new SqlConnection() { ConnectionString = ConnectionString })
            {
                using (var cmd = new SqlCommand() { Connection = cn, CommandText = selectStatement })
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("@Id", pId);
                        cn.Open();
                        var reader = cmd.ExecuteReader();
                        reader.Read();
                        if (reader.HasRows)
                        {
                            ContactName = reader.GetString(2);
                            ContactTitle = reader.GetString(3);
                        }
                        else
                        {
                            return false;
                        }

                    }
                    catch (Exception e)
                    {
                        mHasException = true;
                        mLastException = e;
                    }
                }
            }

            return IsSuccessFul;
        }
        /// <summary>
        /// Using C#7 Out parameter to return data from a database table into a list.
        /// </summary>
        /// <param name="Customers"></param>
        /// <returns></returns>
        public bool CustomersWithErrors3(out List<Customers> Customers)
        {
            mHasException = false;

            Customers = new List<Customers>();

            var selectStatement = "SELECT cust.CustomerIdentifier,cust.CompanyName,cust.ContactName,ct.ContactTitle, " +
                                  "cust.[Address] AS street,cust.City,cust.PostalCode,cust.Country,cust.Phone, " +
                                  "cust.ContactTypeIdentifier FROM dbo.Customers AS cust " +
                                  "INNER JOIN ContactType AS ct ON cust.ContactTypeIdentifier = ct.ContactTypeIdentifier;";


            using (var cn = new SqlConnection() { ConnectionString = ConnectionString })
            {
                using (var cmd = new SqlCommand() { Connection = cn, CommandText = selectStatement })
                {
                    try
                    {
                        cn.Open();
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Customers.Add(new Customers()
                            {
                                CustomerIdentifier = reader.GetInt32(0),
                                CompanyName = reader.GetBoolean(1).ToString(), // deliberate use of wrong method to get CompanyName
                                ContactName = reader.GetString(2),
                                ContactTitle = reader.GetString(3),
                                Street = reader.GetString(4),
                                City = reader.GetString(5),
                                PostalCode = reader.GetString(6),
                                Country = reader.GetString(7),
                                Phone = reader.GetString(8)
                            });
                        }

                    }
                    catch (Exception e)
                    {
                        mHasException = true;
                        mLastException = e;
                    }
                }
            }

            return IsSuccessFul;
        }
    }
}

