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
	public class FADepreciationService : MarshalByRefObject, IFADepreciationService
	{
		#region Private functions and declaration

		private FADepreciation MapObject(NullHandler oReader)
		{
			FADepreciation oFADepreciation = new FADepreciation();
			oFADepreciation.FADepreciationID = oReader.GetInt32("FADepreciationID");
			oFADepreciation.FADepreciationNo = oReader.GetString("FADepreciationNo");
			oFADepreciation.BUID = oReader.GetInt32("BUID");
			oFADepreciation.BUName = oReader.GetString("BUName");
			oFADepreciation.DepreciationDate = oReader.GetDateTime("DepreciationDate");
			oFADepreciation.ApprovedBy = oReader.GetInt32("ApprovedBy");
			oFADepreciation.ApprovedByName = oReader.GetString("ApprovedByName");
			oFADepreciation.Remarks = oReader.GetString("Remarks");
			return oFADepreciation;
		}

		private FADepreciation CreateObject(NullHandler oReader)
		{
			FADepreciation oFADepreciation = new FADepreciation();
			oFADepreciation = MapObject(oReader);
			return oFADepreciation;
		}

		private List<FADepreciation> CreateObjects(IDataReader oReader)
		{
			List<FADepreciation> oFADepreciation = new List<FADepreciation>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				FADepreciation oItem = CreateObject(oHandler);
				oFADepreciation.Add(oItem);
			}
			return oFADepreciation;
		}

		#endregion

		#region Interface implementation
			public FADepreciation Save(FADepreciation oFADepreciation, Int64 nUserID)
			{
				TransactionContext tc = null;
                List<FADepreciation> oFADepreciations = new List<FADepreciation>();
				try
				{
					tc = TransactionContext.Begin(true);
					AuthorizationRoleDA.CheckUserPermission(tc, nUserID,EnumModuleName.FADepreciation, EnumRoleOperationType.Add);
                    IDataReader reader = FADepreciationDA.InsertUpdate(tc, oFADepreciation, EnumDBOperation.Insert, nUserID);
                    oFADepreciations = CreateObjects(reader);		
					reader.Close();
					tc.End();
                    oFADepreciation.FADepreciations = oFADepreciations;
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					{
						tc.HandleError();
						oFADepreciation = new FADepreciation();
						oFADepreciation.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oFADepreciation;
			}

            public FADepreciation Approval(FADepreciation oFADepreciation, Int64 nUserID)
            {
                TransactionContext tc = null;
                List<FADepreciation> oFADepreciations = new List<FADepreciation>();
                try
                {
                    tc = TransactionContext.Begin(true);
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.FADepreciation, EnumRoleOperationType.Approved);
                    IDataReader reader = FADepreciationDA.Approval(tc, oFADepreciation,  nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFADepreciation = CreateObject(oReader);
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
                        oFADepreciation = new FADepreciation();
                        oFADepreciation.ErrorMessage = e.Message.Split('!')[0];
                    }
                    #endregion
                }
                return oFADepreciation;
            }
			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					FADepreciation oFADepreciation = new FADepreciation();
					oFADepreciation.FADepreciationID = id;
					AuthorizationRoleDA.CheckUserPermission(tc, nUserId,  EnumModuleName.FADepreciation, EnumRoleOperationType.Delete);
					DBTableReferenceDA.HasReference(tc, "FADepreciation", id);
					FADepreciationDA.Delete(tc, oFADepreciation, EnumDBOperation.Delete, nUserId);
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

			public FADepreciation Get(int id, Int64 nUserId)
			{
				FADepreciation oFADepreciation = new FADepreciation();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = FADepreciationDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oFADepreciation = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get FADepreciation", e);
					#endregion
				}
				return oFADepreciation;
			}

			public List<FADepreciation> Gets (string sSQL, Int64 nUserID)
			{
				List<FADepreciation> oFADepreciations = new List<FADepreciation>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = FADepreciationDA.Gets(tc, sSQL);
					oFADepreciations = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get FADepreciation", e);
					#endregion
				}
				return oFADepreciations;
			}

		#endregion
	}

}
