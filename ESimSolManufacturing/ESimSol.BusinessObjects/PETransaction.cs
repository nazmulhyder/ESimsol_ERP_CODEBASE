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
    #region PETransaction
    public class PETransaction : BusinessObject
    {
        public PETransaction()
        {
            PETransactionID = 0;
            ProductionExecutionID = 0;
            MeasurementUnitID = 0;
            Quantity = 0;
            TransactionDate = DateTime.Now;
            UnitName = "";
            UnitSymbol = "";
            EntryByName = "";
            CycleTime = 0;
            Cavity = 0;
            ShortCounter = 0;
            ProductionHour = 0;
            Remarks = "";
            ShiftID = 0;
            ShiftName = "";
            MachineID = 0;
            MachineName = "";
            OperationEmpID = 0;
            BUID = 0;
            ProductionStepID = 0;
            OperatorPerMachine = 0;
            OperationEmpByName = "";
        }

        #region Properties
        public int PETransactionID { get; set; }
        public int ProductionExecutionID { get; set; }
        public int MeasurementUnitID { get; set; }
        public double Quantity { get; set; }
        public DateTime TransactionDate { get; set; }
        public string UnitName { get; set; }
        public string UnitSymbol { get; set; }
        public string EntryByName { get; set; }
        public int CycleTime { get; set; }
        public int Cavity { get; set; }
        public int ShortCounter { get; set; }
        public double ProductionHour { get; set; }
        public string Remarks { get; set; }
        public int OperationEmpID { get; set; }
        public string OperationEmpByName { get; set; }
        public int ShiftID { get; set; }
        public string ShiftName { get; set; }
        public int MachineID { get; set; }
        public string MachineName { get; set; }
        public int BUID { get; set; }
        public int ProductionStepID { get; set; }
        public int ProductNatureInInt { get; set; }
        public double OperatorPerMachine { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived PETransaction
        public string Params { get; set; }
        
        public string TransactionDateInString
        {
            get
            {
                return this.TransactionDate.ToString("dd MMM yyyy hh:mm:ss tt");
            }
        }
        #endregion


        #region Functions

        public static List<PETransaction> Gets(string sSQL, long nUserID)
        {
            return PETransaction.Service.Gets(sSQL,nUserID);
        }
        public static List<PETransaction> Gets(int nRecipeID, long nUserID)
        {
            return PETransaction.Service.Gets(nRecipeID, nUserID);
        }
        public PETransaction Get(int id, long nUserID)
        {
            return PETransaction.Service.Get(id, nUserID);
        }
        public PETransaction Save(long nUserID)
        {
            return PETransaction.Service.Save(this, nUserID);
        }
        public double GetYetToProductionHour(string sSQL, long nUserID)
        {
            return PETransaction.Service.GetYetToProductionHour(sSQL, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return PETransaction.Service.Delete(id, nUserID);
        }

        #endregion

        #region Non DB Function
     
        #endregion

        #region ServiceFactory
        internal static IPETransactionService Service
        {
            get { return (IPETransactionService)Services.Factory.CreateService(typeof(IPETransactionService)); }
        }
        #endregion
    }
    #endregion

    #region IPETransaction interface

    public interface IPETransactionService
    {
        PETransaction Get(int id, Int64 nUserID);
        List<PETransaction> Gets(string sSQL, Int64 nUserID);
        List<PETransaction> Gets(int nRecipeID, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        PETransaction Save(PETransaction oPETransaction, Int64 nUserID);
        double GetYetToProductionHour(string sSQL, Int64 nUserID);
    }
    #endregion
    
   
}
