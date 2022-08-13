using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{
    #region BuyerPercent
    public class BuyerPercent
    {
        public BuyerPercent()
        {
            BuyerPercentID = 0;
            BPosition = "";
            BPercent = 0;
            ErrorMessage = "";
            LastUpdateBy = 0;
            LastUpdateDateTime = DateTime.Now;

        }
        #region Property
        public int BuyerPercentID { get; set; }
        public string BPosition { get; set; }
        public double BPercent { get; set; }
        public int LastUpdateBy { get; set; }
        public DateTime LastUpdateDateTime { get; set; }
        public string LastUpdateByName { get; set; }
        public string ErrorMessage { get; set; }

        #endregion
        #region Derived Property
        public string LastUpdateDateTimeInString
        {
            get
            {
                return LastUpdateDateTime.ToString("dd MMM yyyy");
            }
        }
        public string BuyerPercentInST
        {
            get
            {
                return this.BPercent + "%";
            }
        }

        #endregion
        #region Functions
        public static List<BuyerPercent> Gets(long nUserID)
        {
            return BuyerPercent.Service.Gets(nUserID);
        }
        public static List<BuyerPercent> Gets(string sSQL, long nUserID)
        {
            return BuyerPercent.Service.Gets(sSQL, nUserID);
        }
        public BuyerPercent Get(int id, long nUserID)
        {
            return BuyerPercent.Service.Get(id, nUserID);
        }
        public BuyerPercent Save(long nUserID)
        {
            return BuyerPercent.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return BuyerPercent.Service.Delete(id, nUserID);
        }
        #endregion
        #region ServiceFactory
        internal static IBuyerPercentService Service
        {
            get { return (IBuyerPercentService)Services.Factory.CreateService(typeof(IBuyerPercentService)); }
        }
        #endregion
    }
    #endregion
    #region IBuyerPercent interface
    public interface IBuyerPercentService
    {
        BuyerPercent Get(int id, Int64 nUserID);
        List<BuyerPercent> Gets(Int64 nUserID);
        List<BuyerPercent> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        BuyerPercent Save(BuyerPercent oBuyerPercent, Int64 nUserID);
    }
    #endregion
}
