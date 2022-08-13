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
	public class FNProductionConsumptionService : MarshalByRefObject, IFNProductionConsumptionService
	{
		#region Private functions and declaration
		private FNProductionConsumption MapObject(NullHandler oReader)
		{
			FNProductionConsumption oFNProductionConsumption = new FNProductionConsumption();
			oFNProductionConsumption.FNPConsumptionID = oReader.GetInt32("FNPConsumptionID");
            oFNProductionConsumption.FNProductionID = oReader.GetInt32("FNProductionID");
            oFNProductionConsumption.FNPBatchID = oReader.GetInt32("FNPBatchID");
            oFNProductionConsumption.FNRDetailID = oReader.GetInt32("FNRDetailID");
            oFNProductionConsumption.FNBatchCardID = oReader.GetInt32("FNBatchCardID");
            oFNProductionConsumption.QCStatus = (EnumQCStatus)oReader.GetInt32("QCStatus");
			oFNProductionConsumption.ProductID = oReader.GetInt32("ProductID");
			oFNProductionConsumption.LotID = oReader.GetInt32("LotID");
            oFNProductionConsumption.Qty = oReader.GetDouble("Qty");
            oFNProductionConsumption.UnitPrice = oReader.GetDouble("UnitPrice");
            oFNProductionConsumption.LotBalance = oReader.GetDouble("LotBalance");
			oFNProductionConsumption.MUID = oReader.GetInt32("MUID");
			oFNProductionConsumption.ProductCode = oReader.GetString("ProductCode");
			oFNProductionConsumption.ProductName = oReader.GetString("ProductName");
			oFNProductionConsumption.LotNo = oReader.GetString("LotNo");
            oFNProductionConsumption.FNRNo = oReader.GetString("FNRNo");
            
            oFNProductionConsumption.MUName = oReader.GetString("MUName");
            oFNProductionConsumption.Symbol = oReader.GetString("Symbol");
            oFNProductionConsumption.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oFNProductionConsumption.IsQCDone = oReader.GetBoolean("IsQCDone");
			return oFNProductionConsumption;
		}

		private FNProductionConsumption CreateObject(NullHandler oReader)
		{
			FNProductionConsumption oFNProductionConsumption = new FNProductionConsumption();
			oFNProductionConsumption = MapObject(oReader);
			return oFNProductionConsumption;
		}

		private List<FNProductionConsumption> CreateObjects(IDataReader oReader)
		{
			List<FNProductionConsumption> oFNProductionConsumption = new List<FNProductionConsumption>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				FNProductionConsumption oItem = CreateObject(oHandler);
				oFNProductionConsumption.Add(oItem);
			}
			return oFNProductionConsumption;
		}

		#endregion

		#region Interface implementation
			public FNProductionConsumption Save(FNProductionConsumption oFNProductionConsumption, Int64 nUserID)
			{
                TransactionContext tc = null;
                FNProduction oFNProduction = new FNProduction();
                oFNProduction = oFNProductionConsumption.FNProduction;
                try
                {
                    tc = TransactionContext.Begin(true);
                    IDataReader reader;
                    NullHandler oReader;
                    if (oFNProductionConsumption.FNProductionID == 0)
                    {
                        if (oFNProductionConsumption.FNProduction != null)
                        {
                            reader = FNProductionDA.InsertUpdate(tc, oFNProductionConsumption.FNProduction, (int)EnumDBOperation.Insert, nUserID);
                            oReader = new NullHandler(reader);
                            if (reader.Read())
                            {
                                oFNProduction = FNProductionService.CreateObject(oReader);
                            }
                            reader.Close();
                            oFNProductionConsumption.FNProductionID = oFNProduction.FNProductionID;
                        }
                        else { throw new Exception("No FN Production information found to save."); }
                    }
                    if (oFNProductionConsumption.FNPConsumptionID <= 0)
                    {
                        reader = FNProductionConsumptionDA.InsertUpdate(tc, oFNProductionConsumption, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        reader = FNProductionConsumptionDA.InsertUpdate(tc, oFNProductionConsumption, EnumDBOperation.Update, nUserID);
                    }
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFNProductionConsumption = new FNProductionConsumption();
                        oFNProductionConsumption = CreateObject(oReader);
                    }
                    reader.Close();
                    tc.End();
                }
                catch (Exception ex)
                {
                    #region Handle Exception
                    if (tc != null)
                        tc.HandleError();
                    oFNProductionConsumption.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                    #endregion
                }
                oFNProductionConsumption.FNProduction = oFNProduction;
                return oFNProductionConsumption;
			}

			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					FNProductionConsumption oFNProductionConsumption = new FNProductionConsumption();
					oFNProductionConsumption.FNPConsumptionID = id;
					FNProductionConsumptionDA.Delete(tc, oFNProductionConsumption, EnumDBOperation.Delete, nUserId);
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

			public FNProductionConsumption Get(int id, Int64 nUserId)
			{
				FNProductionConsumption oFNProductionConsumption = new FNProductionConsumption();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = FNProductionConsumptionDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oFNProductionConsumption = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get FNProductionConsumption", e);
					#endregion
				}
				return oFNProductionConsumption;
			}

			public List<FNProductionConsumption> Gets(int id, Int64 nUserID)
			{
				List<FNProductionConsumption> oFNProductionConsumptions = new List<FNProductionConsumption>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = FNProductionConsumptionDA.Gets(id, tc);
					oFNProductionConsumptions = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					FNProductionConsumption oFNProductionConsumption = new FNProductionConsumption();
					oFNProductionConsumption.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oFNProductionConsumptions;
			}

			public List<FNProductionConsumption> Gets (string sSQL, Int64 nUserID)
			{
				List<FNProductionConsumption> oFNProductionConsumptions = new List<FNProductionConsumption>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = FNProductionConsumptionDA.Gets(tc, sSQL);
					oFNProductionConsumptions = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get FNProductionConsumption", e);
					#endregion
				}
				return oFNProductionConsumptions;
			}

		#endregion
	}

}
