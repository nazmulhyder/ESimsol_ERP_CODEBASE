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
    public class TAPExecutionService : MarshalByRefObject, ITAPExecutionService
    {
        #region Private functions and declaration
        private TAPExecution MapObject(NullHandler oReader)
        {
            TAPExecution oTAPExecution = new TAPExecution();
            oTAPExecution.TAPExecutionID = oReader.GetInt32("TAPExecutionID");
            oTAPExecution.TAPDetailID = oReader.GetInt32("TAPDetailID");
            oTAPExecution.TAPID = oReader.GetInt32("TAPID");
            oTAPExecution.OrderStepID = oReader.GetInt32("OrderStepID");
            oTAPExecution.RequiredDataType = (EnumRequiredDataType)oReader.GetInt32("RequiredDataType");
            oTAPExecution.RequiredDataTypeInInt = oReader.GetInt32("RequiredDataType");
            oTAPExecution.TAPDetailSequence = oReader.GetInt32("TAPDetailSequence");
            oTAPExecution.OrderStepParentID = oReader.GetInt32("OrderStepParentID");
            oTAPExecution.UpdatedData = oReader.GetString("UpdatedData");
            oTAPExecution.UpdateBy = oReader.GetInt32("UpdateBy");
            oTAPExecution.UpdatedByName = oReader.GetString("UpdatedByName");
            oTAPExecution.OrderStepName = oReader.GetString("OrderStepName");
            oTAPExecution.ApprovalPlanDate = oReader.GetDateTime("ApprovalPlanDate");
            oTAPExecution.IsDone = oReader.GetBoolean("IsDone");
            oTAPExecution.DoneDate = oReader.GetDateTime("DoneDate");
            return oTAPExecution;
        }

        private TAPExecution CreateObject(NullHandler oReader)
        {
            TAPExecution oTAPExecution = new TAPExecution();
            oTAPExecution = MapObject(oReader);
            return oTAPExecution;
        }

        private List<TAPExecution> CreateObjects(IDataReader oReader)
        {
            List<TAPExecution> oTAPExecution = new List<TAPExecution>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                TAPExecution oItem = CreateObject(oHandler);
                oTAPExecution.Add(oItem);
            }
            return oTAPExecution;
        }

        #endregion

        #region Interface implementation
     
        public List<TAPExecution> Save(TAPExecution oTAPExecution, Int64 nUserID)
        {
            
            TransactionContext tc = null;
            List<TAPExecution> oTAPExecutions = new List<TAPExecution>();
            TAPExecution oTempTAPExecution = new TAPExecution();
            try
            {
                tc = TransactionContext.Begin(true);
                foreach (TAPExecution oItem in oTAPExecution.TAPExecutions)
                {
                    IDataReader reader;
                    if (oItem.TAPExecutionID <= 0)
                    {

                        AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.TAPExecution, EnumRoleOperationType.Add);
                        reader = TAPExecutionDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.TAPExecution, EnumRoleOperationType.Edit);
                        reader = TAPExecutionDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oTempTAPExecution = new TAPExecution();
                        oTempTAPExecution = CreateObject(oReader);
                    }
                    oTAPExecutions.Add(oTempTAPExecution);
                    reader.Close();
                }
               
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oTempTAPExecution = new TAPExecution();
                oTempTAPExecution.ErrorMessage = e.Message.Split('!')[0];
                oTAPExecutions.Add(oTempTAPExecution);
                #endregion
            }
            return oTAPExecutions ;
        }

        public TAPExecution SingleSave(TAPExecution oTAPExecution, Int64 nUserID)
        {
            
            TransactionContext tc = null;
            TAPExecution oTempTAPExecution = new TAPExecution();
            List<TAPExecution> oTAPExecutions = new List<TAPExecution>();
            try
            {
                tc = TransactionContext.Begin(true);

                    IDataReader reader;
                    if (oTAPExecution.TAPExecutionID <= 0)
                    {

                        AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.TAPExecution, EnumRoleOperationType.Add);
                        reader = TAPExecutionDA.InsertUpdate(tc, oTAPExecution, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.TAPExecution, EnumRoleOperationType.Edit);
                        reader = TAPExecutionDA.InsertUpdate(tc, oTAPExecution, EnumDBOperation.Update, nUserID);
                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oTempTAPExecution = new TAPExecution();
                        oTempTAPExecution = CreateObject(oReader);
                    }
                    reader.Close();
                    #region  TAP Execution GET
                    reader = TAPExecutionDA.GetOrderSteps(oTempTAPExecution.TAPID, false, tc);
                    oTempTAPExecution.TAPExecutions = CreateObjects(reader);
                    reader.Close();

                    #endregion


                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oTempTAPExecution = new TAPExecution();
                oTempTAPExecution.ErrorMessage = e.Message.Split('!')[0];
                
                #endregion
            }
            return oTempTAPExecution;
        }

        public TAPExecution Done(TAPExecution oTAPExecution, Int64 nUserID)
        {

            TransactionContext tc = null;
            TAPExecution oTempTAPExecution = new TAPExecution();
            List<TAPExecution> oTAPExecutions = new List<TAPExecution>();
            try
            {
                tc = TransactionContext.Begin(true);

                IDataReader reader;
                NullHandler oReader = null;

                reader = TAPExecutionDA.Done(oTAPExecution, tc, nUserID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTempTAPExecution = new TAPExecution();
                    oTempTAPExecution = CreateObject(oReader);
                }
                reader.Close();
                #region  TAP Execution GET
                reader = TAPExecutionDA.GetOrderSteps(oTempTAPExecution.TAPID, false, tc);
                oTempTAPExecution.TAPExecutions = CreateObjects(reader);
                reader.Close();
                #endregion



                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oTempTAPExecution = new TAPExecution();
                oTempTAPExecution.ErrorMessage = e.Message.Split('!')[0];

                #endregion
            }
            return oTempTAPExecution;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                TAPExecution oTAPExecution = new TAPExecution();
                oTAPExecution.TAPExecutionID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.TAPExecution, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "TAPExecution", id);
                TAPExecutionDA.Delete(tc, oTAPExecution, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete TAPExecution. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public TAPExecution Get(int id, Int64 nUserId)
        {
            TAPExecution oAccountHead = new TAPExecution();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = TAPExecutionDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get TAPExecution", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<TAPExecution> Gets(Int64 nUserID)
        {
            List<TAPExecution> oTAPExecution = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TAPExecutionDA.Gets(tc);
                oTAPExecution = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TAPExecution", e);
                #endregion
            }

            return oTAPExecution;
        }

        public List<TAPExecution> GetOrderSteps(int nTAPID, Int64 nUserID)
        {
            List<TAPExecution> oTAPExecution = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TAPExecutionDA.GetOrderSteps(nTAPID,false,tc);
                oTAPExecution = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TAPExecution", e);
                #endregion
            }

            return oTAPExecution;
        }
        
        public List<TAPExecution> Gets(string sSQL, Int64 nUserID)
        {
            List<TAPExecution> oTAPExecutions = new List<TAPExecution>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TAPExecutionDA.Gets(tc, sSQL);
                oTAPExecutions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TAPExecution", e);
                #endregion
            }

            return oTAPExecutions;
        }


        public List<TAPExecution> GetsByTaPs(TAP oTAP, Int64 nUserID)
        {
            List<TAPExecution> oTAPExecutions = new List<TAPExecution>();
            List<TAPExecution> oNewTAPExecutions = new List<TAPExecution>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                foreach(TAP oItem in oTAP.TAPs)
                {
                    reader = TAPExecutionDA.GetOrderSteps(oItem.TAPID,true, tc);
                    oTAPExecutions = CreateObjects(reader);
                    reader.Close();
                    oNewTAPExecutions.AddRange(oTAPExecutions);
                }
                
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TAPExecution", e);
                #endregion
            }

            return oNewTAPExecutions;
        }

        #endregion
    }   
    
    
}
