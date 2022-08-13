using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{

    #region ProformaInvoiceRequiredDoc
    
    public class ProformaInvoiceRequiredDoc : BusinessObject
    {
        public ProformaInvoiceRequiredDoc()
        {

            ProformaInvoiceRequiredDocID = 0;
            ProformaInvoiceID = 0;
            DocType = EnumDocumentType.None;
            ProformaInvoiceRequiredDocLogID = 0;
            ProformaInvoiceLogID = 0;
            ErrorMessage = "";
        }

        #region Properties
         
        public int ProformaInvoiceRequiredDocID { get; set; }
         
        public int ProformaInvoiceID { get; set; }
         
        public EnumDocumentType DocType { get; set; }
         
        public int ProformaInvoiceLogID { get; set; }
         
        public int ProformaInvoiceRequiredDocLogID { get; set; }
         
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public int DocTypeInInt { get; set; }
        public string DocTypeInString
        {
            get
            {
                return EnumObject.jGet(this.DocType);
            }
        }
        #endregion

        #region Functions

        public static List<ProformaInvoiceRequiredDoc> Gets(int id, long nUserID)
        {
            return ProformaInvoiceRequiredDoc.Service.Gets(id, nUserID);
        }

        public static List<ProformaInvoiceRequiredDoc> GetsPILog(int id, long nUserID) // id is PI Log Id
        {
            return ProformaInvoiceRequiredDoc.Service.GetsPILog(id, nUserID);
        }
        public static List<ProformaInvoiceRequiredDoc> Gets(string sSQL, long nUserID)
        {
             return ProformaInvoiceRequiredDoc.Service.Gets(sSQL, nUserID);
        }

        public ProformaInvoiceRequiredDoc Get(int id, long nUserID)
        {
            return ProformaInvoiceRequiredDoc.Service.Get(id, nUserID);
        }

        public ProformaInvoiceRequiredDoc Save(long nUserID)
        {
            return ProformaInvoiceRequiredDoc.Service.Save(this, nUserID);
        }
        public string ProformaInvoiceRequiredDocSave(List<ProformaInvoiceRequiredDoc> oProformaInvoiceRequiredDocs, long nUserID)
        {
            return ProformaInvoiceRequiredDoc.Service.ProformaInvoiceRequiredDocSave(oProformaInvoiceRequiredDocs, nUserID);

        }
        public string Delete(int id, long nUserID)
        {
            return ProformaInvoiceRequiredDoc.Service.Delete(id, nUserID);
        }



        #endregion

        #region ServiceFactory

 
        internal static IProformaInvoiceRequiredDocService Service
        {
            get { return (IProformaInvoiceRequiredDocService)Services.Factory.CreateService(typeof(IProformaInvoiceRequiredDocService)); }
        }

        #endregion
    }
    #endregion

    #region IProformaInvoiceRequiredDoc interface
     
    public interface IProformaInvoiceRequiredDocService
    {
         
        ProformaInvoiceRequiredDoc Get(int id, Int64 nUserID);
         
        List<ProformaInvoiceRequiredDoc> Gets(int id,Int64 nUserID);
         
        List<ProformaInvoiceRequiredDoc> GetsPILog(int id, Int64 nUserID);
         
        List<ProformaInvoiceRequiredDoc> Gets(string sSQL, Int64 nUserID);
         
        ProformaInvoiceRequiredDoc Save(ProformaInvoiceRequiredDoc oProformaInvoiceRequiredDoc, Int64 nUserID);
         
        string ProformaInvoiceRequiredDocSave(List<ProformaInvoiceRequiredDoc> oProformaInvoiceRequiredDocs, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);

    }
    #endregion
}
