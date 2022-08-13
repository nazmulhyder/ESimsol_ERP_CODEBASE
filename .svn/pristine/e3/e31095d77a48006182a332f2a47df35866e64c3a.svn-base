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
	public class GUPReportSetUpService : MarshalByRefObject, IGUPReportSetUpService
	{
		#region Private functions and declaration

		private GUPReportSetUp MapObject(NullHandler oReader)
		{
			GUPReportSetUp oGUPReportSetUp = new GUPReportSetUp();
			oGUPReportSetUp.GUPReportSetUpID = oReader.GetInt32("GUPReportSetUpID");
			oGUPReportSetUp.ProductionStepID = oReader.GetInt32("ProductionStepID");
			oGUPReportSetUp.Sequence = oReader.GetInt32("Sequence");
            oGUPReportSetUp.StepName = oReader.GetString("StepName");
			return oGUPReportSetUp;
		}

		private GUPReportSetUp CreateObject(NullHandler oReader)
		{
			GUPReportSetUp oGUPReportSetUp = new GUPReportSetUp();
			oGUPReportSetUp = MapObject(oReader);
			return oGUPReportSetUp;
		}

		private List<GUPReportSetUp> CreateObjects(IDataReader oReader)
		{
			List<GUPReportSetUp> oGUPReportSetUp = new List<GUPReportSetUp>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				GUPReportSetUp oItem = CreateObject(oHandler);
				oGUPReportSetUp.Add(oItem);
			}
			return oGUPReportSetUp;
		}

		#endregion

		#region Interface implementation
			public GUPReportSetUp Save(GUPReportSetUp oGUPReportSetUp, Int64 nUserID)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oGUPReportSetUp.GUPReportSetUpID <= 0)
					{
						//AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "GUPReportSetUp", EnumRoleOperationType.Add);
						reader = GUPReportSetUpDA.InsertUpdate(tc, oGUPReportSetUp, EnumDBOperation.Insert, nUserID);
					}
					else{
						//AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "GUPReportSetUp", EnumRoleOperationType.Edit);
						reader = GUPReportSetUpDA.InsertUpdate(tc, oGUPReportSetUp, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oGUPReportSetUp = new GUPReportSetUp();
						oGUPReportSetUp = CreateObject(oReader);
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
						oGUPReportSetUp = new GUPReportSetUp();
						oGUPReportSetUp.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oGUPReportSetUp;
			}

			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					GUPReportSetUp oGUPReportSetUp = new GUPReportSetUp();
					oGUPReportSetUp.GUPReportSetUpID = id;
					DBTableReferenceDA.HasReference(tc, "GUPReportSetUp", id);
					GUPReportSetUpDA.Delete(tc, oGUPReportSetUp, EnumDBOperation.Delete, nUserId);
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

			public GUPReportSetUp Get(int id, Int64 nUserId)
			{
				GUPReportSetUp oGUPReportSetUp = new GUPReportSetUp();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = GUPReportSetUpDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oGUPReportSetUp = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get GUPReportSetUp", e);
					#endregion
				}
				return oGUPReportSetUp;
			}

			public List<GUPReportSetUp> Gets(Int64 nUserID)
			{
				List<GUPReportSetUp> oGUPReportSetUps = new List<GUPReportSetUp>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = GUPReportSetUpDA.Gets(tc);
					oGUPReportSetUps = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					GUPReportSetUp oGUPReportSetUp = new GUPReportSetUp();
					oGUPReportSetUp.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oGUPReportSetUps;
			}

			public List<GUPReportSetUp> Gets (string sSQL, Int64 nUserID)
			{
				List<GUPReportSetUp> oGUPReportSetUps = new List<GUPReportSetUp>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = GUPReportSetUpDA.Gets(tc, sSQL);
					oGUPReportSetUps = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get GUPReportSetUp", e);
					#endregion
				}
				return oGUPReportSetUps;
			}
            public List<GUPReportSetUp> UpDown(GUPReportSetUp oGUPReportSetUp, Int64 nUserID)
            {

                TransactionContext tc = null;
                IDataReader reader;
                List<GUPReportSetUp> oGUPReportSetUps = new List<GUPReportSetUp>();
                try
                {
                    tc = TransactionContext.Begin(true);
                    reader = GUPReportSetUpDA.UpDown(tc, oGUPReportSetUp, nUserID);
                    oGUPReportSetUps = CreateObjects(reader);
                    reader.Close();
                    tc.End();
                }
                catch (Exception ex)
                {
                    #region Handle Exception
                    tc.HandleError();
                    oGUPReportSetUp = new GUPReportSetUp();
                    oGUPReportSetUp.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                    oGUPReportSetUps.Add(oGUPReportSetUp);
                    #endregion

                }
                return oGUPReportSetUps;
            }


		#endregion
	}

}
