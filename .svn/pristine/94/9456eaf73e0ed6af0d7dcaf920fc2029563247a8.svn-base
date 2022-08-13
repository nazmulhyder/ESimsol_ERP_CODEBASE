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

    #region NOADetail
    public class NOADetail : BusinessObject
    {
        public NOADetail()
        {
            NOADetailLogID = 0;
            NOALogID = 0;
            NOADetailID = 0;
            NOAID = 0;
            MUnitID = 0;
            ProductID = 0;
            ProductCode = "";
            ProductName = "";
            RequiredQty = 0;
            ApprovedRate = 0;
            Note = "";
            MUnitName = "";
            PurchaseQty = 0;
            ErrorMessage = "";
            Rate = "";
            UnitSymbol = "";
            LPP = 0;
            SupplierID = 0;
            Date = DateTime.Now;
            MPRNO = "";
            NOADetails = new List<NOADetail>();
            NOAQuotationList = new List<NOAQuotation>();
            PRDetailID = 0;
            Specifications = "";
        }

        #region Properties
        public int NOADetailLogID { get; set; }
        public int NOALogID { get; set; }
        public int NOADetailID { get; set; }
        public int NOAID { get; set; }
        public int LPP { get; set; }
        public DateTime Date { get; set; }
        public double PurchaseQty { get; set; }
        public string UnitSymbol { get; set; }
        public string MPRNO { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Rate { get; set; }
        public string ProductSpec { get; set; }
        public int ProductID { get; set; }
        public double RequiredQty { get; set; }
        public double ApprovedRate { get; set; }
        public int MUnitID { get; set; }
        public int PQDetailID { get; set; }
        public string Note { get; set; }
        public string MUnitName { get; set; }
        public int SupplierID { get; set; } 
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived
        public int ContractorID { get; set; }
        public List<NOADetail> NOADetails { get; set; }
        public NOA NOA { get; set; }
        public List<NOAQuotation> NOAQuotationList { get; set; }
        public int PRDetailID { get; set; }
        public string Specifications { get; set; }
        public string DateSt
        {
            get
            {
                return this.Date.ToString("dd.MM.yy");
            }
        }
        #endregion

        #region Functions
        public static List<NOADetail> Gets(int nUserID)
        {
            return NOADetail.Service.Gets(nUserID);
        }
        public static List<NOADetail> Gets(string sSQL, int nUserID)
        {
            return NOADetail.Service.Gets(sSQL,nUserID);
        }
        public NOADetail Get(int id, int nUserID)
        {
            return NOADetail.Service.Get(id, nUserID);
        }
    
        public NOADetail Save(int nUserID)
        {
            return NOADetail.Service.Save(this, nUserID);
        }
        public static List<NOADetail> Gets(long nNOAId, int nUserID)
        {
            return NOADetail.Service.Gets(nNOAId, nUserID);
        }
        public static List<NOADetail> GetsByLog(long nNOAId, int nUserID)
        {
            return NOADetail.Service.GetsByLog(nNOAId, nUserID);
        }
        public static List<NOADetail> GetsBy(int nNOAId, int nContractorID, int nUserID)
        {
            return NOADetail.Service.GetsBy( nNOAId,  nContractorID, nUserID);
        }
        public static List<SupplierRateProcess> Delete(int nNOAID, int nNOADetailID,int nUserID)
        {
            return NOADetail.Service.Delete(nNOAID, nNOADetailID, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static INOADetailService Service
        {
            get { return (INOADetailService)Services.Factory.CreateService(typeof(INOADetailService)); }
        }
        #endregion
    }
    #endregion

    #region INOADetail interface
    public interface INOADetailService
    {
        NOADetail Get(int id, int nUserID);

        List<NOADetail> Gets(int nUserID);
        List<NOADetail> Gets(string sSQL, int nUserID);
        
        List<NOADetail> Gets(long nNOAId, int nUserID);
        List<NOADetail> GetsByLog(long nNOAId, int nUserID);
        List<NOADetail> GetsBy(int nNOAId, int nContractorID, int nUserID);
        List<SupplierRateProcess> Delete(int nNOAID, int nNOADetailID,  int nUserID);
        NOADetail Save(NOADetail oNOADetail, int nUserID);
      
    }
    #endregion

  
    
   
}
