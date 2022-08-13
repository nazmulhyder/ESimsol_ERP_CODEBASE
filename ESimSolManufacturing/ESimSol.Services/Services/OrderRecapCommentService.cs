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
	public class OrderRecapCommentService : MarshalByRefObject, IOrderRecapCommentService
	{
		#region Private functions and declaration

		private OrderRecapComment MapObject(NullHandler oReader)
		{
			OrderRecapComment oOrderRecapComment = new OrderRecapComment();
			oOrderRecapComment.OrderRecapCommentID = oReader.GetInt32("OrderRecapCommentID");
			oOrderRecapComment.OrderRecapID = oReader.GetInt32("OrderRecapID");
			oOrderRecapComment.CommentsBy = oReader.GetString("CommentsBy");
			oOrderRecapComment.CommentsText = oReader.GetString("CommentsText");
			return oOrderRecapComment;
		}

		private OrderRecapComment CreateObject(NullHandler oReader)
		{
			OrderRecapComment oOrderRecapComment = new OrderRecapComment();
			oOrderRecapComment = MapObject(oReader);
			return oOrderRecapComment;
		}

		private List<OrderRecapComment> CreateObjects(IDataReader oReader)
		{
			List<OrderRecapComment> oOrderRecapComment = new List<OrderRecapComment>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				OrderRecapComment oItem = CreateObject(oHandler);
				oOrderRecapComment.Add(oItem);
			}
			return oOrderRecapComment;
		}

		#endregion

		#region Interface implementation
			public OrderRecapComment Save(OrderRecapComment oOrderRecapComment, Int64 nUserID)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oOrderRecapComment.OrderRecapCommentID <= 0)
					{
                        AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.OrderRecapComment, EnumRoleOperationType.Add);
						reader = OrderRecapCommentDA.InsertUpdate(tc, oOrderRecapComment, EnumDBOperation.Insert, nUserID);
					}
					else{
                        AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.OrderRecapComment, EnumRoleOperationType.Edit);
						reader = OrderRecapCommentDA.InsertUpdate(tc, oOrderRecapComment, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oOrderRecapComment = new OrderRecapComment();
						oOrderRecapComment = CreateObject(oReader);
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
						oOrderRecapComment = new OrderRecapComment();
						oOrderRecapComment.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oOrderRecapComment;
			}

			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					OrderRecapComment oOrderRecapComment = new OrderRecapComment();
					oOrderRecapComment.OrderRecapCommentID = id;
					AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.OrderRecapComment, EnumRoleOperationType.Delete);
					DBTableReferenceDA.HasReference(tc, "OrderRecapComment", id);
					OrderRecapCommentDA.Delete(tc, oOrderRecapComment, EnumDBOperation.Delete, nUserId);
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

			public OrderRecapComment Get(int id, Int64 nUserId)
			{
				OrderRecapComment oOrderRecapComment = new OrderRecapComment();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = OrderRecapCommentDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oOrderRecapComment = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get OrderRecapComment", e);
					#endregion
				}
				return oOrderRecapComment;
			}

            public List<OrderRecapComment> Gets(int id, Int64 nUserID)
            {
                List<OrderRecapComment> oOrderRecapComments = new List<OrderRecapComment>();
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin();
                    IDataReader reader = null;
                    reader = OrderRecapCommentDA.Gets(tc,id);
                    oOrderRecapComments = CreateObjects(reader);
                    reader.Close();
                    tc.End();
                }
                catch (Exception e)
                {
                    #region Handle Exception
                    if (tc != null)
                        tc.HandleError();
                    OrderRecapComment oOrderRecapComment = new OrderRecapComment();
                    oOrderRecapComment.ErrorMessage = e.Message.Split('!')[0];
                    oOrderRecapComments.Add(oOrderRecapComment);
                    #endregion
                }
                return oOrderRecapComments;
            }

			public List<OrderRecapComment> Gets(Int64 nUserID)
			{
				List<OrderRecapComment> oOrderRecapComments = new List<OrderRecapComment>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = OrderRecapCommentDA.Gets(tc);
					oOrderRecapComments = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
                    OrderRecapComment oOrderRecapComment = new OrderRecapComment();
					oOrderRecapComment.ErrorMessage =  e.Message.Split('!')[0];
                    oOrderRecapComments.Add(oOrderRecapComment);
					#endregion
				}
				return oOrderRecapComments;
			}

			public List<OrderRecapComment> Gets (string sSQL, Int64 nUserID)
			{
				List<OrderRecapComment> oOrderRecapComment = new List<OrderRecapComment>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = OrderRecapCommentDA.Gets(tc, sSQL);
					oOrderRecapComment = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get OrderRecapComment", e);
					#endregion
				}
				return oOrderRecapComment;
			}

		#endregion
	}

}
