using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects.ReportingObject
{
    public class FNGreyReceive
    {
        public FNGreyReceive()
        {
            FNExOID = 0; 
            ReceiveDate = DateTime.Now;
            Qty = 0;
            OutQty = 0;
            BuyerID = 0;
            IsInHouse = false;
            BuyerName = string.Empty;
            ProcessName = string.Empty;
            IsYarnDyed = false;
        }
        #region Properties
         public int FNExOID {get; set;}
         public DateTime ReceiveDate {get; set;}
         public double Qty {get; set;}
         public double OutQty { get; set; }
         public int BuyerID {get; set;}
         public bool IsInHouse {get; set;}
         public string BuyerName {get; set;}
         public string ProcessName {get; set;}
         public bool IsYarnDyed {get; set;}
        public string ErrorMessage { get; set; }

        #endregion

        #region Derive

        public double ExeYDQty { get; set; }

        public double ExeSDQty { get; set; }

        public double SCWYDQty { get; set; }

        public double SCWSDQty { get; set; }

        public double TotalReceiveQty
        {
            get
            {
                return this.ExeYDQty + this.ExeSDQty + this.SCWYDQty + this.SCWSDQty;
            }
        }

        public double StockQty { get; set; }
        public double CumulitiveQty { get; set; }

        public double QtyInMtr
        {
            get
            {
                return Global.GetMeter(this.Qty, 2);
            }
        }
        public double OutQtyInMtr
        {
            get
            {
                return Global.GetMeter(this.OutQty, 2);
            }
        }

        public string ReceiveDateStr
        {
            get
            {
                return this.ReceiveDate.ToString("dd MMM yyyy");
            }
        }
        #endregion


        #region Functions
        public static List<FNGreyReceive> Gets(bool bIsDate, DateTime dtStart, DateTime dtEnd, string sBuyerIDs, long nUserID)
        {
            return FNGreyReceive.Service.Gets(bIsDate, dtStart, dtEnd, sBuyerIDs, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFNGreyReceiveService Service
        {
            get { return (IFNGreyReceiveService)Services.Factory.CreateService(typeof(IFNGreyReceiveService)); }
        }
        #endregion
    }

    #region IFNGreyReceive interface
    public interface IFNGreyReceiveService
    {
        List<FNGreyReceive> Gets(bool bIsDate, DateTime dtStart, DateTime dtEnd, string sBuyerIDs, long nUserID);
    }
    #endregion
}

