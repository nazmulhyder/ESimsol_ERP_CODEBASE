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
    public class DeliveryZone
    {
        public DeliveryZone()
        {
            DeliveryZoneID = 0;
            DeliveryZoneName = "";
            ErrorMessage = "";
        }

        #region Properties
        public int DeliveryZoneID { get; set; }
        public string DeliveryZoneName { get; set; }
        public string ErrorMessage { get; set; }
        
        #endregion

        #region Functions
        public static List<DeliveryZone> Gets(int nUserID)
        {
            return DeliveryZone.Service.Gets(nUserID);
        }
        public static List<DeliveryZone> Gets(string sSQL, int nUserID)
        {
            return DeliveryZone.Service.Gets(sSQL,nUserID);
        }
        public DeliveryZone Save(int nUserID)
        {
            return DeliveryZone.Service.Save(this, nUserID);
        }
        public DeliveryZone Get(int nEPIDID, int nUserID)
        {
            return DeliveryZone.Service.Get(nEPIDID, nUserID);
        }
        public string Delete(int nId, int nUserID)
        {
            return DeliveryZone.Service.Delete(nId, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IDeliveryZoneService Service
        {
            get { return (IDeliveryZoneService)Services.Factory.CreateService(typeof(IDeliveryZoneService)); }
        }
        #endregion
    }

    #region IDeliveryZone interface
    public interface IDeliveryZoneService
    {
        List<DeliveryZone> Gets(int nUserID);
        List<DeliveryZone> Gets(string sSQL, int nUserID);
        DeliveryZone Save(DeliveryZone oDeliveryZone, int nUserID);
        DeliveryZone Get(int nEPIDID, int nUserID);
        string Delete(int id, int nUserID);
    }
    #endregion
}
