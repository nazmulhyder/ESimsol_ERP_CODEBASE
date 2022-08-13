using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class NoticeService : MarshalByRefObject, INoticeService
    {
        #region Private functions and declaration
        private Notice MapObject(NullHandler oReader)
        {
            Notice oNotice = new Notice();
            oNotice.NoticeID = oReader.GetInt32("NoticeID");
            oNotice.NoticeNo = oReader.GetString("NoticeNo");
            oNotice.Title = oReader.GetString("Title");
            oNotice.Description = oReader.GetString("Description");
            oNotice.IssueDate = oReader.GetDateTime("IssueDate");
            oNotice.ExpireDate = oReader.GetDateTime("ExpireDate");
            oNotice.IsActive = oReader.GetBoolean("IsActive");
            oNotice.PostedBy = oReader.GetString("PostedBy");
            return oNotice;
        }

        private Notice CreateObject(NullHandler oReader)
        {
            Notice oNotice = MapObject(oReader);
            return oNotice;
        }

        private List<Notice> CreateObjects(IDataReader oReader)
        {
            List<Notice> oNotice = new List<Notice>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                Notice oItem = CreateObject(oHandler);
                oNotice.Add(oItem);
            }
            return oNotice;
        }

        #endregion

        #region Interface implementation
        public NoticeService() { }

        public Notice IUD(Notice oNotice, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);
               
                if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update || nDBOperation == (int)EnumDBOperation.Approval)
                {
                    reader = NoticeDA.IUD(tc, oNotice, nDBOperation, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oNotice = new Notice();
                        oNotice = CreateObject(oReader);
                    }
                    reader.Close();
                }
                else if (nDBOperation == (int)EnumDBOperation.Delete )
                {
                    reader = NoticeDA.IUD(tc, oNotice, nDBOperation, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    reader.Close();
                    oNotice.ErrorMessage = Global.DeleteMessage;
                }
                
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oNotice = new Notice();
                oNotice.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }
            return oNotice;
        }


        public Notice Get(int nNoticeID, Int64 nUserId)
        {
            Notice oNotice = new Notice();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = NoticeDA.Get(tc, nNoticeID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oNotice = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oNotice = new Notice();
                oNotice.ErrorMessage = ex.Message;
                #endregion
            }

            return oNotice;
        }

        public List<Notice> Gets(string sSQL, Int64 nUserID)
        {
            List<Notice> oNotices = new List<Notice>();
            Notice oNotice = new Notice();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = NoticeDA.Gets(tc, sSQL);
                oNotices = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oNotice.ErrorMessage = ex.Message;
                oNotices.Add(oNotice);
                #endregion
            }

            return oNotices;
        }

        #endregion
    }
}
