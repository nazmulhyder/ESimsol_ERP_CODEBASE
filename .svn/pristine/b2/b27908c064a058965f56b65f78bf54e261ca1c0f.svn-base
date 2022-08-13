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
    #region FabricRequisitionRoll
    public class FabricRequisitionRoll : BusinessObject
    {
        public FabricRequisitionRoll()
        {
            FabricRequisitionRollID = 0;
            FabricRequisitionDetailID = 0;
            LotID = 0;
            DisburseBy = 0;
            ReceiveDate = DateTime.MinValue;
            Qty = 0;
            ErrorMessage = "";
            ReceiveByName = "";
            LotNo = "";
            DispoNo = "";
            MUName = "Y";
            FBQCDetailID = 0;
            RollNo = 0;
            RollType = 0;
            FabricBatchQCLotID = 0;
            YetQty = 0;
        }

        #region Property
        public int FabricRequisitionRollID { get; set; }
        public int FabricRequisitionDetailID { get; set; }
        public int FabricBatchQCLotID { get; set; }
        public int LotID { get; set; }
        public int FBQCDetailID { get; set; }
        public int DisburseBy { get; set; }
        public DateTime ReceiveDate { get; set; }
        public double Qty { get; set; }
        public double RollNo { get; set; }
        public int RollType { get; set; }
        public double YetQty { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property        
        public int WUID { get; set; }
        public int FEOID { get; set; }
        public string DispoNo { get; set; }
        public string MUName { get; set; }
        public string ReceiveByName { get; set; }
        public string LotNo { get; set; }
        public string ReceiveDateST
        {
            get
            {
                if (this.ReceiveDate == DateTime.MinValue) return "";
                return ReceiveDate.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #region Functions
        public static List<FabricRequisitionRoll> Gets(long nUserID)
        {
            return FabricRequisitionRoll.Service.Gets(nUserID);
        }
        public static List<FabricRequisitionRoll> Gets(string sSQL, long nUserID)
        {
            return FabricRequisitionRoll.Service.Gets(sSQL, nUserID);
        }
        public FabricRequisitionRoll Get(int id, long nUserID)
        {
            return FabricRequisitionRoll.Service.Get(id, nUserID);
        }
        public static FabricRequisitionRoll GetByDetailID(int id, long nUserID)
        {
            return FabricRequisitionRoll.Service.GetByDetailID(id, nUserID);
        }
        public FabricRequisitionRoll Save(long nUserID)
        {
            return FabricRequisitionRoll.Service.Save(this, nUserID);
        }
        public List<FabricRequisitionRoll> SaveFabricRequisitionRoll(List<FabricRequisitionRoll> oFabricRequisitionRolls, long nUserID)
        {
            return FabricRequisitionRoll.Service.SaveFabricRequisitionRoll(oFabricRequisitionRolls, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return FabricRequisitionRoll.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricRequisitionRollService Service
        {
            get { return (IFabricRequisitionRollService)Services.Factory.CreateService(typeof(IFabricRequisitionRollService)); }
        }
        #endregion
    }
    #endregion

    #region IFabricRequisitionRoll interface
    public interface IFabricRequisitionRollService
    {
        FabricRequisitionRoll Get(int id, Int64 nUserID);
        FabricRequisitionRoll GetByDetailID(int id, Int64 nUserID);
        List<FabricRequisitionRoll> Gets(Int64 nUserID);
        List<FabricRequisitionRoll> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        FabricRequisitionRoll Save(FabricRequisitionRoll oFabricRequisitionRoll, Int64 nUserID);
        List<FabricRequisitionRoll> SaveFabricRequisitionRoll(List<FabricRequisitionRoll> oFabricRequisitionRolls, Int64 nUserID);
    }
    #endregion
}
