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
	public class RSDetailAdditonalService : MarshalByRefObject, IRSDetailAdditonalService
	{
		#region Private functions and declaration

		private RSDetailAdditonal MapObject(NullHandler oReader)
		{
			RSDetailAdditonal oRSDetailAdditonal = new RSDetailAdditonal();
			oRSDetailAdditonal.RSDetailAdditonalID = oReader.GetInt32("RSDetailAdditonalID");
			oRSDetailAdditonal.RouteSheetDetailID = oReader.GetInt32("RouteSheetDetailID");
            oRSDetailAdditonal.RouteSheetID = oReader.GetInt32("RouteSheetID");
			oRSDetailAdditonal.SequenceNo = oReader.GetInt32("SequenceNo");
			oRSDetailAdditonal.InOutType = oReader.GetInt32("InOutType");
			oRSDetailAdditonal.Qty = oReader.GetDouble("Qty");
			oRSDetailAdditonal.LotID = oReader.GetInt32("LotID");
            oRSDetailAdditonal.Note = oReader.GetString("Note");
            oRSDetailAdditonal.LotNo = oReader.GetString("LotNo");
			oRSDetailAdditonal.IssuedByID = oReader.GetInt32("IssuedByID");
            oRSDetailAdditonal.ApprovedByID = oReader.GetInt32("ApprovedByID");
            oRSDetailAdditonal.IssueDate = oReader.GetDateTime("IssueDate");
            oRSDetailAdditonal.IssuedByName = oReader.GetString("IssuedByName");
            oRSDetailAdditonal.ApprovedByName = oReader.GetString("ApprovedByName");
            oRSDetailAdditonal.WUName = oReader.GetString("WUName");
            
            oRSDetailAdditonal.ProductType = (EnumProductNature)oReader.GetInt32("ProductType");
			return oRSDetailAdditonal;
		}

		private RSDetailAdditonal CreateObject(NullHandler oReader)
		{
			RSDetailAdditonal oRSDetailAdditonal = new RSDetailAdditonal();
			oRSDetailAdditonal = MapObject(oReader);
			return oRSDetailAdditonal;
		}

		private List<RSDetailAdditonal> CreateObjects(IDataReader oReader)
		{
			List<RSDetailAdditonal> oRSDetailAdditonal = new List<RSDetailAdditonal>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				RSDetailAdditonal oItem = CreateObject(oHandler);
				oRSDetailAdditonal.Add(oItem);
			}
			return oRSDetailAdditonal;
		}

		#endregion

		#region Interface implementation
			public RSDetailAdditonal Save(RSDetailAdditonal oRSDetailAdditonal, Int64 nUserID)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oRSDetailAdditonal.RSDetailAdditonalID <= 0)
					{
						reader = RSDetailAdditonalDA.InsertUpdate(tc, oRSDetailAdditonal, EnumDBOperation.Insert, nUserID);
					}
					else
                    {
						reader = RSDetailAdditonalDA.InsertUpdate(tc, oRSDetailAdditonal, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oRSDetailAdditonal = new RSDetailAdditonal();
						oRSDetailAdditonal = CreateObject(oReader);
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
						oRSDetailAdditonal = new RSDetailAdditonal();
						oRSDetailAdditonal.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oRSDetailAdditonal;
			}

			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				RSDetailAdditonal oRSDetailAdditonal = new RSDetailAdditonal();
				try
				{
					tc = TransactionContext.Begin(true);
					oRSDetailAdditonal.RSDetailAdditonalID = id;
					DBTableReferenceDA.HasReference(tc, "RSDetailAdditonal", id);
					RSDetailAdditonalDA.Delete(tc, oRSDetailAdditonal, EnumDBOperation.Delete, nUserId);

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

			public RSDetailAdditonal Get(int id, Int64 nUserId)
			{
				RSDetailAdditonal oRSDetailAdditonal = new RSDetailAdditonal();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = RSDetailAdditonalDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oRSDetailAdditonal = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get RSDetailAdditonal", e);
					#endregion
				}
				return oRSDetailAdditonal;
			}

			public List<RSDetailAdditonal> Gets(Int64 nUserID)
			{
				List<RSDetailAdditonal> oRSDetailAdditonals = new List<RSDetailAdditonal>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = RSDetailAdditonalDA.Gets(tc);
					oRSDetailAdditonals = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					RSDetailAdditonal oRSDetailAdditonal = new RSDetailAdditonal();
					oRSDetailAdditonal.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oRSDetailAdditonals;
			}

			public List<RSDetailAdditonal> Gets (string sSQL, Int64 nUserID)
			{
				List<RSDetailAdditonal> oRSDetailAdditonals = new List<RSDetailAdditonal>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = RSDetailAdditonalDA.Gets(tc, sSQL);
					oRSDetailAdditonals = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get RSDetailAdditonal", e);
					#endregion
				}
				return oRSDetailAdditonals;
			}

		#endregion
	}

}
