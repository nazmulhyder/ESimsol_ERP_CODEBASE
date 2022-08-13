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
    public class ExportBillParticularService : MarshalByRefObject, IExportBillParticularService
    {
        #region Private functions and declaration
        private ExportBillParticular MapObject(NullHandler oReader)
        {
            ExportBillParticular oExportBillParticular = new ExportBillParticular();
            oExportBillParticular.ExportBillParticularID = oReader.GetInt32("ExportBillParticularID");
            oExportBillParticular.Name = oReader.GetString("Name");
            oExportBillParticular.InOutType = (EnumInOutType)oReader.GetInt32("InOutType");
            oExportBillParticular.InOutTypeInInt = oReader.GetInt32("InOutType");
            oExportBillParticular.Activity = oReader.GetBoolean("Activity");
            return oExportBillParticular;
        }
        private ExportBillParticular CreateObject(NullHandler oReader)
        {
            ExportBillParticular oExportBillParticular = new ExportBillParticular();
            oExportBillParticular = MapObject(oReader);
            return oExportBillParticular;
        }
        private List<ExportBillParticular> CreateObjects(IDataReader oReader)
        {
            List<ExportBillParticular> oExportBillParticular = new List<ExportBillParticular>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportBillParticular oItem = CreateObject(oHandler);
                oExportBillParticular.Add(oItem);
            }
            return oExportBillParticular;
        }
        #endregion

        #region Interface implementation
        public ExportBillParticular Save(ExportBillParticular oExportBillParticular, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oExportBillParticular.ExportBillParticularID <= 0)
                {
                    reader = ExportBillParticularDA.InsertUpdate(tc, oExportBillParticular, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ExportBillParticularDA.InsertUpdate(tc, oExportBillParticular, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportBillParticular = new ExportBillParticular();
                    oExportBillParticular = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oExportBillParticular = new ExportBillParticular();
                oExportBillParticular.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oExportBillParticular;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ExportBillParticular oExportBillParticular = new ExportBillParticular();
                oExportBillParticular.ExportBillParticularID = id;
                ExportBillParticularDA.Delete(tc, oExportBillParticular, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return Global.DeleteMessage; 
        }
        public ExportBillParticular Get(int id, Int64 nUserId)
        {
            ExportBillParticular oAccountHead = new ExportBillParticular();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = ExportBillParticularDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get ExportBillParticular", e);
                #endregion
            }

            return oAccountHead;
        }
        public List<ExportBillParticular> Gets(Int64 nUserID)
        {
            List<ExportBillParticular> oExportBillParticular = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportBillParticularDA.Gets(tc);
                oExportBillParticular = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportBillParticular", e);
                #endregion
            }
            return oExportBillParticular;
        }
        public List<ExportBillParticular> Gets(bool bActivity,Int64 nUserID)
        {
            List<ExportBillParticular> oExportBillParticular = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportBillParticularDA.Gets(tc, bActivity);
                oExportBillParticular = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportBillParticular", e);
                #endregion
            }
            return oExportBillParticular;
        }
        public List<ExportBillParticular> Gets(string sSQL, Int64 nUserID)
        {
            List<ExportBillParticular> oExportBillParticular = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportBillParticularDA.Gets(tc, sSQL);
                oExportBillParticular = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportBillParticular", e);
                #endregion
            }
            return oExportBillParticular;
        }

        #endregion
    }
}
