using FindMyBook.Models;
using FindMyBook.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace FindMyBook.Controllers
{
    public class BookController : Controller
    {
        findMyBookEntities1 db = new findMyBookEntities1();

        //private void FetchBookDetails()
        //{
        //    var bookDetails = (from book in db.table_book_detail
        //                       join author in db.table_author on book.author_id_FK equals author.author_id
        //                       join publisher in db.table_publisher on book.publisher_id_FK equals publisher.publisher_id
        //                       join status in db.table_book_status on book.book_status_id_FK equals status.book_status_id
        //                       select new BookViewModel
        //                       {
        //                           BookId = book.book_id,
        //                           Title = book.book_name,
        //                           ISBN = book.book_isbn_number,
        //                           BookLanguage = book.book_language,
        //                           Price = book.book_price,
        //                           Pages = book.pages,
        //                           PublishedDate = book.book_published_date,
        //                           Rating = book.rating,
        //                           AuthorName = author.author_name,
        //                           PublisherName = publisher.publisher_name,
        //                           Status = status.book_status,
        //                           BookImage = book.book_image // Include book image in the view model
        //                       }).ToList();
        //}

        // GET: Book
        public ActionResult Index()
        {
            var bookDetails = (from book in db.table_book_detail
                               join author in db.table_author on book.author_id_FK equals author.author_id
                               join publisher in db.table_publisher on book.publisher_id_FK equals publisher.publisher_id
                               join status in db.table_book_status on book.book_status_id_FK equals status.book_status_id
                               select new BookViewModel
                               {
                                   BookId = book.book_id,
                                   Title = book.book_name,
                                   ISBN = book.book_isbn_number,
                                   BookLanguage = book.book_language,
                                   Price = book.book_price,
                                   Pages = book.pages,
                                   PublishedDate = book.book_published_date,
                                   Rating = book.rating,
                                   AuthorName = author.author_name,
                                   PublisherName = publisher.publisher_name,
                                   Status = status.book_status,
                                   BookImage = book.book_image // Include book image in the view model
                               }).ToList();

            return View(bookDetails);
        }


        [HttpGet]
        public ActionResult AddBook()
        {
            ViewBag.Authors = new SelectList(db.table_author.ToList(), "author_id", "author_name");
            ViewBag.Publishers = new SelectList(db.table_publisher.ToList(), "publisher_id", "publisher_name");
            ViewBag.BookStatus = new SelectList(db.table_book_status.ToList(), "book_status_id", "book_status");
            return View();
        }


        [HttpPost]
        public ActionResult AddBook(table_book_detail table_Book_Detail)
        {
            try
            {
                if (db.table_book_detail.Any(bookDetail => bookDetail.book_isbn_number == table_Book_Detail.book_isbn_number))
                {
                    return Json(new { status = "409", message = "The ISBN number already exist!" });
                    //return View();

                }
                else
                {
                    string fileName = Path.GetFileNameWithoutExtension(table_Book_Detail.BookImageFile.FileName);
                    string extension = Path.GetExtension(table_Book_Detail.BookImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    table_Book_Detail.book_image = "../BookImages/" + fileName;
                    fileName = Path.Combine(Server.MapPath("../BookImages/"), fileName);
                    table_Book_Detail.BookImageFile.SaveAs(fileName);


                    db.table_book_detail.Add(table_Book_Detail);
                    db.SaveChanges();

                    ModelState.Clear();

                    return RedirectToAction("AddBook", "Book");
                    //return Json(new { status = "200", message = "The book detail has been saved successfully." });

                }
            }
            catch (Exception ex)
            {
                ViewBag.Notification = "An error occurred: " + ex.Message;


                //return View(table_Book_Detail);
            }

            ViewBag.Authors = new SelectList(db.table_author.ToList(), "author_id", "author_name", table_Book_Detail.author_id_FK);
            ViewBag.Publishers = new SelectList(db.table_publisher.ToList(), "publisher_id", "publisher_name", table_Book_Detail.publisher_id_FK);
            ViewBag.BookStatus = new SelectList(db.table_book_status.ToList(), "book_status_id", "book_status", table_Book_Detail.book_status_id_FK);

            return View(table_Book_Detail);
        }


        // Find Books
        public ActionResult FindBooks()
        {

                var bookDetails = (from book in db.table_book_detail
                                   join author in db.table_author on book.author_id_FK equals author.author_id
                                   join publisher in db.table_publisher on book.publisher_id_FK equals publisher.publisher_id
                                   join status in db.table_book_status on book.book_status_id_FK equals status.book_status_id
                                   select new BookViewModel
                                   {
                                       BookId = book.book_id,
                                       Title = book.book_name,
                                       ISBN = book.book_isbn_number,
                                       BookLanguage = book.book_language,
                                       Price = book.book_price,
                                       Pages = book.pages,
                                       PublishedDate = book.book_published_date,
                                       Rating = book.rating,
                                       AuthorName = author.author_name,
                                       PublisherName = publisher.publisher_name,
                                       Status = status.book_status,
                                       BookImage = book.book_image // Include book image in the view model
                                   }).ToList();
            return View(bookDetails);
        }

        public ActionResult Details(int id)
        {
            var bookDetails = (from book in db.table_book_detail
                               join author in db.table_author on book.author_id_FK equals author.author_id
                               join publisher in db.table_publisher on book.publisher_id_FK equals publisher.publisher_id
                               join status in db.table_book_status on book.book_status_id_FK equals status.book_status_id
                               where book.book_id == id
                               select new BookViewModel
                               {
                                   BookId = book.book_id,
                                   Title = book.book_name,
                                   ISBN = book.book_isbn_number,
                                   BookLanguage = book.book_language,
                                   Price = book.book_price,
                                   Pages = book.pages,
                                   PublishedDate = book.book_published_date,
                                   Rating = book.rating,
                                   AuthorName = author.author_name,
                                   PublisherName = publisher.publisher_name,
                                   Status = status.book_status,
                                   BookImage = book.book_image // Include book image in the view model
                               }).FirstOrDefault();
            return View(bookDetails);
        }

        // Add these methods to your BookController class

        [HttpGet]
        public ActionResult EditBook(int id)
        {
            var book = db.table_book_detail.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }

            ViewBag.Authors = new SelectList(db.table_author.ToList(), "author_id", "author_name", book.author_id_FK);
            ViewBag.Publishers = new SelectList(db.table_publisher.ToList(), "publisher_id", "publisher_name", book.publisher_id_FK);
            ViewBag.BookStatus = new SelectList(db.table_book_status.ToList(), "book_status_id", "book_status", book.book_status_id_FK);

            return View(book);
        }

        [HttpPost]
        public ActionResult EditBook(table_book_detail updatedBook, HttpPostedFileBase newImage)
        {
            try
            {
                var existingBook = db.table_book_detail.Find(updatedBook.book_id);
                if (existingBook == null)
                {
                    return HttpNotFound();
                }

                // Check if ISBN exists for other books
                var isbnExists = db.table_book_detail
                    .Any(b => b.book_isbn_number == updatedBook.book_isbn_number && b.book_id != updatedBook.book_id);
                if (isbnExists)
                {
                    ModelState.AddModelError("book_isbn_number", "This ISBN already exists for another book.");
                    PrepareViewBagForEdit(updatedBook);
                    return View(updatedBook);
                }

                // Handle new image upload if provided
                if (newImage != null && newImage.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(newImage.FileName);
                    string extension = Path.GetExtension(newImage.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = "../BookImages/" + fileName;
                    fileName = Path.Combine(Server.MapPath("../BookImages/"), fileName);
                    newImage.SaveAs(fileName);

                    // Delete old image if it exists
                    if (!string.IsNullOrEmpty(existingBook.book_image))
                    {
                        string oldImagePath = Server.MapPath(existingBook.book_image);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    existingBook.book_image = path;
                }

                // Update other properties
                existingBook.book_name = updatedBook.book_name;
                existingBook.book_isbn_number = updatedBook.book_isbn_number;
                existingBook.book_language = updatedBook.book_language;
                existingBook.book_price = updatedBook.book_price;
                existingBook.pages = updatedBook.pages;
                existingBook.book_published_date = updatedBook.book_published_date;
                existingBook.rating = updatedBook.rating;
                existingBook.author_id_FK = updatedBook.author_id_FK;
                existingBook.publisher_id_FK = updatedBook.publisher_id_FK;
                existingBook.book_status_id_FK = updatedBook.book_status_id_FK;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the book: " + ex.Message);
                PrepareViewBagForEdit(updatedBook);
                return View(updatedBook);
            }
        }

        private void PrepareViewBagForEdit(table_book_detail book)
        {
            ViewBag.Authors = new SelectList(db.table_author.ToList(), "author_id", "author_name", book.author_id_FK);
            ViewBag.Publishers = new SelectList(db.table_publisher.ToList(), "publisher_id", "publisher_name", book.publisher_id_FK);
            ViewBag.BookStatus = new SelectList(db.table_book_status.ToList(), "book_status_id", "book_status", book.book_status_id_FK);
        }

        [HttpGet]
        public ActionResult DeleteBook(int id)
        {
            var book = db.table_book_detail.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }

            var bookViewModel = new BookViewModel
            {
                BookId = book.book_id,
                Title = book.book_name,
                ISBN = book.book_isbn_number,
                BookLanguage = book.book_language,
                Price = book.book_price,
                Pages = book.pages,
                PublishedDate = book.book_published_date,
                Rating = book.rating,
                BookImage = book.book_image,
                AuthorName = db.table_author.Find(book.author_id_FK)?.author_name,
                PublisherName = db.table_publisher.Find(book.publisher_id_FK)?.publisher_name,
                Status = db.table_book_status.Find(book.book_status_id_FK)?.book_status
            };

            return View(bookViewModel);
        }

        [HttpPost, ActionName("DeleteBook")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteBookConfirmed(int id)
        {
            try
            {
                var book = db.table_book_detail.Find(id);
                if (book == null)
                {
                    return HttpNotFound();
                }

                // Delete the associated image file
                if (!string.IsNullOrEmpty(book.book_image))
                {
                    string imagePath = Server.MapPath(book.book_image);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                db.table_book_detail.Remove(book);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while deleting the book: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

    }
}