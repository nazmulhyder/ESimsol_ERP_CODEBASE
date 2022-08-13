using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
 

namespace ESimSol.Services.Services
{
    public class BarCodeCommentService : MarshalByRefObject, IBarCodeCommentService
    {
        #region Private functions and declaration
        private BarCodeComment MapObject(NullHandler oReader)
        {
            BarCodeComment oBarCodeComment = new BarCodeComment();
            oBarCodeComment.BarCodeCommentID = oReader.GetInt32("BarCodeCommentID");
            oBarCodeComment.BarCodeCommentLogID = oReader.GetInt32("BarCodeCommentLogID");   
            oBarCodeComment.OrderRecapID = oReader.GetInt32("OrderRecapID");
            oBarCodeComment.Comments = oReader.GetString("Comments");
            oBarCodeComment.OrderRecapLogID = oReader.GetInt32("OrderRecapLogID");
            return oBarCodeComment;
        }

        private BarCodeComment CreateObject(NullHandler oReader)
        {
            BarCodeComment oBarCodeComment = new BarCodeComment();
            oBarCodeComment = MapObject(oReader);
            return oBarCodeComment;
        }

        private List<BarCodeComment> CreateObjects(IDataReader oReader)
        {
            List<BarCodeComment> oBarCodeComment = new List<BarCodeComment>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                BarCodeComment oItem = CreateObject(oHandler);
                oBarCodeComment.Add(oItem);
            }
            return oBarCodeComment;
        }

        #endregion

        #region Interface implementation
        public BarCodeCommentService() { }

        public List<BarCodeComment> Save(OrderRecap oOrderRecap, Int64 nUserID)
        {
            BarCodeComment oBarCodeComment = new BarCodeComment();
            List<BarCodeComment> oBarCodeComments = new List<BarCodeComment>();
            string sBarCodeCommentIDs = "";
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                
                foreach (BarCodeComment oItem in oOrderRecap.BarCodeComments)
                {
                    oBarCodeComment = new BarCodeComment();
                    IDataReader reader;
                    if (oItem.BarCodeCommentID <= 0)
                    {
                        reader = BarCodeCommentDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        reader = BarCodeCommentDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oBarCodeComment = new BarCodeComment();
                        oBarCodeComment = CreateObject(oReader);
                        oBarCodeComments.Add(oBarCodeComment);
                        sBarCodeCommentIDs = sBarCodeCommentIDs + oBarCodeComment.BarCodeCommentID.ToString() + ",";
                    }
                    reader.Close();
                }
                if (sBarCodeCommentIDs.Length > 0)
                {
                    sBarCodeCommentIDs = sBarCodeCommentIDs.Remove(sBarCodeCommentIDs.Length - 1, 1);
                }
                oBarCodeComment = new BarCodeComment();
                oBarCodeComment.OrderRecapID = oOrderRecap.OrderRecapID;
                BarCodeCommentDA.Delete(tc, oBarCodeComment, EnumDBOperation.Delete, sBarCodeCommentIDs, nUserID);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oBarCodeComments = new List<BarCodeComment>();
                oBarCodeComment = new BarCodeComment();
                oBarCodeComment.ErrorMessage = e.Message.Split('~')[0];
                oBarCodeComments.Add(oBarCodeComment);
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save BarCodeComment. Because of " + e.Message, e);
                #endregion
            }
            return oBarCodeComments;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                BarCodeComment oBarCodeComment = new BarCodeComment();
                oBarCodeComment.BarCodeCommentID = id;
                BarCodeCommentDA.Delete(tc, oBarCodeComment, EnumDBOperation.Delete, "", nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete BarCodeComment. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public BarCodeComment Get(int id, Int64 nUserId)
        {
            BarCodeComment oAccountHead = new BarCodeComment();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = BarCodeCommentDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get BarCodeComment", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<BarCodeComment> Gets(Int64 nUserID)
        {
            List<BarCodeComment> oBarCodeComment = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BarCodeCommentDA.Gets(tc);
                oBarCodeComment = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BarCodeComment", e);
                #endregion
            }

            return oBarCodeComment;
        }

        public List<BarCodeComment> Gets(int nOrderRecapID, Int64 nUserID)
        {
            List<BarCodeComment> oBarCodeComment = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BarCodeCommentDA.Gets(tc, nOrderRecapID);
                oBarCodeComment = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BarCodeComment", e);
                #endregion
            }

            return oBarCodeComment;
        }
        public List<BarCodeComment> GetsForLog(int nOrderRecapLogID, Int64 nUserID)
        {
            List<BarCodeComment> oBarCodeComment = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BarCodeCommentDA.GetsForLog(tc, nOrderRecapLogID);
                oBarCodeComment = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BarCodeComment", e);
                #endregion
            }

            return oBarCodeComment;
        }

        
        #endregion
    }
}
