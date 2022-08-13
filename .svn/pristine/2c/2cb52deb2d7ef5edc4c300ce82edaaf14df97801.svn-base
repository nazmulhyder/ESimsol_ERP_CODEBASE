using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Data;


namespace ESimSol.BusinessObjects
{
    #region PTUTransection
    
    public class PTUTransection : BusinessObject
    {
        public PTUTransection()
        {
            PTUTransectionID = 0;
            GUProductionTracingUnitDetailID = 0;                        
            PLineConfigureID = 0;
            MeasurementUnitID = 0;
            Quantity = 0;
            OperationDate = DateTime.Now;
            Note = "";
            LineNo = "";
            OperationBy = "";
            ErrorMessage = "";
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            ColorName = "";
            SizeName = "";
            ColorID = 0;
            SizeID = 0;
            MeasurementUnitName = "";
            GUProductionOrderNo = "";
            StyleNo = "";
            DBServerDateTime = DateTime.Now;
            GUProductionTracingUnitID = 0;
            ProductionStepID = 0;
            GUProductionProcedureID = 0;
            ActualWorkingHour = 0;
			UseHelper = 0;
            UseOperator = 0;
            BUID = 0;
            ProductionUnitID = 0;
            IsUsesValueUpdate = false;
        }

        #region Properties
         
        public int PTUTransectionID { get; set; }
         
        public int GUProductionTracingUnitDetailID { get; set; }
         
        public int ProductionStepID { get; set; }  
         
        public int PLineConfigureID { get; set; }
         
        public int MeasurementUnitID { get; set; }
         
        public double Quantity { get; set; }
         
        public DateTime OperationDate { get; set; }
         
        public string Note { get; set; }
         
        public string ErrorMessage { get; set; }
         
        public string LineNo { get; set; }
         
        public string OperationBy { get; set; }
         
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
         
        public string ColorName { get; set; }        
         
        public string MeasurementUnitName { get; set; }
         
        public string StyleNo { get; set; }
        public int GUProductionProcedureID { get; set; }
        public string GUProductionOrderNo { get; set; }
        public string  SizeName { get; set; }
        public int ColorID { get; set; }
        public int SizeID { get; set; }
        public DateTime DBServerDateTime { get; set; }
        public int GUProductionTracingUnitID { get; set; }
        public double ActualWorkingHour { get; set; }
        public int UseHelper { get; set; }
        public int UseOperator { get; set; }        

        #endregion

        #region Derived Property
        public bool IsUsesValueUpdate { get; set; }
        public int BUID { get; set; }
        public int ProductionUnitID { get; set; }
        public string OperationDateInString
        {
            get
            {
                return OperationDate.ToString("dd MMM yyyy");
            }
        }
        public string OperationDateTimeInString
        {
            get
            {
                return DBServerDateTime.ToString("HH:mm tt");
            }
        }

        public string FullOperationDateTimeInString
        {
            get
            {
                return DBServerDateTime.ToString("dd MMM yyyy h:mm:ss tt");
            }
        }
        public List<PTUTransection> PTUTransections { get; set; }
        #endregion


        #region Function
        public static List<PTUTransection> GetPTUViewDetails(int nProductionStepID, DateTime dOperationDate, int nExcutionFatoryId, int nGUProductionOrderID, long nUserID)
        {
            return PTUTransection.Service.GetPTUViewDetails(nProductionStepID, dOperationDate, nExcutionFatoryId, nGUProductionOrderID, nUserID);
        }
        public static List<PTUTransection> GetPTUViewDetails(int nGUProductionTracingUnitDetailID, long nUserID)
        {
            return PTUTransection.Service.GetPTUViewDetails(nGUProductionTracingUnitDetailID, nUserID);
        }
        public static List<PTUTransection> GetPTUTransactionHistory(int nGUProductionOrderID, int nProductionStepID, long nUserID)
        {
            return PTUTransection.Service.GetPTUTransactionHistory(nGUProductionOrderID,nProductionStepID, nUserID);
        }
        public static List<PTUTransection> Gets(string sSQL, long nUserID)
        {
            return PTUTransection.Service.Gets(sSQL, nUserID);
        }
        public static List<PTUTransection> Gets_byPOIDs(string sPOIDs, long nUserID)
        {
            return PTUTransection.Service.Gets_byPOIDs(sPOIDs,  nUserID);
        }
        public  PTUTransection UpdatePTUTransaction( long nUserID)
        {
            return PTUTransection.Service.UpdatePTUTransaction(this, nUserID);
        }
        public static DataSet GetDailyProductionReport(DateTime StartDate, DateTime EndDate, int BUID, int ProductionUnitID, long nUserID)
        {
            return PTUTransection.Service.GetDailyProductionReport(StartDate, EndDate, BUID, ProductionUnitID, nUserID);
        }     
        #endregion


        #region ServiceFactory

        internal static IPTUTransectionService Service
        {
            get { return (IPTUTransectionService)Services.Factory.CreateService(typeof(IPTUTransectionService)); }
        }

        #endregion
    }
    #endregion
    
    #region IPTUTransection interface
     
    public interface IPTUTransectionService
    {
         
        List<PTUTransection> GetPTUTransactionHistory(int nGUProductionOrderID, int nProductionStepID, Int64 nUserID);

        List<PTUTransection> GetPTUViewDetails(int nProductionStepID, DateTime dOperationDate, int nExcutionFatoryId, int nGUProductionOrderID, Int64 nUserID);

        List<PTUTransection> GetPTUViewDetails(int nGUProductionTracingUnitDetailID, Int64 nUserID);
         
        List<PTUTransection> Gets(string sSQL, Int64 nUserID);

         
        List<PTUTransection> Gets_byPOIDs(string sPOIDs, Int64 nUserID);
         
        PTUTransection UpdatePTUTransaction(PTUTransection oPTUTransection, Int64 nUserID);
        DataSet GetDailyProductionReport(DateTime StartDate, DateTime EndDate, int BUID, int ProductionUnitID, Int64 nUserID);
    }
    #endregion
}
