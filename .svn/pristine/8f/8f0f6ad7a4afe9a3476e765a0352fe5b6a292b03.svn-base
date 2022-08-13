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
	public class BTMADetailService : MarshalByRefObject, IBTMADetailService
	{
		#region Private functions and declaration

		private BTMADetail MapObject(NullHandler oReader)
		{
			BTMADetail oBTMADetail = new BTMADetail();
			oBTMADetail.BTMADetailID = oReader.GetInt32("BTMADetailID");
			oBTMADetail.BTMAID = oReader.GetInt32("BTMAID");
			oBTMADetail.ProductID = oReader.GetInt32("ProductID");
			oBTMADetail.ProductName = oReader.GetString("ProductName");
			oBTMADetail.Qty = oReader.GetDouble("Qty");
            oBTMADetail.PINo = oReader.GetString("PINo");
			oBTMADetail.MUnitID = oReader.GetInt32("MUnitID");
            oBTMADetail.MUName = oReader.GetString("MUName");
            oBTMADetail.UnitPrice = oReader.GetDouble("UnitPrice");
			return oBTMADetail;
		}

		private BTMADetail CreateObject(NullHandler oReader)
		{
			BTMADetail oBTMADetail = new BTMADetail();
			oBTMADetail = MapObject(oReader);
			return oBTMADetail;
		}

		private List<BTMADetail> CreateObjects(IDataReader oReader)
		{
			List<BTMADetail> oBTMADetail = new List<BTMADetail>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				BTMADetail oItem = CreateObject(oHandler);
				oBTMADetail.Add(oItem);
			}
			return oBTMADetail;
		}

		#endregion

		#region Interface implementation
			public BTMADetail Save(BTMADetail oBTMADetail, Int64 nUserID)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oBTMADetail.BTMADetailID <= 0)
					{
						reader = BTMADetailDA.InsertUpdate(tc, oBTMADetail, EnumDBOperation.Insert, nUserID,"");
					}
					else{
						reader = BTMADetailDA.InsertUpdate(tc, oBTMADetail, EnumDBOperation.Update, nUserID,"");
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oBTMADetail = new BTMADetail();
						oBTMADetail = CreateObject(oReader);
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
						oBTMADetail = new BTMADetail();
						oBTMADetail.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oBTMADetail;
			}

			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					BTMADetail oBTMADetail = new BTMADetail();
					oBTMADetail.BTMADetailID = id;
					DBTableReferenceDA.HasReference(tc, "BTMADetail", id);
					BTMADetailDA.Delete(tc, oBTMADetail, EnumDBOperation.Delete, nUserId,"");
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

			public BTMADetail Get(int id, Int64 nUserId)
			{
				BTMADetail oBTMADetail = new BTMADetail();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = BTMADetailDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oBTMADetail = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get BTMADetail", e);
					#endregion
				}
				return oBTMADetail;
			}

			public List<BTMADetail> Gets(Int64 nUserID)
			{
				List<BTMADetail> oBTMADetails = new List<BTMADetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = BTMADetailDA.Gets(tc);
					oBTMADetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					BTMADetail oBTMADetail = new BTMADetail();
					oBTMADetail.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oBTMADetails;
			}
            public List<BTMADetail> Gets(int id, Int64 nUserID)
            {
                List<BTMADetail> oBTMADetails = new List<BTMADetail>();
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin();
                    IDataReader reader = null;
                    reader = BTMADetailDA.Gets(id, tc);
                    oBTMADetails = CreateObjects(reader);
                    reader.Close();
                    tc.End();
                }
                catch (Exception e)
                {
                    #region Handle Exception
                    if (tc != null)
                        tc.HandleError();
                    BTMADetail oBTMADetail = new BTMADetail();
                    oBTMADetail.ErrorMessage = e.Message.Split('!')[0];
                    #endregion
                }
                return oBTMADetails;
            }
			public List<BTMADetail> Gets (string sSQL, Int64 nUserID)
			{
				List<BTMADetail> oBTMADetails = new List<BTMADetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = BTMADetailDA.Gets(tc, sSQL);
					oBTMADetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get BTMADetail", e);
					#endregion
				}
				return oBTMADetails;
			}

		#endregion
	}

}
