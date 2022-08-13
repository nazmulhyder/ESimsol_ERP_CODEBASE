using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;

namespace ESimSol.Services.Services
{
    public class BUWisePartyService : MarshalByRefObject, IBUWisePartyService
    {
        #region Private functions and declaration

        private BUWiseParty MapObject(NullHandler oReader)
        {
            BUWiseParty oBUWiseParty = new BUWiseParty();
            oBUWiseParty.BUWisePartyID = oReader.GetInt32("BUWisePartyID");
            oBUWiseParty.BUID = oReader.GetInt32("BUID");
            oBUWiseParty.ContractorID = oReader.GetInt32("ContractorID");
            oBUWiseParty.BUName = oReader.GetString("BUName");
            oBUWiseParty.ContractorName = oReader.GetString("ContractorName");
            return oBUWiseParty;
        }

        private BUWiseParty CreateObject(NullHandler oReader)
        {
            BUWiseParty oBUWiseParty = new BUWiseParty();
            oBUWiseParty = MapObject(oReader);
            return oBUWiseParty;
        }

        private List<BUWiseParty> CreateObjects(IDataReader oReader)
        {
            List<BUWiseParty> oBUWiseParty = new List<BUWiseParty>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                BUWiseParty oItem = CreateObject(oHandler);
                oBUWiseParty.Add(oItem);
            }
            return oBUWiseParty;
        }

        #endregion

        #region Interface implementation
        public BUWiseParty Save(BUWiseParty oBUWiseParty, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oBUWiseParty.BUWisePartyID <= 0)
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "BUWiseParty", EnumRoleOperationType.Add);
                    reader = BUWisePartyDA.InsertUpdate(tc, oBUWiseParty, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "BUWiseParty", EnumRoleOperationType.Edit);
                    reader = BUWisePartyDA.InsertUpdate(tc, oBUWiseParty, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBUWiseParty = new BUWiseParty();
                    oBUWiseParty = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oBUWiseParty = new BUWiseParty();
                    oBUWiseParty.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oBUWiseParty;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                BUWiseParty oBUWiseParty = new BUWiseParty();
                oBUWiseParty.BUWisePartyID = id;
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "BUWiseParty", EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "BUWiseParty", id);
                BUWisePartyDA.Delete(tc, oBUWiseParty, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exceptionif (tc != null)
                tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return "Data delete successfully";
        }

        public BUWiseParty Get(int id, Int64 nUserId)
        {
            BUWiseParty oBUWiseParty = new BUWiseParty();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = BUWisePartyDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBUWiseParty = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get BUWiseParty", e);
                #endregion
            }
            return oBUWiseParty;
        }

        public List<BUWiseParty> Gets(int nID, Int64 nUserID)
        {
            List<BUWiseParty> oBUWisePartys = new List<BUWiseParty>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = BUWisePartyDA.Gets(tc, nID);
                oBUWisePartys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                BUWiseParty oBUWiseParty = new BUWiseParty();
                oBUWiseParty.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oBUWisePartys;
        }

        public List<BUWiseParty> Gets(string sSQL, Int64 nUserID)
        {
            List<BUWiseParty> oBUWisePartys = new List<BUWiseParty>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = BUWisePartyDA.Gets(tc, sSQL);
                oBUWisePartys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BUWiseParty", e);
                #endregion
            }
            return oBUWisePartys;
        }

        #endregion
    }

}
