using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects.ReportingObject;
using ESimSol.Services.DataAccess.ReportingDA;
using ICS.Core.DataAccess;

using ICS.Core.Utility;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    [Serializable]
    public class RptProductionCostAnalysisService : MarshalByRefObject, IRptProductionCostAnalysisService
    {
        #region Private functions and declaration
        private RptProductionCostAnalysis MapObject(NullHandler oReader)
        {
            RptProductionCostAnalysis oRptProductionCostAnalysis = new RptProductionCostAnalysis();
            oRptProductionCostAnalysis.RouteSheetID = oReader.GetInt32("RouteSheetID");
            oRptProductionCostAnalysis.MachineName = oReader.GetString("MachineName");
            oRptProductionCostAnalysis.UsesWeight = oReader.GetDouble("UsesWeight");
            oRptProductionCostAnalysis.OrderNo = oReader.GetString("OrderNo");
            oRptProductionCostAnalysis.BuyerName = oReader.GetString("BuyerName");
            oRptProductionCostAnalysis.Shade = oReader.GetString("Shade");
            oRptProductionCostAnalysis.ProductName = oReader.GetString("ProductName");
            oRptProductionCostAnalysis.BatchNo = oReader.GetString("BatchNo");
            oRptProductionCostAnalysis.ProductionQty = oReader.GetDouble("ProductionQty");
            oRptProductionCostAnalysis.StartTime = oReader.GetDateTime("StartTime");
            oRptProductionCostAnalysis.EndTime = oReader.GetDateTime("EndTime");
            oRptProductionCostAnalysis.RedyingForRSNo = oReader.GetString("RedyingForRSNo");
            oRptProductionCostAnalysis.DyesQty = oReader.GetDouble("DyesQty");
            oRptProductionCostAnalysis.AdditionalDyesQty = oReader.GetDouble("AdditionalDyesQty");
            oRptProductionCostAnalysis.ShadePercentage = oReader.GetDouble("ShadePercentage");
            oRptProductionCostAnalysis.AdditionalPercentage = oReader.GetDouble("AdditionalPercentage");
            oRptProductionCostAnalysis.DyesCost = oReader.GetDouble("DyesCost");
            oRptProductionCostAnalysis.ChemicalCost = oReader.GetDouble("ChemicalCost");
            oRptProductionCostAnalysis.Remark = oReader.GetString("Remark");
            oRptProductionCostAnalysis.CombineRSNo = oReader.GetString("CombineRSNo");
            oRptProductionCostAnalysis.IsInHouse = oReader.GetBoolean("IsInHouse");

            #region DUProductionRFT
            oRptProductionCostAnalysis.OrderType = oReader.GetInt32("OrderType");
            oRptProductionCostAnalysis.ContractorID = oReader.GetInt32("ContractorID");
            oRptProductionCostAnalysis.DyeingOrderID = oReader.GetInt32("DyeingOrderID");
            oRptProductionCostAnalysis.ProductID = oReader.GetInt32("ProductID");
            oRptProductionCostAnalysis.DyeingOrderDetailID = oReader.GetInt32("DyeingOrderDetailID");
            oRptProductionCostAnalysis.MachineID = oReader.GetInt32("MachineID");
            oRptProductionCostAnalysis.ColorName = oReader.GetString("ColorName");
            oRptProductionCostAnalysis.PSBatchNo = oReader.GetString("PSBatchNo");
            oRptProductionCostAnalysis.RouteSheetNo = oReader.GetString("RouteSheetNo");
            oRptProductionCostAnalysis.Qty = oReader.GetDouble("Qty");
            oRptProductionCostAnalysis.RSState = oReader.GetInt32("RSState");
            oRptProductionCostAnalysis.AddCount = oReader.GetInt32("AddCount");
            oRptProductionCostAnalysis.ShadeName = oReader.GetString("ShadeName");
            oRptProductionCostAnalysis.Liquor = oReader.GetDouble("Liquor");
            oRptProductionCostAnalysis.LabdipDetailID = oReader.GetInt32("LabdipDetailID");
            oRptProductionCostAnalysis.LabdipNo = oReader.GetString("LabdipNo");
            oRptProductionCostAnalysis.OrderTypeSt = oReader.GetString("OrderTypeSt");
            oRptProductionCostAnalysis.IsReDyeing = oReader.GetBoolean("IsReDyeing");
            oRptProductionCostAnalysis.RouteSheetCombineID = oReader.GetInt32("RouteSheetCombineID");
            #endregion



            return oRptProductionCostAnalysis;
        }

        private RptProductionCostAnalysis CreateObject(NullHandler oReader)
        {
            RptProductionCostAnalysis oRptProductionCostAnalysis = new RptProductionCostAnalysis();
            oRptProductionCostAnalysis = MapObject(oReader);
            return oRptProductionCostAnalysis;
        }

        private List<RptProductionCostAnalysis> CreateObjects(IDataReader oReader)
        {
            List<RptProductionCostAnalysis> oRptProductionCostAnalysiss = new List<RptProductionCostAnalysis>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RptProductionCostAnalysis oItem = CreateObject(oHandler);
                oRptProductionCostAnalysiss.Add(oItem);
            }
            return oRptProductionCostAnalysiss;
        }
        #endregion

        #region Interface implementation
        public RptProductionCostAnalysisService() { }

        public List<RptProductionCostAnalysis> MailContent(int PSSID, DateTime StartTime, DateTime EndTime, Int64 nUserId)
        {
            List<RptProductionCostAnalysis> oRptProductionCostAnalysiss = new List<RptProductionCostAnalysis>() ;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RptProductionCostAnalysisDA.MailContent(tc, PSSID, StartTime, EndTime);
                oRptProductionCostAnalysiss = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Production Cost Analysis ", e);

                #endregion
            }
            return oRptProductionCostAnalysiss;
        }
        public List<RptProductionCostAnalysis> MailContentDUProductionRFT(int PSSID, DateTime StartTime, DateTime EndTime, int nViewType,Int64 nUserId)
        {
            List<RptProductionCostAnalysis> oRptProductionCostAnalysiss = new List<RptProductionCostAnalysis>();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RptProductionCostAnalysisDA.MailContentDUProductionRFT(tc, PSSID, StartTime, EndTime, nViewType);
                oRptProductionCostAnalysiss = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Production Cost Analysis ", e);

                #endregion
            }
            return oRptProductionCostAnalysiss;
        }

        #endregion
    }
}
