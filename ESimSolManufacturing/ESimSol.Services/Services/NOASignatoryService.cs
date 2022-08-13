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
	public class NOASignatoryService : MarshalByRefObject, INOASignatoryService
	{
		#region Private functions and declaration

		private NOASignatory MapObject(NullHandler oReader)
		{
            NOASignatory oNOASignatory = new NOASignatory();
            oNOASignatory.NOASignatoryLogID = oReader.GetInt32("NOASignatoryLogID");
            oNOASignatory.NOALogID = oReader.GetInt32("NOALogID");
            oNOASignatory.NOASignatoryID = oReader.GetInt32("NOASignatoryID");
            oNOASignatory.ApprovalHeadID = oReader.GetInt32("ApprovalHeadID");
			oNOASignatory.NOAID = oReader.GetInt32("NOAID");
			oNOASignatory.SLNo  = oReader.GetInt32("SLNo");
			oNOASignatory.ReviseNo = oReader.GetInt32("ReviseNo");
			oNOASignatory.RequestTo = oReader.GetInt32("RequestTo");
			oNOASignatory.ApproveBy = oReader.GetInt32("ApproveBy");
			oNOASignatory.ApproveDate = oReader.GetDateTime("ApproveDate");
            oNOASignatory.IsApprove = oReader.GetBoolean("IsApprove");
            oNOASignatory.Name_Request = oReader.GetString("Name_Request");
            oNOASignatory.HeadName = oReader.GetString("HeadName");
            oNOASignatory.Note = oReader.GetString("Note");
			return oNOASignatory;
		}

		private NOASignatory CreateObject(NullHandler oReader)
		{
			NOASignatory oNOASignatory = new NOASignatory();
			oNOASignatory = MapObject(oReader);
			return oNOASignatory;
		}

		private List<NOASignatory> CreateObjects(IDataReader oReader)
		{
			List<NOASignatory> oNOASignatory = new List<NOASignatory>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				NOASignatory oItem = CreateObject(oHandler);
				oNOASignatory.Add(oItem);
			}
			return oNOASignatory;
		}

		#endregion

		#region Interface implementation
	
            public List<NOASignatory> SaveAll(List<NOASignatory> oNOASignatorys,string sNOASignatoryID, Int64 nUserID)
            {
                NOASignatory oNOASignatory = new NOASignatory();
                List<NOASignatory> oNOASignatorys_Return = new List<NOASignatory>();
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin(true);
                    IDataReader reader;
                    if (oNOASignatorys != null)
                    {
                        NOASignatoryDA.DeleteAll(tc, oNOASignatorys[0].NOAID, sNOASignatoryID);
                        foreach (NOASignatory oItem in oNOASignatorys)
                        {
                            if (oItem.NOASignatoryID <= 0)
                            {
                                reader = NOASignatoryDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                            }
                            else
                            {
                                reader = NOASignatoryDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                            }
                            NullHandler oReader = new NullHandler(reader);
                            if (reader.Read())
                            {
                                oNOASignatory = new NOASignatory();
                                oNOASignatory = CreateObject(oReader);
                                oNOASignatorys_Return.Add(oNOASignatory);
                            }
                            reader.Close();
                        }
                    }
                    tc.End();
                }
                catch (Exception e)
                {
                    #region Handle Exception
                    if (tc != null)
                        tc.HandleError();
                    oNOASignatorys_Return = new List<NOASignatory>();
                    oNOASignatory = new NOASignatory();
                    oNOASignatory.ErrorMessage = e.Message.Split('~')[0];
                    oNOASignatorys_Return.Add(oNOASignatory);

                    #endregion
                }
                return oNOASignatorys_Return;
            }
            public NOASignatory Save(NOASignatory oNOASignatory, Int64 nUserID)
            {
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin(true);
                    IDataReader reader;
                    if (oNOASignatory.NOASignatoryID <= 0)
                    {
                        reader = NOASignatoryDA.InsertUpdate(tc, oNOASignatory, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        reader = NOASignatoryDA.InsertUpdate(tc, oNOASignatory, EnumDBOperation.Update, nUserID);
                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oNOASignatory = new NOASignatory();
                        oNOASignatory = CreateObject(oReader);
                    }
                    reader.Close();
                    tc.End();
                }
                catch (Exception e)
                {
                    #region Handle Exception
                    if (tc != null)
                        tc.HandleError();
                    oNOASignatory = new NOASignatory();
                    oNOASignatory.ErrorMessage = e.Message.Split('~')[0];
                    #endregion
                }
                return oNOASignatory;
            }
            public string Delete(NOASignatory oNOASignatory, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "NOASignatory", EnumRoleOperationType.Delete);
                    //DBTableReferenceDA.HasReference(tc, "NOASignatory", id);
					NOASignatoryDA.Delete(tc, oNOASignatory, EnumDBOperation.Delete, nUserId);
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exceptionif (tc != null)
					tc.HandleError();
					return e.Message.Split('~')[0];
					#endregion
				}
				return Global.DeleteMessage;
			}
			public NOASignatory Get(int id, Int64 nUserId)
			{
				NOASignatory oNOASignatory = new NOASignatory();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = NOASignatoryDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oNOASignatory = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get NOASignatory", e);
					#endregion
				}
				return oNOASignatory;
			}
            public List<NOASignatory> Gets(int nNOAID, Int64 nUserID)
            {
                List<NOASignatory> oNOASignatorys = new List<NOASignatory>();
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin();
                    IDataReader reader = null;
                    reader = NOASignatoryDA.Gets(tc, nNOAID);
                    oNOASignatorys = CreateObjects(reader);
                    reader.Close();
                    tc.End();
                }
                catch (Exception e)
                {
                    #region Handle Exception
                    if (tc != null)
                        tc.HandleError();
                    NOASignatory oNOASignatory = new NOASignatory();
                    oNOASignatory.ErrorMessage = e.Message.Split('!')[0];
                    #endregion
                }
                return oNOASignatorys;
            }
            public List<NOASignatory> GetsByLog(int nNOAID, Int64 nUserID)
            {
                List<NOASignatory> oNOASignatorys = new List<NOASignatory>();
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin();
                    IDataReader reader = null;
                    reader = NOASignatoryDA.GetsByLog(tc, nNOAID);
                    oNOASignatorys = CreateObjects(reader);
                    reader.Close();
                    tc.End();
                }
                catch (Exception e)
                {
                    #region Handle Exception
                    if (tc != null)
                        tc.HandleError();
                    NOASignatory oNOASignatory = new NOASignatory();
                    oNOASignatory.ErrorMessage = e.Message.Split('!')[0];
                    #endregion
                }
                return oNOASignatorys;
            }
			public List<NOASignatory> Gets (string sSQL, Int64 nUserID)
			{
				List<NOASignatory> oNOASignatorys = new List<NOASignatory>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = NOASignatoryDA.Gets(tc, sSQL);
					oNOASignatorys = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get NOASignatory", e);
					#endregion
				}
				return oNOASignatorys;
			}

		#endregion
	}

}
