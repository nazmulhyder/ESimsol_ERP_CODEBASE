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
using ESimSol.BusinessObjects;

namespace ESimSol.Services.Services
{
    [Serializable]
    public class RptDUOrderStatusService : MarshalByRefObject, IRptDUOrderStatusService
    {
        #region Private functions and declaration
        private RptDUOrderStatus MapObject(NullHandler oReader)
        {
            RptDUOrderStatus oRptDUOrderStatus = new RptDUOrderStatus();
            oRptDUOrderStatus.DyeingOrderID = oReader.GetInt32("DyeingOrderID");
            oRptDUOrderStatus.ProductID = oReader.GetInt32("ProductID");
            oRptDUOrderStatus.ProductBaseID = oReader.GetInt32("ProductBaseID");
            oRptDUOrderStatus.ProductCategoryID = oReader.GetInt32("ProductCategoryID");
            oRptDUOrderStatus.ProductName = oReader.GetString("ProductName");
            oRptDUOrderStatus.ProductName = oReader.GetString("ProductName");
            oRptDUOrderStatus.ProductCode = oReader.GetString("ProductCode");
            oRptDUOrderStatus.CategoryName = oReader.GetString("CategoryName");
            oRptDUOrderStatus.ProductBaseName = oReader.GetString("ProductBaseName");
            oRptDUOrderStatus.Startdate = oReader.GetDateTime("Startdate");
            oRptDUOrderStatus.Enddate = oReader.GetDateTime("Enddate");
            oRptDUOrderStatus.OrderQty = oReader.GetDouble("OrderQty");
            oRptDUOrderStatus.SRSQty = oReader.GetDouble("SRSQty");
            oRptDUOrderStatus.SRMQty = oReader.GetDouble("SRMQty");
            oRptDUOrderStatus.YarnOut = oReader.GetDouble("YarnOut");
            oRptDUOrderStatus.QtyQC = oReader.GetDouble("QtyQC");
            oRptDUOrderStatus.QtyDC = oReader.GetDouble("QtyDC");
            oRptDUOrderStatus.QtyRC = oReader.GetDouble("QtyRC");
            oRptDUOrderStatus.OrderType = oReader.GetInt32("OrderType");
            oRptDUOrderStatus.QtyDyeing = oReader.GetDouble("QtyDyeing");
            oRptDUOrderStatus.Qty_Hydro = oReader.GetDouble("Qty_Hydro");
            oRptDUOrderStatus.Qty_Drier = oReader.GetDouble("Qty_Drier");
            oRptDUOrderStatus.Qty_WQC = oReader.GetDouble("Qty_WQC");
            oRptDUOrderStatus.YarnReceive = oReader.GetDouble("YarnReceive");
            
            return oRptDUOrderStatus;
        }

        private RptDUOrderStatus CreateObject(NullHandler oReader)
        {
            RptDUOrderStatus oRptDUOrderStatus = new RptDUOrderStatus();
            oRptDUOrderStatus = MapObject(oReader);
            return oRptDUOrderStatus;
        }

        private List<RptDUOrderStatus> CreateObjects(IDataReader oReader)
        {
            List<RptDUOrderStatus> oRptDUOrderStatuss = new List<RptDUOrderStatus>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RptDUOrderStatus oItem = CreateObject(oHandler);
                oRptDUOrderStatuss.Add(oItem);
            }
            return oRptDUOrderStatuss;
        }
        #endregion

        #region Interface implementation
        public RptDUOrderStatusService() { }

        public List<RptDUOrderStatus> MailContent(string ProductIDs, int nReportType, DateTime StartTime, DateTime EndTime, int BUID, long nUserID)
        {
            List<RptDUOrderStatus> oRptDUOrderStatuss = new List<RptDUOrderStatus>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RptDUOrderStatusDA.MailContent(tc, ProductIDs, nReportType, StartTime, EndTime, BUID, nUserID);
                oRptDUOrderStatuss = CreateObjects(reader);
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
            return oRptDUOrderStatuss;
        }

        public DataSet Gets(string sSql, Int64 nUserID)
        {
            TransactionContext tc = null;
            DataSet oDataSet = new DataSet();
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = RptDUOrderStatusDA.GetsGroupBy(tc, sSql);
                oDataSet.Load(reader, LoadOption.OverwriteChanges, new string[3]);
                reader.Close();
                tc.End();

            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                #endregion
            }
            return oDataSet;
        }



        #endregion
    }
}
