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
    public class FabricDeliveryChallanService : MarshalByRefObject, IFabricDeliveryChallanService
    {
        #region Private functions and declaration
        private FabricDeliveryChallan MapObject(NullHandler oReader)
        {
            FabricDeliveryChallan oFDC = new FabricDeliveryChallan();
            oFDC.FDCID = oReader.GetInt32("FDCID");
            oFDC.ChallanNo = oReader.GetString("ChallanNo");
            oFDC.FDOID = oReader.GetInt32("FDOID");
            oFDC.ContractorID = oReader.GetInt32("ContractorID");
            oFDC.DeliveryPoint = oReader.GetString("DeliveryPoint");
            oFDC.VehicleNo = oReader.GetString("VehicleNo");
            oFDC.DriverName = oReader.GetString("DriverName");
            oFDC.DriverMobile = oReader.GetString("DriverMobile");
            oFDC.Note = oReader.GetString("Note");
            oFDC.ApproveBy = oReader.GetInt32("ApproveBy");
            oFDC.ApproveDate = oReader.GetDateTime("ApproveDate");
            oFDC.DisburseBy = oReader.GetInt32("DisburseBy");
            oFDC.DisburseDate = oReader.GetDateTime("DisburseDate");
            oFDC.IssueDate = oReader.GetDateTime("IssueDate");
            oFDC.WorkingUnitID = oReader.GetInt32("WorkingunitID");
            oFDC.VehicleTypeID = oReader.GetInt32("VehicleTypeID");
            oFDC.Qty = oReader.GetDouble("Qty");
            oFDC.FabricDONo = oReader.GetString("FabricDONo");
            oFDC.FDODate = oReader.GetDateTime("DODate");
            oFDC.BuyerName = oReader.GetString("BuyerName");
            oFDC.DeliveryToName = oReader.GetString("DeliveryToName");
            oFDC.BuyerCPName = oReader.GetString("BuyerCPName");
            oFDC.ApproveByName = oReader.GetString("ApproveByName");
            oFDC.PreparedByName = oReader.GetString("PreparedByName");
            oFDC.DisburseByName = oReader.GetString("DisburseByName");
            oFDC.WorkingUnitName = oReader.GetString("StoreName");
            oFDC.VehicleTypeName = oReader.GetString("VehicleTypeName");
            oFDC.DeliveryMan = oReader.GetString("DeliveryMan");
            oFDC.GatePassNo = oReader.GetString("GatePassNo");
            oFDC.CPDeliveryMan = oReader.GetString("CPDeliveryMan");
            oFDC.IsSample = oReader.GetBoolean("IsSample");
            oFDC.FDOType = (EnumDOType)oReader.GetInt16("FDOType");
            return oFDC;
        }

        public static FabricDeliveryChallan CreateObject(NullHandler oReader)
        {
            FabricDeliveryChallan oFabricDeliveryChallan = new FabricDeliveryChallan();
            FabricDeliveryChallanService oFDCService = new FabricDeliveryChallanService();
            oFabricDeliveryChallan = oFDCService.MapObject(oReader);
            return oFabricDeliveryChallan;
        }

        private List<FabricDeliveryChallan> CreateObjects(IDataReader oReader)
        {
            List<FabricDeliveryChallan> oFabricDeliveryChallans = new List<FabricDeliveryChallan>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricDeliveryChallan oItem = CreateObject(oHandler);
                oFabricDeliveryChallans.Add(oItem);
            }
            return oFabricDeliveryChallans;
        }
        #endregion

        #region Interface implementatio
        public FabricDeliveryChallanService() { }

        public FabricDeliveryChallan IUD(FabricDeliveryChallan oFDC, int nDBOperation, Int64 nUserId)
        {
            List<FabricDeliveryChallanDetail> oFDCDetails = new List<FabricDeliveryChallanDetail>();
            TransactionContext tc = null;
            double nQty = 0;
            string sFDCDetaillIDs = "";
            try
            {
                oFDCDetails = oFDC.FDCDetails;
                tc = TransactionContext.Begin(true);
                IDataReader reader;

                reader = FabricDeliveryChallanDA.IUD(tc, oFDC, nDBOperation, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFDC = new FabricDeliveryChallan();
                    oFDC = CreateObject(oReader);
                }
                reader.Close();
                #region Details Part
                if (oFDCDetails != null)
                {
                    foreach (FabricDeliveryChallanDetail oItem in oFDCDetails)
                    {
                        nQty = nQty + oItem.Qty;
                        IDataReader readertnc;
                        oItem.FDCID = oFDC.FDCID;
                        if (oItem.FDCDID <= 0)
                        {
                            readertnc = FabricDeliveryChallanDetailDA.IUD(tc, oItem, (int)EnumDBOperation.Insert, nUserId, "");
                        }
                        else
                        {
                            readertnc = FabricDeliveryChallanDetailDA.IUD(tc, oItem, (int)EnumDBOperation.Update, nUserId, "");
                        }
                        NullHandler oReaderTNC = new NullHandler(readertnc);

                        if (readertnc.Read())
                        {
                            sFDCDetaillIDs = sFDCDetaillIDs + oReaderTNC.GetString("FDCDID") + ",";
                        }
                        readertnc.Close();
                    }
                    oFDC.Qty = nQty;
                    if (sFDCDetaillIDs.Length > 0)
                    {
                        sFDCDetaillIDs = sFDCDetaillIDs.Remove(sFDCDetaillIDs.Length - 1, 1);
                    }
                    FabricDeliveryChallanDetail oFabricDeliveryOrderDetail = new FabricDeliveryChallanDetail();
                    oFabricDeliveryOrderDetail.FDCID = oFDC.FDCID;
                    FabricDeliveryChallanDetailDA.Delete(tc, oFabricDeliveryOrderDetail, (int)EnumDBOperation.Delete, nUserId, sFDCDetaillIDs);
                    sFDCDetaillIDs = "";

                }
                #endregion
             
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFDC = new FabricDeliveryChallan();
                oFDC.ErrorMessage = (e.Message.Contains("~")) ? e.Message.Split('~')[0] : e.Message;
                #endregion
            }
            return oFDC;
        }
        public FabricDeliveryChallan Approve(FabricDeliveryChallan oFDC,  Int64 nUserId)
        {
            List<FabricDeliveryChallanDetail> oFDCDetails = new List<FabricDeliveryChallanDetail>();
            TransactionContext tc = null;
            try
            {
                oFDCDetails = oFDC.FDCDetails;
                tc = TransactionContext.Begin(true);
                IDataReader reader;

                reader = FabricDeliveryChallanDA.IUD(tc, oFDC, (int)EnumDBOperation.Approval, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFDC = new FabricDeliveryChallan();
                    oFDC = CreateObject(oReader);
                }
                reader.Close();
                

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFDC = new FabricDeliveryChallan();
                oFDC.ErrorMessage = (e.Message.Contains("~")) ? e.Message.Split('~')[0] : e.Message;
                #endregion
            }
            return oFDC;
        }
        public FabricDeliveryChallan UndoApprove(FabricDeliveryChallan oFDC, Int64 nUserId)
        {
            List<FabricDeliveryChallanDetail> oFDCDetails = new List<FabricDeliveryChallanDetail>();
            TransactionContext tc = null;
            try
            {
                oFDCDetails = oFDC.FDCDetails;
                tc = TransactionContext.Begin(true);
                IDataReader reader;

                reader = FabricDeliveryChallanDA.IUD(tc, oFDC, (int)EnumDBOperation.UnApproval, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFDC = new FabricDeliveryChallan();
                    oFDC = CreateObject(oReader);
                }
                reader.Close();


                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFDC = new FabricDeliveryChallan();
                oFDC.ErrorMessage = (e.Message.Contains("~")) ? e.Message.Split('~')[0] : e.Message;
                #endregion
            }
            return oFDC;
        }
        public String Delete(FabricDeliveryChallan oFabricDeliveryChallan, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricDeliveryChallanDA.Delete(tc, oFabricDeliveryChallan, (int)EnumDBOperation.Delete, nUserID);
                tc.End();

            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                return e.Message;
                #endregion
            }
            return Global.DeleteMessage;
        }
        public FabricDeliveryChallan Get(int nFDCID, Int64 nUserId)
        {
            FabricDeliveryChallan oFDC = new FabricDeliveryChallan();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricDeliveryChallanDA.Get(tc, nFDCID, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFDC = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFDC = new FabricDeliveryChallan();
                oFDC.ErrorMessage = "Failed to get information.";
                #endregion
            }

            return oFDC;
        }

        public List<FabricDeliveryChallan> Gets(string sSQL, Int64 nUserId)
        {
            List<FabricDeliveryChallan> oFDCs = new List<FabricDeliveryChallan>();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricDeliveryChallanDA.Gets(tc, sSQL, nUserId);
                oFDCs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFDCs = new List<FabricDeliveryChallan>();
                #endregion
            }

            return oFDCs;
        }

        public FabricDeliveryChallan FDCDisburse(FabricDeliveryChallan oFDC, Int64 nUserId)
        {
            

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricDeliveryChallanDA.IUD(tc, oFDC, (int)EnumDBOperation.Request, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFDC = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFDC = new FabricDeliveryChallan();
                oFDC.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : "Unable to disburse.";
                #endregion
            }

            return oFDC;
        }


        #endregion
    }
}