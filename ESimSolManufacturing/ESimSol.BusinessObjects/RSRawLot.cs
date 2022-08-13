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
    #region RSRawLot

    public class RSRawLot : BusinessObject
    {
        public RSRawLot()
        {
            RSRawLotID = 0;
            RouteSheetID = 0;
            LotID = 0;
            ProductType = EnumProductType.None;
            ProductID = 0;
            ProductBaseID = 0;
            ProductName = "";
            CurrencyID=0;
            Qty = 0;
            NumOfCone = 0;
            UnitPrice = 0;
            FCUnitPrice = 0;
            WorkingUnitID = 0;
            RSShiftID = 0;
            ErrorMessage = "";
            MUnit = "";
            RouteSheetNo = "";
            DyeingOrderID = 0;
            DyeingOrderDetailID = 0;
        }

        #region Properties
        public int RSRawLotID { get; set; }
        public int RouteSheetID { get; set; }
        public int LotID { get; set; }
        public int CurrencyID { get; set; }
        public EnumProductType ProductType { get; set; }
        public double Balance { get; set; }
        public double Qty { get; set; }
        public double UnitPrice { get; set; }
        public double FCUnitPrice { get; set; }
        public int NumOfCone { get; set; }
        public int ProductID { get; set; }
        public int ProductBaseID { get; set; }
        public int WorkingUnitID { get; set; }
        public string ProductName { get; set; }
        public string RouteSheetNo { get; set; }
        
        public string MUnit { get; set; }
        public string LotNo { get; set; }
        public int RSShiftID { get; set; }
        public int DyeingOrderID { get; set; }
        public int DyeingOrderDetailID { get; set; }
        
        public string OperationUnitName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string Param { get; set; }
        public int ProductTypeInt { get { return (int)this.ProductType; } }
        public string ProductTypeSt { get { return EnumObject.jGet(this.ProductType); } }
        #endregion

        #region Functions

        public static List<RSRawLot> Gets(long nUserID)
        {
            return RSRawLot.Service.Gets(nUserID);
        }
        public static List<RSRawLot> Gets(string sSQL, Int64 nUserID)
        {
            return RSRawLot.Service.Gets(sSQL, nUserID);
        }
        public static List<RSRawLot> GetsByRSID(int RSID,Int64 nUserID)
        {
            return RSRawLot.Service.GetsByRSID(RSID, nUserID);
        }

        public RSRawLot Get(int nId, long nUserID)
        {
            return RSRawLot.Service.Get(nId, nUserID);
        }

        public RSRawLot Save(long nUserID)
        {
            return RSRawLot.Service.Save(this, nUserID);
        }
        public List<RSRawLot> SaveMultiple(List<RSRawLot> oRSRawLots, long nUserID)
        {
            return RSRawLot.Service.SaveMultiple(oRSRawLots, nUserID);
        }

        public string Delete(int nId, long nUserID)
        {
            return RSRawLot.Service.Delete(nId, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IRSRawLotService Service
        {
            get { return (IRSRawLotService)Services.Factory.CreateService(typeof(IRSRawLotService)); }
        }
        #endregion

    }
    #endregion

    #region IRSRawLot interface

    public interface IRSRawLotService
    {
        RSRawLot Get(int id, long nUserID);
        List<RSRawLot> Gets(long nUserID);
        List<RSRawLot> Gets(string sSQL, Int64 nUserID);
        List<RSRawLot> GetsByRSID(int RSID, Int64 nUserID);
        string Delete(int id, long nUserID);
        RSRawLot Save(RSRawLot oRSRawLot, long nUserID);
        List<RSRawLot> SaveMultiple(List<RSRawLot> oRSRawLots, long nUserID);
    }
    #endregion
}