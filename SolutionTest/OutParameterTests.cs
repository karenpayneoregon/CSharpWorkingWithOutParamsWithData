using System;
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
    /// </summary>
    [TestClass]
    public class OutParameterTests
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
                Assert.IsTrue(1== 0, $"Expected to find customer with id of {id}");
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
            var id = 999;
            // act
            if (ops.CustomersSingleByOutParameterNoCustomer(id, out var customer))
            {
                Assert.IsTrue(customer.CompanyName == "Consolidated Holdings");
            }
            else
            {
                Assert.IsTrue(customer == null,"Expected customer to be null");
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
    }
}
