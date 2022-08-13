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
    [Serializable]
    public class ProductionCostService : MarshalByRefObject, IProductionCostService
    {
        #region Private functions and declaration
        private ProductionCost MapObject(NullHandler oReader)
        {
            ProductionCost oProductionCost = new ProductionCost();

            oProductionCost.ProductionDate = oReader.GetDateTime("ProductionDate");
            oProductionCost.OrderNo = oProductionCost.RouteSheetNo = oReader.GetString("OrderNo"); ;
            oProductionCost.RouteSheetID = oReader.GetInt32("RouteSheetID");
            oProductionCost.RSYarnID = oReader.GetInt32("RSYarnID");
            oProductionCost.LotID = oReader.GetInt32("LotID");
            oProductionCost.YarnQtyInLBS = oReader.GetDouble("YarnQtyInLBS");
            oProductionCost.YarnUnitPrice = oReader.GetDouble("YarnUnitPrice");
            oProductionCost.DyesQty = oReader.GetDouble("DyesQty");
            oProductionCost.DyesPrice = oReader.GetDouble("DyesPrice");
            oProductionCost.ChemicalQty = oReader.GetDouble("ChemicalQty");
            oProductionCost.ChemicalPrice = oReader.GetDouble("ChemicalPrice");
            oProductionCost.RouteSheetNo = oReader.GetString("RouteSheetNo");
            oProductionCost.BuyerName = oReader.GetString("BuyerName");

            return oProductionCost;
        }

        private ProductionCost CreateObject(NullHandler oReader)
        {
            ProductionCost oProductionCost = new ProductionCost();
            oProductionCost = MapObject(oReader);
            return oProductionCost;
        }

        private List<ProductionCost> CreateObjects(IDataReader oReader)
        {
            List<ProductionCost> oProductionCosts = new List<ProductionCost>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ProductionCost oItem = CreateObject(oHandler);
                oProductionCosts.Add(oItem);
            }
            return oProductionCosts;
        }
        #endregion

        #region Interface implementation
        public ProductionCostService() { }

        public List<ProductionCost> Gets(DateTime startDate, DateTime EndDate, string sBuyerIDs, string sRouteSheetNos, string sPTUIDs, Int64 nUserId)
        {
            List<ProductionCost> oProductionCosts = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductionCostDA.Gets(tc, startDate, EndDate, sBuyerIDs, sRouteSheetNos, sPTUIDs);
                oProductionCosts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Purchase Invoices", e);
                #endregion
            }
            return oProductionCosts;
        }

        #endregion
    }
}
