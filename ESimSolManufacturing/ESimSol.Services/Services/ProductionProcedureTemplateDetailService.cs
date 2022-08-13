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
    public class ProductionProcedureTemplateDetailService : MarshalByRefObject, IProductionProcedureTemplateDetailService
    {
        #region Private functions and declaration
        private ProductionProcedureTemplateDetail MapObject(NullHandler oReader)
        {
            ProductionProcedureTemplateDetail oProductionProcedureTemplateDetail = new ProductionProcedureTemplateDetail();
            oProductionProcedureTemplateDetail.ProductionProcedureTemplateDetailID = oReader.GetInt32("ProductionProcedureTemplateDetailID");
            oProductionProcedureTemplateDetail.ProductionProcedureTemplateID = oReader.GetInt32("ProductionProcedureTemplateID");
            oProductionProcedureTemplateDetail.ProductionStepID = oReader.GetInt32("ProductionStepID");
            oProductionProcedureTemplateDetail.Sequence = oReader.GetInt32("Sequence");
            oProductionProcedureTemplateDetail.Remarks = oReader.GetString("Remarks");
            oProductionProcedureTemplateDetail.StepName = oReader.GetString("StepName");
            return oProductionProcedureTemplateDetail;
        }

        private ProductionProcedureTemplateDetail CreateObject(NullHandler oReader)
        {
            ProductionProcedureTemplateDetail oProductionProcedureTemplateDetail = new ProductionProcedureTemplateDetail();
            oProductionProcedureTemplateDetail = MapObject(oReader);
            return oProductionProcedureTemplateDetail;
        }

        private List<ProductionProcedureTemplateDetail> CreateObjects(IDataReader oReader)
        {
            List<ProductionProcedureTemplateDetail> oProductionProcedureTemplateDetail = new List<ProductionProcedureTemplateDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ProductionProcedureTemplateDetail oItem = CreateObject(oHandler);
                oProductionProcedureTemplateDetail.Add(oItem);
            }
            return oProductionProcedureTemplateDetail;
        }

        #endregion

        #region Interface implementation
        public ProductionProcedureTemplateDetailService() { }

        public ProductionProcedureTemplateDetail Save(ProductionProcedureTemplateDetail oProductionProcedureTemplateDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oProductionProcedureTemplateDetail.ProductionProcedureTemplateDetailID <= 0)
                {
                    reader = ProductionProcedureTemplateDetailDA.InsertUpdate(tc, oProductionProcedureTemplateDetail, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ProductionProcedureTemplateDetailDA.InsertUpdate(tc, oProductionProcedureTemplateDetail, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProductionProcedureTemplateDetail = new ProductionProcedureTemplateDetail();
                    oProductionProcedureTemplateDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save ProductionProcedureTemplateDetail. Because of " + e.Message, e);
                #endregion
            }
            return oProductionProcedureTemplateDetail;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ProductionProcedureTemplateDetail oProductionProcedureTemplateDetail = new ProductionProcedureTemplateDetail();
                oProductionProcedureTemplateDetail.ProductionProcedureTemplateDetailID = id;
                ProductionProcedureTemplateDetailDA.Delete(tc, oProductionProcedureTemplateDetail, EnumDBOperation.Delete, nUserId, "");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete ProductionProcedureTemplateDetail. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public ProductionProcedureTemplateDetail Get(int id, Int64 nUserId)
        {
            ProductionProcedureTemplateDetail oAccountHead = new ProductionProcedureTemplateDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ProductionProcedureTemplateDetailDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get ProductionProcedureTemplateDetail", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<ProductionProcedureTemplateDetail> Gets(Int64 nUserID)
        {
            List<ProductionProcedureTemplateDetail> oProductionProcedureTemplateDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductionProcedureTemplateDetailDA.Gets(tc);
                oProductionProcedureTemplateDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductionProcedureTemplateDetail", e);
                #endregion
            }

            return oProductionProcedureTemplateDetail;
        }

        public List<ProductionProcedureTemplateDetail> Gets(int id, Int64 nUserID)
        {
            List<ProductionProcedureTemplateDetail> oProductionProcedureTemplateDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductionProcedureTemplateDetailDA.Gets(tc, id);
                oProductionProcedureTemplateDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductionProcedureTemplateDetail", e);
                #endregion
            }

            return oProductionProcedureTemplateDetail;
        }
        #endregion
    }
}
