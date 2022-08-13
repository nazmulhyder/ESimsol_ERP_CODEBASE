using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Linq;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class RouteSheetYarnOutService : MarshalByRefObject, IRouteSheetYarnOutService
    {
        #region Private functions and declaration

        private RouteSheetYarnOut MapObject(NullHandler oReader)
        {
            RouteSheetYarnOut oRouteSheetYarnOut = new RouteSheetYarnOut();
            oRouteSheetYarnOut.RouteSheetID = oReader.GetInt32("RouteSheetID");
            oRouteSheetYarnOut.RouteSheetNo = oReader.GetString("RouteSheetNo");
            oRouteSheetYarnOut.ProductID_Raw = oReader.GetInt32("ProductID_Raw");
            oRouteSheetYarnOut.LotID = oReader.GetInt32("LotID");
            oRouteSheetYarnOut.Qty = oReader.GetDouble("Qty");
            oRouteSheetYarnOut.RSState = (EnumRSState)oReader.GetInt16("RSState");
            oRouteSheetYarnOut.EventTime = oReader.GetDateTime("EventTime");
            oRouteSheetYarnOut.ProductCode = oReader.GetString("ProductCode");
            oRouteSheetYarnOut.ProductName = oReader.GetString("ProductName");
            oRouteSheetYarnOut.LotNo = oReader.GetString("LotNo");
            oRouteSheetYarnOut.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oRouteSheetYarnOut.OperationUnitName = oReader.GetString("OperationUnitName");
            oRouteSheetYarnOut.LocationName = oReader.GetString("LocationName");
            oRouteSheetYarnOut.OrderNo = oReader.GetString("OrderNo");
            oRouteSheetYarnOut.UserName = oReader.GetString("UserName");
            oRouteSheetYarnOut.OrderType = oReader.GetInt32("OrderType");
            oRouteSheetYarnOut.NoOfHanksCone = oReader.GetInt32("NoOfHanksCone");
            oRouteSheetYarnOut.DyeingType = oReader.GetString("DyeingType");
            
            return oRouteSheetYarnOut;
        }

        public static RouteSheetYarnOut CreateObject(NullHandler oReader)
        {
            RouteSheetYarnOut oRouteSheetYarnOut = new RouteSheetYarnOut();
            RouteSheetYarnOutService oService = new RouteSheetYarnOutService();
            oRouteSheetYarnOut = oService.MapObject(oReader);
            return oRouteSheetYarnOut;
        }
        private List<RouteSheetYarnOut> CreateObjects(IDataReader oReader)
        {
            List<RouteSheetYarnOut> oRouteSheetYarnOuts = new List<RouteSheetYarnOut>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RouteSheetYarnOut oItem = CreateObject(oHandler);
                oRouteSheetYarnOuts.Add(oItem);
            }
            return oRouteSheetYarnOuts;
        }

        #endregion

        #region Interface implementation
        public RouteSheetYarnOutService() { }

        public List<RouteSheetYarnOut> Gets(string sSQL, long nUserID)
        {
            List<RouteSheetYarnOut> oRouteSheetYarnOuts = new List<BusinessObjects.RouteSheetYarnOut>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RouteSheetYarnOutDA.Gets(tc, sSQL);
                oRouteSheetYarnOuts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                RouteSheetYarnOut oRouteSheetYarnOut = new RouteSheetYarnOut();
                oRouteSheetYarnOut.ErrorMessage = e.Message;
                oRouteSheetYarnOuts.Add(oRouteSheetYarnOut);
                #endregion
            }
            return oRouteSheetYarnOuts;
        }

        #endregion
    }
}