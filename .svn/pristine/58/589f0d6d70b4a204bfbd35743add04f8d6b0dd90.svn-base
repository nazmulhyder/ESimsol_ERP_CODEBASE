using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Drawing;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
namespace ESimSol.BusinessObjects
{
   public class LotBaseTestResult :BusinessObject
    {
       public LotBaseTestResult()
       {
        LotBaseTestResultID =0;
        LotBaseTestID =0;
        LotID =0;
        Qty =0;
        Note =string.Empty;
        Params = string.Empty;
        ErrorMessage = string.Empty;
        LotBaseTestResults = new List<LotBaseTestResult>();

       }
        #region properties
        public int LotBaseTestResultID { get; set; }
        public int LotBaseTestID { get; set; }
        public int LotID { get; set; }
        public double Qty { get; set; }
        public string Note { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        public List<LotBaseTestResult> LotBaseTestResults { get; set; }
        public DateTime DBServerDateTime { get; set; }
        public string UserName { get; set; }
        #endregion
        #region Derived properties
        public string Name { get; set; }
        public string DBServerDateTimeSt
        {
            get
            {
                if (this.DBServerDateTime == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.DBServerDateTime.ToString("dd MMM yyyy");
                }
            }
        }
        #endregion
        #region Functions

        public static LotBaseTestResult Get(int nLotBaseTestResultID, long nUserID)
        {
            return LotBaseTestResult.Service.Get(nLotBaseTestResultID, nUserID);
        }
        public static List<LotBaseTestResult> Gets(string sSQL, long nUserID)
        {
            return LotBaseTestResult.Service.Gets(sSQL, nUserID);
        }
        public LotBaseTestResult IUD(int nDBOperation, long nUserID)
        {
            return LotBaseTestResult.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static ILotBaseTestResultService Service
        {
            get { return (ILotBaseTestResultService)Services.Factory.CreateService(typeof(ILotBaseTestResultService)); }
        }

        #endregion

    }
    #region  LotBaseTestResult interface
    public interface ILotBaseTestResultService
    {

        LotBaseTestResult Get(int nDUDyeingTypeID, Int64 nUserID);
        List<LotBaseTestResult> Gets(string sSQL, Int64 nUserID);
        LotBaseTestResult IUD(LotBaseTestResult oDUDyeingType, int nDBOperation, Int64 nUserID);

    }
    #endregion
}
