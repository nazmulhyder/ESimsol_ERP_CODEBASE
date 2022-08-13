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
    #region YarnRequisition
    public class YarnRequisition : BusinessObject
    {
        public YarnRequisition()
        {
            YarnRequisitionID = 0;
            BUID = 0;
            RequisitionNo = "";
            RequisitionDate = DateTime.Today;
            MerchandiserID = 0;
            BusinessSessionID = 0;
            BuyerID = 0;
            ApprovedBy = 0;
            Remarks = "";
            BUName = "";
            BuyerName = "";
            MerchandiserName = "";
            ApprovedByName = "";
            SessionName = "";
            ErrorMessage = "";
            Params = "";
            YarnRequisitionDetails = new List<YarnRequisitionDetail>();
            YarnRequisitionProducts = new List<YarnRequisitionProduct>();
        }

        #region Property
        public int YarnRequisitionID { get; set; }
        public int BUID { get; set; }
        public string RequisitionNo { get; set; }
        public DateTime RequisitionDate { get; set; }
        public int MerchandiserID { get; set; }
        public int BusinessSessionID { get; set; }
        public int BuyerID { get; set; }
        public int ApprovedBy { get; set; }
        public string Remarks { get; set; }
        public string BUName { get; set; }
        public string BuyerName { get; set; }
        public string MerchandiserName { get; set; }
        public string ApprovedByName { get; set; }
        public string SessionName { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        #endregion

        #region Derived Property
        public List<YarnRequisitionDetail> YarnRequisitionDetails { get; set; }
        public List<YarnRequisitionProduct> YarnRequisitionProducts { get; set; }
        public string RequisitionDateSt
        {
            get
            {
                return RequisitionDate.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #region Functions
        public static List<YarnRequisition> Gets(long nUserID)
        {
            return YarnRequisition.Service.Gets(nUserID);
        }
        public static List<YarnRequisition> Gets(string sSQL, long nUserID)
        {
            return YarnRequisition.Service.Gets(sSQL, nUserID);
        }
        public YarnRequisition Get(int id, long nUserID)
        {
            return YarnRequisition.Service.Get(id, nUserID);
        }
        public YarnRequisition Save(long nUserID)
        {
            return YarnRequisition.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return YarnRequisition.Service.Delete(id, nUserID);
        }
        public YarnRequisition Approve(long nUserID)
        {
            return YarnRequisition.Service.Approve(this, nUserID);
        }
        public YarnRequisition UnApprove(long nUserID)
        {
            return YarnRequisition.Service.UnApprove(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IYarnRequisitionService Service
        {
            get { return (IYarnRequisitionService)Services.Factory.CreateService(typeof(IYarnRequisitionService)); }
        }
        #endregion
    }
    #endregion

    #region IYarnRequisition interface
    public interface IYarnRequisitionService
    {
        YarnRequisition Get(int id, Int64 nUserID);
        List<YarnRequisition> Gets(Int64 nUserID);
        List<YarnRequisition> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        YarnRequisition Save(YarnRequisition oYarnRequisition, Int64 nUserID);
        YarnRequisition Approve(YarnRequisition oYarnRequisition, Int64 nUserID);
        YarnRequisition UnApprove(YarnRequisition oYarnRequisition, Int64 nUserID);
    }
    #endregion
}
