using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Linq;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;


namespace ESimSol.Services.Services
{
    public class FabricExecutionOrderYarnReceiveService : MarshalByRefObject, IFabricExecutionOrderYarnReceiveService
    {
        #region Private functions and declaration
        private static FabricExecutionOrderYarnReceive MapObject(NullHandler oReader)
        {
            FabricExecutionOrderYarnReceive oFabricExecutionOrderYarnReceive = new FabricExecutionOrderYarnReceive();
            oFabricExecutionOrderYarnReceive.FEOYID = oReader.GetInt32("FEOYID");
            oFabricExecutionOrderYarnReceive.WYRequisitionID = oReader.GetInt32("WYRequisitionID");
            oFabricExecutionOrderYarnReceive.FSCDID = oReader.GetInt32("FSCDID");
            oFabricExecutionOrderYarnReceive.IssueLotID = oReader.GetInt32("IssueLotID");
            //oFabricExecutionOrderYarnReceive.LotID = oReader.GetInt32("DestinationLotID");
            oFabricExecutionOrderYarnReceive.ReqQty = oReader.GetDouble("ReqQty");
            oFabricExecutionOrderYarnReceive.OrderQty = oReader.GetDouble("OrderQty");
            oFabricExecutionOrderYarnReceive.ReceiveQty = oReader.GetDouble("ReceiveQty");
            oFabricExecutionOrderYarnReceive.ReceiveBy = oReader.GetInt32("ReceiveBy");
            oFabricExecutionOrderYarnReceive.ReceiveDate = oReader.GetDateTime("ReceiveDate");
            oFabricExecutionOrderYarnReceive.DestinationLotID = oReader.GetInt32("DestinationLotID");
            oFabricExecutionOrderYarnReceive.WUID = oReader.GetInt32("WUID");

            oFabricExecutionOrderYarnReceive.DyeingOrderID = oReader.GetInt32("DyeingOrderID");
            oFabricExecutionOrderYarnReceive.DyeingOrderDetailID = oReader.GetInt32("DyeingOrderDetailID");
            oFabricExecutionOrderYarnReceive.FEOSDID = oReader.GetInt32("FEOSDID");
            oFabricExecutionOrderYarnReceive.ProductID = oReader.GetInt32("ProductID");
            oFabricExecutionOrderYarnReceive.ProductName = oReader.GetString("ProductName");
            oFabricExecutionOrderYarnReceive.ProductCode = oReader.GetString("ProductCode");
            oFabricExecutionOrderYarnReceive.LotNo = oReader.GetString("LotNo");
            oFabricExecutionOrderYarnReceive.DestinationLotNo = oReader.GetString("DestinationLotNo");
            oFabricExecutionOrderYarnReceive.LotBalance = oReader.GetDouble("LotBalance");
            oFabricExecutionOrderYarnReceive.UnitPrice = oReader.GetDouble("UnitPrice");
            oFabricExecutionOrderYarnReceive.UnitName = oReader.GetString("UnitName");
            oFabricExecutionOrderYarnReceive.WarpWeftType = (EnumWarpWeft) oReader.GetInt32("WarpWeftType");

            oFabricExecutionOrderYarnReceive.BuyerID = oReader.GetInt32("BuyerID");
            oFabricExecutionOrderYarnReceive.DispoNo = oReader.GetString("DispoNo");
            oFabricExecutionOrderYarnReceive.OrderType = (EnumOrderType)oReader.GetInt16("OrderType");
            oFabricExecutionOrderYarnReceive.IsInHouse = oReader.GetBoolean("IsInHouse");
            oFabricExecutionOrderYarnReceive.RequisitionNo = oReader.GetString("RequisitionNo");
            oFabricExecutionOrderYarnReceive.BuyerName = oReader.GetString("BuyerName");
            oFabricExecutionOrderYarnReceive.ColorName = oReader.GetString("ColorName");
            oFabricExecutionOrderYarnReceive.Warp = oReader.GetString("Warp");
            oFabricExecutionOrderYarnReceive.Length = oReader.GetString("Length");
            oFabricExecutionOrderYarnReceive.Remarks = oReader.GetString("Remarks");
            oFabricExecutionOrderYarnReceive.HanksCone = oReader.GetInt32("HanksCone");
            oFabricExecutionOrderYarnReceive.BagQty = oReader.GetInt32("BagQty");
            oFabricExecutionOrderYarnReceive.BalanceQty = oReader.GetDouble("BalanceQty");
            oFabricExecutionOrderYarnReceive.Dia = oReader.GetString("Dia");
            oFabricExecutionOrderYarnReceive.TFLength = oReader.GetDouble("TFLength");
            oFabricExecutionOrderYarnReceive.TFLengthLB = oReader.GetDouble("TFLengthLB");
            oFabricExecutionOrderYarnReceive.BeamCount = oReader.GetInt32("BeamCount");
            oFabricExecutionOrderYarnReceive.TFConeSet = oReader.GetInt32("TFConeSet");
            oFabricExecutionOrderYarnReceive.BeamNo = oReader.GetString("BeamNo");

            oFabricExecutionOrderYarnReceive.IssuedLength = oReader.GetDouble("IssuedLength");
            oFabricExecutionOrderYarnReceive.ReqCones = oReader.GetDouble("ReqCones");
            oFabricExecutionOrderYarnReceive.NumberOfCone = oReader.GetInt32("NumberOfCone");
            oFabricExecutionOrderYarnReceive.RequiredWarpLength = oReader.GetDouble("RequiredWarpLength");
            oFabricExecutionOrderYarnReceive.BeamFinish = oReader.GetDouble("BeamFinish");

            oFabricExecutionOrderYarnReceive.ExeNo = oReader.GetString("ExeNo");
            oFabricExecutionOrderYarnReceive.FEOSID = oReader.GetInt32("FEOSID");
            oFabricExecutionOrderYarnReceive.ReceiveByName = oReader.GetString("ReceiveByName");
            oFabricExecutionOrderYarnReceive.IssueDate = oReader.GetDateTime("IssueDate");
            oFabricExecutionOrderYarnReceive.IssueBy = oReader.GetInt32("IssueBy");
            oFabricExecutionOrderYarnReceive.IssueByName = oReader.GetString("IssueByName");
            oFabricExecutionOrderYarnReceive.WYarnType = (EnumWYarnType)oReader.GetInt32("WYarnType");
            oFabricExecutionOrderYarnReceive.RequisitionType = (EnumInOutType)oReader.GetInt32("RequisitionType");
            
            return oFabricExecutionOrderYarnReceive;
        }
        

