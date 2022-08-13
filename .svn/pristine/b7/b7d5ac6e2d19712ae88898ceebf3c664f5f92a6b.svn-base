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
    public class ExportPartyInfoBillService : MarshalByRefObject, IExportPartyInfoBillService
    {
        #region Private functions and declaration
        private ExportPartyInfoBill MapObject(NullHandler oReader)
        {
            ExportPartyInfoBill oExportPartyInfoBill = new ExportPartyInfoBill();
            oExportPartyInfoBill.ExportPartyInfoBillID = oReader.GetInt32("ExportPartyInfoBillID");
            oExportPartyInfoBill.ReferenceID = oReader.GetInt32("ReferenceID");
            oExportPartyInfoBill.RefType = (EnumMasterLCType) oReader.GetInt32("RefType");
            oExportPartyInfoBill.RefTypeInInt= oReader.GetInt32("RefType");
            oExportPartyInfoBill.ExportPartyInfoID = oReader.GetInt32("ExportPartyInfoID");
            oExportPartyInfoBill.ExportPartyInfoDetailID = oReader.GetInt32("ExportPartyInfoDetailID");
            oExportPartyInfoBill.PartyInfo = oReader.GetString("PartyInfo");
            oExportPartyInfoBill.RefNo = oReader.GetString("RefNo");
            oExportPartyInfoBill.RefDate = oReader.GetString("RefDate");
            return oExportPartyInfoBill;
        }

        private ExportPartyInfoBill CreateObject(NullHandler oReader)
        {
            ExportPartyInfoBill oExportPartyInfoBill = new ExportPartyInfoBill();
            oExportPartyInfoBill = MapObject(oReader);
            return oExportPartyInfoBill;
        }

        private List<ExportPartyInfoBill> CreateObjects(IDataReader oReader)
        {
            List<ExportPartyInfoBill> oExportPartyInfoBill = new List<ExportPartyInfoBill>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportPartyInfoBill oItem = CreateObject(oHandler);
                oExportPartyInfoBill.Add(oItem);
            }
            return oExportPartyInfoBill;
        }

        #endregion

        #region Interface implementation
        public ExportPartyInfoBillService() { }

        public ExportPartyInfoBill Save(ExportPartyInfoBill oExportPartyInfoBill, int nUserID)
        {
            string sExportPartyInfoBillIDs = ""; int nExportBillID = 0;
            List<ExportPartyInfoBill> oExportPartyInfoBills = new List<ExportPartyInfoBill>();
            oExportPartyInfoBills = oExportPartyInfoBill.ExportPartyInfoBills;
            nExportBillID = oExportPartyInfoBill.ReferenceID;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                foreach (ExportPartyInfoBill oItem in oExportPartyInfoBills)
                {
                    if (oItem.ExportPartyInfoBillID <= 0)
                    {
                        reader = ExportPartyInfoBillDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        reader = ExportPartyInfoBillDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oExportPartyInfoBill = new ExportPartyInfoBill();
                        oExportPartyInfoBill = CreateObject(oReader);
                        sExportPartyInfoBillIDs = sExportPartyInfoBillIDs + oExportPartyInfoBill.ExportPartyInfoBillID.ToString() + ",";
                    }
                    reader.Close();
                }

                #region Delete Not in List
                if (sExportPartyInfoBillIDs.Length > 0)
                {
                    sExportPartyInfoBillIDs = sExportPartyInfoBillIDs.Remove(sExportPartyInfoBillIDs.Length - 1, 1);
                }
                oExportPartyInfoBill = new ExportPartyInfoBill();
                oExportPartyInfoBill.ReferenceID = nExportBillID;
                ExportPartyInfoBillDA.Delete(tc, oExportPartyInfoBill, EnumDBOperation.Delete, nUserID, sExportPartyInfoBillIDs);
                #endregion

                oExportPartyInfoBill = new ExportPartyInfoBill();
                oExportPartyInfoBill.ErrorMessage = "Data Update Successfully";

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oExportPartyInfoBill = new ExportPartyInfoBill();
                oExportPartyInfoBill.ErrorMessage = e.Message.Split('~')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save ExportPartyInfoBill. Because of " + e.Message, e);
                #endregion
            }
            return oExportPartyInfoBill;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ExportPartyInfoBill oExportPartyInfoBill = new ExportPartyInfoBill();
                oExportPartyInfoBill.ExportPartyInfoBillID = id;                
                DBTableReferenceDA.HasReference(tc, "ExportPartyInfoBill", id);
                ExportPartyInfoBillDA.Delete(tc, oExportPartyInfoBill, EnumDBOperation.Delete, nUserId, "");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete ExportPartyInfoBill. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public ExportPartyInfoBill Get(int id, int nUserId)
        {
            ExportPartyInfoBill oAccountHead = new ExportPartyInfoBill();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportPartyInfoBillDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get ExportPartyInfoBill", e);
                #endregion
            }

            return oAccountHead;
        }



        public List<ExportPartyInfoBill> Gets(int nUserID)
        {
            List<ExportPartyInfoBill> oExportPartyInfoBill = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportPartyInfoBillDA.Gets(tc);
                oExportPartyInfoBill = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportPartyInfoBill", e);
                #endregion
            }

            return oExportPartyInfoBill;
        }
        public List<ExportPartyInfoBill> Gets(string sSQL, int nUserID)
        {
            List<ExportPartyInfoBill> oExportPartyInfoBill = new List<ExportPartyInfoBill>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                if (sSQL == "")
                {
                    sSQL = "Select * from View_ExportPartyInfoBill";
                }
                reader = ExportPartyInfoBillDA.Gets(tc, sSQL);
                oExportPartyInfoBill = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportPartyInfoBill", e);
                #endregion
            }

            return oExportPartyInfoBill;
        }

        public List<ExportPartyInfoBill> GetsByExportLC(int nReferenceID, int nRefType, int nUserID)
        {
            List<ExportPartyInfoBill> oExportPartyInfoBills = new List<ExportPartyInfoBill>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportPartyInfoBillDA.GetsByExportLC(tc, nReferenceID, nRefType);
                oExportPartyInfoBills = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oExportPartyInfoBills = new List<ExportPartyInfoBill>();
                ExportPartyInfoBill oExportPartyInfoBill = new ExportPartyInfoBill();
                oExportPartyInfoBill.ErrorMessage = e.Message;
                oExportPartyInfoBills.Add(oExportPartyInfoBill);
                #endregion
            }
            return oExportPartyInfoBills;
        }
        #endregion
    }
}
