using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{
    public class KnitDyeingYarnConsumption : BusinessObject
    {
        public KnitDyeingYarnConsumption()
        {
            KnitDyeingYarnConsumptionID = 0;
            KnitDyeingProgramDetailID = 0;
            YarnID = 0;
            UsagesParcent = 0;
            FinishReqQty = 0;
            GracePercent = 0;
            ReqQty = 0;
            MUnitID = 0;
            Remarks = "";
            YarnName = "";
            FabricTypeID = 0;
            FabricTypeName = "";
            MUnitSymbol = "";
        }
        #region Properties
        public int KnitDyeingYarnConsumptionID { get; set; }
        public int KnitDyeingProgramDetailID { get; set; }
        public int YarnID { get; set; }
        public double UsagesParcent { get; set; }
        public double FinishReqQty { get; set; }
        public double GracePercent { get; set; }
        public double ReqQty { get; set; }
        public int MUnitID { get; set; }
        public string Remarks { get; set; }
        public string YarnName { get; set; }
        public int FabricTypeID { get; set; }
        public string FabricTypeName { get; set; }
        public string MUnitSymbol { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Functions
        public static List<KnitDyeingYarnConsumption> Gets(int id, long nUserID)
        {
            return KnitDyeingYarnConsumption.Service.Gets(id, nUserID);
        }
        public static List<KnitDyeingYarnConsumption> Gets(string sSQL, long nUserID)
        {
            return KnitDyeingYarnConsumption.Service.Gets(sSQL, nUserID);
        }
        #endregion
    
        #region ServiceFactory
        internal static IKnitDyeingYarnConsumptionService Service
        {
            get { return (IKnitDyeingYarnConsumptionService)Services.Factory.CreateService(typeof(IKnitDyeingYarnConsumptionService)); }
        }
        #endregion
    }

    #region IKnitDyeingYarnConsumptionService interface

    public interface IKnitDyeingYarnConsumptionService
    {
        List<KnitDyeingYarnConsumption> Gets(int id, long nUserID);
        List<KnitDyeingYarnConsumption> Gets(string sSQL, long nUserID);
    }
    #endregion
}
