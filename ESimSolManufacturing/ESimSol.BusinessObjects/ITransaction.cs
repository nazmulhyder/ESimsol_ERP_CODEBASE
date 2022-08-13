using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region ITransaction
    public class ITransaction
    {
        #region  Constructor
        public ITransaction()
        {
            ITransactionID = 0;
            ProductID = 0;
            LotID = 0;
            Qty = 0;
            CurrentBalance = 0;
            MUnitID = 0;
            UnitPrice = 0;
            CurrencyID = 0;
            InOutType = EnumInOutType.None;
            Description = "";
            WorkingUnitID = 0;
            TriggerParentID = 0;
            TriggerParentType = EnumTriggerParentsType.None;
            TransactionTime = DateTime.Now;
            DBUserID = 0;
            ProductCode = "";
            ProductName = "";
            LotNo = "";
            RefNo = "";
            UnitName = "";
            USymbol = "";
            StoreName = "";
            AlreadyReturnQty = 0;
            ReturnQty = 0;
            ErrorMessage = "";
            Params = "";
            UserName="";
        }
        #endregion

        #region Properties
        public int ITransactionID { get; set; }
        public int ProductID { get; set; }
        public int LotID { get; set; }
        public double Qty { get; set; }
        public double CurrentBalance { get; set; }
        public int MUnitID { get; set; }
        public double UnitPrice { get; set; }
        public int CurrencyID { get; set; }
        public EnumInOutType InOutType { get; set; }
        public string Description { get; set; }
        public int WorkingUnitID { get; set; }
        public int TriggerParentID { get; set; }
        public EnumTriggerParentsType TriggerParentType { get; set; }
        public DateTime TransactionTime { get; set; }
        public int DBUserID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string LotNo { get; set; }
        public string RefNo { get; set; }
        public string UnitName { get; set; }
        public string USymbol { get; set; }
        public string StoreName { get; set; }
        public double AlreadyReturnQty { get; set; }
        public double ReturnQty { get; set; }
        public string UserName { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        #endregion

        #region Derived Property
        public string TriggerParentTypeSt
        {
            get
            {
                return EnumObject.jGet(this.TriggerParentType);
            }
        }
        public string InOutTypeSt
        {
            get
            {
                return EnumObject.jGet(this.InOutType);
            }
        }
        public string TransactionTimeSt
        {
            get
            {
                return this.TransactionTime.ToString("dd MMM yyyy hh:mm tt");
            }
        }
        public double InOutValue
        {
            get
            {
                return (this.Qty * this.UnitPrice);
            }
        }
        public double YetToReturn
        {
            get
            {
                if(this.Qty>this.AlreadyReturnQty)
                {
                    return this.Qty - this.AlreadyReturnQty;
                }
                else
                {
                    return 0;
                }
            }
        }

        public double Qty_In { get { return (this.InOutType == EnumInOutType.Receive ? this.Qty : 0); } }
        public double Qty_Out { get { return (this.InOutType == EnumInOutType.Disburse ? this.Qty : 0); } }
        #endregion

        #region Functions
        public ITransaction Get(int nITransactionID, int nUserID)
        {
            return ITransaction.Service.Get(nITransactionID, nUserID);
        }
        public static List<ITransaction> Gets(int nUserID)
        {
            return ITransaction.Service.Gets(nUserID);
        }
        public static List<ITransaction> Gets(string sSQl, int nUserID)
        {
            return ITransaction.Service.Gets(sSQl, nUserID);
        }
        public ITransaction UpdateTransaction(int nUserID)
        {
            return ITransaction.Service.UpdateTransaction(this, nUserID);
        }
        #endregion

        #region Non DB Function
        public string IDInString(List<ITransaction> oITransactions)
        {
            string sReturn = "";
            foreach (ITransaction oItem in oITransactions)
            {
                sReturn = sReturn + oItem.ITransactionID.ToString() + ",";
            }
            if (sReturn.Length > 0) sReturn = sReturn.Remove(sReturn.Length - 1, 1);
            return sReturn;
        }
        #endregion

        #region ServiceFactory
        internal static IITransactionService Service
        {
            get { return (IITransactionService)Services.Factory.CreateService(typeof(IITransactionService)); }
        }
        #endregion
    }
    #endregion

    #region IITransaction interface
    public interface IITransactionService
    {
        ITransaction Get(int nITransactionID, int nUserID);
        List<ITransaction> Gets(int nUserID);
        List<ITransaction> Gets(string sSQl, int nUserID);
        ITransaction UpdateTransaction(ITransaction oITransaction, int nUserID);
    }
    #endregion
}