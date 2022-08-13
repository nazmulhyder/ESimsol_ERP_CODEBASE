using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;


using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class DUDyeingStepService : MarshalByRefObject, IDUDyeingStepService
    {
        #region Private functions and declaration
        private DUDyeingStep MapObject(NullHandler oReader)
        {
            DUDyeingStep oDUDyeingStep = new DUDyeingStep();
            oDUDyeingStep.DUDyeingStepID = oReader.GetInt32("DUDyeingStepID");
            oDUDyeingStep.DyeingStepType = oReader.GetInt32("DyeingStepType");
            oDUDyeingStep.Name = oReader.GetString("Name");
            oDUDyeingStep.ShortName = oReader.GetString("ShortName");
            
            return oDUDyeingStep;
        }

        private DUDyeingStep CreateObject(NullHandler oReader)
        {
            DUDyeingStep oDUDyeingStep = new DUDyeingStep();
            oDUDyeingStep = MapObject(oReader);
            return oDUDyeingStep;
        }

        private List<DUDyeingStep> CreateObjects(IDataReader oReader)
        {
            List<DUDyeingStep> oDUDyeingSteps = new List<DUDyeingStep>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DUDyeingStep oItem = CreateObject(oHandler);
                oDUDyeingSteps.Add(oItem);
            }
            return oDUDyeingSteps;
        }

        #endregion

        #region Interface implementation
        public DUDyeingStepService() { }


        public DUDyeingStep Save(DUDyeingStep oDUDyeingStep, Int64 nUserId)
        {

            TransactionContext tc = null;
          
            try
            {
                tc = TransactionContext.Begin(true);
                #region DUDyeingStep
                IDataReader reader;
                if (oDUDyeingStep.DUDyeingStepID <= 0)
                {
                    reader = DUDyeingStepDA.InsertUpdate(tc, oDUDyeingStep, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = DUDyeingStepDA.InsertUpdate(tc, oDUDyeingStep, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUDyeingStep = new DUDyeingStep();
                    oDUDyeingStep = CreateObject(oReader);
                }
                reader.Close();
                #endregion
                
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oDUDyeingStep = new DUDyeingStep();
                oDUDyeingStep.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oDUDyeingStep;
        }
      
        public String Delete(DUDyeingStep oDUDyeingStep, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                DUDyeingStepDA.Delete(tc, oDUDyeingStep, EnumDBOperation.Delete, nUserID);
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
            return Global.DeleteMessage;
        }
        public DUDyeingStep Get(int id, Int64 nUserId)
        {
            DUDyeingStep oDUDyeingStep = new DUDyeingStep();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DUDyeingStepDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUDyeingStep = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oDUDyeingStep;
        }
        public DUDyeingStep GetBy(int nDyeingStepType, Int64 nUserId)
        {
            DUDyeingStep oDUDyeingStep = new DUDyeingStep();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DUDyeingStepDA.GetBy(tc, nDyeingStepType);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUDyeingStep = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oDUDyeingStep;
        }
     

        public List<DUDyeingStep> Gets(Int64 nUserId)
        {
            List<DUDyeingStep> oDUDyeingSteps = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUDyeingStepDA.Gets(tc);
                oDUDyeingSteps = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oDUDyeingSteps;
        }
        public List<DUDyeingStep> GetsByOrderSetup( string sDUOrderSetupID,Int64 nUserId)
        {
            List<DUDyeingStep> oDUDyeingSteps = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUDyeingStepDA.GetsByOrderSetup(tc, sDUOrderSetupID);
                oDUDyeingSteps = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oDUDyeingSteps;
        }
     
        public DUDyeingStep Activate(DUDyeingStep oDUDyeingStep, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DUDyeingStepDA.Activate(tc, oDUDyeingStep);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUDyeingStep = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oDUDyeingStep = new DUDyeingStep();
                oDUDyeingStep.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oDUDyeingStep;
        }
    

        #endregion
    }
}