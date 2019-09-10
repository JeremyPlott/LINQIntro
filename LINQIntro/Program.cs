using System;
using System.Collections.Generic;
using System.Linq;
using CSharpToSQLLibrary;

namespace LINQIntro {
    //class User {
    //    public string Name { get; set; }
    //    public bool IsAdmin { get; set; }
    //}
    class State {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    class Program {

        void Run() {
            var connection = new Connection(@"localhost\sqlexpress", "PRSdb");
            connection.Open();
            Users.Connection = connection;
            Vendors.Connection = connection;
            Products.Connection = connection;


            var statecodes = new State[] { //Example of SQL only having state codes, and outputting a different string using C# only
                new State() { Code = "WA", Name = "Washington" },
                new State() { Code = "XX", Name = "Outer Space"}
            };

            var vendorswithstates = from vend in Vendors.GetAll()
                                    join sta in statecodes
                                    on vend.State equals sta.Code
                                    select new {
                                        Vendor = vend.Name, State = sta.Name
                                    };
            foreach(var vs in vendorswithstates) {
                Console.WriteLine($"Vendor {vs.Vendor} is in {vs.State}");
            }


            var admins = from usr in Users.GetAll()
                         where (bool)usr.IsAdmin
                         select usr;

            var userName = from usr in Users.GetAll()
                       where usr.Username.Equals("NotAGiraffe") // .Equals is recommended over == for comparing strings.
                     //where usr.Username.Contains("Giraffe")
                       select usr;

            var reviewers = from usr in Users.GetAll()
                           where (bool)usr.IsReviewer                                                             
                           select usr;

            var vendors = from vend in Vendors.GetAll()
                          orderby vend.Name
                          select vend;

            var venCode = from vend in Vendors.GetAll()
                          where vend.Code.Equals("DNRUS")
                          select vend;

            var priceSum = (from prod in Products.GetAll()
                           select prod.Price).Sum();
            //Products.GetAll().Sum(prod => prod.Price)  || alternative method with lambda. More efficient for program performance?
            //Products.GetAll().Where(prod => prod.Price < 50)
            //Products.GetAll().Where(prod => prod.Price < 50).Sum(prod => prod.Price) // linq expressions can be chained together


            var products = from prod in Products.GetAll()
                           join vend in Vendors.GetAll()
                           on prod.VendorId equals vend.Id
                           select new {
                               Product = prod.Name, //name of column with data to populate
                               Price = prod.Price,
                               Vendor = vend.Name
                           };


            foreach (var user in userName) {
                Console.WriteLine($"Found user {user.Username} || {user}");
            }

            foreach(var user in admins) {
                Console.WriteLine($"{user.Firstname} {user.Lastname} is an admin");
            }

            foreach (var user in reviewers) {
                Console.WriteLine($"{user.Firstname} {user.Lastname} is a reviewer");
            }
            Console.WriteLine();


            foreach (var ven in vendors) {
                Console.WriteLine($"{ven} is a vendor");
            }

            foreach (var ven in venCode) {
                Console.WriteLine($"{ven} is a vendor");
            }

            Console.WriteLine($"The sum of all product prices is ${priceSum}");
            Console.WriteLine();

            foreach(var prod in products) {
                Console.WriteLine($"{prod.Product} priced at ${prod.Price} is from {prod.Vendor}");
            }
            

                connection.Close();
        }

        static void Main(string[] args) {

            var pgm = new Program();
            pgm.Run();

            //var users = new User[] {
            //    new User() { Name = "Adam", IsAdmin = false },
            //    new User() { Name = "Barb", IsAdmin = true },
            //    new User() { Name = "Chris", IsAdmin = false },
            //    new User() { Name = "Donna", IsAdmin = true },
            //    new User() { Name = "Ed", IsAdmin = false },
            //    new User() { Name = "Faith", IsAdmin = true },
            //};

            //// query syntax
            ////var admins = from usr in users
            ////             where usr.IsAdmin //(where usr.IsAdmin == true) is like running around with a sign that says "I'm an amateur"
            ////             orderby user.Name descending
            ////             select usr;

            ////method syntax with lambda
            //var admins = users.Where(usr => usr.IsAdmin).OrderByDescending(usr => usr.Name);

            //foreach(var usr in admins) {
            //    Console.WriteLine($"{usr.Name} is an admin");
            //}

            //// query syntax - recommended if possible to use
            //// method syntax
            //// some things can't be done in query syntax, but method syntax is a little more cryptic.

            //int[] ints = { 2, 4, 6, 8, 7, 5, 3, 1 };

            ////query syntax
            ////var ascInts = from i in ints //this defines query, but doesn't execute until the ascInts variable is used
            ////              where i % 2 == 1 && i < 7
            ////              orderby i descending // ascending is default. 
            ////              select i;

            ////method syntax
            //var ascInts = (from i in ints //this defines query, but doesn't execute until the ascInts variable is used
            //               where i % 2 == 1 && i < 7
            //               orderby i descending // ascending is default. 
            //               select i);

            //var avg = ascInts.Average(i => i);

            //Console.WriteLine($"Average of odds < 7 is {avg}");

            //// lambda syntax. (x => x.firstname == "greg") => is one character, the fat arrow. foreach x in 
            //// var users = new List<user>; -> (user => user.Firstname == "Greg")
            //// lamba syntax that functions as a foreach through a list

            //foreach (var i in ascInts) {
            //    Console.Write($"{i} ");
            //}
        }
    }
}
