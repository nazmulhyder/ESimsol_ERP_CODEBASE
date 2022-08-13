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
    #region SparePartsChallanDetail

    public class SparePartsChallanDetail : BusinessObject
    {
        public SparePartsChallanDetail()
        {
            SparePartsChallanDetailID = 0;
            SparePartsChallanID = 0;
            SparePartsRequisitionDetailID = 0;
            ProductID = 0;
            ProductName = "";
            ProductCode = "";
            LotID = 0;
            MUnitID = 0;
            MUnitName = "";
            MUnitSymbol = "";
            ChallanQty = 0;
            CurrentStockQty = 0;
            Remarks = "";
        }

        #region Properties
        public int SparePartsChallanDetailID { get; set; }
        public int SparePartsChallanID { get; set; }
        public int SparePartsRequisitionDetailID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public int LotID { get; set; }
        public string LotNo { get; set; }
        public int MUnitID { get; set; }
        public string MUnitName { get; set; }
        public string MUnitSymbol { get; set; }
        public double RequisitionQty { get; set; }
        public double ChallanQty { get; set; }
        public double CurrentStockQty { get; set; }
        public string Remarks { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public double YetToChallanQty
        {
            get
            {
                return this.RequisitionQty - this.ChallanQty;
            }
        }

        #endregion

        #region Functions

        public static List<SparePartsChallanDetail> Gets(long nUserID)
        {
            return SparePartsChallanDetail.Service.Gets(nUserID);
        }
        public static List<SparePartsChallanDetail> GetsBySparePartsChallanID(int nSparePartsChallanID, long nUserID)
        {
            return SparePartsChallanDetail.Service.GetsBySparePartsChallanID(nSparePartsChallanID, nUserID);
        }
        public static List<SparePartsChallanDetail> Gets(string sSQL, Int64 nUserID)
        {
            return SparePartsChallanDetail.Service.Gets(sSQL, nUserID);
        }
        public SparePartsChallanDetail Get(int nId, long nUserID)
        {
            return SparePartsChallanDetail.Service.Get(nId, nUserID);
        }
        public SparePartsChallanDetail Save(long nUserID)
        {
            return SparePartsChallanDetail.Service.Save(this, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return SparePartsChallanDetail.Service.Delete(nId, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static ISparePartsChallanDetailService Service
        {
            get { return (ISparePartsChallanDetailService)Services.Factory.CreateService(typeof(ISparePartsChallanDetailService)); }
        }
        #endregion
    }
    #endregion

    #region ISparePartsChallanDetail interface

    public interface ISparePartsChallanDetailService
    {
        SparePartsChallanDetail Get(int id, long nUserID);
        List<SparePartsChallanDetail> Gets(long nUserID);
        List<SparePartsChallanDetail> GetsBySparePartsChallanID(int nSparePartsChallanID, long nUserID);
        List<SparePartsChallanDetail> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, long nUserID);
        SparePartsChallanDetail Save(SparePartsChallanDetail oSparePartsChallanDetail, long nUserID);
    }
    #endregion
}