using ICS.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESimSol.BusinessObjects
{
    public class WUSubContractYarnChallanDetail : BusinessObject
    {
		public WUSubContractYarnChallanDetail()
		{
			WUSubContractYarnChallanDetailID = 0;
			WUSubContractYarnChallanID = 0;
			WUSubContractID = 0;
			WUSubContractYarnConsumptionID = 0;
			IssueStoreID = 0;
			YarnID = 0;
			LotID = 0;
			MUnitID = 0;
			Qty = 0;
			Remarks = "";
			BagQty = 0;
			DBUserID = 0;
			DBServerDateTime = DateTime.Today;
			ErrorMessage = "";
            StoreName = "";
            YarnCode = "";
			YarnName = "";
			LotNo = "";
            LotBalance = "";
			MUSymbol = "";
			YetToChallanQty = 0;
		}

		#region Properties

		public int WUSubContractYarnChallanDetailID { get; set; }
		public int WUSubContractYarnChallanID { get; set; }
		public int WUSubContractID { get; set; }
		public int WUSubContractYarnConsumptionID { get; set; }
		public int IssueStoreID { get; set; }
		public int YarnID { get; set; }
		public int LotID { get; set; }
		public int MUnitID { get; set; }
		public double Qty { get; set; }
		public string Remarks { get; set; }
		public int BagQty { get; set; }
		public int DBUserID { get; set; }
		public DateTime DBServerDateTime { get; set; }
		public string ErrorMessage { get; set; }
		public string StoreName { get; set; }
        public string YarnCode { get; set; }
        public string YarnName { get; set; }
		public string LotNo { get; set; }
        public string LotBalance { get; set; }
        public string MUSymbol { get; set; }
		public double YetToChallanQty { get; set; }

		#endregion

		#region Functions

		public static List<WUSubContractYarnChallanDetail> Gets(int nId, long nUserID)
        {
            return WUSubContractYarnChallanDetail.Service.Gets(nId, nUserID);
        }

		#endregion

		#region ServiceFactory
		internal static IWUSubContractYarnChallanDetailService Service
        {
            get { return (IWUSubContractYarnChallanDetailService)Services.Factory.CreateService(typeof(IWUSubContractYarnChallanDetailService)); }
        }
        #endregion
    }


    #region IWUSubContractYarnChallanDetail interface

    public interface IWUSubContractYarnChallanDetailService
    {
        List<WUSubContractYarnChallanDetail> Gets(int nid, long nUserID);
	}

    #endregion
}

