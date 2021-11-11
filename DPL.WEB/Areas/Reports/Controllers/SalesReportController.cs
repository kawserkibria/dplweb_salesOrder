using DPL.WEB.ACCMS;
using DPL.WEB.Models;
using Dutility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace DPL.WEB.Areas.Reports.Controllers
{
    public class SalesReportController : Controller
    {
        ACCMS.SWJAGClient accms = new SWJAGClient();
        EXTRA.SWPRJClient objExtra = new EXTRA.SWPRJClient();
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
        public ActionResult SalesCollectionPerformance()
        {
            string User = "";
            if (Session["USERID"] != null && Session["USERID"].ToString() != "")
            {
                if (Session["userLevel"].ToString() == "0")
                {
                    User = "Admin";
                }
                else
                {
                    User = "User";
                }

                var ddd = Session["USERID"].ToString();
                ViewBag.MName = Session["USERID"].ToString();


                var dddgg = User + ":" + Session["USERID"].ToString();

                ViewBag.MNameMerz = User + ":" + Session["USERID"].ToString();


                return View(@"~/Areas/Reports/Views/SalesReport/SalesCollectionPerformance.cshtml");
            }
            else
            {
                return RedirectToAction("LogIn", "LogIn", new { Area = "", returnUrl = UrlParameter.Optional });
            }
        }
        
        public ActionResult mGetBranch()
        {
           var branchName = accms.mfillBranchNew("0003", Utility.gblnAccessControl, Utility.gstrUserName).ToList();
           return Json(branchName, JsonRequestBehavior.AllowGet);
        }

        public ActionResult mGetLedger(string strBranchId, int ledgerType, int intStatus,string gstrUserName)
        {
            //var strBranchId = Utility.gstrGetBranchID("0003", strBranchName);
            var allLedger = objExtra.mGetLedgerGroupLoad("0003", ledgerType, gstrUserName, intStatus, strBranchId).ToList();
            return Json(allLedger, JsonRequestBehavior.AllowGet);
        }
        public ActionResult salesColStatment()
        {
  

           
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
        public void RptListView(string strVoucherNO, string vstrLedgerName)
        {

            //ReportQuery obj = new ReportQuery();
            List<RProductSales> list = null;
            List<RProductSales> SubReportDetails = null;

            //string  strLedgerName= Utility.
            string Rname = "OrderView";

            list = mGetSalesInvoiceReportPriview("0003", strVoucherNO, vstrLedgerName).ToList();
            SubReportDetails = mGetSalesInvoiceReportPriviewSubReport("0003", strVoucherNO).ToList();

            HttpContext.Session["ReportDataList"] = list;
            //HttpContext.Session["SubReportDataList"] = SubReportDetails;
            HttpContext.Session["reportname"] = Rname.ToLower().Trim();
            Response.Redirect("~/Reports/report-viewer.aspx", false);
        }

        public List<RProductSales> mGetSalesInvoiceReportPriview(string strDeComID, string strcomRef, string vstrLedgerName)
        {
            string strSQL = null;
            int intMode = 0;
            string connectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection gcnMain = new SqlConnection(connectionString))
            {
                if (gcnMain.State == ConnectionState.Open)
                {
                    gcnMain.Close();
                }
                gcnMain.Open();
                SqlDataReader dr;

                List<RProductSales> ooAccLedger = new List<RProductSales>();
                if (vstrLedgerName == "")
                {
                    strSQL = "SELECT ACC_LEDGER.LEDGER_NAME_MERZE  FROM ACC_LEDGER,ACC_COMPANY_VOUCHER WHERE  ACC_LEDGER.LEDGER_NAME =ACC_COMPANY_VOUCHER.LEDGER_NAME AND ACC_COMPANY_VOUCHER.COMP_REF_NO =" + strcomRef + " ";
                    SqlCommand cmd1 = new SqlCommand(strSQL, gcnMain);
                    dr = cmd1.ExecuteReader();
                    if (dr.Read())
                    {
                        vstrLedgerName = dr["LEDGER_NAME_MERZE"].ToString();
                    }
                    dr.Close();
                }
                else if (vstrLedgerName == null)
                {
                    strSQL = "SELECT ACC_LEDGER.LEDGER_NAME_MERZE  FROM ACC_LEDGER,ACC_COMPANY_VOUCHER WHERE  ACC_LEDGER.LEDGER_NAME =ACC_COMPANY_VOUCHER.LEDGER_NAME AND ACC_COMPANY_VOUCHER.COMP_REF_NO =" + strcomRef + " ";
                    SqlCommand cmd1 = new SqlCommand(strSQL, gcnMain);
                    dr = cmd1.ExecuteReader();
                    if (dr.Read())
                    {
                        vstrLedgerName = dr["LEDGER_NAME_MERZE"].ToString();
                    }
                    dr.Close();
                }
                SqlCommand cmdUpdate = new SqlCommand();
                string strcopy = "";
                cmdUpdate.Connection = gcnMain;
                strSQL = "UPDATE ACC_COMPANY_VOUCHER SET INTCOPY=INTCOPY+ 1 ";
                strSQL = strSQL + "WHERE (COMP_REF_NO = '" + strcomRef + "') ";
                strSQL = strSQL + "AND COMP_VOUCHER_TYPE=16 ";
                cmdUpdate.CommandText = strSQL;
                cmdUpdate.ExecuteNonQuery();

                strSQL = "SELECT INTCOPY FROM ACC_COMPANY_VOUCHER ";
                strSQL = strSQL + "WHERE (COMP_REF_NO = '" + strcomRef + "') ";
                strSQL = strSQL + "AND COMP_VOUCHER_TYPE=16 ";
                cmdUpdate.CommandText = strSQL;
                dr = cmdUpdate.ExecuteReader();
                if (dr.Read())
                {
                    if (Convert.ToInt32(dr["INTCOPY"].ToString()) > 1)
                    {
                        strcopy = "Duplicate";
                    }
                    else
                    {
                        strcopy = "Original";
                    }
                }
                dr.Close();
                strSQL = "SELECT  substring(ACC_COMPANY_VOUCHER.COMP_REF_NO,7,30) COMP_REF_NO ,'" + vstrLedgerName + "' LEDGER_NAME, ACC_COMPANY_VOUCHER.COMP_VOUCHER_DATE, ACC_COMPANY_VOUCHER.COMP_VOUCHER_NET_AMOUNT, ";
                strSQL = strSQL + "ACC_COMPANY_VOUCHER.COMP_VOUCHER_DUE_DATE, ACC_COMPANY_VOUCHER.ORDER_NO, ACC_COMPANY_VOUCHER.ORDER_DATE, ACC_BILL_TRAN.BILL_QUANTITY, ACC_BILL_TRAN.BILL_QUANTITY_BONUS, ";
                strSQL = strSQL + "ACC_BILL_TRAN.BILL_AMOUNT, INV_STOCKITEM.STOCKITEM_NAME, INV_STOCKITEM.POWER_CLASS, INV_STOCKITEM.STOCKGROUP_NAME, INV_STOCKITEM.STOCKCATEGORY_NAME, ";
                strSQL = strSQL + "ACC_LEDGER.LEDGER_NAME_MERZE, ACC_LEDGER.LEDGER_ADDRESS1, ACC_LEDGER.LEDGER_ADDRESS2, ACC_COMPANY_VOUCHER.PREPARED_BY, ACC_COMPANY_VOUCHER.SALES_REP,  ";
                strSQL = strSQL + "ACC_LEDGER_1.LEDGER_CODE +'-' + ACC_LEDGER_1.LEDGER_NAME as Sales_Rep,ACC_LEDGER_1.HOMOEO_HALL  +',' +  ACC_LEDGER.LEDGER_ADDRESS1  as LEDGER_ADDRESS11 ,";
                strSQL = strSQL + "ACC_BILL_TRAN.BILL_RATE, ACC_BILL_TRAN.G_COMM_PER, ACC_LEDGER_1.LEDGER_NAME_MERZE AS Sales_Rep , ACC_LEDGER.TERITORRY_CODE, ACC_LEDGER.TERRITORRY_NAME, ";
                strSQL = strSQL + "ACC_COMPANY_VOUCHER.PREPARED_DATE,ACC_COMPANY_VOUCHER.ONLINE,ACC_COMPANY_VOUCHER.COMP_VOUCHER_NARRATION ";
                strSQL = strSQL + "FROM ACC_COMPANY_VOUCHER AS ACC_COMPANY_VOUCHER INNER JOIN ";
                strSQL = strSQL + "ACC_BILL_TRAN AS ACC_BILL_TRAN ON ACC_COMPANY_VOUCHER.COMP_REF_NO = ACC_BILL_TRAN.COMP_REF_NO INNER JOIN ";
                strSQL = strSQL + "INV_STOCKITEM AS INV_STOCKITEM ON ACC_BILL_TRAN.STOCKITEM_NAME = INV_STOCKITEM.STOCKITEM_NAME FULL OUTER JOIN ";
                strSQL = strSQL + "ACC_LEDGER ON ACC_COMPANY_VOUCHER.SALES_REP = ACC_LEDGER.LEDGER_NAME FULL OUTER JOIN ";
                strSQL = strSQL + "ACC_LEDGER AS ACC_LEDGER_1 ON ACC_COMPANY_VOUCHER.SALES_REP = ACC_LEDGER_1.LEDGER_NAME ";
                if (strcomRef != "")
                {
                    strSQL = strSQL + "WHERE (ACC_COMPANY_VOUCHER.COMP_REF_NO = '" + strcomRef + "') ";
                }
                strSQL = strSQL + "ORDER BY INV_STOCKITEM.STOCKITEM_NAME ";
                SqlCommand cmd = new SqlCommand(strSQL, gcnMain);
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    RProductSales oLedg = new RProductSales();
                    oLedg.strRefNo = dr["COMP_REF_NO"].ToString();
                    //oLedg.strLedgername = dr["LEDGER_NAME"].ToString();
                    oLedg.strLedgername = vstrLedgerName.Replace("'", "");
                    oLedg.strVoucheDate = Convert.ToDateTime(dr["COMP_VOUCHER_DATE"]).ToString("dd-MM-yyyy");
                    oLedg.DblVNetAmt = Convert.ToDouble(dr["COMP_VOUCHER_NET_AMOUNT"].ToString());
                    if (dr["PREPARED_DATE"].ToString() != "")
                    {
                        oLedg.strVDDate = Convert.ToDateTime(dr["PREPARED_DATE"]).ToString("dd-MM-yyyy");
                    }
                    else
                    {
                        oLedg.strVDDate = "";
                    }
                    oLedg.strSALESREP = dr["Sales_Rep"].ToString();
                    if (dr["ORDER_NO"].ToString() != "")
                    {
                        if (dr["ONLINE"].ToString() == "1")
                        {
                            oLedg.strOrderNo = Utility.Mid(dr["ORDER_NO"].ToString(), 10, dr["ORDER_NO"].ToString().Length - 10);
                        }
                        else
                        {
                            oLedg.strOrderNo = dr["ORDER_NO"].ToString();
                        }
                    }
                    else
                    {
                        oLedg.strOrderNo = "";
                    }

                    if (dr["ORDER_DATE"].ToString() != "")
                    {
                        oLedg.strOrderDate = Convert.ToDateTime(dr["ORDER_DATE"]).ToString("dd-MM-yyyy");
                    }
                    else
                    {
                        oLedg.strOrderDate = "";
                    }

                    oLedg.intSalesQty = Convert.ToDouble(dr["BILL_QUANTITY"].ToString());
                    oLedg.intBonusQty = Convert.ToDouble(dr["BILL_QUANTITY_BONUS"].ToString());
                    oLedg.DblBillRate = Convert.ToDouble(dr["BILL_RATE"].ToString());
                    oLedg.DblBillAmount = Convert.ToDouble(dr["BILL_AMOUNT"].ToString());
                    oLedg.DblGCommPer = Convert.ToDouble(dr["G_COMM_PER"].ToString());
                    oLedg.strStockItemName = dr["STOCKITEM_NAME"].ToString();
                    oLedg.strPowrClass = dr["POWER_CLASS"].ToString();
                    oLedg.strStockGroupName = dr["STOCKGROUP_NAME"].ToString();
                    oLedg.strStockCategoryName = dr["STOCKCATEGORY_NAME"].ToString();
                    oLedg.strAddress1 = dr["LEDGER_ADDRESS11"].ToString();
                    oLedg.strTerritorycode = dr["TERITORRY_CODE"].ToString();
                    oLedg.strLedgerTerritory = dr["TERRITORRY_NAME"].ToString();
                    if (dr["PREPARED_BY"].ToString() != "")
                    {
                        oLedg.strParyName = dr["PREPARED_BY"].ToString();
                    }
                    else
                    {
                        oLedg.strParyName = "";
                    }
                    if (dr["COMP_VOUCHER_NARRATION"].ToString() != "")
                    {
                        oLedg.strString1 = dr["COMP_VOUCHER_NARRATION"].ToString();
                    }
                    else
                    {
                        oLedg.strString1 = "";
                    }
                    if (dr["STOCKGROUP_NAME"].ToString() == "Dilution")
                    {
                        oLedg.intDilutionMode = 1;
                        intMode = 1;

                    }

                    oLedg.intDilutionMode = intMode;

                    oLedg.strCopy = strcopy;
                    ooAccLedger.Add(oLedg);
                }
                if (!dr.HasRows)
                {
                    RProductSales oLedg = new RProductSales();
                    oLedg.strRefNo = "";
                    oLedg.strLedgername = "";
                    oLedg.strVoucheDate = "";
                    oLedg.DblVNetAmt = 0;
                    oLedg.strVDDate = "";
                    oLedg.strSALESREP = "";
                    oLedg.strOrderNo = "";
                    oLedg.strOrderDate = "";
                    oLedg.intSalesQty = 0;
                    oLedg.intBonusQty = 0;
                    oLedg.DblBillRate = 0;
                    oLedg.DblBillAmount = 0;
                    oLedg.DblGCommPer = 0;
                    oLedg.strStockItemName = "";
                    oLedg.strPowrClass = "";
                    oLedg.strStockGroupName = "";
                    oLedg.strStockCategoryName = "";
                    oLedg.strParyName = "";
                    oLedg.strString1 = "";
                    oLedg.strCopy = "";
                    oLedg.intDilutionMode = 0;
                    ooAccLedger.Add(oLedg);
                }
                dr.Close();
                gcnMain.Close();
                cmd.Dispose();
                return ooAccLedger;
            }
        }
        public List<RProductSales> mGetSalesInvoiceReportPriviewSubReport(string strDeComID, string strcomRef)
        {
            string strSQL = null;
            string connectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection gcnMain = new SqlConnection(connectionString))
            {
                if (gcnMain.State == ConnectionState.Open)
                {
                    gcnMain.Close();
                }
                gcnMain.Open();
                SqlDataReader dr;

                List<RProductSales> ooAccLedger = new List<RProductSales>();
                strSQL = "SELECT  sum(ACC_BILL_TRAN.BILL_QUANTITY)BILL_QUANTITY, sum(ACC_BILL_TRAN.BILL_QUANTITY_BONUS)BILL_QUANTITY_BONUS , ";
                strSQL = strSQL + "INV_STOCKITEM.POWER_CLASS	 FROM ACC_COMPANY_VOUCHER AS ACC_COMPANY_VOUCHER ";
                strSQL = strSQL + "INNER JOIN ACC_BILL_TRAN AS ACC_BILL_TRAN ON ACC_COMPANY_VOUCHER.COMP_REF_NO = ACC_BILL_TRAN.COMP_REF_NO ";
                strSQL = strSQL + "INNER JOIN INV_STOCKITEM AS INV_STOCKITEM ON ACC_BILL_TRAN.STOCKITEM_NAME = INV_STOCKITEM.STOCKITEM_NAME FULL OUTER JOIN ACC_LEDGER ON ACC_COMPANY_VOUCHER.SALES_REP = ACC_LEDGER.LEDGER_NAME ";
                strSQL = strSQL + "FULL OUTER JOIN ACC_LEDGER AS ACC_LEDGER_1 ON ACC_COMPANY_VOUCHER.SALES_REP = ACC_LEDGER_1.LEDGER_NAME ";
                strSQL = strSQL + "WHERE (ACC_COMPANY_VOUCHER.COMP_REF_NO = '" + strcomRef + "') and ACC_BILL_TRAN.STOCKGROUP_NAME='Dilution'  ";
                strSQL = strSQL + "group by   INV_STOCKITEM.POWER_CLASS ";
                strSQL = strSQL + "ORDER BY INV_STOCKITEM.POWER_CLASS desc ";

                SqlCommand cmd = new SqlCommand(strSQL, gcnMain);
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    RProductSales oLedg = new RProductSales();
                    oLedg.intSalesQty = Convert.ToDouble(dr["BILL_QUANTITY"].ToString());
                    oLedg.intBonusQty = Convert.ToDouble(dr["BILL_QUANTITY_BONUS"].ToString());
                    //oLedg.strStockItemName = dr["STOCKITEM_NAME"].ToString();
                    oLedg.strPowrClass = dr["POWER_CLASS"].ToString();
                    //oLedg.strStockGroupName = dr["STOCKGROUP_NAME"].ToString();
                    ooAccLedger.Add(oLedg);
                }
                if (!dr.HasRows)
                {
                    RProductSales oLedg = new RProductSales();
                    oLedg.strRefNo = "";
                    oLedg.strLedgername = "";
                    oLedg.strVoucheDate = "";
                    oLedg.DblVNetAmt = 0;
                    oLedg.strVDDate = "";
                    oLedg.strSALESREP = "";
                    oLedg.strOrderNo = "";
                    oLedg.strOrderDate = "";
                    oLedg.intSalesQty = 0;
                    oLedg.intBonusQty = 0;
                    oLedg.DblBillRate = 0;
                    oLedg.DblBillAmount = 0;
                    oLedg.DblGCommPer = 0;
                    oLedg.strStockItemName = "";
                    oLedg.strPowrClass = "";
                    oLedg.strStockGroupName = "";
                    oLedg.strStockCategoryName = "";
                    ooAccLedger.Add(oLedg);
                }
                dr.Close();
                gcnMain.Close();
                cmd.Dispose();
                System.Net.ServicePointManager.Expect100Continue = false;
                return ooAccLedger;
            }
        }

        


    }
}