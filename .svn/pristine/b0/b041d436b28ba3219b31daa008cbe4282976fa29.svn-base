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
    #region CashFlowSetup
    public class CashFlowSetup : BusinessObject
    {
        public CashFlowSetup()
        {
            CashFlowSetupID = 0;
            CFTransactionCategory = EnumCFTransactionCategory.Operating_Activities;
            CFTransactionGroup = EnumCFTransactionGroup.Cash_Receipts;
            CFDataType = EnumCFDataType.Net_Trunover_of_SCI;
            CFTransactionCategoryInInt = 0;
            CFTransactionGroupInInt = 0;
            CFDataTypeInInt = 0;
            SubGroupID = 0;
            DisplayCaption = "";
            Remarks = "";
            SubGroupCode = "";
            SubGroupName = "";
            SubGroupType = 0;
            Amount = 0;
            IsPaymentDetails = false;
            ErrorMessage = "";
        }

        #region Property
        public int CashFlowSetupID { get; set; }
        public EnumCFTransactionCategory CFTransactionCategory { get; set; }
        public EnumCFTransactionGroup CFTransactionGroup { get; set; }
        public EnumCFDataType CFDataType { get; set; }
        public int SubGroupID { get; set; }
        public string DisplayCaption { get; set; }
        public string Remarks { get; set; }
        public string SubGroupCode { get; set; }
        public string SubGroupName { get; set; }
        public int SubGroupType { get; set; }
        public double Amount { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public int CFTransactionCategoryInInt { get; set; }
        public int CFTransactionGroupInInt { get; set; }
        public int CFDataTypeInInt { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsPaymentDetails { get; set; }
        public string StartDateSt { get { return this.StartDate.ToString("dd MMM yyyy"); } }
        public string EndDateSt { get { return this.EndDate.ToString("dd MMM yyyy"); } }
        public string SessionDate { get; set; }
        public int BUID { get; set; }
        public Company Company { get; set; }
        public List<CashFlowSetup> CashFlowSetups { get; set; }
        public string CFTransactionCategoryInString
        {
            get
            {
                return EnumObject.jGet(this.CFTransactionCategory);
            }
        }
        public string CFTransactionGroupInString
        {
            get
            {
                return EnumObject.jGet(this.CFTransactionGroup);
            }
        }
        public string CFDataTypeInString
        {
            get
            {
                return EnumObject.jGet(this.CFDataType);
            }
        }

        public string AmountSt
        {
            get
            {
                if (this.Amount == 0.00)
                {
                    return "-";
                }
                else if (this.Amount < 0.00)
                {
                    return "(" + Global.MillionFormat(this.Amount * (-1)) + ")";
                }
                else
                {
                    return Global.MillionFormat(this.Amount);
                }
            }
        }
        #endregion

        #region Functions
        public static List<CashFlowSetup> Gets(int nBUID, DateTime dStartDate, DateTime dEndDate, bool bIsDetails, long nUserID)
        {
            return CashFlowSetup.Service.Gets(nBUID, dStartDate, dEndDate,bIsDetails, nUserID);
        }

        public static List<CashFlowSetup> Gets(long nUserID)
        {
            return CashFlowSetup.Service.Gets(nUserID);
        }
        public static List<CashFlowSetup> Gets(string sSQL, long nUserID)
        {
            return CashFlowSetup.Service.Gets(sSQL, nUserID);
        }
        public CashFlowSetup Get(int id, long nUserID)
        {
            return CashFlowSetup.Service.Get(id, nUserID);
        }
        public CashFlowSetup Save(long nUserID)
        {
            return CashFlowSetup.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return CashFlowSetup.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ICashFlowSetupService Service
        {
            get { return (ICashFlowSetupService)Services.Factory.CreateService(typeof(ICashFlowSetupService)); }
        }
        #endregion
    }
    #endregion

    #region ICashFlowSetup interface
    public interface ICashFlowSetupService
    {
        CashFlowSetup Get(int id, Int64 nUserID);
        List<CashFlowSetup> Gets(int nBUID, DateTime dStartDate, DateTime dEndDate, bool bIsDetails, Int64 nUserID);
        List<CashFlowSetup> Gets(Int64 nUserID);
        List<CashFlowSetup> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        CashFlowSetup Save(CashFlowSetup oCashFlowSetup, Int64 nUserID);
    }
    #endregion
}