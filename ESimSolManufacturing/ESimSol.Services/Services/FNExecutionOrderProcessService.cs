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
	public class FNExecutionOrderProcessService : MarshalByRefObject, IFNExecutionOrderProcessService
	{
		#region Private functions and declaration

		private FNExecutionOrderProcess MapObject(NullHandler oReader)
		{
			FNExecutionOrderProcess oFNExecutionOrderProcess = new FNExecutionOrderProcess();
			oFNExecutionOrderProcess.FNExOProcessID = oReader.GetInt32("FNExOProcessID");
			oFNExecutionOrderProcess.FNExOID = oReader.GetInt32("FNExOID");
			oFNExecutionOrderProcess.Remark = oReader.GetString("Remark");
			oFNExecutionOrderProcess.Sequence = oReader.GetInt32("Sequence");
			oFNExecutionOrderProcess.FNTPID = oReader.GetInt32("FNTPID");
			oFNExecutionOrderProcess.FNTreatment = (EnumFNTreatment) oReader.GetInt32("FNTreatment");
            oFNExecutionOrderProcess.FNProcess = oReader.GetString("FNProcess");
            oFNExecutionOrderProcess.Code = oReader.GetString("Code");

			return oFNExecutionOrderProcess;
		}

		public FNExecutionOrderProcess CreateObject(NullHandler oReader)
		{
			FNExecutionOrderProcess oFNExecutionOrderProcess = new FNExecutionOrderProcess();
			oFNExecutionOrderProcess = MapObject(oReader);
			return oFNExecutionOrderProcess;
		}

		public List<FNExecutionOrderProcess> CreateObjects(IDataReader oReader)
		{
			List<FNExecutionOrderProcess> oFNExecutionOrderProcess = new List<FNExecutionOrderProcess>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				FNExecutionOrderProcess oItem = CreateObject(oHandler);
				oFNExecutionOrderProcess.Add(oItem);
			}
			return oFNExecutionOrderProcess;
		}

		#endregion

		#region Interface implementation
        //public FNExecutionOrder Save(FNExecutionOrder oFNExecutionOrder, Int64 nUserID)
        //{
        //    TransactionContext tc = null;
        //    List<FNExecutionOrderProcess> oFNExecutionOrderProcessList = new List<FNExecutionOrderProcess>();
        //    try
        //    {
        //        tc = TransactionContext.Begin(true);
        //        IDataReader reader;
        //        oFNExecutionOrderProcessList = oFNExecutionOrder.FNExecutionOrderProcessList;
        //        foreach (FNExecutionOrderProcess oItem in oFNExecutionOrderProcessList)
        //        {
        //            oItem.FNExOID = oFNExecutionOrder.FNExOID;
        //            if (oItem.FNExOProcessID <= 0)
        //            {
        //                reader = FNExecutionOrderProcessDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
        //            }
        //            else
        //            {
        //                reader = FNExecutionOrderProcessDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
        //            }
        //            NullHandler oReader = new NullHandler(reader);
        //            if (reader.Read())
        //            {

        //            }
        //            reader.Close();
        //        }
        //        reader = FNExecutionOrderProcessDA.Gets(oFNExecutionOrder.FNExOID, tc);
        //        oFNExecutionOrder.FNExecutionOrderProcessList = CreateObjects(reader);
        //        reader.Close();
        //        tc.End();
        //    }
        //    catch (Exception e)
        //    {
        //        #region Handle Exception
        //        if (tc != null)
        //        {
        //            tc.HandleError();
        //            oFNExecutionOrder = new FNExecutionOrder();
        //            oFNExecutionOrder.ErrorMessage = e.Message.Split('!')[0];
        //        }
        //        #endregion
        //    }
        //    return oFNExecutionOrder;
        //}


        public FNExecutionOrderProcess UpDown(FNExecutionOrderProcess oFNExecutionOrderProcess, Int64 nUserID)
        {

            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);
                reader = FNExecutionOrderProcessDA.UpDown(tc, oFNExecutionOrderProcess, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNExecutionOrderProcess = new FNExecutionOrderProcess();
                    oFNExecutionOrderProcess = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFNExecutionOrderProcess = new FNExecutionOrderProcess();
                oFNExecutionOrderProcess.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion

            }
            return oFNExecutionOrderProcess;
        }
        public List<FNExecutionOrderProcess> RefreshSequence(List<FNExecutionOrderProcess> oFNExecutionOrderProcess, Int64 nUserId)
        {
            List<FNExecutionOrderProcess> oFNExecutionOrderProcessRet = new List<FNExecutionOrderProcess>();
            FNExecutionOrderProcess oFNExecutionOrderP = new FNExecutionOrderProcess();
            //oFNExecutionOrderProcessRet
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                if (oFNExecutionOrderProcess.Count > 0)
                {
                    foreach (FNExecutionOrderProcess oItem in oFNExecutionOrderProcess)
                    {
                        if (oItem.FNExOProcessID > 0 && oItem.Sequence > 0)
                        {
                            FNExecutionOrderProcessDA.UpdateSequence(tc, oItem);
                            oFNExecutionOrderProcessRet.Add(oItem);
                        }
                    }
                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFNExecutionOrderProcessRet = new List<FNExecutionOrderProcess>();
                oFNExecutionOrderP = new FNExecutionOrderProcess();
                oFNExecutionOrderP.ErrorMessage = e.Message;
                oFNExecutionOrderProcessRet.Add(oFNExecutionOrderP);
                #endregion
            }
            return oFNExecutionOrderProcessRet;
        }
			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					FNExecutionOrderProcess oFNExecutionOrderProcess = new FNExecutionOrderProcess();
					oFNExecutionOrderProcess.FNExOProcessID = id;
					
					DBTableReferenceDA.HasReference(tc, "FNExecutionOrderProcess", id);
					FNExecutionOrderProcessDA.Delete(tc, oFNExecutionOrderProcess, EnumDBOperation.Delete, nUserId);
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

			public FNExecutionOrderProcess Get(int id, Int64 nUserId)
			{
				FNExecutionOrderProcess oFNExecutionOrderProcess = new FNExecutionOrderProcess();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = FNExecutionOrderProcessDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oFNExecutionOrderProcess = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get FNExecutionOrderProcess", e);
					#endregion
				}
				return oFNExecutionOrderProcess;
			}

			public List<FNExecutionOrderProcess> Gets(int nID, Int64 nUserID)
			{
				List<FNExecutionOrderProcess> oFNExecutionOrderProcesss = new List<FNExecutionOrderProcess>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
                    reader = FNExecutionOrderProcessDA.Gets(nID,tc);
					oFNExecutionOrderProcesss = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					FNExecutionOrderProcess oFNExecutionOrderProcess = new FNExecutionOrderProcess();
					oFNExecutionOrderProcess.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oFNExecutionOrderProcesss;
			}

			public List<FNExecutionOrderProcess> Gets (string sSQL, Int64 nUserID)
			{
				List<FNExecutionOrderProcess> oFNExecutionOrderProcesss = new List<FNExecutionOrderProcess>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = FNExecutionOrderProcessDA.Gets(tc, sSQL);
					oFNExecutionOrderProcesss = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get FNExecutionOrderProcess", e);
					#endregion
				}
				return oFNExecutionOrderProcesss;
			}

		#endregion
	}

}
