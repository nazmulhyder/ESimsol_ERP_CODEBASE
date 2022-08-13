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
    public class BankBranchBUService : MarshalByRefObject, IBankBranchBUService
    {
        #region Private functions and declaration

        private BankBranchBU MapObject(NullHandler oReader)
        {
            BankBranchBU oBankBranchBU = new BankBranchBU();
            oBankBranchBU.BankBranchBUID = oReader.GetInt32("BankBranchBUID");
            oBankBranchBU.BUID = oReader.GetInt32("BUID");
            oBankBranchBU.BankBranchID = oReader.GetInt32("BankBranchID");
            oBankBranchBU.BUName = oReader.GetString("BUName");
            oBankBranchBU.BUShortName = oReader.GetString("BUShortName");
            oBankBranchBU.BranchCode = oReader.GetString("BranchCode");
            oBankBranchBU.BranchName = oReader.GetString("BranchName");
            return oBankBranchBU;
        }

        private BankBranchBU CreateObject(NullHandler oReader)
        {
            BankBranchBU oBankBranchBU = new BankBranchBU();
            oBankBranchBU = MapObject(oReader);
            return oBankBranchBU;
        }

        private List<BankBranchBU> CreateObjects(IDataReader oReader)
        {
            List<BankBranchBU> oBankBranchBU = new List<BankBranchBU>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                BankBranchBU oItem = CreateObject(oHandler);
                oBankBranchBU.Add(oItem);
            }
            return oBankBranchBU;
        }

        #endregion

        #region Interface implementation
        public BankBranchBU Save(BankBranchBU oBankBranchBU, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oBankBranchBU.BankBranchBUID <= 0)
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "BankBranchBU", EnumRoleOperationType.Add);
                    reader = BankBranchBUDA.InsertUpdate(tc, oBankBranchBU, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "BankBranchBU", EnumRoleOperationType.Edit);
                    reader = BankBranchBUDA.InsertUpdate(tc, oBankBranchBU, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBankBranchBU = new BankBranchBU();
                    oBankBranchBU = CreateObject(oReader);
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
                    oBankBranchBU = new BankBranchBU();
                    oBankBranchBU.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oBankBranchBU;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                BankBranchBU oBankBranchBU = new BankBranchBU();
                oBankBranchBU.BankBranchBUID = id;
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "BankBranchBU", EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "BankBranchBU", id);
                BankBranchBUDA.Delete(tc, oBankBranchBU, EnumDBOperation.Delete, nUserId);
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

        public BankBranchBU Get(int id, Int64 nUserId)
        {
            BankBranchBU oBankBranchBU = new BankBranchBU();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = BankBranchBUDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBankBranchBU = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get BankBranchBU", e);
                #endregion
            }
            return oBankBranchBU;
        }

        public List<BankBranchBU> Gets(int nID, Int64 nUserID)
        {
            List<BankBranchBU> oBankBranchBUs = new List<BankBranchBU>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = BankBranchBUDA.Gets(tc, nID);
                oBankBranchBUs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                BankBranchBU oBankBranchBU = new BankBranchBU();
                oBankBranchBU.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oBankBranchBUs;
        }

        public List<BankBranchBU> Gets(string sSQL, Int64 nUserID)
        {
            List<BankBranchBU> oBankBranchBUs = new List<BankBranchBU>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = BankBranchBUDA.Gets(tc, sSQL);
                oBankBranchBUs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BankBranchBU", e);
                #endregion
            }
            return oBankBranchBUs;
        }

        #endregion
    }
    
  
}
