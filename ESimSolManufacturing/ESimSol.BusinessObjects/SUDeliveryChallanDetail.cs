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
    #region SUDeliveryChallanDetail
    public class SUDeliveryChallanDetail : BusinessObject
    {
        public SUDeliveryChallanDetail()
        {
            SUDeliveryChallanDetailID = 0;
            SUDeliveryChallanID = 0;
            SUDeliveryOrderDetailID = 0;
            ProductID = 0;
            LotID = 0;
            MUnitID = 0;
            Qty = 0;
            ProgramQty = 0;
            ProductCode = "";
            ProductName = "";
            ProductShortName = "";
            ErrorMessage = "";
            WorkingUnitID = 0;
            LotNo = "";
            DCDRemark = "";

            MeasurementUnitName = "";
            MeasurementUnitSymbol = "";
            SUDeliveryProgramID = 0;
            ChallanDate = DateTime.Now;
            ChallanNo = "";
            UnitPrice = 0;
            YetToChallan = 0;
            YetToChallanCountWise = 0;
            WUName = "";
            Bags = 0;
            ExportPIID = 0;
            BuyerID = 0;
            RemainingQty = 0;
            ProgramDate = DateTime.Now;
            Remarks = "";
            DEOID = 0;
        }

        #region Properties
        public int SUDeliveryChallanDetailID { get; set; }
        public int SUDeliveryChallanID { get; set; }
        public int SUDeliveryOrderDetailID { get; set; }
        public int ProductID { get; set; }
        public int MUnitID { get; set; }
        public double Qty { get; set; }
        public double UnitPrice { get; set; }
        public double ProgramQty { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductShortName { get; set; }
        public string ErrorMessage { get; set; }
        public int WorkingUnitID { get; set; }
        public int LotID { get; set; }
        public string LotNo { get; set; }
        public string DCDRemark { get; set; }
        public string MeasurementUnitName { get; set; }
        public string MeasurementUnitSymbol { get; set; }
        public int SUDeliveryProgramID { get; set; }
        public DateTime ChallanDate { get; set; }
        public string ChallanNo { get; set; }
        public double YetToChallan { get; set; }
        public double YetToChallanCountWise { get; set; }
        public string WUName { get; set; }
        public int Bags { get; set; }
        public int ExportPIID { get; set; }
        public int BuyerID { get; set; }
        public double RemainingQty { get; set; }
        public string Remarks { get; set; } //Used for SUDeliveryChallanRemark
        public int DEOID { get; set; }
        #endregion

        #region Derived Property
        public DateTime ProgramDate { get; set; } 
        public string YetToChallanSt
        {
            get
            {
                if (this.YetToChallan < 0) return "(" + Global.MillionFormat(this.YetToChallan * (-1)) + ")";
                else return Global.MillionFormat(this.YetToChallan);
            }
        }
        public string YetToChallanCountWiseSt
        {
            get
            {
                if (this.YetToChallanCountWise < 0) return "(" + Global.MillionFormat(this.YetToChallanCountWise * (-1)) + ")";
                else return Global.MillionFormat(this.YetToChallanCountWise);
            }
        }
        public string QtySt
        {
            get
            {
                return Global.MillionFormat(this.Qty);
            }
        }
        public string QtyLbsSt
        {
            get
            {
                return Global.MillionFormat(Global.GetLBS(this.Qty,2));
            }
        }
        
        public string ChallanDateSt
        {
            get
            {
                return this.ChallanDate.ToString("dd MMM yyyy");
            }
        }

        public string ProgramDateSt
        {
            get
            {
                return this.ProgramDate.ToString("dd MMM yyyy");
            }
        }

        public string LotNoWithStore
        {
            get
            {
                if (string.IsNullOrEmpty(this.LotNo))
                {
                    return "";
                }
                else {
                    return this.LotNo + " (" + this.WUName + ")";
                }
            }
        }

        public string BagsSt
        {
            get
            {
                if (this.Bags == 0)
                {
                    return "-";
                }
                else {
                    return this.Bags.ToString();
                }
            }
        }

        #endregion

        #region Functions
        public static List<SUDeliveryChallanDetail> Gets(int nSUDeliveryChallanID, long nUserID)
        {
            return SUDeliveryChallanDetail.Service.Gets(nSUDeliveryChallanID, nUserID);
        }
        public static List<SUDeliveryChallanDetail> Gets(string sSQL, long nUserID)
        {
            return SUDeliveryChallanDetail.Service.Gets(sSQL, nUserID);
        }
        public SUDeliveryChallanDetail Get(int nSUDeliveryChallanId, long nUserID)
        {
            return SUDeliveryChallanDetail.Service.Get(nSUDeliveryChallanId, nUserID);
        }
        public SUDeliveryChallanDetail Save(long nUserID)
        {
            return SUDeliveryChallanDetail.Service.Save(this, nUserID);
        }
        public string Delete(int nSUDeliveryChallanId, long nUserID)
        {
            return SUDeliveryChallanDetail.Service.Delete(nSUDeliveryChallanId, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ISUDeliveryChallanDetailService Service
        {
            get { return (ISUDeliveryChallanDetailService)Services.Factory.CreateService(typeof(ISUDeliveryChallanDetailService)); }
        }
        #endregion

    }
    #endregion

    #region ISUDeliveryChallanDetail interface
    public interface ISUDeliveryChallanDetailService
    {
        List<SUDeliveryChallanDetail> Gets(int nSUDeliveryChallanID, long nUserID);
        List<SUDeliveryChallanDetail> Gets(string sSQL, long nUserID);
        SUDeliveryChallanDetail Get(int nSUDeliveryChallanId, long nUserID);
        SUDeliveryChallanDetail Save(SUDeliveryChallanDetail oSUDeliveryChallanDetail, long nUserID);
        string Delete(int nSUDeliveryChallanId, long nUserID);
    }
    #endregion
}
