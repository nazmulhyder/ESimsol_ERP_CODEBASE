using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.Services.Services
{
    public class RouteSheetApproveService : MarshalByRefObject, IRouteSheetApproveService
    {
        #region Private functions and declaration
        #region For Product Wise
        private RouteSheetApprove MapObject(NullHandler oReader)
        {
            RouteSheetApprove oRouteSheetApprove = new RouteSheetApprove();
            oRouteSheetApprove.ProductID = oReader.GetInt32("ProductID");
            oRouteSheetApprove.ProductBaseID = oReader.GetInt32("ProductBaseID");
            oRouteSheetApprove.ProductCode = oReader.GetString("ProductCode");
            oRouteSheetApprove.ProductName = oReader.GetString("ProductName");
            oRouteSheetApprove.BaseName = oReader.GetString("BaseName");
            oRouteSheetApprove.MUName = oReader.GetString("MUName");
            oRouteSheetApprove.RSCount = oReader.GetInt32("RSCount");
            oRouteSheetApprove.StockQty = oReader.GetDouble("StockQty");
            oRouteSheetApprove.RSQty = oReader.GetDouble("RSQty");
            oRouteSheetApprove.Qty_Booking = oReader.GetDouble("Qty_Booking");
            return oRouteSheetApprove;
        }
        private RouteSheetApprove CreateObject(NullHandler oReader)
        {
            RouteSheetApprove oPIReport = new RouteSheetApprove();
            oPIReport = MapObject(oReader);
            return oPIReport;
        }
        private List<RouteSheetApprove> CreateObjects(IDataReader oReader)
        {
            List<RouteSheetApprove> oPIReport = new List<RouteSheetApprove>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RouteSheetApprove oItem = CreateObject(oHandler);
                oPIReport.Add(oItem);
            }
            return oPIReport;
        }
        #endregion

        #region For Lot Wise
        private RouteSheetApprove MapObject_Lot(NullHandler oReader)
        {
            RouteSheetApprove oRouteSheetApprove = new RouteSheetApprove();
            oRouteSheetApprove.ProductID = oReader.GetInt32("ProductID");
            oRouteSheetApprove.LotID = oReader.GetInt32("LotID");
            oRouteSheetApprove.LotNo = oReader.GetString("LotNo");
            oRouteSheetApprove.WUnit = oReader.GetString("WUnit");
            oRouteSheetApprove.MUName = oReader.GetString("MUName");
            oRouteSheetApprove.RSCount = oReader.GetInt32("RSCount");
            oRouteSheetApprove.StockQty = oReader.GetDouble("StockQty");
            oRouteSheetApprove.RSQty = oReader.GetDouble("RSQty");
            oRouteSheetApprove.Qty_Booking = oReader.GetDouble("Qty_Booking");
            return oRouteSheetApprove;
        }
        private RouteSheetApprove CreateObject_Lot(NullHandler oReader)
        {
            RouteSheetApprove oPIReport = new RouteSheetApprove();
            oPIReport = MapObject_Lot(oReader);
            return oPIReport;
        }
        private List<RouteSheetApprove> CreateObjects_Lot(IDataReader oReader)
        {
            List<RouteSheetApprove> oPIReport = new List<RouteSheetApprove>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RouteSheetApprove oItem = CreateObject_Lot(oHandler);
                oPIReport.Add(oItem);
            }
            return oPIReport;
        }
        #endregion
        #region For RS Wise
        private RouteSheetApprove MapObject_RS(NullHandler oReader)
        {
            RouteSheetApprove oRouteSheetApprove = new RouteSheetApprove();
            oRouteSheetApprove.ProductID = oReader.GetInt32("ProductID");
            oRouteSheetApprove.LotID = oReader.GetInt32("LotID");
            oRouteSheetApprove.LotNo = oReader.GetString("LotNo");
            oRouteSheetApprove.RouteSheetNo = oReader.GetString("RouteSheetNo");
            oRouteSheetApprove.ProductName = oReader.GetString("ProductName");
            oRouteSheetApprove.RouteSheetDate = oReader.GetDateTime("RouteSheetDate");
            oRouteSheetApprove.RSState = (EnumRSState)oReader.GetInt32("RSState");
            oRouteSheetApprove.WUnit = oReader.GetString("WUnit");
            oRouteSheetApprove.MUName = oReader.GetString("MUName");
            oRouteSheetApprove.RSCount = oReader.GetInt32("RSCount");
            oRouteSheetApprove.StockQty = oReader.GetDouble("StockQty");
            oRouteSheetApprove.Qty = oReader.GetDouble("Qty");
            oRouteSheetApprove.RSQty = oReader.GetDouble("RSQty");
            oRouteSheetApprove.Qty_Booking = oReader.GetDouble("Qty_Booking");
            oRouteSheetApprove.RouteSheetID = oReader.GetInt32("RouteSheetID");
            oRouteSheetApprove.OrderNo = oReader.GetString("OrderNo");
            oRouteSheetApprove.OrderType = oReader.GetInt16("OrderType");
            oRouteSheetApprove.ColorName = oReader.GetString("ColorName");
            return oRouteSheetApprove;
        }
        private RouteSheetApprove CreateObject_RS(NullHandler oReader)
        {
            RouteSheetApprove oPIReport = new RouteSheetApprove();
            oPIReport = MapObject_RS(oReader);
            return oPIReport;
        }
        private List<RouteSheetApprove> CreateObjects_RS(IDataReader oReader)
        {
            List<RouteSheetApprove> oPIReport = new List<RouteSheetApprove>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RouteSheetApprove oItem = CreateObject_RS(oHandler);
                oPIReport.Add(oItem);
            }
            return oPIReport;
        }
        #endregion
        #endregion
      

        #region Interface implementation
        public RouteSheetApproveService() { }
        public RouteSheetApprove Get(int nRouteSheetID, long nUserID)
        {
            RouteSheetApprove oRouteSheet = new RouteSheetApprove();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = RouteSheetApproveDA.Get(tc, nRouteSheetID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRouteSheet = CreateObject_RS(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();

                oRouteSheet.ErrorMessage = e.Message;
                #endregion
            }

            return oRouteSheet;
        }
        public List<RouteSheetApprove> Gets(int nReportType,string sSQL, Int64 nUserID)
        {
            List<RouteSheetApprove> oPIReport = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                
                if (nReportType == 1)// Product Wise
                {
                    reader = RouteSheetApproveDA.Gets(tc, sSQL);
                    oPIReport = CreateObjects(reader);
                }
                else if (nReportType == 2)// Lot Wise
                {
                    reader = RouteSheetApproveDA.Gets(tc, sSQL);
                    oPIReport = CreateObjects_Lot(reader);
                }
                else // RS Wise
                {
                    reader = RouteSheetApproveDA.Gets(tc, sSQL);
                    oPIReport = CreateObjects_RS(reader);
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
                throw new ServiceException("Failed to Get Data", e);
                #endregion
            }

            return oPIReport;
        }

        public List<RouteSheetApprove> Gets_LotWise(int  nProductID, Int64 nUserID)
        {
            List<RouteSheetApprove> oPIReport = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RouteSheetApproveDA.Gets_LotWise(tc, nProductID);
                oPIReport = CreateObjects_Lot(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Data", e);
                #endregion
            }

            return oPIReport;
        }
        public List<RouteSheetApprove> Gets_RSWise( Int64 nUserID)
        {
            List<RouteSheetApprove> oPIReport = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RouteSheetApproveDA.Gets_RSWise(tc);
                oPIReport = CreateObjects_RS(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Data", e);
                #endregion
            }

            return oPIReport;
        }

        #endregion
    }
}