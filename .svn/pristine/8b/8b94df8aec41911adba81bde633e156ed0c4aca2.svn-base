using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region DULotDistribution
    public class DULotDistribution : BusinessObject
    {
        
        #region  Constructor
        public DULotDistribution()
        {
            DULotDistributionID = 0;
            LotID = 0;
            Qty = 0;
            DODID = 0;
            PTUID = 0;
            LocationName = "";
            OperationUnitName = "";
            DODID_Dest = 0;
            WorkingUnitID = 0;
            ErrorMessage = "";
            IsFinish = false;
            IsRaw = false;
            MUName = "";
            LotBalance = 0;
            DispoNo = "";
            BUID = 0;
            DBServerDateTime = DateTime.Now;
        }
        #endregion
        
        #region Properties

        public int DULotDistributionID { get; set; }
        public int DODID { get; set; }
        public int LotID { get; set; }
        public bool IsFinish { get; set; }
        public bool IsRaw { get; set; }
        public int WorkingUnitID { get; set; }
        public int PTUID { get; set; }
        public double Qty { get; set; }
        public string DispoNo { get; set; }
        public double LotBalance { get; set; }
        public DateTime DBServerDateTime { get; set; }
        #region Derived Properties
        public string DBServerDateTimeST
        {
            get
            {
                return this.DBServerDateTime.ToString("dd MMM yyyy");
            }
        }
        public string WorkingUnitName
        {
            get
            {
                return this.LocationName + "[" + this.OperationUnitName.ToString() + "]";
            }
        }
        public string LocationName { get; set; }
        public string OperationUnitName { get; set; }
        public string LotNo { get; set; }
        public string OrderNo { get; set; }
        public string ContractorName { get; set; }
        public string ProductName { get; set; }
        public string ColorName { get; set; }
        public int ProductID { get; set; }
        public double UnitPrice { get; set; }
        public int DODID_Dest { get; set; }
        public string MUName { get; set; }
        public int BUID { get; set; }
        public string ErrorMessage { get; set; }
             
        #endregion

        #endregion
        #region Functions
        public static DULotDistribution Get(int nDULotDistributionID, long nUserID)
        {
            return DULotDistribution.Service.Get(nDULotDistributionID, nUserID);
        }
        public static List<DULotDistribution> Gets(int nDODID, int nWUID, long nUserID)
        {
            return DULotDistribution.Service.Gets( nDODID,  nWUID, nUserID);
        }
        public static List<DULotDistribution> GetsByWU(int nDODID, string sWUIDs, long nUserID)
        {
            return DULotDistribution.Service.GetsByWU(nDODID, sWUIDs, nUserID);
        }
        public static List<DULotDistribution> GetsBy(int nDODID,  long nUserID)
        {
            return DULotDistribution.Service.GetsBy(nDODID,  nUserID);
        }
        public static List<DULotDistribution> GetsByLot(int nLotID, long nUserID)
        {
            return DULotDistribution.Service.GetsByLot(nLotID, nUserID);
        }
        public static List<DULotDistribution> Gets(string sSQL, long nUserID)
        {
            return DULotDistribution.Service.Gets(sSQL, nUserID);
        }
        public DULotDistribution Save_Transfer(long nUserID)
        {
            return DULotDistribution.Service.Save_Transfer(this, nUserID);
        }
        public DULotDistribution Save_Reduce(long nUserID)
        {
            return DULotDistribution.Service.Save_Reduce(this, nUserID);
        }
        //public string Delete(int id, long nUserID)
        //{
        //    return DULotDistribution.Service.Delete(id, nUserID);
        //}
     
        #endregion

        #region Functions [NonDB]

        public static double GetTotalQty(List<DULotDistribution> oDULotDistributions)
        {
            double nReturn = 0;
            foreach (DULotDistribution oItem in oDULotDistributions)
            {
                
                    nReturn += oItem.Qty;
            }
            return nReturn;
        }

        #endregion


        #region ServiceFactory
        internal static IDULotDistributionService Service
        {
            get { return (IDULotDistributionService)Services.Factory.CreateService(typeof(IDULotDistributionService)); }
        }
        #endregion

        
    }
    #endregion



    #region IDULotDistributionService interface
    public interface IDULotDistributionService
    {
        DULotDistribution Get(int nDULotDistributionID, Int64 nUserID);
        List<DULotDistribution> Gets(int nDODID, int nWUID, Int64 nUserID);
        List<DULotDistribution> GetsByWU(int nDODID, string sWUIDs, Int64 nUserID);
        List<DULotDistribution> GetsBy(int nDODID, Int64 nUserID);
        List<DULotDistribution> GetsByLot(int nLotID, Int64 nUserID);
        List<DULotDistribution> Gets(string sSQL, Int64 nUserID);
        DULotDistribution Save_Transfer(DULotDistribution oDULotDistribution, long nUserID);
        DULotDistribution Save_Reduce(DULotDistribution oDULotDistribution, long nUserID);
        //string Delete(int id, Int64 nUserID);
      
    }
    #endregion
    
}

