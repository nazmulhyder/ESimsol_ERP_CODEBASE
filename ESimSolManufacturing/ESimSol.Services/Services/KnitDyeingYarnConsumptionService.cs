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
    public class KnitDyeingYarnConsumptionService : MarshalByRefObject, IKnitDyeingYarnConsumptionService
    {
        private KnitDyeingYarnConsumption MapObject(NullHandler oReader)
        {
            KnitDyeingYarnConsumption oKnitDyeingYarnConsumption = new KnitDyeingYarnConsumption();

            oKnitDyeingYarnConsumption.KnitDyeingYarnConsumptionID = oReader.GetInt32("KnitDyeingYarnConsumptionID");
            oKnitDyeingYarnConsumption.KnitDyeingProgramDetailID = oReader.GetInt32("KnitDyeingProgramDetailID");
            oKnitDyeingYarnConsumption.YarnID = oReader.GetInt32("YarnID");
            oKnitDyeingYarnConsumption.UsagesParcent = oReader.GetDouble("UsagesParcent");
            oKnitDyeingYarnConsumption.FinishReqQty = oReader.GetDouble("FinishReqQty");
            oKnitDyeingYarnConsumption.GracePercent = oReader.GetDouble("GracePercent");
            oKnitDyeingYarnConsumption.ReqQty = oReader.GetDouble("ReqQty");
            oKnitDyeingYarnConsumption.MUnitID = oReader.GetInt32("MUnitID");
            oKnitDyeingYarnConsumption.Remarks = oReader.GetString("Remarks");
            oKnitDyeingYarnConsumption.YarnName = oReader.GetString("YarnName");
            oKnitDyeingYarnConsumption.MUnitSymbol = oReader.GetString("UnitSymbol");
            oKnitDyeingYarnConsumption.FabricTypeName = oReader.GetString("FabricTypeName");
            oKnitDyeingYarnConsumption.FabricTypeID = oReader.GetInt32("FabricTypeID");
            return oKnitDyeingYarnConsumption;
        }

        private KnitDyeingYarnConsumption CreateObject(NullHandler oReader)
        {
            KnitDyeingYarnConsumption oKnitDyeingYarnConsumption = new KnitDyeingYarnConsumption();
            oKnitDyeingYarnConsumption = MapObject(oReader);
            return oKnitDyeingYarnConsumption;
        }

        private List<KnitDyeingYarnConsumption> CreateObjects(IDataReader oReader)
        {
            List<KnitDyeingYarnConsumption> oKnitDyeingYarnConsumption = new List<KnitDyeingYarnConsumption>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                KnitDyeingYarnConsumption oItem = CreateObject(oHandler);
                oKnitDyeingYarnConsumption.Add(oItem);
            }
            return oKnitDyeingYarnConsumption;
        }
        public List<KnitDyeingYarnConsumption> Gets(int id, Int64 nUserID)
        {
            List<KnitDyeingYarnConsumption> oKnitDyeingYarnConsumptions = new List<KnitDyeingYarnConsumption>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = KnitDyeingYarnConsumptionDA.Gets(tc, id);
                oKnitDyeingYarnConsumptions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                KnitDyeingYarnConsumption oKnitDyeingYarnConsumption = new KnitDyeingYarnConsumption();
                oKnitDyeingYarnConsumption.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oKnitDyeingYarnConsumptions;
        }
        public List<KnitDyeingYarnConsumption> Gets(string sSQL, Int64 nUserID)
        {
            List<KnitDyeingYarnConsumption> oKnitDyeingYarnConsumptions = new List<KnitDyeingYarnConsumption>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = KnitDyeingYarnConsumptionDA.Gets(tc, sSQL);
                oKnitDyeingYarnConsumptions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                KnitDyeingYarnConsumption oKnitDyeingYarnConsumption = new KnitDyeingYarnConsumption();
                oKnitDyeingYarnConsumption.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oKnitDyeingYarnConsumptions;
        }
    }
}
