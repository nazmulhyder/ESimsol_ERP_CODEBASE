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
namespace ESimSol.Services.Services.ReportingService
{
    public class RptGreyFabricStockService :  MarshalByRefObject, IRptGreyFabricStockService
    {
        #region Private functions and declaration
        private RptGreyFabricStock MapObject(NullHandler oReader)
        {
            RptGreyFabricStock oRptGreyFabricStock = new RptGreyFabricStock();
            oRptGreyFabricStock.DispoNo = oReader.GetString("ExeNo");
            oRptGreyFabricStock.CustomerName = oReader.GetString("ContractorName");
            oRptGreyFabricStock.isYD = oReader.GetBoolean("isYD");
            oRptGreyFabricStock.Grade = oReader.GetString("GradeName");
            oRptGreyFabricStock.OpeningQty = oReader.GetDouble("QtyOpening");
            oRptGreyFabricStock.QtyIn = oReader.GetDouble("QtyIn");
            oRptGreyFabricStock.QtyOut = oReader.GetDouble("QtyOut");
            oRptGreyFabricStock.ClosingQty = oReader.GetDouble("QtyClosing");
            oRptGreyFabricStock.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oRptGreyFabricStock.LotID = oReader.GetInt32("LotID");
            oRptGreyFabricStock.SCNo = oReader.GetString("SCNo");
            oRptGreyFabricStock.FabricNo = oReader.GetString("FabricNo");
            oRptGreyFabricStock.ReviseNo = oReader.GetInt32("ReviseNo");
            oRptGreyFabricStock.OrderType = oReader.GetInt32("OrderType");
            oRptGreyFabricStock.OrderTypeSt = oReader.GetString("OrderTypeSt");
            oRptGreyFabricStock.Construction = oReader.GetString("Construction");
            oRptGreyFabricStock.ProcessTypeName = oReader.GetString("ProcessTypeName");
            oRptGreyFabricStock.ColorInfo = oReader.GetString("ColorInfo");
            oRptGreyFabricStock.StyleNo = oReader.GetString("StyleNo");
            oRptGreyFabricStock.ProcessType = oReader.GetInt32("ProcessType");
            return oRptGreyFabricStock;
        }
        private RptGreyFabricStock CreateObject(NullHandler oReader)
        {
            RptGreyFabricStock oRptGreyFabricStock = new RptGreyFabricStock();
            oRptGreyFabricStock = MapObject(oReader);
            return oRptGreyFabricStock;
        }

        private List<RptGreyFabricStock> CreateObjects(IDataReader oReader)
        {
            List<RptGreyFabricStock> oRptGreyFabricStocks = new List<RptGreyFabricStock>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RptGreyFabricStock oItem = CreateObject(oHandler);
                oRptGreyFabricStocks.Add(oItem);
            }
            return oRptGreyFabricStocks;
        }
        #endregion

          #region Interface implementation
        public RptGreyFabricStockService() { }

        public List<RptGreyFabricStock> Gets(string sSQL, DateTime StartTime, DateTime EndTime,int ReportType,int StoreID, Int64 nUserId)
        {
            List<RptGreyFabricStock> oRptGreyFabricStocks = new List<RptGreyFabricStock>() ;
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RptGreyFabricStockDA.Gets(tc, sSQL, StartTime, EndTime, ReportType, StoreID);
                oRptGreyFabricStocks = CreateObjects(reader);
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
            return oRptGreyFabricStocks;
        }
        #endregion
    }
}
