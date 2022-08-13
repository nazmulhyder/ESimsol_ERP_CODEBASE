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
    #region PartsRequisition

    public class PartsRequisition : BusinessObject
    {
        public PartsRequisition()
        {
            PartsRequisitionID = 0;
            ServiceOrderID = 0;
            VehicleRegID = 0;
            RequisitionNo = "";
            CustomerName = "";
            BUID = 0;
            PRTypeInt = 0;
            RequisitionBy = 0;
            PRStatus = EnumCRStatus.Initiallize;
            PRStatusInt = 0;
            IssueDate = DateTime.Today;
            StoreID = 0;
            Remarks = "";
            Note = "";
            DeliveryBy = 0;
            ApprovedBy = 0;
            StoreCode = "";
            StoreName = "";
            RequisitionByName = "";
            ApprovedByName = "";
            DeliveryByName = "";            
            Amount = 0;
            PartsRequisitionLogID = 0;
            ErrorMessage = "";
            PartsRequisitionDetails = new List<PartsRequisitionDetail>();
            Company = new Company();
            BusinessUnit = new BusinessUnit();
            ClientOperationSetting = new ClientOperationSetting();
            
        }

        #region Properties
        public int PartsRequisitionID { get; set; }
        public int ServiceOrderID { get; set; }
        public int VehicleRegID { get; set; }
        public string RequisitionNo { get; set; }
        public int BUID { get; set; }
        public EnumPRequisutionType PRType { get; set; }
        public int PRTypeInt { get; set; }
        public int RequisitionBy { get; set; }
        public string CustomerName { get; set; }
        public EnumCRStatus PRStatus { get; set; }
        public int PRStatusInt { get; set; }
        public DateTime IssueDate { get; set; }
        public int StoreID { get; set; }
        public string Remarks { get; set; }
        public string Note { get; set; }
        public int DeliveryBy { get; set; }
        public int ApprovedBy { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public string ServiceOrderNo { get; set; }
        public string ModelNo { get; set; }
        public string ChassisNo { get; set; }
        public string EngineNo { get; set; }
        public string VehicleRegNo { get; set; }
        public string RequisitionByName { get; set; }
        public string ApprovedByName { get; set; }
        public string DeliveryByName { get; set; }
        public double Amount { get; set; }
        public int PartsRequisitionLogID { get; set; }        
        public string ActionTypeExtra { get; set; }
        public EnumCRActionType PRActionType { get; set; }
        public int PRActionTypeInt { get; set; }
        public ClientOperationSetting ClientOperationSetting { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        
        public string PRStatusSt
        {
            get
            {
                return EnumObject.jGet(this.PRStatus);
            }
        }
        public string PRTypeSt
        {
            get
            {
                if (this.PRType == 0) 
                {
                    return "None";
                }
                return EnumObject.jGet(this.PRType);
            }
        }
        public string IssueDateSt
        {
            get
            {
                return this.IssueDate.ToString("dd MMM yyyy");
            }
        }
        
        public List<PartsRequisition> PartsRequisitions { get; set; }
        public List<PartsRequisitionDetail> PartsRequisitionDetails { get; set; }
        public List<User> Users { get; set; }
        public Company Company { get; set; }
        public BusinessUnit BusinessUnit { get; set; }
        #endregion

        #region Functions
        public static List<PartsRequisition> Gets(long nUserID)
        {
            return PartsRequisition.Service.Gets(nUserID);
        }
        public static List<PartsRequisition> Gets(string sSQL, long nUserID)
        {
            return PartsRequisition.Service.Gets(sSQL, nUserID);
        }
        public PartsRequisition Get(int id, long nUserID)
        {
            return PartsRequisition.Service.Get(id, nUserID);
        }
        public PartsRequisition GetLog(int id, long nUserID)
        {
            return PartsRequisition.Service.GetLog(id, nUserID);
        }
        public PartsRequisition ChangeStatus(long nUserID)
        {
            return PartsRequisition.Service.ChangeStatus(this, nUserID);
        }
        public PartsRequisition Save(long nUserID)
        {
            return PartsRequisition.Service.Save(this, nUserID);
        }
        public PartsRequisition Delivery(long nUserID)
        {
            return PartsRequisition.Service.Delivery(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return PartsRequisition.Service.Delete(id, nUserID);
        }
        public PartsRequisition AcceptPartsRequisitionRevise(long nUserID)
        {
            return PartsRequisition.Service.AcceptPartsRequisitionRevise(this, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IPartsRequisitionService Service
        {
            get { return (IPartsRequisitionService)Services.Factory.CreateService(typeof(IPartsRequisitionService)); }
        }
        #endregion
    }
    #endregion

    #region IPartsRequisition interface

    public interface IPartsRequisitionService
    {
        PartsRequisition Get(int id, Int64 nUserID);
        PartsRequisition GetLog(int id, Int64 nUserID);
        List<PartsRequisition> Gets(Int64 nUserID);
        List<PartsRequisition> Gets(string sSQL, Int64 nUserID);
        PartsRequisition ChangeStatus(PartsRequisition oPartsRequisition, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        PartsRequisition AcceptPartsRequisitionRevise(PartsRequisition oPartsRequisition, Int64 nUserID);
        PartsRequisition Save(PartsRequisition oPartsRequisition, Int64 nUserID);
        PartsRequisition Delivery(PartsRequisition oPartsRequisition, Int64 nUserID);
    }
    #endregion
}