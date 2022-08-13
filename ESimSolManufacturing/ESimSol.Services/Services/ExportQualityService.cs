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
    public class ExportQualityService : MarshalByRefObject, IExportQualityService
    {
        #region Private functions and declaration
        private ExportQuality MapObject(NullHandler oReader)
        {
            ExportQuality oExportQuality = new ExportQuality();
            oExportQuality.ExportQualityID = oReader.GetInt32("ExportQualityID");
            oExportQuality.Name = oReader.GetString("Name");
            oExportQuality.Activity = oReader.GetBoolean("Activity");
            return oExportQuality;
        }
        private ExportQuality CreateObject(NullHandler oReader)
        {
            ExportQuality oExportQuality = new ExportQuality();
            oExportQuality = MapObject(oReader);
            return oExportQuality;
        }
        private List<ExportQuality> CreateObjects(IDataReader oReader)
        {
            List<ExportQuality> oExportQuality = new List<ExportQuality>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportQuality oItem = CreateObject(oHandler);
                oExportQuality.Add(oItem);
            }
            return oExportQuality;
        }

        #endregion

        #region Interface implementation
        public ExportQuality Save(ExportQuality oExportQuality, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oExportQuality.ExportQualityID <= 0)
                {
                    reader = ExportQualityDA.InsertUpdate(tc, oExportQuality, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ExportQualityDA.InsertUpdate(tc, oExportQuality, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportQuality = new ExportQuality();
                    oExportQuality = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oExportQuality.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oExportQuality;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ExportQuality oExportQuality = new ExportQuality();
                oExportQuality.ExportQualityID = id;
                //DBTableReferenceDA.HasReference(tc, "ExportQuality", id);
                ExportQualityDA.Delete(tc, oExportQuality, EnumDBOperation.Delete, nUserId);
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
        public List<ExportQuality> Gets(Int64 nUserID)
        {
            List<ExportQuality> oExportQualitys = new List<ExportQuality>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportQualityDA.Gets(tc);
                oExportQualitys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oExportQualitys = new List<ExportQuality>();
                ExportQuality oExportQuality = new ExportQuality();
                oExportQuality.ErrorMessage = e.Message.Split('~')[0];
                oExportQualitys.Add(oExportQuality);
                #endregion
            }
            return oExportQualitys;
        }
        public ExportQuality Get(int id, Int64 nUserId)
        {
            ExportQuality oExportQuality = new ExportQuality();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = ExportQualityDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportQuality = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oExportQuality.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oExportQuality;
        }
        public List<ExportQuality> Gets(bool bActivity, Int64 nUserID)
        {
            List<ExportQuality> oExportQualitys = new List<ExportQuality>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportQualityDA.GetsActivity(tc, bActivity);
                oExportQualitys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oExportQualitys = new List<ExportQuality>();
                ExportQuality oExportQuality = new ExportQuality();
                oExportQuality.ErrorMessage = e.Message.Split('~')[0];
                oExportQualitys.Add(oExportQuality);
                #endregion
            }
            return oExportQualitys;
        }
        #endregion
    }
}
