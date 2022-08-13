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
    public class TwistingService : MarshalByRefObject, ITwistingService
    {
        #region Private functions and declaration
        private Twisting MapObject(NullHandler oReader)
        {
            Twisting oTwisting = new Twisting();
            oTwisting.TwistingID = oReader.GetInt32("TwistingID");
            oTwisting.ReceiveDate = oReader.GetDateTime("ReceiveDate");
            oTwisting.DyeingOrderID = oReader.GetInt32("DyeingOrderID");
            oTwisting.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oTwisting.ProductID_TW = oReader.GetInt32("ProductID_TW");
            oTwisting.Qty = oReader.GetDouble("Qty");
            oTwisting.Note = oReader.GetString("Note");
            oTwisting.Status = (EnumTwistingStatus)oReader.GetInt32("Status");
            oTwisting.ApproveDate = oReader.GetDateTime("ApproveDate");
            oTwisting.CompletedDate = oReader.GetDateTime("CompletedDate");
            oTwisting.ApproveByID = oReader.GetInt32("ApproveByID");
            oTwisting.CompletedByID = oReader.GetInt32("CompletedByID");
            oTwisting.ApproveByName = oReader.GetString("ApproveByName");
            oTwisting.CompletedByName = oReader.GetString("CompletedByName");
            oTwisting.MachineDes = oReader.GetString("MachineDes");
            oTwisting.DyeingOrderNo = oReader.GetString("DyeingOrderNo");
            oTwisting.ProductCode = oReader.GetString("ProductCode");
            oTwisting.ProductName = oReader.GetString("ProductName");
            oTwisting.ContractorName = oReader.GetString("ContractorName");
            oTwisting.TWNo = oReader.GetString("TWNo");
            oTwisting.IsInHouse = oReader.GetBoolean("IsInHouse");
            oTwisting.CompleteWorkingUnitID = oReader.GetInt32("CompleteWorkingUnitID");
            oTwisting.TwistingOrderType = (EnumTwistingOrderType)oReader.GetInt32("TwistingOrderType");

            return oTwisting;
        }

        private Twisting CreateObject(NullHandler oReader)
        {
            Twisting oTwisting = new Twisting();
            oTwisting = MapObject(oReader);
            return oTwisting;
        }

        private List<Twisting> CreateObjects(IDataReader oReader)
        {
            List<Twisting> oTwisting = new List<Twisting>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                Twisting oItem = CreateObject(oHandler);
                oTwisting.Add(oItem);
            }
            return oTwisting;
        }

        #endregion

        #region Interface implementation
        public TwistingService() { }
        public Twisting Save(Twisting oTwisting, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<TwistingDetail> oTwistingDetails = new List<TwistingDetail>();
                oTwistingDetails = oTwisting.TwistingDetails;
                string sTwistingDetailIDS = "";

                IDataReader reader;
                if (oTwisting.TwistingID <= 0)
                {
                    oTwisting.Status = EnumTwistingStatus.Initialize;
                    reader = TwistingDA.InsertUpdate(tc, oTwisting, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = TwistingDA.InsertUpdate(tc, oTwisting, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTwisting = CreateObject(oReader);
                }
                reader.Close();

                #region Twisting Part

                foreach (TwistingDetail oItem in oTwistingDetails)
                {
                    IDataReader readerdetail;
                    oItem.TwistingID = oTwisting.TwistingID;
                    if (oItem.TwistingDetailID <= 0)
                    {
                        readerdetail = TwistingDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID,"");
                    }
                    else
                    {
                        readerdetail = TwistingDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID,"");
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sTwistingDetailIDS = sTwistingDetailIDS + oReaderDetail.GetString("TwistingDetailID") + ",";
                    }
                    readerdetail.Close();
                }
                TwistingDetail oTwistingDetail = new TwistingDetail();
                oTwistingDetail.TwistingID = oTwisting.TwistingID;
                if (sTwistingDetailIDS.Length > 0)
                {
                    sTwistingDetailIDS = sTwistingDetailIDS.Remove(sTwistingDetailIDS.Length - 1, 1);
                }
                TwistingDetailDA.Delete(tc, oTwistingDetail, EnumDBOperation.Delete, nUserID, sTwistingDetailIDS);
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
                oTwisting.ErrorMessage = Message;
                #endregion
            }

            return oTwisting;

        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                Twisting oTwisting = new Twisting();
                oTwisting.TwistingID = id;
                TwistingDA.Delete(tc, oTwisting, EnumDBOperation.Delete, nUserId);
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

        public Twisting Approve(Twisting oTwisting, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                //IDataReader reader = TwistingDA.Approve(tc, oTwisting, EnumDBOperation.Approval, nUserId);
                IDataReader reader = TwistingDA.InsertUpdate(tc, oTwisting, EnumDBOperation.Approval, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTwisting = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oTwisting = new Twisting();
                oTwisting.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oTwisting;
        }
        //(DB Operation: 102 For Issue, 103 For Receive) 
        public Twisting UndoApprove(Twisting oTwisting, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                //IDataReader reader = TwistingDA.UndoApprove(tc, oTwisting, EnumDBOperation.UnApproval, nUserId);
                IDataReader reader = TwistingDA.InsertUpdate(tc, oTwisting, EnumDBOperation.UnApproval, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTwisting = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oTwisting = new Twisting();
                oTwisting.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oTwisting;
        }
        public Twisting Complete(Twisting oTwisting, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                //IDataReader reader = TwistingDA.Complete(tc, oTwisting, EnumDBOperation.Delivered, nUserId);
                IDataReader reader = TwistingDA.InsertUpdate(tc, oTwisting, EnumDBOperation.Delivered, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTwisting = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oTwisting = new Twisting();
                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                oTwisting.ErrorMessage = Message;
                #endregion
            }
            return oTwisting;
        }
        
        public Twisting Get(int id, Int64 nUserId)
        {
            Twisting oTwisting = new Twisting();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = TwistingDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTwisting = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get Twisting", e);
                #endregion
            }
            return oTwisting;
        }
        public List<Twisting> Gets(Int64 nUserID)
        {
            List<Twisting> oTwistings = new List<Twisting>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TwistingDA.Gets(tc);
                oTwistings = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Twisting", e);
                #endregion
            }
            return oTwistings;
        }
        public List<Twisting> Gets(string sSQL, Int64 nUserID)
        {
            List<Twisting> oTwistings = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TwistingDA.Gets(tc, sSQL);
                oTwistings = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Twisting", e);
                #endregion
            }
            return oTwistings;
        }
        #endregion
    }
}