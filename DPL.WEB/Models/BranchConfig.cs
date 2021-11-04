using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPL.WEB.Models
{
    public class BranchConfig
    {
        public string BranchID { get; set; }
        public string BranchName { get; set; }
        public string BranchAddress1 { get; set; }
        public string BranchAddress2 { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Comments { get; set; }
        public string DefaultBranch { get; set; }
        public string Status { get; set; }
        public double dblbranchOpening { get; set; }

    }
}
