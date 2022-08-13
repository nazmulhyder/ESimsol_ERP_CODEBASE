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
    public class ProductionStepService : MarshalByRefObject, IProductionStepService
    {
        #region Private functions and declaration
        private ProductionStep MapObject(NullHandler oReader)
        {
            ProductionStep oProductionStep = new ProductionStep();
            oProductionStep.ProductionStepID = oReader.GetInt32("ProductionStepID");
            oProductionStep.StepName = oReader.GetString("StepName");
            oProductionStep.ShortName = oReader.GetString("ShortName");
            oProductionStep.ProductionStepType = (EnumProductionStepType)oReader.GetInt32("ProductionStepType");
            oProductionStep.ProductionStepTypeInt = oReader.GetInt32("ProductionStepType");
            oProductionStep.Note = oReader.GetString("Note");

            return oProductionStep;
        }

        private ProductionStep CreateObject(NullHandler oReader)
        {
            ProductionStep oProductionStep = new ProductionStep();
            oProductionStep = MapObject(oReader);
            return oProductionStep;
        }

        private List<ProductionStep> CreateObjects(IDataReader oReader)
        {
            List<ProductionStep> oProductionStep = new List<ProductionStep>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ProductionStep oItem = CreateObject(oHandler);
                oProductionStep.Add(oItem);
            }
            return oProductionStep;
        }

        #endregion

        #region Interface implementation
        public ProductionStepService() { }

        public ProductionStep Save(ProductionStep oProductionStep, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oProductionStep.ProductionStepID <= 0)
                {
                    reader = ProductionStepDA.InsertUpdate(tc, oProductionStep, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ProductionStepDA.InsertUpdate(tc, oProductionStep, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProductionStep = new ProductionStep();
                    oProductionStep = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save ProductionStep. Because of " + e.Message, e);
                #endregion
            }
            return oProductionStep;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ProductionStep oProductionStep = new ProductionStep();
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.ProductionStep, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "ProductionStep", id);
                oProductionStep.ProductionStepID = id;
                ProductionStepDA.Delete(tc, oProductionStep, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete ProductionStep. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public ProductionStep Get(int id, Int64 nUserId)
        {
            ProductionStep oAccountHead = new ProductionStep();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ProductionStepDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get ProductionStep", e);
                #endregion
            }

            return oAccountHead;
        }
                
        public List<ProductionStep> Gets(Int64 nUserID)
        {
            List<ProductionStep> oProductionStep = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductionStepDA.Gets(tc);
                oProductionStep = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductionStep", e);
                #endregion
            }

            return oProductionStep;
        }

        public List<ProductionStep> Gets(string sSQL, Int64 nUserID)
        {
            List<ProductionStep> oProductionStep = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductionStepDA.Gets(tc, sSQL);
                oProductionStep = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductionStep", e);
                #endregion
            }

            return oProductionStep;
        }
        #endregion
    }
}
