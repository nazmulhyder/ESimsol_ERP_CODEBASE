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
	public class PAMDetailService : MarshalByRefObject, IPAMDetailService
	{
		#region Private functions and declaration

		private PAMDetail MapObject(NullHandler oReader)
		{
			PAMDetail oPAMDetail = new PAMDetail();
			oPAMDetail.PAMDetailID = oReader.GetInt32("PAMDetailID");
			oPAMDetail.PAMID = oReader.GetInt32("PAMID");
			oPAMDetail.ColorID = oReader.GetInt32("ColorID");
			oPAMDetail.MinQuantity = oReader.GetDouble("MinQuantity");
			oPAMDetail.Quantity = oReader.GetDouble("Quantity");
            oPAMDetail.ColorWiseYetToRecapQty = oReader.GetDouble("ColorWiseYetToRecapQty");
			oPAMDetail.MaxQuantity = oReader.GetDouble("MaxQuantity");
            oPAMDetail.ConfirmWeek = oReader.GetString("ConfirmWeek");
            oPAMDetail.DesignationWeek = oReader.GetString("DesignationWeek");
            oPAMDetail.ForwardWeek = oReader.GetString("ForwardWeek");
            oPAMDetail.WearHouseWeek = oReader.GetString("WearHouseWeek");
            oPAMDetail.Remarks = oReader.GetString("Remarks");
            oPAMDetail.StyleNo = oReader.GetString("StyleNo");
			oPAMDetail.ColorName = oReader.GetString("ColorName");
			return oPAMDetail;
		}

		private PAMDetail CreateObject(NullHandler oReader)
		{
			PAMDetail oPAMDetail = new PAMDetail();
			oPAMDetail = MapObject(oReader);
			return oPAMDetail;
		}

		private List<PAMDetail> CreateObjects(IDataReader oReader)
		{
			List<PAMDetail> oPAMDetail = new List<PAMDetail>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				PAMDetail oItem = CreateObject(oHandler);
				oPAMDetail.Add(oItem);
			}
			return oPAMDetail;
		}

		#endregion

		#region Interface implementation

			public PAMDetail Get(int id, Int64 nUserId)
			{
				PAMDetail oPAMDetail = new PAMDetail();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = PAMDetailDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oPAMDetail = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get PAMDetail", e);
					#endregion
				}
				return oPAMDetail;
			}

			public List<PAMDetail> Gets(int nPAMID, Int64 nUserID)
			{
				List<PAMDetail> oPAMDetails = new List<PAMDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = PAMDetailDA.Gets(nPAMID, tc);
					oPAMDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					PAMDetail oPAMDetail = new PAMDetail();
					oPAMDetail.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oPAMDetails;
			}

			public List<PAMDetail> Gets (string sSQL, Int64 nUserID)
			{
				List<PAMDetail> oPAMDetails = new List<PAMDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = PAMDetailDA.Gets(tc, sSQL);
					oPAMDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get PAMDetail", e);
					#endregion
				}
				return oPAMDetails;
			}

		#endregion
	}

}
