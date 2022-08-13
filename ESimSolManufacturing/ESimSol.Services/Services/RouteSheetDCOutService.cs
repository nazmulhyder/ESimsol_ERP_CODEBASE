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
    public class RouteSheetDCOutService : MarshalByRefObject, IRouteSheetDCOutService
    {
        #region Private functions and declaration
        private RouteSheetDCOut MapObject(NullHandler oReader)
        {
            RouteSheetDCOut oRouteSheetDCOut = new RouteSheetDCOut();
            oRouteSheetDCOut.DateTime = (oReader.GetDateTime("DateTime")).ToString("dd MMM yyyy");
            oRouteSheetDCOut.ProductID = oReader.GetInt32("ProductID");
            oRouteSheetDCOut.ProductCode = oReader.GetString("ProductCode");
            oRouteSheetDCOut.ProductName = oReader.GetString("ProductName");
            oRouteSheetDCOut.RouteSheetNo = oReader.GetString("RouteSheetNo");
            oRouteSheetDCOut.MUName = oReader.GetString("MUName");
            oRouteSheetDCOut.Qty = oReader.GetDouble("Qty");
            oRouteSheetDCOut.LotNo = oReader.GetString("LotNo");
            oRouteSheetDCOut.OperationUnitName = oReader.GetString("OperationUnitName");
            oRouteSheetDCOut.LocationName = oReader.GetString("LocationName");
            oRouteSheetDCOut.UserName = oReader.GetString("UserName");
            oRouteSheetDCOut.Shift = oReader.GetString("Shift");

            oRouteSheetDCOut.QtyAdd = oReader.GetDouble("QtyAdd");
            oRouteSheetDCOut.QtyRet = oReader.GetDouble("QtyRet");
            oRouteSheetDCOut.Balance = oReader.GetDouble("Balance");

            oRouteSheetDCOut.IssueDate = oReader.GetDateTime("IssueDate");
            oRouteSheetDCOut.ApprovedDate = oReader.GetDateTime("ApprovedDate");
            oRouteSheetDCOut.InOutType = (EnumInOutType)oReader.GetInt16("InOutType");
            oRouteSheetDCOut.SequenceNo = oReader.GetInt32("SequenceNo");
            oRouteSheetDCOut.IssuedByName = oReader.GetString("IssuedByName");
            oRouteSheetDCOut.ApprovedByName = oReader.GetString("ApprovedByName");
            oRouteSheetDCOut.RouteSheetNo = oReader.GetString("RouteSheetNo");
            oRouteSheetDCOut.ProductCategoryName = oReader.GetString("ProductCategoryName");
            oRouteSheetDCOut.ProductType = (EnumProductNature)oReader.GetInt32("ProductType");
            
            return oRouteSheetDCOut;
        }

        private RouteSheetDCOut CreateObject(NullHandler oReader)
        {
            RouteSheetDCOut oPIReport = new RouteSheetDCOut();
            oPIReport = MapObject(oReader);
            return oPIReport;
        }

        private List<RouteSheetDCOut> CreateObjects(IDataReader oReader)
        {
            List<RouteSheetDCOut> oPIReport = new List<RouteSheetDCOut>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RouteSheetDCOut oItem = CreateObject(oHandler);
                oPIReport.Add(oItem);
            }
            return oPIReport;
        }

        #endregion

        #region Interface implementation
        public RouteSheetDCOutService() { }

        public List<RouteSheetDCOut> Gets(string sSQL, Int64 nUserID)
        {
            List<RouteSheetDCOut> oPIReport = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RouteSheetDCOutDA.Gets(tc, sSQL);
                oPIReport = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get vwForABP", e);
                #endregion
            }

            return oPIReport;
        }


        public List<RouteSheetDCOut> Gets_V2(string sSQL, Int64 nUserID)
        {
            List<RouteSheetDCOut> oPIReport = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RouteSheetDCOutDA.Gets(tc, sSQL);
                oPIReport = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get vwForABP", e);
                #endregion
            }

            return oPIReport;
        }
       
        #endregion
    }
}