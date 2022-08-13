using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

namespace ESimSol.BusinessObjects
{
    public class TempDesignation
    {
        public TempDesignation()
        {
            DesignationID = 0;
            Designation = "";
            DesignationResponsibilitys = new List<DesignationResponsibility>();
            Sequence = 0;
            Column1 = 0;
            Column2 = 0;
            Column3 = 0;
            Column4 = 0;
            Column5 = 0;
            DepartmentRequirementDesignationID = 0;

        }
        public int DesignationID { get; set; }
        public string Designation { get; set; }
        public List<DesignationResponsibility> DesignationResponsibilitys { get; set; }
        public int Sequence { get; set; }
        public int Column1 { get; set; }
        public int Column2 { get; set; }
        public int Column3 { get; set; }
        public int Column4 { get; set; }
        public int Column5 { get; set; }
        public int DepartmentRequirementDesignationID { get; set; }

    }
}
