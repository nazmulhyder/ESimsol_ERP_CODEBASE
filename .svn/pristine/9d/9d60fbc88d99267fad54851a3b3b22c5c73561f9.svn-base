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
    public class ExportTnCCaptionService : MarshalByRefObject, IExportTnCCaptionService
    {
        #region Private functions and declaration
        private ExportTnCCaption MapObject(NullHandler oReader)
        {
            ExportTnCCaption oExportTnCCaption = new ExportTnCCaption();
            oExportTnCCaption.ExportTnCCaptionID = oReader.GetInt32("ExportTnCCaptionID");
            oExportTnCCaption.Name = oReader.GetString("Name");
            oExportTnCCaption.Activity = oReader.GetBoolean("Activity");
            oExportTnCCaption.Sequence = oReader.GetInt32("Sequence");
            return oExportTnCCaption;
        }
        private ExportTnCCaption CreateObject(NullHandler oReader)
        {
            ExportTnCCaption oExportTnCCaption = new ExportTnCCaption();
            oExportTnCCaption = MapObject(oReader);
            return oExportTnCCaption;
        }
        private List<ExportTnCCaption> CreateObjects(IDataReader oReader)
        {
            List<ExportTnCCaption> oExportTnCCaption = new List<ExportTnCCaption>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportTnCCaption oItem = CreateObject(oHandler);
                oExportTnCCaption.Add(oItem);
            }
            return oExportTnCCaption;
        }

        #endregion

        #region Interface implementation
        public ExportTnCCaption Save(ExportTnCCaption oExportTnCCaption, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oExportTnCCaption.ExportTnCCaptionID <= 0)
                {
                    reader = ExportTnCCaptionDA.InsertUpdate(tc, oExportTnCCaption, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ExportTnCCaptionDA.InsertUpdate(tc, oExportTnCCaption, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportTnCCaption = new ExportTnCCaption();
                    oExportTnCCaption = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oExportTnCCaption.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oExportTnCCaption;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ExportTnCCaption oExportTnCCaption = new ExportTnCCaption();
                oExportTnCCaption.ExportTnCCaptionID = id;
                //DBTableReferenceDA.HasReference(tc, "ExportTnCCaption", id);
                ExportTnCCaptionDA.Delete(tc, oExportTnCCaption, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }
        public List<ExportTnCCaption> Gets(Int64 nUserID)
        {
            List<ExportTnCCaption> oExportTnCCaptions = new List<ExportTnCCaption>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportTnCCaptionDA.Gets(tc);
                oExportTnCCaptions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oExportTnCCaptions = new List<ExportTnCCaption>();
                ExportTnCCaption oExportTnCCaption = new ExportTnCCaption();
                oExportTnCCaption.ErrorMessage = e.Message.Split('~')[0];
                oExportTnCCaptions.Add(oExportTnCCaption);
                #endregion
            }
            return oExportTnCCaptions;
        }
        public ExportTnCCaption Get(int id, Int64 nUserId)
        {
            ExportTnCCaption oExportTnCCaption = new ExportTnCCaption();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = ExportTnCCaptionDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportTnCCaption = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oExportTnCCaption.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oExportTnCCaption;
        }
        public List<ExportTnCCaption> Gets(bool bActivity, Int64 nUserID)
        {
            List<ExportTnCCaption> oExportTnCCaptions = new List<ExportTnCCaption>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportTnCCaptionDA.GetsActivity(tc, bActivity);
                oExportTnCCaptions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oExportTnCCaptions = new List<ExportTnCCaption>();
                ExportTnCCaption oExportTnCCaption = new ExportTnCCaption();
                oExportTnCCaption.ErrorMessage = e.Message.Split('~')[0];
                oExportTnCCaptions.Add(oExportTnCCaption);
                #endregion
            }
            return oExportTnCCaptions;
        }
        #endregion
    }
}
