using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
using System.Linq;


namespace ESimSol.Services.Services
{
    [Serializable]
    public class FabricExecutionOrderService : MarshalByRefObject, IFabricExecutionOrderService
    {
        #region Private functions and declaration
        private FabricExecutionOrder MapObject(NullHandler oReader)
        {
            FabricExecutionOrder oFEO = new FabricExecutionOrder();
            oFEO.FEOID = oReader.GetInt32("FEOID");
            oFEO.FEONo = oReader.GetString("FEONo");
            oFEO.FabricID = oReader.GetInt32("FabricID");
            oFEO.ProductID = oReader.GetInt32("ProductID");
            oFEO.OrderType = (EnumOrderType)oReader.GetInt16("OrderType");
            oFEO.BuyerID = oReader.GetInt32("BuyerID");
            oFEO.ContractorPersonalID = oReader.GetInt32("ContractorPersonalID");
            oFEO.StyleNo = oReader.GetString("StyleNo");
            oFEO.MktPersonID = oReader.GetInt32("MktPersonID");
            oFEO.OrderRef = oReader.GetString("OrderRef");
            oFEO.BuyerRef = oReader.GetString("BuyerRef");
            oFEO.Process = oReader.GetString("Process");
            oFEO.Emirzing = oReader.GetString("Emirzing");
            oFEO.LightSourceDes = oReader.GetString("LightSourceDes");
            oFEO.ReqFinishedGSM = oReader.GetDouble("ReqFinishedGSM");
            oFEO.GarmentWash = oReader.GetString("GarmentWash");
            oFEO.TestStandard = oReader.GetString("TestStandard");
            oFEO.FinalInspection = oReader.GetString("FinalInspection");
            oFEO.FinishWidth = oReader.GetString("FinishWidth");
            oFEO.CW = oReader.GetString("CW");
            oFEO.EndUse = oReader.GetString("EndUse");
            oFEO.Qty = oReader.GetDouble("Qty");
            oFEO.PPSampleQty = oReader.GetDouble("PPSampleQty");
            oFEO.TestSampleQty = oReader.GetDouble("TestSampleQty");
            oFEO.Note = oReader.GetString("Note");
            oFEO.OrderDate = oReader.GetDateTime("OrderDate");
            oFEO.ExpectedDeliveryDate = oReader.GetDateTime("ExpectedDeliveryDate");
            oFEO.ExpDelEndDate = oReader.GetDateTime("ExpDelEndDate");
            oFEO.SaleOrderID = oReader.GetInt32("SaleOrderID");
            oFEO.IsInHouse = oReader.GetBoolean("IsInHouse");
            oFEO.PreShipmentSampleReq = oReader.GetBoolean("PreShipmentSampleReq");
            oFEO.IsFinish = oReader.GetBoolean("IsFinish");
            oFEO.LightSourceID = oReader.GetInt32("LightSourceID");
            oFEO.PPSampleDate = oReader.GetDateTime("PPSampleDate");
            oFEO.PreSampleDate = oReader.GetDateTime("PreSampleDate");
            oFEO.GreyQty = oReader.GetDouble("GreyQty");

            oFEO.PreparedByName = oReader.GetString("PreparedByName");
            oFEO.DONo = oReader.GetString("DONo");

            oFEO.ApproveBy = oReader.GetInt32("ApproveBy");
            oFEO.ApproveDate = oReader.GetDateTime("ApproveDate");
            oFEO.ApproveByName = oReader.GetString("ApproveByName");

            oFEO.FabricNo = oReader.GetString("FabricNo");
            oFEO.FabricIssueDate = oReader.GetDateTime("FabricIssueDate");
            oFEO.FabricApprovedDate = oReader.GetDateTime("FabricApprovedDate");
            oFEO.FabricRemark = oReader.GetString("FabricRemark");
            oFEO.Composition = oReader.GetString("Composition");
            oFEO.Construction = oReader.GetString("Construction");

            oFEO.ColorName = oReader.GetString("ColorName");
            oFEO.ProcessType = oReader.GetInt32("ProcessType");
            oFEO.FabricWeave = oReader.GetInt32("FabricWeave");
            oFEO.FinishType = oReader.GetInt32("FinishType");

            oFEO.ProcessTypeName = oReader.GetString("ProcessTypeName");
            oFEO.FabricWeaveName = oReader.GetString("FabricWeaveName");
            oFEO.FinishTypeName = oReader.GetString("FinishTypeName");
           
            oFEO.Weave = oReader.GetString("Weave");

            oFEO.PINo = oReader.GetString("PINo");
            oFEO.IssueDatePI = oReader.GetDateTime("IssueDatePI");
            oFEO.LCNo = oReader.GetString("LCNo");
            oFEO.OpeningDateLC = oReader.GetDateTime("OpeningDateLC");
            oFEO.ShipmentDate = oReader.GetDateTime("ShipmentDate");

            oFEO.BuyerName = oReader.GetString("BuyerName");
            oFEO.BCPerson = oReader.GetString("BCPerson");

            oFEO.MKTPerson = oReader.GetString("MKTPerson");
            oFEO.PaymentType = oReader.GetInt32("PaymentType");
            oFEO.FabricDeliveryOrderQty = oReader.GetDouble("FabricDeliveryOrderQty");
            oFEO.FEOColor = oReader.GetString("Color");
            oFEO.IsYarnDyed = oReader.GetBoolean("IsYarnDyed");
            oFEO.FactoryName = oReader.GetString("FactoryName");
            oFEO.FDO = oReader.GetString("FDO");
            oFEO.DEONo = oReader.GetString("DEONo");
           
            //write by Mahabub
            oFEO.ProductionDoneQty = oReader.GetDouble("ProductionDoneQty");
            oFEO.InProductionQty = oReader.GetDouble("InProductionQty");
            oFEO.WarpDoneQty = oReader.GetDouble("WarpDoneQty");
            oFEO.NoOfProcessProgram = oReader.GetInt32("NoOfProcessProgram");
            oFEO.LogCount = oReader.GetInt32("LogCount");
            oFEO.ReviseDate = oReader.GetDateTime("LastUpdatedDate");

            oFEO.FEOYarnQty = oReader.GetDouble("FEOYarnQty");
            oFEO.YarnTransferQty = oReader.GetDouble("YarnTransferQty");
            oFEO.CountFBQCDetail = oReader.GetInt32("CountFBQCDetail");
            oFEO.FYDNo = oReader.GetString("FYDNo");
            oFEO.MUName = oReader.GetString("MUName");
            return oFEO;
        }

