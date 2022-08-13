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
	public class KnitDyeingPTULogService : MarshalByRefObject, IKnitDyeingPTULogService
	{
		#region Private functions and declaration

		private KnitDyeingPTULog MapObject(NullHandler oReader)
		{
			KnitDyeingPTULog oKnitDyeingPTULog = new KnitDyeingPTULog();
            oKnitDyeingPTULog.KnitDyeingPTULogID = oReader.GetInt32("KnitDyeingPTULogID");
            oKnitDyeingPTULog.KnitDyeingPTUID = oReader.GetInt32("KnitDyeingPTUID");
            oKnitDyeingPTULog.KnitDyeingPTURefType = (EnumKnitDyeingPTURefType)oReader.GetInt32("KnitDyeingPTURefType");
            oKnitDyeingPTULog.KnitDyeingPTURefTypeInt =oReader.GetInt32("KnitDyeingPTURefType");
            
            oKnitDyeingPTULog.KnitDyeingPTURefObjID = oReader.GetInt32("KnitDyeingPTURefObjID");
            oKnitDyeingPTULog.Qty = oReader.GetDouble("Qty");
            oKnitDyeingPTULog.Remarks = oReader.GetString("Remarks");
            oKnitDyeingPTULog.DBUserID = oReader.GetInt32("DBUserID");
            oKnitDyeingPTULog.DBServerDateTime = oReader.GetDateTime("DBServerDateTime");
            oKnitDyeingPTULog.UserName = oReader.GetString("UserName");
            oKnitDyeingPTULog.RefNo = oReader.GetString("RefNo");
            
			return oKnitDyeingPTULog;
		}

		private KnitDyeingPTULog CreateObject(NullHandler oReader)
		{
			KnitDyeingPTULog oKnitDyeingPTULog = new KnitDyeingPTULog();
			oKnitDyeingPTULog = MapObject(oReader);
			return oKnitDyeingPTULog;
		}

		private List<KnitDyeingPTULog> CreateObjects(IDataReader oReader)
		{
			List<KnitDyeingPTULog> oKnitDyeingPTULog = new List<KnitDyeingPTULog>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				KnitDyeingPTULog oItem = CreateObject(oHandler);
				oKnitDyeingPTULog.Add(oItem);
			}
			return oKnitDyeingPTULog;
		}

		#endregion

		#region Interface implementation
			public KnitDyeingPTULog Save(KnitDyeingPTULog oKnitDyeingPTULog, Int64 nUserID)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oKnitDyeingPTULog.KnitDyeingPTULogID <= 0)
					{
						reader = KnitDyeingPTULogDA.InsertUpdate(tc, oKnitDyeingPTULog, EnumDBOperation.Insert, nUserID);
					}
					else{
						reader = KnitDyeingPTULogDA.InsertUpdate(tc, oKnitDyeingPTULog, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oKnitDyeingPTULog = new KnitDyeingPTULog();
						oKnitDyeingPTULog = CreateObject(oReader);
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
						oKnitDyeingPTULog = new KnitDyeingPTULog();
						oKnitDyeingPTULog.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oKnitDyeingPTULog;
			}

			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					KnitDyeingPTULog oKnitDyeingPTULog = new KnitDyeingPTULog();
					oKnitDyeingPTULog.KnitDyeingPTULogID = id;
					DBTableReferenceDA.HasReference(tc, "KnitDyeingPTULog", id);
					KnitDyeingPTULogDA.Delete(tc, oKnitDyeingPTULog, EnumDBOperation.Delete, nUserId);
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
            public List<KnitDyeingPTULog> Gets(int nID, int nEType, Int64 nUserID)
            {
                List<KnitDyeingPTULog> oKnitDyeingPTULogs = new List<KnitDyeingPTULog>();
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin();
                    IDataReader reader = null;
                    reader = KnitDyeingPTULogDA.Gets(tc, nID, nEType);
                    oKnitDyeingPTULogs = CreateObjects(reader);
                    reader.Close();
                    tc.End();
                }
                catch (Exception e)
                {
                    #region Handle Exception
                    if (tc != null)
                        tc.HandleError();
                    KnitDyeingPTULog oKnitDyeingPTULog = new KnitDyeingPTULog();
                    oKnitDyeingPTULog.ErrorMessage = e.Message.Split('!')[0];
                    #endregion
                }
                return oKnitDyeingPTULogs;
            }
			public KnitDyeingPTULog Get(int id, Int64 nUserId)
			{
				KnitDyeingPTULog oKnitDyeingPTULog = new KnitDyeingPTULog();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = KnitDyeingPTULogDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oKnitDyeingPTULog = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get KnitDyeingPTULog", e);
					#endregion
				}
				return oKnitDyeingPTULog;
			}

			public List<KnitDyeingPTULog> Gets(Int64 nUserID)
			{
				List<KnitDyeingPTULog> oKnitDyeingPTULogs = new List<KnitDyeingPTULog>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = KnitDyeingPTULogDA.Gets(tc);
					oKnitDyeingPTULogs = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					KnitDyeingPTULog oKnitDyeingPTULog = new KnitDyeingPTULog();
					oKnitDyeingPTULog.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oKnitDyeingPTULogs;
			}

			public List<KnitDyeingPTULog> Gets (string sSQL, Int64 nUserID)
			{
				List<KnitDyeingPTULog> oKnitDyeingPTULogs = new List<KnitDyeingPTULog>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = KnitDyeingPTULogDA.Gets(tc, sSQL);
					oKnitDyeingPTULogs = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get KnitDyeingPTULog", e);
					#endregion
				}
				return oKnitDyeingPTULogs;
			}

		#endregion
	}

}
