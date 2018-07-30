<p align="left">
    <a href="https://social.technet.microsoft.com/profile/kareninstructor/">
    <img alt="logo" src="Assets/Karen.png">
    </a>
</p>

<p><a href="https://social.technet.microsoft.com/profile/kareninstructor/">TechNet: C# 7 new methods for returning data with out parameters and ValueTuple</a></p>

<p>ValueTuple</p>
'''csharp
public async Task<(bool Success, List<Customer> Customers)> GetSCustomersUsingTuplesAsync()
{
    mHasException = false;

    var results = await Task.Run(() =>
        {
            mHasException = false;

            var customersList = new List<Customer>();

            const string selectStatement =
                "SELECT cust.CustomerIdentifier,cust.CompanyName,cust.ContactName,ct.ContactTitle, " +
                "cust.[Address] AS street,cust.City,ISNULL(cust.PostalCode,''),cust.Country,cust.Phone, " +
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
                            customersList.Add(new Customer()
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

            // IsSuccessFul equates to returning all data, no data or partial data
            // customerList will contain, no customers, some customers, all customers
            return (IsSuccessFul, customersList);

        }).ConfigureAwait(false);

    return results;
}
'''
<p>Out parameter</p>
'''csharp
public bool Customers3(out List<Customer> Customers)
{
    mHasException = false;

    Customers = new List<Customer>();

    const string selectStatement = 
        "SELECT cust.CustomerIdentifier,cust.CompanyName,cust.ContactName,ct.ContactTitle, " +
        "cust.[Address] AS street,cust.City,ISNULL(cust.PostalCode,''),cust.Country,cust.Phone, " +
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
                    Customers.Add(new Customer()
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
'''