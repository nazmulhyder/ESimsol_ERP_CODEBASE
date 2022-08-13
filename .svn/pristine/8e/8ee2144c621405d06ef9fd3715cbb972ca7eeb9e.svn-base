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
    public class ExportQuality
    {
        public ExportQuality()
        {
            ExportQualityID = 0;
            Name = "";
            Activity = true;
            ErrorMessage = "";
        }

        #region Properties
        public int ExportQualityID { get; set; }
        public string Name { get; set; }
        public bool Activity { get; set; }
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
    
        #endregion

        #region Functions
        public static List<ExportQuality> Gets(long nUserID)
        {
            return ExportQuality.Service.Gets(nUserID);
        }
        public static List<ExportQuality> Gets(bool bActivity, long nUserID)
        {
            return ExportQuality.Service.Gets(bActivity,nUserID);
        }
        public ExportQuality Save(long nUserID)
        {
            return ExportQuality.Service.Save(this, nUserID);
        }
        public ExportQuality Get(int nID, long nUserID)
        {
            return ExportQuality.Service.Get(nID, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return ExportQuality.Service.Delete(nId, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IExportQualityService Service
        {
            get { return (IExportQualityService)Services.Factory.CreateService(typeof(IExportQualityService)); }
        }
        #endregion
    }

    #region IExportQuality interface
    public interface IExportQualityService
    {
        List<ExportQuality> Gets(bool bActivity,long nUserID);
        List<ExportQuality> Gets(long nUserID);
        ExportQuality Save(ExportQuality oExportQuality, long nUserID);
        ExportQuality Get(int nID, long nUserID);
        string Delete(int id, long nUserID);
    }
    #endregion
}
