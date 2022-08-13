using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
 

namespace ESimSol.Services.Services
{

    public class SessionWiseRecapSummaryService : MarshalByRefObject, ISessionWiseRecapSummaryService
    {
        #region Private functions and declaration
        private SessionWiseRecapSummary MapObject(NullHandler oReader)
        {
            SessionWiseRecapSummary oSessionWiseRecapSummary = new SessionWiseRecapSummary();
            oSessionWiseRecapSummary.TempID = oReader.GetInt32("TempID");
            oSessionWiseRecapSummary.BuyerID = oReader.GetInt32("BuyerID");
            oSessionWiseRecapSummary.BuyerName = oReader.GetString("BuyerName");
            oSessionWiseRecapSummary.BusinessSessionID = oReader.GetInt32("BusinessSessionID");
            oSessionWiseRecapSummary.StyleCount = oReader.GetInt32("StyleCount");
            oSessionWiseRecapSummary.OrderQty = oReader.GetDouble("OrderQty");
            oSessionWiseRecapSummary.OrderValue = oReader.GetDouble("OrderValue");
            oSessionWiseRecapSummary.ShipmentQty = oReader.GetDouble("ShipmentQty");
            oSessionWiseRecapSummary.ShipmentValue = oReader.GetDouble("ShipmentValue");
            oSessionWiseRecapSummary.EndosmentCommission = oReader.GetDouble("EndosmentCommission");
            oSessionWiseRecapSummary.B2BCommisssion = oReader.GetDouble("B2BCommisssion");
            oSessionWiseRecapSummary.TotalCommission = oReader.GetDouble("TotalCommission");
            return oSessionWiseRecapSummary;
        }

        private SessionWiseRecapSummary CreateObject(NullHandler oReader)
        {
            SessionWiseRecapSummary oSessionWiseRecapSummary = new SessionWiseRecapSummary();
            oSessionWiseRecapSummary = MapObject(oReader);
            return oSessionWiseRecapSummary;
        }

        private List<SessionWiseRecapSummary> CreateObjects(IDataReader oReader)
        {
            List<SessionWiseRecapSummary> oSessionWiseRecapSummary = new List<SessionWiseRecapSummary>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SessionWiseRecapSummary oItem = CreateObject(oHandler);
                oSessionWiseRecapSummary.Add(oItem);
            }
            return oSessionWiseRecapSummary;
        }

        #endregion

        #region Interface implementation
        public List<SessionWiseRecapSummary> Gets(int nSessionID, string sBuyerIDs,  Int64 nUserID)
        {
            List<SessionWiseRecapSummary> oSessionWiseRecapSummary = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SessionWiseRecapSummaryDA.Gets(tc, nSessionID, sBuyerIDs);
                oSessionWiseRecapSummary = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SessionWiseRecapSummary", e);
                #endregion
            }

            return oSessionWiseRecapSummary;
        }

        #endregion
    }   
  
}
