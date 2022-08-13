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
    #region BDYEAC
    
    public class BDYEAC : BusinessObject
    {
        public BDYEAC()
        {
            BDYEACID = 0;
            ExportBillID = 0;
            ExportLCID= 0;
            BankName = "";
            InvoiceDate = DateTime.Now;
            DeliveryDate = DateTime.Now;
            SupplierName = "";
            ImportLCNo = "";
            ImportLCDate = DateTime.Now;
            ExportBillNo = "";
            BillAmount= 0;
            ExportLCNo  ="";
            LCOpeningDate = DateTime.Now;
            PartyName = "";
            PartyAddress = "";
            BUID = 0;
            BUName = "";
            BUAddress = "";
            BUShortName = "";
            MasterLCNos = "";
            MasterLCDates = "";
            GarmentsQty = "";
            ExportLCAmount = 0;
            Amount = 0;
            IsPrint = false;
            ErrorMessage = "";
            BDYEACs = new List<BDYEAC>();
            BDYEACDetails = new  List<BDYEACDetail>();
            ImportInvoiceDetailID = 0;
            ImportLCID = 0;
        }

        #region Properties
        
        public int BDYEACID { get; set; }

        public int ExportBillID { get; set; }

        public int ExportLCID { get; set; }
        public string MasterLCDates { get; set; }
        public string BankName { get; set; }
        public string GarmentsQty { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime ImportLCDate { get; set; }
        public DateTime LCOpeningDate { get; set; }
        public string SupplierName { get; set; }
        public string ExportBillNo { get; set; }
        public string ImportLCNo { get; set; }
        public double BillAmount { get; set; }
        public string MasterLCNos { get; set; }
         public string ExportLCNo { get; set; }
         public string PartyName { get; set; }
         public string PartyAddress { get; set; }
         public string BUName { get; set; }
        public string BUAddress { get; set; }
        public string BUShortName { get; set; }
        public int BUID { get; set; }
        public int ImportLCID { get; set; }
        public int ContractorID { get; set; }
        public int ImportInvoiceDetailID { get; set; }
        public double ExportLCAmount { get; set; }
        public double Amount { get; set; }
        public bool IsPrint { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property

        public List<BDYEACDetail> BDYEACDetails { get; set; }
        public List<BDYEAC> BDYEACs { get; set; }
        public BusinessUnit BusinessUnit { get; set; }
        public Company Company { get; set; }
        public string  IsPrintInString
        {
            get
            {
                if(this.IsPrint)
                {
                    return "Yes";
                }
                else
                {
                    return "No";
                }
            }
        }
         public string InvoiceDateInString
            {
                get
                {
                  return  this.InvoiceDate.ToString("dd MMM yyyy");
                }
            }
         public string DeliveryDateInString
         {
             get
             {
                 return this.DeliveryDate.ToString("dd MMM yyyy");
             }
         }
         public string ImportLCDateInString
         {
             get
             {
                 return this.ImportLCDate.ToString("dd MMM yyyy");
             }
         }
         public string LCOpeningDateInString
         {
             get
             {
                 return this.LCOpeningDate.ToString("dd MMM yyyy");
             }
         }

       
        #endregion
         #region Non DB function 
         public string GetMLCNoORDates(string sMLCNoORDates)
         {
             string sReturn = "";
             string [] st = sMLCNoORDates.Split(',');
             for (int i = 0; i<st.Length-1;i++ )
             {
                
                     sReturn += st[i] + ",";
             }
            if(st.Length>1)
            {
                sReturn = sReturn.Substring(0, sReturn.Length - 1);
                sReturn += " And " + st[st.Length];
            }
             return sReturn;
         }
         #endregion

         #region Functions

         public static List<BDYEAC> Gets(long nUserID)
        {
            return BDYEAC.Service.Gets(nUserID);
        }
        public static List<BDYEAC> Gets(string sSQL, Int64 nUserID)
        {
            return BDYEAC.Service.Gets(sSQL, nUserID);
        }
     
        public BDYEAC Get(int nId, long nUserID)
        {
            return BDYEAC.Service.Get(nId,nUserID);
        }
               
        public BDYEAC Save(long nUserID)
        {
            return BDYEAC.Service.Save(this, nUserID);
        }

        public BDYEAC CreatePrint(long nUserID)
        {
            return BDYEAC.Service.CreatePrint(this, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return BDYEAC.Service.Delete(nId, nUserID);
        }
    

        #endregion

        #region ServiceFactory
        internal static IBDYEACService Service
        {
            get { return (IBDYEACService)Services.Factory.CreateService(typeof(IBDYEACService)); }
        }
        #endregion
    }
    #endregion

    #region IBDYEAC interface
    
    public interface IBDYEACService
    {
        BDYEAC Get(int id, long nUserID);
        List<BDYEAC> Gets(long nUserID);
        List<BDYEAC> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, long nUserID);
        BDYEAC Save(BDYEAC oBDYEAC, long nUserID);

        BDYEAC CreatePrint(BDYEAC oBDYEAC, long nUserID);
        
    }
    #endregion
}