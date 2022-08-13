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
    #region DeliveryChallanDetail
    public class DeliveryChallanDetail : BusinessObject
    {
        public DeliveryChallanDetail()
        {
            DeliveryChallanDetailID = 0;
            DeliveryChallanID = 0;
            DODetailID = 0;
            LotID = 0;
            ProductID = 0;
            MUnitID = 0;
            Qty = 0;
            BagQty = 0;
            Note = string.Empty;
            ColorName = "";
            SizeName = "";
            ProductDescription = "";
            ModelReferenceName = "";
            PTUUnit2DistributionID = 0;
            StyleNo = "";
            YetToReturnQty = 0;
            ReportingQty = 0;
            ReportingUnit = "";
            ReferenceCaption = "";
            ProductCode = "";
            QtyPerCarton = 0;
            PINo = "";
            DONo = "";
            ChallanNo = "";
            Measurement = "";
            ErrorMessage = "";
        }

        #region Property
        public int DeliveryChallanDetailID { get; set; }
        public int DeliveryChallanID { get; set; }
        public int DODetailID { get; set; }
        public int LotID { get; set; }
        public int ProductID { get; set; }
        public int MUnitID { get; set; }
        public double Qty { get; set; }
        public int BagQty { get; set; }
        public string Note { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public string ProductDescription { get; set; }
        public string ModelReferenceName { get; set; }
        public double YetToDeliveryOrderQty { get; set; }
        public int PTUUnit2DistributionID { get; set; }
        public string StyleNo { get; set; }
        public double ReportingQty { get; set; }
        public string ReportingUnit { get; set; }
        public double YetToReturnQty { get; set; }
        public string ReferenceCaption { get; set; }
        public int QtyPerCarton { get; set; }
        public string ChallanNo { get; set; }
        public string  PINo { get; set; }
        public string DONo { get; set; }
        public DateTime ChallanDate { get; set; }
        public string Measurement { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string MUnit { get; set; }
        public string LotNo { get; set; }
        public string ProductWithColorName
        {
            get
            {
                if(this.ColorName!="" && this.ColorName!=null)
                {
                    return this.ProductName + "[" + this.ColorName + "]";
                }
                else
                {
                    return this.ProductName;
                        
                }
            }
        }
        public string ChallanDateInString
        {
            get
            {
                return this.ChallanDate.ToString("dd MMM yyyy");
            }
        }
        public string QtyInstring
        {
            get
            {
                return Global.MillionFormatActualDigit(this.Qty) + " " + this.MUnit;
            }
        }
        #endregion

        #region Functions

        public DeliveryChallanDetail Get(int id, long nUserID)
        {
            return DeliveryChallanDetail.Service.Get(id, nUserID);
        }
        public static List<DeliveryChallanDetail> Gets(int nDOID, long nUserID)
        {
            return DeliveryChallanDetail.Service.Gets(nDOID, nUserID);
        }

        public static List<DeliveryChallanDetail> Gets(string sSQL, long nUserID)
        {
            return DeliveryChallanDetail.Service.Gets(sSQL, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IDeliveryChallanDetailService Service
        {
            get { return (IDeliveryChallanDetailService)Services.Factory.CreateService(typeof(IDeliveryChallanDetailService)); }
        }
        #endregion
    }
    #endregion

    #region IDeliveryChallanDetail interface
    public interface IDeliveryChallanDetailService
    {
        DeliveryChallanDetail Get(int id, Int64 nUserID);
        List<DeliveryChallanDetail> Gets(int nDOID, Int64 nUserID);
        List<DeliveryChallanDetail> Gets(string sSQL, Int64 nUserID);

    }
    #endregion
}
