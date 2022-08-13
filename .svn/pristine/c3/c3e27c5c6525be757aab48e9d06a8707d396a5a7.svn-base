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
    public class FNOrderFabricReceiveService : MarshalByRefObject, IFNOrderFabricReceiveService
    {
        #region Private functions and declaration
        private static FNOrderFabricReceive MapObject(NullHandler oReader)
        {
            FNOrderFabricReceive oFNOrderFabricReceive = new FNOrderFabricReceive();
            oFNOrderFabricReceive.FNOrderFabricReceiveID = oReader.GetInt32("FNOrderFabricReceiveID");
            oFNOrderFabricReceive.FSCDID = oReader.GetInt32("FSCDID");
            oFNOrderFabricReceive.LotID = oReader.GetInt32("LotID");
            oFNOrderFabricReceive.Qty = oReader.GetDouble("Qty");
            oFNOrderFabricReceive.QtyTrIn = oReader.GetDouble("QtyTrIn");
            oFNOrderFabricReceive.QtyTrOut = oReader.GetDouble("QtyTrOut");
            oFNOrderFabricReceive.QtyReturn = oReader.GetDouble("QtyReturn");
            oFNOrderFabricReceive.QtyCon = oReader.GetDouble("QtyCon");
            oFNOrderFabricReceive.Grade = oReader.GetInt32("Grade");
            oFNOrderFabricReceive.FabricReqRollID = oReader.GetInt32("FabricReqRollID");
            oFNOrderFabricReceive.ReceiveBy = oReader.GetInt32("ReceiveBy");
            oFNOrderFabricReceive.ReceiveDate = oReader.GetDateTime("ReceiveDate");
            oFNOrderFabricReceive.WUID = oReader.GetInt32("WUID");
            oFNOrderFabricReceive.LotNo = oReader.GetString("LotNo");
            oFNOrderFabricReceive.FabricSource = oReader.GetString("FabricSource");
            oFNOrderFabricReceive.ReceiveByName = oReader.GetString("ReceiveByName");
            oFNOrderFabricReceive.UnitPrice = oReader.GetDouble("UnitPrice");
            oFNOrderFabricReceive.FEOID = oReader.GetInt32("FEOID");
            oFNOrderFabricReceive.FNExONo = oReader.GetString("FNExONo");
            oFNOrderFabricReceive.IssueDate = oReader.GetDateTime("IssueDate");
            //oFNOrderFabricReceive.ProcessType = (EnumFabricProcessType)oReader.GetInt16("ProcessType");
            oFNOrderFabricReceive.FabricProcessID = oReader.GetInt32("FabricProcessID");
            oFNOrderFabricReceive.ProcessTypeName = oReader.GetString("ProcessTypeName");
            oFNOrderFabricReceive.ProductID = oReader.GetInt32("ProductID");
            oFNOrderFabricReceive.ProductName = oReader.GetString("ProductName");
            oFNOrderFabricReceive.ProductCode = oReader.GetString("ProductCode");
            oFNOrderFabricReceive.ChallanNo = oReader.GetString("ChallanNo");
            oFNOrderFabricReceive.BuyerID = oReader.GetInt32("BuyerID");
            oFNOrderFabricReceive.BuyerName = oReader.GetString("BuyerName");
            oFNOrderFabricReceive.FabricNo = oReader.GetString("FabricNo");
            oFNOrderFabricReceive.Construction = oReader.GetString("Construction");
            oFNOrderFabricReceive.OrderType = (EnumOrderType)oReader.GetInt16("OrderType");
            oFNOrderFabricReceive.IsInHouse = oReader.GetBoolean("IsInHouse");
            oFNOrderFabricReceive.MUName = oReader.GetString("MUName");
            oFNOrderFabricReceive.MUSymbol = oReader.GetString("MUSymbol");
            oFNOrderFabricReceive.LotBalance = oReader.GetDouble("LotBalance");
            return oFNOrderFabricReceive;
        }

        public static FNOrderFabricReceive CreateObject(NullHandler oReader)
        {
            FNOrderFabricReceive oFNOrderFabricReceive = new FNOrderFabricReceive();
            oFNOrderFabricReceive = MapObject(oReader);
            return oFNOrderFabricReceive;
        }

        private List<FNOrderFabricReceive> CreateObjects(IDataReader oReader)
        {
            List<FNOrderFabricReceive> oFNOrderFabricReceives = new List<FNOrderFabricReceive>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FNOrderFabricReceive oItem = CreateObject(oHandler);
                oFNOrderFabricReceives.Add(oItem);
            }
            return oFNOrderFabricReceives;
        }

        #endregion

        #region Interface implementation
        public FNOrderFabricReceiveService() { }

        public FNOrderFabricReceive IUD(FNOrderFabricReceive oFNEOFR, int nDBOperation, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                NullHandler oReader;


                reader = FNOrderFabricReceiveDA.IUDInsertUpdate(tc, oFNEOFR, nDBOperation, nUserId);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNEOFR = new FNOrderFabricReceive();
                    oFNEOFR = CreateObject(oReader);
                }
                reader.Close();
                tc.End();

                if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    oFNEOFR = new FNOrderFabricReceive();
                    oFNEOFR.ErrorMessage = Global.DeleteMessage;
                }
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFNEOFR = new FNOrderFabricReceive();
                oFNEOFR.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }
            return oFNEOFR;
        }

        public FNOrderFabricReceive Get(int nFNExeFRID, Int64 nUserId)
        {
            FNOrderFabricReceive oFNOrderFabricReceive = new FNOrderFabricReceive();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FNOrderFabricReceiveDA.Get(tc, nFNExeFRID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNOrderFabricReceive = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oFNOrderFabricReceive.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }

            return oFNOrderFabricReceive;
        }

        public List<FNOrderFabricReceive> Gets(string sSQL, Int64 nUserId)
        {
            List<FNOrderFabricReceive> oFNOrderFabricReceives = new List<FNOrderFabricReceive>();
            FNOrderFabricReceive oFNOrderFabricReceive = new FNOrderFabricReceive();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FNOrderFabricReceiveDA.Gets(tc, sSQL);
                oFNOrderFabricReceives = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oFNOrderFabricReceive.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                oFNOrderFabricReceives.Add(oFNOrderFabricReceive);
                #endregion
            }

            return oFNOrderFabricReceives;
        }
   

        public List<FNOrderFabricReceive> Receive(List<FNOrderFabricReceive> oFNOrderFabricReceives, Int64 nUserID)
        {
            FNOrderFabricReceive oFNOrderFabricReceive = new FNOrderFabricReceive();
            List<FNOrderFabricReceive> oTempFNOrderFabricReceives = new List<FNOrderFabricReceive>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                foreach (FNOrderFabricReceive oItem in oFNOrderFabricReceives)
                {
                    if (oItem.ReceiveBy == 0)
                    {
                        IDataReader reader;
                        reader = FNOrderFabricReceiveDA.IUDInsertUpdate(tc, oItem, (int)EnumDBOperation.Receive, nUserID);
                        NullHandler oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oFNOrderFabricReceive = new FNOrderFabricReceive();
                            oFNOrderFabricReceive = CreateObject(oReader);
                            oTempFNOrderFabricReceives.Add(oFNOrderFabricReceive);
                        }
                        reader.Close();

                    }

                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oTempFNOrderFabricReceives = new List<FNOrderFabricReceive>();
                    oFNOrderFabricReceive = new FNOrderFabricReceive();
                    oFNOrderFabricReceive.ErrorMessage = e.Message.Split('!')[0];
                    oTempFNOrderFabricReceives.Add(oFNOrderFabricReceive);
                }
                #endregion
            }
            return oTempFNOrderFabricReceives;
        }

        public List<FNOrderFabricReceive> SaveList(List<FNOrderFabricReceive> oFNOrderFabricReceives, Int64 nUserID)
        {
            FNOrderFabricReceive oFNOrderFabricReceive = new FNOrderFabricReceive();
            List<FNOrderFabricReceive> _oFNOrderFabricReceives = new List<FNOrderFabricReceive>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                foreach (FNOrderFabricReceive oTempFNOrderFabricReceive in oFNOrderFabricReceives)
                {
                    IDataReader reader;
                    if (oTempFNOrderFabricReceive.FNOrderFabricReceiveID <= 0)
                    {
                        reader = FNOrderFabricReceiveDA.IUDInsertUpdate(tc, oTempFNOrderFabricReceive, (int)EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        reader = FNOrderFabricReceiveDA.IUDInsertUpdate(tc, oTempFNOrderFabricReceive, (int)EnumDBOperation.Update, nUserID);
                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFNOrderFabricReceive = new FNOrderFabricReceive();
                        oFNOrderFabricReceive = CreateObject(oReader);
                    }
                    reader.Close();
                    _oFNOrderFabricReceives.Add(oFNOrderFabricReceive);
                }
                tc.End();

            }
            catch (Exception e)
            {
                if (tc != null)
                {
                    tc.HandleError();
                    oFNOrderFabricReceive = new FNOrderFabricReceive();
                    oFNOrderFabricReceive.ErrorMessage = e.Message;
                    _oFNOrderFabricReceives = new List<FNOrderFabricReceive>();
                    _oFNOrderFabricReceives.Add(oFNOrderFabricReceive);
                }
            }
            return _oFNOrderFabricReceives;
        }








        //public FNOrderFabricReceive FNExeorderReceiveByChallan(FNOrderFabricReceive oFNEOFR, Int64 nUserId)
        //{
        //    TransactionContext tc = null;
        //    try
        //    {
        //        tc = TransactionContext.Begin(true);
        //        IDataReader reader;
        //        NullHandler oReader;
                
        //        //Lot Create
        //        if (oFNEOFR.LotNo.Trim() == "") { throw new Exception("Please enter lot no"); }
        //        if (oFNEOFR.LotID <= 0) { throw new Exception("Challan lot not found."); }
        //        if (oFNEOFR.FTPLDetailID > 0)
        //        {
        //            Lot oLot = new Lot();
        //            string sSQL = "Select top(1)* from Lot Where ParentType=116 And WorkingUnitID=" + oFNEOFR.WUID + " And ParentLotID=" + oFNEOFR.LotID + "";

        //            reader = LotDA.Gets(tc, sSQL);
        //            if (reader.Read()) { oLot.LotID = (int)reader["LotID"]; }
        //            reader.Close();

        //            if (oLot.LotID>0) { oFNEOFR.LotID = oLot.LotID; }
        //            else
        //            {
        //                oLot.ProductID = oFNEOFR.ProductID;
        //                oLot.LotNo = oFNEOFR.LotNo.Trim();
        //                oLot.LogNo = oFNEOFR.LotNo.Trim();
        //                oLot.UnitPrice = oFNEOFR.UnitPrice;
        //                oLot.ParentType = EnumTriggerParentsType._FabricReceiveForFinishing;
        //                oLot.ParentLotID = oFNEOFR.LotID;
        //                oLot.ParentID = 0;
        //                oLot.WorkingUnitID = oFNEOFR.WUID;
        //                reader = LotDA.IUD(tc, oLot, (int)EnumDBOperation.Insert, nUserId);
        //                oReader = new NullHandler(reader);
        //                if (reader.Read())
        //                {
        //                    oLot = new Lot();
        //                    oLot = LotService.CreateObject(oReader);
        //                }
        //                reader.Close();
        //                if (oLot.LotID <= 0) { throw new Exception((oLot.ErrorMessage != null && oLot.ErrorMessage != "") ? oLot.ErrorMessage : "Unable to create lot."); }
        //                oFNEOFR.LotID = oLot.LotID;
        //            }
                    

                   
        //        }
                

        //        // Insert FNExeOrderFabricReceive
        //        reader = FNOrderFabricReceiveDA.IUD(tc, oFNEOFR, (int)EnumDBOperation.Insert, nUserId);
        //        oReader = new NullHandler(reader);
        //        if (reader.Read())
        //        {
        //            oFNEOFR = new FNOrderFabricReceive();
        //            oFNEOFR = CreateObject(oReader);
        //        }
        //        reader.Close();

        //        //Receive
        //        reader = FNOrderFabricReceiveDA.Receive(tc, oFNEOFR.FNExeFRID, nUserId);
        //        oReader = new NullHandler(reader);
        //        if (reader.Read())
        //        {
        //            oFNEOFR = new FNOrderFabricReceive();
        //            oFNEOFR = CreateObject(oReader);
        //        }
        //        reader.Close();

        //        tc.End();
        //    }
        //    catch (Exception ex)
        //    {
        //        #region Handle Exception
        //        if (tc != null)
        //            tc.HandleError();
        //        oFNEOFR = new FNOrderFabricReceive();
        //        oFNEOFR.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
        //        #endregion
        //    }
        //    return oFNEOFR;
        //}


        #endregion
    }
}
