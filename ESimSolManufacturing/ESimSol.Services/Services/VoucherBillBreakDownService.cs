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
    public class VoucherBillBreakDownService : MarshalByRefObject, IVoucherBillBreakDownService
    {
        #region Private functions and declaration
        private VoucherBillBreakDown MapObject(NullHandler oReader)
        {
            VoucherBillBreakDown oVoucherBillBreakDown = new VoucherBillBreakDown();
            oVoucherBillBreakDown.VoucherBillID = oReader.GetInt32("VoucherBillID");
            oVoucherBillBreakDown.BillNo = oReader.GetString("BillNo");
            oVoucherBillBreakDown.OpeningValue = oReader.GetDouble("OpeiningValue");
            oVoucherBillBreakDown.IsDrOpen = oReader.GetBoolean("IsDrOpen");
            oVoucherBillBreakDown.DebitAmount = oReader.GetDouble("DebitAmount");
            oVoucherBillBreakDown.CreditAmount = oReader.GetDouble("CreditAmount");
            oVoucherBillBreakDown.ClosingValue = oReader.GetDouble("ClosingValue");
            oVoucherBillBreakDown.IsDrClosing = oReader.GetBoolean("IsDrClosing");
            oVoucherBillBreakDown.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oVoucherBillBreakDown.VoucherID = oReader.GetInt32("VoucherID");
            oVoucherBillBreakDown.VoucherNo = oReader.GetString("VoucherNo");
            oVoucherBillBreakDown.VoucherDate = oReader.GetDateTime("VoucherDate");
            oVoucherBillBreakDown.AccountHeadName = oReader.GetString("AccountHeadName");
            return oVoucherBillBreakDown;
        }

        private VoucherBillBreakDown CreateObject(NullHandler oReader)
        {
            VoucherBillBreakDown oVoucherBillBreakDown = new VoucherBillBreakDown();
            oVoucherBillBreakDown = MapObject(oReader);
            return oVoucherBillBreakDown;
        }

        private List<VoucherBillBreakDown> CreateObjects(IDataReader oReader)
        {
            List<VoucherBillBreakDown> oVoucherBillBreakDown = new List<VoucherBillBreakDown>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                VoucherBillBreakDown oItem = CreateObject(oHandler);
                oVoucherBillBreakDown.Add(oItem);
            }
            return oVoucherBillBreakDown;
        }

        #endregion

        #region Interface implementation
        public VoucherBillBreakDownService() { }
        public List<VoucherBillBreakDown> Gets(int nAccountHeadID, int nCurrencyID, DateTime StartDate, DateTime EndDate, bool bIsApproved, int nCompanyID, int nUserId)
        {
            List<VoucherBillBreakDown> oVoucherBillBreakDown = null;

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VoucherBillBreakDownDA.Gets(tc, nAccountHeadID, nCurrencyID, StartDate, EndDate, bIsApproved, nCompanyID);
                oVoucherBillBreakDown = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();


                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VoucherBillBreakDown", e);
                #endregion
            }
            return oVoucherBillBreakDown;
        }

        public List<VoucherBillBreakDown> GetsForVoucherBill(int nAccountHeadID, int nVoucherBill, int nCurrencyID, DateTime StartDate, DateTime EndDate, bool IsApproved, int nCompanyID, int nUserId)
        {
            List<VoucherBillBreakDown> oVoucherBillBreakDown = null;

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VoucherBillBreakDownDA.GetsForVoucherBill(tc, nAccountHeadID, nVoucherBill, nCurrencyID, StartDate, EndDate, IsApproved, nCompanyID);
                oVoucherBillBreakDown = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VoucherBillBreakDown", e);
                #endregion
            }

            return oVoucherBillBreakDown;
        }
        #endregion
    }
}
