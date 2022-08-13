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
	public class RecycleProcessService : MarshalByRefObject, IRecycleProcessService
	{
		#region Private functions and declaration

		private RecycleProcess MapObject(NullHandler oReader)
		{
			RecycleProcess oRecycleProcess = new RecycleProcess();
			oRecycleProcess.RecycleProcessID = oReader.GetInt32("RecycleProcessID");
			oRecycleProcess.ProcessDate = oReader.GetDateTime("ProcessDate");
			oRecycleProcess.RefNo = oReader.GetString("RefNo");
			oRecycleProcess.Note = oReader.GetString("Note");
			oRecycleProcess.BUID = oReader.GetInt32("BUID");
            oRecycleProcess.RecycleProcessType = (EnumRecycleProcessType)oReader.GetInt32("RecycleProcessType");
			oRecycleProcess.ProductNature = (EnumProductNature) oReader.GetInt32("ProductNature");
            oRecycleProcess.ProductNatureInInt = oReader.GetInt32("ProductNature");
            oRecycleProcess.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oRecycleProcess.ApprovedByName = oReader.GetString("ApprovedByName");
            oRecycleProcess.Qty = oReader.GetDouble("Qty");
			return oRecycleProcess;
		}

		private RecycleProcess CreateObject(NullHandler oReader)
		{
			RecycleProcess oRecycleProcess = new RecycleProcess();
			oRecycleProcess = MapObject(oReader);
			return oRecycleProcess;
		}

		private List<RecycleProcess> CreateObjects(IDataReader oReader)
		{
			List<RecycleProcess> oRecycleProcess = new List<RecycleProcess>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				RecycleProcess oItem = CreateObject(oHandler);
				oRecycleProcess.Add(oItem);
			}
			return oRecycleProcess;
		}

		#endregion

		#region Interface implementation
        public RecycleProcess Save(RecycleProcess oRecycleProcess, Int64 nUserID)
        {
            TransactionContext tc = null;
            string sRecycleProcessDetailIDs = "";
            List<RecycleProcessDetail> oRecycleProcessDetails = new List<RecycleProcessDetail>();
            RecycleProcessDetail oRecycleProcessDetail = new RecycleProcessDetail();
            oRecycleProcessDetails = oRecycleProcess.RecycleProcessDetails;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oRecycleProcess.RecycleProcessID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.RecycleProcess, EnumRoleOperationType.Add);
                    reader = RecycleProcessDA.InsertUpdate(tc, oRecycleProcess, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.RecycleProcess, EnumRoleOperationType.Edit);
                    reader = RecycleProcessDA.InsertUpdate(tc, oRecycleProcess, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRecycleProcess = new RecycleProcess();
                    oRecycleProcess = CreateObject(oReader);
                }
                reader.Close();
                #region Recycle Process Detail Detail Part
                foreach (RecycleProcessDetail oItem in oRecycleProcessDetails)
                {
                    IDataReader readerdetail;
                    oItem.RecycleProcessID = oRecycleProcess.RecycleProcessID;
                    if (oItem.RecycleProcessDetailID <= 0)
                    {
                        readerdetail = RecycleProcessDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                    }
                    else
                    {
                        readerdetail = RecycleProcessDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sRecycleProcessDetailIDs = sRecycleProcessDetailIDs + oReaderDetail.GetString("RecycleProcessDetailID") + ",";
                    }
                    readerdetail.Close();
                }
                if (sRecycleProcessDetailIDs.Length > 0)
                {
                    sRecycleProcessDetailIDs = sRecycleProcessDetailIDs.Remove(sRecycleProcessDetailIDs.Length - 1, 1);
                }
                oRecycleProcessDetail = new RecycleProcessDetail();
                oRecycleProcessDetail.RecycleProcessID = oRecycleProcess.RecycleProcessID;
                RecycleProcessDetailDA.Delete(tc, oRecycleProcessDetail, EnumDBOperation.Delete, nUserID, sRecycleProcessDetailIDs);
                #endregion

                #region Get Recycle Process
                reader = RecycleProcessDA.Get(tc, oRecycleProcess.RecycleProcessID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRecycleProcess = new RecycleProcess();
                    oRecycleProcess = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oRecycleProcess = new RecycleProcess();
                    oRecycleProcess.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oRecycleProcess;
        }

        public RecycleProcess Approve(RecycleProcess oRecycleProcess, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.RecycleProcess, EnumRoleOperationType.Approved);
                reader = RecycleProcessDA.InsertUpdate(tc, oRecycleProcess, EnumDBOperation.Approval, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRecycleProcess = new RecycleProcess();
                    oRecycleProcess = CreateObject(oReader);
                }
                reader.Close();
             
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oRecycleProcess = new RecycleProcess();
                    oRecycleProcess.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oRecycleProcess;
        }

		public string Delete(int id, Int64 nUserId)
		{
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin(true);
				RecycleProcess oRecycleProcess = new RecycleProcess();
				oRecycleProcess.RecycleProcessID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.RecycleProcess, EnumRoleOperationType.Delete);
				DBTableReferenceDA.HasReference(tc, "RecycleProcess", id);
				RecycleProcessDA.Delete(tc, oRecycleProcess, EnumDBOperation.Delete, nUserId);
				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exceptionif (tc != null)
				tc.HandleError();
				return e.Message.Split('!')[0];
				#endregion
			}
			return Global.DeleteMessage;
		}

		public RecycleProcess Get(int id, Int64 nUserId)
		{
			RecycleProcess oRecycleProcess = new RecycleProcess();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = RecycleProcessDA.Get(tc, id);
				NullHandler oReader = new NullHandler(reader);
				if (reader.Read())
				{
				oRecycleProcess = CreateObject(oReader);
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
				throw new ServiceException("Failed to Get RecycleProcess", e);
				#endregion
			}
			return oRecycleProcess;
		}

		public List<RecycleProcess> Gets(Int64 nUserID)
		{
			List<RecycleProcess> oRecycleProcesss = new List<RecycleProcess>();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = null;
				reader = RecycleProcessDA.Gets(tc);
				oRecycleProcesss = CreateObjects(reader);
				reader.Close();
				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exception
				if (tc != null)
				tc.HandleError();
				RecycleProcess oRecycleProcess = new RecycleProcess();
				oRecycleProcess.ErrorMessage =  e.Message.Split('!')[0];
				#endregion
			}
			return oRecycleProcesss;
		}

		public List<RecycleProcess> Gets (string sSQL, Int64 nUserID)
		{
			List<RecycleProcess> oRecycleProcesss = new List<RecycleProcess>();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = null;
				reader = RecycleProcessDA.Gets(tc, sSQL);
				oRecycleProcesss = CreateObjects(reader);
				reader.Close();
				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exception
				if (tc != null);
				tc.HandleError();
				ExceptionLog.Write(e);
				throw new ServiceException("Failed to Get RecycleProcess", e);
				#endregion
			}
			return oRecycleProcesss;
		}

		#endregion
	}

}
