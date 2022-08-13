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
    #region GUProductionOrderDetail
    
    public class GUProductionOrderDetail : BusinessObject
    {
        public GUProductionOrderDetail()
        {
            GUProductionOrderDetailID = 0;
            GUProductionOrderID = 0;
            ColorID = 0;
            UnitID = 0;
            Qty = 0;
            ColorName = "";
            UnitName = "";
            Symbol = "";
            OrderQty = 0;
            YetToProductionQty = 0;
            SizeID = 0;
            SizeName = "";
            ErrorMessage = "";
        }

        #region Properties
         
        public int GUProductionOrderDetailID { get; set; }
         
        public int GUProductionOrderID { get; set; }
         
        public int ColorID { get; set; }
         
        public int UnitID { get; set; }
         
        public double Qty { get; set; }
         
        public string ColorName { get; set; }
         
        public string UnitName { get; set; }
         
        public string Symbol { get; set; }
         
        public double OrderQty { get; set; }
         
        public double YetToProductionQty { get; set; }
         public int SizeID { get; set; }
         public string SizeName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        
        
        
        #endregion

        #region Functions

        public static List<GUProductionOrderDetail> Gets(long nUserID)
        {
            return GUProductionOrderDetail.Service.Gets( nUserID);
        }
        public static List<GUProductionOrderDetail> Gets(string sSQL, Int64 nUserID)
        {
            return GUProductionOrderDetail.Service.Gets(sSQL, nUserID);
        }public static List<GUProductionOrderDetail> GetsByGUProductionOrder(int nid, long nUserID)
        {
            return GUProductionOrderDetail.Service.GetsByGUProductionOrder(nid, nUserID);
        }

        public static List<GUProductionOrderDetail> Gets(int nid, long nUserID)
        {
            return GUProductionOrderDetail.Service.Gets(nid, nUserID);
        }

        public GUProductionOrderDetail Get(int id, long nUserID)
        {
            return GUProductionOrderDetail.Service.Get(id, nUserID);
        }

        public GUProductionOrderDetail Save(long nUserID)
        {
            return GUProductionOrderDetail.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return GUProductionOrderDetail.Service.Delete(id, nUserID);
        }


        #endregion

        #region ServiceFactory


        internal static IGUProductionOrderDetailService Service
        {
            get { return (IGUProductionOrderDetailService)Services.Factory.CreateService(typeof(IGUProductionOrderDetailService)); }
        }

        #endregion
    }
    #endregion

    #region IGUProductionOrderDetail interface
     
    public interface IGUProductionOrderDetailService
    {
         
        GUProductionOrderDetail Get(int id, Int64 nUserID);
         
        List<GUProductionOrderDetail> Gets(Int64 nUserID);

        List<GUProductionOrderDetail> Gets(int nid, Int64 nUserID);
        List<GUProductionOrderDetail> Gets(string sSQL, Int64 nUserID);
         
        List<GUProductionOrderDetail> GetsByGUProductionOrder(int nid, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        GUProductionOrderDetail Save(GUProductionOrderDetail oGUProductionOrderDetail, Int64 nUserID);
    }
    #endregion
}
