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
    [Serializable]
    public class OperationCategorySetupService : MarshalByRefObject, IOperationCategorySetupService
    {
        #region Private functions and declaration
        private OperationCategorySetup MapObject(NullHandler oReader)
        {
            OperationCategorySetup oOperationCategorySetup = new OperationCategorySetup();
            oOperationCategorySetup.OperationCategorySetupID = oReader.GetInt32("OperationCategorySetupID");
            oOperationCategorySetup.StatementSetupID = oReader.GetInt32("StatementSetupID");
            oOperationCategorySetup.CategorySetupName = oReader.GetString("CategorySetupName");
            oOperationCategorySetup.DebitCredit = (EnumDebitCredit)oReader.GetInt16("DebitCredit");
            oOperationCategorySetup.Note = oReader.GetString("Note");
            oOperationCategorySetup.CounLedgerGroup = oReader.GetInt32("CounLedgerGroup");
            return oOperationCategorySetup;
        }
        private OperationCategorySetup CreateObject(NullHandler oReader)
        {
            OperationCategorySetup oChallanDetail = new OperationCategorySetup();
            oChallanDetail = MapObject(oReader);
            return oChallanDetail;
        }

        private List<OperationCategorySetup> CreateObjects(IDataReader oReader)
        {
            List<OperationCategorySetup> oOperationCategorySetups = new List<OperationCategorySetup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                OperationCategorySetup oItem = CreateObject(oHandler);
                oOperationCategorySetups.Add(oItem);
            }
            return oOperationCategorySetups;
        }
        #endregion

        #region Interface implementation
        public OperationCategorySetup Save(OperationCategorySetup oOperationCategorySetup, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oOperationCategorySetup.OperationCategorySetupID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.OperationCategorySetup, EnumRoleOperationType.Add);
                    reader = OperationCategorySetupDA.InsertUpdate(tc, oOperationCategorySetup, EnumDBOperation.Insert, nUserID, "");
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.OperationCategorySetup, EnumRoleOperationType.Edit);
                    reader = OperationCategorySetupDA.InsertUpdate(tc, oOperationCategorySetup, EnumDBOperation.Update, nUserID, "");
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oOperationCategorySetup = new OperationCategorySetup();
                    oOperationCategorySetup = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save OperationCategorySetup. Because of " + e.Message, e);
                #endregion
            }
            return oOperationCategorySetup;
        }
        public string Delete(int id, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                OperationCategorySetup oOperationCategorySetup = new OperationCategorySetup();
                oOperationCategorySetup.OperationCategorySetupID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.OperationCategorySetup, EnumRoleOperationType.Delete);
                OperationCategorySetupDA.Delete(tc, oOperationCategorySetup, EnumDBOperation.Delete, nUserID,"");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];

                #endregion
            }
            return "Data delete successfully";
        }
        public OperationCategorySetup Get(int nID, int nUserID)
        {
            OperationCategorySetup oOperationCategorySetup = new OperationCategorySetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = OperationCategorySetupDA.Get(tc, nID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oOperationCategorySetup = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get OperationCategorySetup", e);
                #endregion
            }
            return oOperationCategorySetup;
        }

        public List<OperationCategorySetup> Gets(int nStatementSetupID, int nUserID)
        {
            List<OperationCategorySetup> oOperationCategorySetups = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = OperationCategorySetupDA.Gets(tc, nStatementSetupID);
                oOperationCategorySetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get OperationCategorySetups", e);
                #endregion
            }

            return oOperationCategorySetups;
        }
        #endregion
    }
}
