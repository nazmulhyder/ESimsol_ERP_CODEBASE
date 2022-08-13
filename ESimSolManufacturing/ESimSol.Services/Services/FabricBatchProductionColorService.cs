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
    public class FabricBatchProductionColorService : MarshalByRefObject, IFabricBatchProductionColorService
    {
        #region Private functions and declaration
        public static FabricBatchProductionColor MapObject(NullHandler oReader)
        {
            FabricBatchProductionColor oFabricBatchProductionColor = new FabricBatchProductionColor();
            oFabricBatchProductionColor.FBPColorID = oReader.GetInt32("FBPColorID");
            oFabricBatchProductionColor.FBPBID = oReader.GetInt32("FBPBID");
            oFabricBatchProductionColor.Note = oReader.GetString("Note");
            oFabricBatchProductionColor.Qty = oReader.GetDouble("Qty");
            oFabricBatchProductionColor.Name = oReader.GetString("Name");
           
            return oFabricBatchProductionColor;
        }

        public static FabricBatchProductionColor CreateObject(NullHandler oReader)
        {
            FabricBatchProductionColor oFabricBatchProductionColor = new FabricBatchProductionColor();
            oFabricBatchProductionColor = MapObject(oReader);
            return oFabricBatchProductionColor;
        }

        private List<FabricBatchProductionColor> CreateObjects(IDataReader oReader)
        {
            List<FabricBatchProductionColor> oFabricBatchProductionColor = new List<FabricBatchProductionColor>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricBatchProductionColor oItem = CreateObject(oHandler);
                oFabricBatchProductionColor.Add(oItem);
            }
            return oFabricBatchProductionColor;
        }

        #endregion

        public List<FabricBatchProductionColor> Gets(int nFBPBID, Int64 nUserID)
        {
            List<FabricBatchProductionColor> oFabricBatchProductionColors = new List<FabricBatchProductionColor>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricBatchProductionColorDA.Gets(tc, nFBPBID);
                oFabricBatchProductionColors = CreateObjects(reader);
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
            return oFabricBatchProductionColors;
        }

        public FabricBatchProductionColor Save(FabricBatchProductionColor oFabricBatchProductionColor, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {                
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFabricBatchProductionColor.FBPColorID <= 0)
                {
                    reader = FabricBatchProductionColorDA.IUD(tc, EnumDBOperation.Insert, oFabricBatchProductionColor, nUserID);
                }
                else
                {
                    reader = FabricBatchProductionColorDA.IUD(tc, EnumDBOperation.Update, oFabricBatchProductionColor, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricBatchProductionColor = new FabricBatchProductionColor();
                    oFabricBatchProductionColor = CreateObject(oReader);
                }       
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricBatchProductionColor = new FabricBatchProductionColor();
                oFabricBatchProductionColor.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricBatchProductionColor;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricBatchProductionColor oFabricBatchProductionColor = new FabricBatchProductionColor();
                oFabricBatchProductionColor.FBPColorID = id;
                FabricBatchProductionColorDA.Delete(tc,EnumDBOperation.Delete, oFabricBatchProductionColor,nUserId);
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
