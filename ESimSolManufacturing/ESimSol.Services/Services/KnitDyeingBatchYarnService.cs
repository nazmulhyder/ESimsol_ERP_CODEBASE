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
	public class KnitDyeingBatchYarnService : MarshalByRefObject, IKnitDyeingBatchYarnService
	{
		#region Private functions and declaration

		private KnitDyeingBatchYarn MapObject(NullHandler oReader)
		{
			KnitDyeingBatchYarn oKnitDyeingBatchYarn = new KnitDyeingBatchYarn();
			oKnitDyeingBatchYarn.KnitDyeingBatchYarnID = oReader.GetInt32("KnitDyeingBatchYarnID");
			oKnitDyeingBatchYarn.KnitDyeingBatchID = oReader.GetInt32("KnitDyeingBatchID");
			oKnitDyeingBatchYarn.CompositionID = oReader.GetInt32("CompositionID");
			oKnitDyeingBatchYarn.CompositionName = oReader.GetString("CompositionName");
			oKnitDyeingBatchYarn.LotID = oReader.GetInt32("LotID");
            oKnitDyeingBatchYarn.LotNo = oReader.GetString("LotNo");
			oKnitDyeingBatchYarn.Remarks = oReader.GetString("Remarks");
			return oKnitDyeingBatchYarn;
		}

		private KnitDyeingBatchYarn CreateObject(NullHandler oReader)
		{
			KnitDyeingBatchYarn oKnitDyeingBatchYarn = new KnitDyeingBatchYarn();
			oKnitDyeingBatchYarn = MapObject(oReader);
			return oKnitDyeingBatchYarn;
		}

		private List<KnitDyeingBatchYarn> CreateObjects(IDataReader oReader)
		{
			List<KnitDyeingBatchYarn> oKnitDyeingBatchYarn = new List<KnitDyeingBatchYarn>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				KnitDyeingBatchYarn oItem = CreateObject(oHandler);
				oKnitDyeingBatchYarn.Add(oItem);
			}
			return oKnitDyeingBatchYarn;
		}

		#endregion

		#region Interface implementation
			public KnitDyeingBatchYarn Save(KnitDyeingBatchYarn oKnitDyeingBatchYarn, Int64 nUserID)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oKnitDyeingBatchYarn.KnitDyeingBatchYarnID <= 0)
					{
						reader = KnitDyeingBatchYarnDA.InsertUpdate(tc, oKnitDyeingBatchYarn, EnumDBOperation.Insert, nUserID, "");
					}
					else{
						reader = KnitDyeingBatchYarnDA.InsertUpdate(tc, oKnitDyeingBatchYarn, EnumDBOperation.Update, nUserID, "");
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oKnitDyeingBatchYarn = new KnitDyeingBatchYarn();
						oKnitDyeingBatchYarn = CreateObject(oReader);
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
						oKnitDyeingBatchYarn = new KnitDyeingBatchYarn();
						oKnitDyeingBatchYarn.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oKnitDyeingBatchYarn;
			}

			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					KnitDyeingBatchYarn oKnitDyeingBatchYarn = new KnitDyeingBatchYarn();
					oKnitDyeingBatchYarn.KnitDyeingBatchYarnID = id;
					DBTableReferenceDA.HasReference(tc, "KnitDyeingBatchYarn", id);
					KnitDyeingBatchYarnDA.Delete(tc, oKnitDyeingBatchYarn, EnumDBOperation.Delete, nUserId,"");
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

			public KnitDyeingBatchYarn Get(int id, Int64 nUserId)
			{
				KnitDyeingBatchYarn oKnitDyeingBatchYarn = new KnitDyeingBatchYarn();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = KnitDyeingBatchYarnDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oKnitDyeingBatchYarn = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get KnitDyeingBatchYarn", e);
					#endregion
				}
				return oKnitDyeingBatchYarn;
			}

			public List<KnitDyeingBatchYarn> Gets(Int64 nUserID)
			{
				List<KnitDyeingBatchYarn> oKnitDyeingBatchYarns = new List<KnitDyeingBatchYarn>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = KnitDyeingBatchYarnDA.Gets(tc);
					oKnitDyeingBatchYarns = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					KnitDyeingBatchYarn oKnitDyeingBatchYarn = new KnitDyeingBatchYarn();
					oKnitDyeingBatchYarn.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oKnitDyeingBatchYarns;
			}

			public List<KnitDyeingBatchYarn> Gets (string sSQL, Int64 nUserID)
			{
				List<KnitDyeingBatchYarn> oKnitDyeingBatchYarns = new List<KnitDyeingBatchYarn>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = KnitDyeingBatchYarnDA.Gets(tc, sSQL);
					oKnitDyeingBatchYarns = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get KnitDyeingBatchYarn", e);
					#endregion
				}
				return oKnitDyeingBatchYarns;
			}

		#endregion
	}

}
