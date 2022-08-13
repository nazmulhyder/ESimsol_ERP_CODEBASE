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
    #region GUQCDetail
    public class GUQCDetail : BusinessObject
    {
        public GUQCDetail()
        {
            GUQCDetailID = 0;
            GUQCID = 0;
            OrderRecapID = 0;
            QCPassQty = 0;
            RejectQty = 0;
            Remarks = "";
            ErrorMessage = "";
        }

        #region Property
        public int GUQCDetailID { get; set; }
        public int GUQCID { get; set; }
        public int OrderRecapID { get; set; }
        public int QCPassQty { get; set; }
        public int RejectQty { get; set; }
        public string Remarks { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string StyleNo { get; set; }
        public string OrderRecapNo { get; set; }
        public int TechnicalSheetID { get; set; }
        public int TotalQuantity { get; set; }
        public int AlredyQCQty { get; set; }
        public int YetToQCQty { get; set; }
        #endregion

        #region Functions
        public static List<GUQCDetail> Gets(int nGUQCID, long nUserID)
        {
            return GUQCDetail.Service.Gets(nGUQCID, nUserID);
        }
        public static List<GUQCDetail> Gets(string sSQL, long nUserID)
        {
            return GUQCDetail.Service.Gets(sSQL, nUserID);
        }
        public GUQCDetail Get(int id, long nUserID)
        {
            return GUQCDetail.Service.Get(id, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IGUQCDetailService Service
        {
            get { return (IGUQCDetailService)Services.Factory.CreateService(typeof(IGUQCDetailService)); }
        }
        #endregion

        public List<GUQCDetail> GUQCDetails { get; set; }
    }
    #endregion

    #region IGUQCDetail interface
    public interface IGUQCDetailService
    {
        GUQCDetail Get(int id, Int64 nUserID);
        List<GUQCDetail> Gets(int nGUQCID, Int64 nUserID);
        List<GUQCDetail> Gets(string sSQL, Int64 nUserID);
    }
    #endregion
}
