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
    #region DispoProductionCommentViewer
    public class DispoProductionCommentViewer : BusinessObject
    {
        public DispoProductionCommentViewer()
        {
            DispoProductionCommentViewerID = 0;
            DispoProductionCommentID = 0;
            DBServerDateTime = DateTime.Now;
            DBUserID = 0;
            ErrorMessage = "";
            UserName = "";
        }

        #region Property
        public int DispoProductionCommentViewerID { get; set; }
        public int DispoProductionCommentID { get; set; }
        public int DBUserID { get; set; }
        public DateTime DBServerDateTime { get; set; }
        public string UserName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property

        public string DBServerDateTimeInString
        {
            get
            {
                return DBServerDateTime.ToString("dd MMM yyyy hh:mm tt");
            }
        }

        #endregion

        #region Functions
        public static List<DispoProductionCommentViewer> Gets(long nUserID)
        {
            return DispoProductionCommentViewer.Service.Gets(nUserID);
        }
        public static List<DispoProductionCommentViewer> Gets(string sSQL, long nUserID)
        {
            return DispoProductionCommentViewer.Service.Gets(sSQL, nUserID);
        }
        public DispoProductionCommentViewer Get(int id, long nUserID)
        {
            return DispoProductionCommentViewer.Service.Get(id, nUserID);
        }
        public List<DispoProductionCommentViewer> Save(List<DispoProductionCommentViewer> oObjs, long nUserID)
        {
            return DispoProductionCommentViewer.Service.Save(oObjs, nUserID);
        }
        //public string Delete(int id, long nUserID)
        //{
        //    return DispoProductionCommentViewer.Service.Delete(id, nUserID);
        //}
        #endregion

        #region ServiceFactory
        internal static IDispoProductionCommentViewerService Service
        {
            get { return (IDispoProductionCommentViewerService)Services.Factory.CreateService(typeof(IDispoProductionCommentViewerService)); }
        }
        #endregion
    }
    #endregion

    #region IDispoProductionCommentViewer interface
    public interface IDispoProductionCommentViewerService
    {
        DispoProductionCommentViewer Get(int id, Int64 nUserID);
        List<DispoProductionCommentViewer> Gets(Int64 nUserID);
        List<DispoProductionCommentViewer> Gets(string sSQL, Int64 nUserID);
        //string Delete(int id, Int64 nUserID);
        List<DispoProductionCommentViewer> Save(List<DispoProductionCommentViewer> oDispoProductionCommentViewers, Int64 nUserID);


    }
    #endregion
}
