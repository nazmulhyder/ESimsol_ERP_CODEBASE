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
    [Serializable]
    public class ImportInvChallanService : MarshalByRefObject, IImportInvChallanService
    {
        #region Private functions and declaration
        private ImportInvChallan MapObject(NullHandler oReader)
        {
            ImportInvChallan oImportInvChallan = new ImportInvChallan();
            oImportInvChallan.ImportInvChallanID = oReader.GetInt32("ImportInvChallanID");
            oImportInvChallan.ChallanNo = oReader.GetString("ChallanNo");
            oImportInvChallan.ChallanDate = oReader.GetDateTime("ChallanDate");
            oImportInvChallan.IsGRN = oReader.GetBoolean("IsGRN");
            //oImportInvChallan.PackCountByInInt = oReader.GetInt32("PackCountBy");
            oImportInvChallan.Note = oReader.GetString("Note");
            oImportInvChallan.DriverName = oReader.GetString("DriverName");
            oImportInvChallan.ImportInvoiceNo = oReader.GetString("ImportInvoiceNo");
            oImportInvChallan.VehicleInfo = oReader.GetString("VehicleInfo");
            oImportInvChallan.CotractNo = oReader.GetString("CotractNo");
            oImportInvChallan.ReceiveBy = oReader.GetInt32("ReceiveBy");
            oImportInvChallan.ApproveBy = oReader.GetInt32("ApproveBy");
            oImportInvChallan.ReceiveBy = oReader.GetInt32("ReceiveBy");
            oImportInvChallan.PackCountByInInt = oReader.GetInt32("PackCountBy");
            oImportInvChallan.PrepareByName = oReader.GetString("PrepareByName");
            oImportInvChallan.ApproveByName = oReader.GetString("ApproveByName");
            oImportInvChallan.ReceiveByName = oReader.GetString("ReceiveByName");
            return oImportInvChallan;
        }

        private ImportInvChallan CreateObject(NullHandler oReader)
        {
            ImportInvChallan oImportInvChallan = new ImportInvChallan();
            oImportInvChallan = MapObject(oReader);
            return oImportInvChallan;
        }

        private List<ImportInvChallan> CreateObjects(IDataReader oReader)
        {
            List<ImportInvChallan> oImportInvChallans = new List<ImportInvChallan>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportInvChallan oItem = CreateObject(oHandler);
                oImportInvChallans.Add(oItem);
            }
            return oImportInvChallans;
        }
        #endregion

        #region Interface implementation
        public ImportInvChallanService() { }

        #region Save Import Invoice & Import Invoice Product
        public ImportInvChallan Save(ImportInvChallan oImportInvChallan, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<ImportInvChallanDetail> oImportInvChallanDetails = new List<ImportInvChallanDetail>();
                oImportInvChallanDetails = oImportInvChallan.ImportInvChallanDetails;
                string sImportInvChallanDetailIDS = "";

                IDataReader reader;
                if (oImportInvChallan.ImportInvChallanID <= 0)
                {
                    reader = ImportInvChallanDA.InsertUpdate(tc, oImportInvChallan, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ImportInvChallanDA.InsertUpdate(tc, oImportInvChallan, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportInvChallan = CreateObject(oReader);
                }
                reader.Close();

                #region ImportInvChallan Part

                foreach (ImportInvChallanDetail oItem in oImportInvChallanDetails)
                {
                    IDataReader readerdetail;
                    oItem.ImportInvChallanID = oImportInvChallan.ImportInvChallanID;
                    if (oItem.ImportInvChallanDetailID <= 0)
                    {
                        readerdetail = ImportInvChallanDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID,"");
                    }
                    else
                    {
                        readerdetail = ImportInvChallanDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID,"");
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sImportInvChallanDetailIDS = sImportInvChallanDetailIDS + oReaderDetail.GetString("ImportInvChallanDetailID") + ",";
                    }
                    readerdetail.Close();
                }
                ImportInvChallanDetail oImportInvChallanDetail = new ImportInvChallanDetail();
                oImportInvChallanDetail.ImportInvChallanID = oImportInvChallan.ImportInvChallanID;
                if (sImportInvChallanDetailIDS.Length > 0)
                {
                    sImportInvChallanDetailIDS = sImportInvChallanDetailIDS.Remove(sImportInvChallanDetailIDS.Length - 1, 1);
                }
                ImportInvChallanDetailDA.Delete(tc, oImportInvChallanDetail, EnumDBOperation.Delete, nUserID, sImportInvChallanDetailIDS);
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oImportInvChallan.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oImportInvChallan;

        }
        public ImportInvChallan Approved(ImportInvChallan oImportInvChallan, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<ImportInvChallanDetail> oImportInvChallanDetails = new List<ImportInvChallanDetail>();

                IDataReader reader;
                if (oImportInvChallan.ImportInvChallanID <= 0)
                {
                    reader = ImportInvChallanDA.InsertUpdate(tc, oImportInvChallan, EnumDBOperation.Approval, nUserID);
                }
                else
                {
                    reader = ImportInvChallanDA.InsertUpdate(tc, oImportInvChallan, EnumDBOperation.Approval, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportInvChallan = CreateObject(oReader);
                }
                reader.Close();
                
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oImportInvChallan.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oImportInvChallan;

        }
        public ImportInvChallan Received(ImportInvChallan oImportInvChallan, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<ImportInvChallanDetail> oImportInvChallanDetails = new List<ImportInvChallanDetail>();

                IDataReader reader;
                if (oImportInvChallan.ImportInvChallanID <= 0)
                {
                    reader = ImportInvChallanDA.InsertUpdate(tc, oImportInvChallan, EnumDBOperation.Request, nUserID);
                }
                else
                {
                    reader = ImportInvChallanDA.InsertUpdate(tc, oImportInvChallan, EnumDBOperation.Request, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportInvChallan = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oImportInvChallan.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oImportInvChallan;

        }
   
        public List<ImportInvChallan> Gets(string sSQL, Int64 nUserID)
        {
            List<ImportInvChallan> oImportInvChallan = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportInvChallanDA.Gets(tc, sSQL);
                oImportInvChallan = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportInvChallan", e);
                #endregion
            }

            return oImportInvChallan;
        }

        #endregion

        #region Delete
        public String Delete(ImportInvChallan oImportInvChallan, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ImportInvChallanDA.Delete(tc, oImportInvChallan, EnumDBOperation.Delete, nUserID);
                tc.End();

            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                return e.Message;
                #endregion
            }
            return Global.DeleteMessage;
        }

        #endregion

        #region Retrive Information

        public ImportInvChallan Get(int nImportInvChallanID, Int64 nUserID)
        {
            ImportInvChallan oImportInvChallan = new ImportInvChallan();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportInvChallanDA.Get(nImportInvChallanID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportInvChallan = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ImportInvChallan", e);
                #endregion
            }

            return oImportInvChallan;
        }

        public ImportInvChallan GetByInvoice(int nInvoiceID, Int64 nUserID)
        {
            ImportInvChallan oImportInvChallan = new ImportInvChallan();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportInvChallanDA.GetByInvoice(nInvoiceID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportInvChallan = CreateObject(oReader);
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportInvChallans", e);
                #endregion
            }

            return oImportInvChallan;
        }

        public List<ImportInvChallan> Gets(Int64 nUserID)
        {
            List<ImportInvChallan> oImportInvChallans = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportInvChallanDA.Gets(tc);
                oImportInvChallans = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportInvChallans", e);
                #endregion
            }

            return oImportInvChallans;
        }

        #endregion



        #endregion


    }

}
