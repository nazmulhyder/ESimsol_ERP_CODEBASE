using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.Services.Services
{
    public class VoucherBillService : MarshalByRefObject, IVoucherBillService
    {
        #region Private functions and declaration
        private VoucherBill MapObject(NullHandler oReader)
        {
            VoucherBill oVoucherBill = new VoucherBill();
            oVoucherBill.VoucherBillID = oReader.GetInt32("VoucherBillID");
            oVoucherBill.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oVoucherBill.SubLedgerID = oReader.GetInt32("SubLedgerID");
            oVoucherBill.BUID = oReader.GetInt32("BUID");            
            oVoucherBill.BillNo = oReader.GetString("BillNo");
            oVoucherBill.BillDate = oReader.GetDateTime("BillDate");
            oVoucherBill.DueDate = oReader.GetDateTime("DueDate");
            oVoucherBill.CreditDays = oReader.GetInt32("CreditDays");
            oVoucherBill.Amount = oReader.GetDouble("Amount");
            oVoucherBill.IsActive = oReader.GetBoolean("IsActive");
            oVoucherBill.CurrencyID = oReader.GetInt32("CurrencyID");
            oVoucherBill.CurrencyRate = oReader.GetDouble("CurrencyRate");
            oVoucherBill.CurrencyAmount = oReader.GetDouble("CurrencyAmount");
            oVoucherBill.ReferenceType = (EnumVoucherBillReferenceType)oReader.GetInt32("ReferenceType");
            oVoucherBill.ReferenceTypeInInt = oReader.GetInt32("ReferenceType");
            oVoucherBill.ReferenceObjID = oReader.GetInt32("ReferenceObjID");
            oVoucherBill.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oVoucherBill.BaseCurrencyID = oReader.GetInt32("BaseCurrencyID");
            oVoucherBill.BaseCurrencySymbol = oReader.GetString("BaseCurrencySymbol");
            oVoucherBill.RemainningBalance = oReader.GetDouble("RemainningBalance");
            oVoucherBill.AccountHeadName = oReader.GetString("AccountHeadName");
            oVoucherBill.SubLedgerCode = oReader.GetString("SubLedgerCode");
            oVoucherBill.SubLedgerName = oReader.GetString("SubLedgerName");
            oVoucherBill.BUCode = oReader.GetString("BUCode");
            oVoucherBill.BUName = oReader.GetString("BUName");
            oVoucherBill.BUShortName = oReader.GetString("BUShortName");
            oVoucherBill.IsDebit = oReader.GetBoolean("IsDebit");
            oVoucherBill.OpeningBillAmount = oReader.GetDouble("OpeningBillAmount");
            oVoucherBill.OpeningBillDate = oReader.GetDateTime("OpeningBillDate");
            oVoucherBill.Remarks = oReader.GetString("Remarks");
            oVoucherBill.OverDueDays = oReader.GetInt32("OverDueDays");
            oVoucherBill.DueDays = oReader.GetInt32("DueDays");
            oVoucherBill.IsHoldBill = oReader.GetBoolean("IsHoldBill");
            oVoucherBill.DueCheque = oReader.GetDouble("DueCheque");
            return oVoucherBill;
        }

        private VoucherBill CreateObject(NullHandler oReader)
        {
            VoucherBill oVoucherBill = new VoucherBill();
            oVoucherBill = MapObject(oReader);
            return oVoucherBill;
        }

        private List<VoucherBill> CreateObjects(IDataReader oReader)
        {
            List<VoucherBill> oVoucherBill = new List<VoucherBill>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                VoucherBill oItem = CreateObject(oHandler);
                oVoucherBill.Add(oItem);
            }
            return oVoucherBill;
        }

        #endregion

        #region Interface implementation

        public VoucherBillService() { }

        public VoucherBill Save(VoucherBill oVoucherBill, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oVoucherBill.VoucherBillID <= 0)
                {
                    reader = VoucherBillDA.InsertUpdate(tc, oVoucherBill, EnumDBOperation.Insert,nUserId);
                }
                else
                {
                    reader = VoucherBillDA.InsertUpdate(tc, oVoucherBill, EnumDBOperation.Update,nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVoucherBill = new VoucherBill();
                    oVoucherBill = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oVoucherBill = new VoucherBill();
                oVoucherBill.ErrorMessage = e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save VoucherBill. Because of " + e.Message, e);
                
                #endregion
            }
            return oVoucherBill;
        }
        public VoucherBill HoldUnHold(VoucherBill oVoucherBill, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                VoucherBillDA.HoldUnHold(tc, oVoucherBill);
                reader = VoucherBillDA.Get(tc, oVoucherBill.VoucherBillID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVoucherBill = new VoucherBill();
                    oVoucherBill = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oVoucherBill = new VoucherBill();
                oVoucherBill.ErrorMessage = e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save VoucherBill. Because of " + e.Message, e);

                #endregion
            }
            return oVoucherBill;
        }
       
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                VoucherBill oVoucherBill = new VoucherBill();
                oVoucherBill.VoucherBillID = id;                
                VoucherBillDA.Delete(tc, oVoucherBill, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                #endregion
            }
            return "Delete Successful";
        }

      
        public VoucherBill Get(int id, int nUserId)
        {
            VoucherBill oVoucherBill = new VoucherBill();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = VoucherBillDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVoucherBill = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VoucherBill", e);
                #endregion
            }

            return oVoucherBill;
        }

        public List<VoucherBill> GetsBy(int nAccountHeadID, int nUserId)
        {
            List<VoucherBill> oVoucherBills = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VoucherBillDA.Gets(tc, nAccountHeadID);
                oVoucherBills = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VoucherBill", e);
                #endregion
            }

            return oVoucherBills;
        }

        public List<VoucherBill> Gets(string sSQL, int nUserId)
        {
            List<VoucherBill> oVoucherBills = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VoucherBillDA.Gets(tc,sSQL);
                oVoucherBills = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException( e.Message);
                #endregion
            }

            return oVoucherBills;
        }
        public List<VoucherBill> Gets(int nUserId)
        {
            List<VoucherBill> oVoucherBills = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = VoucherBillDA.Gets(tc);
                oVoucherBills = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }

            return oVoucherBills;
        }


        public List<VoucherBill> GetsReceivableOrPayableBill(int nComponentType, int nUserId)
        {
            List<VoucherBill> oVoucherBills = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = VoucherBillDA.GetsReceivableOrPayableBill(tc, nComponentType);
                oVoucherBills = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }

            return oVoucherBills;
        }
        #endregion
    }   


   
}
