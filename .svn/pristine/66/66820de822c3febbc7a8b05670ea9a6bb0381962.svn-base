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
	public class LotMixingService : MarshalByRefObject, ILotMixingService
	{
		#region Private functions and declaration

		private LotMixing MapObject(NullHandler oReader)
		{
			LotMixing oLotMixing = new LotMixing();
            oLotMixing.LotMixingID = oReader.GetInt32("LotMixingID");
            oLotMixing.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oLotMixing.SLNo = oReader.GetString("SLNo");
            oLotMixing.SLNoFull = oReader.GetString("SLNoFull");
			oLotMixing.BUID = oReader.GetInt32("BUID");
			oLotMixing.CreateDate = oReader.GetDateTime("CreateDate");
            oLotMixing.ApproveByID = oReader.GetInt32("ApproveByID");
            oLotMixing.CompleteByID = oReader.GetInt32("CompleteByID");
			oLotMixing.ApproveDate = oReader.GetDateTime("ApproveDate");
            oLotMixing.Remarks = oReader.GetString("Remarks");
            oLotMixing.BUName = oReader.GetString("BUName");
            oLotMixing.PrepareByName = oReader.GetString("PrepareByName");
            oLotMixing.ApproveByName = oReader.GetString("ApproveByName");
            oLotMixing.CompleteByName = oReader.GetString("CompleteByName");
            oLotMixing.Percentage = oReader.GetDouble("Percentage");
			return oLotMixing;
		}

		private LotMixing CreateObject(NullHandler oReader)
		{
			LotMixing oLotMixing = new LotMixing();
			oLotMixing = MapObject(oReader);
			return oLotMixing;
		}

		private List<LotMixing> CreateObjects(IDataReader oReader)
		{
			List<LotMixing> oLotMixing = new List<LotMixing>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				LotMixing oItem = CreateObject(oHandler);
				oLotMixing.Add(oItem);
			}
			return oLotMixing;
		}

		#endregion

		#region Interface implementation
        public LotMixing Save(LotMixing oLotMixing, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<LotMixingDetail> oLotMixingDetails = new List<LotMixingDetail>();
                oLotMixingDetails = oLotMixing.LotMixingDetails;
                string sLotMixingDetailIDS = "";

                IDataReader reader;
                if (oLotMixing.LotMixingID <= 0)
                {
                    oLotMixing.Status = EnumTwistingStatus.Initialize;
                    reader = LotMixingDA.InsertUpdate(tc, oLotMixing, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = LotMixingDA.InsertUpdate(tc, oLotMixing, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLotMixing = CreateObject(oReader);
                }
                reader.Close();

                #region LotMixing Part

                foreach (LotMixingDetail oItem in oLotMixingDetails)
                {
                    IDataReader readerdetail;
                    oItem.LotMixingID = oLotMixing.LotMixingID;
                    if (oItem.LotMixingDetailID <= 0)
                    {
                        readerdetail = LotMixingDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                    }
                    else
                    {
                        readerdetail = LotMixingDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sLotMixingDetailIDS = sLotMixingDetailIDS + oReaderDetail.GetString("LotMixingDetailID") + ",";
                    }
                    readerdetail.Close();
                }
                LotMixingDetail oLotMixingDetail = new LotMixingDetail();
                oLotMixingDetail.LotMixingID = oLotMixing.LotMixingID;
                if (sLotMixingDetailIDS.Length > 0)
                {
                    sLotMixingDetailIDS = sLotMixingDetailIDS.Remove(sLotMixingDetailIDS.Length - 1, 1);
                }
                LotMixingDetailDA.Delete(tc, oLotMixingDetail, EnumDBOperation.Delete, nUserID, sLotMixingDetailIDS);
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
                oLotMixing.ErrorMessage = Message;
                #endregion
            }

            return oLotMixing;

        }

        public LotMixing Approve(LotMixing oLotMixing, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oLotMixing.LotMixingID <= 0)
                
                oLotMixing.Status = EnumTwistingStatus.Initialize;
                reader = LotMixingDA.InsertUpdate(tc, oLotMixing, EnumDBOperation.Approval, nUserID);
                
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLotMixing = CreateObject(oReader);
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
                oLotMixing.ErrorMessage = Message;
                #endregion
            }

            return oLotMixing;
        }
        public LotMixing UndoApprove(LotMixing oLotMixing, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oLotMixing.LotMixingID <= 0)

                    oLotMixing.Status = EnumTwistingStatus.Initialize;
                reader = LotMixingDA.InsertUpdate(tc, oLotMixing, EnumDBOperation.UnApproval, nUserID);

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLotMixing = CreateObject(oReader);
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
                oLotMixing.ErrorMessage = Message;
                #endregion
            }

            return oLotMixing;
        }
        public LotMixing Complete(LotMixing oLotMixing, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oLotMixing.LotMixingID <= 0)

                    oLotMixing.Status = EnumTwistingStatus.Initialize;
                reader = LotMixingDA.InsertUpdate(tc, oLotMixing, EnumDBOperation.Disburse, nUserID);

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLotMixing = CreateObject(oReader);
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
                oLotMixing.ErrorMessage = Message;
                #endregion
            }

            return oLotMixing;
        }
       
		public string Delete(int id, Int64 nUserId)
		{
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin(true);
				LotMixing oLotMixing = new LotMixing();
				oLotMixing.LotMixingID = id;
				DBTableReferenceDA.HasReference(tc, "LotMixing", id);
				LotMixingDA.Delete(tc, oLotMixing, EnumDBOperation.Delete, nUserId);
				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exceptionif (tc != null)
				tc.HandleError();
				return e.Message.Split('!')[0];
				#endregion
			}
			return "Data delete successfully";
		}

		public LotMixing Get(int id, Int64 nUserId)
		{
			LotMixing oLotMixing = new LotMixing();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = LotMixingDA.Get(tc, id);
				NullHandler oReader = new NullHandler(reader);
				if (reader.Read())
				{
				oLotMixing = CreateObject(oReader);
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
				throw new ServiceException("Failed to Get LotMixing", e);
				#endregion
			}
			return oLotMixing;
		}

		public List<LotMixing> Gets(Int64 nUserID)
		{
			List<LotMixing> oLotMixings = new List<LotMixing>();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = null;
				reader = LotMixingDA.Gets(tc);
				oLotMixings = CreateObjects(reader);
				reader.Close();
				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exception
				if (tc != null)
				tc.HandleError();
				LotMixing oLotMixing = new LotMixing();
				oLotMixing.ErrorMessage =  e.Message.Split('!')[0];
				#endregion
			}
			return oLotMixings;
		}

		public List<LotMixing> Gets (string sSQL, Int64 nUserID)
		{
			List<LotMixing> oLotMixings = new List<LotMixing>();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = null;
				reader = LotMixingDA.Gets(tc, sSQL);
				oLotMixings = CreateObjects(reader);
				reader.Close();
				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exception
				if (tc != null);
				tc.HandleError();
				ExceptionLog.Write(e);
				throw new ServiceException("Failed to Get LotMixing", e);
				#endregion
			}
			return oLotMixings;
		}

		#endregion
	}

}
