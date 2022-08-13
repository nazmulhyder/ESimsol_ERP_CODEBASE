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
    #region ReturnChallanDetail
    public class ReturnChallanDetail : BusinessObject
    {
        public ReturnChallanDetail()
        {
            ReturnChallanDetailID = 0;
            ReturnChallanID = 0;
            DeliveryChallanDetailID = 0;
            LotID = 0;
            ProductID = 0;
            MUnitID = 0;
            Qty = 0;
            DeliveryChallanQty = 0;
            YetToReturnQty = 0;
            Note = string.Empty;
            ColorName = "";
            DONo = "";
            ReturnChallanNo = "";
            Symbol = "";
            ReturnDate = DateTime.Today;
            ErrorMessage = "";
        }

        #region Property
        public int ReturnChallanDetailID { get; set; }
        public int ReturnChallanID { get; set; }
        public int DeliveryChallanID { get; set; }
        public string PINo { get; set; }
        public string DeliveryChallanNo { get; set; }
        public int DeliveryChallanDetailID { get; set; }
        public int LotID { get; set; }
        public int ProductID { get; set; }
        public int MUnitID { get; set; }
        public double Qty { get; set; }
        public string Note { get; set; }
        public double DeliveryChallanQty { get; set; }
        public string DONo { get; set; }
        public double YetToReturnQty { get; set; }
        public string ColorName { get; set; }
        public string ReturnChallanNo { get; set; }
        public string Symbol { get; set; }
        public DateTime ReturnDate { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string MUnit { get; set; }
        public string LotNo { get; set; }
        public string ReturnDateInString
        {
            get
            {
                return this.ReturnDate.ToString("dd MMM yyyy");
            }
        }

        public string ProductWithColorName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.ColorName))
                {
                    return this.ProductName + "[" + this.ColorName + "]";
                }
                else
                {
                    return this.ProductName;
                }
            }
        }
        public string QtyInString
        {
            get
            {
                return Global.MillionFormatActualDigit(this.Qty)+" "+this.MUnit;
            }
        }
        #endregion

        #region Functions

        public ReturnChallanDetail Get(int id, long nUserID)
        {
            return ReturnChallanDetail.Service.Get(id, nUserID);
        }
        public static List<ReturnChallanDetail> Gets(int nDOID, long nUserID)
        {
            return ReturnChallanDetail.Service.Gets(nDOID, nUserID);
        }

        public static List<ReturnChallanDetail> Gets(string sSQL, long nUserID)
        {
            return ReturnChallanDetail.Service.Gets(sSQL, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IReturnChallanDetailService Service
        {
            get { return (IReturnChallanDetailService)Services.Factory.CreateService(typeof(IReturnChallanDetailService)); }
        }
        #endregion
    }
    #endregion

    #region IReturnChallanDetail interface
    public interface IReturnChallanDetailService
    {
        ReturnChallanDetail Get(int id, Int64 nUserID);
        List<ReturnChallanDetail> Gets(int nDOID, Int64 nUserID);
        List<ReturnChallanDetail> Gets(string sSQL, Int64 nUserID);

    }
    #endregion
}
