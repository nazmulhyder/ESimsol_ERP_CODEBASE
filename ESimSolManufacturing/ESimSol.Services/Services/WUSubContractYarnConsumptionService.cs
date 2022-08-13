using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ESimSol.Services.Services
{
    public class WUSubContractYarnConsumptionService : MarshalByRefObject, IWUSubContractYarnConsumptionService
    {
        #region Private functions and declaration
        private WUSubContractYarnConsumption MapObject(NullHandler oReader)
        {
            WUSubContractYarnConsumption oWUSubContractYarnConsumption = new WUSubContractYarnConsumption();
            oWUSubContractYarnConsumption.WUSubContractYarnConsumptionID = oReader.GetInt32("WUSubContractYarnConsumptionID");
            oWUSubContractYarnConsumption.WUSubContractID = oReader.GetInt32("WUSubContractID");
            oWUSubContractYarnConsumption.WarpWeftType = (EnumWarpWeft)oReader.GetInt32("WarpWeftType");
            oWUSubContractYarnConsumption.WarpWeftTypeInt = oReader.GetInt32("WarpWeftType");
            oWUSubContractYarnConsumption.YarnID = oReader.GetInt32("YarnID");
            oWUSubContractYarnConsumption.MUnitID = oReader.GetInt32("MUnitID");
            oWUSubContractYarnConsumption.ConsumptionPerUnit = oReader.GetDouble("ConsumptionPerUnit");
            oWUSubContractYarnConsumption.ConsumptionQty = oReader.GetDouble("ConsumptionQty");
            oWUSubContractYarnConsumption.Remarks = oReader.GetString("Remarks");
            oWUSubContractYarnConsumption.YarnCode = oReader.GetString("YarnCode");
            oWUSubContractYarnConsumption.YarnName = oReader.GetString("YarnName");
            oWUSubContractYarnConsumption.MUSymbol = oReader.GetString("MUSymbol");
            oWUSubContractYarnConsumption.UnitType = oReader.GetString("UnitType");
            oWUSubContractYarnConsumption.YetToChallanQty = oReader.GetDouble("YetToChallanQty");
            return oWUSubContractYarnConsumption;
        }

        private List<WUSubContractYarnConsumption> CreateObjects(IDataReader oReader)
        {
            List<WUSubContractYarnConsumption> oWUSubContractYarnConsumptions = new List<WUSubContractYarnConsumption>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                WUSubContractYarnConsumption oType = CreateObject(oHandler);
                oWUSubContractYarnConsumptions.Add(oType);
            }
            return oWUSubContractYarnConsumptions;
        }

        private WUSubContractYarnConsumption CreateObject(NullHandler oReader)
        {
            WUSubContractYarnConsumption oWUSubContractYarnConsumption = new WUSubContractYarnConsumption();
            oWUSubContractYarnConsumption = MapObject(oReader);
            return oWUSubContractYarnConsumption;
        }
        #endregion

        #region Interface implementation
        public WUSubContractYarnConsumptionService() { }

        public List<WUSubContractYarnConsumption> Gets(int id, Int64 nUserID)
        {
            List<WUSubContractYarnConsumption> oWUSubContractYarnConsumptions = new List<WUSubContractYarnConsumption>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = WUSubContractYarnConsumptionDA.Gets(tc, id);
                oWUSubContractYarnConsumptions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                WUSubContractYarnConsumption oWUSubContractYarnConsumption = new WUSubContractYarnConsumption();
                oWUSubContractYarnConsumption.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }

            return oWUSubContractYarnConsumptions;
        }

        public List<WUSubContractYarnConsumption> Get(string sSQL, int nCurrentUserID)
        {
            List<WUSubContractYarnConsumption> oWUSubContractYarnConsumption = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = WUSubContractYarnConsumptionDA.Get(tc, sSQL);
                oWUSubContractYarnConsumption = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get WUSubContractYarnConsumption", e);
                #endregion
            }

            return oWUSubContractYarnConsumption;
        }

        #endregion
    }
}
