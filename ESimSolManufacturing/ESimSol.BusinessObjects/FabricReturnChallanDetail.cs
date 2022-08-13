using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region FabricReturnChallanDetail
    public class FabricReturnChallanDetail : BusinessObject
    {
        public FabricReturnChallanDetail()
        {  
            FabricReturnChallanDetailID = 0;
            FabricReturnChallanID = 0;
            FDCDID = 0;
            LotID = 0;
            Qty = 0;
            Qty_DO = 0;
            Qty_DC = 0;
            MUnitID = 0;
            MUName = "";
            ErrorMessage = "";
            ChallanNo = "";
            ProductID = 0;
            FRChallan = new FabricReturnChallan();
            FRCDetails = new List<FabricReturnChallanDetail>();
            ReturnDate = DateTime.Now;
        }

        #region Properties
        public int FabricReturnChallanDetailID { get; set; }
        public int FabricReturnChallanID { get; set; }
        public int FDCDID { get; set; }
        public int ProductID { get; set; }
        public int LotID { get; set; }
        public int MUnitID { get; set; }
        public double Qty { get; set; }
        public double Qty_DO { get; set; }
        public double Qty_DC { get; set; }
        public string MUName { get; set; }
        public string ChallanNo { get; set; }
        public string Construction { get; set; }
        public double Qty_Return_Prv { get; set; }
        public string ExeNo { get; set; }
        public int FNBatchQCDetailID { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string LotNo { get; set; }
        public FabricReturnChallan FRChallan { get; set; }
        public string FabricNo { get; set; }
        public List<FabricReturnChallanDetail> FRCDetails { get; set; }
        public double Qty_Due
        {
            get 
            {
                double nDue = (this.Qty_DO - this.Qty - this.Qty_Return_Prv);

                return (nDue < 0 ? 0 : nDue);
            }
        }
        public string QtySt
        {
            get { return Global.MillionFormat(this.Qty, 2); }
        }
        public string QtyMSt
        {
            get
            {
                return Global.MillionFormat(Global.GetMeter(this.Qty, 2));
            }
        }
        public double ChallanQty { get; set; } 
        public string ChallanQtyMSt
        {
            get
            {
                return Global.MillionFormat(Global.GetMeter(this.ChallanQty, 2));
            }
        }
        public DateTime ReturnDate { get; set; }
        #endregion

        #region Functions
        public static List<FabricReturnChallanDetail> Gets(long nUserID)
        {
            return FabricReturnChallanDetail.Service.Gets(nUserID);
        }
        public static List<FabricReturnChallanDetail> Gets(string sSQL, long nUserID)
        {
            return FabricReturnChallanDetail.Service.Gets(sSQL, nUserID);
        }
        public static List<FabricReturnChallanDetail> Gets(int nFRCID, long nUserID)
        {
            return FabricReturnChallanDetail.Service.Gets(nFRCID, nUserID);
        }
        public FabricReturnChallanDetail Get(int nId, long nUserID)
        {
            return FabricReturnChallanDetail.Service.Get(nId, nUserID);
        }
        public FabricReturnChallanDetail Save(long nUserID)
        {
            return FabricReturnChallanDetail.Service.Save(this, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return FabricReturnChallanDetail.Service.Delete(nId, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricReturnChallanDetailService Service
        {
            get { return (IFabricReturnChallanDetailService)Services.Factory.CreateService(typeof(IFabricReturnChallanDetailService)); }
        }
        #endregion



    }
    #endregion

    #region IFabric interface
    public interface IFabricReturnChallanDetailService
    {
        FabricReturnChallanDetail Get(int id, long nUserID);
        List<FabricReturnChallanDetail> Gets(long nUserID);
        List<FabricReturnChallanDetail> Gets(string sSQL, long nUserID);
        List<FabricReturnChallanDetail> Gets(int nFRCID, long nUserID);
        string Delete(int id, long nUserID);
        FabricReturnChallanDetail Save(FabricReturnChallanDetail oFabricReturnChallanDetail, long nUserID);
    }
    #endregion
}
