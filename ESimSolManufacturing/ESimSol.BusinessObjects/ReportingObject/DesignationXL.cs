using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESimSol.BusinessObjects.ReportingObject
{
    public class DesignationXL
    {
        public DesignationXL()
        {
            SL = "";
            Code = "";
            Name = "";
            ParentCode = "";
        }
        public string SL { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ParentCode { get; set; }

    }
}
