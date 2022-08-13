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
	public class FADepreciationDetailService : MarshalByRefObject, IFADepreciationDetailService
	{
		#region Private functions and declaration

		private FADepreciationDetail MapObject(NullHandler oReader)
		{
			FADepreciationDetail oFADepreciationDetail = new FADepreciationDetail();
			oFADepreciationDetail.FADepreciationDetailID = oReader.GetInt32("FADepreciationDetailID");
			oFADepreciationDetail.FADepreciationID = oReader.GetInt32("FADepreciationID");
			oFADepreciationDetail.FAScheduleID = oReader.GetInt32("FAScheduleID");
			oFADepreciationDetail.FARegisterID = oReader.GetInt32("FARegisterID");
			oFADepreciationDetail.DepreciationAmount = oReader.GetDouble("DepreciationAmount");
			oFADepreciationDetail.StartDate = oReader.GetDateTime("StartDate");
			oFADepreciationDetail.EndDate = oReader.GetDateTime("EndDate");
			oFADepreciationDetail.DepreciationRate = oReader.GetDouble("DepreciationRate");
			oFADepreciationDetail.FAMethod = (EnumFAMethod)  oReader.GetInt32("FAMethod");
			oFADepreciationDetail.FACodeFull = oReader.GetString("FACodeFull");
			oFADepreciationDetail.ProductCategoryName = oReader.GetString("ProductCategoryName");
			oFADepreciationDetail.ProductCode = oReader.GetString("ProductCode");
			oFADepreciationDetail.ProductName = oReader.GetString("ProductName");
			oFADepreciationDetail.CurrencySymbol = oReader.GetString("CurrencySymbol");
			return oFADepreciationDetail;
		}

		private FADepreciationDetail CreateObject(NullHandler oReader)
		{
			FADepreciationDetail oFADepreciationDetail = new FADepreciationDetail();
			oFADepreciationDetail = MapObject(oReader);
			return oFADepreciationDetail;
		}

		private List<FADepreciationDetail> CreateObjects(IDataReader oReader)
		{
			List<FADepreciationDetail> oFADepreciationDetail = new List<FADepreciationDetail>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				FADepreciationDetail oItem = CreateObject(oHandler);
				oFADepreciationDetail.Add(oItem);
			}
			return oFADepreciationDetail;
		}

		#endregion

		#region Interface implementation

			public FADepreciationDetail Get(int id, Int64 nUserId)
			{
				FADepreciationDetail oFADepreciationDetail = new FADepreciationDetail();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = FADepreciationDetailDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oFADepreciationDetail = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get FADepreciationDetail", e);
					#endregion
				}
				return oFADepreciationDetail;
			}

			public List<FADepreciationDetail> Gets(int id, Int64 nUserID)
			{
				List<FADepreciationDetail> oFADepreciationDetails = new List<FADepreciationDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = FADepreciationDetailDA.Gets(id, tc);
					oFADepreciationDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					FADepreciationDetail oFADepreciationDetail = new FADepreciationDetail();
					oFADepreciationDetail.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oFADepreciationDetails;
			}

			public List<FADepreciationDetail> Gets (string sSQL, Int64 nUserID)
			{
				List<FADepreciationDetail> oFADepreciationDetails = new List<FADepreciationDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = FADepreciationDetailDA.Gets(tc, sSQL);
					oFADepreciationDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get FADepreciationDetail", e);
					#endregion
				}
				return oFADepreciationDetails;
			}

		#endregion
	}

}
