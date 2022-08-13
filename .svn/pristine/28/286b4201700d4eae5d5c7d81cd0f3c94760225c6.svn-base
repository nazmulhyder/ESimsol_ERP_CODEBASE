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
	public class FNMachineService : MarshalByRefObject, IFNMachineService
	{
		#region Private functions and declaration

		private FNMachine MapObject(NullHandler oReader)
		{
			FNMachine oFNMachine = new FNMachine();
			oFNMachine.FNMachineID = oReader.GetInt32("FNMachineID");
			oFNMachine.FNMachineType =  (EnumFNMachineType) oReader.GetInt32("FNMachineType");
            oFNMachine.FNMachineTypeInt = oReader.GetInt32("FNMachineType");
			oFNMachine.Code = oReader.GetString("Code");
			oFNMachine.Name = oReader.GetString("Name");
			oFNMachine.Note = oReader.GetString("Note");
            oFNMachine.FreeTime = oReader.GetString("FreeTime");
			oFNMachine.NoOfBath = oReader.GetInt32("NoOfBath");
			oFNMachine.IsAtive = oReader.GetBoolean("IsAtive");
			return oFNMachine;
		}

		private FNMachine CreateObject(NullHandler oReader)
		{
			FNMachine oFNMachine = new FNMachine();
			oFNMachine = MapObject(oReader);
			return oFNMachine;
		}

		private List<FNMachine> CreateObjects(IDataReader oReader)
		{
			List<FNMachine> oFNMachine = new List<FNMachine>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				FNMachine oItem = CreateObject(oHandler);
				oFNMachine.Add(oItem);
			}
			return oFNMachine;
		}

		#endregion

		#region Interface implementation
			public FNMachine Save(FNMachine oFNMachine, Int64 nUserID)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oFNMachine.FNMachineID <= 0)
					{
						reader = FNMachineDA.InsertUpdate(tc, oFNMachine, EnumDBOperation.Insert, nUserID);
					}
					else{
						reader = FNMachineDA.InsertUpdate(tc, oFNMachine, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oFNMachine = new FNMachine();
						oFNMachine = CreateObject(oReader);
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
						oFNMachine = new FNMachine();
						oFNMachine.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oFNMachine;
			}

            public FNMachine ChangeActivety(FNMachine oFNMachine, Int64 nUserID)
            {
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin(true);
                    FNMachineDA.ChangeActivety(tc, oFNMachine);
                    IDataReader reader = FNMachineDA.Get(tc, oFNMachine.FNMachineID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFNMachine = CreateObject(oReader);
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
                        oFNMachine = new FNMachine();
                        oFNMachine.ErrorMessage = e.Message.Split('!')[0];
                    }
                    #endregion
                }
                return oFNMachine;
            }
			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					FNMachine oFNMachine = new FNMachine();
					oFNMachine.FNMachineID = id;
					DBTableReferenceDA.HasReference(tc, "FNMachine", id);
					FNMachineDA.Delete(tc, oFNMachine, EnumDBOperation.Delete, nUserId);
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

			public FNMachine Get(int id, Int64 nUserId)
			{
				FNMachine oFNMachine = new FNMachine();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = FNMachineDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oFNMachine = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get FNMachine", e);
					#endregion
				}
				return oFNMachine;
			}

			public List<FNMachine> Gets(Int64 nUserID)
			{
				List<FNMachine> oFNMachines = new List<FNMachine>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = FNMachineDA.Gets(tc);
					oFNMachines = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					FNMachine oFNMachine = new FNMachine();
					oFNMachine.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oFNMachines;
			}

			public List<FNMachine> Gets (string sSQL, Int64 nUserID)
			{
				List<FNMachine> oFNMachines = new List<FNMachine>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = FNMachineDA.Gets(tc, sSQL);
					oFNMachines = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get FNMachine", e);
					#endregion
				}
				return oFNMachines;
			}

		#endregion
	}

}
