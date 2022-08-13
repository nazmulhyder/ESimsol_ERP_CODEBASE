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
   
    #region ImportBL
    [DataContract]
    public class ImportBL : BusinessObject
    {
        public ImportBL()
        {
            ImportBLID = 0;
            BLNo ="";
            BLDate = DateTime.Now;
            ETA = DateTime.Now;
            BLQuantity =0;           
            ShippingLine = 0;
            LandingPort = 0;
            DestinationPort = 0;
            PlaceOfIssue = 0;
            IssueDate = DateTime.Now;
            ContainerCount = 0;
            ShipmentDate = DateTime.Now;
            VesselInfo = "";
            ImportInvoiceID = 0;
            BLType = 0;
            ShippingLineName = "";
            LandingPortName = "";
            DestinationPortName = "";
        }

        #region Properties
        [DataMember]
        public int ImportBLID { get; set; }
        [DataMember]
        public string BLNo { get; set; }

        [DataMember]
        public string ShippingLineName { get; set; }
        [DataMember]
        public string LandingPortName { get; set; }
        [DataMember]
        public string DestinationPortName { get; set; }

        [DataMember]
        public DateTime BLDate { get; set; }
        [DataMember]
        public DateTime ETA { get; set; }
        [DataMember]
        public int BLQuantity { get; set; }
        //[DataMember]
        //public int MeasurementUnitID { get; set; }
        [DataMember]
        public int ShippingLine { get; set; }
        [DataMember]
        public int LandingPort { get; set; }
        [DataMember]
        public int DestinationPort { get; set; }
        [DataMember]
        public int PlaceOfIssue { get; set; }  
        [DataMember]
        public DateTime IssueDate { get; set; }
        [DataMember]
        public int ContainerCount { get; set; }
        [DataMember]
        public DateTime ShipmentDate { get; set; }
        [DataMember]
        public string VesselInfo { get; set; }
        [DataMember]
        public int ImportInvoiceID { get; set; }
        [DataMember]
        public int BLType { get; set; }
         [DataMember]
        public string ErrorMessage { get; set; }
        
       
         public string BLDateInString
        {
            get
            {
                return this.BLDate.ToString("dd MMM yyyy");
            }
        }
         public string ETAInString
         {
             get
             {
                 return this.ETA.ToString("dd MMM yyyy");
             }
         }
         public string IssueDateInString
         {
             get
             {
                 return this.IssueDate.ToString("dd MMM yyyy");
             }
         }
         public string ShipmentDateInString
         {
             get
             {
                 return this.ShipmentDate.ToString("dd MMM yyyy");
             }
         }

        
        #endregion

        #region Derived Property
        //[DataMember]
       // public List<BLLot> BLLots = new List<BLLot>();
        public List<RouteLocation> RouteLocations = new List<RouteLocation>();
        public ImportInvoice PIM = new ImportInvoice();  
        #endregion


        #region Functions

        public static List<ImportBL> Gets(int nUserID)
        {
            return ImportBL.Service.Gets( nUserID);
        }
        public ImportBL Get(int id, int nUserID)
        {
            return ImportBL.Service.Get(id, nUserID);
        }
        public ImportBL GetByInvoice(int id, int nUserID)
        {
            return ImportBL.Service.GetByInvoice(id, nUserID);
        }
        public ImportBL Save(int nUserID)
        {
            return ImportBL.Service.Save(this, nUserID);
        }

        public string Delete(int nUserID)
        {
            return ImportBL.Service.Delete(this, nUserID);
        }

     
        #endregion

        #region ServiceFactory

        internal static IImportBLService Service
        {
            get { return (IImportBLService)Services.Factory.CreateService(typeof(IImportBLService)); }
        }
        #endregion
    }
    #endregion

    #region ImportBLs
    public class ImportBLs : IndexedBusinessObjects
    {
        #region Collection Class Methods
        public void Add(ImportBL item)
        {
            base.AddItem(item);
        }
        public void Remove(ImportBL item)
        {
            base.RemoveItem(item);
        }
        public ImportBL this[int index]
        {
            get { return (ImportBL)GetItem(index); }
        }
        public int GetIndex(int id)
        {
            return base.GetIndex(new ID(id));
        }
        #endregion
    }
    #endregion

    #region IImportBL interface
    public interface IImportBLService
    {
        ImportBL Get(int id, Int64 nUserID);
        ImportBL GetByInvoice(int id, Int64 nUserID);
        List<ImportBL> Gets(Int64 nUserID);
        string Delete(ImportBL oImportBL, Int64 nUserID);
        ImportBL Save(ImportBL oImportBL, Int64 nUserID);
        
    }
    #endregion
}