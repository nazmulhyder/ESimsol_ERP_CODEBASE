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

    public class MerchandiserDashboardService : MarshalByRefObject, IMerchandiserDashboardService
    {
        #region Private functions and declaration
        private MerchandiserDashboard MapObject(NullHandler oReader)
        {
            MerchandiserDashboard oMerchandiserDashboard = new MerchandiserDashboard();
            oMerchandiserDashboard.TechnicalSheetID = oReader.GetInt32("TechnicalSheetID");
            oMerchandiserDashboard.BuyerID = oReader.GetInt32("BuyerID");
            oMerchandiserDashboard.MerchandiserName = oReader.GetString("MerchandiserName");
            oMerchandiserDashboard.StyleNo = oReader.GetString("StyleNo");
            oMerchandiserDashboard.NumberOfCostSheet = oReader.GetInt32("NumberOfCostSheet");
            oMerchandiserDashboard.NumberOfOrderRecap = oReader.GetInt32("NumberOfOrderRecap");
            oMerchandiserDashboard.BuyerName = oReader.GetString("BuyerName");
            oMerchandiserDashboard.SessionName = oReader.GetString("SessionName");
            oMerchandiserDashboard.NumberOfPendingTask = oReader.GetInt32("NumberOfPendingTask");
            oMerchandiserDashboard.NumberOfCompleteTask = oReader.GetInt32("NumberOfCompleteTask");
            oMerchandiserDashboard.NumberOfProductionOrder = oReader.GetInt32("NumberOfProductionOrder");
            oMerchandiserDashboard.NumberOfPEPlan = oReader.GetInt32("NumberOfPEPlan");
            oMerchandiserDashboard.NumberOfPendingTask = oReader.GetInt32("NumberOfPendingTask");
            oMerchandiserDashboard.NumberOfCompleteTask = oReader.GetInt32("NumberOfCompleteTask");
            return oMerchandiserDashboard;
        }

        private MerchandiserDashboard CreateObject(NullHandler oReader)
        {
            MerchandiserDashboard oMerchandiserDashboard = new MerchandiserDashboard();
            oMerchandiserDashboard = MapObject(oReader);
            return oMerchandiserDashboard;
        }

        private List<MerchandiserDashboard> CreateObjects(IDataReader oReader)
        {
            List<MerchandiserDashboard> oMerchandiserDashboard = new List<MerchandiserDashboard>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                MerchandiserDashboard oItem = CreateObject(oHandler);
                oMerchandiserDashboard.Add(oItem);
            }
            return oMerchandiserDashboard;
        }

        #endregion



        #region Interface implementation

        public List<MerchandiserDashboard> Gets(string sMainSQL, string sPOSQL, string sORSQL, string sCSSQL, string sPESQL, string sPendingSQL, string sCompleteSQL)
        {
            List<MerchandiserDashboard> oMerchandiserDashboards = new List<MerchandiserDashboard>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = MerchandiserDashboardDA.Gets(tc, sMainSQL, sPOSQL, sORSQL, sCSSQL, sPESQL, sPendingSQL, sCompleteSQL);
                oMerchandiserDashboards = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Merchandiser Dashboard", e);
                #endregion
            }

            return oMerchandiserDashboards;
        }


        #endregion


    }
    
    
}
