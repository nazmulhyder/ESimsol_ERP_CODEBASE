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
    public class ContractorAddressService : MarshalByRefObject, IContractorAddressService
    {
        #region Private functions and declaration
        private ContractorAddress MapObject(NullHandler oReader)
        {
            ContractorAddress oContractorPersonal = new ContractorAddress();
            oContractorPersonal.ContractorAddressID = oReader.GetInt32("ContractorAddressID");
            oContractorPersonal.ContractorID = oReader.GetInt32("ContractorID");
            oContractorPersonal.AddressType = (EnumAddressType)oReader.GetInt16("AddressType");
            oContractorPersonal.AddressTypeInt = oReader.GetInt16("AddressType");
            oContractorPersonal.Address = oReader.GetString("Address");
            oContractorPersonal.Note = oReader.GetString("Note");
          //  oContractorPersonal.ContractorName = oReader.GetString("ContractorName");
            return oContractorPersonal;
        }

        private ContractorAddress CreateObject(NullHandler oReader)
        {
            ContractorAddress oContractorPersonal = new ContractorAddress();
            oContractorPersonal = MapObject(oReader);
            return oContractorPersonal;
        }

        private List<ContractorAddress> CreateObjects(IDataReader oReader)
        {
            List<ContractorAddress> oContractorPersonal = new List<ContractorAddress>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ContractorAddress oItem = CreateObject(oHandler);
                oContractorPersonal.Add(oItem);
            }
            return oContractorPersonal;
        }

        #endregion

        #region Interface implementation
        public ContractorAddressService() { }

        public ContractorAddress Save(ContractorAddress oContractorPersonal, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oContractorPersonal.ContractorAddressID <= 0)
                {
                    reader = ContractorAddressDA.InsertUpdate(tc, oContractorPersonal, EnumDBOperation.Insert,nUserId);
                }
                else
                {
                    reader = ContractorAddressDA.InsertUpdate(tc, oContractorPersonal, EnumDBOperation.Update,nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oContractorPersonal = new ContractorAddress();
                    oContractorPersonal = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save ContractorAddress. Because of " + e.Message.Split('~')[0], e);
                #endregion
            }
            return oContractorPersonal;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ContractorAddress oContractorPersonal = new ContractorAddress();
                oContractorPersonal.ContractorAddressID = id;
                ContractorAddressDA.Delete(tc, oContractorPersonal, EnumDBOperation.Delete,nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete ContractorPersonal. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public ContractorAddress Get(int id, Int64 nUserId)
        {
            ContractorAddress oAccountHead = new ContractorAddress();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ContractorAddressDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get ContractorPersonal", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<ContractorAddress> Gets(Int64 nUserId)
        {
            List<ContractorAddress> oContractorPersonal = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ContractorAddressDA.Gets(tc);
                oContractorPersonal = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ContractorPersonal", e);
                #endregion
            }

            return oContractorPersonal;
        }

        public List<ContractorAddress> Gets(string sSQL, Int64 nUserId)
        {
            List<ContractorAddress> oContractorPersonal = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ContractorAddressDA.Gets(tc, sSQL);
                oContractorPersonal = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ContractorPersonal", e);
                #endregion
            }

            return oContractorPersonal;
        }

        public List<ContractorAddress> GetsByContractor(int nContractorID, Int64 nUserId)
        {
            List<ContractorAddress> oContractorPersonal = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ContractorAddressDA.GetsByContractor(tc, nContractorID);
                oContractorPersonal = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ContractorPersonal", e);
                #endregion
            }

            return oContractorPersonal;
        }

        public List<ContractorAddress> GetsBy(int nContractorID, string sAddtessType, Int64 nUserId)
        {
            List<ContractorAddress> oContractorPersonal = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ContractorAddressDA.GetsBy(tc, nContractorID, sAddtessType);
                oContractorPersonal = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ContractorPersonal", e);
                #endregion
            }

            return oContractorPersonal;
        }
      

        #endregion
    }
}
