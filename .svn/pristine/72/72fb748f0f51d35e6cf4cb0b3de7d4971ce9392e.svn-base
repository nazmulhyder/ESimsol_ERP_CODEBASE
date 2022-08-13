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
    #region BDYEACDetail
    public class BDYEACDetail : BusinessObject
    {
        public BDYEACDetail()
        {
            BDYEACDetailID = 0;
            BDYEACID = 0;
            ProductName = "";
            Qty = 0;
            ErrorMessage = "";
        }
        #region Properties
        public int BDYEACDetailID { get; set; }
        public int BDYEACID { get; set; }
        public string ProductName { get; set; }
        public double Qty { get; set; }
	
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Properties
        public double QtyInKg { 
            get
            {
                return Global.GetKG(this.Qty, 2);
            }
        }
        #endregion

        #region Functions

        public BDYEACDetail Get(int id, int nUserID)
        {
            return BDYEACDetail.Service.Get(id, nUserID);
        }
        public BDYEACDetail Save(int nUserID)
        {
            return BDYEACDetail.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return BDYEACDetail.Service.Delete(id, nUserID);
        }
        public static List<BDYEACDetail> Gets(int nUserID)
        {
            return BDYEACDetail.Service.Gets(nUserID);
        }
        public static List<BDYEACDetail> Gets(int nGRNID, int nUserID)
        {
            return BDYEACDetail.Service.Gets(nGRNID, nUserID);
        }
        public static List<BDYEACDetail> Gets(string sSQL, int nUserID)
        {
            return BDYEACDetail.Service.Gets(sSQL, nUserID);
        }
       
        #endregion


        #region ServiceFactory
        internal static IBDYEACDetailService Service
        {
            get { return (IBDYEACDetailService)Services.Factory.CreateService(typeof(IBDYEACDetailService)); }
        }
        #endregion
    }
    #endregion
       

    #region IBDYEACDetail interface
    public interface IBDYEACDetailService
    {
        BDYEACDetail Get(int id, int nUserID);
        List<BDYEACDetail> Gets(int nUserID);
        List<BDYEACDetail> Gets(int nGRNID, int nUserID);
        string Delete(int id, int nUserID);
        BDYEACDetail Save(BDYEACDetail oBDYEACDetail, int nUserID);
        
        List<BDYEACDetail> Gets(string sSQL, int nUserID);

    }
    #endregion
}