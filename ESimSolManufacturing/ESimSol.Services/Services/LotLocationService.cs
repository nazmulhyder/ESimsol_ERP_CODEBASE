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
	public class LotLocationService : MarshalByRefObject, ILotLocationService
	{
		#region Private functions and declaration

		private LotLocation MapObject(NullHandler oReader)
		{
			LotLocation oLotLocation = new LotLocation();
			oLotLocation.LotLocationID = oReader.GetInt32("LotLocationID");
			oLotLocation.LotID = oReader.GetInt32("LotID");
			oLotLocation.LotLocationName = oReader.GetString("LotLocationName");
			oLotLocation.Remarks = oReader.GetString("Remarks");
			return oLotLocation;
		}

		private LotLocation CreateObject(NullHandler oReader)
		{
			LotLocation oLotLocation = new LotLocation();
			oLotLocation = MapObject(oReader);
			return oLotLocation;
		}

		private List<LotLocation> CreateObjects(IDataReader oReader)
		{
			List<LotLocation> oLotLocation = new List<LotLocation>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				LotLocation oItem = CreateObject(oHandler);
				oLotLocation.Add(oItem);
			}
			return oLotLocation;
		}

		#endregion

		#region Interface implementation
			public LotLocation Save(LotLocation oLotLocation, Int64 nUserID)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oLotLocation.LotLocationID <= 0)
					{
						reader = LotLocationDA.InsertUpdate(tc, oLotLocation, EnumDBOperation.Insert, nUserID);
					}
					else{
						reader = LotLocationDA.InsertUpdate(tc, oLotLocation, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oLotLocation = new LotLocation();
						oLotLocation = CreateObject(oReader);
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
						oLotLocation = new LotLocation();
						oLotLocation.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oLotLocation;
			}

			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					LotLocation oLotLocation = new LotLocation();
					oLotLocation.LotLocationID = id;
					DBTableReferenceDA.HasReference(tc, "LotLocation", id);
					LotLocationDA.Delete(tc, oLotLocation, EnumDBOperation.Delete, nUserId);
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

			public LotLocation Get(int id, Int64 nUserId)
			{
				LotLocation oLotLocation = new LotLocation();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = LotLocationDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oLotLocation = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get LotLocation", e);
					#endregion
				}
				return oLotLocation;
			}

			public List<LotLocation> Gets(Int64 nUserID)
			{
				List<LotLocation> oLotLocations = new List<LotLocation>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = LotLocationDA.Gets(tc);
					oLotLocations = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					LotLocation oLotLocation = new LotLocation();
					oLotLocation.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oLotLocations;
			}

			public List<LotLocation> Gets (string sSQL, Int64 nUserID)
			{
				List<LotLocation> oLotLocations = new List<LotLocation>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = LotLocationDA.Gets(tc, sSQL);
					oLotLocations = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get LotLocation", e);
					#endregion
				}
				return oLotLocations;
			}

		#endregion
	}

}