        public static FabricExecutionOrderYarnReceive CreateObject(NullHandler oReader)
        {
            FabricExecutionOrderYarnReceive oFabricExecutionOrderYarnReceive = new FabricExecutionOrderYarnReceive();
            oFabricExecutionOrderYarnReceive = MapObject(oReader);
            return oFabricExecutionOrderYarnReceive;
        }

        private List<FabricExecutionOrderYarnReceive> CreateObjects(IDataReader oReader)
        {
            List<FabricExecutionOrderYarnReceive> oFabricExecutionOrderYarnReceives = new List<FabricExecutionOrderYarnReceive>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricExecutionOrderYarnReceive oItem = CreateObject(oHandler);
                oFabricExecutionOrderYarnReceives.Add(oItem);
            }
            return oFabricExecutionOrderYarnReceives;
        }

        #endregion

        #region Interface implementation
        public FabricExecutionOrderYarnReceiveService() { }
        public FabricExecutionOrderYarnReceive Get(int nFNExeFRID, Int64 nUserId)
        {
            FabricExecutionOrderYarnReceive oFabricExecutionOrderYarnReceive = new FabricExecutionOrderYarnReceive();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricExecutionOrderYarnReceiveDA.Get(tc, nFNExeFRID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricExecutionOrderYarnReceive = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oFabricExecutionOrderYarnReceive.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }

            return oFabricExecutionOrderYarnReceive;
        }
        public List<FabricExecutionOrderYarnReceive> Gets(string sSQL, Int64 nUserId)
        {
            List<FabricExecutionOrderYarnReceive> oFabricExecutionOrderYarnReceives = new List<FabricExecutionOrderYarnReceive>();
            FabricExecutionOrderYarnReceive oFabricExecutionOrderYarnReceive = new FabricExecutionOrderYarnReceive();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricExecutionOrderYarnReceiveDA.Gets(tc, sSQL);
                oFabricExecutionOrderYarnReceives = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oFabricExecutionOrderYarnReceive.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                oFabricExecutionOrderYarnReceives.Add(oFabricExecutionOrderYarnReceive);
                #endregion
            }

