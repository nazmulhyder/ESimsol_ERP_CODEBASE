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
    public class LightSource
    {
        public LightSource()
        {
            LightSourceID = 0;
            Descriptions = "";
            ErrorMessage = "";
        }

        #region Properties
        public int LightSourceID { get; set; }
        public string Descriptions { get; set; }
        public string ErrorMessage { get; set; }
        public string NameTwo { get; set; } /// For Carry
        #endregion

        #region Functions
        public static List<LightSource> Gets(long nUserID)
        {
            return LightSource.Service.Gets(nUserID);
        }

        public LightSource Save(long nUserID)
        {
            return LightSource.Service.Save(this, nUserID);
        }
        public LightSource Get(int nEPIDID, long nUserID)
        {
            return LightSource.Service.Get(nEPIDID, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return LightSource.Service.Delete(nId, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ILightSourceService Service
        {
            get { return (ILightSourceService)Services.Factory.CreateService(typeof(ILightSourceService)); }
        }
        #endregion
    }

    #region ILightSource interface
    public interface ILightSourceService
    {
        List<LightSource> Gets(long nUserID);
        LightSource Save(LightSource oLightSource, long nUserID);
        LightSource Get(int nEPIDID, long nUserID);
        string Delete(int id, long nUserID);
    }
    #endregion
}