        public static FabricExecutionOrder CreateObject(NullHandler oReader)
        {
            FabricExecutionOrder oFabricExecutionOrder = new FabricExecutionOrder();
            FabricExecutionOrderService oFEOService = new FabricExecutionOrderService();
            oFabricExecutionOrder = oFEOService.MapObject(oReader);
            return oFabricExecutionOrder;
        }

        private List<FabricExecutionOrder> CreateObjects(IDataReader oReader)
        {
            List<FabricExecutionOrder> oFabricExecutionOrders = new List<FabricExecutionOrder>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricExecutionOrder oItem = CreateObject(oHandler);
                oFabricExecutionOrders.Add(oItem);
            }
            return oFabricExecutionOrders;
        }
        #endregion

        #region Interface implementatio
        public FabricExecutionOrderService() { }

        public FabricExecutionOrder IUD(FabricExecutionOrder oFEO, int nDBOperation, Int64 nUserId)
        {
            FabricExecutionOrderFabric oFEOF = new FabricExecutionOrderFabric();
            string sFabricID = "";
            TransactionContext tc = null;
            try
            {
                List<FabricExecutionOrderFabric> oFEOFs = new List<FabricExecutionOrderFabric>();
                oFEOFs = oFEO.FEOFs;

                tc = TransactionContext.Begin(true);
                IDataReader reader;

                reader = FabricExecutionOrderDA.IUD(tc, oFEO, nDBOperation, nUserId);

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFEO = new FabricExecutionOrder();
                    oFEO = CreateObject(oReader);
                }
                if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    oFEO = new FabricExecutionOrder();
                    oFEO.ErrorMessage = Global.DeleteMessage;
                }
                reader.Close();

