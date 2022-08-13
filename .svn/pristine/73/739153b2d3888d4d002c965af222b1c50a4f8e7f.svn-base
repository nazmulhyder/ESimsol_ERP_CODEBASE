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
    public class ExportBankForwardingService : MarshalByRefObject, IExportBankForwardingService
    {
        #region Private functions and declaration
        private ExportBankForwarding MapObject(NullHandler oReader)
        {
            ExportBankForwarding oExportBankForwarding = new ExportBankForwarding();
            oExportBankForwarding.ExportBankForwardingID = oReader.GetInt32("ExportBankForwardingID");
            oExportBankForwarding.ExportBillID = oReader.GetInt32("ExportBillID");
            oExportBankForwarding.Copies_Original = oReader.GetInt32("Copies_Original");
            oExportBankForwarding.Copies = oReader.GetInt32("Copies");
            oExportBankForwarding.Name_Bank = oReader.GetString("BankName");
            oExportBankForwarding.Name_Print = oReader.GetString("Name_Print");
            return oExportBankForwarding;
        }
        private ExportBankForwarding CreateObject(NullHandler oReader)
        {
            ExportBankForwarding oExportBankForwarding = new ExportBankForwarding();
            oExportBankForwarding = MapObject(oReader);
            return oExportBankForwarding;
        }
        private List<ExportBankForwarding> CreateObjects(IDataReader oReader)
        {
            List<ExportBankForwarding> oExportBankForwarding = new List<ExportBankForwarding>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportBankForwarding oItem = CreateObject(oHandler);
                oExportBankForwarding.Add(oItem);
            }
            return oExportBankForwarding;
        }
        #endregion

        #region Interface implementation
        public ExportBankForwarding Save(ExportBankForwarding oExportBankForwarding, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
               

                foreach (ExportBankForwarding Oitem in oExportBankForwarding.ExportBankForwardings)
                {
                    IDataReader reader = null;

                    Oitem.ExportBillID = oExportBankForwarding.ExportBillID;
                    if (Oitem.ExportBankForwardingID <= 0)
                    {
                        reader=ExportBankForwardingDA.InsertUpdate(tc, Oitem, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        reader=ExportBankForwardingDA.InsertUpdate(tc, Oitem, EnumDBOperation.Update, nUserID);
                    }
                    reader.Close();
                }
                // reader = ExportBankForwardingDA.Get(tc, 1);
                //NullHandler oReader = new NullHandler(reader);
                //if (reader.Read())
                //{
                //    oExportBankForwarding = new ExportBankForwarding();
                //    oExportBankForwarding = CreateObject(oReader);
                //}
             
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oExportBankForwarding = new ExportBankForwarding();
                oExportBankForwarding.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oExportBankForwarding;
        }
        public string Delete(int nExportBankForwardingID, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ExportBankForwarding oExportBankForwarding = new ExportBankForwarding();
                oExportBankForwarding.ExportBankForwardingID = nExportBankForwardingID;
                //oExportBankForwarding.ExportBillID = nExportBillID;
                ExportBankForwardingDA.Delete(tc, oExportBankForwarding, EnumDBOperation.Delete, nUserId,"");
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
        public ExportBankForwarding DeleteBYExportBillID(int nExportBillID, Int64 nUserId)
        {
            ExportBankForwarding oExportBankForwarding =new ExportBankForwarding();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                oExportBankForwarding.ExportBillID = nExportBillID;
                ExportBankForwardingDA.DeleteBYExportBillID(tc, oExportBankForwarding);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oExportBankForwarding = new ExportBankForwarding();
                oExportBankForwarding.ErrorMessage= e.Message.Split('!')[0];
                #endregion
            }
            return oExportBankForwarding;
        }
        public ExportBankForwarding Get(int id, Int64 nUserId)
        {
            ExportBankForwarding oAccountHead = new ExportBankForwarding();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = ExportBankForwardingDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get ExportBankForwarding", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<ExportBankForwarding> Gets(int nExportBillID, Int64 nUserID)
        {
            List<ExportBankForwarding> oExportBankForwarding = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportBankForwardingDA.Gets(tc, nExportBillID);
                oExportBankForwarding = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportBankForwarding", e);
                #endregion
            }
            return oExportBankForwarding;
        }
   

        #endregion
    }
}
