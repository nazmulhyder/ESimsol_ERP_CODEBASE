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
	public class KnitDyeingYarnRequisitionService : MarshalByRefObject, IKnitDyeingYarnRequisitionService
	{
		#region Private functions and declaration

		private KnitDyeingYarnRequisition MapObject(NullHandler oReader)
		{
			KnitDyeingYarnRequisition oKnitDyeingYarnRequisition = new KnitDyeingYarnRequisition();
			oKnitDyeingYarnRequisition.KnitDyeingYarnRequisitionID = oReader.GetInt32("KnitDyeingYarnRequisitionID");
			oKnitDyeingYarnRequisition.KnitDyeingProgramID = oReader.GetInt32("KnitDyeingProgramID");
            oKnitDyeingYarnRequisition.KnitDyeingYarnRequisitionLogID = oReader.GetInt32("KnitDyeingYarnRequisitionLogID");
            oKnitDyeingYarnRequisition.KnitDyeingProgramLogID = oReader.GetInt32("KnitDyeingProgramLogID");
			oKnitDyeingYarnRequisition.YarnCountID = oReader.GetInt32("YarnCountID");
			oKnitDyeingYarnRequisition.UsagesParcent = oReader.GetDouble("UsagesParcent");
            oKnitDyeingYarnRequisition.FabricTypeID = oReader.GetInt32("FabricTypeID");
            oKnitDyeingYarnRequisition.FinishRequiredQty = oReader.GetDouble("FinishRequiredQty");
            oKnitDyeingYarnRequisition.GracePercent = oReader.GetDouble("GracePercent");
			oKnitDyeingYarnRequisition.RequiredQty = oReader.GetDouble("RequiredQty");
			oKnitDyeingYarnRequisition.MUnitID = oReader.GetInt32("MUnitID");
			oKnitDyeingYarnRequisition.Remarks = oReader.GetString("Remarks");
            oKnitDyeingYarnRequisition.YarnName = oReader.GetString("YarnName");
            oKnitDyeingYarnRequisition.UnitSymbol = oReader.GetString("UnitSymbol");
            oKnitDyeingYarnRequisition.FabricTypeName = oReader.GetString("FabricTypeName");
			return oKnitDyeingYarnRequisition;
		}

		private KnitDyeingYarnRequisition CreateObject(NullHandler oReader)
		{
			KnitDyeingYarnRequisition oKnitDyeingYarnRequisition = new KnitDyeingYarnRequisition();
			oKnitDyeingYarnRequisition = MapObject(oReader);
			return oKnitDyeingYarnRequisition;
		}

		private List<KnitDyeingYarnRequisition> CreateObjects(IDataReader oReader)
		{
			List<KnitDyeingYarnRequisition> oKnitDyeingYarnRequisition = new List<KnitDyeingYarnRequisition>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				KnitDyeingYarnRequisition oItem = CreateObject(oHandler);
				oKnitDyeingYarnRequisition.Add(oItem);
			}
			return oKnitDyeingYarnRequisition;
		}

		#endregion

		#region Interface implementation
			public KnitDyeingYarnRequisition Save(KnitDyeingYarnRequisition oKnitDyeingYarnRequisition, Int64 nUserID)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oKnitDyeingYarnRequisition.KnitDyeingYarnRequisitionID <= 0)
					{
						reader = KnitDyeingYarnRequisitionDA.InsertUpdate(tc, oKnitDyeingYarnRequisition, EnumDBOperation.Insert, nUserID,"");
					}
					else{
						reader = KnitDyeingYarnRequisitionDA.InsertUpdate(tc, oKnitDyeingYarnRequisition, EnumDBOperation.Update, nUserID,"");
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oKnitDyeingYarnRequisition = new KnitDyeingYarnRequisition();
						oKnitDyeingYarnRequisition = CreateObject(oReader);
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
						oKnitDyeingYarnRequisition = new KnitDyeingYarnRequisition();
						oKnitDyeingYarnRequisition.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oKnitDyeingYarnRequisition;
			}

			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					KnitDyeingYarnRequisition oKnitDyeingYarnRequisition = new KnitDyeingYarnRequisition();
					oKnitDyeingYarnRequisition.KnitDyeingYarnRequisitionID = id;
					DBTableReferenceDA.HasReference(tc, "KnitDyeingYarnRequisition", id);
					KnitDyeingYarnRequisitionDA.Delete(tc, oKnitDyeingYarnRequisition, EnumDBOperation.Delete, nUserId,"");
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

            public List<KnitDyeingYarnRequisition> Gets(int id, Int64 nUserID)
            {
                List<KnitDyeingYarnRequisition> oKnitDyeingYarnRequisitions = new List<KnitDyeingYarnRequisition>();
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin();
                    IDataReader reader = null;
                    reader = KnitDyeingYarnRequisitionDA.Gets(tc, id);
                    oKnitDyeingYarnRequisitions = CreateObjects(reader);
                    reader.Close();
                    tc.End();
                }
                catch (Exception e)
                {
                    #region Handle Exception
                    if (tc != null)
                        tc.HandleError();
                    KnitDyeingYarnRequisition oKnitDyeingYarnRequisition = new KnitDyeingYarnRequisition();
                    oKnitDyeingYarnRequisition.ErrorMessage = e.Message.Split('!')[0];
                    #endregion
                }
                return oKnitDyeingYarnRequisitions;
            }
            //
            public List<KnitDyeingYarnRequisition> GetsLog(int LogId, Int64 nUserID)
            {
                List<KnitDyeingYarnRequisition> oKnitDyeingYarnRequisitions = new List<KnitDyeingYarnRequisition>();
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin();
                    IDataReader reader = null;
                    reader = KnitDyeingYarnRequisitionDA.GetsLog(tc, LogId);
                    oKnitDyeingYarnRequisitions = CreateObjects(reader);
                    reader.Close();
                    tc.End();
                }
                catch (Exception e)
                {
                    #region Handle Exception
                    if (tc != null)
                        tc.HandleError();
                    KnitDyeingYarnRequisition oKnitDyeingYarnRequisition = new KnitDyeingYarnRequisition();
                    oKnitDyeingYarnRequisition.ErrorMessage = e.Message.Split('!')[0];
                    #endregion
                }
                return oKnitDyeingYarnRequisitions;
            }
			public KnitDyeingYarnRequisition Get(int id, Int64 nUserId)
			{
				KnitDyeingYarnRequisition oKnitDyeingYarnRequisition = new KnitDyeingYarnRequisition();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = KnitDyeingYarnRequisitionDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oKnitDyeingYarnRequisition = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get KnitDyeingYarnRequisition", e);
					#endregion
				}
				return oKnitDyeingYarnRequisition;
			}

			public List<KnitDyeingYarnRequisition> Gets(Int64 nUserID)
			{
				List<KnitDyeingYarnRequisition> oKnitDyeingYarnRequisitions = new List<KnitDyeingYarnRequisition>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = KnitDyeingYarnRequisitionDA.Gets(tc);
					oKnitDyeingYarnRequisitions = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					KnitDyeingYarnRequisition oKnitDyeingYarnRequisition = new KnitDyeingYarnRequisition();
					oKnitDyeingYarnRequisition.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oKnitDyeingYarnRequisitions;
			}

			public List<KnitDyeingYarnRequisition> Gets (string sSQL, Int64 nUserID)
			{
				List<KnitDyeingYarnRequisition> oKnitDyeingYarnRequisitions = new List<KnitDyeingYarnRequisition>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = KnitDyeingYarnRequisitionDA.Gets(tc, sSQL);
					oKnitDyeingYarnRequisitions = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get KnitDyeingYarnRequisition", e);
					#endregion
				}
				return oKnitDyeingYarnRequisitions;
			}

		#endregion
	}

}
