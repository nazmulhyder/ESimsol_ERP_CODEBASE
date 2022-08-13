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
    #region TwistingDetail
    
    public class TwistingDetail : BusinessObject
    {
        public TwistingDetail()
        {
            TwistingID = 0;
            TwistingDetailID = 0;
            Qty=0.0;
            UnitPrice = 0.0;
            Note = "";
            LotID = 0;
            ErrorMessage = "";
            ColorName = "";
            MUnitID = 0;
            YetQty = 0;
        }

        #region Properties
        public int TwistingDetailID { get; set; }
        public int TwistingID { get; set; }
        public int ProductID { get; set; }
        public int DyeingOrderDetailID { get; set; }
        public double Qty { get; set; }
        public double Qty_Order { get; set; }
        public double UnitPrice { get; set; }
        public double BagCount { get; set; }
        public string Note { get; set; }
        public string MUnit { get; set; }
        public int MUnitID { get; set; }
        public int CurrencyID { get; set; }
        public string ErrorMessage { get; set; }
        public int LotID { get; set; }
        public string LotNo { get; set; }
        public bool IsLotMendatory { get; set; }
        public EnumInOutType InOutType { get; set; }
        #endregion
        #region Derived Property
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ColorName { get; set; }
        public double YetQty { get; set; }
        public int InOutTypeInt { get { return (int)this.InOutType; } }
        public double Amount
        {
            get
            {
                return Qty * UnitPrice;
            }
            
        }
        #endregion

        #region Property For Order Follow Up
        #endregion

        #region Functions
        public  TwistingDetail Get(int nId, long nUserID)
        {
            return TwistingDetail.Service.Get(nId, nUserID);
        }
        public static List<TwistingDetail> Gets(int nTwistingID, long nUserID)
        {
            return TwistingDetail.Service.Gets(nTwistingID, nUserID);
        }
        public static List<TwistingDetail> Gets(string sSQL, long nUserID)
        {
            return TwistingDetail.Service.Gets(sSQL, nUserID);
        }
        public TwistingDetail Save(long nUserID)
        {
            return TwistingDetail.Service.Save(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return TwistingDetail.Service.Delete(this, nUserID);
        }
     
        #endregion


        #region ServiceFactory
        internal static ITwistingDetailService Service
        {
            get { return (ITwistingDetailService)Services.Factory.CreateService(typeof(ITwistingDetailService)); }
        }
        #endregion
    }
    #endregion

    #region ITwistingDetail interface
    
    public interface ITwistingDetailService
    {
        TwistingDetail Get(int id, long nUserID);
        List<TwistingDetail> Gets(int nTwistingID, long nUserID);
        List<TwistingDetail> Gets(string sSQL, long nUserID);
        string Delete(TwistingDetail oTwisting, long nUserID);
        TwistingDetail Save(TwistingDetail oTwistingDetail, long nUserID);
    }
    #endregion
}