            return oFabricExecutionOrderYarnReceives;
        }
        public List<FabricExecutionOrderYarnReceive> Gets(int nWYID, Int64 nUserId)
        {
            List<FabricExecutionOrderYarnReceive> oFabricExecutionOrderYarnReceives = new List<FabricExecutionOrderYarnReceive>();
            FabricExecutionOrderYarnReceive oFabricExecutionOrderYarnReceive = new FabricExecutionOrderYarnReceive();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricExecutionOrderYarnReceiveDA.Gets(tc, nWYID);
                oFabricExecutionOrderYarnReceives = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oFabricExecutionOrderYarnReceive.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                oFabricExecutionOrderYarnReceives.Add(oFabricExecutionOrderYarnReceive);
                #endregion
            }

            return oFabricExecutionOrderYarnReceives;
        }
        //public FabricExecutionOrderYarnReceive Receive(FabricExecutionOrderYarnReceive oFEOYR, Int64 nUserId)
        //{
        //    TransactionContext tc = null;
        //    try
        //    {
        //        tc = TransactionContext.Begin(true);
        //        IDataReader reader;
        //        NullHandler oReader;

        //        //Lot Create
        //        Lot oLot = new Lot();
        //        if (oFEOYR.LotNo.Trim() == "") { throw new Exception("Please enter lot no"); }

        //        string sSQL = "";
        //        //if(oFEOYR.ChallanDetailID>0){
        //        //    if(oFEOYR.LotID <= 0){ throw new Exception("Challan lot not found."); }
        //        //    sSQL = "Select top(1)* from Lot Where ParentType=" + (int)EnumTriggerParentsType._YarnReceiveForFabricExecution+ " And WorkingUnitID=" + oFEOYR.WUID + " And ParentLotID=" + oFEOYR.LotID + "";
        //        //    reader = LotDA.Gets(tc, sSQL);
        //        //    if (reader.Read()) { oLot.LotID = (int)reader["LotID"]; }
        //        //    reader.Close();
        //        //}
        //         if (oFEOYR.LotID > 0)
        //        {
        //            sSQL = "Select top(1)* from Lot Where ParentType=" + (int)EnumTriggerParentsType._YarnReceiveForFabricExecution+ " And WorkingUnitID=" + oFEOYR.WUID + " And LotID=" + oFEOYR.LotID + "";
        //            reader = LotDA.Gets(tc, sSQL);
        //            if (reader.Read()) { oLot.LotID = (int)reader["LotID"]; }
        //            reader.Close();
        //        }

        //        //if (oLot.LotID > 0 || oFEOYR.LotID > 0) { oFEOYR.LotID = (oFEOYR.LotID>0)?oFEOYR.LotID: oLot.LotID; }
        //        //else
        //        if (oLot.LotID<=0)
        //        {
        //            oLot.ProductID = oFEOYR.ProductID;
        //            oLot.LotNo = oFEOYR.LotNo.Trim();
        //            oLot.LogNo = oFEOYR.LotNo.Trim();
        //            oLot.UnitPrice = 1;
        //            oLot.ParentType = EnumTriggerParentsType._YarnReceiveForFabricExecution;
        //            oLot.ParentLotID = oFEOYR.LotID;
        //            oLot.ParentID = oFEOYR.FEOYID;// Parent ID
        //            oLot.WorkingUnitID = oFEOYR.WUID;// Select from GUI
        //            reader = LotDA.InsertUpdate(tc, oLot, EnumDBOperation.Insert, nUserId);
        //            oReader = new NullHandler(reader);
        //            if (reader.Read())
        //            {
        //                oLot = new Lot();
        //                oLot = LotService.CreateObject(oReader);
        //            }
        //            reader.Close();
        //            if (oLot.LotID <= 0) { throw new Exception((oLot.ErrorMessage != null && oLot.ErrorMessage != "") ? oLot.ErrorMessage : "Unable to create lot."); }                    
        //        }
                
        //        //Receive
        //        oFEOYR.LotID = oLot.LotID;// SET New lot ID to the object lotID.
        //        oFEOYR.LotNo = oLot.LotNo;
        //        reader = FabricExecutionOrderYarnReceiveDA.Receive(tc, oFEOYR, nUserId);
        //        oReader = new NullHandler(reader);
        //        if (reader.Read())
        //        {
        //            oFEOYR = new FabricExecutionOrderYarnReceive();
        //            oFEOYR = CreateObject(oReader);
        //        }
        //        reader.Close();

        //        tc.End();
        //    }
        //    catch (Exception ex)
        //    {
        //        #region Handle Exception
        //        if (tc != null)
        //            tc.HandleError();
        //        oFEOYR = new FabricExecutionOrderYarnReceive();
        //        oFEOYR.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
        //        #endregion
        //    }
        //    return oFEOYR;
        //}
        //public List<FabricExecutionOrderYarnReceive> Receives(FabricExecutionOrderYarnReceive oEOYReceive, Int64 nUserId)
        //{
        //    List<FabricExecutionOrderYarnReceive> oFEOYRs = new List<FabricExecutionOrderYarnReceive>();
        //    List<FabricExecutionOrderYarnReceive> oTempFEOYRs = new List<FabricExecutionOrderYarnReceive>();
        //    int nWUID = oEOYReceive.WUID;
        //    oTempFEOYRs = oEOYReceive.FEOYRs;
        //    TransactionContext tc = null;
        //    try
        //    {
        //        tc = TransactionContext.Begin(true);
        //        IDataReader reader;
        //        NullHandler oReader;

        //        foreach (FabricExecutionOrderYarnReceive oFEOYR in oTempFEOYRs)
        //        {
        //            oFEOYR.WUID = nWUID;
        //            //Lot Create
        //            Lot oLot = new Lot();
        //            if (oFEOYR.LotNo.Trim() == "") { throw new Exception("Please enter lot no"); }

        //            string sSQL = "";
        //            //if (oFEOYR.ChallanDetailID > 0 || oFEOYR.SUDeliveryChallanDetailID>0)
        //            //{
        //            //    if (oFEOYR.LotID <= 0) { throw new Exception("Challan lot not found."); }
        //            //    sSQL = "Select top(1)* from Lot Where ParentType=" + (int)EnumTriggerParentsType._YarnReceiveForFabricExecution + " And WorkingUnitID=" + oFEOYR.WUID + " And ParentLotID=" + oFEOYR.LotID + " And LotNo='" + oFEOYR.LotNo + "'";
        //            //    reader = LotDA.Gets(tc, sSQL);
        //            //    if (reader.Read()) { oLot.LotID = (int)reader["LotID"]; }
        //            //    reader.Close();
        //            //}
        //            if (oFEOYR.LotID > 0 && oLot.LotID<=0)
        //            {
        //                sSQL = "Select top(1)* from Lot Where ParentType=" + (int)EnumTriggerParentsType._YarnReceiveForFabricExecution + " And WorkingUnitID=" + oFEOYR.WUID + " And LotNo='" + oFEOYR.LotNo + "'";//LotID=" + oFEOYR.LotID + "";
        //                reader = LotDA.Gets(tc, sSQL);
        //                if (reader.Read()) { oLot.LotID = (int)reader["LotID"]; }
        //                reader.Close();
        //            }

        //            //else if (oFEOYR.LotID > 0)
        //            //{
        //            //    sSQL = "Select top(1)* from Lot Where ParentType=" + (int)EnumTriggerParentsType._YarnReceiveForFabricExecution + " And WorkingUnitID=" + oFEOYR.WUID + " And LotID=" + oFEOYR.LotID + "";
        //            //    reader = LotDA.Gets(tc, sSQL);
        //            //    if (reader.Read()) { oLot.LotID = (int)reader["LotID"]; }
        //            //    reader.Close();
        //            //}

        //            //if (oLot.LotID > 0 || oFEOYR.LotID > 0) { oFEOYR.LotID = (oFEOYR.LotID > 0) ? oFEOYR.LotID : oLot.LotID; }
        //            //else
        //            if (oLot.LotID <= 0)
        //            {
        //                oLot.ProductID = oFEOYR.ProductID;
        //                oLot.LotNo = oFEOYR.LotNo.Trim();
        //                oLot.LogNo = oFEOYR.LotNo.Trim();
        //                oLot.UnitPrice = 1;
        //                oLot.ParentType = EnumTriggerParentsType._YarnReceiveForFabricExecution;
        //                oLot.ParentLotID = oFEOYR.LotID;
        //                oLot.ParentID = 0;
        //                oLot.WorkingUnitID = oFEOYR.WUID;
        //                oLot.ProductID = oFEOYR.ProductID;
        //                reader = LotDA.InsertUpdate(tc, oLot, EnumDBOperation.Insert, nUserId);
        //                oReader = new NullHandler(reader);
        //                if (reader.Read())
        //                {
        //                    oLot = new Lot();
        //                    oLot = LotService.CreateObject(oReader);
        //                }
        //                reader.Close();
        //                if (oLot.LotID <= 0) { throw new Exception((oLot.ErrorMessage != null && oLot.ErrorMessage != "") ? oLot.ErrorMessage : "Unable to create lot."); }
        //            }

        //            //Receive
        //            oFEOYR.LotID = oLot.LotID;// SET New lot ID to the object lotID.
        //            reader = FabricExecutionOrderYarnReceiveDA.Receive(tc, oFEOYR, nUserId);
        //            oReader = new NullHandler(reader);
        //            if (reader.Read())
        //            {
        //                oEOYReceive = new FabricExecutionOrderYarnReceive();
        //                oEOYReceive = CreateObject(oReader);
        //            }
        //            reader.Close();
        //            oFEOYRs.Add(oEOYReceive);
        //        }
        //        tc.End();
        //    }
        //    catch (Exception ex)
        //    {
        //        #region Handle Exception
        //        if (tc != null)
        //            tc.HandleError();
        //        oEOYReceive = new FabricExecutionOrderYarnReceive();
        //        oEOYReceive.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
        //        oFEOYRs.Add(oEOYReceive);
        //        #endregion
        //    }
        //    return oFEOYRs;
        //}

        public FabricExecutionOrderYarnReceive ReceiveFEOY(FabricExecutionOrderYarnReceive oFabricExecutionOrderYarnReceive, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader = FabricExecutionOrderYarnReceiveDA.IUD(tc, oFabricExecutionOrderYarnReceive, EnumDBOperation.Receive, nUserId, "");
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricExecutionOrderYarnReceive = new FabricExecutionOrderYarnReceive();
                    oFabricExecutionOrderYarnReceive = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFabricExecutionOrderYarnReceive = new FabricExecutionOrderYarnReceive();
                oFabricExecutionOrderYarnReceive.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }
            return oFabricExecutionOrderYarnReceive;
        }
    
        public string Delete(int nFEOYID, int nFEOID, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricExecutionOrderYarnReceive oFabricExecutionOrderYarnReceive = new FabricExecutionOrderYarnReceive();
                oFabricExecutionOrderYarnReceive.FEOYID = nFEOYID;
                oFabricExecutionOrderYarnReceive.FSCDID = nFEOID;
                FabricExecutionOrderYarnReceiveDA.Delete(tc, oFabricExecutionOrderYarnReceive, EnumDBOperation.Delete, nUserId,"");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }
      
        public List<FabricExecutionOrderYarnReceive> SaveDetail(List<FabricExecutionOrderYarnReceive> oFabricExecutionOrderYarnReceives, Int64 nUserID)
        {
            FabricExecutionOrderYarnReceive oFabricExecutionOrderYarnReceive = new FabricExecutionOrderYarnReceive();
            List<FabricExecutionOrderYarnReceive> _oFabricExecutionOrderYarnReceives = new List<FabricExecutionOrderYarnReceive>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                foreach (FabricExecutionOrderYarnReceive oItem in oFabricExecutionOrderYarnReceives)
                {
                    reader = null;
                    if (oItem.FEOYID > 0)
                    {
                        reader = FabricExecutionOrderYarnReceiveDA.UpdateDetail(tc, oItem);
                        NullHandler oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oFabricExecutionOrderYarnReceive = new FabricExecutionOrderYarnReceive();
                            oFabricExecutionOrderYarnReceive = CreateObject(oReader);
                            _oFabricExecutionOrderYarnReceives.Add(oFabricExecutionOrderYarnReceive);
                        }
                    }
                    
                    reader.Close();
                }
                
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricExecutionOrderYarnReceive = new FabricExecutionOrderYarnReceive();
                oFabricExecutionOrderYarnReceive.ErrorMessage = e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save FabricExecutionOrderYarnReceive. Because of " + e.Message, e);
                _oFabricExecutionOrderYarnReceives.Add(oFabricExecutionOrderYarnReceive);
                #endregion
            }
            return _oFabricExecutionOrderYarnReceives;
        }

        public FabricExecutionOrderYarnReceive UpdateObj(FabricExecutionOrderYarnReceive oFabricExecutionOrderYarnReceive, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                FabricExecutionOrderYarnReceiveDA.UpdateObj(tc, oFabricExecutionOrderYarnReceive);
                reader = FabricExecutionOrderYarnReceiveDA.GetRequisitionDetail(tc, oFabricExecutionOrderYarnReceive.FEOYID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricExecutionOrderYarnReceive = new FabricExecutionOrderYarnReceive();
                    oFabricExecutionOrderYarnReceive = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                oFabricExecutionOrderYarnReceive.ErrorMessage = Message;
                #endregion
            }
            return oFabricExecutionOrderYarnReceive;
        }

        #endregion
    }
}
