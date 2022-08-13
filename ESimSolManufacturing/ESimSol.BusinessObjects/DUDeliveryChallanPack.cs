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
    #region DUDeliveryChallanPack
    public class DUDeliveryChallanPack : BusinessObject
    {
        public DUDeliveryChallanPack()
        {
            DUDeliveryChallanPackID = 0;
            DUDeliveryChallanDetailID = 0;
            DUDeliveryChallanID = 0;
            RouteSheetPackingID = 0;
            QTY = 0;
            BagWeight = 0;
            LotNo = "";
            LogNo = "";
            Balance = 0;
            LotUnitPrice = 0;
            LotMUnitID = 0;
            ColorName = "";
            Shade = 0;
            MUnit = "";
            ErrorMessage = "";
            PackingWeight = 0;
            BagNo = 0;
        }

        #region Property
        public int DUDeliveryChallanPackID { get; set; }
        public int DUDeliveryChallanDetailID { get; set; }
        public int DUDeliveryChallanID { get; set; }
        public int RouteSheetPackingID { get; set; }
        public double QTY { get; set; }
        public double BagWeight{ get; set; }
        
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public int BagNo { get; set; }
        public double PackingWeight { get; set; }
        public string LotNo { get; set; }
        public string LogNo { get; set; }
        public double Balance { get; set; }
        public double LotUnitPrice { get; set; }
        public int LotMUnitID { get; set; }
        public string ColorName { get; set; }
        public int Shade { get; set; }
        public string MUnit { get; set; }
        public double GrossWeight
        {
            get
            {
                return this.QTY + this.BagWeight;
            }
            set{}
        }
        #endregion

        #region Functions
        public static List<DUDeliveryChallanPack> Gets(long nUserID)
        {
            return DUDeliveryChallanPack.Service.Gets(nUserID);
        }
        public static List<DUDeliveryChallanPack> Gets(string sSQL, long nUserID)
        {
            return DUDeliveryChallanPack.Service.Gets(sSQL, nUserID);
        }
        public DUDeliveryChallanPack Get(int id, long nUserID)
        {
            return DUDeliveryChallanPack.Service.Get(id, nUserID);
        }
        public DUDeliveryChallanPack Save(long nUserID)
        {
            return DUDeliveryChallanPack.Service.Save(this, nUserID);
        }
        public List<DUDeliveryChallanPack> SavePackingDetails(List<DUDeliveryChallanPack> oDUDeliveryChallanPacks, long nUserID)
        {
            return DUDeliveryChallanPack.Service.SavePackingDetails(oDUDeliveryChallanPacks, nUserID);
        }
        public string Delete(DUDeliveryChallanPack oDUDeliveryChallanPack, long nUserID)
        {
            return DUDeliveryChallanPack.Service.Delete(oDUDeliveryChallanPack, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IDUDeliveryChallanPackService Service
        {
            get { return (IDUDeliveryChallanPackService)Services.Factory.CreateService(typeof(IDUDeliveryChallanPackService)); }
        }
        #endregion
    }
    #endregion

    #region IDUDeliveryChallanPack interface
    public interface IDUDeliveryChallanPackService
    {
        DUDeliveryChallanPack Get(int id, Int64 nUserID);
        List<DUDeliveryChallanPack> Gets(Int64 nUserID);
        List<DUDeliveryChallanPack> Gets(string sSQL, Int64 nUserID);
        string Delete(DUDeliveryChallanPack oDUDeliveryChallanPack, Int64 nUserID);
        DUDeliveryChallanPack Save(DUDeliveryChallanPack oDUDeliveryChallanPack, Int64 nUserID);
        List<DUDeliveryChallanPack> SavePackingDetails(List<DUDeliveryChallanPack> oDUDeliveryChallanPacks, Int64 nUserID);
    }
    #endregion
}
