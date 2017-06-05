using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DISOpenDataCloud.Controllers
{
    public class DataScopeController : Controller
    {
        // GET: DataScope
        public ActionResult Index()
        {
            return View();
        }

        // GET: DataScope/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DataScope/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DataScope/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: DataScope/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DataScope/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: DataScope/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DataScope/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
