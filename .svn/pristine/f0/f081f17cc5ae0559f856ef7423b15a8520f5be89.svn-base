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
    #region NOAQuotation
    public class NOAQuotation : BusinessObject
    {
        public NOAQuotation()
        {
            NOAQuotationLogID = 0;
            NOADetailLogID = 0;
            NOAQuotationID = 0;
            UnitPrice  = 0;
            NOAID = 0;
            PQID = 0;
            SupplierID = 0;
            SupplierName = "";
            ShortName = "";
            NOADetailID = 0;
            PQDetailID = 0;
            bIsExist = true;
            ErrorMessage = "";
            NOAQuotations = new List<NOAQuotation>();
            DiscountInpercent=0;
            DiscountInAmount = 0;
            VatInPercent = 0;
            VatInAmount = 0;
            TransportCostInPercent = 0;
            TransportCostInAmount = 0;
        }

        #region Properties
        public int NOAQuotationLogID { get; set; }
        public int NOADetailLogID { get; set; }
        public int NOAQuotationID { get; set; }
        public int NOAID { get; set; }
        public int PQID { get; set; }
        public int   NOADetailID { get; set; }
        public int PQDetailID { get; set; }
        public string SupplierName { get; set; }
        public string ShortName { get; set; }
        public int SupplierID { get; set; }
        public double UnitPrice { get; set; }
        public string PQNo { get; set; }
        public bool bIsExist { get; set; }
        public string ErrorMessage { get; set; }
        public double DiscountInpercent { get; set; } 
        public double DiscountInAmount { get; set; }
        public double VatInPercent { get; set; }
        public double VatInAmount { get; set; }
        public double TransportCostInPercent { get; set; }
        public double TransportCostInAmount { get; set; }
        #endregion

        #region Derived
       public int Maxcount { get; set; }
        public string Specifications { get; set; }
        public List<NOAQuotation> NOAQuotations { get; set; }
     
       
        #endregion

        #region Functions
        public static List<NOAQuotation> Gets(int nUserID)
        {
            return NOAQuotation.Service.Gets(nUserID);
        }
        public NOAQuotation Get(int id, int nUserID)
        {
            return NOAQuotation.Service.Get(id, nUserID);
        }

        public static List<NOAQuotation> Save(NOAQuotation oNOAQuotation, int nUserID)
        {
            return NOAQuotation.Service.Save(oNOAQuotation, nUserID);
        }
        public static List<NOAQuotation> Gets(long nNOADetailID, int nUserID)
        {
            return NOAQuotation.Service.Gets(nNOADetailID, nUserID);
        }
        public string Delete(int nNOAQuotationID,  int nUserID)
        {
            return NOAQuotation.Service.Delete(nNOAQuotationID, nUserID);
        }
        public static List<NOAQuotation> Gets(string sSQL, int nUserID)
        {
            return NOAQuotation.Service.Gets(sSQL, nUserID);
        }
        public static List<NOAQuotation> GetsByLog(string sSQL, int nUserID)
        {
            return NOAQuotation.Service.GetsByLog(sSQL, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static INOAQuotationService Service
        {
            get { return (INOAQuotationService)Services.Factory.CreateService(typeof(INOAQuotationService)); }
        }
        #endregion
    }
    #endregion

    #region INOAQuotation interface
    public interface INOAQuotationService
    {
        NOAQuotation Get(int id, int nUserID);
        List<NOAQuotation> Gets(int nUserID);
        List<NOAQuotation> Gets(string sSQL, int nUserID);
        List<NOAQuotation> GetsByLog(string sSQL, int nUserID);
        List<NOAQuotation> Gets(long nNOADetailID, int nUserID);
        string Delete(int nNOAQuotationID,  int nUserID);
       List<NOAQuotation> Save(NOAQuotation oNOAQuotation, int nUserID);
    }
    #endregion
    
  
}
