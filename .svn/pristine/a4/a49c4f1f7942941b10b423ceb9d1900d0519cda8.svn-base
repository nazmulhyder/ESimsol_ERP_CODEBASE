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

    public class COA_ChartOfAccountCostCenterService : MarshalByRefObject, ICOA_ChartOfAccountCostCenterService
    {
        #region Private functions and declaration
        private COA_ChartOfAccountCostCenter MapObject(NullHandler oReader)
        {
            COA_ChartOfAccountCostCenter oCOA_ChartOfAccountCostCenter = new COA_ChartOfAccountCostCenter();
            oCOA_ChartOfAccountCostCenter.CACCID = oReader.GetInt32("CACCID");
            oCOA_ChartOfAccountCostCenter.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oCOA_ChartOfAccountCostCenter.CCID = oReader.GetInt32("CCID");
            oCOA_ChartOfAccountCostCenter.CCCode = oReader.GetString("CCCode");
            oCOA_ChartOfAccountCostCenter.CCName = oReader.GetString("CCName");
            oCOA_ChartOfAccountCostCenter.COACode = oReader.GetString("AccountCode");
            oCOA_ChartOfAccountCostCenter.COAName = oReader.GetString("AccountHeadName");
            return oCOA_ChartOfAccountCostCenter;
        }

        private COA_ChartOfAccountCostCenter CreateObject(NullHandler oReader)
        {
            COA_ChartOfAccountCostCenter oCOA_ChartOfAccountCostCenter = new COA_ChartOfAccountCostCenter();
            oCOA_ChartOfAccountCostCenter = MapObject(oReader);
            return oCOA_ChartOfAccountCostCenter;
        }

        private List<COA_ChartOfAccountCostCenter> CreateObjects(IDataReader oReader)
        {
            List<COA_ChartOfAccountCostCenter> oCOA_ChartOfAccountCostCenter = new List<COA_ChartOfAccountCostCenter>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                COA_ChartOfAccountCostCenter oItem = CreateObject(oHandler);
                oCOA_ChartOfAccountCostCenter.Add(oItem);
            }
            return oCOA_ChartOfAccountCostCenter;
        }

        #endregion

        #region Interface implementation
        public COA_ChartOfAccountCostCenterService() { }

        public COA_ChartOfAccountCostCenter Save(COA_ChartOfAccountCostCenter oCOA_ChartOfAccountCostCenter, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oCOA_ChartOfAccountCostCenter.CACCID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.COA_ChartOfAccountCostCenter, EnumRoleOperationType.Add);
                    reader = COA_ChartOfAccountCostCenterDA.InsertUpdate(tc, oCOA_ChartOfAccountCostCenter, EnumDBOperation.Insert,nUserId);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.COA_ChartOfAccountCostCenter, EnumRoleOperationType.Edit);
                    reader = COA_ChartOfAccountCostCenterDA.InsertUpdate(tc, oCOA_ChartOfAccountCostCenter, EnumDBOperation.Update,nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCOA_ChartOfAccountCostCenter = new COA_ChartOfAccountCostCenter();
                    oCOA_ChartOfAccountCostCenter = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save COA_ChartOfAccountCostCenter. Because of " + e.Message, e);
                #endregion
            }
            return oCOA_ChartOfAccountCostCenter;
        }
        public bool Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                COA_ChartOfAccountCostCenter oCOA_ChartOfAccountCostCenter = new COA_ChartOfAccountCostCenter();
                oCOA_ChartOfAccountCostCenter.CACCID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.COA_ChartOfAccountCostCenter, EnumRoleOperationType.Delete);
                //COA_ChartOfAccountCostCenterDA.Delete(tc, oCOA_ChartOfAccountCostCenter, EnumDBOperation.Delete);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Delete COA_ChartOfAccountCostCenter. Because of " + e.Message, e);
                #endregion
            }
            return true;
        }

        public COA_ChartOfAccountCostCenter Get(int id, int nUserId)
        {
            COA_ChartOfAccountCostCenter oAccountHead = new COA_ChartOfAccountCostCenter();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = COA_ChartOfAccountCostCenterDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get COA_ChartOfAccountCostCenter", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<COA_ChartOfAccountCostCenter> Gets(int nUserId)
        {
            List<COA_ChartOfAccountCostCenter> oCOA_ChartOfAccountCostCenter = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = COA_ChartOfAccountCostCenterDA.Gets(tc);
                oCOA_ChartOfAccountCostCenter = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get COA_ChartOfAccountCostCenter", e);
                #endregion
            }

            return oCOA_ChartOfAccountCostCenter;
        }

        public List<COA_ChartOfAccountCostCenter> GetsByCostCente(int nCCID, int nUserId)
        {
            List<COA_ChartOfAccountCostCenter> oCOA_ChartOfAccountCostCenter = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = COA_ChartOfAccountCostCenterDA.GetsByCostCente(tc, nCCID);
                oCOA_ChartOfAccountCostCenter = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get COA_ChartOfAccountCostCenter", e);
                #endregion
            }

            return oCOA_ChartOfAccountCostCenter;
        }

        
        public List<COA_ChartOfAccountCostCenter> Gets(string sSQL, int nUserId)
        {
            List<COA_ChartOfAccountCostCenter> oCOA_ChartOfAccountCostCenter = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = COA_ChartOfAccountCostCenterDA.Gets(tc, sSQL);
                oCOA_ChartOfAccountCostCenter = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get COA_ChartOfAccountCostCenter", e);
                #endregion
            }

            return oCOA_ChartOfAccountCostCenter;
        }

        #endregion
    }

    
}
