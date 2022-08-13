using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class ConsumptionForecastService : MarshalByRefObject, IConsumptionForecastService
    {
        #region Private functions and declaration
        private ConsumptionForecast MapObject(NullHandler oReader)
        {
            ConsumptionForecast oConsumptionForecast = new ConsumptionForecast();

            oConsumptionForecast.ProductID = oReader.GetInt32("ProductID");
            oConsumptionForecast.ProductCode = oReader.GetString("ProductCode");
            oConsumptionForecast.IsBooking = oReader.GetBoolean("IsBooking");
            oConsumptionForecast.ProductName = oReader.GetString("ProductName");
            oConsumptionForecast.BookingMUnitID = oReader.GetInt32("BookingMUnitID");
            oConsumptionForecast.BookingMUSambol = oReader.GetString("BookingMUSambol");
            oConsumptionForecast.BookingMUnitID = oReader.GetInt32("BookingMUnitID");
            oConsumptionForecast.BookingMUSambol = oReader.GetString("BookingMUSambol");
            oConsumptionForecast.BookingQty = oReader.GetDouble("BookingQty");
            oConsumptionForecast.RRMUnitID = oReader.GetInt32("RRMUnitID");
            oConsumptionForecast.RRMUnitQty = oReader.GetDouble("RRMUnitQty");
            oConsumptionForecast.RRMUSambol = oReader.GetString("RRMUSambol");
            oConsumptionForecast.StockUnitID = oReader.GetInt32("StockUnitID");
            oConsumptionForecast.StockQty = oReader.GetDouble("StockQty");
            oConsumptionForecast.ConsumtionQty = oReader.GetDouble("ConsumtionQty");
            oConsumptionForecast.YetToConsumtionQty= oReader.GetDouble("YetToConsumtionQty");
            oConsumptionForecast.StockMUSambol = oReader.GetString("StockMUSambol");

            return oConsumptionForecast;
        }
        private ConsumptionForecast CreateObject(NullHandler oReader)
        {
            ConsumptionForecast oConsumptionForecast = new ConsumptionForecast();
            oConsumptionForecast = MapObject(oReader);
            return oConsumptionForecast;
        }
        private List<ConsumptionForecast> CreateObjects(IDataReader oReader)
        {
            List<ConsumptionForecast> oConsumptionForecast = new List<ConsumptionForecast>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ConsumptionForecast oItem = CreateObject(oHandler);
                oConsumptionForecast.Add(oItem);
            }
            return oConsumptionForecast;
        }
        #endregion

        #region Interface implementation
        public ConsumptionForecastService() { }
        public List<ConsumptionForecast> PrepareConsumptionForecast(ConsumptionForecast oConsumptionForecast, Int64 nUserID)
        {
            List<ConsumptionForecast> oConsumptionForecasts = new List<ConsumptionForecast>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ConsumptionForecastDA.PrepareConsumptionForecast(tc, oConsumptionForecast);
                oConsumptionForecasts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ConsumptionForecast", e);
                #endregion
            }
            return oConsumptionForecasts;
        }
        
        #endregion
    }
}