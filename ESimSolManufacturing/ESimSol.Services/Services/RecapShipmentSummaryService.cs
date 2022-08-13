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

    public class RecapShipmentSummaryService : MarshalByRefObject, IRecapShipmentSummaryService
    {
        #region Private functions and declaration
        private RecapShipmentSummary MapObject(NullHandler oReader)
        {
            RecapShipmentSummary oRecapShipmentSummary = new RecapShipmentSummary();
            oRecapShipmentSummary.BuyerID = oReader.GetInt32("BuyerID");
            oRecapShipmentSummary.BuyerName = oReader.GetString("BuyerName");
            oRecapShipmentSummary.StyleNo = oReader.GetString("StyleNo");
            oRecapShipmentSummary.OrderNo = oReader.GetString("OrderNo");
            oRecapShipmentSummary.CMValue = oReader.GetDouble("CMValue");
            oRecapShipmentSummary.FOBValue = oReader.GetDouble("FOBValue");
            oRecapShipmentSummary.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oRecapShipmentSummary.ShipmentDate = oReader.GetDateTime("ShipmentDate");
            oRecapShipmentSummary.Qty = oReader.GetDouble("Qty");
            oRecapShipmentSummary.DataViewType = oReader.GetInt32("DataViewType");
            oRecapShipmentSummary.ShipmentMonth = oReader.GetInt32("ShipmentMonth");
            oRecapShipmentSummary.NumberOfShipment = oReader.GetInt32("NumberOfShipment");
            oRecapShipmentSummary.ShipmentMonthInString = oReader.GetString("ShipmentMonthInString");
            return oRecapShipmentSummary;
        }

        private RecapShipmentSummary CreateObject(NullHandler oReader)
        {
            RecapShipmentSummary oRecapShipmentSummary = new RecapShipmentSummary();
            oRecapShipmentSummary = MapObject(oReader);
            return oRecapShipmentSummary;
        }

        private List<RecapShipmentSummary> CreateObjects(IDataReader oReader)
        {
            List<RecapShipmentSummary> oRecapShipmentSummary = new List<RecapShipmentSummary>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RecapShipmentSummary oItem = CreateObject(oHandler);
                oRecapShipmentSummary.Add(oItem);
            }
            return oRecapShipmentSummary;
        }

        #endregion

        #region Interface implementation
        public List<RecapShipmentSummary> Gets(string sYear, int BUID, int nUserType,  Int64 nUserID)
        {
            List<RecapShipmentSummary> oRecapShipmentSummary = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RecapShipmentSummaryDA.Gets(tc, sYear, BUID, nUserType, nUserID);
                oRecapShipmentSummary = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RecapShipmentSummary", e);
                #endregion
            }

            return oRecapShipmentSummary;
        }

        #endregion
    }   
    
  
}
