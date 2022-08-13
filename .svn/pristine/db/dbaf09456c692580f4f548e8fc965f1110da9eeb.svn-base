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
    public class ProductionCostReDyeingService : MarshalByRefObject, IProductionCostReDyeingService
    {
        #region Private functions and declaration
        private ProductionCostReDyeing MapObject(NullHandler oReader)
        {
            ProductionCostReDyeing oProductionCostReDyeing = new ProductionCostReDyeing();

            oProductionCostReDyeing.ProductionDate = oReader.GetDateTime("ProductionDate");
            oProductionCostReDyeing.OrderNo = oProductionCostReDyeing.RouteSheetNo = oReader.GetString("OrderNo"); ;
            oProductionCostReDyeing.RouteSheetID = oReader.GetInt32("RouteSheetID");
            oProductionCostReDyeing.RSYarnID = oReader.GetInt32("RSYarnID");
            oProductionCostReDyeing.LotID = oReader.GetInt32("LotID");
            oProductionCostReDyeing.YarnQtyInLBS = oReader.GetDouble("YarnQtyInLBS");
            oProductionCostReDyeing.YarnUnitPrice = oReader.GetDouble("YarnUnitPrice");
            oProductionCostReDyeing.DyesQty = oReader.GetDouble("DyesQty");
            oProductionCostReDyeing.DyesPrice = oReader.GetDouble("DyesPrice");
            oProductionCostReDyeing.ChemicalQty = oReader.GetDouble("ChemicalQty");
            oProductionCostReDyeing.ChemicalPrice = oReader.GetDouble("ChemicalPrice");
            oProductionCostReDyeing.RouteSheetNo = oReader.GetString("RouteSheetNo");
            oProductionCostReDyeing.BuyerName = oReader.GetString("BuyerName");

            oProductionCostReDyeing.ReDyeProductionDate = oReader.GetDateTime("ReDyeProductionDate");
            oProductionCostReDyeing.ReDyeOrderNo = oReader.GetString("ReDyeOrderNo"); ;
            oProductionCostReDyeing.ReDyeRouteSheetID = oReader.GetInt32("ReDyeRouteSheetID");
            oProductionCostReDyeing.ReDyeRSYarnID = oReader.GetInt32("ReDyeRSYarnID");
            oProductionCostReDyeing.ReDyeLotID = oReader.GetInt32("ReDyeLotID");
            oProductionCostReDyeing.ReDyeYarnQtyInLBS = oReader.GetDouble("ReDyeYarnQtyInLBS");
            oProductionCostReDyeing.ReDyeYarnUnitPrice = oReader.GetDouble("ReDyeYarnUnitPrice");
            oProductionCostReDyeing.ReDyeDyesQty = oReader.GetDouble("ReDyeDyesQty");
            oProductionCostReDyeing.ReDyeDyesPrice = oReader.GetDouble("ReDyeDyesPrice");
            oProductionCostReDyeing.ReDyeChemicalQty = oReader.GetDouble("ReDyeChemicalQty");
            oProductionCostReDyeing.ReDyeChemicalPrice = oReader.GetDouble("ReDyeChemicalPrice");
            oProductionCostReDyeing.ReDyeRouteSheetNo = oReader.GetString("ReDyeRouteSheetNo");
            oProductionCostReDyeing.ReDyeBuyerName = oReader.GetString("ReDyeBuyerName");

            return oProductionCostReDyeing;
        }

        private ProductionCostReDyeing CreateObject(NullHandler oReader)
        {
            ProductionCostReDyeing oProductionCostReDyeing = new ProductionCostReDyeing();
            oProductionCostReDyeing = MapObject(oReader);
            return oProductionCostReDyeing;
        }

        private List<ProductionCostReDyeing> CreateObjects(IDataReader oReader)
        {
            List<ProductionCostReDyeing> oProductionCostReDyeings = new List<ProductionCostReDyeing>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ProductionCostReDyeing oItem = CreateObject(oHandler);
                oProductionCostReDyeings.Add(oItem);
            }
            return oProductionCostReDyeings;
        }
        #endregion

        #region Interface implementation
        public ProductionCostReDyeingService() { }

        public List<ProductionCostReDyeing> Gets(DateTime startDate, DateTime EndDate, string sBuyerIDs, string sRouteSheetNos, string sPTUIDs, Int64 nUserId)
        {
            List<ProductionCostReDyeing> oProductionCostReDyeings = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductionCostReDyeingDA.Gets(tc, startDate, EndDate, sBuyerIDs, sRouteSheetNos, sPTUIDs);
                oProductionCostReDyeings = CreateObjects(reader);
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
            return oProductionCostReDyeings;
        }

        #endregion
    }
}
