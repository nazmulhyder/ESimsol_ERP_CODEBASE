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
    #region ReturnChallanRegister
    public class ReturnChallanRegister : BusinessObject
    {
        public ReturnChallanRegister()
        {
            ReturnChallanDetailID = 0;
            ReturnChallanID = 0;
            DeliveryChallanDetailID = 0;
            ProductCategoryID = 0;
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
            ContractorName = "";
            ReceivedByName = "";
            BUID = 0;
            Remarks = "";
            ReturnDate = DateTime.Today;
            ErrorMessage = "";
        }

        #region Property
        public int ReturnChallanDetailID { get; set; }
        public int ReturnChallanID { get; set; }
        public int DeliveryChallanID { get; set; }
        public int ProductCategoryID { get; set; }
        public string PINo { get; set; }
        public string DeliveryChallanNo { get; set; }
        public string Remarks { get; set; }
        public int DeliveryChallanDetailID { get; set; }
        public int LotID { get; set; }
        public int ProductID { get; set; }
        public int MUnitID { get; set; }
        public double Qty { get; set; }
        public string ContractorName { get; set; }
        public string ReceivedByName { get; set; }
        public int BUID { get; set; }
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
        public EnumReportLayout ReportLayout { get; set; }
        public string SearchingData { get; set; }
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


        public static List<ReturnChallanRegister> Gets(string sSQL, long nUserID)
        {
            return ReturnChallanRegister.Service.Gets(sSQL, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IReturnChallanRegisterService Service
        {
            get { return (IReturnChallanRegisterService)Services.Factory.CreateService(typeof(IReturnChallanRegisterService)); }
        }
        #endregion
    }
    #endregion

    #region IReturnChallanDetail interface
    public interface IReturnChallanRegisterService
    {

        List<ReturnChallanRegister> Gets(string sSQL, Int64 nUserID);

    }
    #endregion
}
