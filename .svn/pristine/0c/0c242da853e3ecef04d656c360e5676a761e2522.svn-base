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
    public class ProductionProcedureTemplateService : MarshalByRefObject, IProductionProcedureTemplateService
    {
        #region Private functions and declaration
        private ProductionProcedureTemplate MapObject(NullHandler oReader)
        {
            ProductionProcedureTemplate oProductionProcedureTemplate = new ProductionProcedureTemplate();
            oProductionProcedureTemplate.ProductionProcedureTemplateID = oReader.GetInt32("ProductionProcedureTemplateID");
            oProductionProcedureTemplate.TemplateNo = oReader.GetString("TemplateNo");
            oProductionProcedureTemplate.TemplateName = oReader.GetString("TemplateName");
            oProductionProcedureTemplate.Remarks = oReader.GetString("Remarks");
            return oProductionProcedureTemplate;
        }

        private ProductionProcedureTemplate CreateObject(NullHandler oReader)
        {
            ProductionProcedureTemplate oProductionProcedureTemplate = new ProductionProcedureTemplate();
            oProductionProcedureTemplate = MapObject(oReader);
            return oProductionProcedureTemplate;
        }

        private List<ProductionProcedureTemplate> CreateObjects(IDataReader oReader)
        {
            List<ProductionProcedureTemplate> oProductionProcedureTemplate = new List<ProductionProcedureTemplate>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ProductionProcedureTemplate oItem = CreateObject(oHandler);
                oProductionProcedureTemplate.Add(oItem);
            }
            return oProductionProcedureTemplate;
        }

        #endregion

        #region Interface implementation
        public ProductionProcedureTemplateService() { }

        public ProductionProcedureTemplate Save(ProductionProcedureTemplate oProductionProcedureTemplate, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {

                tc = TransactionContext.Begin(true);
                List<ProductionProcedureTemplateDetail> oProductionProcedureTemplateDetails = new List<ProductionProcedureTemplateDetail>();
                ProductionProcedureTemplateDetail oProductionProcedureTemplateDetail = new ProductionProcedureTemplateDetail();
                oProductionProcedureTemplateDetails = oProductionProcedureTemplate.ProductionProcedureTemplateDetails;
                string sProductionProcedureTemplateDetailIDs = "";

                #region Package Template part
                IDataReader reader;
                if (oProductionProcedureTemplate.ProductionProcedureTemplateID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ProductionProcedureTemplate, EnumRoleOperationType.Add);
                    reader = ProductionProcedureTemplateDA.InsertUpdate(tc, oProductionProcedureTemplate, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ProductionProcedureTemplate, EnumRoleOperationType.Edit);
                    reader = ProductionProcedureTemplateDA.InsertUpdate(tc, oProductionProcedureTemplate, EnumDBOperation.Update, nUserID);
                }

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProductionProcedureTemplate = new ProductionProcedureTemplate();
                    oProductionProcedureTemplate = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region Package Template Detail Part
                if (oProductionProcedureTemplateDetails != null)
                {
                    foreach (ProductionProcedureTemplateDetail oItem in oProductionProcedureTemplateDetails)
                    {
                        IDataReader readerdetail;
                        oItem.ProductionProcedureTemplateID = oProductionProcedureTemplate.ProductionProcedureTemplateID;
                        if (oItem.ProductionProcedureTemplateDetailID <= 0)
                        {
                            readerdetail = ProductionProcedureTemplateDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            readerdetail = ProductionProcedureTemplateDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sProductionProcedureTemplateDetailIDs = sProductionProcedureTemplateDetailIDs + oReaderDetail.GetString("ProductionProcedureTemplateDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sProductionProcedureTemplateDetailIDs.Length > 0)
                    {
                        sProductionProcedureTemplateDetailIDs = sProductionProcedureTemplateDetailIDs.Remove(sProductionProcedureTemplateDetailIDs.Length - 1, 1);
                    }

                }
                oProductionProcedureTemplateDetail = new ProductionProcedureTemplateDetail();
                oProductionProcedureTemplateDetail.ProductionProcedureTemplateID = oProductionProcedureTemplate.ProductionProcedureTemplateID;
                ProductionProcedureTemplateDetailDA.Delete(tc, oProductionProcedureTemplateDetail, EnumDBOperation.Delete, nUserID, sProductionProcedureTemplateDetailIDs);
                #endregion
                
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                oProductionProcedureTemplate.ErrorMessage = Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save ProductionProcedureTemplate. Because of " + e.Message, e);
                #endregion
            }
            return oProductionProcedureTemplate;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ProductionProcedureTemplate oProductionProcedureTemplate = new ProductionProcedureTemplate();
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.ProductionProcedureTemplate, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "ProductionProcedureTemplate", id);
                oProductionProcedureTemplate.ProductionProcedureTemplateID = id;
                ProductionProcedureTemplateDA.Delete(tc, oProductionProcedureTemplate, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete ProductionProcedureTemplate. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public ProductionProcedureTemplate Get(int id, Int64 nUserId)
        {
            ProductionProcedureTemplate oAccountHead = new ProductionProcedureTemplate();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ProductionProcedureTemplateDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get ProductionProcedureTemplate", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<ProductionProcedureTemplate> Gets(Int64 nUserID)
        {
            List<ProductionProcedureTemplate> oProductionProcedureTemplate = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductionProcedureTemplateDA.Gets(tc);
                oProductionProcedureTemplate = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductionProcedureTemplate", e);
                #endregion
            }

            return oProductionProcedureTemplate;
        }

        public List<ProductionProcedureTemplate> Gets(string sSQL, Int64 nUserID)
        {
            List<ProductionProcedureTemplate> oProductionProcedureTemplate = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductionProcedureTemplateDA.Gets(tc, sSQL);
                oProductionProcedureTemplate = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductionProcedureTemplate", e);
                #endregion
            }

            return oProductionProcedureTemplate;
        }
        #endregion
    }
}
