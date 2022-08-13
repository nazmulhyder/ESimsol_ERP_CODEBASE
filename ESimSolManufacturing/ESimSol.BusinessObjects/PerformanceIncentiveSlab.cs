using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;


namespace ESimSol.BusinessObjects
{
    #region PerformanceIncentiveSlab

    public class PerformanceIncentiveSlab : BusinessObject
    {
        public PerformanceIncentiveSlab()
        {
            PISlabID = 0;
            PIID = 0;
            MinPoint = 0;
            MaxPoint = 0;
            Value = 0;
            IsPercentOfRate = true;
            ErrorMessage = "";
            
        }

        #region Properties
        public int PISlabID  { get; set; }
        public int PIID { get; set; }
        public double MinPoint { get; set; }
        public double MaxPoint { get; set; }
        public double Value { get; set; }
        public bool IsPercentOfRate { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<PerformanceIncentiveSlab> PerformanceIncentiveSlabs { get; set; }
        public PerformanceIncentive PerformanceIncentive { get; set; }
        //public string PISlab { get; set; }
        //public string Benifits { get; set; }

        public string PISlab
        {
            get
            {
                return this.MinPoint.ToString() + "-" + this.MaxPoint.ToString();
            }
        }

        public string Benifits
        {
            get
            {
                if (IsPercentOfRate)
                {
                    return this.Value.ToString() + "% of Incentve Rate";
                }
                else
                {
                   return this.Value.ToString();
                }
            }
        }

        #endregion

        #region Functions
        public static PerformanceIncentiveSlab Get(int Id, long nUserID)
        {
            return PerformanceIncentiveSlab.Service.Get(Id, nUserID);
        }
        public static PerformanceIncentiveSlab Get(string sSQL, long nUserID)
        {
            return PerformanceIncentiveSlab.Service.Get(sSQL, nUserID);
        }
        public static List<PerformanceIncentiveSlab> Gets(long nUserID)
        {
            return PerformanceIncentiveSlab.Service.Gets(nUserID);
        }
        public static List<PerformanceIncentiveSlab> Gets(string sSQL, long nUserID)
        {
            return PerformanceIncentiveSlab.Service.Gets(sSQL, nUserID);
        }
        public PerformanceIncentiveSlab IUD(int nDBOperation, long nUserID)
        {
            return PerformanceIncentiveSlab.Service.IUD(this, nDBOperation, nUserID);
        }


        #endregion

        #region ServiceFactory
        internal static IPerformanceIncentiveSlabService Service
        {
            get { return (IPerformanceIncentiveSlabService)Services.Factory.CreateService(typeof(IPerformanceIncentiveSlabService)); }
        }

        #endregion
    }
    #endregion

    #region IPerformanceIncentiveSlab interface

    public interface IPerformanceIncentiveSlabService
    {
        PerformanceIncentiveSlab Get(int id, Int64 nUserID);
        PerformanceIncentiveSlab Get(string sSQL, Int64 nUserID);
        List<PerformanceIncentiveSlab> Gets(Int64 nUserID);
        List<PerformanceIncentiveSlab> Gets(string sSQL, Int64 nUserID);
        PerformanceIncentiveSlab IUD(PerformanceIncentiveSlab oPerformanceIncentiveSlab, int nDBOperation, Int64 nUserID);
    }
    #endregion
}
