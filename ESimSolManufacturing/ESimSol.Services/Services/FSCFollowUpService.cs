using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;

namespace ESimSol.Services.Services
{
    public class FSCFollowUpService : MarshalByRefObject, IFSCFollowUpService
    {
        #region Private functions and declaration

        private FSCFollowUp MapObject(NullHandler oReader)
        {
            FSCFollowUp oFSCFollowUp = new FSCFollowUp();
            oFSCFollowUp.SCNoFull = oReader.GetString("SCNoFull");
            oFSCFollowUp.SCDate = oReader.GetDateTime("SCDate");
            oFSCFollowUp.ExeNo = oReader.GetString("ExeNo");
            oFSCFollowUp.ExeDate = oReader.GetDateTime("ExeDate");
            oFSCFollowUp.Qty_PO = oReader.GetDouble("Qty_PO");
            oFSCFollowUp.Qty_Dispo = oReader.GetDouble("Qty_Dispo");
            oFSCFollowUp.BuyerName = oReader.GetString("BuyerName");
            oFSCFollowUp.ContractorName = oReader.GetString("ContractorName");
            oFSCFollowUp.MKTPersonName = oReader.GetString("MKTPersonName");
            oFSCFollowUp.ProcessType = oReader.GetInt32("ProcessType");
            oFSCFollowUp.ProcessTypeName = oReader.GetString("ProcessTypeName");
            oFSCFollowUp.Construction = oReader.GetString("Construction");
            oFSCFollowUp.FinishTypeName = oReader.GetString("FinishTypeName");
            oFSCFollowUp.FinishDesign = oReader.GetString("FinishDesign");
            oFSCFollowUp.FabricWeave = oReader.GetInt32("FabricWeave");
            oFSCFollowUp.FabricWeaveName = oReader.GetString("FabricWeaveName");
            oFSCFollowUp.DateDUPSchedule = oReader.GetDateTime("DateDUPSchedule");
            oFSCFollowUp.QtyReqDyed = oReader.GetDouble("QtyReqDyed");
            oFSCFollowUp.QtyDUPSchedule = oReader.GetDouble("QtyDUPSchedule");
            oFSCFollowUp.DateLotAssign = oReader.GetDateTime("DateLotAssign");
            oFSCFollowUp.QtyLotAssign = oReader.GetDouble("QtyLotAssign");
            oFSCFollowUp.DateIssueDUReq = oReader.GetDateTime("DateIssueDUReq");
            oFSCFollowUp.QtyDUReq = oReader.GetDouble("QtyDUReq");
            oFSCFollowUp.DateReceiveDUReq = oReader.GetDateTime("DateReceiveDUReq");
            oFSCFollowUp.QtySoftWinding = oReader.GetDouble("QtySoftWinding");
            oFSCFollowUp.DateRSInFloor = oReader.GetDateTime("DateRSInFloor");
            oFSCFollowUp.DateBatchload = oReader.GetDateTime("DateBatchload");
            oFSCFollowUp.QtyDyeMachine = oReader.GetDouble("QtyDyeMachine");
            oFSCFollowUp.QtyHydro = oReader.GetDouble("QtyHydro");
            oFSCFollowUp.QtyDryer = oReader.GetDouble("QtyDryer");
            oFSCFollowUp.QtyApproval = oReader.GetDouble("QtyApproval");
            oFSCFollowUp.DateHWRecd = oReader.GetDateTime("DateHWRecd");
            oFSCFollowUp.QtyFreshDye = oReader.GetDouble("QtyFreshDye");
            oFSCFollowUp.DateBeamTr = oReader.GetDateTime("DateBeamTr");
            oFSCFollowUp.QtyBeamTr = oReader.GetDouble("QtyBeamTr");
            oFSCFollowUp.DateWarpingStart = oReader.GetDateTime("DateWarpingStart");
            oFSCFollowUp.QtyWarping = oReader.GetDouble("QtyWarping");
            oFSCFollowUp.DateWarpingEnd = oReader.GetDateTime("DateWarpingEnd");
            oFSCFollowUp.DateSizingStart = oReader.GetDateTime("DateSizingStart");
            oFSCFollowUp.QtySizing = oReader.GetDouble("QtySizing");
            oFSCFollowUp.DateSizingEnd = oReader.GetDateTime("DateSizingEnd");
            oFSCFollowUp.DateDrawingStart = oReader.GetDateTime("DateDrawingStart");
            oFSCFollowUp.QtyDrawing = oReader.GetDouble("QtyDrawing");
            oFSCFollowUp.DateDrawingEnd = oReader.GetDateTime("DateDrawingEnd");
            oFSCFollowUp.DateLoomStart = oReader.GetDateTime("DateLoomStart");
            oFSCFollowUp.QtyLoom = oReader.GetDouble("QtyLoom");
            oFSCFollowUp.DateLoomEnd = oReader.GetDateTime("DateLoomEnd");
            oFSCFollowUp.QtyGreyIns = oReader.GetDouble("QtyGreyIns");
            oFSCFollowUp.DatePretreatment = oReader.GetDateTime("DatePretreatment");
            oFSCFollowUp.QtyPretreatment = oReader.GetDouble("QtyPretreatment");
            oFSCFollowUp.DateFnDyeing = oReader.GetDateTime("DateFnDyeing");
            oFSCFollowUp.QtyFnDyeing = oReader.GetDouble("QtyFnDyeing");
            oFSCFollowUp.DateFinishing = oReader.GetDateTime("DateFinishing");
            oFSCFollowUp.QtyFinishing = oReader.GetDouble("QtyFinishing");
            oFSCFollowUp.DateFNInsRecd = oReader.GetDateTime("DateFNInsRecd");
            oFSCFollowUp.QtyFNIns = oReader.GetDouble("QtyFNIns");
            oFSCFollowUp.DateFNInsDC = oReader.GetDateTime("DateFNInsDC");
            oFSCFollowUp.QtyDO = oReader.GetDouble("QtyDO");
            oFSCFollowUp.IsPrint = oReader.GetBoolean("IsPrint");
            oFSCFollowUp.ContractorID = oReader.GetInt32("ContractorID");
            oFSCFollowUp.BCPID = oReader.GetInt32("BCPID");
            oFSCFollowUp.MKTPersonID = oReader.GetInt32("MKTPersonID");
            oFSCFollowUp.FabricID = oReader.GetInt32("FabricID");
            oFSCFollowUp.MUID = oReader.GetInt32("MUID");
            oFSCFollowUp.ProductName = oReader.GetString("ProductName");
            oFSCFollowUp.FabricNo = oReader.GetString("FabricNo");
            oFSCFollowUp.ColorInfo = oReader.GetString("ColorInfo");
            oFSCFollowUp.FabricWidth = oReader.GetString("FabricWidth");
            oFSCFollowUp.BuyerReference = oReader.GetString("BuyerReference");
            oFSCFollowUp.StyleNo = oReader.GetString("StyleNo");
            oFSCFollowUp.PINo = oReader.GetString("PINo");
            oFSCFollowUp.LCNo = oReader.GetString("LCNo");
            oFSCFollowUp.ProductID = oReader.GetInt32("ProductID");
            oFSCFollowUp.FinishType = oReader.GetInt32("FinishType");
            oFSCFollowUp.BuyerID = oReader.GetInt32("BuyerID");
            oFSCFollowUp.FSCID = oReader.GetInt32("FSCID");
            oFSCFollowUp.FSCDetailID = oReader.GetInt32("FSCDetailID");
            oFSCFollowUp.DyeingOrderID = oReader.GetInt32("DyeingOrderID");
            oFSCFollowUp.MUnit = oReader.GetString("MUnit");
            oFSCFollowUp.FabricDesignID = oReader.GetInt32("FabricDesignID");
            oFSCFollowUp.FabricDesign = oReader.GetString("FabricDesign");
            oFSCFollowUp.QtyGreyRecd = oReader.GetDouble("QtyGreyRecd");
            oFSCFollowUp.QtyFnBatch = oReader.GetDouble("QtyFnBatch");
            oFSCFollowUp.QtyFnInspRecd = oReader.GetDouble("QtyFnInspRecd");
            oFSCFollowUp.QtyDC = oReader.GetDouble("QtyDC");
            oFSCFollowUp.QtyRC = oReader.GetDouble("QtyRC");
            oFSCFollowUp.QtyStoreRecd = oReader.GetDouble("QtyStoreRecd");
            oFSCFollowUp.StockInHand = oReader.GetDouble("StockInHand");
            oFSCFollowUp.DateDryerUnLoad = oReader.GetDateTime("DateDryerUnLoad");
            oFSCFollowUp.DateBatchUnload = oReader.GetDateTime("DateBatchUnload");
            oFSCFollowUp.DateDCOut = oReader.GetDateTime("DateDCOut");
            oFSCFollowUp.DateHydroLoad = oReader.GetDateTime("DateHydroLoad");
            oFSCFollowUp.DateApproval = oReader.GetDateTime("DateApproval");

            return oFSCFollowUp;
        }

