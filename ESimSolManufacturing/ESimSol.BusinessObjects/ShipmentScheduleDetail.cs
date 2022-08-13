using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region ShipmentScheduleDetail
  
    public class ShipmentScheduleDetail : BusinessObject
    {
        public ShipmentScheduleDetail()
        {
            ShipmentScheduleDetailID = 0;
            ShipmentScheduleID = 0;
            ColorID = 0;
            UnitID = 0;
            Qty = 0;
            ColorName = "";
            UnitName = "";
            Symbol = "";
            OrderQty = 0;
            YetToPoductionOrderQty = 0;
            SizeID = 0;
            SizeName = "";
            ErrorMessage = "";
        }

        #region Properties
         
        public int ShipmentScheduleDetailID { get; set; }
         
        public int ShipmentScheduleID { get; set; }
         
        public int ColorID { get; set; }
         
        public int UnitID { get; set; }
         
        public double Qty { get; set; }
         
        public string ColorName { get; set; }
         
        public string UnitName { get; set; }
         
        public string Symbol { get; set; }
         
        public double OrderQty { get; set; }
        public double YetToPoductionOrderQty { get; set; }
         public int SizeID { get; set; }
         public string SizeName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        
        
        
        #endregion

        #region Functions

        public static List<ShipmentScheduleDetail> Gets(long nUserID)
        {
            return ShipmentScheduleDetail.Service.Gets( nUserID);
        }
        public static List<ShipmentScheduleDetail> Gets(string sSQL, Int64 nUserID)
        {
            return ShipmentScheduleDetail.Service.Gets(sSQL, nUserID);
        }public static List<ShipmentScheduleDetail> GetsByShipmentSchedule(int nid, long nUserID)
        {
            return ShipmentScheduleDetail.Service.GetsByShipmentSchedule(nid, nUserID);
        }

        public static List<ShipmentScheduleDetail> Gets(int nid, long nUserID)
        {
            return ShipmentScheduleDetail.Service.Gets(nid, nUserID);
        }

        public ShipmentScheduleDetail Get(int id, long nUserID)
        {
            return ShipmentScheduleDetail.Service.Get(id, nUserID);
        }

        public ShipmentScheduleDetail Save(long nUserID)
        {
            return ShipmentScheduleDetail.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return ShipmentScheduleDetail.Service.Delete(id, nUserID);
        }


        #endregion

        #region ServiceFactory


        internal static IShipmentScheduleDetailService Service
        {
            get { return (IShipmentScheduleDetailService)Services.Factory.CreateService(typeof(IShipmentScheduleDetailService)); }
        }

        #endregion
    }
    #endregion

    #region IShipmentScheduleDetail interface
     
    public interface IShipmentScheduleDetailService
    {
         
        ShipmentScheduleDetail Get(int id, Int64 nUserID);
         
        List<ShipmentScheduleDetail> Gets(Int64 nUserID);

        List<ShipmentScheduleDetail> Gets(int nid, Int64 nUserID);
        List<ShipmentScheduleDetail> Gets(string sSQL, Int64 nUserID);
         
        List<ShipmentScheduleDetail> GetsByShipmentSchedule(int nid, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        ShipmentScheduleDetail Save(ShipmentScheduleDetail oShipmentScheduleDetail, Int64 nUserID);
    }
    #endregion
}
