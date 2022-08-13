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
	public class CostSheetCMService : MarshalByRefObject, ICostSheetCMService
	{
		#region Private functions and declaration

		private CostSheetCM MapObject(NullHandler oReader)
		{
			CostSheetCM oCostSheetCM = new CostSheetCM();
			oCostSheetCM.CostSheetCMID = oReader.GetInt32("CostSheetCMID");
			oCostSheetCM.CostSheetID = oReader.GetInt32("CostSheetID");
            oCostSheetCM.CMType = (EnumCMType)oReader.GetInt32("CMType");
            oCostSheetCM.CMTypeInt = oReader.GetInt32("CMType");
			oCostSheetCM.NumberOfMachine = oReader.GetInt32("NumberOfMachine");
			oCostSheetCM.MachineCost = oReader.GetDouble("MachineCost");
			oCostSheetCM.ProductionPerDay = oReader.GetDouble("ProductionPerDay");
			oCostSheetCM.BufferDays = oReader.GetInt32("BufferDays");
			oCostSheetCM.TotalRequiredDays = oReader.GetInt32("TotalRequiredDays");
			oCostSheetCM.CMAdditionalPerent = oReader.GetDouble("CMAdditionalPerent");
            oCostSheetCM.CMPart = oReader.GetString("CMPart");
            
            oCostSheetCM.CSQty = oReader.GetDouble("CSQty");
			return oCostSheetCM;
		}

		private CostSheetCM CreateObject(NullHandler oReader)
		{
			CostSheetCM oCostSheetCM = new CostSheetCM();
			oCostSheetCM = MapObject(oReader);
			return oCostSheetCM;
		}

		private List<CostSheetCM> CreateObjects(IDataReader oReader)
		{
			List<CostSheetCM> oCostSheetCM = new List<CostSheetCM>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				CostSheetCM oItem = CreateObject(oHandler);
				oCostSheetCM.Add(oItem);
			}
			return oCostSheetCM;
		}

		#endregion

		#region Interface implementation


        public CostSheetCM Get(int id, Int64 nUserId)
        {
            CostSheetCM oCostSheetCM = new CostSheetCM();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = CostSheetCMDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCostSheetCM = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get CostSheetCM", e);
                #endregion
            }
            return oCostSheetCM;
        }

			public List<CostSheetCM> Gets(int CSID, Int64 nUserID)
			{
				List<CostSheetCM> oCostSheetCMs = new List<CostSheetCM>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = CostSheetCMDA.Gets(CSID, tc);
					oCostSheetCMs = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					CostSheetCM oCostSheetCM = new CostSheetCM();
					oCostSheetCM.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oCostSheetCMs;
			}
            public List<CostSheetCM> GetsByLog(int CSLogID, Int64 nUserID)
			{
				List<CostSheetCM> oCostSheetCMs = new List<CostSheetCM>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = CostSheetCMDA.GetsByLog(CSLogID, tc);
					oCostSheetCMs = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					CostSheetCM oCostSheetCM = new CostSheetCM();
					oCostSheetCM.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oCostSheetCMs;
			}
        

			public List<CostSheetCM> Gets (string sSQL, Int64 nUserID)
			{
				List<CostSheetCM> oCostSheetCMs = new List<CostSheetCM>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = CostSheetCMDA.Gets(tc, sSQL);
					oCostSheetCMs = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get CostSheetCM", e);
					#endregion
				}
				return oCostSheetCMs;
			}

		#endregion
	}

}
