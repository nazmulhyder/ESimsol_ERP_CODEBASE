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
	public class FPDataService : MarshalByRefObject, IFPDataService
	{
		#region Private functions and declaration

		private FPData MapObject(NullHandler oReader)
		{
			FPData oFPData = new FPData();
			oFPData.FPDataID = oReader.GetInt32("FPDataID");
			oFPData.OperationalCost = oReader.GetDouble("OperationalCost");
			oFPData.BTBCost = oReader.GetDouble("BTBCost");
			oFPData.ExportHMonth = oReader.GetString("ExportHMonth");
			oFPData.ExportHQty = oReader.GetDouble("ExportHQty");
			oFPData.EHValue =  oReader.GetDouble("EHValue");
			oFPData.ExportQty = oReader.GetDouble("ExportQty");
			oFPData.ExportValue = oReader.GetDouble("ExportValue");
            oFPData.FPDate = oReader.GetDateTime("FPDate");
            
			return oFPData;
		}

		private FPData CreateObject(NullHandler oReader)
		{
			FPData oFPData = new FPData();
			oFPData = MapObject(oReader);
			return oFPData;
		}

		private List<FPData> CreateObjects(IDataReader oReader)
		{
			List<FPData> oFPData = new List<FPData>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				FPData oItem = CreateObject(oHandler);
				oFPData.Add(oItem);
			}
			return oFPData;
		}

		#endregion

		#region Interface implementation
			public FPData Save(FPData oFPData, Int64 nUserID)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oFPData.FPDataID <= 0)
					{
                        //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "FPData", EnumRoleOperationType.Add);
						reader = FPDataDA.InsertUpdate(tc, oFPData, EnumDBOperation.Insert, nUserID);
					}
					else{
                        //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "FPData", EnumRoleOperationType.Edit);
						reader = FPDataDA.InsertUpdate(tc, oFPData, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oFPData = new FPData();
						oFPData = CreateObject(oReader);
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
						oFPData = new FPData();
						oFPData.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oFPData;
			}



			public FPData Get(int id, Int64 nUserId)
			{
				FPData oFPData = new FPData();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = FPDataDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oFPData = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get FPData", e);
					#endregion
				}
				return oFPData;
			}

	

			public List<FPData> Gets (string sSQL, Int64 nUserID)
			{
				List<FPData> oFPDatas = new List<FPData>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = FPDataDA.Gets(tc, sSQL);
					oFPDatas = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get FPData", e);
					#endregion
				}
				return oFPDatas;
			}

		#endregion
	}

}
