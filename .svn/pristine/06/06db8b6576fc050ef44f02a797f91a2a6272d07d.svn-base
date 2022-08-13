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
	public class DocPrintEngineDetailService : MarshalByRefObject, IDocPrintEngineDetailService
	{
		#region Private functions and declaration

		private DocPrintEngineDetail MapObject(NullHandler oReader)
		{
			DocPrintEngineDetail oDocPrintEngineDetail = new DocPrintEngineDetail();
			oDocPrintEngineDetail.DocPrintEngineDetailID = oReader.GetInt32("DocPrintEngineDetailID");
			oDocPrintEngineDetail.DocPrintEngineID = oReader.GetInt32("DocPrintEngineID");
			oDocPrintEngineDetail.SLNo = oReader.GetString("SLNo");
			oDocPrintEngineDetail.SetWidths = oReader.GetString("SetWidths");
			oDocPrintEngineDetail.SetAligns = oReader.GetString("SetAligns");
			oDocPrintEngineDetail.SetFields = oReader.GetString("SetFields");
			oDocPrintEngineDetail.FontSize = oReader.GetString("FontSize");
			oDocPrintEngineDetail.RowHeight = oReader.GetDouble("RowHeight");
			oDocPrintEngineDetail.TableName = oReader.GetString("TableName");
			return oDocPrintEngineDetail;
		}

		private DocPrintEngineDetail CreateObject(NullHandler oReader)
		{
			DocPrintEngineDetail oDocPrintEngineDetail = new DocPrintEngineDetail();
			oDocPrintEngineDetail = MapObject(oReader);
			return oDocPrintEngineDetail;
		}

		private List<DocPrintEngineDetail> CreateObjects(IDataReader oReader)
		{
			List<DocPrintEngineDetail> oDocPrintEngineDetail = new List<DocPrintEngineDetail>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				DocPrintEngineDetail oItem = CreateObject(oHandler);
				oDocPrintEngineDetail.Add(oItem);
			}
			return oDocPrintEngineDetail;
		}

		#endregion

		#region Interface implementation
			public DocPrintEngineDetail Save(DocPrintEngineDetail oDocPrintEngineDetail, Int64 nUserID)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oDocPrintEngineDetail.DocPrintEngineDetailID <= 0)
					{
						reader = DocPrintEngineDetailDA.InsertUpdate(tc, oDocPrintEngineDetail, EnumDBOperation.Insert, nUserID);
					}
					else{
						reader = DocPrintEngineDetailDA.InsertUpdate(tc, oDocPrintEngineDetail, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oDocPrintEngineDetail = new DocPrintEngineDetail();
						oDocPrintEngineDetail = CreateObject(oReader);
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
						oDocPrintEngineDetail = new DocPrintEngineDetail();
						oDocPrintEngineDetail.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oDocPrintEngineDetail;
			}

			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					DocPrintEngineDetail oDocPrintEngineDetail = new DocPrintEngineDetail();
					oDocPrintEngineDetail.DocPrintEngineDetailID = id;
					DBTableReferenceDA.HasReference(tc, "DocPrintEngineDetail", id);
					DocPrintEngineDetailDA.Delete(tc, oDocPrintEngineDetail, EnumDBOperation.Delete, nUserId);
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

			public DocPrintEngineDetail Get(int id, Int64 nUserId)
			{
				DocPrintEngineDetail oDocPrintEngineDetail = new DocPrintEngineDetail();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = DocPrintEngineDetailDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oDocPrintEngineDetail = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get DocPrintEngineDetail", e);
					#endregion
				}
				return oDocPrintEngineDetail;
			}

			public List<DocPrintEngineDetail> Gets(Int64 nUserID)
			{
				List<DocPrintEngineDetail> oDocPrintEngineDetails = new List<DocPrintEngineDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = DocPrintEngineDetailDA.Gets(tc);
					oDocPrintEngineDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					DocPrintEngineDetail oDocPrintEngineDetail = new DocPrintEngineDetail();
					oDocPrintEngineDetail.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oDocPrintEngineDetails;
			}

			public List<DocPrintEngineDetail> Gets (string sSQL, Int64 nUserID)
			{
				List<DocPrintEngineDetail> oDocPrintEngineDetails = new List<DocPrintEngineDetail>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = DocPrintEngineDetailDA.Gets(tc, sSQL);
					oDocPrintEngineDetails = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get DocPrintEngineDetail", e);
					#endregion
				}
				return oDocPrintEngineDetails;
			}

		#endregion
	}

}
