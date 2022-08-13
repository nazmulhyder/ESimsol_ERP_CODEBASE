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
    public class FabricBatchProductionBreakageService : MarshalByRefObject, IFabricBatchProductionBreakageService
    {
        #region Private functions and declaration
        public static FabricBatchProductionBreakage MapObject(NullHandler oReader)
        {
            FabricBatchProductionBreakage oFabricBatchProductionBreakage = new FabricBatchProductionBreakage();
            oFabricBatchProductionBreakage.FBreakageWeavingProcess = (EnumWeavingProcess)oReader.GetInt32("FBreakageWeavingProcess");
            oFabricBatchProductionBreakage.FBreakageID = oReader.GetInt32("FBreakageID");
            oFabricBatchProductionBreakage.FabricBreakageName = oReader.GetString("FabricBreakageName");
            oFabricBatchProductionBreakage.DurationInMin = oReader.GetInt32("DurationInMin");
            oFabricBatchProductionBreakage.Note = oReader.GetString("Note");
            oFabricBatchProductionBreakage.NoOfBreakage = oReader.GetInt32("NoOfBreakage");
            oFabricBatchProductionBreakage.FBLDetailID = oReader.GetInt32("FBLDetailID");
            oFabricBatchProductionBreakage.FBPBreakageID = oReader.GetInt32("FBPBreakageID");
            return oFabricBatchProductionBreakage;
        }

        public static FabricBatchProductionBreakage CreateObject(NullHandler oReader)
        {
            FabricBatchProductionBreakage oFabricBatchProductionBreakage = new FabricBatchProductionBreakage();
            oFabricBatchProductionBreakage = MapObject(oReader);
            return oFabricBatchProductionBreakage;
        }

        private List<FabricBatchProductionBreakage> CreateObjects(IDataReader oReader)
        {
            List<FabricBatchProductionBreakage> oFabricBatchProductionBreakage = new List<FabricBatchProductionBreakage>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricBatchProductionBreakage oItem = CreateObject(oHandler);
                oFabricBatchProductionBreakage.Add(oItem);
            }
            return oFabricBatchProductionBreakage;
        }

        #endregion
        public List<FabricBatchProductionBreakage> Gets(int nFBPBID, Int64 nUserID)
        {
            List<FabricBatchProductionBreakage> oFabricBatchProductionBreakages = new List<FabricBatchProductionBreakage>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricBatchProductionBreakageDA.Gets(tc, nFBPBID);
                oFabricBatchProductionBreakages = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Fabric Yarn Out", e);
                #endregion
            }
            return oFabricBatchProductionBreakages;
        }
        public FabricBatchProductionBreakage Save(FabricBatchProductionBreakage oFabricBatchProductionBreakage, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
               
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFabricBatchProductionBreakage.FBPBreakageID <= 0)
                {
                    reader = FabricBatchProductionBreakageDA.IUD(tc, EnumDBOperation.Insert, oFabricBatchProductionBreakage, nUserID);
                }
                else
                {
                    reader = FabricBatchProductionBreakageDA.IUD(tc, EnumDBOperation.Update, oFabricBatchProductionBreakage, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricBatchProductionBreakage = new FabricBatchProductionBreakage();
                    oFabricBatchProductionBreakage = CreateObject(oReader);
                }
                reader.Close();        
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricBatchProductionBreakage = new FabricBatchProductionBreakage();
                oFabricBatchProductionBreakage.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricBatchProductionBreakage;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricBatchProductionBreakage oFabricBatchProductionBreakage = new FabricBatchProductionBreakage();
                oFabricBatchProductionBreakage.FBPBreakageID = id;
                FabricBatchProductionBreakageDA.Delete(tc,EnumDBOperation.Delete, oFabricBatchProductionBreakage,nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }
    }
    
  
}
