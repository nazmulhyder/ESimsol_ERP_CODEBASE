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
    public class ExportDocForwardingService : MarshalByRefObject, IExportDocForwardingService
    {
        #region Private functions and declaration
        private ExportDocForwarding MapObject(NullHandler oReader)
        {
            ExportDocForwarding oExportDocForwarding = new ExportDocForwarding();
            oExportDocForwarding.ExportDocForwardingID = oReader.GetInt32("ExportDocForwardingID");
            oExportDocForwarding.ReferenceID = oReader.GetInt32("ReferenceID");
            oExportDocForwarding.RefType = (EnumMasterLCType)oReader.GetInt32("RefType");
            oExportDocForwarding.RefTypeInInt = oReader.GetInt32("RefType");
            oExportDocForwarding.ExportDocID = oReader.GetInt32("ExportDocID");
            oExportDocForwarding.Copies = oReader.GetInt32("Copies");
            oExportDocForwarding.Name_Doc = oReader.GetString("DocName");
            oExportDocForwarding.Name_Print = oReader.GetString("Name_Print");
            oExportDocForwarding.DocumentType = (EnumDocumentType)oReader.GetInt32("DocumentType");
            return oExportDocForwarding;
        }
        private ExportDocForwarding CreateObject(NullHandler oReader)
        {
            ExportDocForwarding oExportDocForwarding = new ExportDocForwarding();
            oExportDocForwarding = MapObject(oReader);
            return oExportDocForwarding;
        }
        private List<ExportDocForwarding> CreateObjects(IDataReader oReader)
        {
            List<ExportDocForwarding> oExportDocForwarding = new List<ExportDocForwarding>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportDocForwarding oItem = CreateObject(oHandler);
                oExportDocForwarding.Add(oItem);
            }
            return oExportDocForwarding;
        }
        #endregion

        #region Interface implementation
        public ExportDocForwarding Save(ExportDocForwarding oExportDocForwarding, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                //IDataReader reader;

                foreach (ExportDocForwarding Oitem in oExportDocForwarding.ExportDocForwardings)
                {

                    if (Oitem.ExportDocForwardingID <= 0)
                    {
                         ExportDocForwardingDA.InsertUpdate(tc, Oitem, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        ExportDocForwardingDA.InsertUpdate(tc, Oitem, EnumDBOperation.Update, nUserID);
                    }
                }
                // reader = ExportDocForwardingDA.Get(tc, 1);
                //NullHandler oReader = new NullHandler(reader);
                //if (reader.Read())
                //{
                //    oExportDocForwarding = new ExportDocForwarding();
                //    oExportDocForwarding = CreateObject(oReader);
                //}
               // reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oExportDocForwarding = new ExportDocForwarding();
                oExportDocForwarding.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oExportDocForwarding;
        }
        public string Delete(int nExportBillID, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ExportDocForwarding oExportDocForwarding = new ExportDocForwarding();
                oExportDocForwarding.ExportDocForwardingID = nExportBillID;
                oExportDocForwarding.ReferenceID = nExportBillID;
                //ExportDocForwardingDA.Delete(tc, oExportDocForwarding, EnumDBOperation.Delete, nUserId);
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
        public ExportDocForwarding DeleteBYExportBillID(int nExportBillID, Int64 nUserId)
        {
            ExportDocForwarding oExportDocForwarding =new ExportDocForwarding();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                
                oExportDocForwarding.ReferenceID = nExportBillID;
                ExportDocForwardingDA.DeleteBYExportBillID(tc, oExportDocForwarding);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oExportDocForwarding = new ExportDocForwarding();
                oExportDocForwarding.ErrorMessage= e.Message.Split('!')[0];
                #endregion
            }
            return oExportDocForwarding;
        }
        public ExportDocForwarding Get(int id, Int64 nUserId)
        {
            ExportDocForwarding oAccountHead = new ExportDocForwarding();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = ExportDocForwardingDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get ExportDocForwarding", e);
                #endregion
            }

            return oAccountHead;
        }
        
        public List<ExportDocForwarding> Gets(int nReferenceID, int nRefType, Int64 nUserID)
        {
            List<ExportDocForwarding> oExportDocForwarding = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportDocForwardingDA.Gets(tc, nReferenceID, nRefType);
                oExportDocForwarding = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportDocForwarding", e);
                #endregion
            }
            return oExportDocForwarding;
        }
   

        #endregion
    }
}
