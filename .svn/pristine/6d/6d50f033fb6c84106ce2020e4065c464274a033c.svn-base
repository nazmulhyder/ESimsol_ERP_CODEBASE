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
	public class ReportConfigureService : MarshalByRefObject, IReportConfigureService
	{
		#region Private functions and declaration

		private ReportConfigure MapObject(NullHandler oReader)
		{
			ReportConfigure oReportConfigure = new ReportConfigure();
			oReportConfigure.ReportConfigureID = oReader.GetInt32("ReportConfigureID");
			oReportConfigure.UserID = oReader.GetInt32("UserID");
			oReportConfigure.FieldNames = oReader.GetString("FieldNames");
			oReportConfigure.CaptionNames = oReader.GetString("CaptionNames");
			oReportConfigure.ReportType = (EnumReportType) oReader.GetInt32("ReportType");
            oReportConfigure.ReportTypeInInt = oReader.GetInt32("ReportType");
			return oReportConfigure;
		}

		private ReportConfigure CreateObject(NullHandler oReader)
		{
			ReportConfigure oReportConfigure = new ReportConfigure();
			oReportConfigure = MapObject(oReader);
			return oReportConfigure;
		}

		private List<ReportConfigure> CreateObjects(IDataReader oReader)
		{
			List<ReportConfigure> oReportConfigure = new List<ReportConfigure>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				ReportConfigure oItem = CreateObject(oHandler);
				oReportConfigure.Add(oItem);
			}
			return oReportConfigure;
		}

		#endregion

		#region Interface implementation
			public ReportConfigure Save(ReportConfigure oReportConfigure, Int64 nUserID)
			{
				TransactionContext tc = null;

				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oReportConfigure.ReportConfigureID <= 0)
					{
						
						reader = ReportConfigureDA.InsertUpdate(tc, oReportConfigure, EnumDBOperation.Insert, nUserID);
					}
					else{
						reader = ReportConfigureDA.InsertUpdate(tc, oReportConfigure, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oReportConfigure = new ReportConfigure();
						oReportConfigure = CreateObject(oReader);
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
						oReportConfigure = new ReportConfigure();
						oReportConfigure.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oReportConfigure;
			}

			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					ReportConfigure oReportConfigure = new ReportConfigure();
					oReportConfigure.ReportConfigureID = id;
					DBTableReferenceDA.HasReference(tc, "ReportConfigure", id);
					ReportConfigureDA.Delete(tc, oReportConfigure, EnumDBOperation.Delete, nUserId);
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

            public ReportConfigure Get(int nReportType, Int64 nUserId)
			{
				ReportConfigure oReportConfigure = new ReportConfigure();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
                    IDataReader reader = ReportConfigureDA.Get(tc, nReportType, nUserId);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oReportConfigure = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get ReportConfigure", e);
					#endregion
				}
				return oReportConfigure;
			}

			public List<ReportConfigure> Gets(Int64 nUserID)
			{
				List<ReportConfigure> oReportConfigures = new List<ReportConfigure>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = ReportConfigureDA.Gets(tc);
					oReportConfigures = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					ReportConfigure oReportConfigure = new ReportConfigure();
					oReportConfigure.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oReportConfigures;
			}

			public List<ReportConfigure> Gets (string sSQL, Int64 nUserID)
			{
				List<ReportConfigure> oReportConfigures = new List<ReportConfigure>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = ReportConfigureDA.Gets(tc, sSQL);
					oReportConfigures = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get ReportConfigure", e);
					#endregion
				}
				return oReportConfigures;
			}

		#endregion
	}

}
