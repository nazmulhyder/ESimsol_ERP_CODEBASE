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
    public class ProductionProcedureService : MarshalByRefObject, IProductionProcedureService
    {
        #region Private functions and declaration
        private ProductionProcedure MapObject(NullHandler oReader)
        {
            ProductionProcedure oProductionProcedure = new ProductionProcedure();
            oProductionProcedure.ProductionProcedureID = oReader.GetInt32("ProductionProcedureID");
            oProductionProcedure.ProductionSheetID = oReader.GetInt32("ProductionSheetID");
            oProductionProcedure.ProductionStepID = oReader.GetInt32("ProductionStepID");
            oProductionProcedure.Sequence = oReader.GetInt32("Sequence");
            oProductionProcedure.Remarks = oReader.GetString("Remarks");
            oProductionProcedure.StepName = oReader.GetString("StepName");
            oProductionProcedure.Measurement = oReader.GetString("Measurement");
            oProductionProcedure.ThickNess = oReader.GetString("ThickNess");
            oProductionProcedure.ProductionStepType = (EnumProductionStepType)oReader.GetInt32("ProductionStepType");

            return oProductionProcedure;
        }

        private ProductionProcedure CreateObject(NullHandler oReader)
        {
            ProductionProcedure oProductionProcedure = new ProductionProcedure();
            oProductionProcedure = MapObject(oReader);
            return oProductionProcedure;
        }

        private List<ProductionProcedure> CreateObjects(IDataReader oReader)
        {
            List<ProductionProcedure> oProductionProcedure = new List<ProductionProcedure>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ProductionProcedure oItem = CreateObject(oHandler);
                oProductionProcedure.Add(oItem);
            }
            return oProductionProcedure;
        }

        #endregion

        #region Interface implementation
        public ProductionProcedureService() { }

        public List<ProductionProcedure> Save(ProductionSheet oProductionSheet, Int64 nUserID)
        {
            TransactionContext tc = null; string sProductionProcedureIDs = "";
            List<ProductionProcedure> oProductionProcedures = new List<ProductionProcedure>();
            ProductionProcedure oProductionProcedure = new ProductionProcedure();
            oProductionProcedures = oProductionSheet.ProductionProcedures;
            try
            {
                tc = TransactionContext.Begin(true);
                #region Insert ProductionProcedures
                if (oProductionProcedures != null)
                {
                    foreach (ProductionProcedure oItem in oProductionProcedures)
                    {
                        IDataReader readerdetail;
                        oItem.ProductionSheetID = oProductionSheet.ProductionSheetID;
                        if (oItem.ProductionProcedureID <= 0)
                        {
                            readerdetail = ProductionProcedureDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            readerdetail = ProductionProcedureDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sProductionProcedureIDs = sProductionProcedureIDs + oReaderDetail.GetString("ProductionProcedureID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sProductionProcedureIDs.Length > 0)
                    {
                        sProductionProcedureIDs = sProductionProcedureIDs.Remove(sProductionProcedureIDs.Length - 1, 1);
                    }

                }

                oProductionProcedure = new ProductionProcedure();
                oProductionProcedure.ProductionSheetID = oProductionSheet.ProductionSheetID;
                ProductionProcedureDA.Delete(tc, oProductionProcedure, EnumDBOperation.Delete, nUserID, sProductionProcedureIDs);
                #endregion

                #region Gets
                IDataReader reader;
                reader = ProductionProcedureDA.Gets(tc, oProductionSheet.ProductionSheetID);
                oProductionProcedures = CreateObjects(reader);
                reader.Close();
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oProductionProcedures = new List<ProductionProcedure>();
                oProductionProcedure = new ProductionProcedure();
                oProductionProcedure.ErrorMessage = e.Message.Split('~')[0];
                oProductionProcedures.Add(oProductionProcedure);

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save ProductionProcedure. Because of " + e.Message, e);
                #endregion
            }
            return oProductionProcedures;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ProductionProcedure oProductionProcedure = new ProductionProcedure();
                oProductionProcedure.ProductionProcedureID = id;
                ProductionProcedureDA.Delete(tc, oProductionProcedure, EnumDBOperation.Delete, nUserId, "");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete ProductionProcedure. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public ProductionProcedure Get(int id, Int64 nUserId)
        {
            ProductionProcedure oAccountHead = new ProductionProcedure();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ProductionProcedureDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get ProductionProcedure", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<ProductionProcedure> Gets(Int64 nUserID)
        {
            List<ProductionProcedure> oProductionProcedure = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductionProcedureDA.Gets(tc);
                oProductionProcedure = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductionProcedure", e);
                #endregion
            }

            return oProductionProcedure;
        }


        public List<ProductionProcedure> Gets(string sSQL, Int64 nUserID)
        {
            List<ProductionProcedure> oProductionProcedure = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductionProcedureDA.Gets(tc, sSQL);
                oProductionProcedure = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductionProcedure", e);
                #endregion
            }

            return oProductionProcedure;
        }

        public List<ProductionProcedure> Gets(int nProductionSheetID, Int64 nUserID)
        {
            List<ProductionProcedure> oProductionProcedure = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductionProcedureDA.Gets(tc, nProductionSheetID);
                oProductionProcedure = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductionProcedure", e);
                #endregion
            }

            return oProductionProcedure;
        }

        #endregion
    }
}
