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
    public class PartyWiseBankService : MarshalByRefObject, IPartyWiseBankService
    {
        #region Private functions and declaration
        private PartyWiseBank MapObject(NullHandler oReader)
        {
            PartyWiseBank oPartyWiseBank = new PartyWiseBank();
            oPartyWiseBank.PartyWiseBankID = oReader.GetInt32("PartyWiseBankID");
            oPartyWiseBank.BankID = oReader.GetInt32("BankID");
            oPartyWiseBank.BankName = oReader.GetString("BankName");
            oPartyWiseBank.BranchName = oReader.GetString("BranchName");
            oPartyWiseBank.ContractorID = oReader.GetInt32("ContractorID");
            oPartyWiseBank.AccountName = oReader.GetString("AccountName");
            oPartyWiseBank.AccountNo = oReader.GetString("AccountNo");
            oPartyWiseBank.ContractorName = oReader.GetString("ContractorName");
            return oPartyWiseBank;
        }

        private PartyWiseBank CreateObject(NullHandler oReader)
        {
            PartyWiseBank oPartyWiseBank = new PartyWiseBank();
            oPartyWiseBank = MapObject(oReader);
            return oPartyWiseBank;
        }

        private List<PartyWiseBank> CreateObjects(IDataReader oReader)
        {
            List<PartyWiseBank> oPartyWiseBank = new List<PartyWiseBank>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PartyWiseBank oItem = CreateObject(oHandler);
                oPartyWiseBank.Add(oItem);
            }
            return oPartyWiseBank;
        }

        #endregion

        #region Interface implementation
        public PartyWiseBankService() { }

        public PartyWiseBank Save(PartyWiseBank oPartyWiseBank, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oPartyWiseBank.PartyWiseBankID <= 0)
                {
                    reader = PartyWiseBankDA.InsertUpdate(tc, oPartyWiseBank, EnumDBOperation.Insert,nUserId);
                }
                else
                {
                    reader = PartyWiseBankDA.InsertUpdate(tc, oPartyWiseBank, EnumDBOperation.Update,nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPartyWiseBank = new PartyWiseBank();
                    oPartyWiseBank = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save PartyWiseBank. Because of " + e.Message.Split('~')[0], e);
                #endregion
            }
            return oPartyWiseBank;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                PartyWiseBank oPartyWiseBank = new PartyWiseBank();
                oPartyWiseBank.PartyWiseBankID = id;
                PartyWiseBankDA.Delete(tc, oPartyWiseBank, EnumDBOperation.Delete,nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete PartyWiseBank. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public PartyWiseBank Get(int id, Int64 nUserId)
        {
            PartyWiseBank oAccountHead = new PartyWiseBank();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PartyWiseBankDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get PartyWiseBank", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<PartyWiseBank> Gets(Int64 nUserId)
        {
            List<PartyWiseBank> oPartyWiseBank = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PartyWiseBankDA.Gets(tc);
                oPartyWiseBank = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PartyWiseBank", e);
                #endregion
            }

            return oPartyWiseBank;
        }

        public List<PartyWiseBank> Gets(string sSQL, Int64 nUserId)
        {
            List<PartyWiseBank> oPartyWiseBank = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PartyWiseBankDA.Gets(tc, sSQL);
                oPartyWiseBank = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PartyWiseBank", e);
                #endregion
            }

            return oPartyWiseBank;
        }

        public List<PartyWiseBank> GetsByContractor(int nContractorID, Int64 nUserId)
        {
            List<PartyWiseBank> oPartyWiseBank = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PartyWiseBankDA.GetsByContractor(tc, nContractorID);
                oPartyWiseBank = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PartyWiseBank", e);
                #endregion
            }

            return oPartyWiseBank;
        }

        #endregion
    }
}
