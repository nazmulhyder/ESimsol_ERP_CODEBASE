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
    public class VoucherDetailService : MarshalByRefObject, IVoucherDetailService
    {
        #region Private functions and declaration
        private VoucherDetail MapObject(NullHandler oReader)
        {
            VoucherDetail oVoucherDetail = new VoucherDetail();
            oVoucherDetail.VoucherDetailID = oReader.GetInt32("VoucherDetailID");
            oVoucherDetail.VoucherID = oReader.GetInt32("VoucherID");
            oVoucherDetail.BUID = oReader.GetInt32("BUID");
            oVoucherDetail.AreaID = oReader.GetInt32("AreaID");
            oVoucherDetail.ZoneID = oReader.GetInt32("ZoneID");
            oVoucherDetail.SiteID = oReader.GetInt32("SiteID");
            oVoucherDetail.ProductID = oReader.GetInt32("ProductID");
            oVoucherDetail.DeptID = oReader.GetInt32("DeptID");
            oVoucherDetail.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oVoucherDetail.CostCenterID = oReader.GetInt32("CostCenterID");
            oVoucherDetail.CurrencyID = oReader.GetInt32("CurrencyID");
            oVoucherDetail.AmountInCurrency = oReader.GetDouble("AmountInCurrency");
            oVoucherDetail.ConversionRate = oReader.GetDouble("ConversionRate");
            oVoucherDetail.Amount = oReader.GetDouble("Amount");
            oVoucherDetail.IsDebit = oReader.GetBoolean("IsDebit");
            oVoucherDetail.Narration = oReader.GetString("Narration");
            oVoucherDetail.AccountHeadCode = oReader.GetString("AccountHeadCode");
            oVoucherDetail.AccountHeadName = oReader.GetString("AccountHeadName");
            oVoucherDetail.OperationType = (EnumVoucherOperationType)oReader.GetInt32("OperationType"); ;
            oVoucherDetail.VoucherDate = oReader.GetDateTime("VoucherDate");
            oVoucherDetail.VoucherTypeID = oReader.GetInt32("VoucherTypeID");
            oVoucherDetail.AuthorizedBy = oReader.GetInt32("AuthorizedBy");
            oVoucherDetail.CUName = oReader.GetString("CUName");
            oVoucherDetail.CUSymbol = oReader.GetString("CUSymbol");            
            oVoucherDetail.AreaCode = oReader.GetString("AreaCode");
            oVoucherDetail.AreaName = oReader.GetString("AreaName");
            oVoucherDetail.AreaShortName = oReader.GetString("AreaShortName");
            oVoucherDetail.ZoneCode = oReader.GetString("ZoneCode");
            oVoucherDetail.ZoneName = oReader.GetString("ZoneName");
            oVoucherDetail.ZoneShortName = oReader.GetString("ZoneShortName");
            oVoucherDetail.SiteCode = oReader.GetString("SiteCode");
            oVoucherDetail.SiteName = oReader.GetString("SiteName");
            oVoucherDetail.SiteShortName = oReader.GetString("SiteShortName");
            oVoucherDetail.PCode = oReader.GetString("PCode");
            oVoucherDetail.PName = oReader.GetString("PName");
            oVoucherDetail.PShortName = oReader.GetString("PShortName");
            oVoucherDetail.DeptCode = oReader.GetString("DeptCode");
            oVoucherDetail.DeptName = oReader.GetString("DeptName");
            oVoucherDetail.DeptShortName = oReader.GetString("DeptShortName");
            oVoucherDetail.CCCode = oReader.GetString("CCCode");
            oVoucherDetail.CCName = oReader.GetString("CCName");
            oVoucherDetail.IsAreaEffect = oReader.GetBoolean("IsAreaEffect");
            oVoucherDetail.IsZoneEffect = oReader.GetBoolean("IsZoneEffect");
            oVoucherDetail.IsSiteEffect = oReader.GetBoolean("IsSiteEffect");
            oVoucherDetail.IsCostCenterApply = oReader.GetBoolean("IsCostCenterApply");
            oVoucherDetail.IsBillRefApply = oReader.GetBoolean("IsBillRefApply");
            oVoucherDetail.IsInventoryApply = oReader.GetBoolean("IsInventoryApply");
            oVoucherDetail.IsOrderReferenceApply = oReader.GetBoolean("IsOrderReferenceApply");
            oVoucherDetail.IsChequeApply = (EnumAccountOperationType)oReader.GetInt16("AccountOperationType") == EnumAccountOperationType.BankAccount;
            oVoucherDetail.IsPaymentCheque = oReader.GetBoolean("IsPaymentCheque");
            if (oVoucherDetail.IsDebit)
            {
                oVoucherDetail.DebitAmount = oReader.GetDouble("AmountInCurrency");
                oVoucherDetail.CreditAmount = 0.00;

                oVoucherDetail.BCDebitAmount = oReader.GetDouble("Amount");
                oVoucherDetail.BCCreditAmount = 0.00;
            }
            else
            {
                oVoucherDetail.DebitAmount = 0.00;
                oVoucherDetail.CreditAmount = oReader.GetDouble("AmountInCurrency");

                oVoucherDetail.BCDebitAmount = 0.00;
                oVoucherDetail.BCCreditAmount = oReader.GetDouble("Amount");
            }
            oVoucherDetail.LedgerBalance = oReader.GetString("LedgerBalance");
            return oVoucherDetail;
        }

        private VoucherDetail CreateObject(NullHandler oReader)
        {
            VoucherDetail oVoucherDetail = new VoucherDetail();
            oVoucherDetail = MapObject(oReader);
            return oVoucherDetail;
        }

        private List<VoucherDetail> CreateObjects(IDataReader oReader)
        {
            List<VoucherDetail> oVoucherDetail = new List<VoucherDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                VoucherDetail oItem = CreateObject(oHandler);
                oVoucherDetail.Add(oItem);
            }
            return oVoucherDetail;
        }

        #endregion

        #region Interface implementation
        public VoucherDetailService() { }

        public VoucherDetail Save(VoucherDetail oVoucherDetail, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oVoucherDetail.VoucherDetailID <= 0)
                {
                    reader = VoucherDetailDA.InsertUpdate(tc, oVoucherDetail, EnumDBOperation.Insert,"");
                }
                else
                {
                    reader = VoucherDetailDA.InsertUpdate(tc, oVoucherDetail, EnumDBOperation.Update,"");
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVoucherDetail = new VoucherDetail();
                    oVoucherDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save VoucherDetail. Because of " + e.Message, e);
                #endregion
            }
            return oVoucherDetail;
        }
                               
        public VoucherDetail GetProfitLossAccountTransaction(int nBUID, DateTime dStartDate, DateTime dEndDate, int nCompanyID, int nUserID)
        {
            VoucherDetail oAccountHead = new VoucherDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = VoucherDetailDA.GetProfitLossAccountTransaction(tc, nBUID, dStartDate, dEndDate, nCompanyID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
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
                throw new ServiceException(e.Message);
                #endregion
            }

            return oAccountHead;
        }

        public VoucherDetail GetDividendTransaction(int nBUID, DateTime dStartDate, DateTime dEndDate, int nUserID)
        {
            VoucherDetail oAccountHead = new VoucherDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = VoucherDetailDA.GetDividendTransaction(tc, nBUID, dStartDate, dEndDate);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
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
                throw new ServiceException(e.Message);
                #endregion
            }

            return oAccountHead;
        }

        public VoucherDetail GetRetaindEarningTransaction(int nBUID, DateTime dStartDate, DateTime dEndDate, int nUserID)
        {
            VoucherDetail oAccountHead = new VoucherDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = VoucherDetailDA.GetRetaindEarningTransaction(tc, nBUID, dStartDate, dEndDate);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
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
                throw new ServiceException(e.Message);
                #endregion
            }

            return oAccountHead;
        }
        
        public VoucherDetail Get(int id, int nUserId)
        {
            VoucherDetail oAccountHead = new VoucherDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = VoucherDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
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
                throw new ServiceException(e.Message);
                #endregion
            }

            return oAccountHead;
        }

        public double GetCurrentBalance(int nAccountHeadID, bool bIsDebit, DateTime dStartDate, DateTime dEndDate, int nUserId)
        {
            double nCurrentBalance = 0;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = VoucherDetailDA.GetCurrentBalance(tc, nAccountHeadID,bIsDebit,dStartDate,dEndDate);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    nCurrentBalance = oReader.GetDouble("CurrentBalance");
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
                throw new ServiceException(e.Message);
                #endregion
            }

            return nCurrentBalance;
        }

        public List<VoucherDetail> Gets(int nUserId)
        {
            List<VoucherDetail> oVoucherDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VoucherDetailDA.Gets(tc);
                oVoucherDetail = CreateObjects(reader);
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

            return oVoucherDetail;
        }

        public List<VoucherDetail> Gets(long nVoucherId, int nUserId)
        {
            List<VoucherDetail> oVoucherDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VoucherDetailDA.Gets(tc, nVoucherId, nUserId);
                oVoucherDetail = CreateObjects(reader);
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

            return oVoucherDetail;
        }
        public List<VoucherDetail> Gets(string sSQL, int nUserId)
        {
            List<VoucherDetail> oVoucherDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VoucherDetailDA.Gets(tc, sSQL);
                oVoucherDetail = CreateObjects(reader);
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

            return oVoucherDetail;
        }
        #endregion
    }   
}