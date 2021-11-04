using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DPL.WEB.Areas.Reports.Controllers
{
    public class SalesReportController : Controller
    {
        // GET: Reports/SalesReport
        public ActionResult Index()
        {
            //return View("~/Reports/Views/SalesReport/View1.cshtml");
            return PartialView(@"~/Areas/Reports/Views/SalesReport/Index.cshtml");

        }
        public ActionResult PartialView()
        {
            //return View("~/Reports/Views/SalesReport/View1.cshtml");
            return PartialView(@"~/Areas/Reports/Views/SalesReport/PartialView.cshtml");

        }


        public ActionResult salesColStatment()
        {
            //return View("~/Reports/Views/SalesReport/View1.cshtml");
            return PartialView(@"~/Areas/Reports/Views/SalesReport/salesColStatmentView.cshtml");

        }

        public ActionResult mMSheet()
        {
            //return View("~/Reports/Views/SalesReport/View1.cshtml");
            return PartialView(@"~/Areas/Reports/Views/SalesReport/mMSheetView.cshtml");

        }

        public ActionResult finalStatment()
        {
            //return View("~/Reports/Views/SalesReport/View1.cshtml");
            return PartialView(@"~/Areas/Reports/Views/SalesReport/finalStatmentView.cshtml");

        }


    }
}