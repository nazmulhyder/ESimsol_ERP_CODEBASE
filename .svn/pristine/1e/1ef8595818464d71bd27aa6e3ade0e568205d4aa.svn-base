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
	public class FNMachineProcessService : MarshalByRefObject, IFNMachineProcessService
	{
		#region Private functions and declaration

		private FNMachineProcess MapObject(NullHandler oReader)
		{
			FNMachineProcess oFNMachineProcess = new FNMachineProcess();
			oFNMachineProcess.FNMProcessID = oReader.GetInt32("FNMProcessID");
			oFNMachineProcess.FNMachineID = oReader.GetInt32("FNMachineID");
			oFNMachineProcess.FNMachineNameCode = oReader.GetString("FNMachineNameCode");
			oFNMachineProcess.FNTPID = oReader.GetInt32("FNTPID");
			oFNMachineProcess.FNTreatment = (EnumFNTreatment) oReader.GetInt32("FNTreatment");
            oFNMachineProcess.FNProcess = oReader.GetString("FNProcess");
            oFNMachineProcess.FNTreatmentInt = oReader.GetInt32("FNTreatment");
			oFNMachineProcess.InActiveBy = oReader.GetInt32("InActiveBy");
			oFNMachineProcess.InactiveByName = oReader.GetString("InactiveByName");
            oFNMachineProcess.Description = oReader.GetString("Description");
			oFNMachineProcess.InActiveDate = oReader.GetDateTime("InActiveDate");
			return oFNMachineProcess;
		}

		private FNMachineProcess CreateObject(NullHandler oReader)
		{
			FNMachineProcess oFNMachineProcess = new FNMachineProcess();
			oFNMachineProcess = MapObject(oReader);
			return oFNMachineProcess;
		}

		private List<FNMachineProcess> CreateObjects(IDataReader oReader)
		{
			List<FNMachineProcess> oFNMachineProcess = new List<FNMachineProcess>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				FNMachineProcess oItem = CreateObject(oHandler);
				oFNMachineProcess.Add(oItem);
			}
			return oFNMachineProcess;
		}

		#endregion

		#region Interface implementation
			public FNMachine Save(FNMachine oFNMachine, Int64 nUserID)
			{
				TransactionContext tc = null;
                List<FNMachineProcess> oFNMachineProcessList = new List<FNMachineProcess>();
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
                    oFNMachineProcessList = oFNMachine.FNMachineProcessList;
                    foreach (FNMachineProcess oItem in oFNMachineProcessList)
                    {
                        oItem.FNMachineID = oFNMachine.FNMachineID;
                        if (oItem.FNMProcessID <= 0)
                        {
                            reader = FNMachineProcessDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            reader = FNMachineProcessDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                        }
                        NullHandler oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            
                        }
                        reader.Close();
                    }
                    reader = FNMachineProcessDA.Gets(oFNMachine.FNMachineID, tc);
                    oFNMachine.FNMachineProcessList = CreateObjects(reader);
                    reader.Close();
				    tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					{
						tc.HandleError();
                        oFNMachine = new FNMachine();
                        oFNMachine.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oFNMachine;
			}

            public FNMachineProcess ChangeActivety(FNMachineProcess oFNMachineProcess, Int64 nUserID)
            {
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin(true);
                    FNMachineProcessDA.ChangeActivety(tc, oFNMachineProcess, nUserID);
                    IDataReader reader = FNMachineProcessDA.Get(tc, oFNMachineProcess.FNMProcessID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFNMachineProcess = CreateObject(oReader);
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
                        oFNMachineProcess = new FNMachineProcess();
                        oFNMachineProcess.ErrorMessage = e.Message.Split('!')[0];
                    }
                    #endregion
                }
                return oFNMachineProcess;
            }

			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					FNMachineProcess oFNMachineProcess = new FNMachineProcess();
					oFNMachineProcess.FNMProcessID = id;
					DBTableReferenceDA.HasReference(tc, "FNMachineProcess", id);
					FNMachineProcessDA.Delete(tc, oFNMachineProcess, EnumDBOperation.Delete, nUserId);
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

			public FNMachineProcess Get(int id, Int64 nUserId)
			{
				FNMachineProcess oFNMachineProcess = new FNMachineProcess();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = FNMachineProcessDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oFNMachineProcess = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get FNMachineProcess", e);
					#endregion
				}
				return oFNMachineProcess;
			}

            public List<FNMachineProcess> Gets(int nFNMachineID, Int64 nUserID)
			{
				List<FNMachineProcess> oFNMachineProcesss = new List<FNMachineProcess>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
                    reader = FNMachineProcessDA.Gets(nFNMachineID, tc);
					oFNMachineProcesss = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					FNMachineProcess oFNMachineProcess = new FNMachineProcess();
					oFNMachineProcess.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oFNMachineProcesss;
			}

			public List<FNMachineProcess> Gets (string sSQL, Int64 nUserID)
			{
				List<FNMachineProcess> oFNMachineProcesss = new List<FNMachineProcess>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = FNMachineProcessDA.Gets(tc, sSQL);
					oFNMachineProcesss = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get FNMachineProcess", e);
					#endregion
				}
				return oFNMachineProcesss;
			}

		#endregion
	}

}
