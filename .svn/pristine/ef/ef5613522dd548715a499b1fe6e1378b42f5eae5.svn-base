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
    public class ImageCommentService : MarshalByRefObject, IImageCommentService
    {
        #region Private functions and declaration
        private ImageComment MapObject(NullHandler oReader)
        {
            ImageComment oImageComment = new ImageComment();
            oImageComment.ImageCommentID = oReader.GetInt32("ImageCommentID");
            oImageComment.TechnicalSheetID = oReader.GetInt32("TechnicalSheetID");
            oImageComment.Comments = oReader.GetString("Comments");            
            return oImageComment;
        }

        private ImageComment CreateObject(NullHandler oReader)
        {
            ImageComment oImageComment = new ImageComment();
            oImageComment = MapObject(oReader);
            return oImageComment;
        }

        private List<ImageComment> CreateObjects(IDataReader oReader)
        {
            List<ImageComment> oImageComment = new List<ImageComment>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImageComment oItem = CreateObject(oHandler);
                oImageComment.Add(oItem);
            }
            return oImageComment;
        }

        #endregion

        #region Interface implementation
        public ImageCommentService() { }

        public List<ImageComment> Save(TechnicalSheet oTechnicalSheet, Int64 nUserID)
        {
            ImageComment oImageComment = new ImageComment();
            List<ImageComment> oImageComments = new List<ImageComment>();
            string sImageCommentIDs = "";
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                
                foreach (ImageComment oItem in oTechnicalSheet.ImageComments)
                {
                    oImageComment = new ImageComment();
                    IDataReader reader;
                    if (oItem.ImageCommentID <= 0)
                    {
                        reader = ImageCommentDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        reader = ImageCommentDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oImageComment = new ImageComment();
                        oImageComment = CreateObject(oReader);
                        oImageComments.Add(oImageComment);
                        sImageCommentIDs = sImageCommentIDs + oImageComment.ImageCommentID.ToString() + ",";
                    }
                    reader.Close();
                }
                if (sImageCommentIDs.Length > 0)
                {
                    sImageCommentIDs = sImageCommentIDs.Remove(sImageCommentIDs.Length - 1, 1);
                }
                oImageComment = new ImageComment();
                oImageComment.TechnicalSheetID = oTechnicalSheet.TechnicalSheetID;
                ImageCommentDA.Delete(tc, oImageComment, EnumDBOperation.Delete, sImageCommentIDs, nUserID);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oImageComments = new List<ImageComment>();
                oImageComment = new ImageComment();
                oImageComment.ErrorMessage = e.Message.Split('~')[0];
                oImageComments.Add(oImageComment);
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save ImageComment. Because of " + e.Message, e);
                #endregion
            }
            return oImageComments;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ImageComment oImageComment = new ImageComment();
                oImageComment.ImageCommentID = id;
                ImageCommentDA.Delete(tc, oImageComment, EnumDBOperation.Delete, "", nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete ImageComment. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public ImageComment Get(int id, Int64 nUserId)
        {
            ImageComment oAccountHead = new ImageComment();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImageCommentDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get ImageComment", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<ImageComment> Gets(Int64 nUserID)
        {
            List<ImageComment> oImageComment = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImageCommentDA.Gets(tc);
                oImageComment = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImageComment", e);
                #endregion
            }

            return oImageComment;
        }

        public List<ImageComment> Gets(int nTechnicalSheetID, Int64 nUserID)
        {
            List<ImageComment> oImageComment = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImageCommentDA.Gets(tc, nTechnicalSheetID);
                oImageComment = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImageComment", e);
                #endregion
            }

            return oImageComment;
        }
        #endregion
    }
}
