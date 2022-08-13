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
    public class FabricReturnChallanService : MarshalByRefObject, IFabricReturnChallanService
    {
        #region Private functions and declaration
        private static FabricReturnChallan MapObject(NullHandler oReader)
        {
            FabricReturnChallan oFabricReturnChallan = new FabricReturnChallan();
            oFabricReturnChallan.FabricReturnChallanID = oReader.GetInt32("FabricReturnChallanID");
            oFabricReturnChallan.FabricDeliveryChallanID = oReader.GetInt32("FabricDeliveryChallanID");
            oFabricReturnChallan.ReturnNo = oReader.GetString("ReturnNo");
            oFabricReturnChallan.ReturnDate = oReader.GetDateTime("ReturnDate");
            oFabricReturnChallan.StoreID = oReader.GetInt32("StoreID");
            oFabricReturnChallan.BuyerID = oReader.GetInt32("BuyerID");
            oFabricReturnChallan.Remarks = oReader.GetString("Remarks");
            oFabricReturnChallan.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oFabricReturnChallan.ApprovedDate = oReader.GetDateTime("ApprovedDate");
            oFabricReturnChallan.ReceivedBy = oReader.GetInt32("ReceivedBy");
            oFabricReturnChallan.ReceivedDate = oReader.GetDateTime("ReceivedDate");
            oFabricReturnChallan.ApprovedByName = oReader.GetString("ApprovedByName");
            oFabricReturnChallan.ReceivedByName = oReader.GetString("ReceivedByName");
            oFabricReturnChallan.BuyerID = oReader.GetInt32("BuyerID");
            oFabricReturnChallan.BuyerName = oReader.GetString("BuyerName");
            oFabricReturnChallan.ChallanNo = oReader.GetString("ChallanNo");
            oFabricReturnChallan.ChallanDate = oReader.GetDateTime("ChallanDate");
            oFabricReturnChallan.StoreName = oReader.GetString("StoreName");
            oFabricReturnChallan.IssuedByName = oReader.GetString("IssuedByName");
            oFabricReturnChallan.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oFabricReturnChallan.PartyChallanNo = oReader.GetString("PartyChallanNo");
            oFabricReturnChallan.VehicleInfo = oReader.GetString("VehicleInfo");
            oFabricReturnChallan.GetInNo = oReader.GetString("GetInNo");
            oFabricReturnChallan.ReturnPerson = oReader.GetString("ReturnPerson");
            return oFabricReturnChallan;
        }

        public static FabricReturnChallan CreateObject(NullHandler oReader)
        {
            FabricReturnChallan oFabricReturnChallan = new FabricReturnChallan();
            oFabricReturnChallan = MapObject(oReader);
            return oFabricReturnChallan;
        }

        private List<FabricReturnChallan> CreateObjects(IDataReader oReader)
        {
            List<FabricReturnChallan> oFabricReturnChallan = new List<FabricReturnChallan>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricReturnChallan oItem = CreateObject(oHandler);
                oFabricReturnChallan.Add(oItem);
            }
            return oFabricReturnChallan;
        }
        #endregion

        #region Interface implementation

        public FabricReturnChallan Save(FabricReturnChallan oFabricReturnChallan, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<FabricReturnChallanDetail> oFabricReturnChallanDetails = new List<FabricReturnChallanDetail>();
                oFabricReturnChallanDetails = oFabricReturnChallan.FRCDetails;
                string sFabricReturnChallanDetailIDS = "";

                IDataReader reader;
                if (oFabricReturnChallan.FabricReturnChallanID <= 0)
                {
                    reader = FabricReturnChallanDA.InsertUpdate(tc, oFabricReturnChallan, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = FabricReturnChallanDA.InsertUpdate(tc, oFabricReturnChallan, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricReturnChallan = CreateObject(oReader);
                }
                reader.Close();

                #region FabricReturnChallan Part

                foreach (FabricReturnChallanDetail oItem in oFabricReturnChallanDetails)
                {
                    IDataReader readerdetail;
                    oItem.FabricReturnChallanID = oFabricReturnChallan.FabricReturnChallanID;
                    if (oItem.Qty > 0)
                    {
                        if (oItem.FabricReturnChallanDetailID <= 0)
                        {
                            readerdetail = FabricReturnChallanDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            readerdetail = FabricReturnChallanDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sFabricReturnChallanDetailIDS = sFabricReturnChallanDetailIDS + oReaderDetail.GetString("FabricReturnChallanDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                }
                FabricReturnChallanDetail oFabricReturnChallanDetail = new FabricReturnChallanDetail();
                oFabricReturnChallanDetail.FabricReturnChallanID = oFabricReturnChallan.FabricReturnChallanID;
                if (sFabricReturnChallanDetailIDS.Length > 0)
                {
                    sFabricReturnChallanDetailIDS = sFabricReturnChallanDetailIDS.Remove(sFabricReturnChallanDetailIDS.Length - 1, 1);
                    FabricReturnChallanDetailDA.Delete(tc, oFabricReturnChallanDetail, EnumDBOperation.Delete, nUserID, sFabricReturnChallanDetailIDS);
                }
                #endregion
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
                oFabricReturnChallan.ErrorMessage = Message;
                #endregion
            }
            return oFabricReturnChallan;
        }


        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricReturnChallan oFabricReturnChallan = new FabricReturnChallan();
                oFabricReturnChallan.FabricReturnChallanID = id;
                FabricReturnChallanDA.Delete(tc, oFabricReturnChallan, EnumDBOperation.Delete, nUserId);
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

        public FabricReturnChallan Get(int id, Int64 nUserId)
        {
            FabricReturnChallan oFabricReturnChallan = new FabricReturnChallan();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricReturnChallanDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricReturnChallan = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFabricReturnChallan = new FabricReturnChallan();
                oFabricReturnChallan.ErrorMessage = ex.Message.Split('!')[0];
                #endregion
            }
            return oFabricReturnChallan;
        }
        public List<FabricReturnChallan> Gets(Int64 nUserID)
        {
            List<FabricReturnChallan> oFabricReturnChallans = new List<FabricReturnChallan>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricReturnChallanDA.Gets(tc);
                oFabricReturnChallans = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                FabricReturnChallan oFabricReturnChallan = new FabricReturnChallan();
                oFabricReturnChallan.ErrorMessage = ex.Message.Split('!')[0];
                oFabricReturnChallans.Add(oFabricReturnChallan);
                #endregion
            }
            return oFabricReturnChallans;
        }
        public List<FabricReturnChallan> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricReturnChallan> oFabricReturnChallans = new List<FabricReturnChallan>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricReturnChallanDA.Gets(tc, sSQL);
                oFabricReturnChallans = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                FabricReturnChallan oFabricReturnChallan = new FabricReturnChallan();
                oFabricReturnChallan.ErrorMessage = ex.Message.Split('!')[0];
                oFabricReturnChallans.Add(oFabricReturnChallan);
                #endregion
            }
            return oFabricReturnChallans;
        }

        public FabricReturnChallan ApproveOrReceive(FabricReturnChallan oFRC, int nDBOperation, Int64 nUserId)
        {
      
            TransactionContext tc = null;
            try
            {
               
                tc = TransactionContext.Begin(true);
                IDataReader reader;

                reader = FabricReturnChallanDA.ApproveOrReceive(tc, oFRC, nDBOperation, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFRC = new FabricReturnChallan();
                    oFRC = CreateObject(oReader);
                }
                reader.Close();

            

                if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    oFRC = new FabricReturnChallan();
                    oFRC.ErrorMessage = Global.DeleteMessage;
                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFRC = new FabricReturnChallan();
                oFRC.ErrorMessage = (e.Message.Contains("!")) ? e.Message.Split('!')[0] : e.Message;
                #endregion
            }
            return oFRC;
        }
        #endregion
    }
}