                #region FabricExecutionOrderFabric
                if (oFEOFs.Count > 0)
                {
                    if (!oFEOFs.Where(x => x.FabricID == oFEO.FabricID).ToList().Any())
                        throw new Exception("FEO fabric not found in fabric list.");

                    string sExportPIIDs = string.Join(",", oFEOFs.Select(o => o.ExportPIID).Distinct());
                    int i = 0;
                    foreach (FabricExecutionOrderFabric oItem in oFEOFs)
                    {
                        if (oItem.FabricID > 0)
                        {
                            IDataReader readerdetailFEOF;
                            oItem.FEOID = oFEO.FEOID;
                            oItem.FEO_BuyerID = oFEO.BuyerID;
                            oItem.FEO_FabricID = oFEO.FabricID;
                            oItem.ExportPIIDs = sExportPIIDs;
                            readerdetailFEOF = FabricExecutionOrderFabricDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserId, "");

                            NullHandler oFEOF_Field = new NullHandler(readerdetailFEOF);
                            if (readerdetailFEOF.Read())
                            {
                                sFabricID = sFabricID + oFEOF_Field.GetString("FabricID") + "~" + oFEOF_Field.GetString("ExportPIDetailID") + ",";
                            }
                            readerdetailFEOF.Close();
                            i++;
                        }
                    }
                    if (sFabricID.Length > 0)
                    {
                        sFabricID = sFabricID.Remove(sFabricID.Length - 1, 1);
                    }
                    oFEOF = new FabricExecutionOrderFabric();
                    oFEOF.FEOID = oFEO.FEOID;
                    FabricExecutionOrderFabricDA.Delete(tc, oFEOF, EnumDBOperation.Delete, nUserId, sFabricID);
                }
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFEO = new FabricExecutionOrder();
                oFEO.ErrorMessage = (e.Message.Contains("!")) ? e.Message.Split('!')[0] : e.Message;
                #endregion
            }
            return oFEO;
        }
        public FabricExecutionOrder UnapproveFEO(FabricExecutionOrder oFEO, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = FabricExecutionOrderDA.UnapproveFEO(tc, oFEO, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFEO = new FabricExecutionOrder();
                    oFEO = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFEO = new FabricExecutionOrder();
                oFEO.ErrorMessage = (e.Message.Contains("!")) ? e.Message.Split('!')[0] : e.Message;
                #endregion
            }
            return oFEO;
        }

        public FabricExecutionOrder Copy(FabricExecutionOrder oFabricExecutionOrder, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = FabricExecutionOrderDA.Copy(tc, oFabricExecutionOrder, EnumDBOperation.Insert, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricExecutionOrder = new FabricExecutionOrder();
                    oFabricExecutionOrder = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricExecutionOrder.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricExecutionOrder;
        }
        public FabricExecutionOrder SaveLog(FabricExecutionOrder oFabricExecutionOrder, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = FabricExecutionOrderDA.SaveLog(tc, oFabricExecutionOrder, EnumDBOperation.Revise, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricExecutionOrder = new FabricExecutionOrder();
                    oFabricExecutionOrder = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricExecutionOrder.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricExecutionOrder;
        }

        public FabricExecutionOrder UpdateFinish(int nFEOID, bool bIsFinish, Int64 nUserID)
        {
            FabricExecutionOrder oFabricExecutionOrder = new FabricExecutionOrder();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = FabricExecutionOrderDA.UpdateFinish(tc, nFEOID, bIsFinish, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricExecutionOrder = new FabricExecutionOrder();
                    oFabricExecutionOrder = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricExecutionOrder.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricExecutionOrder;
        }
        public FabricExecutionOrder Get(int nFEOID, Int64 nUserId)
        {
            FabricExecutionOrder oFEO = new FabricExecutionOrder();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricExecutionOrderDA.Get(tc, nFEOID, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFEO = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFEO = new FabricExecutionOrder();
                oFEO.ErrorMessage = e.Message;
                #endregion
            }

            return oFEO;
        }
        public List<FabricExecutionOrder> Gets(string sSQL, Int64 nUserId)
        {
            List<FabricExecutionOrder> oFEOs = new List<FabricExecutionOrder>();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricExecutionOrderDA.Gets(tc, sSQL, nUserId);
                oFEOs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                FabricExecutionOrder oFEO = new FabricExecutionOrder();
                oFEO.ErrorMessage = e.Message;
                oFEOs.Add(oFEO);
                #endregion
            }

            return oFEOs;
        }
        public FabricExecutionOrder Get(string sSQL, Int64 nUserId)
        {
            FabricExecutionOrder oFEO = new FabricExecutionOrder();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricExecutionOrderDA.Get(tc, sSQL);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFEO = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFEO = new FabricExecutionOrder();
                oFEO.ErrorMessage = e.Message;
                #endregion
            }

            return oFEO;
        }

        public List<FabricExecutionOrder> GetByFEONo(int nIsInHouse, string sFEONo, int nYear, Int64 nUserId)
        {
            List<FabricExecutionOrder> oFEOs = new List<FabricExecutionOrder>();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricExecutionOrderDA.GetByFEONo(tc, nIsInHouse, sFEONo, nYear);
                oFEOs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                FabricExecutionOrder oFEO = new FabricExecutionOrder();
                oFEO.ErrorMessage = e.Message;
                oFEOs.Add(oFEO);
                #endregion
            }

            return oFEOs;
        }

        public FabricExecutionOrder ProcessYarnDyed(int nFEOID, Int64 nUserId)
        {
            TransactionContext tc = null;
            FabricExecutionOrder oFEO = new FabricExecutionOrder();
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = FabricExecutionOrderDA.ProcessYarnDyed(tc, nFEOID, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFEO = new FabricExecutionOrder();
                    oFEO = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFEO = new FabricExecutionOrder();
                oFEO.ErrorMessage = (e.Message.Contains("!")) ? e.Message.Split('!')[0] : e.Message;
                #endregion
            }
            return oFEO;
        }


        #endregion
    }
}