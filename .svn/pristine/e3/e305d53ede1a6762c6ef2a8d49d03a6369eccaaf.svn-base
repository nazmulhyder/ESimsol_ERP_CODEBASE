using System;
using System.IO;
using System.ComponentModel.DataAnnotations;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region CommercialInvoiceDoc
    public class CommercialInvoiceDoc : BusinessObject
    {
        public CommercialInvoiceDoc()
        {
            CommercialInvoiceDocID = 0;
            CommercialInvoiceID = 0;
            DocType = EnumDocumentType.None;
            DocTypeInInt = 0;
            DocName = "";
            Note = "";
            DocFile = null;
            FileType = "";
        
        }
        #region Properties
         
        public int CommercialInvoiceDocID { get; set; }
         
        public int CommercialInvoiceID { get; set; }
         
        public EnumDocumentType DocType { get; set; }
         
        public int DocTypeInInt { get; set; }
         
        public string DocName { get; set; }
         
        public string Note { get; set; }
         
        public byte[] DocFile { get; set; }
         
        public string FileType { get; set; }
        #endregion

        #region derived property
         
        public List<CommercialInvoiceDoc> CommercialInvoiceDocList { get; set; }
         
        //public List<DocumentType>   DocumentTypes { get; set; }
        
        public string AttatchFileinString
        {
            get
            {
                return "Download";
            }
        }
        public string DocTypeInString
        {
            get
            {
                return  EnumObject.jGet(this.DocType); 
            }
        }
        #endregion

        #region Functions
        public CommercialInvoiceDoc Save(long nUserID)
        {
            return CommercialInvoiceDoc.Service.Save(this, nUserID);
        }
        public static List<CommercialInvoiceDoc> Gets(int id, long nUserID)
        {
            return CommercialInvoiceDoc.Service.Gets(id, nUserID);
        }
        public static CommercialInvoiceDoc GetWithDocFile(int id, long nUserID)
        {
            return CommercialInvoiceDoc.Service.GetWithDocFile(id, nUserID);
        }
        public static CommercialInvoiceDoc Get(int id, long nUserID)
        {
            return CommercialInvoiceDoc.Service.Get(id, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return CommercialInvoiceDoc.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ICommercialInvoiceDocService Service
        {
            get { return (ICommercialInvoiceDocService)Services.Factory.CreateService(typeof(ICommercialInvoiceDocService)); }
        }
        #endregion
    }
    #endregion


    #region ICommercialInvoiceDocService interface
     
    public interface ICommercialInvoiceDocService
    {
         
        List<CommercialInvoiceDoc> Gets(int id, Int64 nUserID);
         
        CommercialInvoiceDoc Get(int id, Int64 nUserID);
         
        CommercialInvoiceDoc GetWithDocFile(int id, Int64 nUserID);
         
        CommercialInvoiceDoc Save(CommercialInvoiceDoc oCommercialInvoiceDoc, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
    }
    #endregion
}
