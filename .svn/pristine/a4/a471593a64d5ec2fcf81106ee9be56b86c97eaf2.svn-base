using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region ExportPILCMappingDetail
    public class ExportPILCMappingDetail : BusinessObject
    {
        public ExportPILCMappingDetail()
        {
            ExportPILCMappingID = 0;
            ExportPIID = 0;
            ExportPIDetailID = 0;
            ProductID = 0;
            Qty = 0;
            UnitPrice = 0;
            ProductCode = "";
            ProductName = "";
            RateUnit = 0;
        }

        #region Properties        
        public int ExportPILCMappingID { get; set; }
        public int ExportPIID { get; set; }
        public int ExportPIDetailID { get; set; }        
        public int ProductID { get; set; }
        public double Qty { get; set; }
        public double Qty_Delivery { get; set; }
        public double Qty_Invoice { get; set; }
        public int RateUnit { get; set; }
        public double UnitPrice { get; set; }        
        public int MUnitID { get; set; }    
        #endregion

        #region Derived Property        
        public double EBillQty { get; set; }        
        public string MUName { get; set; }        
        public string ProductCode { get; set; }        
        public string ProductName { get; set; }
        public string ProductNameCode
        {
            get 
            {
                return  this.ProductName + "[" + this.ProductCode + "]";
            }
        }        
        public string PINo { get; set; }
        public string FabricNo { get; set; }
        public string Construction { get; set; }
        public string FabricWidth { get; set; }
        public string ProcessTypeName { get; set; }
        public string FabricWeaveName { get; set; }
        public string FinishTypeName { get; set; }
        public string ColorInfo { get; set; }
        public string StyleRef { get; set; }
        public string StyleNo { get; set; }
        public int ReviseNo { get; set; }
        public bool IsDeduct { get; set; }
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
                    return (this.UnitPrice * (this.Qty/this.RateUnit));
                }
            }
        }
        public string UnitPriceSt
        {
            get
            {
                if (this.RateUnit <= 1)
                {
                    return Global.MillionFormat(this.UnitPrice);
                }
                else
                {
                    return Global.MillionFormat(this.UnitPrice) + "/" + this.RateUnit.ToString();
                }
            }
        }
        #endregion

        #region Functions
        public static List<ExportPILCMappingDetail> GetsBy(int nExportLCID, Int64 nUserID)
        {
            return ExportPILCMappingDetail.Service.GetsBy(nExportLCID, nUserID);            
        }
        public static List<ExportPILCMappingDetail> Gets(string sSql, Int64 nUserID)
        {
            return ExportPILCMappingDetail.Service.Gets(sSql, nUserID);
        }
        #endregion
        
        #region ServiceFactory
        internal static IExportPILCMappingDetailService Service
        {
            get { return (IExportPILCMappingDetailService)Services.Factory.CreateService(typeof(IExportPILCMappingDetailService)); }
        }
        #endregion
    }
    #endregion

    #region IExportPILCMappingDetail interface
    public interface IExportPILCMappingDetailService
    {
        List<ExportPILCMappingDetail> GetsBy(int nExportLCID, Int64 nUserID);
        List<ExportPILCMappingDetail> Gets(string sSql, Int64 nUserID);      
    }
    #endregion
}
