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
	public class FNProductionBatchService : MarshalByRefObject, IFNProductionBatchService
	{
		#region Private functions and declaration

		private FNProductionBatch MapObject(NullHandler oReader)
		{
			FNProductionBatch oFNProductionBatch = new FNProductionBatch();
            oFNProductionBatch.FNPBatchID = oReader.GetInt32("FNPBatchID");
            oFNProductionBatch.FNBatchCardID = oReader.GetInt32("FNBatchCardID");
			oFNProductionBatch.FNProductionID = oReader.GetInt32("FNProductionID");
            oFNProductionBatch.FNBatchID = oReader.GetInt32("FNBatchID");
            oFNProductionBatch.FNMachineID = oReader.GetInt32("FNMachineID");
			oFNProductionBatch.StartQty = oReader.GetDouble("StartQty");
			oFNProductionBatch.EndQty = oReader.GetDouble("EndQty");
			oFNProductionBatch.StartDateTime = oReader.GetDateTime("StartDateTime");
			oFNProductionBatch.EndDateTime = oReader.GetDateTime("EndDateTime");
			oFNProductionBatch.StartBatcherID = oReader.GetInt32("StartBatcherID");
			oFNProductionBatch.EndBatcherID = oReader.GetInt32("EndBatcherID");
			oFNProductionBatch.MachineSpeed = oReader.GetInt32("MachineSpeed");
            oFNProductionBatch.StartWidth = oReader.GetDouble("StartWidth");
            oFNProductionBatch.EndWidth = oReader.GetDouble("EndWidth");
			oFNProductionBatch.FlameIntensity = oReader.GetInt32("FlameIntensity");
			oFNProductionBatch.FlamePosition = oReader.GetInt32("FlamePosition");
			oFNProductionBatch.Pressure_Bar = oReader.GetDouble("Pressure_Bar");
			oFNProductionBatch.Temp_C = oReader.GetDouble("Temp_C");
			oFNProductionBatch.Remark = oReader.GetString("Remark");
			oFNProductionBatch.StartBatcherName = oReader.GetString("StartBatcherName");
			oFNProductionBatch.EndBatcherName = oReader.GetString("EndBatcherName");
			oFNProductionBatch.BatchNo = oReader.GetString("BatchNo");
            oFNProductionBatch.FNExONo = oReader.GetString("FNExONo");
            oFNProductionBatch.Ref_FNPBatchID = oReader.GetInt32("Ref_FNPBatchID");
            oFNProductionBatch.ShadeID = (EnumShade)oReader.GetInt16("ShadeID");
            oFNProductionBatch.QCStatus = (EnumQCStatus)oReader.GetInt16("QCStatus");
            oFNProductionBatch.FNPBQualityID = oReader.GetInt32("FNPBQualityID");
            oFNProductionBatch.ShiftID = oReader.GetInt32("ShiftID");
            oFNProductionBatch.FNTPID = oReader.GetInt32("FNTPID");
            oFNProductionBatch.FNExOID = oReader.GetInt32("FNExOID");
            oFNProductionBatch.DeriveFNBatchID = oReader.GetInt32("DeriveFNBatchID");
            oFNProductionBatch.PH = oReader.GetDouble("PH");
            oFNProductionBatch.FNExOQty = oReader.GetDouble("FNExOQty");
            oFNProductionBatch.ShiftName = oReader.GetString("ShiftName");
            oFNProductionBatch.UserName = oReader.GetString("UserName");
            oFNProductionBatch.FNProcess = oReader.GetString("FNProcess");
            oFNProductionBatch.IsProduction = oReader.GetBoolean("IsProduction");
            oFNProductionBatch.FNTreatmentSubProcessID = oReader.GetInt32("FNTreatmentSubProcessID");

			return oFNProductionBatch;
		}

		private FNProductionBatch CreateObject(NullHandler oReader)
		{
			FNProductionBatch oFNProductionBatch = new FNProductionBatch();
			oFNProductionBatch = MapObject(oReader);
			return oFNProductionBatch;
		}

		private List<FNProductionBatch> CreateObjects(IDataReader oReader)
		{
			List<FNProductionBatch> oFNProductionBatch = new List<FNProductionBatch>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				FNProductionBatch oItem = CreateObject(oHandler);
				oFNProductionBatch.Add(oItem);
			}
			return oFNProductionBatch;
		}

		#endregion

		#region Interface implementation
		public FNProductionBatch Save(FNProductionBatch oFNProductionBatch, Int64 nUserID)
		{
            TransactionContext tc = null;
            FNProduction oFNProduction = new FNProduction();
            oFNProduction = oFNProductionBatch.FNProduction;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                NullHandler oReader;
                if (oFNProductionBatch.FNProductionID == 0)
                {
                    if (oFNProductionBatch.FNProduction != null)
                    {
                        reader = FNProductionDA.InsertUpdate(tc, oFNProductionBatch.FNProduction, (int)EnumDBOperation.Insert, nUserID);
                        oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oFNProduction = FNProductionService.CreateObject(oReader);
                        }
                        reader.Close();
                        oFNProductionBatch.FNProductionID = oFNProduction.FNProductionID;
                    }
                    else { throw new Exception("No FN Execution Order information found to save."); }
                }
                if (oFNProductionBatch.FNPBatchID <= 0)
                {
                    reader = FNProductionBatchDA.InsertUpdate(tc, oFNProductionBatch, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = FNProductionBatchDA.InsertUpdate(tc, oFNProductionBatch, EnumDBOperation.Update, nUserID);
                }
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNProductionBatch = new FNProductionBatch();
                    oFNProductionBatch = CreateObject(oReader);
                }
                  
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFNProductionBatch.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }
            oFNProductionBatch.FNProduction = oFNProduction;
            return oFNProductionBatch;
		}
        public FNProductionBatch Save_Process(FNProductionBatch oFNProductionBatch, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                NullHandler oReader;

                string sFNProductionConsumptionIDs ="";
                FNProductionConsumption oFNProductionConsumption = new FNProductionConsumption();
                List<FNProductionConsumption> oFNPConsumptions = new List<FNProductionConsumption>();
                oFNPConsumptions = oFNProductionBatch.FNProductionConsumptions;

                if (oFNProductionBatch.FNPBatchID <= 0)
                {
                    reader = FNProductionBatchDA.InsertUpdate(tc, oFNProductionBatch, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = FNProductionBatchDA.InsertUpdate(tc, oFNProductionBatch, EnumDBOperation.Update, nUserID);
                }
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNProductionBatch = new FNProductionBatch();
                    oFNProductionBatch = CreateObject(oReader);
                }
                reader.Close();

                #region FNProductionConsumption Part

                foreach (FNProductionConsumption oItem in oFNPConsumptions)
                {
                    IDataReader readerdetail;
                    oItem.FNPBatchID = oFNProductionBatch.FNPBatchID;
                    if (oItem.FNPConsumptionID <= 0)
                    {
                        readerdetail = FNProductionConsumptionDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        readerdetail = FNProductionConsumptionDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                    }
                    if (readerdetail.Read())
                    {
                       NullHandler oReaderDetail = new NullHandler(readerdetail);
                       sFNProductionConsumptionIDs = sFNProductionConsumptionIDs + oReaderDetail.GetString("FNPConsumptionID") + ",";
                       readerdetail.Close();
                    }
                    readerdetail.Close();
                }

                if (sFNProductionConsumptionIDs.Length>0)
                    sFNProductionConsumptionIDs = sFNProductionConsumptionIDs.Substring(0, sFNProductionConsumptionIDs.Length - 1);

                oFNProductionConsumption.FNPBatchID = oFNProductionBatch.FNPBatchID;
              //  FNProductionConsumptionDA.Delete(tc, oFNProductionConsumption, EnumDBOperation.Delete, sFNProductionConsumptionIDs, nUserID);
                #endregion

                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFNProductionBatch.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }
            return oFNProductionBatch;
        }

		public string Delete(int id, Int64 nUserId)
		{
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin(true);
				FNProductionBatch oFNProductionBatch = new FNProductionBatch();
				oFNProductionBatch.FNPBatchID = id;					
				FNProductionBatchDA.Delete(tc, oFNProductionBatch, EnumDBOperation.Delete, nUserId);
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

		public FNProductionBatch Get(int id, Int64 nUserId)
		{
			FNProductionBatch oFNProductionBatch = new FNProductionBatch();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = FNProductionBatchDA.Get(tc, id);
				NullHandler oReader = new NullHandler(reader);
				if (reader.Read())
				{
				oFNProductionBatch = CreateObject(oReader);
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
				throw new ServiceException("Failed to Get FNProductionBatch", e);
				#endregion
			}
			return oFNProductionBatch;
		}

		public List<FNProductionBatch> Gets(Int64 nUserID)
		{
			List<FNProductionBatch> oFNProductionBatchs = new List<FNProductionBatch>();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = null;
				reader = FNProductionBatchDA.Gets(tc);
				oFNProductionBatchs = CreateObjects(reader);
				reader.Close();
				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exception
				if (tc != null)
				tc.HandleError();
				FNProductionBatch oFNProductionBatch = new FNProductionBatch();
				oFNProductionBatch.ErrorMessage =  e.Message.Split('!')[0];
				#endregion
			}
			return oFNProductionBatchs;
		}
            
        public List<FNProductionBatch> Gets(int id, Int64 nUserID)
        {
            List<FNProductionBatch> oFNProductionBatchs = new List<FNProductionBatch>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FNProductionBatchDA.Gets(id, tc);
                oFNProductionBatchs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FNProductionBatch oFNProductionBatch = new FNProductionBatch();
                oFNProductionBatch.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFNProductionBatchs;
        }
		public List<FNProductionBatch> Gets (string sSQL, Int64 nUserID)
		{
			List<FNProductionBatch> oFNProductionBatchs = new List<FNProductionBatch>();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = null;
				reader = FNProductionBatchDA.Gets(tc, sSQL);
				oFNProductionBatchs = CreateObjects(reader);
				reader.Close();
				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exception
				if (tc != null)
				tc.HandleError();
				ExceptionLog.Write(e);
				throw new ServiceException("Failed to Get FNProductionBatch", e);
				#endregion
			}
			return oFNProductionBatchs;
		}
        public FNProductionBatch QCRequest(FNProductionBatch oFNProductionBatch, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            IDataReader reader;
            NullHandler oReader;
            try
            {
                tc = TransactionContext.Begin(true);
                reader = FNProductionBatchDA.InsertUpdate(tc, oFNProductionBatch, eEnumDBOperation, nUserID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNProductionBatch = new FNProductionBatch();
                    oFNProductionBatch = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oFNProductionBatch = new FNProductionBatch();
                oFNProductionBatch.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFNProductionBatch;
        }
		#endregion
	}
}
