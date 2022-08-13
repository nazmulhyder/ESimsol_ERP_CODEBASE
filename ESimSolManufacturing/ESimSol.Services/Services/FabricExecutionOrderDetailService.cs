using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;


namespace ESimSol.Services.Services
{
    [Serializable]
    public class FabricExecutionOrderDetailService : MarshalByRefObject, IFabricExecutionOrderDetailService
    {
        #region Private functions and declaration
        private FabricExecutionOrderDetail MapObject(NullHandler oReader)
        {
            FabricExecutionOrderDetail oFEODetail = new FabricExecutionOrderDetail();
            oFEODetail.FEODID = oReader.GetInt32("FEODID");
            oFEODetail.FEOID = oReader.GetInt32("FEOID");
            oFEODetail.ProductID = oReader.GetInt32("ProductID");
            oFEODetail.Qty = oReader.GetDouble("Qty");

            oFEODetail.BuyerID = oReader.GetInt32("BuyerID");
            oFEODetail.FabricID = oReader.GetInt32("FabricID");

            oFEODetail.ProductCode = oReader.GetString("ProductCode");
            oFEODetail.ProductName = oReader.GetString("ProductName");
            oFEODetail.ShortName = oReader.GetString("ShortName");
            oFEODetail.UnitPrice = oReader.GetDouble("UnitPrice");
            oFEODetail.SuggestedQty = oReader.GetDouble("SuggestedQty");
            return oFEODetail;
        }

        private FabricExecutionOrderDetail CreateObject(NullHandler oReader)
        {
            FabricExecutionOrderDetail oFabricExecutionOrderDetail = new FabricExecutionOrderDetail();
            oFabricExecutionOrderDetail = MapObject(oReader);
            return oFabricExecutionOrderDetail;
        }

        private List<FabricExecutionOrderDetail> CreateObjects(IDataReader oReader)
        {
            List<FabricExecutionOrderDetail> oFabricExecutionOrderDetails = new List<FabricExecutionOrderDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricExecutionOrderDetail oItem = CreateObject(oHandler);
                oFabricExecutionOrderDetails.Add(oItem);
            }
            return oFabricExecutionOrderDetails;
        }
        #endregion

        #region Interface implementatio
        public FabricExecutionOrderDetailService() { }

        public FabricExecutionOrderDetail IUD(FabricExecutionOrderDetail oFEODetail, int nDBOperation, Int64 nUserId)
        {
            TransactionContext tc = null;
            FabricExecutionOrder oFEO = new FabricExecutionOrder();
            List<FabricExecutionOrderNote> oFEONotes = new List<FabricExecutionOrderNote>();
            FabricExecutionOrderNote oFEONote = new FabricExecutionOrderNote();
            List<FabricExecutionOrderFabric> oFEOFs = new List<FabricExecutionOrderFabric>();
            FabricExecutionOrderFabric oFEOF = new FabricExecutionOrderFabric();
            string sFabricID = "";
            oFEOFs = oFEODetail.FEOFs;

            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                NullHandler oReader;
                if (nDBOperation == (int)EnumDBOperation.Insert && oFEODetail.FEOID == 0)
                {

                    if (oFEODetail.FEO != null)
                    {
                        reader = FabricExecutionOrderDA.IUD(tc, oFEODetail.FEO, nDBOperation, nUserId);
                        oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oFEO = FabricExecutionOrderService.CreateObject(oReader);
                        }
                        reader.Close();
                    }
                    else { throw new Exception("No fabric pattern information found to save."); }
                    oFEODetail.FEOID = oFEO.FEOID;
                    oFEODetail.FEONotes = oFEODetail.FEONotes.Where(x => x.Note.Trim() != "").ToList();
                    if (oFEODetail.FEONotes.Count() > 0)
                    {
                        foreach (FabricExecutionOrderNote oItem in oFEODetail.FEONotes)
                        {
                            oFEONote = new FabricExecutionOrderNote();
                            oItem.FEOID = oFEO.FEOID;
                            reader = FabricExecutionOrderNoteDA.IUD(tc, oItem, nDBOperation, nUserId);
                            oReader = new NullHandler(reader);
                            if (reader.Read())
                            {
                                oFEONote = FabricExecutionOrderNoteService.CreateObject(oReader);
                            }
                            reader.Close();
                            oFEONotes.Add(oFEONote);
                        }
                    }
                }


