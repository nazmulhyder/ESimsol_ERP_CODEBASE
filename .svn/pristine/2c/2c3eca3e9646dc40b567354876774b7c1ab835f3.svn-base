using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region SparePartsRequisition

    public class SparePartsRequisition : BusinessObject
    {
        public SparePartsRequisition()
        {
            SparePartsRequisitionID = 0;
            RequisitionNo = "";
            BUID = 0;
            RequisitionBy = 0;
            SPStatus = EnumSPStatus.Initiallize;
            SPStatusInt = 0;
            IssueDate = DateTime.Today;
            CRID = 0;
            Remarks = "";
            ApprovedBy = 0;
            RequisitionByName = "";
            ApprovedByName = "";
            DeliveryByName = "";
            CapitalResourceName = "";
            SparePartsRequisitionLogID = 0;
            ErrorMessage = "";
            ActionTypeExtra = "";
            SparePartsRequisitionDetails = new List<SparePartsRequisitionDetail>();
            Company = new Company();
            BusinessUnit = new BusinessUnit();
        }

        #region Properties
        public int SparePartsRequisitionID { get; set; }
        public string RequisitionNo { get; set; }
        public int BUID { get; set; }
        public int RequisitionBy { get; set; }
        public EnumSPStatus SPStatus { get; set; }
        public int SPStatusInt { get; set; }
        public DateTime IssueDate { get; set; }
        public int CRID { get; set; }
        public string Remarks { get; set; }
        public int ApprovedBy { get; set; }
        public string RequisitionByName { get; set; }
        public string ApprovedByName { get; set; }
        public string DeliveryByName { get; set; }
        public string CapitalResourceName { get; set; }
        public int SparePartsRequisitionLogID { get; set; }
        public string ActionTypeExtra { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string SPStatusSt
        {
            get
            {
                return EnumObject.jGet(this.SPStatus);
            }
        }
        public string IssueDateSt
        {
            get
            {
                return this.IssueDate.ToString("dd MMM yyyy");
            }
        }
        public List<SparePartsRequisition> SparePartsRequisitions { get; set; }
        public List<SparePartsRequisitionDetail> SparePartsRequisitionDetails { get; set; }
        public List<User> Users { get; set; }
        public Company Company { get; set; }
        public BusinessUnit BusinessUnit { get; set; }
        #endregion

        #region Functions
        public static List<SparePartsRequisition> Gets(long nUserID)
        {
            return SparePartsRequisition.Service.Gets(nUserID);
        }
        public static List<SparePartsRequisition> Gets(string sSQL, long nUserID)
        {
            return SparePartsRequisition.Service.Gets(sSQL, nUserID);
        }
        public SparePartsRequisition Get(int id, long nUserID)
        {
            return SparePartsRequisition.Service.Get(id, nUserID);
        }
        public SparePartsRequisition GetLog(int id, long nUserID)
        {
            return SparePartsRequisition.Service.GetLog(id, nUserID);
        }
        public SparePartsRequisition ChangeStatus(long nUserID)
        {
            return SparePartsRequisition.Service.ChangeStatus(this, nUserID);
        }
        public SparePartsRequisition Save(long nUserID)
        {
            return SparePartsRequisition.Service.Save(this, nUserID);
        }
        //public SparePartsRequisition Delivery(long nUserID)
        //{
        //    return SparePartsRequisition.Service.Delivery(this, nUserID);
        //}
        public string Delete(int id, long nUserID)
        {
            return SparePartsRequisition.Service.Delete(id, nUserID);
        }
        public SparePartsRequisition AcceptSparePartsRequisitionRevise(long nUserID)
        {
            return SparePartsRequisition.Service.AcceptSparePartsRequisitionRevise(this, nUserID);
        }
        public SparePartsRequisition UpdateVoucherEffect(long nUserID)
        {
            return SparePartsRequisition.Service.UpdateVoucherEffect(this, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static ISparePartsRequisitionService Service
        {
            get { return (ISparePartsRequisitionService)Services.Factory.CreateService(typeof(ISparePartsRequisitionService)); }
        }
        #endregion
    }
    #endregion


    #region ISparePartsRequisition interface

    public interface ISparePartsRequisitionService
    {
        SparePartsRequisition Get(int id, Int64 nUserID);
        SparePartsRequisition GetLog(int id, Int64 nUserID);
        List<SparePartsRequisition> Gets(Int64 nUserID);
        List<SparePartsRequisition> Gets(string sSQL, Int64 nUserID);
        SparePartsRequisition ChangeStatus(SparePartsRequisition oSparePartsRequisition, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        SparePartsRequisition AcceptSparePartsRequisitionRevise(SparePartsRequisition oSparePartsRequisition, Int64 nUserID);
        SparePartsRequisition Save(SparePartsRequisition oSparePartsRequisition, Int64 nUserID);
        //SparePartsRequisition Delivery(SparePartsRequisition oSparePartsRequisition, Int64 nUserID);
        SparePartsRequisition UpdateVoucherEffect(SparePartsRequisition oSparePartsRequisition, Int64 nUserID);   
    }
    #endregion
}