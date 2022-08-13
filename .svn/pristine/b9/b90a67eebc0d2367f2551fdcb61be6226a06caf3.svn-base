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
    public class FabricTransferPackingListService : MarshalByRefObject, IFabricTransferPackingListService
    {
        #region Private functions and declaration
        public static FabricTransferPackingList MapObject(NullHandler oReader)
        {
            FabricTransferPackingList oFabricTransferPackingList = new FabricTransferPackingList();
            oFabricTransferPackingList.FTPListID = oReader.GetInt32("FTPListID");
            oFabricTransferPackingList.FTNID = oReader.GetInt32("FTNID");
            oFabricTransferPackingList.FEOID = oReader.GetInt32("FEOID");
            oFabricTransferPackingList.Note = oReader.GetString("Note");
            oFabricTransferPackingList.StoreID = oReader.GetInt32("StoreID");
            oFabricTransferPackingList.PackingListDate = oReader.GetDateTime("PackingListDate");
            oFabricTransferPackingList.FEONo = oReader.GetString("FEONo");
            oFabricTransferPackingList.CountRoll = oReader.GetInt32("CountRoll");
            oFabricTransferPackingList.TotalRollQty = oReader.GetDouble("TotalRollQty");
            oFabricTransferPackingList.IsInHouse = oReader.GetBoolean("IsInHouse");
            oFabricTransferPackingList.OrderType = (EnumOrderType)oReader.GetInt32("OrderType");
            oFabricTransferPackingList.BuyerName = oReader.GetString("BuyerName");
            oFabricTransferPackingList.FabType = oReader.GetString("FabType");
            oFabricTransferPackingList.Construction = oReader.GetString("Construction");
            oFabricTransferPackingList.FTNNo = oReader.GetString("FTNNo");
            oFabricTransferPackingList.Color = oReader.GetString("Color");
            oFabricTransferPackingList.WarpLot = oReader.GetString("WarpLot");
            oFabricTransferPackingList.WeftLot = oReader.GetString("WeftLot");
            oFabricTransferPackingList.FinishWidth = oReader.GetString("FinishWidth");
            oFabricTransferPackingList.GreyWidth = oReader.GetString("GreyWidth");
            oFabricTransferPackingList.NoteDate = oReader.GetDateTime("NoteDate");
            oFabricTransferPackingList.DetailQty = oReader.GetDouble("DetailQty");

            return oFabricTransferPackingList;
        }
        public static FabricTransferPackingList CreateObject(NullHandler oReader)
        {
            FabricTransferPackingList oFabricTransferPackingList = new FabricTransferPackingList();
            oFabricTransferPackingList = MapObject(oReader);
            return oFabricTransferPackingList;
        }
        public List<FabricTransferPackingList> CreateObjects(IDataReader oReader)
        {
            List<FabricTransferPackingList> oFabricTransferPackingList = new List<FabricTransferPackingList>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricTransferPackingList oItem = CreateObject(oHandler);
                oFabricTransferPackingList.Add(oItem);
            }
            return oFabricTransferPackingList;
        }

        #endregion

        #region Interface implementation
        public FabricTransferPackingList Save(FabricTransferPackingList oFabricTransferPackingList, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFabricTransferPackingList.FTPListID <= 0)
                {
                    reader = FabricTransferPackingListDA.InsertUpdate(tc, oFabricTransferPackingList, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = FabricTransferPackingListDA.InsertUpdate(tc, oFabricTransferPackingList, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricTransferPackingList = new FabricTransferPackingList();
                    oFabricTransferPackingList = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricTransferPackingList = new FabricTransferPackingList();
                oFabricTransferPackingList.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricTransferPackingList;
        }
    
        public string Delete(int id, int nFTNID, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricTransferPackingList oFabricTransferPackingList = new FabricTransferPackingList();
                oFabricTransferPackingList.FTPListID = id;
                oFabricTransferPackingList.FTNID = nFTNID;
                DBTableReferenceDA.HasReference(tc, "FabricTransferPackingList", id);
                FabricTransferPackingListDA.Delete(tc, oFabricTransferPackingList, EnumDBOperation.Delete, nUserId);
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
        public string Untag(int id, Int64 nUserId) //UnTag
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricTransferPackingList oFabricTransferPackingList = new FabricTransferPackingList();
                oFabricTransferPackingList.FTPListID = id;
                DBTableReferenceDA.HasReference(tc, "FabricTransferPackingList", id);
                FabricTransferPackingListDA.Untag(tc, oFabricTransferPackingList.FTPListID);
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
        public List<FabricTransferPackingList> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricTransferPackingList> oFabricTransferPackingLists = new List<FabricTransferPackingList>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricTransferPackingListDA.Gets(tc, sSQL);
                oFabricTransferPackingLists = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricTransferPackingLists = new List<FabricTransferPackingList>();
                FabricTransferPackingList oFabricTransferPackingList = new FabricTransferPackingList();
                oFabricTransferPackingList.ErrorMessage = e.Message.Split('~')[0];
                oFabricTransferPackingLists.Add(oFabricTransferPackingList);
                #endregion
            }
            return oFabricTransferPackingLists;
        }
        public List<FabricTransferPackingList> Gets(Int64 nUserID)
        {
            List<FabricTransferPackingList> oFabricTransferPackingLists = new List<FabricTransferPackingList>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricTransferPackingListDA.Gets(tc);
                oFabricTransferPackingLists = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricTransferPackingLists = new List<FabricTransferPackingList>();
                FabricTransferPackingList oFabricTransferPackingList = new FabricTransferPackingList();
                oFabricTransferPackingList.ErrorMessage = e.Message.Split('~')[0];
                oFabricTransferPackingLists.Add(oFabricTransferPackingList);
                #endregion
            }
            return oFabricTransferPackingLists;
        }
        public FabricTransferPackingList Get(int id, Int64 nUserId)
        {
            FabricTransferPackingList oFabricTransferPackingList = new FabricTransferPackingList();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricTransferPackingListDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricTransferPackingList = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricTransferPackingList = new FabricTransferPackingList();
                oFabricTransferPackingList.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricTransferPackingList;
        }

        #endregion
    }
}
