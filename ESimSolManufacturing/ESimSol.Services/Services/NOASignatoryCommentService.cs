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
	public class NOASignatoryCommentService : MarshalByRefObject, INOASignatoryCommentService
	{
		#region Private functions and declaration

		private NOASignatoryComment MapObject(NullHandler oReader)
		{
			NOASignatoryComment oNOASignatoryComment = new NOASignatoryComment();
            oNOASignatoryComment.NOASignatoryCommentID = oReader.GetInt32("NOASignatoryCommentID");
            oNOASignatoryComment.NOADetailID = oReader.GetInt32("NOADetailID");
            oNOASignatoryComment.PQDetailID = oReader.GetInt32("PQDetailID");
            oNOASignatoryComment.Comment = oReader.GetString("Comment");
            oNOASignatoryComment.SupplierName = oReader.GetString("SupplierName");
            oNOASignatoryComment.Name = oReader.GetString("Name");
            oNOASignatoryComment.UnitPrice = oReader.GetDouble("UnitPrice");
            oNOASignatoryComment.DBServerDateTime = oReader.GetDateTime("DBServerDateTime");
            oNOASignatoryComment.NOASignatoryID = oReader.GetInt32("NOASignatoryID");
          
			return oNOASignatoryComment;
		}

		private NOASignatoryComment CreateObject(NullHandler oReader)
		{
			NOASignatoryComment oNOASignatoryComment = new NOASignatoryComment();
			oNOASignatoryComment = MapObject(oReader);
			return oNOASignatoryComment;
		}

		private List<NOASignatoryComment> CreateObjects(IDataReader oReader)
		{
			List<NOASignatoryComment> oNOASignatoryComment = new List<NOASignatoryComment>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				NOASignatoryComment oItem = CreateObject(oHandler);
				oNOASignatoryComment.Add(oItem);
			}
			return oNOASignatoryComment;
		}

		#endregion

		#region Interface implementation
			public NOASignatoryComment Save(NOASignatoryComment oNOASignatoryComment, Int64 nUserID)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oNOASignatoryComment.NOASignatoryCommentID <= 0)
					{
						//AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "NOASignatoryComment", EnumRoleOperationType.Add);
						reader = NOASignatoryCommentDA.InsertUpdate(tc, oNOASignatoryComment, EnumDBOperation.Insert, nUserID);
					}
					else{
						//AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "NOASignatoryComment", EnumRoleOperationType.Edit);
						reader = NOASignatoryCommentDA.InsertUpdate(tc, oNOASignatoryComment, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oNOASignatoryComment = new NOASignatoryComment();
						oNOASignatoryComment = CreateObject(oReader);
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
						oNOASignatoryComment = new NOASignatoryComment();
						oNOASignatoryComment.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oNOASignatoryComment;
			}
            public List<NOASignatoryComment> SaveAll(List<NOASignatoryComment> oNOASignatoryComments, Int64 nUserID)
            {

                NOASignatoryComment oNOASignatoryComment = new NOASignatoryComment();
                List<NOASignatoryComment> oNOASignatoryComments_Return = new List<NOASignatoryComment>();
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin(true);
                    IDataReader reader;
                    foreach (NOASignatoryComment oItem in oNOASignatoryComments)
                    {
                        if (oItem.NOASignatoryCommentID <= 0)
                        {

                            reader = NOASignatoryCommentDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {

                            reader = NOASignatoryCommentDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                        }
                        //IDataReader reader = NOASignatoryCommentDA.Get(tc, oItem.NOASignatoryCommentID);
                        NullHandler oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oNOASignatoryComment = new NOASignatoryComment();
                            oNOASignatoryComment = CreateObject(oReader);
                            oNOASignatoryComments_Return.Add(oNOASignatoryComment);
                        }
                        reader.Close();
                    }

                    tc.End();
                }
                catch (Exception e)
                {
                    #region Handle Exception
                    if (tc != null)
                        tc.HandleError();
                    oNOASignatoryComments_Return = new List<NOASignatoryComment>();
                    oNOASignatoryComment = new NOASignatoryComment();
                    oNOASignatoryComment.ErrorMessage = e.Message.Split('~')[0];
                    oNOASignatoryComments_Return.Add(oNOASignatoryComment);

                    #endregion
                }
                return oNOASignatoryComments_Return;
            }
			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					NOASignatoryComment oNOASignatoryComment = new NOASignatoryComment();
					oNOASignatoryComment.NOASignatoryCommentID = id;
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "NOASignatoryComment", EnumRoleOperationType.Delete);
					DBTableReferenceDA.HasReference(tc, "NOASignatoryComment", id);
					NOASignatoryCommentDA.Delete(tc, oNOASignatoryComment, EnumDBOperation.Delete, nUserId);
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

			public NOASignatoryComment Get(int id, Int64 nUserId)
			{
				NOASignatoryComment oNOASignatoryComment = new NOASignatoryComment();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = NOASignatoryCommentDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oNOASignatoryComment = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get NOASignatoryComment", e);
					#endregion
				}
				return oNOASignatoryComment;
			}

			public List<NOASignatoryComment> Gets( int nNOADetailID,Int64 nUserID)
			{
				List<NOASignatoryComment> oNOASignatoryComments = new List<NOASignatoryComment>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
                    reader = NOASignatoryCommentDA.Gets(tc, nNOADetailID);
					oNOASignatoryComments = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					NOASignatoryComment oNOASignatoryComment = new NOASignatoryComment();
					oNOASignatoryComment.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oNOASignatoryComments;
			}

			public List<NOASignatoryComment> Gets (string sSQL, Int64 nUserID)
			{
				List<NOASignatoryComment> oNOASignatoryComments = new List<NOASignatoryComment>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = NOASignatoryCommentDA.Gets(tc, sSQL);
					oNOASignatoryComments = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get NOASignatoryComment", e);
					#endregion
				}
				return oNOASignatoryComments;
			}

		#endregion
	}

}
