using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FindMyBook.Models;

namespace FindMyBook.Controllers
{
    public class ReportsController : Controller
    {
        private findMyBookEntities1 db = new findMyBookEntities1();

        // GET: Reports/SalesReport
        public ActionResult SalesReport(DateTime? startDate, DateTime? endDate, string bookName)
        {
            var salesReportQuery = db.table_customer_order_book
                .Join(db.table_book_detail,
                      order => order.order_id,
                      book => book.book_id,
                      (order, book) => new SalesReportViewModel
                      {
                          OrderId = order.order_id,
                          OrderDate = order.order_date,
                          BookName = book.book_name,
                          BookPrice = book.book_price
                      });

            if (startDate.HasValue)
            {
                salesReportQuery = salesReportQuery.Where(sr => sr.OrderDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                salesReportQuery = salesReportQuery.Where(sr => sr.OrderDate <= endDate.Value);
            }

            if (!string.IsNullOrEmpty(bookName))
            {
                salesReportQuery = salesReportQuery.Where(sr => sr.BookName.Contains(bookName));
            }

            var salesReport = salesReportQuery.ToList();
            return View(salesReport);
        }

        // GET: Reports/CustomerReport
        public ActionResult CustomerReport(string customerName, string email)
        {
            var customerReportQuery = db.table_customer.AsQueryable();

            if (!string.IsNullOrEmpty(customerName))
            {
                customerReportQuery = customerReportQuery.Where(c =>
                    (c.customer_first_name + " " + c.customer_last_name).Contains(customerName));
            }

            if (!string.IsNullOrEmpty(email))
            {
                customerReportQuery = customerReportQuery.Where(c => c.email.Contains(email));
            }

            var customerReport = customerReportQuery.Select(customer => new CustomerReportViewModel
            {
                CustomerId = customer.customer_id,
                CustomerName = customer.customer_first_name + " " + customer.customer_last_name,
                Email = customer.email,
                PhoneNumber = customer.customer_phone_number
            }).ToList();

            // Data for chart (example: percentage of verified emails)
            ViewBag.EmailVerifiedCount = customerReport.Count(c => !string.IsNullOrEmpty(c.Email)); // Example logic
            ViewBag.EmailNotVerifiedCount = customerReport.Count(c => string.IsNullOrEmpty(c.Email));

            return View(customerReport);
        }

        // GET: Reports/InventoryReport
        public ActionResult InventoryReport(string searchTerm, string sortOrder)
        {
            var books = db.table_book_detail.AsQueryable();

            // Apply search filter
            if (!string.IsNullOrEmpty(searchTerm))
            {
                books = books.Where(b => b.book_name.Contains(searchTerm) || b.book_isbn_number.Contains(searchTerm));
            }

            // Sort books based on price
            switch (sortOrder)
            {
                case "Price_Asc":
                    books = books.OrderBy(b => b.book_price);
                    break;
                case "Price_Desc":
                    books = books.OrderByDescending(b => b.book_price);
                    break;
                default:
                    books = books.OrderBy(b => b.book_name); // Default sorting by book name
                    break;
            }

            var inventoryReport = books.Select(book => new InventoryReportViewModel
            {
                BookId = book.book_id,
                BookName = book.book_name,
                ISBN = book.book_isbn_number,
                Price = book.book_price
            }).ToList();

            return View(inventoryReport);
        }
    }
}
