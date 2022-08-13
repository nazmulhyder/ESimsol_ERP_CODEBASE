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
	public class ExportClaimSettleService : MarshalByRefObject, IExportClaimSettleService
	{
		#region Private functions and declaration

		private ExportClaimSettle MapObject(NullHandler oReader)
		{
			ExportClaimSettle oExportClaimSettle = new ExportClaimSettle();
			oExportClaimSettle.ExportClaimSettleID = oReader.GetInt32("ExportClaimSettleID");
			oExportClaimSettle.ExportBillID = oReader.GetInt32("ExportBillID");
            oExportClaimSettle.InoutTypeInt = oReader.GetInt32("InoutType");
            oExportClaimSettle.InOutType = (EnumInOutType)oReader.GetInt32("InoutType");
			oExportClaimSettle.SettleName  = oReader.GetString("SettleName");
            oExportClaimSettle.SettleByName = oReader.GetString("SettleByName");
            
			oExportClaimSettle.Amount = oReader.GetDouble("Amount");
			oExportClaimSettle.CurrencyID = oReader.GetInt32("CurrencyID");
			return oExportClaimSettle;
		}

		private ExportClaimSettle CreateObject(NullHandler oReader)
		{
			ExportClaimSettle oExportClaimSettle = new ExportClaimSettle();
			oExportClaimSettle = MapObject(oReader);
			return oExportClaimSettle;
		}

		private List<ExportClaimSettle> CreateObjects(IDataReader oReader)
		{
			List<ExportClaimSettle> oExportClaimSettle = new List<ExportClaimSettle>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				ExportClaimSettle oItem = CreateObject(oHandler);
				oExportClaimSettle.Add(oItem);
			}
			return oExportClaimSettle;
		}

		#endregion

		#region Interface implementation
			public ExportClaimSettle Save(ExportClaimSettle oExportClaimSettle, Int64 nUserID)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oExportClaimSettle.ExportClaimSettleID <= 0)
					{
						reader = ExportClaimSettleDA.InsertUpdate(tc, oExportClaimSettle, EnumDBOperation.Insert, nUserID);
					}
					else{
						reader = ExportClaimSettleDA.InsertUpdate(tc, oExportClaimSettle, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oExportClaimSettle = new ExportClaimSettle();
						oExportClaimSettle = CreateObject(oReader);
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
						oExportClaimSettle = new ExportClaimSettle();
						oExportClaimSettle.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oExportClaimSettle;
			}

			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					ExportClaimSettle oExportClaimSettle = new ExportClaimSettle();
					oExportClaimSettle.ExportClaimSettleID = id;
					DBTableReferenceDA.HasReference(tc, "ExportClaimSettle", id);
					ExportClaimSettleDA.Delete(tc, oExportClaimSettle, EnumDBOperation.Delete, nUserId);
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

			public ExportClaimSettle Get(int id, Int64 nUserId)
			{
				ExportClaimSettle oExportClaimSettle = new ExportClaimSettle();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = ExportClaimSettleDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oExportClaimSettle = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get ExportClaimSettle", e);
					#endregion
				}
				return oExportClaimSettle;
			}

			public List<ExportClaimSettle> Gets(Int64 nUserID)
			{
				List<ExportClaimSettle> oExportClaimSettles = new List<ExportClaimSettle>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = ExportClaimSettleDA.Gets(tc);
					oExportClaimSettles = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					ExportClaimSettle oExportClaimSettle = new ExportClaimSettle();
					oExportClaimSettle.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oExportClaimSettles;
			}

			public List<ExportClaimSettle> Gets (string sSQL, Int64 nUserID)
			{
				List<ExportClaimSettle> oExportClaimSettles = new List<ExportClaimSettle>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = ExportClaimSettleDA.Gets(tc, sSQL);
					oExportClaimSettles = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get ExportClaimSettle", e);
					#endregion
				}
				return oExportClaimSettles;
			}
            public List<ExportClaimSettle> GetsByBillID(int nExportBillID, Int64 nUserID)
            {
                List<ExportClaimSettle> oExportClaimSettles = new List<ExportClaimSettle>();
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin();
                    IDataReader reader = null;
                    reader = ExportClaimSettleDA.GetsByBillID(nExportBillID, tc);
                    oExportClaimSettles = CreateObjects(reader);
                    reader.Close();
                    tc.End();
                }
                catch (Exception e)
                {
                    #region Handle Exception
                    if (tc != null) ;
                    tc.HandleError();
                    ExceptionLog.Write(e);
                    throw new ServiceException("Failed to Get ExportClaimSettle", e);
                    #endregion
                }
                return oExportClaimSettles;
            }

		#endregion
	}

}
