using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPL.WEB.Models
{
    public class Invoice
    {
        public int intRow { get; set; }
        public string strLedgerName { get; set; }
        public string strGroupName { get; set; }
        public string strSalesLedger { get; set; }
        public string strPurchaseLedger { get; set; }
        public string strSalesRepresentative { get; set; }
        public string strItemName { get; set; }
        public string strUnit { get; set; }
        public string strUom { get; set; }
        public string strBranchID { get; set; }
        public string strBranchName { get; set; }
        public string strGodownsName { get; set; }
        public string strBatch { get; set; }
        public double  dblQty { get; set; }
        public double dblRate { get; set; }
        public double dblNetAmount { get; set; }
        public double dblBillAmount { get; set; }
        public double dblBonusQty { get; set; }
        public double dblDiscount { get; set; }
        public string strBillKey { get; set; }
        public string strRefNo { get; set; }
        public string strTeritorryCode { get; set; }
        public string strTeritorryName { get; set; }
        public string strMereString { get; set; }
        public string strAddress { get; set; }
        public double dblCommAmount { get; set; }
    }
}
