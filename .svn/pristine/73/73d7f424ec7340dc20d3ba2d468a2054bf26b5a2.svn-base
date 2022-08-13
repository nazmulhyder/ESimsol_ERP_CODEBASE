using ICS.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESimSol.BusinessObjects
{
    public class WUSubContractYarnChallan : BusinessObject
    {
        public WUSubContractYarnChallan()
        {
            WUSubContractYarnChallanID = 0;
            WUSubContractID = 0;
            ChallanNo = "";
            ChallanDate = DateTime.Today;
            TruckNo = "";
            DriverName = "";
            DeliveryPoint = "";
            Remarks = "";
            ApprovedBy = 0;
            DBUserID = 0;
            DBServerDateTime = DateTime.Today;
            ErrorMessage = "";
            ApprovedByName = "";
            EntyUserName = "";
            JobNo = "";
            ContractDate = DateTime.Today;
            SupplierName = "";
            Construction = "";
            BuyerName = "";
            OrderQty = 0;
            MUSymbol = "";
            WUSubContractYarnChallanDetails = new List<WUSubContractYarnChallanDetail>();
        }

        #region Properties

        public int WUSubContractYarnChallanID { get; set; }
        public int WUSubContractID { get; set; }
        public string ChallanNo { get; set; }
        public DateTime ChallanDate { get; set; }
        public string TruckNo { get; set; }
        public string DriverName { get; set; }
        public string DeliveryPoint { get; set; }
        public string Remarks { get; set; }
        public int ApprovedBy { get; set; }
        public int DBUserID { get; set; }
        public DateTime DBServerDateTime { get; set; }
        public string ErrorMessage { get; set; }
        public string ApprovedByName { get; set; }
        public string EntyUserName { get; set; }
        public string JobNo { get; set; }
        public DateTime ContractDate { get; set; }
        public string SupplierName { get; set; }
        public string Construction { get; set; }
        public string BuyerName { get; set; }
        public double OrderQty { get; set; }
        public string MUSymbol { get; set; }
        public List<WUSubContractYarnChallanDetail> WUSubContractYarnChallanDetails { get; set; }

        #endregion

        #region Derived Property

        public string ChallanDateSt
        {
            get
            {
                return this.ChallanDate.ToString("dd MMM yyyy");
            }
        }

        #endregion

        #region Functions

        public WUSubContractYarnChallan Get(int nId, long nUserID)
        {
            return WUSubContractYarnChallan.Service.Get(nId, nUserID);
        }        
        public static List<WUSubContractYarnChallan> Gets(string sSQL, int nCurrentUserID)
        {
            return WUSubContractYarnChallan.Service.Gets(sSQL, nCurrentUserID);
        }
        public WUSubContractYarnChallan Save(long nUserID)
        {
            return WUSubContractYarnChallan.Service.Save(this, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return WUSubContractYarnChallan.Service.Delete(nId, nUserID);
        }
        public WUSubContractYarnChallan Approve(long nUserID)
        {
            return WUSubContractYarnChallan.Service.Approve(this, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IWUSubContractYarnChallanService Service
        {
            get { return (IWUSubContractYarnChallanService)Services.Factory.CreateService(typeof(IWUSubContractYarnChallanService)); }
        }
        #endregion
    }


    #region IWUSubContractYarnChallan interface

    public interface IWUSubContractYarnChallanService
    {
        WUSubContractYarnChallan Get(int id, long nUserID);        
        List<WUSubContractYarnChallan> Gets(string sSQL, int nCurrentUserID);
        string Delete(int id, long nUserID);
        WUSubContractYarnChallan Save(WUSubContractYarnChallan oWUSubContractYarnChallan, long nUserID);
        WUSubContractYarnChallan Approve(WUSubContractYarnChallan oWUSubContractYarnChallan, long nUserID);      
    }

    #endregion
}

