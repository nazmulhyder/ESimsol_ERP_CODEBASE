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
    public class FNBatchRawMaterialService : MarshalByRefObject, IFNBatchRawMaterialService
    {
        #region Private functions and declaration
        private FNBatchRawMaterial MapObject(NullHandler oReader)
        {
            FNBatchRawMaterial oFNBatchRawMaterial = new FNBatchRawMaterial();
            oFNBatchRawMaterial.FBRMID = oReader.GetInt32("FBRMID");
            oFNBatchRawMaterial.FNBatchID = oReader.GetInt32("FNBatchID");
            oFNBatchRawMaterial.FabricID = oReader.GetInt32("FabricID");
            oFNBatchRawMaterial.LotID = oReader.GetInt32("LotID");
            oFNBatchRawMaterial.Qty = oReader.GetDouble("Qty");
            oFNBatchRawMaterial.ReceiveBy = oReader.GetInt32("ReceiveBy");
            oFNBatchRawMaterial.LotNo = oReader.GetString("LotNo");
            oFNBatchRawMaterial.FNBatchNo = oReader.GetString("FNBatchNo");
            oFNBatchRawMaterial.ReceiveByName = oReader.GetString("ReceiveByName");
            oFNBatchRawMaterial.FNOrderFabricReceiveID = oReader.GetInt32("FNOrderFabricReceiveID");

            return oFNBatchRawMaterial;
        }

        private FNBatchRawMaterial CreateObject(NullHandler oReader)
        {
            FNBatchRawMaterial oFNBatchRawMaterial = MapObject(oReader);
            return oFNBatchRawMaterial;
        }

        private List<FNBatchRawMaterial> CreateObjects(IDataReader oReader)
        {
            List<FNBatchRawMaterial> oFNBatchRawMaterial = new List<FNBatchRawMaterial>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FNBatchRawMaterial oItem = CreateObject(oHandler);
                oFNBatchRawMaterial.Add(oItem);
            }
            return oFNBatchRawMaterial;
        }

        #endregion

        #region Interface implementation
        public FNBatchRawMaterialService() { }

        public FNBatchRawMaterial FabricOut(FNBatchRawMaterial oFNBatchRawMaterial, Int64 nUserID)
        {
            List<FNBatchRawMaterial> oFNBatchRawMaterials = new List<FNBatchRawMaterial>();
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);

                foreach (FNBatchRawMaterial oItem in oFNBatchRawMaterial.FNBatchRawMaterials)
                {
                    reader = FNBatchRawMaterialDA.FabricOut(tc, oItem, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFNBatchRawMaterial = new FNBatchRawMaterial();
                        oFNBatchRawMaterial = CreateObject(oReader);
                        oFNBatchRawMaterials.Add(oFNBatchRawMaterial);
                    }
                    reader.Close();
                }

                tc.End();
                oFNBatchRawMaterial = new FNBatchRawMaterial();
                oFNBatchRawMaterial.FNBatchRawMaterials = oFNBatchRawMaterials;
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFNBatchRawMaterial = new FNBatchRawMaterial();
                oFNBatchRawMaterial.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }
            return oFNBatchRawMaterial;
        }

        public FNBatchRawMaterial Get(int nFBRMID, Int64 nUserId)
        {
            FNBatchRawMaterial oFNBatchRawMaterial = new FNBatchRawMaterial();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FNBatchRawMaterialDA.Get(tc, nFBRMID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNBatchRawMaterial = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFNBatchRawMaterial = new FNBatchRawMaterial();
                oFNBatchRawMaterial.ErrorMessage = ex.Message;
                #endregion
            }

            return oFNBatchRawMaterial;
        }

        public List<FNBatchRawMaterial> Gets(string sSQL, Int64 nUserID)
        {
            List<FNBatchRawMaterial> oFNBatchRawMaterials = new List<FNBatchRawMaterial>();
            FNBatchRawMaterial oFNBatchRawMaterial = new FNBatchRawMaterial();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FNBatchRawMaterialDA.Gets(tc, sSQL);
                oFNBatchRawMaterials = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFNBatchRawMaterial.ErrorMessage = ex.Message;
                oFNBatchRawMaterials.Add(oFNBatchRawMaterial);
                #endregion
            }

            return oFNBatchRawMaterials;
        }

        #endregion
    }
}
