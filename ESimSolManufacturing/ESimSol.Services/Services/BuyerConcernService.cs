using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
 

namespace ESimSol.Services.Services
{
    public class BuyerConcernService : MarshalByRefObject, IBuyerConcernService
    {
        #region Private functions and declaration
        private BuyerConcern MapObject(NullHandler oReader)
        {
            BuyerConcern oBuyerConcern = new BuyerConcern();
            oBuyerConcern.BuyerConcernID = oReader.GetInt32("BuyerConcernID");
            oBuyerConcern.BuyerID = oReader.GetInt32("BuyerID");
            oBuyerConcern.ConcernName = oReader.GetString("ConcernName");
            oBuyerConcern.ConcernEmail = oReader.GetString("ConcernEmail");
            oBuyerConcern.ConcernAddress = oReader.GetString("ConcernAddress");
            oBuyerConcern.Note = oReader.GetString("Note");
            return oBuyerConcern;
        }

        private BuyerConcern CreateObject(NullHandler oReader)
        {
            BuyerConcern oBuyerConcern = new BuyerConcern();
            oBuyerConcern = MapObject(oReader);
            return oBuyerConcern;
        }

        private List<BuyerConcern> CreateObjects(IDataReader oReader)
        {
            List<BuyerConcern> oBuyerConcern = new List<BuyerConcern>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                BuyerConcern oItem = CreateObject(oHandler);
                oBuyerConcern.Add(oItem);
            }
            return oBuyerConcern;
        }

        #endregion

        #region Interface implementation
        public BuyerConcernService() { }

        public BuyerConcern Save(BuyerConcern oBuyerConcern, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oBuyerConcern.BuyerConcernID <= 0)
                {
                    reader = BuyerConcernDA.InsertUpdate(tc, oBuyerConcern, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = BuyerConcernDA.InsertUpdate(tc, oBuyerConcern, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBuyerConcern = new BuyerConcern();
                    oBuyerConcern = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save BuyerConcern. Because of " + e.Message, e);
                #endregion
            }
            return oBuyerConcern;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                BuyerConcern oBuyerConcern = new BuyerConcern();
                oBuyerConcern.BuyerConcernID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.BuyerConcern, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "BuyerConcern", id);
                BuyerConcernDA.Delete(tc, oBuyerConcern, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete BuyerConcern. Because of " + e.Message, e);
                #endregion
            }
            return "deleted";
        }

        public BuyerConcern Get(int id, Int64 nUserId)
        {
            BuyerConcern oAccountHead = new BuyerConcern();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = BuyerConcernDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get BuyerConcern", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<BuyerConcern> Gets(Int64 nUserID)
        {
            List<BuyerConcern> oBuyerConcern = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BuyerConcernDA.Gets(tc);
                oBuyerConcern = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BuyerConcern", e);
                #endregion
            }

            return oBuyerConcern;
        }

        public List<BuyerConcern> GetsByContractor(int nContractorID, Int64 nUserID)
        {
            List<BuyerConcern> oBuyerConcerns = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BuyerConcernDA.GetsByContractor(tc, nContractorID);
                oBuyerConcerns = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BuyerConcern", e);
                #endregion
            }

            return oBuyerConcerns;
        }
        #endregion
    }
}
