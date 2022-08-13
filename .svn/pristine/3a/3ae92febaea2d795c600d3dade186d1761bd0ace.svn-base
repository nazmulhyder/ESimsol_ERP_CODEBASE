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
    public class ContractorTypeService : MarshalByRefObject, IContractorTypeService
    {
        #region Private functions and declaration
        private ContractorType MapObject(NullHandler oReader)
        {
            ContractorType oContractorType = new ContractorType();
            oContractorType.ContractorTypeID = oReader.GetInt32("ContractorType");
            oContractorType.ContractorID = oReader.GetInt32("ContractorID");
         
            return oContractorType;
        }

        private ContractorType CreateObject(NullHandler oReader)
        {
            ContractorType oContractorPersonal = new ContractorType();
            oContractorPersonal = MapObject(oReader);
            return oContractorPersonal;
        }

        private List<ContractorType> CreateObjects(IDataReader oReader)
        {
            List<ContractorType> oContractorPersonal = new List<ContractorType>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ContractorType oItem = CreateObject(oHandler);
                oContractorPersonal.Add(oItem);
            }
            return oContractorPersonal;
        }

        #endregion

        #region Interface implementation
        public ContractorTypeService() { }

        public ContractorType Save(ContractorType oContractorPersonal, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oContractorPersonal.ContractorTypeID <= 0)
                {
                    reader = ContractorTypeDA.InsertUpdate(tc, oContractorPersonal, EnumDBOperation.Insert,nUserId);
                }
                else
                {
                    reader = ContractorTypeDA.InsertUpdate(tc, oContractorPersonal, EnumDBOperation.Update,nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oContractorPersonal = new ContractorType();
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
                throw new ServiceException("Failed to Save ContractorType. Because of " + e.Message.Split('~')[0], e);
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
                ContractorType oContractorPersonal = new ContractorType();
                oContractorPersonal.ContractorTypeID = id;
                ContractorTypeDA.Delete(tc, oContractorPersonal, EnumDBOperation.Delete,nUserId);
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

        public ContractorType Get(int id, Int64 nUserId)
        {
            ContractorType oAccountHead = new ContractorType();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ContractorTypeDA.Get(tc, id);
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



        public List<ContractorType> Gets(string sSQL, Int64 nUserId)
        {
            List<ContractorType> oContractorPersonal = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ContractorTypeDA.Gets(tc, sSQL);
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

        public List<ContractorType> Gets(int nContractorID, Int64 nUserId)
        {
            List<ContractorType> oContractorPersonal = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ContractorTypeDA.GetsByContractor(tc, nContractorID);
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
