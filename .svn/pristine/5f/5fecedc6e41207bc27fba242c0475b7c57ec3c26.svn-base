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
    public class BankBranchDeptService : MarshalByRefObject, IBankBranchDeptService
    {
        #region Private functions and declaration

        private BankBranchDept MapObject(NullHandler oReader)
        {
            BankBranchDept oBankBranchDept = new BankBranchDept();
            oBankBranchDept.BankBranchDeptID = oReader.GetInt32("BankBranchDeptID");
            oBankBranchDept.OperationalDept = (EnumOperationalDept) oReader.GetInt32("OperationalDept");
            oBankBranchDept.OperationalDeptInInt = oReader.GetInt32("OperationalDept");
            oBankBranchDept.BankBranchID = oReader.GetInt32("BankBranchID");
            oBankBranchDept.BranchCode = oReader.GetString("BranchCode");
            oBankBranchDept.BranchName = oReader.GetString("BranchName");
            return oBankBranchDept;
        }

        private BankBranchDept CreateObject(NullHandler oReader)
        {
            BankBranchDept oBankBranchDept = new BankBranchDept();
            oBankBranchDept = MapObject(oReader);
            return oBankBranchDept;
        }

        private List<BankBranchDept> CreateObjects(IDataReader oReader)
        {
            List<BankBranchDept> oBankBranchDept = new List<BankBranchDept>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                BankBranchDept oItem = CreateObject(oHandler);
                oBankBranchDept.Add(oItem);
            }
            return oBankBranchDept;
        }

        #endregion

        #region Interface implementation
        public BankBranchDept Save(BankBranchDept oBankBranchDept, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oBankBranchDept.BankBranchDeptID <= 0)
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "BankBranchDept", EnumRoleOperationType.Add);
                    reader = BankBranchDeptDA.InsertUpdate(tc, oBankBranchDept, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "BankBranchDept", EnumRoleOperationType.Edit);
                    reader = BankBranchDeptDA.InsertUpdate(tc, oBankBranchDept, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBankBranchDept = new BankBranchDept();
                    oBankBranchDept = CreateObject(oReader);
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
                    oBankBranchDept = new BankBranchDept();
                    oBankBranchDept.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oBankBranchDept;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                BankBranchDept oBankBranchDept = new BankBranchDept();
                oBankBranchDept.BankBranchDeptID = id;
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "BankBranchDept", EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "BankBranchDept", id);
                BankBranchDeptDA.Delete(tc, oBankBranchDept, EnumDBOperation.Delete, nUserId);
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

        public BankBranchDept Get(int id, Int64 nUserId)
        {
            BankBranchDept oBankBranchDept = new BankBranchDept();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = BankBranchDeptDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBankBranchDept = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get BankBranchDept", e);
                #endregion
            }
            return oBankBranchDept;
        }

        public List<BankBranchDept> Gets(int nID, Int64 nUserID)
        {
            List<BankBranchDept> oBankBranchDepts = new List<BankBranchDept>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = BankBranchDeptDA.Gets(tc, nID);
                oBankBranchDepts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                BankBranchDept oBankBranchDept = new BankBranchDept();
                oBankBranchDept.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oBankBranchDepts;
        }

        public List<BankBranchDept> Gets(string sSQL, Int64 nUserID)
        {
            List<BankBranchDept> oBankBranchDepts = new List<BankBranchDept>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = BankBranchDeptDA.Gets(tc, sSQL);
                oBankBranchDepts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BankBranchDept", e);
                #endregion
            }
            return oBankBranchDepts;
        }

        #endregion
    }
    
   
}