                reader = FabricExecutionOrderDetailDA.IUD(tc, oFEODetail, nDBOperation, nUserId);

                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFEODetail = new FabricExecutionOrderDetail();
                    oFEODetail = CreateObject(oReader);
                }
                if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    oFEODetail = new FabricExecutionOrderDetail();
                    oFEODetail.ErrorMessage = Global.DeleteMessage;
                }
                reader.Close();

                #region FabricExecutionOrderFabric
                if (oFEOFs.Count > 0)
                {
                    string sExportPIIDs = string.Join(",", oFEOFs.Select(o=>o.ExportPIID).Distinct());
                    foreach (FabricExecutionOrderFabric oItem in oFEOFs)
                    {
                        if (oItem.FabricID > 0)
                        {
                            IDataReader readerdetailFEOF;
                            oItem.FEOID = oFEODetail.FEOID;
                            oItem.FEO_BuyerID = oFEODetail.BuyerID;
                            oItem.FEO_FabricID = oFEODetail.FabricID;
                            oItem.ExportPIIDs = sExportPIIDs;
                            readerdetailFEOF = FabricExecutionOrderFabricDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserId, "");

                            NullHandler oFEOF_Field = new NullHandler(readerdetailFEOF);
                            if (readerdetailFEOF.Read())
                            {
                                sFabricID = sFabricID + oFEOF_Field.GetString("FabricID") + "~" + oFEOF_Field.GetString("ExportPIDetailID") + ",";
                            }
                            readerdetailFEOF.Close();
                        }
                    }
                    if (sFabricID.Length > 0)
                    {
                        sFabricID = sFabricID.Remove(sFabricID.Length - 1, 1);
                    }
                     oFEOF = new FabricExecutionOrderFabric();
                    oFEOF.FEOID = oFEODetail.FEOID;
                    FabricExecutionOrderFabricDA.Delete(tc, oFEOF, EnumDBOperation.Delete, nUserId, sFabricID);
                }
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFEODetail = new FabricExecutionOrderDetail();
                oFEODetail.ErrorMessage = (e.Message.Contains("!")) ? e.Message.Split('!')[0] : e.Message;
                #endregion
            }
            oFEODetail.FEO = oFEO;
            oFEODetail.FEONotes = oFEONotes;
            return oFEODetail;
        }

        public FabricExecutionOrderDetail Get(int nFEODID, Int64 nUserId)
        {
            FabricExecutionOrderDetail oFEODetail = new FabricExecutionOrderDetail();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricExecutionOrderDetailDA.Get(tc, nFEODID, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFEODetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFEODetail = new FabricExecutionOrderDetail();
                oFEODetail.ErrorMessage = "Failed to get information.";
                #endregion
            }

            return oFEODetail;
        }
        public List<FabricExecutionOrderDetail> Gets(int nFEOID, Int64 nUserId)
        {
            List<FabricExecutionOrderDetail> oFEODetails = new List<FabricExecutionOrderDetail>();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricExecutionOrderDetailDA.Gets(tc, nFEOID, nUserId);
                oFEODetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFEODetails = new List<FabricExecutionOrderDetail>();
                #endregion
            }

            return oFEODetails;
        }
        public List<FabricExecutionOrderDetail> Gets(string sSQL, Int64 nUserId)
        {
            List<FabricExecutionOrderDetail> oFEODetails = new List<FabricExecutionOrderDetail>();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricExecutionOrderDetailDA.Gets(tc, sSQL, nUserId);
                oFEODetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFEODetails = new List<FabricExecutionOrderDetail>();
                #endregion
            }

            return oFEODetails;
        }
        #endregion
    }
}