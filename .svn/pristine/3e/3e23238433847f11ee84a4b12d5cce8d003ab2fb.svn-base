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
    #region FabricExpDelivery
    public class FabricExpDelivery
    {
        public FabricExpDelivery()
        {
            FabricExpDeliveryID = 0;
            FSCDID = 0;
            LastUpdateBy = 0;
            Qty = 0;
            LastUpdateDateTime = DateTime.Now;
            DeliveryDate = DateTime.Now;
            ErrorMessage = "";
            LastUpdateByName = "";
            ExeNo = "";
            DispoQty = 0;
        }
        #region Property
        public int FabricExpDeliveryID { get; set; }
        public int FSCDID { get; set; }     
        public double Qty { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int LastUpdateBy { get; set; }
        public DateTime LastUpdateDateTime { get; set; }
        public string ErrorMessage { get; set; }
        public string LastUpdateByName { get; set; }
        public string ExeNo { get; set; }
        public double DispoQty { get; set; }

        #endregion
        #region Derived Property     
        public string LastUpdateDateTimeInString
        {
            get
            {
                return LastUpdateDateTime.ToString("dd MMM yyyy");
            }
        }
        public string DeliveryDateInString
        {
            get
            {
                return DeliveryDate.ToString("dd MMM yyyy");
            }
        }
        #endregion
        #region Functions
        public static List<FabricExpDelivery> Gets(long nUserID)
        {
            return FabricExpDelivery.Service.Gets(nUserID);
        }
        public static List<FabricExpDelivery> Gets(string sSQL, long nUserID)
        {
            return FabricExpDelivery.Service.Gets(sSQL, nUserID);
        }
        public FabricExpDelivery Get(int id, long nUserID)
        {
            return FabricExpDelivery.Service.Get(id, nUserID);
        }
        public FabricExpDelivery Save(long nUserID)
        {
            return FabricExpDelivery.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return FabricExpDelivery.Service.Delete(id, nUserID);
        }
        #endregion
        #region ServiceFactory
        internal static IFabricExpDeliveryService Service
        {
            get { return (IFabricExpDeliveryService)Services.Factory.CreateService(typeof(IFabricExpDeliveryService)); }
        }
        #endregion
    }
    #endregion
    #region IFabricExpDelivery interface
    public interface IFabricExpDeliveryService
    {
        FabricExpDelivery Get(int id, Int64 nUserID);
        List<FabricExpDelivery> Gets(Int64 nUserID);
        List<FabricExpDelivery> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        FabricExpDelivery Save(FabricExpDelivery oFabricExpDelivery, Int64 nUserID);
    }
    #endregion
}
