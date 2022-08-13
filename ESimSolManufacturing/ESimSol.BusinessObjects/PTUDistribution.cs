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
    #region PTUDistribution
    public class PTUDistribution : BusinessObject
    {
        
        #region  Constructor
        public PTUDistribution()
        {

            PTUDistributionID = 0;
            LotID = 0;
            Qty = 0;
            PTUID = 0;
            LocationName = "";
            OperationUnitName = "";
            PTUID_Dest = 0;
            ErrorMessage = "";
        }
        #endregion
        
        #region Properties
       
        public int PTUDistributionID { get; set; }
        public int LotID { get; set; }
        public int PTUID { get; set; }
        public double Qty { get; set; }
        #region Derived Properties
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
        public int PTUID_Dest { get; set; }
        public string ErrorMessage { get; set; }
             
        #endregion
        #endregion

        #region Functions
        public static PTUDistribution Get(int nPTUDistributionID, long nUserID)
        {
            return PTUDistribution.Service.Get(nPTUDistributionID, nUserID);
        }
        public static List<PTUDistribution> Gets(int nPTUID, long nUserID)
        {
            return PTUDistribution.Service.Gets(nPTUID, nUserID);
        }
        public static List<PTUDistribution> Gets(int nPTUID, int nWUID, long nUserID)
        {
            return PTUDistribution.Service.Gets(nPTUID, nWUID, nUserID);
        }
        public static List<PTUDistribution> GetsByLot(int nLotID, long nUserID)
        {
            return PTUDistribution.Service.GetsByLot(nLotID,  nUserID);
        }
        public static List<PTUDistribution> Gets(string sSQL, long nUserID)
        {
            return PTUDistribution.Service.Gets(sSQL, nUserID);
        }
        public PTUDistribution PTUToPTU_Transfer(long nUserID)
        {
            return PTUDistribution.Service.PTUToPTU_Transfer(this,   nUserID);
        }
     
        #endregion

        #region Functions [NonDB]

        public static double GetTotalQty(List<PTUDistribution> oPTUDistributions)
        {
            double nReturn = 0;
            foreach (PTUDistribution oItem in oPTUDistributions)
            {
                
                    nReturn += oItem.Qty;
            }
            return nReturn;
        }

        #endregion


        #region ServiceFactory
        internal static IPTUDistributionService Service
        {
            get { return (IPTUDistributionService)Services.Factory.CreateService(typeof(IPTUDistributionService)); }
        }
        #endregion

        
    }
    #endregion

  

    #region IPTUDistribution interface
    public interface IPTUDistributionService
    {
        PTUDistribution Get(int nPTUDistributionID, Int64 nUserID);
        List<PTUDistribution> Gets(int nPTUID, Int64 nUserID);
        List<PTUDistribution> Gets(int nPTUID, int nWUID, Int64 nUserID);
        List<PTUDistribution> GetsByLot(int nLotID, Int64 nUserID);
        List<PTUDistribution> Gets(string sSQL, Int64 nUserID);
        PTUDistribution PTUToPTU_Transfer(PTUDistribution oPTUDistribution, long nUserID);
      
    }
    #endregion
    
}
