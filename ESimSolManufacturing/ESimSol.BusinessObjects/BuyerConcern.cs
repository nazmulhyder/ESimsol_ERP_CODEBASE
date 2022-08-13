using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.BusinessObjects
{
    #region BuyerConcern
    
    public class BuyerConcern : BusinessObject
    {
        public BuyerConcern()
        {
            BuyerConcernID = 0;
            BuyerID=0;
            ConcernName="";
            ConcernEmail="";	
            ConcernAddress="";
            Note = "";
            ErrorMessage = "";
        }

        #region Properties
         
        public int BuyerConcernID { get; set; }
         
        public int BuyerID{ get; set; }
         
        public string ConcernName{ get; set; }
         
        public string ConcernEmail { get; set; }
         
        public string ConcernAddress { get; set; }
         
        public string Note { get; set; }
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property

        #endregion

        #region Functions

        public static List<BuyerConcern> Gets(long nUserID)
        {
            return BuyerConcern.Service.Gets(nUserID);
        }

        public static List<BuyerConcern> GetsByContractor(int nContractorID, long nUserID)
        {
            return BuyerConcern.Service.GetsByContractor(nContractorID, nUserID);
        }

        public BuyerConcern Get(int id, long nUserID)
        {
            return BuyerConcern.Service.Get(id, nUserID);
        }
        public BuyerConcern Save(long nUserID)
        {
            return BuyerConcern.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return BuyerConcern.Service.Delete(id, nUserID);
        }

        #endregion

        #region ServiceFactory

        internal static IBuyerConcernService Service
        {
            get { return (IBuyerConcernService)Services.Factory.CreateService(typeof(IBuyerConcernService)); }
        }
        #endregion
    }
    #endregion

    #region IBuyerConcern interface
     
    public interface IBuyerConcernService
    {        
         
        List<BuyerConcern> GetsByContractor(int nContractorID, Int64 nUserID);
         
        BuyerConcern Get(int id, Int64 nUserID);
         
        List<BuyerConcern> Gets(Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        BuyerConcern Save(BuyerConcern oBuyerConcern, Int64 nUserID);
    }
    #endregion
}
