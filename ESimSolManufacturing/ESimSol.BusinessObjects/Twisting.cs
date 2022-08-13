

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
    #region Twisting

    public class Twisting : BusinessObject
    {
        public Twisting()
        {
            TwistingID=0;	
            ReceiveDate=DateTime.Now;		
            DyeingOrderID=0;	
            ProductID_TW=0;	
            Qty=0;
            TWNo = "";
            ApproveByID = 0;
            CompletedByID = 0;
            Note="";
            Status=EnumTwistingStatus.None;
            ApproveDate = DateTime.MinValue;
            CompletedDate = DateTime.MinValue;
            MachineDes="";
            TwistingOrderTypeInt = 0;
            TwistingOrderType = EnumTwistingOrderType.TwistingOutside;
            CompleteWorkingUnitID = 0;
            IsInHouse = true;
            TwistingDetails = new List<TwistingDetail>();
            TwistingDetails_Product = new List<TwistingDetail>();
            TwistingDetails_Twisting = new List<TwistingDetail>();
        }

        #region Properties

        public int TwistingID { get; set; }
        public DateTime ReceiveDate { get; set; }
        public int DyeingOrderID { get; set; }
        public int ProductID_TW { get; set; }
        public int WorkingUnitID { get; set; }
        public double Qty { get; set; }
        public string TWNo { get; set; }
        public string Note { get; set; }
        public EnumTwistingStatus Status { get; set; }
        public string MachineDes { get; set; }
        public string DyeingOrderNo { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ContractorName { get; set; }
        public int ApproveByID { get; set; }
        public DateTime ApproveDate { get; set; }    
        public string ApproveByName { get; set; }
        public int CompletedByID { get; set; }
        public DateTime CompletedDate { get; set; }
        public string CompletedByName { get; set; }
        public EnumTwistingOrderType TwistingOrderType { get; set; }
        public int TwistingOrderTypeInt { get; set; }
        public int CompleteWorkingUnitID { get; set; }
        public string Param { get; set; }
        public bool IsInHouse { get; set; }
        public string ErrorMessage { get; set; }
        public List<TwistingDetail> TwistingDetails { get; set; }
        public List<TwistingDetail> TwistingDetails_Twisting { get; set; }
        public List<TwistingDetail> TwistingDetails_Product { get; set; } 
        #endregion

        #region Derived Property
        public string TwistingOrderTypeSt
        {
            get
            {
                if (this.TwistingOrderType == EnumTwistingOrderType.None) return "";
                return EnumObject.jGet(this.TwistingOrderType);
            }
        }
        public string ReceiveDateSt
        {
            get
            {
                if (this.ReceiveDate == DateTime.MinValue)
                    return "-";
                else
                    return ReceiveDate.ToString("dd MMM yyyy");
            }
        }
        public string ApproveDateSt
        {
            get
            {
                if (this.ApproveDate  == DateTime.MinValue)
                    return "-";
                else
                    return ApproveDate.ToString("dd MMM yyyy");
            }
        }
        public string CompletedDateSt
        {
            get
            {
                if (this.CompletedDate == DateTime.MinValue)
                    return "-";
                else
                    return CompletedDate.ToString("dd MMM yyyy");
            }
        }
      
        public int StatusInt { get { return (int)Status; } }
        public string StatusSt { get { return EnumObject.jGet(this.Status); } }
        #endregion

        #region Functions
       
        public static List<Twisting> Gets(long nUserID)
        {
            return Twisting.Service.Gets(nUserID);
        }
        public static List<Twisting> Gets(string sSQL, Int64 nUserID)
        {
            return Twisting.Service.Gets(sSQL, nUserID);
        }
        public Twisting Get(int nId, long nUserID)
        {
            return Twisting.Service.Get(nId, nUserID);
        }
        public Twisting Save(long nUserID)
        {
            return Twisting.Service.Save(this, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return Twisting.Service.Delete(nId, nUserID);
        }

        public Twisting Complete(Int64 nUserID)
        {
            return Twisting.Service.Complete(this, nUserID);
        }
     
        public Twisting Approve(Int64 nUserID)
        {
            return Twisting.Service.Approve(this, nUserID);
        }
        public Twisting UndoApprove(Int64 nUserID)
        {
            return Twisting.Service.UndoApprove(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ITwistingService Service
        {
            get { return (ITwistingService)Services.Factory.CreateService(typeof(ITwistingService)); }
        }
        #endregion

    }
    #endregion

    #region ITwisting interface
    public interface ITwistingService
    {
        Twisting Get(int id, long nUserID);
        List<Twisting> Gets(long nUserID);
        List<Twisting> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, long nUserID);
        Twisting Save(Twisting oTwisting, long nUserID);
        Twisting Approve(Twisting oTwisting, Int64 nUserID);
        Twisting UndoApprove(Twisting oTwisting, Int64 nUserID);
        Twisting Complete(Twisting oTwisting, Int64 nUserID);
    }
    #endregion
}
