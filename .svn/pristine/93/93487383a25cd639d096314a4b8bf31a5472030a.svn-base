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
	public class FNProductionBatchQualityService : MarshalByRefObject, IFNProductionBatchQualityService
	{
		#region Private functions and declaration

		private FNProductionBatchQuality MapObject(NullHandler oReader)
		{
			FNProductionBatchQuality oFNProductionBatchQuality = new FNProductionBatchQuality();
			oFNProductionBatchQuality.FNPBQualityID = oReader.GetInt32("FNPBQualityID");
			oFNProductionBatchQuality.FNPBatchID = oReader.GetInt32("FNPBatchID");
			oFNProductionBatchQuality.IsOk = oReader.GetBoolean("IsOk");
			oFNProductionBatchQuality.Remark = oReader.GetString("Remark");
			oFNProductionBatchQuality.BatchNo = oReader.GetString("BatchNo");
			oFNProductionBatchQuality.FNExONo = oReader.GetString("FNExONo");
			oFNProductionBatchQuality.StartBatcherName = oReader.GetString("StartBatcherName");
			oFNProductionBatchQuality.EndBatcherName = oReader.GetString("EndBatcherName");
			oFNProductionBatchQuality.StartQty = oReader.GetDouble("StartQty");
			oFNProductionBatchQuality.EndQty = oReader.GetDouble("EndQty");
			oFNProductionBatchQuality.StartWidth = oReader.GetDouble("StartWidth");
			oFNProductionBatchQuality.EndWidth = oReader.GetDouble("EndWidth");
            oFNProductionBatchQuality.StartDateTime = oReader.GetDateTime("StartDateTime");
            oFNProductionBatchQuality.EndDateTime = oReader.GetDateTime("EndDateTime");
            oFNProductionBatchQuality.QCStatus = (EnumQCStatus)oReader.GetInt32("QCStatus");
			return oFNProductionBatchQuality;
		}

		private FNProductionBatchQuality CreateObject(NullHandler oReader)
		{
			FNProductionBatchQuality oFNProductionBatchQuality = new FNProductionBatchQuality();
			oFNProductionBatchQuality = MapObject(oReader);
			return oFNProductionBatchQuality;
		}

		private List<FNProductionBatchQuality> CreateObjects(IDataReader oReader)
		{
			List<FNProductionBatchQuality> oFNProductionBatchQuality = new List<FNProductionBatchQuality>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				FNProductionBatchQuality oItem = CreateObject(oHandler);
				oFNProductionBatchQuality.Add(oItem);
			}
			return oFNProductionBatchQuality;
		}

		#endregion

		#region Interface implementation
		public FNProductionBatchQuality Save(FNProductionBatchQuality oFNProductionBatchQuality, Int64 nUserID)
		{
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin(true);
				IDataReader reader;
				if (oFNProductionBatchQuality.FNPBQualityID <= 0)
				{
					reader = FNProductionBatchQualityDA.InsertUpdate(tc, oFNProductionBatchQuality, EnumDBOperation.Insert, nUserID);
				}
				else{
						
					reader = FNProductionBatchQualityDA.InsertUpdate(tc, oFNProductionBatchQuality, EnumDBOperation.Update, nUserID);
				}
				NullHandler oReader = new NullHandler(reader);
				if (reader.Read())
				{
					oFNProductionBatchQuality = new FNProductionBatchQuality();
					oFNProductionBatchQuality = CreateObject(oReader);
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
					oFNProductionBatchQuality = new FNProductionBatchQuality();
					oFNProductionBatchQuality.ErrorMessage = e.Message.Split('!')[0];
				}
				#endregion
			}
			return oFNProductionBatchQuality;
		}
        public FNProductionBatchQuality Reprocess(FNProductionBatchQuality oFNProductionBatchQuality, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = FNProductionBatchQualityDA.InsertUpdate(tc, oFNProductionBatchQuality, EnumDBOperation.Revise, nUserID);
                    
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNProductionBatchQuality = new FNProductionBatchQuality();
                    oFNProductionBatchQuality = CreateObject(oReader);
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
                    oFNProductionBatchQuality = new FNProductionBatchQuality();
                    oFNProductionBatchQuality.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oFNProductionBatchQuality;
        }
		public FNProductionBatchQuality Get(int id, Int64 nUserId)
		{
			FNProductionBatchQuality oFNProductionBatchQuality = new FNProductionBatchQuality();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = FNProductionBatchQualityDA.Get(tc, id);
				NullHandler oReader = new NullHandler(reader);
				if (reader.Read())
				{
				oFNProductionBatchQuality = CreateObject(oReader);
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
				throw new ServiceException("Failed to Get FNProductionBatchQuality", e);
				#endregion
			}
			return oFNProductionBatchQuality;
		}
        public List<FNProductionBatchQuality> Gets(Int64 nUserID)
		{
			List<FNProductionBatchQuality> oFNProductionBatchQualitys = new List<FNProductionBatchQuality>();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = null;
				reader = FNProductionBatchQualityDA.Gets(tc);
				oFNProductionBatchQualitys = CreateObjects(reader);
				reader.Close();
				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exception
				if (tc != null)
				tc.HandleError();
				FNProductionBatchQuality oFNProductionBatchQuality = new FNProductionBatchQuality();
				oFNProductionBatchQuality.ErrorMessage =  e.Message.Split('!')[0];
				#endregion
			}
			return oFNProductionBatchQualitys;
		}
		public List<FNProductionBatchQuality> Gets (string sSQL, Int64 nUserID)
		{
			List<FNProductionBatchQuality> oFNProductionBatchQualitys = new List<FNProductionBatchQuality>();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = null;
				reader = FNProductionBatchQualityDA.Gets(tc, sSQL);
				oFNProductionBatchQualitys = CreateObjects(reader);
				reader.Close();
				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exception
				if (tc != null);
				tc.HandleError();
				ExceptionLog.Write(e);
				throw new ServiceException("Failed to Get FNProductionBatchQuality", e);
				#endregion
			}
			return oFNProductionBatchQualitys;
		}

		#endregion
	}

}
