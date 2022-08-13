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
    public class ExportTnCCaption
    {
        public ExportTnCCaption()
        {
            ExportTnCCaptionID = 0;
            Name = "";
            Activity = true;
            Sequence = 0;
            ErrorMessage = "";
        }

        #region Properties
        public int ExportTnCCaptionID { get; set; }
        public string Name { get; set; }
        public bool Activity { get; set; }
        public int Sequence { get; set; }
        public string ErrorMessage { get; set; }
       
        #endregion

        #region Functions
        public static List<ExportTnCCaption> Gets(long nUserID)
        {
            return ExportTnCCaption.Service.Gets(nUserID);
        }
        public static List<ExportTnCCaption> Gets(bool bActivity, long nUserID)
        {
            return ExportTnCCaption.Service.Gets(bActivity,nUserID);
        }
        public ExportTnCCaption Save(long nUserID)
        {
            return ExportTnCCaption.Service.Save(this, nUserID);
        }
        public ExportTnCCaption Get(int nID, long nUserID)
        {
            return ExportTnCCaption.Service.Get(nID, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return ExportTnCCaption.Service.Delete(nId, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IExportTnCCaptionService Service
        {
            get { return (IExportTnCCaptionService)Services.Factory.CreateService(typeof(IExportTnCCaptionService)); }
        }
        #endregion
    }

    #region IExportTnCCaption interface
    public interface IExportTnCCaptionService
    {
        List<ExportTnCCaption> Gets(bool bActivity,long nUserID);
        List<ExportTnCCaption> Gets(long nUserID);
        ExportTnCCaption Save(ExportTnCCaption oExportTnCCaption, long nUserID);
        ExportTnCCaption Get(int nID, long nUserID);
        string Delete(int id, long nUserID);
    }
    #endregion
}
