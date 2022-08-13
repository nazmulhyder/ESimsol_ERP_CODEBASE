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
    public class FabricBatchService : MarshalByRefObject, IFabricBatchService
    {
        #region Private functions and declaration
        private FabricBatch MapObject(NullHandler oReader)
        {
            FabricBatch oFabricBatch = new FabricBatch();
            oFabricBatch.FBID = oReader.GetInt32("FBID");
            oFabricBatch.BatchNo = oReader.GetString("BatchNo");
            oFabricBatch.FabricSalesContractDetailID = oReader.GetInt32("FEOID");
            oFabricBatch.StyleNo = oReader.GetString("StyleNo");
            oFabricBatch.Qty = oReader.GetDouble("Qty");
            oFabricBatch.Status = (EnumFabricBatchState)oReader.GetInt32("Status");
            oFabricBatch.StatusInInt = oReader.GetInt32("Status");
            oFabricBatch.IssueDate = oReader.GetDateTime("IssueDate");
            oFabricBatch.TotalEnds = oReader.GetDouble("TotalEnds");
            oFabricBatch.NoOfSection = oReader.GetInt32("NoOfSection");
            oFabricBatch.WarpCount = oReader.GetDouble("WarpCount");
            oFabricBatch.FEONo = oReader.GetString("FEONo");
            oFabricBatch.PONo = oReader.GetString("PONo");
            oFabricBatch.BuyerName = oReader.GetString("BuyerName");
            oFabricBatch.OrderType = (EnumOrderType)oReader.GetInt32("OrderType");
            oFabricBatch.NoOfWarp = oReader.GetInt32("NoOfWarp");
            oFabricBatch.NoOfWeft = oReader.GetInt32("NoOfWeft");
            oFabricBatch.Construction = oReader.GetString("Construction");
            oFabricBatch.OrderQty = oReader.GetDouble("OrderQty");
            oFabricBatch.QtyPro = oReader.GetDouble("QtyPro");
            oFabricBatch.FEOSID = oReader.GetInt32("FEOSID");
            oFabricBatch.IsInHouse = oReader.GetBoolean("IsInHouse");
            oFabricBatch.FinishType = oReader.GetString("FinishType");
            //oFabricBatch.FBLastBeamNo = oReader.GetString("FBLastBeamNo");
            oFabricBatch.FWPDID = oReader.GetInt32("FWPDID");
            oFabricBatch.FMID = oReader.GetInt32("FMID");
            oFabricBatch.BuyerRef = oReader.GetString("BuyerRef");
            oFabricBatch.Color = oReader.GetString("Color");
            oFabricBatch.ProcessTypeName = oReader.GetString("ProcessTypeName");
            oFabricBatch.Weave = oReader.GetString("Weave");
            oFabricBatch.Width = oReader.GetString("Width");
            oFabricBatch.ProductName = oReader.GetString("ProductName");
            oFabricBatch.IsYarnDyed = oReader.GetBoolean("IsYarnDyed");
            oFabricBatch.FWPD_Length = oReader.GetDouble("FWPD_Length");
            oFabricBatch.WarpingStartTime = oReader.GetDateTime("WarpingStartTime");
            oFabricBatch.SizingStartTime = oReader.GetDateTime("SizingStartTime");
            oFabricBatch.DrawingStartTime = oReader.GetDateTime("DrawingStartTime");
            oFabricBatch.LoomStartTime = oReader.GetDateTime("LoomStartTime");
            oFabricBatch.ContractionPercentage = oReader.GetDouble("ContractionPercentage");
            oFabricBatch.WarpingMachineCode = oReader.GetString("WarpingMachineCode");
            oFabricBatch.WarpingMachineStatus = (EnumMachineStatus)oReader.GetInt32("WarpingMachineStatus");
            oFabricBatch.WMCode = oReader.GetString("WMCode");
            oFabricBatch.WarpBeam = oReader.GetString("WarpBeam");
            oFabricBatch.ReedCount = oReader.GetDouble("ReedCount");
            oFabricBatch.PlanType = (EnumPlanType)oReader.GetInt32("PlanType");
            oFabricBatch.FSpcType = (EnumFabricSpeType)oReader.GetInt32("FSpcType");

            return oFabricBatch;
        }
        private FabricBatch CreateObject(NullHandler oReader)
        {
            FabricBatch oFabricBatch = new FabricBatch();
            oFabricBatch = MapObject(oReader);
            return oFabricBatch;
        }
        private List<FabricBatch> CreateObjects(IDataReader oReader)
        {
            List<FabricBatch> oFabricBatch = new List<FabricBatch>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricBatch oItem = CreateObject(oHandler);
                oFabricBatch.Add(oItem);
            }
            return oFabricBatch;
        }
        #endregion

        #region Interface implementation
        public FabricBatch Save(FabricBatch oFabricBatch, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFabricBatch.FBID <= 0)
                {
                    reader = FabricBatchDA.InsertUpdate(tc, oFabricBatch, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = FabricBatchDA.InsertUpdate(tc, oFabricBatch, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricBatch = new FabricBatch();
                    oFabricBatch = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricBatch = new FabricBatch();
                oFabricBatch.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricBatch;
        }
        public FabricBatch Finish(FabricBatch oFabricBatch, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = FabricBatchDA.Finish(tc, oFabricBatch);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricBatch = new FabricBatch();
                    oFabricBatch = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricBatch = new FabricBatch();
                oFabricBatch.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricBatch;
        }

        public FabricBatch BatchFinish(FabricBatch oFabricBatch, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = FabricBatchDA.BatchFinish(tc, oFabricBatch);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricBatch = new FabricBatch();
                    oFabricBatch = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricBatch = new FabricBatch();
                oFabricBatch.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricBatch;
        }

        
        public FabricBatch RowMatarialOut(FabricBatch oFabricBatch, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                List<FabricBatchRawMaterial> oFabricBatchRawMaterials = new List<FabricBatchRawMaterial>();
                List<FabricBatchRawMaterial> oNewFabricBatchRawMaterials = new List<FabricBatchRawMaterial>();
                oFabricBatchRawMaterials = oFabricBatch.FabricBatchRawMaterials;
                FabricBatchRawMaterial oFabricBatchRawMaterial = new FabricBatchRawMaterial();
                bool IsChemicalOut = false;
                int nWeavingProcess= (int)oFabricBatch.WeavingProcess;
                if (!oFabricBatch.IsYarn)
                {
                    IsChemicalOut = true;
                }
                tc = TransactionContext.Begin(true);
                if (oFabricBatch.IsRawMaterialOut)
                {
                    foreach (FabricBatchRawMaterial oItem in oFabricBatchRawMaterials)
                    {
                        IDataReader reader;
                        if(oItem.ReceiveBy==0)
                        {                            
                            oItem.FBID = oFabricBatch.FBID;
                           if(oFabricBatch.IsYarn)
                           {
                               reader = FabricBatchRawMaterialDA.YarnOut(tc, oItem,oFabricBatch.WeavingProcess, nUserID);
                           }else
                           {
                               reader = FabricBatchRawMaterialDA.ChemicalOut(tc, oItem, nUserID);
                           }
                            NullHandler oReader = new NullHandler(reader);
                            if (reader.Read())
                            {
                            }
                            reader.Close();
                        }
                        
                    }
                    IDataReader fabreader = FabricBatchDA.Get(tc, oFabricBatch.FBID);
                    NullHandler oFbReader = new NullHandler(fabreader);
                    if (fabreader.Read())
                    {
                        oFabricBatch = CreateObject(oFbReader);
                    }
                    fabreader.Close();
                    oNewFabricBatchRawMaterials = new List<FabricBatchRawMaterial>();
                    IDataReader readerdetail = null;
                    if (!IsChemicalOut)
                    {
                        readerdetail = FabricBatchRawMaterialDA.Gets(tc, oFabricBatch.FBID, nWeavingProcess);//yarn
                    }
                    else
                    {
                        readerdetail = FabricBatchRawMaterialDA.Gets(tc, oFabricBatch.FBID, (int)EnumWeavingProcess.Sizing);//for chemical
                    }
                    oFabricBatch.FabricBatchRawMaterials = FabricBatchRawMaterialService.CreateObjects(readerdetail);
                    readerdetail.Close();
                    oFabricBatch.IsRawMaterialOut = true;
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
                                oFabricBatchRawMaterial = FabricBatchRawMaterialService.CreateObject(oReader);
                            }
                            oNewFabricBatchRawMaterials.Add(oFabricBatchRawMaterial);
                            reader.Close();
                        }
                        oFabricBatch.FabricBatchRawMaterials = oNewFabricBatchRawMaterials;
                    }
                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricBatch = new FabricBatch();
                oFabricBatch.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricBatch;
        }


        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricBatch oFabricBatch = new FabricBatch();
                oFabricBatch.FBID = id;
                FabricBatchDA.Delete(tc, oFabricBatch, EnumDBOperation.Delete, nUserId);
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
        public FabricBatch Get(int id, Int64 nUserId)
        {
            FabricBatch oFabricBatch = new FabricBatch();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricBatchDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricBatch = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricBatch = new FabricBatch();
                oFabricBatch.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricBatch;
        }
        public List<FabricBatch> Gets(Int64 nUserID)
        {
            List<FabricBatch> oFabricBatchs = new List<FabricBatch>(); ;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricBatchDA.Gets(tc);
                oFabricBatchs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FabricBatch oFabricBatch = new FabricBatch();
                oFabricBatch.ErrorMessage = e.Message.Split('!')[0];
                oFabricBatchs.Add(oFabricBatch);
                #endregion
            }
            return oFabricBatchs;
        }
        public List<FabricBatch> GetsByFabricSalesContractDetailID(int nFabricSalesContractDetailID, Int64 nUserID)
        {
            List<FabricBatch> oFabricBatchs = new List<FabricBatch>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricBatchDA.GetsByFabricSalesContractDetailID(tc, nFabricSalesContractDetailID);
                oFabricBatchs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FabricBatch oFabricBatch = new FabricBatch();
                oFabricBatch.ErrorMessage = e.Message.Split('!')[0];
                oFabricBatchs.Add(oFabricBatch);
                #endregion
            }
            return oFabricBatchs;
        }
        public List<FabricBatch> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricBatch> oFabricBatchs = new List<FabricBatch>(); ;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricBatchDA.Gets(tc, sSQL);
                oFabricBatchs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FabricBatch oFabricBatch = new FabricBatch();
                oFabricBatch.ErrorMessage = e.Message.Split('!')[0];
                oFabricBatchs.Add(oFabricBatch);
                #endregion
            }
            return oFabricBatchs;
        }

        public FabricBatch GetByBatchNo(string sBatchNo, Int64 nUserId)
        {
            FabricBatch oFabricBatch = new FabricBatch();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricBatchDA.GetByBatchNo(tc, sBatchNo);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricBatch = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricBatch = new FabricBatch();
                oFabricBatch.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricBatch;
        }

        public FabricBatch FabricProductionQCDone(FabricBatch oFabricBatch, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = FabricBatchDA.FabricProductionQCDone(tc, oFabricBatch, EnumDBOperation.Insert, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricBatch = new FabricBatch();
                    oFabricBatch = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricBatch = new FabricBatch();
                oFabricBatch.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricBatch;
        }
        public FabricBatch UpdateBatchNo(FabricBatch oFabricBatch, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = FabricBatchDA.UpdateBatchNo(tc, oFabricBatch, nUserID, EnumDBOperation.Insert);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricBatch = new FabricBatch();
                    oFabricBatch = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricBatch.ErrorMessage = e.Message;
                oFabricBatch.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricBatch;
        }
        #endregion
    }
}
