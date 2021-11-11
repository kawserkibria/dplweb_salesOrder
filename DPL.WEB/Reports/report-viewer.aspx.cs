using CrystalDecisions.CrystalReports.Engine;
using DPL.WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DPL.WEB.Reports
{
    public partial class report_viewer : System.Web.UI.Page
    {
        private String reportTitle = "";
        private String secondParameter = "";
        private String reportHeading = "";
        public string strString = "";
        public string strString2 = "";
        public string strString3 = "";

        public string ReportSecondParameter1 { get; set; }
        public String ReportTitle { set { reportTitle = value; } get { return reportTitle; } }
        public String ReportSecondParameter { set { secondParameter = value; } get { return secondParameter; } }
        public String ReportHeading { set { reportHeading = value; } get { return reportHeading; } }
        private string strComID { get; set; }
        private void InitialiseLabels(ReportDocument rpt)
        {

            ((TextObject)rpt.ReportDefinition.ReportObjects["txtCompanyName"]).Text = "DeepLaid Lab. Ltd.";
            ((TextObject)rpt.ReportDefinition.ReportObjects["txtCompanyAddress1"]).Text = "Dhaka";
            ((TextObject)rpt.ReportDefinition.ReportObjects["txtCompanyAddress2"]).Text = "Kaptan bazzar";
            //((TextObject)rpt.ReportDefinition.ReportObjects["txtCompanyEmail"]).Text = Utility.gstrEmail;
            ((TextObject)rpt.ReportDefinition.ReportObjects["txtCompanyName2"]).Text = "Deeplaid Branch";
            ((TextObject)rpt.ReportDefinition.ReportObjects["txtSecondParameter2"]).Text = "Punch Report";

            ((TextObject)rpt.ReportDefinition.ReportObjects["txtIT"]).Text = "MIS & IT";
            //((TextObject)rpt.ReportDefinition.ReportObjects["txtCompanyWeb"]).Text = Utility.gstrWeb;
            if (ReportTitle != "")
            {
                ((TextObject)rpt.ReportDefinition.ReportObjects["txtReportTitle"]).Text = this.ReportTitle;
                if (ReportSecondParameter != "")
                {
                    ((TextObject)rpt.ReportDefinition.ReportObjects["txtSecondParameter2"]).Text = this.ReportTitle + " From " + '-' + this.secondParameter;
                }
            }
            if (ReportSecondParameter != "")
            {
                ((TextObject)rpt.ReportDefinition.ReportObjects["txtSecondParameter"]).Text = this.secondParameter;
            }
            else
            {
                rpt.ReportDefinition.ReportObjects["txtSecondParameter"].ObjectFormat.EnableSuppress = true;
            }
            if (ReportHeading != "")
            {
                ((TextObject)rpt.ReportDefinition.ReportObjects["txtCompanyPhone"]).Text = ReportHeading;
            }


        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ReportDocument rpt1;

            List<RProductSales> dataList = null;
            if (HttpContext.Current.Session["ReportDataList"] != null)
            {
                dataList = (List<RProductSales>)HttpContext.Current.Session["ReportDataList"];
            }


            ctrlReportViewer.Zoom(100);
            //----------------1
            ctrlReportViewer.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            rptInvoice_view crystalReport = new rptInvoice_view();
            rpt1 = (ReportDocument)crystalReport;
            crystalReport.SetDataSource(dataList);
            //crystalReport.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
            //crystalReport.PrintToPrinter(1, false, 0, 0);
            this.reportTitle = "Blood Group";
            InitialiseLabels(rpt1);
            ctrlReportViewer.ReportSource = crystalReport;



            //ReportDocument rpt1;
            //List<RProductSales> dataList = null;
            //if (HttpContext.Current.Session["ReportDataList"] != null)
            //{
            //    dataList = (List<RProductSales>)HttpContext.Current.Session["ReportDataList"];
            //}
            //string division = string.Empty;

            //ctrlReportViewer.Zoom(100);
            ////----------------1
            //ctrlReportViewer.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            //rptInvoice_view crystalReport = new rptInvoice_view();
            //rpt1 = (ReportDocument)crystalReport;
            //crystalReport.SetDataSource(dataList);

            ////crystalReport.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
            ////crystalReport.PrintToPrinter(1, false, 0, 0);
            //this.reportTitle = "PABX Information";
            ////rpt1.ex
            //InitialiseLabels(rpt1);
            //ctrlReportViewer.ReportSource = crystalReport;
        }
    }
}