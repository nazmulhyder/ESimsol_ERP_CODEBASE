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
    #region ImportPack
    public class ImportPack : BusinessObject
    {
        #region  Constructor
        public ImportPack()
        {
            ImportPackID = 0;
            PackNo = "";
            PackDate = DateTime.Now;
            ImportInvoiceID = 0;
            //LoadingPortID = 0;
            GrossWeight = 0;
            TotalPack = 0;
            //DischargePortID = 0;
            Remarks = "";
            //LoadingPortName = "";
            ImportLCNo = "";
            PackCountBy = EnumPackCountBy.Bales;
            ImportPackDetails = new List<ImportPackDetail>();
            ImportPacks = new List<ImportPack>();
            Origin = "";
            Brand = "";
            MUnitID = 0;
            InvoiceQty = 0;
            Qty = 0;
            UnitConValue = 0;
            MUNameTwo = "";
        }

        #endregion

        #region Properties
        public int ImportPackID { get; set; }
        public string PackNo { get; set; }
        public DateTime PackDate { get; set; }
        public int ImportInvoiceID { get; set; }
        public double UnitPrice { get; set; }
        public double GrossWeight { get; set; }
        public double TotalPack { get; set; }
        public double NetWeight { get; set; }
        public int MUnitID { get; set; } 
        public EnumPackCountBy PackCountBy { get; set; }        
        public string Remarks { get; set; }
        public string ImportInvoiceNo { get; set; }
        public string ImportLCNo { get; set; }
        public string Origin { get; set; }
        public string Brand { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string MUName { get; set; }
        public string LotNo { get; set; }
        public int ProductID { get; set; }
        public int PackCountByInInt { get; set; }
        public string ErrorMessage { get; set; }
        public int ParentPackID { get; set; }
        public double Qty { get; set; }
        public double InvoiceQty { get; set; }
        #region Derive property
        public string MUNameTwo { get; set; }
        public double UnitConValue { get; set; }
        public List<ImportPackDetail> ImportPackDetails { get; set; }
        public List<ImportPack> ImportPacks { get; set; }

        #endregion

        public double NetWeightTwo
        {
            get
            {
                return (this.NetWeight * this.UnitConValue);
            }

        }

        public string PackCountByInString
        {
            get
            {
                return this.PackCountBy.ToString();
            }

        }
        public string PackDateInST
        {
            get
            {
                if (this.PackDate == DateTime.MinValue)
             {
                 return "-";
             }
             else
             {
                 return PackDate.ToString("dd MMM yyyy");
             }
            }
        }
        
       
        #endregion

        #region Function New Version
        public ImportPack Save(int nUserID)
        {
            return ImportPack.Service.Save(this, nUserID);
        }
        public string Delete(int nUserID)
        {
            return ImportPack.Service.Delete(this, nUserID);
        }
        public string Save_FromDO(int nUserID)
        {
            return ImportPack.Service.Save_FromDO(this, nUserID);
        }
        public ImportPack Get(int nImportPackID, int nUserID)
        {
            return ImportPack.Service.Get(nImportPackID, nUserID);
        }
        public ImportPack GetByInvoice(int nInvoiceID, int nUserID)
        {
            return ImportPack.Service.GetByInvoice(nInvoiceID, nUserID);
        }
        public static List<ImportPack> Gets(int nInvoiceID,int nUserID)
        {
            return ImportPack.Service.Gets(nInvoiceID,nUserID);
        }
        public static List<ImportPack> Gets(string sSQL, int nUserID)
        {
            return ImportPack.Service.Gets(sSQL, nUserID);
        }

        #endregion


        #region ServiceFactory
        internal static IImportPackService Service
        {
            get { return (IImportPackService)Services.Factory.CreateService(typeof(IImportPackService)); }
        }
        #endregion

    }
    #endregion

    #region IImportPack interface
    public interface IImportPackService
    {
       string Delete(ImportPack oImportLC, Int64 nUserID);
       ImportPack Save(ImportPack oImportPack, Int64 nUserID);
       ImportPack Get(int nImportPackID, Int64 nUserID);
       ImportPack GetByInvoice(int nImportInvoiceID, Int64 nUserID);
       List<ImportPack> Gets(int nInvoiceID, Int64 nUserID);
       List<ImportPack> Gets(string sSQL, Int64 nUserID);
       string Save_FromDO(ImportPack oImportLC, Int64 nUserID);
        
    }
    #endregion
}
