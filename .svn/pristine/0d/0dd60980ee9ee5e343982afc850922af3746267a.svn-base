using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.BusinessObjects.ReportingObject;
using ESimSol.Services.DataAccess.ReportingDA;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services.ReportingService
{
    public class WUStockReportService : MarshalByRefObject, IWUStockReportService
    {
        #region Private functions and declaration
        private WUStockReport MapObject(NullHandler oReader)
        {
            WUStockReport oWUStockReport = new WUStockReport();
            oWUStockReport.FEOID = oReader.GetInt32("FEOID");
            oWUStockReport.FEONo = oReader.GetString("FEONo");
            oWUStockReport.BuyerID = oReader.GetInt32("BuyerID");
            oWUStockReport.FabricID = oReader.GetInt32("FabricID");
            oWUStockReport.OrderType = (EnumOrderType)oReader.GetInt16("OrderType");
            oWUStockReport.IsInHouse = oReader.GetBoolean("IsInHouse");
            oWUStockReport.OrderQty = oReader.GetDouble("OrderQty");
            oWUStockReport.ProcessTypeName = oReader.GetString("ProcessTypeName");
            oWUStockReport.BuyerName = oReader.GetString("BuyerName");
            oWUStockReport.Construction = oReader.GetString("Construction");
            oWUStockReport.CurrentStockQty = oReader.GetDouble("CurrentStockQty");
            oWUStockReport.StoreWiseReceive = oReader.GetString("StoreWiseReceive");
            oWUStockReport.TransferQty = oReader.GetDouble("TransferQty");
            return oWUStockReport;
        }
        private WUStockReport CreateObject(NullHandler oReader)
        {
            WUStockReport oWUStockReport = new WUStockReport();
            oWUStockReport = MapObject(oReader);
            return oWUStockReport;
        }
        private List<WUStockReport> CreateObjects(IDataReader oReader)
        {
            List<WUStockReport> oWUStockReport = new List<WUStockReport>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                WUStockReport oItem = CreateObject(oHandler);
                oWUStockReport.Add(oItem);
            }
            return oWUStockReport;
        }

        #endregion

        #region Interface implementation
        public List<WUStockReport> Gets(short orderType, int processType, string buyerIds, string feoIds, bool bIsCurrentStock, Int64 nUserID)
        {
            List<WUStockReport> oWUStockReports = new List<WUStockReport>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = WUStockReportDA.Gets(tc, orderType, processType, buyerIds, feoIds, bIsCurrentStock);
                oWUStockReports = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oWUStockReports = new List<WUStockReport>();
                WUStockReport oWUStockReport = new WUStockReport();
                oWUStockReport.ErrorMessage = e.Message.Split('~')[0];
                oWUStockReports.Add(oWUStockReport);
                #endregion
            }
            return oWUStockReports;
        }

        #endregion
    }
}
