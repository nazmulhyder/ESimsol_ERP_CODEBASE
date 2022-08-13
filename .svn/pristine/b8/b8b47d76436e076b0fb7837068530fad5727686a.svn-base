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

    public class ProductionRecipeService : MarshalByRefObject, IProductionRecipeService
    {
        #region Private functions and declaration

        private static ProductionRecipe MapObject(NullHandler oReader)
        {
            ProductionRecipe oProductionRecipe = new ProductionRecipe();
            oProductionRecipe.ProductionRecipeID = oReader.GetInt32("ProductionRecipeID");
            oProductionRecipe.ProductionSheetID = oReader.GetInt32("ProductionSheetID");
            oProductionRecipe.ProductID = oReader.GetInt32("ProductID");
            oProductionRecipe.ProductBaseID = oReader.GetInt32("ProductBaseID");
            oProductionRecipe.RequiredQty = oReader.GetDouble("RequiredQty");
            oProductionRecipe.OutQty = oReader.GetDouble("OutQty");
            oProductionRecipe.QtyInPercent = oReader.GetDouble("QtyInPercent");
            oProductionRecipe.Remarks = oReader.GetString("Remarks");           
            oProductionRecipe.ProductCode = oReader.GetString("ProductCode");
            oProductionRecipe.ProductName = oReader.GetString("ProductName");
            oProductionRecipe.MUnitID = oReader.GetInt32("MUnitID");
            oProductionRecipe.MUName = oReader.GetString("MUName");           
            oProductionRecipe.StockBalance = oReader.GetDouble("StockBalance");
            oProductionRecipe.QtyType = (EnumQtyType)oReader.GetInt32("QtyType");
            oProductionRecipe.QtyTypeInt = oReader.GetInt32("QtyType");
            oProductionRecipe.MUSymbol = oReader.GetString("MUSymbol");
            oProductionRecipe.SheetNo = oReader.GetString("SheetNo");
            oProductionRecipe.ReportingUnit = oReader.GetString("ReportingUnit");
            oProductionRecipe.ReportingYetToRMOQty = oReader.GetDouble("ReportingYetToRMOQty");
            oProductionRecipe.RequisitionWiseYetToOutQty = oReader.GetDouble("RequisitionWiseYetToOutQty");
            oProductionRecipe.RequisitionQty = oReader.GetDouble("RequisitionQty");

            oProductionRecipe.RMRequisitionID = oReader.GetInt32("RMRequisitionID");
            return oProductionRecipe;
        }

        public static ProductionRecipe CreateObject(NullHandler oReader)
        {
            ProductionRecipe oProductionRecipe = new ProductionRecipe();
            oProductionRecipe = MapObject(oReader);
            return oProductionRecipe;
        }

        private List<ProductionRecipe> CreateObjects(IDataReader oReader)
        {
            List<ProductionRecipe> oProductionRecipe = new List<ProductionRecipe>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ProductionRecipe oItem = CreateObject(oHandler);
                oProductionRecipe.Add(oItem);
            }
            return oProductionRecipe;
        }

        #endregion

        #region Interface implementation



        public ProductionRecipe Get(int id, Int64 nUserId)
        {
            ProductionRecipe oProductionRecipe = new ProductionRecipe();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = ProductionRecipeDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProductionRecipe = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ProductionRecipe", e);
                #endregion
            }
            return oProductionRecipe;
        }

        public List<ProductionRecipe> Gets(int nORSID, Int64 nUserID)
        {
            List<ProductionRecipe> oProductionRecipes = new List<ProductionRecipe>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ProductionRecipeDA.Gets(nORSID, tc);
                oProductionRecipes = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ProductionRecipe oProductionRecipe = new ProductionRecipe();
                oProductionRecipe.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oProductionRecipes;
        }
        public List<ProductionRecipe> Gets(string sSQL, Int64 nUserID)
        {
            List<ProductionRecipe> oProductionRecipes = new List<ProductionRecipe>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ProductionRecipeDA.Gets(tc, sSQL);
                oProductionRecipes = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductionRecipe", e);
                #endregion
            }
            return oProductionRecipes;
        }

        public List<ProductionRecipe> GetsByWU(int nPSID, int nWUID, int nRMRequisitionID, Int64 nUserID)
        {
            List<ProductionRecipe> oProductionRecipes = new List<ProductionRecipe>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ProductionRecipeDA.GetsByWU(tc, nPSID, nWUID, nRMRequisitionID);
                oProductionRecipes = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductionRecipe", e);
                #endregion
            }
            return oProductionRecipes;
        }

        

        #endregion
    }
    
   
}
