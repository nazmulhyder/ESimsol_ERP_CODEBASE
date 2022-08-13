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
	public class RSInQCSubStatusService : MarshalByRefObject, IRSInQCSubStatusService
	{
		#region Private functions and declaration

		private RSInQCSubStatus MapObject(NullHandler oReader)
		{
			RSInQCSubStatus oRSInQCSubStatus = new RSInQCSubStatus();
			oRSInQCSubStatus.RouteSheetID = oReader.GetInt32("RouteSheetID");
            oRSInQCSubStatus.RSSubStatus = (EnumRSSubStates)oReader.GetInt32("RSSubStatus");
            oRSInQCSubStatus.RSSubStatusInt = oReader.GetInt32("RSSubStatus");
            oRSInQCSubStatus.Note = oReader.GetString("Note");
            oRSInQCSubStatus.UpdateByName = oReader.GetString("UpdateByName");
            oRSInQCSubStatus.LastUpdateDateTime = oReader.GetDateTime("LastUpdateDateTime");
			return oRSInQCSubStatus;
		}

		private RSInQCSubStatus CreateObject(NullHandler oReader)
		{
			RSInQCSubStatus oRSInQCSubStatus = new RSInQCSubStatus();
			oRSInQCSubStatus = MapObject(oReader);
			return oRSInQCSubStatus;
		}

		private List<RSInQCSubStatus> CreateObjects(IDataReader oReader)
		{
			List<RSInQCSubStatus> oRSInQCSubStatus = new List<RSInQCSubStatus>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				RSInQCSubStatus oItem = CreateObject(oHandler);
				oRSInQCSubStatus.Add(oItem);
			}
			return oRSInQCSubStatus;
		}

		#endregion

		#region Interface implementation
			public RSInQCSubStatus Save(RSInQCSubStatus oRSInQCSubStatus, Int64 nUserID)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oRSInQCSubStatus.RouteSheetID <= 0)
					{
                        oRSInQCSubStatus.RouteSheetID = Convert.ToInt32(oRSInQCSubStatus.Param);
						reader = RSInQCSubStatusDA.InsertUpdate(tc, oRSInQCSubStatus, EnumDBOperation.Insert, nUserID);
					}
					else{
						reader = RSInQCSubStatusDA.InsertUpdate(tc, oRSInQCSubStatus, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oRSInQCSubStatus = new RSInQCSubStatus();
						oRSInQCSubStatus = CreateObject(oReader);
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
						oRSInQCSubStatus = new RSInQCSubStatus();
						oRSInQCSubStatus.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oRSInQCSubStatus;
			}

            public string Delete(RSInQCSubStatus oRSInQCSubStatus, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					RSInQCSubStatusDA.Delete(tc, oRSInQCSubStatus, EnumDBOperation.Delete, nUserId);
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

			public RSInQCSubStatus Get(int id, Int64 nUserId)
			{
				RSInQCSubStatus oRSInQCSubStatus = new RSInQCSubStatus();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = RSInQCSubStatusDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oRSInQCSubStatus = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get RSInQCSubStatus", e);
					#endregion
				}
				return oRSInQCSubStatus;
			}

			public List<RSInQCSubStatus> Gets(Int64 nUserID)
			{
				List<RSInQCSubStatus> oRSInQCSubStatuss = new List<RSInQCSubStatus>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = RSInQCSubStatusDA.Gets(tc);
					oRSInQCSubStatuss = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					RSInQCSubStatus oRSInQCSubStatus = new RSInQCSubStatus();
					oRSInQCSubStatus.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oRSInQCSubStatuss;
			}

			public List<RSInQCSubStatus> Gets (string sSQL, Int64 nUserID)
			{
				List<RSInQCSubStatus> oRSInQCSubStatuss = new List<RSInQCSubStatus>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = RSInQCSubStatusDA.Gets(tc, sSQL);
					oRSInQCSubStatuss = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get RSInQCSubStatus", e);
					#endregion
				}
				return oRSInQCSubStatuss;
			}

		#endregion
	}

}
