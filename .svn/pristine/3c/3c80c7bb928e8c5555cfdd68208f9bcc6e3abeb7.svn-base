using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;
namespace ESimSol.Services.Services
{
    public class DUProductionStatusReportService : MarshalByRefObject, IDUProductionStatusReportService
    {
        #region Private functions and declaration
        private DUProductionStatusReport MapObject(NullHandler oReader)
        {
            DUProductionStatusReport oDUProductionStatusReport = new DUProductionStatusReport();
            oDUProductionStatusReport.DUProGuideLineID = oReader.GetInt32("DUProGuideLineID");
            oDUProductionStatusReport.DyingOrderID = oReader.GetInt32("DyeingOrderID");
            oDUProductionStatusReport.ProductID = oReader.GetInt32("ProductID");
            oDUProductionStatusReport.OrderNo = oReader.GetString("OrderNo");
            oDUProductionStatusReport.OrderType = oReader.GetInt32("OrderType");
            oDUProductionStatusReport.OrderDate = oReader.GetDateTime("OrderDate");
            oDUProductionStatusReport.BuyerName = oReader.GetString("BuyerName");
            oDUProductionStatusReport.BuyerRef = oReader.GetString("BuyerRef");
            oDUProductionStatusReport.ProductName = oReader.GetString("ProductName");
            oDUProductionStatusReport.OrderName = oReader.GetString("OrderName");
            oDUProductionStatusReport.YarnLotNo = oReader.GetString("YarnLotNo");
            oDUProductionStatusReport.Remarks = oReader.GetString("Remarks");
            oDUProductionStatusReport.MUnit = oReader.GetString("MUnit");
            oDUProductionStatusReport.StyleNo = oReader.GetString("StyleNo");
            oDUProductionStatusReport.Qty_Order = oReader.GetDouble("Qty_Order");
            oDUProductionStatusReport.Qty_Rcv = oReader.GetDouble("Qty_Rcv");
            oDUProductionStatusReport.Qty_YetToRcv = oReader.GetDouble("Qty_YetToRcv");
            oDUProductionStatusReport.Qty_SW = oReader.GetDouble("Qty_SW");
            oDUProductionStatusReport.Qty_YetToSW = oReader.GetDouble("Qty_YetToSW");
            oDUProductionStatusReport.Qty_Return = oReader.GetDouble("Qty_Return");
            oDUProductionStatusReport.Qty_Transfer_In = oReader.GetDouble("Qty_Transfer_In");
            oDUProductionStatusReport.Qty_Transfer_Out = oReader.GetDouble("Qty_Transfer_Out");
            
            return oDUProductionStatusReport;
        }

        private DUProductionStatusReport CreateObject(NullHandler oReader)
        {
            DUProductionStatusReport oDUProductionStatusReport = new DUProductionStatusReport();
            oDUProductionStatusReport = MapObject(oReader);
            return oDUProductionStatusReport;
        }

        private List<DUProductionStatusReport> CreateObjects(IDataReader oReader)
        {
            List<DUProductionStatusReport> oDUProductionStatusReport = new List<DUProductionStatusReport>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DUProductionStatusReport oItem = CreateObject(oHandler);
                oDUProductionStatusReport.Add(oItem);
            }
            return oDUProductionStatusReport;
        }

        #endregion

        #region Interface implementation
        public DUProductionStatusReportService() { }
        public List<DUProductionStatusReport> Gets(string sSQL, EnumReportLayout eEnumReportLayout, long nUserID)
        {
            List<DUProductionStatusReport> oDUProductionStatusReports = new List<DUProductionStatusReport>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = DUProductionStatusReportDA.Gets(tc, eEnumReportLayout, sSQL);
                oDUProductionStatusReports = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUProductionStatusReport", e);
                #endregion
            }

            return oDUProductionStatusReports;
        }
        #endregion
    }   
}
