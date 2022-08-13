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
    public class DispoProductionCommentService : MarshalByRefObject, IDispoProductionCommentService
    {
        #region Private functions and declaration

        private DispoProductionComment MapObject(NullHandler oReader)
        {
            DispoProductionComment oDispoProductionComment = new DispoProductionComment();
            oDispoProductionComment.DispoProductionCommentID = oReader.GetInt32("DispoProductionCommentID");
            oDispoProductionComment.FSCDID = oReader.GetInt32("FSCDID");
            oDispoProductionComment.CommentDate = oReader.GetDateTime("CommentDate");
            oDispoProductionComment.UserID = oReader.GetInt32("UserID");
            oDispoProductionComment.Comment = oReader.GetString("Comment");
            oDispoProductionComment.UserName = oReader.GetString("UserName");
            oDispoProductionComment.IsOwn = oReader.GetBoolean("IsOwn");
            oDispoProductionComment.ExeNo = oReader.GetString("ExeNo");

            return oDispoProductionComment;
        }

        private DispoProductionComment CreateObject(NullHandler oReader)
        {
            DispoProductionComment oDispoProductionComment = new DispoProductionComment();
            oDispoProductionComment = MapObject(oReader);
            return oDispoProductionComment;
        }

        private List<DispoProductionComment> CreateObjects(IDataReader oReader)
        {
            List<DispoProductionComment> oDispoProductionComment = new List<DispoProductionComment>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DispoProductionComment oItem = CreateObject(oHandler);
                oDispoProductionComment.Add(oItem);
            }
            return oDispoProductionComment;
        }

        #endregion

        #region Interface implementation
        public DispoProductionComment Save(DispoProductionComment oDispoProductionComment, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                #region DispoProductionComment
                IDataReader reader;
                if (oDispoProductionComment.DispoProductionCommentID <= 0)
                {
                    reader = DispoProductionCommentDA.InsertUpdate(tc, oDispoProductionComment, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = DispoProductionCommentDA.InsertUpdate(tc, oDispoProductionComment, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDispoProductionComment = new DispoProductionComment();
                    oDispoProductionComment = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oDispoProductionComment = new DispoProductionComment();
                    oDispoProductionComment.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oDispoProductionComment;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                DispoProductionComment oDispoProductionComment = new DispoProductionComment();
                oDispoProductionComment.DispoProductionCommentID = id;
                DispoProductionCommentDA.Delete(tc, oDispoProductionComment, EnumDBOperation.Delete, nUserId);
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

        public DispoProductionComment Get(int id, Int64 nUserId)
        {
            DispoProductionComment oDispoProductionComment = new DispoProductionComment();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = DispoProductionCommentDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDispoProductionComment = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get DispoProductionComment", e);
                #endregion
            }
            return oDispoProductionComment;
        }

        public List<DispoProductionComment> Gets(Int64 nUserID)
        {
            List<DispoProductionComment> oDispoProductionComments = new List<DispoProductionComment>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = DispoProductionCommentDA.Gets(tc);
                oDispoProductionComments = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                DispoProductionComment oDispoProductionComment = new DispoProductionComment();
                oDispoProductionComment.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oDispoProductionComments;
        }

        public List<DispoProductionComment> Gets(string sSQL, Int64 nUserID)
        {
            List<DispoProductionComment> oDispoProductionComments = new List<DispoProductionComment>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = DispoProductionCommentDA.Gets(tc, sSQL);
                oDispoProductionComments = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) 
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DispoProductionComment", e);
                #endregion
            }
            return oDispoProductionComments;
        }

        public List<DispoProductionComment> GetsBySP(int nFSCDID, Int64 nUserID)
        {
            List<DispoProductionComment> oDispoProductionComments = new List<DispoProductionComment>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = DispoProductionCommentDA.GetsBySP(tc, nFSCDID, nUserID);
                oDispoProductionComments = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DispoProductionComment", e);
                #endregion
            }
            return oDispoProductionComments;
        }

        #endregion
    }

}
