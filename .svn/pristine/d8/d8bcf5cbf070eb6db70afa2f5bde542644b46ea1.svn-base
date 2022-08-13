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
    public class FabricInHouseChallanService : MarshalByRefObject, IFabricInHouseChallanService
    {
        #region Private functions and declaration
        private static FabricInHouseChallan MapObject(NullHandler oReader)
        {
            FabricInHouseChallan oFabricInHouseChallan = new FabricInHouseChallan();
            oFabricInHouseChallan.ChallanID = oReader.GetInt32("ChallanID");
            oFabricInHouseChallan.ChallanNo = oReader.GetString("ChallanNo");
            oFabricInHouseChallan.ChallanDate = oReader.GetDateTime("ChallanDate");
            oFabricInHouseChallan.ContractorID = oReader.GetInt32("ContractorID");
            oFabricInHouseChallan.VehicleNo = oReader.GetString("VehicleNo");
            oFabricInHouseChallan.GatePassNo = oReader.GetString("GatePassNo");
            oFabricInHouseChallan.DeliveredBy = oReader.GetInt32("DeliveredBy");
            oFabricInHouseChallan.DEOID = oReader.GetInt32("DEOID");
            oFabricInHouseChallan.DEONo = oReader.GetString("DEONo");
            oFabricInHouseChallan.FEOID = oReader.GetInt32("FEOID");
            oFabricInHouseChallan.FEONo = oReader.GetString("FEONo");
            oFabricInHouseChallan.DeliveryOrderID = oReader.GetInt32("DeliveryOrderID");
            oFabricInHouseChallan.DeliveryOrderNo = oReader.GetString("DeliveryOrderNo");
            oFabricInHouseChallan.ContractorName = oReader.GetString("ContractorName");
            oFabricInHouseChallan.DeliveredByName = oReader.GetString("DeliveredByName");
            oFabricInHouseChallan.TxtUnit = oReader.GetInt32("TxtUnit");
           oFabricInHouseChallan.IsALLRcv = oReader.GetBoolean("IsALLRcv");
            return oFabricInHouseChallan;
        }

        public static FabricInHouseChallan CreateObject(NullHandler oReader)
        {
            FabricInHouseChallan oFabricInHouseChallan = MapObject(oReader);
            return oFabricInHouseChallan;
        }

        private List<FabricInHouseChallan> CreateObjects(IDataReader oReader)
        {
            List<FabricInHouseChallan> oFabricInHouseChallan = new List<FabricInHouseChallan>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricInHouseChallan oItem = CreateObject(oHandler);
                oFabricInHouseChallan.Add(oItem);
            }
            return oFabricInHouseChallan;
        }

        #endregion

        #region Interface implementation
        public FabricInHouseChallanService() { }
        public List<FabricInHouseChallan> Gets(string sFEONo, string sChallanNo, string BuyerIDs, bool bIsDate, DateTime dtFrom, DateTime dtTo, bool IsAll, int nUnit, Int64 nUserID)
        {
            List<FabricInHouseChallan> oFabricInHouseChallans = new List<FabricInHouseChallan>();
            FabricInHouseChallan oFabricInHouseChallan = new FabricInHouseChallan();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricInHouseChallanDA.Gets(tc, sFEONo, sChallanNo, BuyerIDs, bIsDate, dtFrom, dtTo, IsAll, nUnit);
                oFabricInHouseChallans = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFabricInHouseChallan.ErrorMessage = ex.Message;
                oFabricInHouseChallans.Add(oFabricInHouseChallan);
                #endregion
            }

            return oFabricInHouseChallans;
        }

        #endregion
    }
}
