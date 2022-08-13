using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class ProductionSheetService : MarshalByRefObject, IProductionSheetService
    {
        #region Private functions and declaration
        private ProductionSheet MapObject(NullHandler oReader)
        {
            ProductionSheet oProductionSheet = new ProductionSheet();
            oProductionSheet.ProductionSheetID = oReader.GetInt32("ProductionSheetID");
            oProductionSheet.ProductCode = oReader.GetString("ProductCode");
            oProductionSheet.ProductName = oReader.GetString("ProductName");
            oProductionSheet.SheetNo = oReader.GetString("SheetNo");
            oProductionSheet.PTUUnit2ID = oReader.GetInt32("PTUUnit2ID");
            oProductionSheet.IssueDate = oReader.GetDateTime("IssueDate");
            oProductionSheet.ProductID = oReader.GetInt32("ProductID");
            oProductionSheet.Note = oReader.GetString("Note");
            oProductionSheet.PONo = oReader.GetString("PONo");
            oProductionSheet.Quantity = oReader.GetDouble("Quantity");
            oProductionSheet.ProdOrderQty = oReader.GetDouble("ProdOrderQty");
            oProductionSheet.ExportSCQty = oReader.GetDouble("ExportSCQty");
            oProductionSheet.BUID = oReader.GetInt32("BUID");
            oProductionSheet.ModelReferenceID = oReader.GetInt32("ModelReferenceID");
            oProductionSheet.Cavity = oReader.GetInt32("Cavity");
            oProductionSheet.FGWeight = oReader.GetDouble("FGWeight");
            oProductionSheet.NaliWeight = oReader.GetDouble("NaliWeight");
            oProductionSheet.WeightFor = oReader.GetDouble("WeightFor");
            oProductionSheet.FGWeightUnitID = oReader.GetInt32("FGWeightUnitID");
            oProductionSheet.RecipeID = oReader.GetInt32("RecipeID");
            oProductionSheet.RecipeName = oReader.GetString("RecipeName");
            oProductionSheet.ColorName = oReader.GetString("ColorName");
            oProductionSheet.YetToSheetQty = oReader.GetDouble("YetToSheetQty");
            oProductionSheet.ExportPINo = oReader.GetString("ExportPINo");
            oProductionSheet.ContractorID = oReader.GetInt32("ContractorID");
            oProductionSheet.ContractorName = oReader.GetString("ContractorName");
            oProductionSheet.ModelReferencenName = oReader.GetString("ModelReferencenName");
            oProductionSheet.SheetStatus = (EnumProductionSheetStatus)oReader.GetInt32("SheetStatus");
            oProductionSheet.SheetStatusInInt = oReader.GetInt32("SheetStatus");
            oProductionSheet.RawMaterialStatus = (EnumRawMaterialStatus)oReader.GetInt32("RawMaterialStatus");
            oProductionSheet.RawMaterialStatusInInt = oReader.GetInt32("RawMaterialStatus");
            oProductionSheet.QCStatus = (EnumQCStatus)oReader.GetInt32("QCStatus");
            oProductionSheet.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oProductionSheet.ApprovedByName = oReader.GetString("ApprovedByName");
            oProductionSheet.UnitID = oReader.GetInt32("UnitID");
            oProductionSheet.UnitSymbol = oReader.GetString("UnitSymbol");
            oProductionSheet.LastExecutionStepName = oReader.GetString("LastExecutionStepName");
            oProductionSheet.LastExecutionStepQty = oReader.GetDouble("LastExecutionStepQty");
            oProductionSheet.QCQty = oReader.GetDouble("QCQty");
            oProductionSheet.MachineID = oReader.GetInt32("MachineID");
            oProductionSheet.MachineName = oReader.GetString("MachineName");
            oProductionSheet.PerCartonFGQty = oReader.GetDouble("PerCartonFGQty");
            oProductionSheet.YetToPlanQty = oReader.GetDouble("YetToPlanQty");
            oProductionSheet.FGWeightUnitSymbol = oReader.GetString("FGWeightUnitSymbol");
            oProductionSheet.BuyerName = oReader.GetString("BuyerName");
            oProductionSheet.ProductDescription = oReader.GetString("ProductDescription");
            oProductionSheet.ProductSize = oReader.GetString("ProductSize");
            oProductionSheet.Measurement = oReader.GetString("Measurement");
            oProductionSheet.OrderColorName = oReader.GetString("OrderColorName");
            oProductionSheet.ProductNature = (EnumProductNature)oReader.GetInt32("ProductNature");
            oProductionSheet.ProductNatureInInt = oReader.GetInt32("ProductNature");
            oProductionSheet.ProductionStartBy = oReader.GetInt32("ProductionStartBy");
            oProductionSheet.ProductionStartByName = oReader.GetString("ProductionStartByName");
            oProductionSheet.ProductionStartDate = oReader.GetDateTime("ProductionStartDate");
            return oProductionSheet;
        }

        private ProductionSheet CreateObject(NullHandler oReader)
        {
            ProductionSheet oProductionSheet = new ProductionSheet();
            oProductionSheet = MapObject(oReader);
            return oProductionSheet;
        }

        private List<ProductionSheet> CreateObjects(IDataReader oReader)
        {
            List<ProductionSheet> oProductionSheet = new List<ProductionSheet>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ProductionSheet oItem = CreateObject(oHandler);
                oProductionSheet.Add(oItem);
            }
            return oProductionSheet;
        }

        #endregion

        #region Interface implementation
        public ProductionSheetService() { }

        public ProductionSheet Save(ProductionSheet oProductionSheet, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<ProductionRecipe> oProductionRecipes = new List<ProductionRecipe>();
            oProductionRecipes = oProductionSheet.ProductionRecipes;
            string sProductionRecipeIDs = "";
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oProductionSheet.ProductionSheetID <= 0)
                {
                    reader = ProductionSheetDA.InsertUpdate(tc, oProductionSheet, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ProductionSheetDA.InsertUpdate(tc, oProductionSheet, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProductionSheet = new ProductionSheet();
                    oProductionSheet = CreateObject(oReader);
                }
                reader.Close();
                #region Production Recipe Part
                foreach (ProductionRecipe oItem in oProductionRecipes)
                {
                    IDataReader readerdetail;
                    oItem.ProductionSheetID = oProductionSheet.ProductionSheetID;
                    if (oItem.ProductionRecipeID <= 0)
                    {
                        readerdetail = ProductionRecipeDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                    }
                    else
                    {
                        readerdetail = ProductionRecipeDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sProductionRecipeIDs = sProductionRecipeIDs + oReaderDetail.GetString("ProductionRecipeID") + ",";
                    }
                    readerdetail.Close();
                }
                if (sProductionRecipeIDs.Length > 0)
                {
                    sProductionRecipeIDs = sProductionRecipeIDs.Remove(sProductionRecipeIDs.Length - 1, 1);
                }
                ProductionRecipe oProductionRecipe = new ProductionRecipe();
                oProductionRecipe.ProductionSheetID = oProductionSheet.ProductionSheetID;
                ProductionRecipeDA.Delete(tc, oProductionRecipe, EnumDBOperation.Delete, nUserID, sProductionRecipeIDs);
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Save ProductionSheet. Because of " + e.Message, e);
                #endregion
            }
            return oProductionSheet;
        }
        public ProductionSheet ChangeRawMaterial(ProductionSheet oProductionSheet, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<ProductionRecipe> oProductionRecipes = new List<ProductionRecipe>();
            oProductionRecipes = oProductionSheet.ProductionRecipes;
            string sProductionRecipeIDs = "";
            try
            {
                tc = TransactionContext.Begin(true);
                #region Production Recipe Part
                foreach (ProductionRecipe oItem in oProductionRecipes)
                {
                    IDataReader readerdetail;
                    oItem.ProductionSheetID = oProductionSheet.ProductionSheetID;
                    if (oItem.ProductionRecipeID <= 0)
                    {
                        readerdetail = ProductionRecipeDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                    }
                    else
                    {
                        readerdetail = ProductionRecipeDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sProductionRecipeIDs = sProductionRecipeIDs + oReaderDetail.GetString("ProductionRecipeID") + ",";
                    }
                    readerdetail.Close();
                }
                if (sProductionRecipeIDs.Length > 0)
                {
                    sProductionRecipeIDs = sProductionRecipeIDs.Remove(sProductionRecipeIDs.Length - 1, 1);
                }
                ProductionRecipe oProductionRecipe = new ProductionRecipe();
                oProductionRecipe.ProductionSheetID = oProductionSheet.ProductionSheetID;
                ProductionRecipeDA.Delete(tc, oProductionRecipe, EnumDBOperation.Delete, nUserID, sProductionRecipeIDs);
                #endregion
                #region Get
                IDataReader reader = ProductionSheetDA.Get(tc, oProductionRecipe.ProductionSheetID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProductionSheet = CreateObject(oReader);
                }
                reader.Close();
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Change Raw Material ProductionSheet. Because of " + e.Message, e);
                #endregion
            }
            return oProductionSheet;
        }
        public ProductionSheet Approve(ProductionSheet oProductionSheet, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ProductionSheetDA.InsertUpdate(tc, oProductionSheet, EnumDBOperation.Approval, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProductionSheet = new ProductionSheet();
                    oProductionSheet = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save ProductionSheet. Because of " + e.Message, e);
                #endregion
            }
            return oProductionSheet;
        }

        public ProductionSheet ProductionStart(ProductionSheet oProductionSheet, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ProductionSheetDA.InsertUpdate(tc, oProductionSheet, EnumDBOperation.Start, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProductionSheet = new ProductionSheet();
                    oProductionSheet = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save ProductionSheet. Because of " + e.Message, e);
                #endregion
            }
            return oProductionSheet;
        }

        public ProductionSheet ProductionUndo(ProductionSheet oProductionSheet, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ProductionSheetDA.InsertUpdate(tc, oProductionSheet, EnumDBOperation.Undo, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProductionSheet = new ProductionSheet();
                    oProductionSheet = CreateObject(oReader);
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
                throw new ServiceException("Failed to Undo Production. Because of " + e.Message, e);
                #endregion
            }
            return oProductionSheet;
        }
        public ProductionSheet UndoApproved(ProductionSheet oProductionSheet, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ProductionSheetDA.InsertUpdate(tc, oProductionSheet, EnumDBOperation.UnApproval, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProductionSheet = new ProductionSheet();
                    oProductionSheet = CreateObject(oReader);
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
                throw new ServiceException("Failed to Undo Production. Because of " + e.Message, e);
                #endregion
            }
            return oProductionSheet;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ProductionSheet oProductionSheet = new ProductionSheet();
                oProductionSheet.ProductionSheetID = id;
                ProductionSheetDA.Delete(tc, oProductionSheet, EnumDBOperation.Delete, nUserId);
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

        public ProductionSheet Get(int id, Int64 nUserId)
        {
            ProductionSheet oProductionSheet = new ProductionSheet();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = ProductionSheetDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProductionSheet = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ProductionSheet", e);
                #endregion
            }
            return oProductionSheet;
        }
        public List<ProductionSheet> Gets(Int64 nUserID)
        {
            List<ProductionSheet> oProductionSheets = new List<ProductionSheet>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductionSheetDA.Gets(tc);
                oProductionSheets = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductionSheet", e);
                #endregion
            }
            return oProductionSheets;
        }        
        public List<ProductionSheet> BUWiseGets(int nBUID,  int nProductNature, Int64 nUserID)
        {
            List<ProductionSheet> oProductionSheets = new List<ProductionSheet>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductionSheetDA.BUWiseGets(nBUID,nProductNature, tc);
                oProductionSheets = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductionSheet", e);
                #endregion
            }
            return oProductionSheets;
        }
        public List<ProductionSheet> Gets(string sSQL,Int64 nUserID)
        {
            List<ProductionSheet> oProductionSheets = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductionSheetDA.Gets(tc,sSQL);
                oProductionSheets = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductionSheet", e);
                #endregion
            }
            return oProductionSheets;
        }        
        #endregion
    }   
}