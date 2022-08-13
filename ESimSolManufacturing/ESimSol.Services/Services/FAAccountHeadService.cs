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
	public class FAAccountHeadService : MarshalByRefObject, IFAAccountHeadService
	{
		#region Private functions and declaration

		private FAAccountHead MapObject(NullHandler oReader)
		{
			FAAccountHead oFAAccountHead = new FAAccountHead();
			oFAAccountHead.FAAccountHeadID = oReader.GetInt32("FAAccountHeadID");
            oFAAccountHead.FAAccountHeadName = oReader.GetString("FAAccountHeadName");
            oFAAccountHead.FAAccountHeadType = (EnumFAAccountHeadType)oReader.GetInt32("FAAccountHeadType");
            oFAAccountHead.FAAccountHeadTypeInt = oReader.GetInt32("FAAccountHeadType");
            oFAAccountHead.ChartsOfAccountID = oReader.GetInt32("ChartsOfAccountID");
            oFAAccountHead.CAHeadName = oReader.GetString("CAHeadName");
			return oFAAccountHead;
		}

		private FAAccountHead CreateObject(NullHandler oReader)
		{
			FAAccountHead oFAAccountHead = new FAAccountHead();
			oFAAccountHead = MapObject(oReader);
			return oFAAccountHead;
		}

		private List<FAAccountHead> CreateObjects(IDataReader oReader)
		{
			List<FAAccountHead> oFAAccountHead = new List<FAAccountHead>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				FAAccountHead oItem = CreateObject(oHandler);
				oFAAccountHead.Add(oItem);
			}
			return oFAAccountHead;
		}

		#endregion

		#region Interface implementation
			public FAAccountHead Save(FAAccountHead oFAAccountHead, Int64 nUserID)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oFAAccountHead.FAAccountHeadID <= 0)
					{
						reader = FAAccountHeadDA.InsertUpdate(tc, oFAAccountHead, EnumDBOperation.Insert, nUserID);
					}
					else{
						reader = FAAccountHeadDA.InsertUpdate(tc, oFAAccountHead, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oFAAccountHead = new FAAccountHead();
						oFAAccountHead = CreateObject(oReader);
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
						oFAAccountHead = new FAAccountHead();
						oFAAccountHead.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oFAAccountHead;
			}

			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					FAAccountHead oFAAccountHead = new FAAccountHead();
					oFAAccountHead.FAAccountHeadID = id;
					DBTableReferenceDA.HasReference(tc, "FAAccountHead", id);
					FAAccountHeadDA.Delete(tc, oFAAccountHead, EnumDBOperation.Delete, nUserId);
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

			public FAAccountHead Get(int id, Int64 nUserId)
			{
				FAAccountHead oFAAccountHead = new FAAccountHead();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = FAAccountHeadDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oFAAccountHead = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get FAAccountHead", e);
					#endregion
				}
				return oFAAccountHead;
			}

			public List<FAAccountHead> Gets(Int64 nUserID)
			{
				List<FAAccountHead> oFAAccountHeads = new List<FAAccountHead>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = FAAccountHeadDA.Gets(tc);
					oFAAccountHeads = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					FAAccountHead oFAAccountHead = new FAAccountHead();
					oFAAccountHead.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oFAAccountHeads;
			}

			public List<FAAccountHead> Gets (string sSQL, Int64 nUserID)
			{
				List<FAAccountHead> oFAAccountHeads = new List<FAAccountHead>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = FAAccountHeadDA.Gets(tc, sSQL);
					oFAAccountHeads = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get FAAccountHead", e);
					#endregion
				}
				return oFAAccountHeads;
			}

		#endregion
	}

}
