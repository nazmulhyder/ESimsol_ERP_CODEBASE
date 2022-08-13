using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class ReportCommentsService : MarshalByRefObject, IReportCommentsService
    {
        #region Private functions and declaration
        private ReportComments MapObject(NullHandler oReader)
        {
            ReportComments oReportComments = new ReportComments();
            oReportComments.RCID = oReader.GetInt32("RCID");
            oReportComments.CommentDate = oReader.GetDateTime("CommentDate");
            oReportComments.Note = oReader.GetString("Note");
            return oReportComments;
        }

        private ReportComments CreateObject(NullHandler oReader)
        {
            ReportComments oReportComments = new ReportComments();
            oReportComments = MapObject(oReader);
            return oReportComments;
        }

        private List<ReportComments> CreateObjects(IDataReader oReader)
        {
            List<ReportComments> oReportComments = new List<ReportComments>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ReportComments oItem = CreateObject(oHandler);
                oReportComments.Add(oItem);
            }
            return oReportComments;
        }

        #endregion

        #region Interface implementation
        public ReportCommentsService() { }

        public ReportComments IUD(ReportComments oReportComments, int nDBOperation, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update)
                {
                    IDataReader reader;

                    if (oReportComments.RCID <= 0)
                    {
                        reader = ReportCommentsDA.InsertUpdate(tc, oReportComments, EnumDBOperation.Insert, nUserId);
                    }
                    else
                    {
                        reader = ReportCommentsDA.InsertUpdate(tc, oReportComments, EnumDBOperation.Update, nUserId);
                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oReportComments = new ReportComments();
                        oReportComments = CreateObject(oReader);
                    }
                    reader.Close();
                }
                else if(nDBOperation == (int)EnumDBOperation.Delete)
                {
                    ReportCommentsDA.Delete(tc, oReportComments, EnumDBOperation.Delete, nUserId);
                    oReportComments = new ReportComments();
                    oReportComments.ErrorMessage = "Delete Successfully.";
                }

                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oReportComments = new ReportComments();
                if (ex.Message.Contains("!")) { oReportComments.ErrorMessage = ex.Message.Split('!')[0]; }
                else { oReportComments.ErrorMessage = ex.Message; }               
                #endregion
            }
            return oReportComments;
        }
        public ReportComments Get(int nRCID, Int64 nUserId)
        {
            ReportComments oAccountHead = new ReportComments();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ReportCommentsDA.Get(tc, nRCID);
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
                throw new ServiceException("Failed to Get ReportComments", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<ReportComments> Gets(string sSQL,Int64 nUserId)
        {
            List<ReportComments> oReportComments = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ReportCommentsDA.Gets(tc, sSQL);
                oReportComments = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ReportComments", e);
                #endregion
            }

            return oReportComments;
        }

        #endregion
    }
}
