using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{

    #region CashFlowHead

    public class CashFlowHead : BusinessObject
    {
        public CashFlowHead()
        {
            CashFlowHeadID = 0;
            CashFlowHeadType = EnumCashFlowHeadType.None;
            CashFlowHeadTypeInt = 0;
            DisplayCaption = "";
            IsDebit = false;
            Remarks = "";
            Sequence = 0;
            ErrorMessage = "";
            CashFlowHeads = new List<CashFlowHead>();
        }

        #region Properties
        public int CashFlowHeadID { get; set; }
        public EnumCashFlowHeadType CashFlowHeadType { get; set; }
        public int CashFlowHeadTypeInt { get; set; }
        public string DisplayCaption { get; set; }
        public bool IsDebit { get; set; }        
        public string Remarks { get; set; }
        public int Sequence { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<CashFlowHead> CashFlowHeads { get; set; }
        public string CashFlowHeadTypeSt
        {
            get
            {
                return EnumObject.jGet(this.CashFlowHeadType);
            }
        }
        public string IsDebitSt
        {
            get
            {
                if (this.CashFlowHeadID <= 0 || this.CashFlowHeadType == EnumCashFlowHeadType.Effected_Accounts || this.CashFlowHeadType == EnumCashFlowHeadType.Begaining_Cash_Caption || this.CashFlowHeadType == EnumCashFlowHeadType.Closing_Cash_Caption || this.CashFlowHeadType == EnumCashFlowHeadType.Net_Cash_Flow_Caption || this.CashFlowHeadType == EnumCashFlowHeadType.Operating_Opening_Caption || this.CashFlowHeadType == EnumCashFlowHeadType.Operating_Closing_Caption || this.CashFlowHeadType == EnumCashFlowHeadType.Investing_Opening_Caption || this.CashFlowHeadType == EnumCashFlowHeadType.Investing_Closing_Caption || this.CashFlowHeadType == EnumCashFlowHeadType.Financing_Opening_Caption || this.CashFlowHeadType == EnumCashFlowHeadType.Financing_Closing_Caption)
                {
                    return "None";
                }
                else
                {
                    if (this.IsDebit)
                    {
                        return "Debit";
                    }
                    else
                    {
                        return "Credit";
                    }
                }


                
            }
        }
        public int IsDebitInt
        {
            get
            {
                if (this.CashFlowHeadID <= 0 || this.CashFlowHeadType == EnumCashFlowHeadType.Effected_Accounts || this.CashFlowHeadType == EnumCashFlowHeadType.Begaining_Cash_Caption || this.CashFlowHeadType == EnumCashFlowHeadType.Closing_Cash_Caption || this.CashFlowHeadType == EnumCashFlowHeadType.Net_Cash_Flow_Caption || this.CashFlowHeadType == EnumCashFlowHeadType.Operating_Opening_Caption || this.CashFlowHeadType == EnumCashFlowHeadType.Operating_Closing_Caption || this.CashFlowHeadType == EnumCashFlowHeadType.Investing_Opening_Caption || this.CashFlowHeadType == EnumCashFlowHeadType.Investing_Closing_Caption || this.CashFlowHeadType == EnumCashFlowHeadType.Financing_Opening_Caption || this.CashFlowHeadType == EnumCashFlowHeadType.Financing_Closing_Caption)
                {
                    return 0; //None
                }
                else
                {
                    if (this.IsDebit)
                    {
                        return 1; //Debit
                    }
                    else
                    {
                        return 2; //Credit
                    }
                }
            }
        }
        #endregion

        #region Functions
        public static List<CashFlowHead> Gets(long nUserID)
        {
            return CashFlowHead.Service.Gets(nUserID);
        }
        public static List<CashFlowHead> Gets(string sSQL, long nUserID)
        {
            return CashFlowHead.Service.Gets(sSQL, nUserID);
        }
        public CashFlowHead Get(int id, long nUserID)
        {
            return CashFlowHead.Service.Get(id, nUserID);
        }
        public CashFlowHead Save(long nUserID)
        {
            return CashFlowHead.Service.Save(this, nUserID);
        }
        public String UpdateScequence(long nUserID)
        {
            return CashFlowHead.Service.UpdateScequence(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return CashFlowHead.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ICashFlowHeadService Service
        {
            get { return (ICashFlowHeadService)Services.Factory.CreateService(typeof(ICashFlowHeadService)); }
        }
        #endregion
    }
    #endregion

    #region ICashFlowHead interface
    public interface ICashFlowHeadService
    {
        CashFlowHead Get(int id, Int64 nUserID);
        List<CashFlowHead> Gets(Int64 nUserID);
        List<CashFlowHead> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        CashFlowHead Save(CashFlowHead oCashFlowHead, Int64 nUserID);
        string UpdateScequence(CashFlowHead oCashFlowHead, Int64 nUserID);
    }
    #endregion
}