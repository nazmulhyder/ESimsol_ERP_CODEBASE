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
	public class PackingListDetailService : MarshalByRefObject, IPackingListDetailService
	{
		#region Private functions and declaration

		private PackingListDetail MapObject(NullHandler oReader)
		{
			 PackingListDetail oPackingListDetail = new PackingListDetail();
			oPackingListDetail.PackingListDetailID = oReader.GetInt32("PackingListDetailID");
			oPackingListDetail.PackingListID = oReader.GetInt32("PackingListID");
			oPackingListDetail.ColorID = oReader.GetInt32("ColorID");
			oPackingListDetail.SizeID = oReader.GetInt32("SizeID");
			oPackingListDetail.Qty = oReader.GetDouble("Qty");
			oPackingListDetail.ColorName = oReader.GetString("ColorName");
			oPackingListDetail.SizeName = oReader.GetString("SizeName");
			return oPackingListDetail;
		}

		private PackingListDetail CreateObject(NullHandler oReader)
		{
			PackingListDetail oPackingListDetail = new PackingListDetail();
			oPackingListDetail = MapObject(oReader);
			return oPackingListDetail;
		}

		private List<PackingListDetail> CreateObjects(IDataReader oReader)
		{
			List<PackingListDetail> oPackingListDetail = new List<PackingListDetail>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				PackingListDetail oItem = CreateObject(oHandler);
				oPackingListDetail.Add(oItem);
			}
			return oPackingListDetail;
		}

		#endregion

		#region Interface implementation
		
			public PackingListDetail Get(int id, Int64 nUserId)
			{
				PackingListDetail oPackingListDetail = new PackingListDetail();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = PackingListDetailDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oPackingListDetail = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get PackingListDetail", e);
					#endregion
				}
				return oPackingListDetail;
			}

            public List<PackingListDetail> Gets(int nPackingID, Int64 nUserID)
			{
				List<PackingListDetail> oPackingListDetails = new List<PackingListDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
                    reader = PackingListDetailDA.Gets(tc, nPackingID);
					oPackingListDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					PackingListDetail oPackingListDetail = new PackingListDetail();
					oPackingListDetail.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oPackingListDetails;
			}

			public List<PackingListDetail> Gets (string sSQL, Int64 nUserID)
			{
				List<PackingListDetail> oPackingListDetails = new List<PackingListDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = PackingListDetailDA.Gets(tc, sSQL);
					oPackingListDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get PackingListDetail", e);
					#endregion
				}
				return oPackingListDetails;
			}

		#endregion
	}

}
