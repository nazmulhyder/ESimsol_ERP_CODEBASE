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
	public class FabricBatchSizingSolutionService : MarshalByRefObject, IFabricBatchSizingSolutionService
	{
		#region Private functions and declaration

		private FabricBatchSizingSolution MapObject(NullHandler oReader)
		{
			FabricBatchSizingSolution oFabricBatchSizingSolution = new FabricBatchSizingSolution();
			oFabricBatchSizingSolution.FBID = oReader.GetInt32("FBID");
			oFabricBatchSizingSolution.WaterQty = oReader.GetDouble("WaterQty");
			oFabricBatchSizingSolution.Dry = oReader.GetDouble("Dry");
			oFabricBatchSizingSolution.Wet = oReader.GetDouble("Wet");
			oFabricBatchSizingSolution.RF = oReader.GetDouble("RF");
			oFabricBatchSizingSolution.Viscosity = oReader.GetDouble("Viscosity");
			oFabricBatchSizingSolution.FinalVolume = oReader.GetDouble("FinalVolume");
			oFabricBatchSizingSolution.RestQty = oReader.GetDouble("RestQty");
			oFabricBatchSizingSolution.PreviousRestQty = oReader.GetDouble("PreviousRestQty");
			return oFabricBatchSizingSolution;
		}

		private FabricBatchSizingSolution CreateObject(NullHandler oReader)
		{
			FabricBatchSizingSolution oFabricBatchSizingSolution = new FabricBatchSizingSolution();
			oFabricBatchSizingSolution = MapObject(oReader);
			return oFabricBatchSizingSolution;
		}

		private List<FabricBatchSizingSolution> CreateObjects(IDataReader oReader)
		{
			List<FabricBatchSizingSolution> oFabricBatchSizingSolution = new List<FabricBatchSizingSolution>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				FabricBatchSizingSolution oItem = CreateObject(oHandler);
				oFabricBatchSizingSolution.Add(oItem);
			}
			return oFabricBatchSizingSolution;
		}

		#endregion

		#region Interface implementation
			public FabricBatchSizingSolution Save(FabricBatchSizingSolution oFabricBatchSizingSolution, Int64 nUserID)
			{
				TransactionContext tc = null;
				try
				{
                    FabricBatchSizingSolution oTempFBSS = new FabricBatchSizingSolution();
                    oTempFBSS = FabricBatchSizingSolution.Get(oFabricBatchSizingSolution.FBID, nUserID);
					tc = TransactionContext.Begin(true);
					IDataReader reader;
                  
                    if(oTempFBSS.FBID>0)
                    {
                        reader = FabricBatchSizingSolutionDA.InsertUpdate(tc, oFabricBatchSizingSolution, EnumDBOperation.Update, nUserID);
                    }
                    else
                    {
                        reader = FabricBatchSizingSolutionDA.InsertUpdate(tc, oFabricBatchSizingSolution, EnumDBOperation.Insert, nUserID);
                    }
                    NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oFabricBatchSizingSolution = new FabricBatchSizingSolution();
						oFabricBatchSizingSolution = CreateObject(oReader);
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
						oFabricBatchSizingSolution = new FabricBatchSizingSolution();
						oFabricBatchSizingSolution.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oFabricBatchSizingSolution;
			}

            public string Delete(int id, Int64 nUserId)
            {
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin(true);
                    FabricBatchSizingSolution oFabricBatchSizingSolution = new FabricBatchSizingSolution();
                   
                    DBTableReferenceDA.HasReference(tc, "FabricBatchSizingSolution", id);
                    FabricBatchSizingSolutionDA.Delete(tc, oFabricBatchSizingSolution, EnumDBOperation.Delete, nUserId);
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
            public double GetPrevQtyForSizing(Int64 nUserId)
            {
                TransactionContext tc = null;
                double SugPrevQty = 0;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
                    reader = FabricBatchSizingSolutionDA.Gets(tc, "SELECT ISNULL(SUM(RestQty),0)-ISNULL(SUM(PreviousRestQty),0) AS SugPrevQty FROM FabricBatchSizingSolution");
                  
                    if (reader.Read())
                    {
                        SugPrevQty = Convert.ToDouble(reader["SugPrevQty"].ToString());
                    }
                    
                   
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get FabricBatchSizingSolution", e);
					#endregion
				}

                return SugPrevQty;
            }

			public FabricBatchSizingSolution Get(int id, Int64 nUserId)
			{
				FabricBatchSizingSolution oFabricBatchSizingSolution = new FabricBatchSizingSolution();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = FabricBatchSizingSolutionDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oFabricBatchSizingSolution = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get FabricBatchSizingSolution", e);
					#endregion
				}
				return oFabricBatchSizingSolution;
			}

			public List<FabricBatchSizingSolution> Gets(Int64 nUserID)
			{
				List<FabricBatchSizingSolution> oFabricBatchSizingSolutions = new List<FabricBatchSizingSolution>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = FabricBatchSizingSolutionDA.Gets(tc);
					oFabricBatchSizingSolutions = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					FabricBatchSizingSolution oFabricBatchSizingSolution = new FabricBatchSizingSolution();
					oFabricBatchSizingSolution.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oFabricBatchSizingSolutions;
			}

			public List<FabricBatchSizingSolution> Gets (string sSQL, Int64 nUserID)
			{
				List<FabricBatchSizingSolution> oFabricBatchSizingSolutions = new List<FabricBatchSizingSolution>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = FabricBatchSizingSolutionDA.Gets(tc, sSQL);
					oFabricBatchSizingSolutions = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get FabricBatchSizingSolution", e);
					#endregion
				}
				return oFabricBatchSizingSolutions;
			}

		#endregion
	}

}
