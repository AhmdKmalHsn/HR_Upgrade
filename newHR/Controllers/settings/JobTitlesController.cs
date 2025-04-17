using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using newHR.Models;

namespace newHR.Controllers.settings
{
    public class JobTitlesController : Controller
    {
        private HrEntities1 db = new HrEntities1();

        // GET: JobTitles
        public async Task<ActionResult> Index()
        {
            return View(await db.JobTitles.ToListAsync());
        }

        // GET: JobTitles/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobTitle jobTitle = await db.JobTitles.FindAsync(id);
            if (jobTitle == null)
            {
                return HttpNotFound();
            }
            return View(jobTitle);
        }

        // GET: JobTitles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: JobTitles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,IsCreated,IsUpdated")] JobTitle jobTitle)
        {
            if (ModelState.IsValid)
            {
                db.JobTitles.Add(jobTitle);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(jobTitle);
        }

        // GET: JobTitles/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobTitle jobTitle = await db.JobTitles.FindAsync(id);
            if (jobTitle == null)
            {
                return HttpNotFound();
            }
            return View(jobTitle);
        }

        // POST: JobTitles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,IsCreated,IsUpdated")] JobTitle jobTitle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jobTitle).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(jobTitle);
        }

        // GET: JobTitles/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobTitle jobTitle = await db.JobTitles.FindAsync(id);
            if (jobTitle == null)
            {
                return HttpNotFound();
            }
            return View(jobTitle);
        }

        // POST: JobTitles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            JobTitle jobTitle = await db.JobTitles.FindAsync(id);
            db.JobTitles.Remove(jobTitle);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
