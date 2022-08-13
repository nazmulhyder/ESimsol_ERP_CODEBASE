using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects.ReportingObject;
using ESimSol.Services.DataAccess.ReportingDA;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
using ESimSol.BusinessObjects;

namespace ESimSol.Services.Services
{
    [Serializable]
    public class RptFEOSalesSummaryService : MarshalByRefObject, IRptFEOSalesSummaryService
    {
        #region Private functions and declaration
        private RptFEOSalesSummary MapObject(NullHandler oReader)
        {
            RptFEOSalesSummary oRptFEOSalesSummary = new RptFEOSalesSummary();
            oRptFEOSalesSummary.MktPersonID = oReader.GetInt32("MktPersonID");
            oRptFEOSalesSummary.BuyerID = oReader.GetInt32("BuyerID");
            oRptFEOSalesSummary.OrderType = (EnumOrderType)oReader.GetInt16("OrderType");
            oRptFEOSalesSummary.OrderDate = oReader.GetDateTime("OrderDate");
            oRptFEOSalesSummary.YarnDyedQty = oReader.GetDouble("YarnDyedQty");
            oRptFEOSalesSummary.SolidDyedQty = oReader.GetDouble("SolidDyedQty");
            oRptFEOSalesSummary.YarnDyedValue = oReader.GetDouble("YarnDyedValue");
            oRptFEOSalesSummary.SolidDyedValue = oReader.GetDouble("SolidDyedValue");
            oRptFEOSalesSummary.BuyerName = oReader.GetString("BuyerName");
            oRptFEOSalesSummary.MktPersonName = oReader.GetString("MktPersonName");
            return oRptFEOSalesSummary;
        }

        private RptFEOSalesSummary CreateObject(NullHandler oReader)
        {
            RptFEOSalesSummary oRptFEOSalesSummary = new RptFEOSalesSummary();
            oRptFEOSalesSummary = MapObject(oReader);
            return oRptFEOSalesSummary;
        }

        private List<RptFEOSalesSummary> CreateObjects(IDataReader oReader)
        {
            List<RptFEOSalesSummary> oRptFEOSalesSummarys = new List<RptFEOSalesSummary>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RptFEOSalesSummary oItem = CreateObject(oHandler);
                oRptFEOSalesSummarys.Add(oItem);
            }
            return oRptFEOSalesSummarys;
        }
        #endregion

        #region Interface implementation
        public RptFEOSalesSummaryService() { }

        public List<RptFEOSalesSummary> Gets(Int16 nOrderType, DateTime dtFrom, DateTime dtTo, bool bIsBuyerWise, Int16 nExeType, bool bIsOrderTypeWise, Int64 nUserId)
        {
            List<RptFEOSalesSummary> oRptFEOSalesSummarys = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RptFEOSalesSummaryDA.Gets(tc, nOrderType, dtFrom, dtTo, bIsBuyerWise, nExeType, bIsOrderTypeWise);
                oRptFEOSalesSummarys = CreateObjects(reader);
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
            return oRptFEOSalesSummarys;
        }

        #endregion
    }
}
