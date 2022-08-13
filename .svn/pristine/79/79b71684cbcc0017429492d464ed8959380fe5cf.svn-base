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
	public class FNTreatmentProcessService : MarshalByRefObject, IFNTreatmentProcessService
	{
		#region Private functions and declaration

		private FNTreatmentProcess MapObject(NullHandler oReader)
		{
			FNTreatmentProcess oFNTreatmentProcess = new FNTreatmentProcess();
			oFNTreatmentProcess.FNTPID = oReader.GetInt32("FNTPID");
			oFNTreatmentProcess.Description = oReader.GetString("Description");
			oFNTreatmentProcess.FNTreatment = (EnumFNTreatment)  oReader.GetInt32("FNTreatment");
           
            oFNTreatmentProcess.FNProcess = oReader.GetString("FNProcess");
            oFNTreatmentProcess.Code = oReader.GetInt32("Code");
            oFNTreatmentProcess.FNTreatmentInt = oReader.GetInt32("FNTreatment");
            oFNTreatmentProcess.IsProduction = oReader.GetBoolean("IsProduction");
           
			return oFNTreatmentProcess;
		}

		private FNTreatmentProcess CreateObject(NullHandler oReader)
		{
			FNTreatmentProcess oFNTreatmentProcess = new FNTreatmentProcess();
			oFNTreatmentProcess = MapObject(oReader);
			return oFNTreatmentProcess;
		}

		private List<FNTreatmentProcess> CreateObjects(IDataReader oReader)
		{
			List<FNTreatmentProcess> oFNTreatmentProcess = new List<FNTreatmentProcess>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				FNTreatmentProcess oItem = CreateObject(oHandler);
				oFNTreatmentProcess.Add(oItem);
			}
			return oFNTreatmentProcess;
		}

		#endregion

		#region Interface implementation
			public FNTreatmentProcess Save(FNTreatmentProcess oFNTreatmentProcess, Int64 nUserID)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oFNTreatmentProcess.FNTPID <= 0)
					{
						reader = FNTreatmentProcessDA.InsertUpdate(tc, oFNTreatmentProcess, EnumDBOperation.Insert, nUserID);
					}
					else{	
						reader = FNTreatmentProcessDA.InsertUpdate(tc, oFNTreatmentProcess, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oFNTreatmentProcess = new FNTreatmentProcess();
						oFNTreatmentProcess = CreateObject(oReader);
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
						oFNTreatmentProcess = new FNTreatmentProcess();
						oFNTreatmentProcess.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oFNTreatmentProcess;
			}

			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					FNTreatmentProcess oFNTreatmentProcess = new FNTreatmentProcess();
					oFNTreatmentProcess.FNTPID = id;
					
					DBTableReferenceDA.HasReference(tc, "FNTreatmentProcess", id);
					FNTreatmentProcessDA.Delete(tc, oFNTreatmentProcess, EnumDBOperation.Delete, nUserId);
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

			public FNTreatmentProcess Get(int id, Int64 nUserId)
			{
				FNTreatmentProcess oFNTreatmentProcess = new FNTreatmentProcess();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = FNTreatmentProcessDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oFNTreatmentProcess = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get FNTreatmentProcess", e);
					#endregion
				}
				return oFNTreatmentProcess;
			}

			public List<FNTreatmentProcess> Gets(Int64 nUserID)
			{
				List<FNTreatmentProcess> oFNTreatmentProcesss = new List<FNTreatmentProcess>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = FNTreatmentProcessDA.Gets(tc);
					oFNTreatmentProcesss = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					FNTreatmentProcess oFNTreatmentProcess = new FNTreatmentProcess();
					oFNTreatmentProcess.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oFNTreatmentProcesss;
			}

			public List<FNTreatmentProcess> Gets (string sSQL, Int64 nUserID)
			{
				List<FNTreatmentProcess> oFNTreatmentProcesss = new List<FNTreatmentProcess>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = FNTreatmentProcessDA.Gets(tc, sSQL);
					oFNTreatmentProcesss = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get FNTreatmentProcess", e);
					#endregion
				}
				return oFNTreatmentProcesss;
			}

		#endregion
	}

}
