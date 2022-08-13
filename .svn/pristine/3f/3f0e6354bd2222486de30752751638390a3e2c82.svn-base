using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class BankBranchService : MarshalByRefObject, IBankBranchService
    {
        #region Private functions and declaration
        private BankBranch MapObject(NullHandler oReader)
        {
            BankBranch oBankBranch = new BankBranch();
            oBankBranch.BankBranchID = oReader.GetInt32("BankBranchID");
            oBankBranch.BankID = oReader.GetInt32("BankID");
            oBankBranch.BranchCode = oReader.GetString("BranchCode");
            oBankBranch.BranchName = oReader.GetString("BranchName");
            oBankBranch.Address = oReader.GetString("Address");
            oBankBranch.SwiftCode = oReader.GetString("SwiftCode");
            oBankBranch.PhoneNo = oReader.GetString("PhoneNo");
            oBankBranch.FaxNo = oReader.GetString("FaxNo");
            oBankBranch.Note = oReader.GetString("Note");
            oBankBranch.IsOwnBank = oReader.GetBoolean("IsOwnBank");
            oBankBranch.IsActive = oReader.GetBoolean("IsActive");
            oBankBranch.BankName = oReader.GetString("BankName");
            return oBankBranch;
        }

        private BankBranch CreateObject(NullHandler oReader)
        {
            BankBranch oBankBranch = new BankBranch();
            oBankBranch = MapObject(oReader);
            return oBankBranch;
        }

        private List<BankBranch> CreateObjects(IDataReader oReader)
        {
            List<BankBranch> oBankBranch = new List<BankBranch>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                BankBranch oItem = CreateObject(oHandler);
                oBankBranch.Add(oItem);
            }
            return oBankBranch;
        }

        #endregion

        #region Interface implementation
        public BankBranchService() { }

        public BankBranch Save(BankBranch oBankBranch, Int64 nUserId)
        {
            TransactionContext tc = null;
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            oBusinessUnits = oBankBranch.BusinessUnits;
            List<BankBranchDept> oBankBranchDepts = new List<BankBranchDept>();
            oBankBranchDepts = oBankBranch.BankBranchDepts;
            try
            {
                #region Bank Branch
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oBankBranch.BankBranchID <= 0)
                {
                    reader = BankBranchDA.InsertUpdate(tc, oBankBranch, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = BankBranchDA.InsertUpdate(tc, oBankBranch, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBankBranch = new BankBranch();
                    oBankBranch = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region Business Unit
                if (oBusinessUnits != null)
                {
                    BankBranchBU oBankBranchBU = new BankBranchBU();
                    oBankBranchBU.BankBranchID = oBankBranch.BankBranchID;
                    BankBranchBUDA.Delete(tc, oBankBranchBU, EnumDBOperation.Delete, nUserId);
                    foreach (BusinessUnit oItem in oBusinessUnits)
                    {
                        IDataReader readerBankBranchBU;
                        oBankBranchBU = new BankBranchBU();
                        oBankBranchBU.BankBranchID = oBankBranch.BankBranchID;
                        oBankBranchBU.BUID = oItem.BusinessUnitID;
                        readerBankBranchBU = BankBranchBUDA.InsertUpdate(tc, oBankBranchBU, EnumDBOperation.Insert, nUserId);
                        NullHandler oReaderTNC = new NullHandler(readerBankBranchBU);
                        readerBankBranchBU.Close();
                    }
                }
                #endregion
                #region Operational Dept
                if (oBankBranchDepts != null)
                {
                    BankBranchDept oBankBranchDept = new BankBranchDept();
                    oBankBranchDept.BankBranchID = oBankBranch.BankBranchID;
                    BankBranchDeptDA.Delete(tc, oBankBranchDept, EnumDBOperation.Delete, nUserId);
                    foreach (BankBranchDept oItem in oBankBranchDepts)
                    {
                        IDataReader readerBankBranchDept;
                        oItem.BankBranchID = oBankBranch.BankBranchID;
                        readerBankBranchDept = BankBranchDeptDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserId);
                        NullHandler oReaderTNC = new NullHandler(readerBankBranchDept);
                        readerBankBranchDept.Close();
                    }
                }
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oBankBranch.ErrorMessage = e.Message;
                oBankBranch.BankBranchID = 0;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save Product. Because of " + e.Message, e);
                #endregion
            }
            return oBankBranch;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                BankBranch oBankBranch = new BankBranch();
                oBankBranch.BankBranchID = id;
                BankBranchDA.Delete(tc, oBankBranch, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Delete Product. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public BankBranch Get(int id, Int64 nUserId)
        {
            BankBranch oBankBranch = new BankBranch();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = BankBranchDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBankBranch = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get Product", e);
                #endregion
            }

            return oBankBranch;
        }
        
        public List<BankBranch> GetsByBank(int nBankID, Int64 nUserId)
        {
            List<BankBranch> oBankBranch = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BankBranchDA.GetsByBank(tc, nBankID);
                oBankBranch = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Product", e);
                #endregion
            }

            return oBankBranch;
        }
        
        public List<BankBranch> GetsByDeptAndBU(string sDeptIDs, int BUID, string sBankName, Int64 nUserId)
        {
            List<BankBranch> oBankBranch = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BankBranchDA.GetsByDeptAndBU(tc, sDeptIDs, BUID, sBankName);
                oBankBranch = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Product", e);
                #endregion
            }

            return oBankBranch;
        }
        public List<BankBranch> Gets(Int64 nUserId)
        {
            List<BankBranch> oBankBranch = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BankBranchDA.Gets(tc);
                oBankBranch = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Product", e);
                #endregion
            }

            return oBankBranch;
        }
        public List<BankBranch> Gets(string sSQL,Int64 nUserId)
        {
            List<BankBranch> oBankBranch = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BankBranchDA.Gets(tc, sSQL);
                oBankBranch = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Product", e);
                #endregion
            }

            return oBankBranch;
        }

        public List<BankBranch> GetsOwnBranchs(Int64 nUserId)
        {
            List<BankBranch> oBankBranch = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BankBranchDA.GetsOwnBranchs(tc);
                oBankBranch = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Product", e);
                #endregion
            }

            return oBankBranch;
        }
        #endregion
    }
}
