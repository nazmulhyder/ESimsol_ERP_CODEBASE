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
    public class FabricBatchRawMaterialService : MarshalByRefObject, IFabricBatchRawMaterialService
    {
        #region Private functions and declaration
        public static FabricBatchRawMaterial MapObject(NullHandler oReader)
        {
            FabricBatchRawMaterial oFabricBatchRawMaterial = new FabricBatchRawMaterial();
            oFabricBatchRawMaterial.WeavingProcess = (EnumWeavingProcess) oReader.GetInt32("WeavingProcess");
            oFabricBatchRawMaterial.ReceiveBy = oReader.GetInt32("ReceiveBy");
            oFabricBatchRawMaterial.ProductID = oReader.GetInt32("ProductID");
            oFabricBatchRawMaterial.ProductName = oReader.GetString("ProductName");
            oFabricBatchRawMaterial.ProductCode = oReader.GetString("ProductCode");
            oFabricBatchRawMaterial.LotID = oReader.GetInt32("LotID");
            oFabricBatchRawMaterial.LotNo = oReader.GetString("LotNo"); 
            oFabricBatchRawMaterial.Qty = oReader.GetDouble("Qty");
            oFabricBatchRawMaterial.FBRMID = oReader.GetInt32("FBRMID");
            oFabricBatchRawMaterial.ReceiveByName = oReader.GetString("ReceiveByName");
            oFabricBatchRawMaterial.FBID = oReader.GetInt32("FBID");
            oFabricBatchRawMaterial.OnlyLotNo = oReader.GetString("OnlyLotNo");
            oFabricBatchRawMaterial.ReceiveByDate = oReader.GetDateTime("ReceiveByDate");
            oFabricBatchRawMaterial.ColorName = oReader.GetString("ColorName");
            oFabricBatchRawMaterial.UnitName = oReader.GetString("UnitName");
            oFabricBatchRawMaterial.StoreName = oReader.GetString("StoreName");
            oFabricBatchRawMaterial.FabricChemicalPlanID = oReader.GetInt32("FabricChemicalPlanID");
            oFabricBatchRawMaterial.ChemicalPlanQty = oReader.GetDouble("ChemicalPlanQty");

            return oFabricBatchRawMaterial;
        }

        public static FabricBatchRawMaterial CreateObject(NullHandler oReader)
        {
            FabricBatchRawMaterial oFabricBatchRawMaterial = new FabricBatchRawMaterial();
            oFabricBatchRawMaterial = MapObject(oReader);
            return oFabricBatchRawMaterial;
        }

        public static List<FabricBatchRawMaterial> CreateObjects(IDataReader oReader)
        {
            List<FabricBatchRawMaterial> oFabricBatchRawMaterial = new List<FabricBatchRawMaterial>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricBatchRawMaterial oItem = CreateObject(oHandler);
                oFabricBatchRawMaterial.Add(oItem);
            }
            return oFabricBatchRawMaterial;
        }

        #endregion

        public List<FabricBatchRawMaterial> Gets(int nFBID, int nWeivingProcess, Int64 nUserID)
        {
            List<FabricBatchRawMaterial> oFabricBatchRawMaterials = new List<FabricBatchRawMaterial>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricBatchRawMaterialDA.Gets(tc, nFBID, nWeivingProcess);
                oFabricBatchRawMaterials = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricBatchRawMaterials = new List<FabricBatchRawMaterial>();
                FabricBatchRawMaterial oFabricBatchRawMaterial = new FabricBatchRawMaterial();
                oFabricBatchRawMaterial.ErrorMessage = e.Message.Split('!')[0];
                oFabricBatchRawMaterials.Add(oFabricBatchRawMaterial);
                #endregion
            }
            return oFabricBatchRawMaterials;
        }
        public List<FabricBatchRawMaterial> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricBatchRawMaterial> oFabricBatchRawMaterials = new List<FabricBatchRawMaterial>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricBatchRawMaterialDA.Gets(tc, sSQL);
                oFabricBatchRawMaterials = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricBatchRawMaterials = new List<FabricBatchRawMaterial>();
                FabricBatchRawMaterial oFabricBatchRawMaterial = new FabricBatchRawMaterial();
                oFabricBatchRawMaterial.ErrorMessage = e.Message.Split('!')[0];
                oFabricBatchRawMaterials.Add(oFabricBatchRawMaterial);
                #endregion
            }
            return oFabricBatchRawMaterials;
        }
        public FabricBatchRawMaterial Save(FabricBatchRawMaterial oFabricBatchRawMaterial, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                List<FabricBatchRawMaterial> oFabricBatchRawMaterials = new List<FabricBatchRawMaterial>();
                List<FabricBatchRawMaterial> oNewFabricBatchRawMaterials = new List<FabricBatchRawMaterial>();
                oFabricBatchRawMaterials = oFabricBatchRawMaterial.FabricBatchRawMaterials;
                oFabricBatchRawMaterial.FabricBatchRawMaterials = new List<FabricBatchRawMaterial>();
                tc = TransactionContext.Begin(true);
                if(oFabricBatchRawMaterial.FabricBatch!=null && oFabricBatchRawMaterial.FabricBatch.FBID>0)
                {

                }
                else
                {
                    if (oFabricBatchRawMaterials.Count > 0)
                    {
                        foreach (FabricBatchRawMaterial oItem in oFabricBatchRawMaterials)
                        {
                            IDataReader reader;
                            if (oItem.FBRMID <= 0)
                            {
                                reader = FabricBatchRawMaterialDA.IUD(tc, EnumDBOperation.Insert, oItem, nUserID);
                            }
                            else
                            {
                                reader = FabricBatchRawMaterialDA.IUD(tc, EnumDBOperation.Update, oItem, nUserID);
                            }
                            NullHandler oReader = new NullHandler(reader);
                            if (reader.Read())
                            {
                                oFabricBatchRawMaterial = new FabricBatchRawMaterial();
                                oFabricBatchRawMaterial = CreateObject(oReader);
                            }
                           oNewFabricBatchRawMaterials .Add(oFabricBatchRawMaterial);
                            reader.Close();
                        }
                        oFabricBatchRawMaterial.FabricBatchRawMaterials = oNewFabricBatchRawMaterials;
                    }
                }
               
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricBatchRawMaterial = new FabricBatchRawMaterial();
                oFabricBatchRawMaterial.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricBatchRawMaterial;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricBatchRawMaterial oFabricBatchRawMaterial = new FabricBatchRawMaterial();
                oFabricBatchRawMaterial.FBRMID = id;
                FabricBatchRawMaterialDA.Delete(tc,EnumDBOperation.Delete, oFabricBatchRawMaterial,nUserId);
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
