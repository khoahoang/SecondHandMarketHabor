using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TraoDoiDoCu.Models;

namespace TraoDoiDoCu.Controllers.Product
{
    public class ProductController : Controller
    {
        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            using(TraoDoiDoCuEntities td = new TraoDoiDoCuEntities())
            {
                var product = td.Products
                                .Where(p => p.ID == id)
                                .FirstOrDefault();
                if (product != null)
                {
                    ViewBag.PosterName = product.User.LastName + " " +product.User.FirstName;
                    ViewBag.Rated = product.User.Rating;
                    ViewBag.Phone = product.User.Phone;
                    return View(product);
                }
            }
            return HttpNotFound();
        }
    }
}
