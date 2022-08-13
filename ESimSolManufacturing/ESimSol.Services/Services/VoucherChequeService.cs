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
    public class VoucherChequeService : MarshalByRefObject, IVoucherChequeService
    {
        #region Private functions and declaration
        private VoucherCheque MapObject(NullHandler oReader)
        {
            VoucherCheque oVoucherCheque = new VoucherCheque();
            oVoucherCheque.VoucherChequeID = oReader.GetInt32("VoucherChequeID");
            oVoucherCheque.VoucherDetailID = oReader.GetInt32("VoucherDetailID");
            oVoucherCheque.ChequeType = (EnumChequeType)oReader.GetInt16("ChequeType");            
            oVoucherCheque.ChequeID = oReader.GetInt32("ChequeID");
            oVoucherCheque.TransactionDate = oReader.GetDateTime("TransactionDate");
            oVoucherCheque.Amount = oReader.GetDouble("Amount");
            oVoucherCheque.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oVoucherCheque.VoucherID = oReader.GetInt32("VoucherID");            
            oVoucherCheque.CCTID = oReader.GetInt32("CCTID");
            oVoucherCheque.AccountHeadName = oReader.GetString("AccountHeadName");
            oVoucherCheque.AccountCode = oReader.GetString("AccountCode");
            oVoucherCheque.ChequeDate = oReader.GetDateTime("ChequeDate");
            oVoucherCheque.ChequeNo = (oVoucherCheque.ChequeType == EnumChequeType.Cash || oVoucherCheque.ChequeType == EnumChequeType.Transfer) ? oVoucherCheque.ChequeType.ToString() : oReader.GetString("ChequeNo");
            oVoucherCheque.BankName = (oVoucherCheque.ChequeType == EnumChequeType.Cash || oVoucherCheque.ChequeType == EnumChequeType.Transfer) ? "Bank" : oReader.GetString("BankName");
            oVoucherCheque.BranchName = (oVoucherCheque.ChequeType == EnumChequeType.Cash || oVoucherCheque.ChequeType == EnumChequeType.Transfer) ? "Branch" : oReader.GetString("BranchName");
            oVoucherCheque.AccountNo = (oVoucherCheque.ChequeType == EnumChequeType.Cash || oVoucherCheque.ChequeType == EnumChequeType.Transfer) ? "Account No" : oReader.GetString("AccountNo");
            oVoucherCheque.CCID = oReader.GetInt32("CCID");
            oVoucherCheque.CostCenterCode = (oVoucherCheque.ChequeType == EnumChequeType.Cash || oVoucherCheque.ChequeType == EnumChequeType.Transfer) ? "Cost Center Code" : oReader.GetString("CostCenterCode");
            oVoucherCheque.CostCenterName = (oVoucherCheque.ChequeType == EnumChequeType.Cash || oVoucherCheque.ChequeType == EnumChequeType.Transfer) ? "Cost Center Name" : oReader.GetString("CostCenterName");
            return oVoucherCheque;
        }

        private VoucherCheque CreateObject(NullHandler oReader)
        {
            VoucherCheque oVoucherCheque = new VoucherCheque();
            oVoucherCheque = MapObject(oReader);
            return oVoucherCheque;
        }

        private List<VoucherCheque> CreateObjects(IDataReader oReader)
        {
            List<VoucherCheque> oVoucherCheque = new List<VoucherCheque>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                VoucherCheque oItem = CreateObject(oHandler);
                oVoucherCheque.Add(oItem);
            }
            return oVoucherCheque;
        }


        #endregion

        #region Interface implementation
        public VoucherChequeService() { }

        public VoucherCheque Save(VoucherCheque oVoucherCheque, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oVoucherCheque.VoucherChequeID <= 0)
                {
                    reader = VoucherChequeDA.InsertUpdate(tc, oVoucherCheque, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = VoucherChequeDA.InsertUpdate(tc, oVoucherCheque, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVoucherCheque = new VoucherCheque();
                    oVoucherCheque = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save VoucherCheque. Because of " + e.Message, e);
                #endregion
            }
            return oVoucherCheque;
        }
        public bool Delete(int id, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                VoucherCheque oVoucherCheque = new VoucherCheque();
                oVoucherCheque.VoucherChequeID = id;
                VoucherChequeDA.Delete(tc, oVoucherCheque, EnumDBOperation.Delete, nUserID);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Delete VoucherCheque. Because of " + e.Message, e);
                #endregion
            }
            return true;
        }

        public VoucherCheque Get(int id, int nUserID)
        {
            VoucherCheque oAccountHead = new VoucherCheque();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = VoucherChequeDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get VoucherCheque", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<VoucherCheque> Gets(int nUserID)
        {
            List<VoucherCheque> oVoucherCheques = new List<VoucherCheque>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VoucherChequeDA.Gets(tc);
                oVoucherCheques = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VoucherCheque", e);
                #endregion
            }

            return oVoucherCheques;
        }

        public List<VoucherCheque> Gets(int nVoucherDetailID, int nUserID)
        {
            List<VoucherCheque> oVoucherCheques = new List<VoucherCheque>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VoucherChequeDA.Gets(tc, nVoucherDetailID);
                oVoucherCheques = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VoucherCheque", e);
                #endregion
            }

            return oVoucherCheques;
        }
        public List<VoucherCheque> GetsBy(int nVoucherID, int nUserID)
        {
            List<VoucherCheque> oVoucherCheques = new List<VoucherCheque>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VoucherChequeDA.GetsBy(tc, nVoucherID);
                oVoucherCheques = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VoucherCheque", e);
                #endregion
            }

            return oVoucherCheques;
        }

        public List<VoucherCheque> Gets(string sSQL, int nUserId)
        {
            List<VoucherCheque> oVoucherCheques = new List<VoucherCheque>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VoucherChequeDA.Gets(tc, sSQL);
                oVoucherCheques = CreateObjects(reader);
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

            return oVoucherCheques;
        }
        #endregion
    }
}
