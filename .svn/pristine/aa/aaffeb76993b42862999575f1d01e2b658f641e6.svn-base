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
	#region KnitDyeingProgramDetail  
	public class KnitDyeingProgramDetail : BusinessObject
	{	
		public KnitDyeingProgramDetail()
		{
			KnitDyeingProgramDetailID = 0; 
			KnitDyeingProgramID = 0;
            KnitDyeingProgramDetailLogID = 0;
            KnitDyeingProgramLogID = 0;
            RefObjectID = 0;
            RefObjectNo = "";
			ColorID = 0;
            ColorName = "";
            GarmentsQty = 0;
            GarmentsMUnitID = 0;
            ConsumPtionMUnitID = 0;
            FabricTypeID = 0;
            FabricTypeName = "";
            GSMID = 0;
            GSMName = "";
            CompositionID = 0;
            CompositionName = "";
            ConsumptionPerDzn = 0;
            FinishDiaID = 0;
            FinishDiaName = "";
			PantoneNo = ""; 
			ApprovedShade = 0;
            ShadeRecipe = "";
			ShadeRemarks = "";
            ReqFinishFabricQty = 0;
            GracePercent = 0;
            ReqFabricQty = 0;
			MUnitID = 0; 
			Remarks = "";
            RefProgramNo = "";
            StyleID = 0;
            YetReqFabricQty = 0;
            RefTypeInt = 0;
			ErrorMessage = "";
            KnitDyeingYarnConsumptions = new List<KnitDyeingYarnConsumption>();
		}

		#region Property
		public int KnitDyeingProgramDetailID { get; set; }
		public int KnitDyeingProgramID { get; set; }
        public int KnitDyeingProgramLogID { get; set; }
        public int KnitDyeingProgramDetailLogID { get; set; }
        public int RefObjectID { get; set; }
        public string RefObjectNo { get; set; }
        public string RefProgramNo { get; set; }
		public int ColorID { get; set; }
        public string ColorName { get; set; }
        public double GarmentsQty { get; set; }
        public int GarmentsMUnitID { get; set; }
        public int FabricTypeID { get; set; }
        public string FabricTypeName { get; set; }
        public int GSMID { get; set; }
        public string GSMName { get; set; }
        public int CompositionID { get; set; }
        public int ConsumPtionMUnitID { get; set; }

        public string CompositionName { get; set; }
        public double ConsumptionPerDzn { get; set; }
        public int FinishDiaID { get; set; }
        public string FinishDiaName { get; set; }
		public string PantoneNo { get; set; }
		public int ApprovedShade { get; set; }
        public string ShadeRecipe { get; set; }
		public string ShadeRemarks { get; set; }
        public double ReqFinishFabricQty { get; set; }
        public double GracePercent { get; set; }
        public double ReqFabricQty { get; set; }
        public int MUnitID { get; set; }

		public string Remarks { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public double YetReqFabricQty { get; set; }
		public string StyleNo { get; set; }
        public string KDProgramNo { get; set; }
        public int StyleID { get; set; }
		public string BuyerName { get; set; }
        public double TotalGarmentsQty { get; set; }
        public int RefTypeInt { get; set; }
        public string TotalGarmentsQtySt
        {
            get
            {
                return TotalGarmentsQty.ToString("#00.00");
            }
        }
        public int FabricID { get; set; }
        public string FabricName { get; set; }
        public int FinishGSMID { get; set; }
        public string FinishGSMName { get; set; }
        public string GarmentsMUnitName { get; set; }
        public string JOBPAMPONo
        {
            get
            {
                string sJOBPAMPONo = "";
                if (this.RefObjectNo != "")
                {
                    sJOBPAMPONo = this.RefObjectNo;
                }
                else
                {
                    sJOBPAMPONo = this.RefProgramNo;
                }
                return sJOBPAMPONo;
            }
        }
        public List<KnitDyeingYarnConsumption> KnitDyeingYarnConsumptions { get; set; }
		#endregion 

		#region Functions 
		public static List<KnitDyeingProgramDetail> Gets(long nUserID)
		{
			return KnitDyeingProgramDetail.Service.Gets(nUserID);
		}
		public static List<KnitDyeingProgramDetail> Gets(string sSQL, long nUserID)
		{
			return KnitDyeingProgramDetail.Service.Gets(sSQL,nUserID);
		}
		public KnitDyeingProgramDetail Get(int id, long nUserID)
		{
			return KnitDyeingProgramDetail.Service.Get(id,nUserID);
		}
		public KnitDyeingProgramDetail Save(long nUserID)
		{
			return KnitDyeingProgramDetail.Service.Save(this,nUserID);
		}
        public static List<KnitDyeingProgramDetail> Gets(int id, long nUserID)
        {
            return KnitDyeingProgramDetail.Service.Gets(id, nUserID);
        }
        public static List<KnitDyeingProgramDetail> GetsLog(int LogId, long nUserID)
        {
            return KnitDyeingProgramDetail.Service.GetsLog(LogId, nUserID);
        }
		public  string  Delete(int id, long nUserID)
		{
			return KnitDyeingProgramDetail.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IKnitDyeingProgramDetailService Service
		{
			get { return (IKnitDyeingProgramDetailService)Services.Factory.CreateService(typeof(IKnitDyeingProgramDetailService)); }
		}
		#endregion
	}
	#endregion

	#region IKnitDyeingProgramDetail interface
	public interface IKnitDyeingProgramDetailService 
	{
		KnitDyeingProgramDetail Get(int id, Int64 nUserID); 
		List<KnitDyeingProgramDetail> Gets(Int64 nUserID);
		List<KnitDyeingProgramDetail> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
        List<KnitDyeingProgramDetail> Gets(int id, long nUserID);
        List<KnitDyeingProgramDetail> GetsLog(int LogId, long nUserID);
        
 		KnitDyeingProgramDetail Save(KnitDyeingProgramDetail oKnitDyeingProgramDetail, Int64 nUserID);
	}
	#endregion
}
