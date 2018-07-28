using System;
using System.Threading.Tasks;
using BackEndLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SolutionTest
{
    /// <summary>
    /// Disucss alternates e.g. using a ValueTuple - deconstruction, a class instance???
    /// 
    /// https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/out-parameter-modifier
    /// https://csharp-station.com/ref-parameters-vs-out-parameters
    /// 
    /// ou should generally prefer a return value over an out param. Out params are a neccissary evil 
    /// if you find yourself writing code that needs to do 2 things. A good example of this is the Try 
    /// pattern (such as Int32.TryParse).
    /// 
    /// https://www.codeproject.com/Tips/1175809/Csharp-New-Inline-Out-Variables
    /// 
    /// https://dailydotnettips.com/back-to-basics-what-is-the-difference-between-ref-and-out-keyword-in-c/
    /// 
    /// 
    /// https://social.technet.microsoft.com/wiki/contents/articles/37675.c-7-0-out-parameter.aspx
    /// </summary>
    [TestClass]
    public class OutParameterTuplesTests
    {
        /// <summary>
        /// Conventional returning of data via a DataTable where the 
        /// exception handling is done via a propery in the base exception class
        /// </summary>
        [TestMethod]
        [TestTraits(Trait.Positive)]
        public void GetCustomersDataTableWithAssertionOnExceptionFromPropertyCheck()
        {
            // arrange
            var ops = new DataOperations();
            // act
            var dt = ops.Customers1();
            // assert
            Assert.IsFalse(ops.HasException);

        }
        /// <summary>
        /// Validate using out parameters to return a DataTable where the exception
        /// assert is done on the method returning which is of type bool.
        /// </summary>
        [TestMethod]
        [TestTraits(Trait.Positive)]
        public void GetCustomerDataTablesWithAssertionFromMethodReturn()
        {
            // arrange
            var ops = new DataOperations();
            // act
            if (ops.Customers2(out var dt))
            {
                // assert
                Assert.IsTrue(dt.Rows.Count >0);
            }
        }
        /// <summary>
        /// Validate using out parameters to return a DataTable where the exception
        /// assert is done on the method returning which is of type bool.
        /// </summary>
        [TestMethod]
        [TestTraits(Trait.Negative)]
        public void GetCustomerDataTablesWithAssertionFromMethodReturn_WithException()
        {
            // arrange
            var ops = new DataOperations();
            // act
            if (ops.CustomersWithError2(out var dt))
            {
                Assert.IsTrue(dt.Rows.Count > 0);
            }
            else
            {
                // assert
                Assert.IsTrue(ops.HasException,
                    "Expected an exception from returning a DataTable");

            }
        }
        /// <summary>
        /// Here the SQL SELECT does not account for null in PostalCode which
        /// means only a sub-set of data is returned. All the other methods
        /// account for this using ISNULL(cust.PostalCode,'').
        /// </summary>
        [TestMethod]
        [TestTraits(Trait.Negative)]
        public void GetCustomerDataTablesWithAssertionFromMethodReturn_WithNullValueException()
        {
            // arrange
            var ops = new DataOperations();
            // act
            if (!ops.CustomersWithErrors3(out var customerList))
            {
                // assert - there are 91 records
                Assert.IsTrue(customerList.Count == 64,
                    "Expected records in the list");
            }
        }
        [TestMethod]
        [TestTraits(Trait.Positive)]
        public void GetSingleCustomerByIdentifierUsingOutParameter()
        {
            // arrange
            var ops = new DataOperations();
            var id = 14;

            // act
            if (ops.CustomersSingleByOutParameter(id,out var customer))
            {
                Assert.IsTrue(customer.CompanyName == "Consolidated Holdings");
            }
            else
            {
                Assert.IsTrue(1== 0, 
                    $"Expected to find customer with id of {id}");
            }
        }
        /// <summary>
        /// Get contact name and title using two out parameters
        /// </summary>
        [TestMethod]
        [TestTraits(Trait.Positive)]
        public void GetContactNameAndTitleSingleCustomerByIdentifierUsingOutParameter()
        {
            // arrange
            var ops = new DataOperations();
            var id = 14;

            // act
            if (ops.CustomerContactNameAndTitleByOutParameterDiscard(id, out var firstName, out var lastName))
            {
                Assert.IsTrue(!string.IsNullOrWhiteSpace(firstName));
            }
            else
            {
                Assert.IsTrue(1 == 0,
                    $"Expected to find customer with id of {id}");
            }
        }
        /// <summary>
        /// Get contact name where there are two out parameters, the second for title
        /// is discarded (same as ValueTuple) by replacing the variable for title with
        /// an underscore.
        /// </summary>
        [TestMethod]
        [TestTraits(Trait.Positive)]
        public void GetContactNameSingleCustomerByIdentifierUsingOutParameter()
        {
            // arrange
            var ops = new DataOperations();
            var id = 14;

            // act
            if (ops.CustomerContactNameAndTitleByOutParameterDiscard(id, out var firstName, out _))
            {
                Assert.IsTrue(!string.IsNullOrWhiteSpace(firstName));
            }
            else
            {
                Assert.IsTrue(1 == 0,
                    $"Expected to find customer with id of {id}");
            }
        }
        /// <summary>
        /// Validate no customer found and handled properly.
        /// </summary>
        [TestMethod]
        [TestTraits(Trait.Negative)]
        public void GetSingleCustomerByIdentifierUsingOutParameterNoCustomerFound()
        {
            // arrange
            var ops = new DataOperations();
            var id = 999; // table does not have CustomerIdentifier

            // act
            if (ops.CustomerSingleByOutParameterNoCustomer(id, out var customer))
            {
                Assert.IsTrue(customer.CompanyName == "Consolidated Holdings");
            }
            else
            {
                Assert.IsTrue(customer == null,
                    "Expected customer to be null");
            }
        }
        /// <summary>
        /// Validate data is returned in a list using out parameter where
        /// the exception assert is done on the value returned on the method call.
        /// </summary>
        [TestMethod]
        [TestTraits(Trait.Positive)]
        public void GetCustomersListWithAssertionFromMethodReturn()
        {
            // arrange
            var ops = new DataOperations();

            // act
            if (ops.Customers3(out var customerList))
            {
                // assert
                Assert.IsTrue(customerList.Count >0,
                    "Expected records in the list");
            }
        }
        /// <summary>
        /// To validate assertion works properly to counter test directly above
        /// </summary>
        [TestMethod]
        [TestTraits(Trait.Negative)]
        public void GetCustomersListWithAssertionFromMethodReturn_WithException()
        {
            // arrange
            var ops = new DataOperations();

            // act
            if (ops.Customers3(out var customerList))
            {
                Assert.IsTrue(customerList.Count > 0);
            }
            else
            {
                // assert
                Assert.IsTrue(ops.HasException,
                    "Expected an exception from returning a list");
            }
        }
        /// <summary>
        /// Demonstration for ValueTuple to return a customer by primary key.
        /// Discards are used as the contact name and title are not required.
        /// </summary>
        [TestMethod]
        [TestTraits(Trait.Positive)]
        public void GetSingleCustomerUsingValueTuple()
        {
            // arrange
            var ops = new DataOperations();

            var id = 1;

            // act
            // ReSharper disable once InconsistentNaming
            var (Success, contactName, contactTitle) = ops.CustomerContactNameTitleUsingTuples(id);

            // no assertion, simply showing returning data.
            Console.WriteLine($"{contactName}, {contactTitle}");
            // assert
            Assert.IsTrue(Success);
        }
        /// <summary>
        /// Negative test to the method above for validation
        /// Discards are used as the contact name and title are not required.
        /// </summary>
        [TestMethod]
        [TestTraits(Trait.Negative)]
        public void GetSingleCustomerUsingValueTupleNotFound()
        {
            // arrange
            var ops = new DataOperations();
            var id = 991; // CustomerIdentifier does not exist

            // act
            // ReSharper disable once InconsistentNaming
            var (Success, _, _) = ops.CustomerContactNameTitleUsingTuples(id);

            // assert
            Assert.IsFalse(Success);
        }
        /// <summary>
        /// Positive test to return a named tuple of bool, list of Customer.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [TestTraits(Trait.Positive)]
        public async Task GetCustomersWithTupleAsync()
        {
            // arrange
            var ops = new DataOperations();

            // act
            var resultsValueTuple = await ops.GetSCustomersUsingTuplesAsync()
                .ConfigureAwait(false);

            // assert
            Assert.IsTrue(resultsValueTuple.Success,
                "Expected async to function to return customers");

            Assert.IsTrue(resultsValueTuple.Customers.Count == 91,
                "Expected 91 customers");
        }
    }
}
