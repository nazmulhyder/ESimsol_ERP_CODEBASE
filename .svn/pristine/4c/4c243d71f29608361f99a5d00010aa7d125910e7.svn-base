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
	public class DUPSLotService : MarshalByRefObject, IDUPSLotService
	{
		#region Private functions and declaration

		private DUPSLot MapObject(NullHandler oReader)
		{
			DUPSLot oDUPSLot = new DUPSLot();
            oDUPSLot.DUPScheduleID = oReader.GetInt32("DUPScheduleID");
            oDUPSLot.DUPScheduleDetailID = oReader.GetInt32("DUPScheduleDetailID");
            oDUPSLot.DUPSLotID = oReader.GetInt32("DUPSLotID");
            oDUPSLot.LotID = oReader.GetInt32("LotID");
            oDUPSLot.DODID = oReader.GetInt32("DODID");
            oDUPSLot.Qty = oReader.GetDouble("Qty");
            oDUPSLot.Balance = oReader.GetDouble("Balance");
            oDUPSLot.LotNo = oReader.GetString("LotNo");
			return oDUPSLot;
		}

		private DUPSLot CreateObject(NullHandler oReader)
		{
			DUPSLot oDUPSLot = new DUPSLot();
			oDUPSLot = MapObject(oReader);
			return oDUPSLot;
		}

		private List<DUPSLot> CreateObjects(IDataReader oReader)
		{
			List<DUPSLot> oDUPSLot = new List<DUPSLot>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				DUPSLot oItem = CreateObject(oHandler);
				oDUPSLot.Add(oItem);
			}
			return oDUPSLot;
		}

		#endregion

		#region Interface implementation
			public DUPSLot Save(DUPSLot oDUPSLot, Int64 nUserID)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oDUPSLot.DUPSLotID <= 0)
					{
						reader = DUPSLotDA.InsertUpdate(tc, oDUPSLot, EnumDBOperation.Insert, nUserID);
					}
					else{
						reader = DUPSLotDA.InsertUpdate(tc, oDUPSLot, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oDUPSLot = new DUPSLot();
						oDUPSLot = CreateObject(oReader);
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
						oDUPSLot = new DUPSLot();
						oDUPSLot.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oDUPSLot;
			}

			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					DUPSLot oDUPSLot = new DUPSLot();
					oDUPSLot.DUPSLotID = id;
					DBTableReferenceDA.HasReference(tc, "DUPSLot", id);
					DUPSLotDA.Delete(tc, oDUPSLot, EnumDBOperation.Delete, nUserId);
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

			public DUPSLot Get(int id, Int64 nUserId)
			{
				DUPSLot oDUPSLot = new DUPSLot();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = DUPSLotDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oDUPSLot = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get DUPSLot", e);
					#endregion
				}
				return oDUPSLot;
			}

			public List<DUPSLot> Gets(Int64 nUserID)
			{
				List<DUPSLot> oDUPSLots = new List<DUPSLot>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = DUPSLotDA.Gets(tc);
					oDUPSLots = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					DUPSLot oDUPSLot = new DUPSLot();
					oDUPSLot.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oDUPSLots;
			}

			public List<DUPSLot> Gets (string sSQL, Int64 nUserID)
			{
				List<DUPSLot> oDUPSLots = new List<DUPSLot>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = DUPSLotDA.Gets(tc, sSQL);
					oDUPSLots = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get DUPSLot", e);
					#endregion
				}
				return oDUPSLots;
			}
            public List<DUPSLot> GetsBy(int nDUPScheduleID, Int64 nUserID)
            {
                List<DUPSLot> oDUPSLots = new List<DUPSLot>();
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin();
                    IDataReader reader = null;
                    reader = DUPSLotDA.GetsBy(tc, nDUPScheduleID);
                    oDUPSLots = CreateObjects(reader);
                    reader.Close();
                    tc.End();
                }
                catch (Exception e)
                {
                    #region Handle Exception
                    if (tc != null) ;
                    tc.HandleError();
                    ExceptionLog.Write(e);
                    throw new ServiceException("Failed to Get DUPSLot", e);
                    #endregion
                }
                return oDUPSLots;
            }

		#endregion
	}

}
