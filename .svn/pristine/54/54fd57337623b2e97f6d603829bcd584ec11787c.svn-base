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
	public class UploadConfigureService : MarshalByRefObject, IUploadConfigureService
	{
		#region Private functions and declaration

		private UploadConfigure MapObject(NullHandler oReader)
		{
			UploadConfigure oUploadConfigure = new UploadConfigure();
			oUploadConfigure.UploadConfigureID = oReader.GetInt32("UploadConfigureID");
			oUploadConfigure.UserID = oReader.GetInt32("UserID");
			oUploadConfigure.FieldNames = oReader.GetString("FieldNames");
			oUploadConfigure.CaptionNames = oReader.GetString("CaptionNames");
			oUploadConfigure.UploadType = (EnumUploadType) oReader.GetInt32("UploadType");
            oUploadConfigure.UploadTypeInInt = oReader.GetInt32("UploadType");
			return oUploadConfigure;
		}

		private UploadConfigure CreateObject(NullHandler oReader)
		{
			UploadConfigure oUploadConfigure = new UploadConfigure();
			oUploadConfigure = MapObject(oReader);
			return oUploadConfigure;
		}

		private List<UploadConfigure> CreateObjects(IDataReader oReader)
		{
			List<UploadConfigure> oUploadConfigure = new List<UploadConfigure>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				UploadConfigure oItem = CreateObject(oHandler);
				oUploadConfigure.Add(oItem);
			}
			return oUploadConfigure;
		}

		#endregion

		#region Interface implementation
			public UploadConfigure Save(UploadConfigure oUploadConfigure, Int64 nUserID)
			{
				TransactionContext tc = null;

				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oUploadConfigure.UploadConfigureID <= 0)
					{
						
						reader = UploadConfigureDA.InsertUpdate(tc, oUploadConfigure, EnumDBOperation.Insert, nUserID);
					}
					else{
						reader = UploadConfigureDA.InsertUpdate(tc, oUploadConfigure, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oUploadConfigure = new UploadConfigure();
						oUploadConfigure = CreateObject(oReader);
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
						oUploadConfigure = new UploadConfigure();
						oUploadConfigure.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oUploadConfigure;
			}

			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					UploadConfigure oUploadConfigure = new UploadConfigure();
					oUploadConfigure.UploadConfigureID = id;
					DBTableReferenceDA.HasReference(tc, "UploadConfigure", id);
					UploadConfigureDA.Delete(tc, oUploadConfigure, EnumDBOperation.Delete, nUserId);
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

            public UploadConfigure Get(int nUploadType, Int64 nUserId)
			{
				UploadConfigure oUploadConfigure = new UploadConfigure();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
                    IDataReader reader = UploadConfigureDA.Get(tc, nUploadType, nUserId);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					    oUploadConfigure = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get UploadConfigure", e);
					#endregion
				}
				return oUploadConfigure;
			}

			public List<UploadConfigure> Gets(Int64 nUserID)
			{
				List<UploadConfigure> oUploadConfigures = new List<UploadConfigure>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = UploadConfigureDA.Gets(tc);
					oUploadConfigures = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					UploadConfigure oUploadConfigure = new UploadConfigure();
					oUploadConfigure.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oUploadConfigures;
			}

			public List<UploadConfigure> Gets (string sSQL, Int64 nUserID)
			{
				List<UploadConfigure> oUploadConfigures = new List<UploadConfigure>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = UploadConfigureDA.Gets(tc, sSQL);
					oUploadConfigures = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get UploadConfigure", e);
					#endregion
				}
				return oUploadConfigures;
			}

		#endregion
	}

}
