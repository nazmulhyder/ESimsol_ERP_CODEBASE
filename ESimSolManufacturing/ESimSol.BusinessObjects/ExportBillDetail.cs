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
    #region ExportBillDetail
    [DataContract]
    public class ExportBillDetail : BusinessObject
    {
        private object obj = null;
        #region  Constructor
        public ExportBillDetail()
        {
            ExportBillDetailID = 0;
            ExportBillID = 0;
            ProductID = 0;
            UnitPrice = 0;
            Qty = 0;
            WtPerBag = 50; // Pls Get it from Report Setup, (Make Export Bill Report Setup) 
            StyleRef = "";
            PIDate = DateTime.Now;
            ProductDesciption = "";
            VersionNo = 0;
            ExportBillDetails = new List<ExportBillDetail>();
            RateUnit = 0;
            IsApplySizer = false;
            ColorID = 0;
            ColorQty = 0;
            ProductCategoryID = 0;
            ProductCategoryName = "";
            UnitPriceTwo = 0;
            ProcessTypeName = "";
            FabricWeaveName = "";
            FinishTypeName = "";
            BuyerReference = "";
            IsDeduct = false;
            NoOfBag = 0;
            ReferenceCaption = "";
            Weight = "";
            Shrinkage = "";
            SLNo = 0;
        }
        #endregion

        #region Properties
        public int ExportBillDetailID { get; set; }
        public int ExportBillID { get; set; }
        public int ProductID { get; set; }
        public double Qty { get; set; }
        public int RateUnit { get; set; }
        public double QtyTwo { get; set; }
        public double UnitPrice { get; set; }
        public double UnitPriceTwo { get; set; }
        public double WtPerBag { get; set; }
        public int MUnitID { get; set; }
        public string MUName { get; set; }
        public string PINo { get; set; }
        public string ColorName { get; set; }
        public string ProductDescription { get; set; }
        public string SizeName { get; set; }
        public int ModelReferenceID { get; set; }
        public string ModelReferenceName { get; set; }
        public string Measurement { get; set; }
        public string ColorInfo { get; set; }
        public string HSCode { get; set; }
        public int VersionNo { get; set; }
        public string Currency { get; set; }
        public string ErrorMessage { get; set; }
        public string ProductDesciption { get; set; }
        public bool IsApplySizer { get; set; }
        public bool IsDeduct { get; set; }
        public int ColorID { get; set; }
        public int ColorQty { get; set; }
        public DateTime PIDate { get; set; }
        public int ProductCategoryID { get; set; }
        public string ProductCategoryName { get; set; }
        public string BuyerReference { get; set; }
        public double NoOfBag { get; set; }
        public string ReferenceCaption { get; set; }
        
        //#region  NoOfBag
        //private int _nNoOfBag = 0;
        //public int NoOfBag
        //{
        //    get
        //    {

        //        if (this.Qty > 0 && WtPerBag > 0)
        //        {
        //            _nNoOfBag = Convert.ToInt32(Math.Ceiling((this.Qty / this.WtPerBag)));
        //        }
        //        return _nNoOfBag;
        //    }
        //}
        //#endregion
        public int ExportPIDetailID { get; set; }
        public string FabricNo { get; set; }
        public string Construction { get; set; }
        public string FabricWidth { get; set; }
        public string ProcessTypeName { get; set; }
        public string FabricWeaveName { get; set; }
        public string FinishTypeName { get; set; }

        #region Derived PRoperty

        public ExportBill ExportBill { get; set; }
        public List<ExportBillDetail> ExportBillDetails { get; set; }
        public int ReviseNo { get; set; }
        #region PINo_Full
        private string _sPINo_Full = "";
        public string PINo_Full
        {
            get
            {
                if (this.ReviseNo > 0)
                {

                    _sPINo_Full = this.PINo + "R-" + this.ReviseNo;
                }
                else
                {
                    _sPINo_Full = this.PINo;
                }
                return _sPINo_Full;
            }
        }
        #endregion
        public int SLNo { get; set; }

        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string StyleRef { get; set; }
        public string StyleNo { get; set; }
        public string Weight { get; set; }
        public string Shrinkage { get; set; }
        public string ProductNameCode
        {
            get
            {
                return ProductName + "[" + ProductCode + "]";
            }
        }
        public string PIDateSt
        {
            get
            {
                DateTime year2000 = new DateTime(2000, 1, 1);
                if (this.PIDate == DateTime.MinValue) return "--";
                else if (this.PIDate < year2000) return "--";
                else return this.PIDate.ToString("dd MMM yyyy");
            }
        }


        #region Amount
        public double Amount
        {
            get
            {
                if (this.RateUnit <= 1)
                {
                    return (this.UnitPrice * this.Qty);
                }
                else
                {
                    return (this.UnitPrice * (this.Qty / this.RateUnit));
                }


            }
        }
        #endregion
                
        #region AmountSt
        string sAmount = "";
        public string AmountSt
        {
            get
            {
                if (this.IsDeduct)
                {
                    sAmount = "("+this.Currency + "" + Global.MillionFormat(this.Amount)+")";
                }
                else
                {
                    sAmount = this.Currency + "" + Global.MillionFormat(this.Amount);
                }
                return sAmount;
            }
        }
        #endregion

        public string UnitPriceSt
        {
            get
            {
                if (this.RateUnit <= 1)
                {
                    return Global.MillionFormatActualDigit(this.UnitPrice);
                }
                else
                {
                    return Global.MillionFormatActualDigit(this.UnitPrice) + "/" + this.RateUnit.ToString();
                }
            }
        }

        #endregion

        #endregion


        #region Functions
        public ExportBillDetail Get(int nId, Int64 nUserID)
        {
            return ExportBillDetail.Service.Get(nId, nUserID);
        }
        public ExportBillDetail GetPIP(int nPIProductID, Int64 nUserID)
        {
            return ExportBillDetail.Service.GetPIP(nPIProductID, nUserID);
        }
        public string Delete(Int64 nUserID)
        {
            return ExportBillDetail.Service.Delete(this, nUserID);
        }
        public ExportBillDetail Save(Int64 nUserID)
        {
            return ExportBillDetail.Service.Save(this, nUserID);
        }
        public ExportBillDetail SaveMultipleBills(Int64 nUserID)
        {
            return ExportBillDetail.Service.SaveMultipleBills(this, nUserID);
        }
        #region  Collection Functions

        public static List<ExportBillDetail> Gets(int nLCBillID, Int64 nUserID)
        {
            return ExportBillDetail.Service.Gets(nLCBillID, nUserID);
        }
        public static List<ExportBillDetail> GetsBySQL(string sSQL, Int64 nUserID)
        {
            return ExportBillDetail.Service.GetsBySQL(sSQL, nUserID);
        }
       

        #region Non DB Members
        public static double TotalQty(List<ExportBillDetail> oExportBillDetails)
        {
            double nTotalQty = 0;
            foreach (ExportBillDetail oitem in oExportBillDetails)
            {
                nTotalQty = nTotalQty + oitem.Qty;
            }
            return nTotalQty;
        }


        #endregion

        #endregion
        #endregion
        #region ServiceFactory

        internal static IExportBillDetailService Service
        {
            get { return (IExportBillDetailService)Services.Factory.CreateService(typeof(IExportBillDetailService)); }
        }

        #endregion

    }
    #endregion

    #region IExportBillDetail interface
    [ServiceContract]
    public interface IExportBillDetailService
    {
        ExportBillDetail Get(int id, Int64 nUserID);
        ExportBillDetail GetPIP(int nPIProductID, Int64 nUserID);
        List<ExportBillDetail> Gets(int nLCBillID, Int64 nUserID);
        List<ExportBillDetail> GetsBySQL(string sSQL, Int64 nUserID);
        string Delete(ExportBillDetail oExportBillDetail, Int64 nUserID);
        ExportBillDetail Save(ExportBillDetail oExportLC, Int64 nUserID);
        ExportBillDetail SaveMultipleBills(ExportBillDetail oExportLC, Int64 nUserID);
    }
    #endregion
}
