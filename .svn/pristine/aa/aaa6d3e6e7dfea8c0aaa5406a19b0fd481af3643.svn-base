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
    #region YarnRequisitionDetail
    public class YarnRequisitionDetail : BusinessObject
    {
        public YarnRequisitionDetail()
        {
            YarnRequisitionDetailID = 0;
            YarnRequisitionID = 0;
            TechnicalSheetID = 0;
            ColorID = 0;
            PentonNo = "";
            GARQty = 0;
            FabricID = 0;
            GSM = "";
            YarnID = 0;
            YarnCount = "";
            YarnPercent = 0;
            ActualConsumption = 0;
            CostingConsumption = 0;
            RequisitionQty = 0;
            MUnitID = 0;
            Remarks = "";
            FabricName = "";
            FabricCode = "";
            YarnName = "";
            YarnCode = "";
            MUSymbol = "";
            OrderRecapNo = "";
            StyleNo = "";
            BuyerName = "";
            ColorName = "";
            StyleDefine = 0;
            ApproxQty = 0;
            ErrorMessage = "";
        }

        #region Property
        public int YarnRequisitionDetailID { get; set; }
        public int YarnRequisitionID { get; set; }
        public int TechnicalSheetID { get; set; }
        public int ColorID { get; set; }
        public string PentonNo { get; set; }
        public double GARQty { get; set; }
        public int FabricID { get; set; }
        public string GSM { get; set; }
        public int YarnID { get; set; }
        public string YarnCount { get; set; }
        public double YarnPercent { get; set; }
        public double ActualConsumption { get; set; }
        public double CostingConsumption { get; set; }
        public double RequisitionQty { get; set; }
        public int MUnitID { get; set; }
        public string Remarks { get; set; }
        public string FabricName { get; set; }
        public string FabricCode { get; set; }
        public string YarnName { get; set; }
        public string YarnCode { get; set; }
        public string MUSymbol { get; set; }
        public string OrderRecapNo { get; set; }
        public string StyleNo { get; set; }
        public string BuyerName { get; set; }
        public string ColorName { get; set; }
        public double ApproxQty { get; set; }
        public int StyleDefine { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string ApproxQtyInSt
        {
            get
            {
                return Global.MillionFormat(this.ApproxQty);
            }
        }
        #endregion

        #region Functions
        public static List<YarnRequisitionDetail> Gets(long nUserID)
        {
            return YarnRequisitionDetail.Service.Gets(nUserID);
        }
        public static List<YarnRequisitionDetail> Gets(string sSQL, long nUserID)
        {
            return YarnRequisitionDetail.Service.Gets(sSQL, nUserID);
        }
        public static List<YarnRequisitionDetail> Gets(int nYarnRequisitionID, long nUserID)
        {
            return YarnRequisitionDetail.Service.Gets(nYarnRequisitionID, nUserID);
        }
        public YarnRequisitionDetail Get(int id, long nUserID)
        {
            return YarnRequisitionDetail.Service.Get(id, nUserID);
        }
        public YarnRequisitionDetail Save(long nUserID)
        {
            return YarnRequisitionDetail.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return YarnRequisitionDetail.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IYarnRequisitionDetailService Service
        {
            get { return (IYarnRequisitionDetailService)Services.Factory.CreateService(typeof(IYarnRequisitionDetailService)); }
        }
        #endregion
    }
    #endregion

    #region IYarnRequisitionDetail interface
    public interface IYarnRequisitionDetailService
    {
        YarnRequisitionDetail Get(int id, Int64 nUserID);
        List<YarnRequisitionDetail> Gets(Int64 nUserID);
        List<YarnRequisitionDetail> Gets(int nYarnRequisitionID, Int64 nUserID);
        List<YarnRequisitionDetail> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        YarnRequisitionDetail Save(YarnRequisitionDetail oYarnRequisitionDetail, Int64 nUserID);
    }
    #endregion
}