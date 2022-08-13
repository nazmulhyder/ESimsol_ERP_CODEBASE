using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Data;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects.ReportingObject
{
    #region ExportPISummary

    public class ExportPISummary : BusinessObject
    {
        #region  Constructor
        public ExportPISummary()
        {
            ExportPIID = 0;
            PINo = "";
            PIDate = new DateTime(1900,01,01);
            BuyerID = 0;
            BuyerName = "";
            ProductID = 0;
            ProductName = "";
            Qty = 0;
            Rate = 0;
            Value = 0;
            DaysToExpire = 0;
            LCID = 0;
            LCNo = "";
            FileNo = "";
            LCDate = new DateTime(1900, 01, 01);
            LCAmdNo = "";
            LCAmdDate = new DateTime(1900, 01, 01);
            InvoiceNo = "";
            InvoiceDate = new DateTime(1900, 01, 01);
            DONo = "";
            DODate = new DateTime(1900, 01, 01);
            ShipmentDate = new DateTime(1900, 01, 01);
            BankName = "";
            BranchName = "";
            MktPersonName = "";
            ErrorMessage = "";
            Params = "";
            LCTName = "";
            LCTNameDays = 0;
            ProductDescription = "";
            DoQty = 0;
            LCRecivedDate = new DateTime(1900, 01, 01);
            ExpiryDate = new DateTime(1900, 01, 01);



            GarmentsName = "";
            Construction = "";
            ProcessType = EnumFabricProcessType.None;
            FabricWeave = EnumFabricWeave.None;
            FinishType = EnumFinishType.None;
            StyleNo = "";
            ColorInfo = "";
            BuyerReference = "";
        }
        #endregion

        #region Properties
        public int ExportPIID { get; set; }
        public string PINo { get; set; }
        public DateTime PIDate { get; set; }
        public int BuyerID { get; set; }
        public string BuyerName { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public double Qty { get; set; }
        public double Rate { get; set; }
        public double Value { get; set; }
        public int DaysToExpire { get; set; }
        public int LCID { get; set; }
        public string LCNo  { get; set; }
        public DateTime LCDate { get; set; }
        public string LCAmdNo { get; set; }
        public DateTime LCAmdDate { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string DONo { get; set; }
        public DateTime DODate { get; set; }
        public DateTime ShipmentDate { get; set; }
        public DateTime LCRecivedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string MktPersonName { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        public string LCTName { get; set; }
        public int LCTNameDays { get; set; }
        public string ProductDescription { get; set; }
        public double DoQty { get; set; }
        public string FileNo { get; set; }




        public string GarmentsName { get; set; }
        public string Construction { get; set; }
        public EnumFabricProcessType ProcessType { get; set; }
        public EnumFabricWeave FabricWeave { get; set; }
        public EnumFinishType FinishType { get; set; }
        public string StyleNo { get; set; }
        public string ColorInfo { get; set; }
        public string BuyerReference { get; set; }

        #endregion

        #region Derive Properties
        public string ProcessTypeSt
        {
            get
            {
                return EnumObject.jGet(this.ProcessType);
            }
        }
        public string FabricWeaveSt
        {
            get
            {
                return EnumObject.jGet(this.FabricWeave);
            }
        }
        public string FinishTypeSt
        {
            get
            {
                //a special text custing was done here for Textile project, here this will not required
                return Enum.GetName(typeof(EnumFinishType), this.FinishType);
            }
        }
        public string StyleColorRef
        {
            get
            {
                return (string.IsNullOrEmpty(this.StyleNo) ? "" : this.StyleNo + ",") + (string.IsNullOrEmpty(this.ColorInfo) ? "" : this.ColorInfo + ",") + this.BuyerReference;
            }
        }

        public string LCAmdNoDateSt
        {
            get
            {
                int nVersionNo = (!string.IsNullOrEmpty(this.LCAmdNo) ? Convert.ToInt32(this.LCAmdNo) : 0);
                return (nVersionNo > 0 ? this.LCAmdDateInStr : "-");
            }
        }
       
        public string NameDaysInString
        {
            get
            {
                if (this.LCTNameDays <= 0)
                {
                    return this.LCTName;
                }
                else
                {
                    return "At " + this.LCTNameDays.ToString() + " Days " + this.LCTName;
                }
            }
        }
        public string ProductNameWithDescription
        {
            get
            {
                return this.ProductName + " " + this.ProductDescription;
            }
        }

        public string ExpiryDateSt
        {
            get
            {
                if (this.LCID <= 0)
                {
                    return "-";
                }
                else
                {
                    DateTime MinValue = new DateTime(1900, 01, 01);
                    DateTime MinValue1 = new DateTime(1905, 01, 01);
                    DateTime MinValue2 = new DateTime(0001, 01, 01);
                    if (this.ExpiryDate == MinValue || this.ExpiryDate == MinValue1 || this.ExpiryDate == MinValue2)
                    {
                        return "-";
                    }
                    else
                    {
                        return ExpiryDate.ToString("dd MMM yyyy");
                    }
                }
            }
        }
        public string LCRecivedDateST
        {
            get
            {
                if (this.LCID <= 0)
                {
                    return "-";
                }
                else
                {
                    DateTime MinValue = new DateTime(1900, 01, 01);
                    DateTime MinValue1 = new DateTime(1905, 01, 01);
                    DateTime MinValue2 = new DateTime(0001, 01, 01);
                    if (this.LCRecivedDate == MinValue || this.LCRecivedDate == MinValue1 || this.LCRecivedDate == MinValue2)
                    {
                        return "-";
                    }
                    else
                    {
                        return LCRecivedDate.ToString("dd MMM yyyy");
                    } 
                }
            }
        }
        public string PIDateInStr 
        { 
            get 
            {
                DateTime MinValue = new DateTime(1900, 01, 01);
                DateTime MinValue1 = new DateTime(0001, 01, 01);
                if (this.PIDate == MinValue || this.PIDate == MinValue1)
                {
                    return "-";
                }
                else
                {
                    return PIDate.ToString("dd MMM yyyy");
                }
            } 
        }
        public string LCDateInStr 
        { 
            get 
            {
                DateTime MinValue = new DateTime(1900, 01, 01);
                DateTime MinValue1 = new DateTime(0001, 01, 01);
                if (this.LCDate == MinValue || this.LCDate == MinValue1)
                {
                    return "-";
                }
                else
                {
                    return LCDate.ToString("dd MMM yyyy");
                }
            } 
        }
        public string LCAmdDateInStr 
        { 
            get 
            {
                DateTime MinValue = new DateTime(1900, 01, 01);
                DateTime MinValue1 = new DateTime(0001, 01, 01);
                if (this.LCAmdDate == MinValue || this.LCAmdDate == MinValue1)
                {
                    return "-";
                }
                else
                {
                    return LCAmdDate.ToString("dd MMM yyyy");
                }
            } 
        }
        public string InvoiceDateInStr 
        { 
            get 
            {
                DateTime MinValue = new DateTime(1900, 01, 01);
                DateTime MinValue1 = new DateTime(0001, 01, 01);
                if (this.InvoiceDate == MinValue || this.InvoiceDate == MinValue1)
                {
                    return "-";
                }
                else
                {
                    return InvoiceDate.ToString("dd MMM yyyy");
                }
            } 
        }
        public string DODateInStr 
        {
            get 
            {
                DateTime MinValue = new DateTime(1900, 01, 01);
                DateTime MinValue1 = new DateTime(0001, 01, 01);
                if (this.DODate == MinValue || this.DODate == MinValue1)
                {
                    return "-";
                }
                else
                {
                    return DODate.ToString("dd MMM yyyy");
                }
            }
        }
        public string ShipmentDateInStr 
        { 
            get 
            {
                DateTime MinValue = new DateTime(1900, 01, 01);
                DateTime MinValue1 = new DateTime(0001, 01, 01);
                if (this.ShipmentDate == MinValue || this.ShipmentDate == MinValue1)
                {
                    return "-";
                }
                else
                {
                    return ShipmentDate.ToString("dd MMM yyyy");
                }
            }
        }

        #endregion

        #region Functions

        public static List<ExportPISummary> Gets(int nTexUnit, string sPINo, int nBuyerID, int nMktPersonID, string sPIStartDate, string sPIEndDate, string sLCRecDateFrom, string sLCRecDateTo, string sDOIssueDateFrom, string sDOIssueDateTo, long nUserID)
        {
            return ExportPISummary.Service.Gets(nTexUnit, sPINo, nBuyerID, nMktPersonID, sPIStartDate, sPIEndDate, sLCRecDateFrom, sLCRecDateTo, sDOIssueDateFrom, sDOIssueDateTo, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IExportPISummaryService Service
        {
            get { return (IExportPISummaryService)Services.Factory.CreateService(typeof(IExportPISummaryService)); }
        }

        #endregion
    }
    #endregion
        


    #region IExportPISummary interface

    public interface IExportPISummaryService
    {

        List<ExportPISummary> Gets(int nTexUnit, string sPINo, int nBuyerID, int nMktPersonID, string sPIStartDate, string sPIEndDate, string sLCRecDateFrom, string sLCRecDateTo, string sDOIssueDateFrom, string sDOIssueDateTo, long nUserID);

    }
    #endregion
}