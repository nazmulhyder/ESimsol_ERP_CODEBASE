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
    public class BankPersonnelService : MarshalByRefObject, IBankPersonnelService
    {
        #region Private functions and declaration
        private BankPersonnel MapObject(NullHandler oReader)
        {
            BankPersonnel oBankPersonnel = new BankPersonnel();
            oBankPersonnel.BankPersonnelID = oReader.GetInt32("BankPersonnelID");
            oBankPersonnel.BankID = oReader.GetInt32("BankID");
            oBankPersonnel.BankBranchID = oReader.GetInt32("BankBranchID");
            oBankPersonnel.Name = oReader.GetString("Name");
            oBankPersonnel.Address = oReader.GetString("Address");
            oBankPersonnel.Phone = oReader.GetString("Phone");
            oBankPersonnel.Email = oReader.GetString("Email");
            oBankPersonnel.Note = oReader.GetString("Note");
            return oBankPersonnel;
        }

        private BankPersonnel CreateObject(NullHandler oReader)
        {
            BankPersonnel oBankPersonnel = new BankPersonnel();
            oBankPersonnel = MapObject(oReader);
            return oBankPersonnel;
        }

        private List<BankPersonnel> CreateObjects(IDataReader oReader)
        {
            List<BankPersonnel> oBankPersonnel = new List<BankPersonnel>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                BankPersonnel oItem = CreateObject(oHandler);
                oBankPersonnel.Add(oItem);
            }
            return oBankPersonnel;
        }

        #endregion

        #region Interface implementation
        public BankPersonnelService() { }

        public BankPersonnel Save(BankPersonnel oBankPersonnel, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oBankPersonnel.BankPersonnelID <= 0)
                {
                    reader = BankPersonnelDA.InsertUpdate(tc, oBankPersonnel, EnumDBOperation.Insert,nUserId);
                }
                else
                {
                    reader = BankPersonnelDA.InsertUpdate(tc, oBankPersonnel, EnumDBOperation.Update,nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBankPersonnel = new BankPersonnel();
                    oBankPersonnel = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save BankPersonnel. Because of " + e.Message, e);
                #endregion
            }
            return oBankPersonnel;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                BankPersonnel oBankPersonnel = new BankPersonnel();
                oBankPersonnel.BankPersonnelID = id;
                BankPersonnelDA.Delete(tc, oBankPersonnel, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete BankPersonnel. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public BankPersonnel Get(int id, Int64 nUserId)
        {
            BankPersonnel oAccountHead = new BankPersonnel();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = BankPersonnelDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get BankPersonnel", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<BankPersonnel> Gets(Int64 nUserId)
        {
            List<BankPersonnel> oBankPersonnel = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BankPersonnelDA.Gets(tc);
                oBankPersonnel = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BankPersonnel", e);
                #endregion
            }

            return oBankPersonnel;
        }

        public List<BankPersonnel> GetsByBank(int nBankID, Int64 nUserId)
        {
            List<BankPersonnel> oBankPersonnel = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BankPersonnelDA.GetsByBank(tc, nBankID);
                oBankPersonnel = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BankPersonnel", e);
                #endregion
            }

            return oBankPersonnel;
        }

        public List<BankPersonnel> GetsByBankBranch(int nBankBranchID, Int64 nUserId)
        {
            List<BankPersonnel> oBankPersonnel = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BankPersonnelDA.GetsByBankBranch(tc, nBankBranchID);
                oBankPersonnel = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BankPersonnel", e);
                #endregion
            }

            return oBankPersonnel;
        }
        #endregion
    }
}
