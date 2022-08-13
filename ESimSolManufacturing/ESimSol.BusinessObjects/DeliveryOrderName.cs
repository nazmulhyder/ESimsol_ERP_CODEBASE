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
    public class DeliveryOrderName
    {
        public DeliveryOrderName()
        {
            DeliveryOrderNameID = 0;
            Name = "";
            Activity = true;
            Sequence = 0;
            ErrorMessage = "";
            IsFoc = false;
            IsGrey = false;
        }

        #region Properties
        public int DeliveryOrderNameID { get; set; }
        public int OrderType { get; set; }
        public string Name { get; set; }
        public bool Activity { get; set; }
        public int Sequence { get; set; }
        public bool IsFoc { get; set; }
        public bool IsGrey { get; set; }
        public string ErrorMessage { get; set; }
        private string _sActivity = "";
        public string ActivitySt
        {
            get
            {
                if (this.Activity == true)
                {
                    _sActivity = "Active";
                }
                if (this.Activity == false)
                {
                    _sActivity = "Inactive";
                }
                return _sActivity;
            }
        }
        private string _sIsFoc = "";
        public string IsFocSt
        {
            get
            {
                if (this.IsFoc == true)
                {
                    _sIsFoc = "Test";
                }
                if (this.IsFoc == false)
                {
                    _sIsFoc = "--";
                }
                return _sIsFoc;
            }
        }
        private string _sIsGrey = "";
        public string IsGreySt
        {
            get
            {
                if (this.IsGrey == true)
                {
                    _sIsGrey = "Grey Received";
                }
                if (this.IsGrey == false)
                {
                    _sIsGrey = "Delivery";
                }
                return _sIsGrey;
            }
        }
        public string OrderTypeSt
        {
            get
            {
                return EnumObject.jGet((EnumFabricRequestType)this.OrderType); ;
            }
        }
        #endregion

        #region Functions
        public static List<DeliveryOrderName> Gets(long nUserID)
        {
            return DeliveryOrderName.Service.Gets(nUserID);
        }
        public static List<DeliveryOrderName> Gets(bool bActivity, long nUserID)
        {
            return DeliveryOrderName.Service.Gets(bActivity,nUserID);
        }
        public DeliveryOrderName Save(long nUserID)
        {
            return DeliveryOrderName.Service.Save(this, nUserID);
        }
        public DeliveryOrderName Get(int nID, long nUserID)
        {
            return DeliveryOrderName.Service.Get(nID, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return DeliveryOrderName.Service.Delete(nId, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IDeliveryOrderNameService Service
        {
            get { return (IDeliveryOrderNameService)Services.Factory.CreateService(typeof(IDeliveryOrderNameService)); }
        }
        #endregion
    }

    #region IDeliveryOrderName interface
    public interface IDeliveryOrderNameService
    {
        List<DeliveryOrderName> Gets(bool bActivity,long nUserID);
        List<DeliveryOrderName> Gets(long nUserID);
        DeliveryOrderName Save(DeliveryOrderName oDeliveryOrderName, long nUserID);
        DeliveryOrderName Get(int nID, long nUserID);
        string Delete(int id, long nUserID);
    }
    #endregion
}