        private FSCFollowUp CreateObject(NullHandler oReader)
        {
            FSCFollowUp oFSCFollowUp = new FSCFollowUp();
            oFSCFollowUp = MapObject(oReader);
            return oFSCFollowUp;
        }

        private List<FSCFollowUp> CreateObjects(IDataReader oReader)
        {
            List<FSCFollowUp> oFSCFollowUp = new List<FSCFollowUp>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FSCFollowUp oItem = CreateObject(oHandler);
                oFSCFollowUp.Add(oItem);
            }
            return oFSCFollowUp;
        }

        #endregion

        #region Interface implementation
        public FSCFollowUp Get(int id, Int64 nUserId)
        {
            FSCFollowUp oFSCFollowUp = new FSCFollowUp();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FSCFollowUpDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFSCFollowUp = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FSCFollowUp", e);
                #endregion
            }
            return oFSCFollowUp;
        }

        public List<FSCFollowUp> Gets(Int64 nUserID)
        {
            List<FSCFollowUp> oFSCFollowUps = new List<FSCFollowUp>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FSCFollowUpDA.Gets(tc);
                oFSCFollowUps = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FSCFollowUp oFSCFollowUp = new FSCFollowUp();
                oFSCFollowUp.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFSCFollowUps;
        }

        public List<FSCFollowUp> Gets(string sSQL, Int64 nUserID)
        {
            List<FSCFollowUp> oFSCFollowUps = new List<FSCFollowUp>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FSCFollowUpDA.Gets(tc, sSQL);
                oFSCFollowUps = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FSCFollowUp", e);
                #endregion
            }
            return oFSCFollowUps;
        }

        #endregion
    }

}
