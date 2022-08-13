using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects.ReportingObject;
using ESimSol.Services.DataAccess.ReportingDA;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    [Serializable]
    public class RptFEOSalesStatementService : MarshalByRefObject, IRptFEOSalesStatementService
    {
        #region Private functions and declaration
        private RptFEOSalesStatement MapObject(NullHandler oReader)
        {
            RptFEOSalesStatement oRptFEOSalesStatement = new RptFEOSalesStatement();
            oRptFEOSalesStatement.OrderDate = oReader.GetDateTime("OrderDate");
            oRptFEOSalesStatement.InHouseExeQty = oReader.GetDouble("InHouseExeQty");
            oRptFEOSalesStatement.OutsideExeQty = oReader.GetDouble("OutsideExeQty");
            oRptFEOSalesStatement.InHouseSalesQty = oReader.GetDouble("InHouseSalesQty");
            oRptFEOSalesStatement.OutsideSalesQty = oReader.GetDouble("OutsideSalesQty");
            oRptFEOSalesStatement.InHouseSalesValue = oReader.GetDouble("InHouseSalesValue");
            oRptFEOSalesStatement.OutsideSalesValue = oReader.GetDouble("OutsideSalesValue");
            oRptFEOSalesStatement.TotalProduction = oReader.GetDouble("TotalProduction");
            return oRptFEOSalesStatement;
        }

        private RptFEOSalesStatement CreateObject(NullHandler oReader)
        {
            RptFEOSalesStatement oRptFEOSalesStatement = new RptFEOSalesStatement();
            oRptFEOSalesStatement = MapObject(oReader);
            return oRptFEOSalesStatement;
        }

        private List<RptFEOSalesStatement> CreateObjects(IDataReader oReader)
        {
            List<RptFEOSalesStatement> oRptFEOSalesStatements = new List<RptFEOSalesStatement>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RptFEOSalesStatement oItem = CreateObject(oHandler);
                oRptFEOSalesStatements.Add(oItem);
            }
            return oRptFEOSalesStatements;
        }
        #endregion

        #region Interface implementation
        public RptFEOSalesStatementService() { }

        public List<RptFEOSalesStatement> Gets(DateTime dtFrom, DateTime dtTo, Int16 nExeType, Int64 nUserId)
        {
            List<RptFEOSalesStatement> oRptFEOSalesStatements = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RptFEOSalesStatementDA.Gets(tc, dtFrom, dtTo, nExeType);
                oRptFEOSalesStatements = CreateObjects(reader);
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
            return oRptFEOSalesStatements;
        }

        #endregion
    }
}
