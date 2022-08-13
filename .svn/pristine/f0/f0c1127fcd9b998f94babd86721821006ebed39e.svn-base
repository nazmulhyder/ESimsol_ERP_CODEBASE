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


    public class OrderStepService : MarshalByRefObject, IOrderStepService
    {
        #region Private functions and declaration
        private OrderStep MapObject(NullHandler oReader)
        {
            OrderStep oOrderStep = new OrderStep();
            oOrderStep.OrderStepID = oReader.GetInt32("OrderStepID");
            oOrderStep.SubStepName = oReader.GetString("SubStepName");
            oOrderStep.StyleType =  (EnumTSType)oReader.GetInt32("StyleType");
            oOrderStep.TnAStep = (EnumTnAStep)oReader.GetInt32("TnAStep");
            oOrderStep.StepType = (EnumStepType)oReader.GetInt32("StepType");
            oOrderStep.OrderStepName = oReader.GetString("OrderStepName");
            oOrderStep.RequiredDataType = (EnumRequiredDataType)oReader.GetInt32("RequiredDataType");
            oOrderStep.RequiredDataTypeInInt = oReader.GetInt32("RequiredDataType");
            oOrderStep.Sequence = oReader.GetInt32("Sequence");
            oOrderStep.IsNotificationSend = oReader.GetBoolean("IsNotificationSend");
            oOrderStep.IsActive = oReader.GetBoolean("IsActive");
            oOrderStep.Note = oReader.GetString("Note");
            return oOrderStep;
        }

        private OrderStep CreateObject(NullHandler oReader)
        {
            OrderStep oOrderStep = new OrderStep();
            oOrderStep = MapObject(oReader);
            return oOrderStep;
        }

        private List<OrderStep> CreateObjects(IDataReader oReader)
        {
            List<OrderStep> oOrderStep = new List<OrderStep>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                OrderStep oItem = CreateObject(oHandler);
                oOrderStep.Add(oItem);
            }
            return oOrderStep;
        }

        #endregion

        #region Interface implementation
        public OrderStepService() { }

        public OrderStep Save(OrderStep oOrderStep, Int64 nUserID)
        {
         
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oOrderStep.OrderStepID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.OrderStep, EnumRoleOperationType.Add);
                    reader = OrderStepDA.InsertUpdate(tc, oOrderStep, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.OrderStep, EnumRoleOperationType.Edit);
                    reader = OrderStepDA.InsertUpdate(tc, oOrderStep, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oOrderStep = new OrderStep();
                    oOrderStep = CreateObject(oReader);
                }
                 reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oOrderStep = new OrderStep();
                oOrderStep.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oOrderStep;
        }
        public OrderStep ActiveInActive(OrderStep oOrderStep, Int64 nUserID)
        {            
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.OrderStep, EnumRoleOperationType.Edit);
                reader = OrderStepDA.InsertUpdate(tc, oOrderStep, EnumDBOperation.Update, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oOrderStep = new OrderStep();
                    oOrderStep = CreateObject(oReader);
                }
                 reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oOrderStep = new OrderStep();
                oOrderStep.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oOrderStep;
        }

        public OrderStep RefreshStepSequence(OrderStep oOrderStep, Int64 nUserID)
        {

            TransactionContext tc = null;
            List<OrderStep> oOrderSteps = new List<OrderStep>();
            oOrderSteps = oOrderStep.OrderSteps;
            try
            {
                tc = TransactionContext.Begin(true);
                foreach(OrderStep oItem in oOrderSteps)
                {
                    OrderStepDA.UpdateSequence(tc, oItem);
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oOrderStep = new OrderStep();
                oOrderStep.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oOrderStep;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                OrderStep oOrderStep = new OrderStep();
                oOrderStep.OrderStepID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.OrderStep, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, EnumModuleName.OrderStep.ToString(), id);
                OrderStepDA.Delete(tc, oOrderStep, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return "Data delete successfully";
        }

        public OrderStep Get(int id, Int64 nUserId)
        {
            OrderStep oAccountHead = new OrderStep();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = OrderStepDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get OrderStep", e);
                #endregion
            }

            return oAccountHead;
        }
        public List<OrderStep> Gets(Int64 nUserID)
        {
            List<OrderStep> oOrderStep = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = OrderStepDA.Gets(tc);
                oOrderStep = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get OrderStep", e);
                #endregion
            }

            return oOrderStep;
        }
 
       public List<OrderStep> Gets(int nStyleType, Int64 nUserID)
        {
            List<OrderStep> oOrderSteps = new List<OrderStep>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = OrderStepDA.Gets(nStyleType, tc);
                oOrderSteps = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get OrderStep", e);
                #endregion
            }

            return oOrderSteps;
        }
        public List<OrderStep> Gets(string sSQL, Int64 nUserID)
        {
            List<OrderStep> oOrderStep = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = OrderStepDA.Gets(tc, sSQL);
                oOrderStep = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get OrderStep", e);
                #endregion
            }

            return oOrderStep;
        }

        #endregion
    }   

}
