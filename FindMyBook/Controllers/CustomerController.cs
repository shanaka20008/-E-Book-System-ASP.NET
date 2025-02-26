using FindMyBook.Models;
using FindMyBook.ViewModels;
using System.Linq;
using System.Web.Mvc;

namespace FindMyBook.Controllers
{
    public class CustomerController : Controller
    {
        private findMyBookEntities1 db = new findMyBookEntities1();

        // GET: Customer
        public ActionResult Index()
        {
            var customerDetails = db.table_customer.Select(c => new CustomerViewModel
            {
                CustomerId = c.customer_id,
                FirstName = c.customer_first_name,
                LastName = c.customer_last_name,
                Address = c.customer_address,
                Username = c.customer_username,
                Email = c.email,
                PhoneNumber = c.customer_phone_number,
                AccountCreationDate = c.account_creation_date,
                ProfileImage = c.customer_image
            }).ToList();

            return View(customerDetails);
        }

        // GET: Customer/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var customer = db.table_customer.Find(id);
            if (customer == null)
            {
                return HttpNotFound(); // This will return a 404 if the customer is not found
            }

            var viewModel = new CustomerViewModel
            {
                CustomerId = customer.customer_id,
                FirstName = customer.customer_first_name,
                LastName = customer.customer_last_name,
                Address = customer.customer_address,
                Username = customer.customer_username,
                Email = customer.email,
                PhoneNumber = customer.customer_phone_number,
                AccountCreationDate = customer.account_creation_date,
                ProfileImage = customer.customer_image
            };

            return View(viewModel);
        }

        // POST: Customer/Edit/5
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Edit(CustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var customer = db.table_customer.Find(model.CustomerId);
                if (customer == null)
                {
                    return HttpNotFound(); // Return 404 if the customer is not found
                }

                customer.customer_first_name = model.FirstName;
                customer.customer_last_name = model.LastName;
                customer.customer_address = model.Address;
                customer.customer_username = model.Username;
                customer.email = model.Email;
                customer.customer_phone_number = model.PhoneNumber;
                customer.confirm_password = "12345678";
                customer.customer_password = "12345678";

                db.SaveChanges();
                return RedirectToAction("Index"); // Redirect to the customer list after editing
            }

            return View(model); // Return the view with the model if validation fails
        }

        // GET: Customer/Delete/5
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var customer = db.table_customer.Find(id);
            if (customer == null)
            {
                return HttpNotFound(); // Return 404 if the customer is not found
            }

            var viewModel = new CustomerViewModel
            {
                CustomerId = customer.customer_id,
                FirstName = customer.customer_first_name,
                LastName = customer.customer_last_name,
                Address = customer.customer_address,
                Username = customer.customer_username,
                Email = customer.email,
                PhoneNumber = customer.customer_phone_number,
                AccountCreationDate = customer.account_creation_date,
                ProfileImage = customer.customer_image
            };

            return View(viewModel); // Return the confirmation view with the customer details
        }

        // POST: Customer/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var customer = db.table_customer.Find(id);
            if (customer == null)
            {
                return HttpNotFound(); // Return 404 if the customer is not found
            }

            db.table_customer.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index"); // Redirect to the customer list after deletion
        }

        // POST: Customer/Delete/5
        // [HttpPost, ActionName("Delete")]
        // [ValidateAntiForgeryToken]
        // public ActionResult DeleteConfirmed(int id)
        // {
        //     var customer = db.table_customer.Find(id);
        //     if (customer == null)
        //     {
        //         return HttpNotFound(); // Return 404 if the customer is not found
        //     }

        //     db.table_customer.Remove(customer);
        //     db.SaveChanges();
        //     return RedirectToAction("Index"); // Redirect to the customer list after deletion
        // }
    }
}