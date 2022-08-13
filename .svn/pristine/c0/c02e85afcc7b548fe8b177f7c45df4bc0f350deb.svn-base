using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    class ExportUPService : MarshalByRefObject, IExportUPService
    {
        #region Private functions and declaration
        private ExportUP MapObject(NullHandler oReader)
        {
            ExportUP oExportUP = new ExportUP();
            oExportUP.ExportUPID = oReader.GetInt32("ExportUPID");
            oExportUP.BUID = oReader.GetInt32("BUID");
            oExportUP.UPNo = oReader.GetString("UPNo");
            oExportUP.ExportUPDate = oReader.GetDateTime("ExportUPDate");
            oExportUP.UPStatus = (EnumUPStatus)oReader.GetInt16("UPStatus");
            oExportUP.PrepareBYID = oReader.GetInt32("PrepareBYID");
            oExportUP.PrepareDate = oReader.GetDateTime("PrepareDate");
            oExportUP.DeliveryByID = oReader.GetInt32("DeliveryByID");
            oExportUP.DeliveryDate = oReader.GetDateTime("DeliveryDate");
            oExportUP.ApproveByID = oReader.GetInt32("ApproveByID");
            oExportUP.ApproveDate = oReader.GetDateTime("ApproveDate");
            oExportUP.ExportUPSetupID = oReader.GetInt32("ExportUPSetupID");
            oExportUP.PrepareByName = oReader.GetString("PrepareByName");
            oExportUP.DeliveryByName = oReader.GetString("DeliveryByName");
            oExportUP.ApproveByName = oReader.GetString("ApproveByName");
            return oExportUP;
        }

        public static ExportUP CreateObject(NullHandler oReader)
        {
            ExportUPService oExportUPService = new ExportUPService();
            ExportUP oExportUPs = oExportUPService.MapObject(oReader);
            return oExportUPs;
        }

        private List<ExportUP> CreateObjects(IDataReader oReader)
        {
            List<ExportUP> oExportUPs = new List<ExportUP>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportUP oItem = CreateObject(oHandler);
                oExportUPs.Add(oItem);
            }
            return oExportUPs;
        }

        #endregion

        #region Interface implementation
        public ExportUPService() { }

        public ExportUP IUD(ExportUP oExportUP, Int64 nUserID)
        {
            ExportUPDetail oExportUPDetail = new ExportUPDetail();
            ExportUP oUG = new ExportUP();
            oUG = oExportUP;

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                #region ExportUP
                IDataReader reader;
                if (oExportUP.ExportUPID <= 0)
                {
                    reader = ExportUPDA.IUD(tc, oExportUP, (int)EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ExportUPDA.IUD(tc, oExportUP, (int)EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportUP = new ExportUP();
                    oExportUP = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region ExportUPDetail

                if (oExportUP.ExportUPID > 0)
                {
                    string sExportUPDetailIDs = "";
                    if (oUG.ExportUPDetails.Count > 0)
                    {
                        IDataReader readerdetail;
                        foreach (ExportUPDetail oDRD in oUG.ExportUPDetails)
                        {
                            oDRD.ExportUPID = oExportUP.ExportUPID;
                            oDRD.Note = (oDRD.Note == null) ? "" : oDRD.Note;
                            if (oDRD.ExportUPDetailID <= 0)
                            {
                                readerdetail = ExportUPDetailDA.IUD(tc, oDRD, (int)EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readerdetail = ExportUPDetailDA.IUD(tc, oDRD, (int)EnumDBOperation.Update, nUserID, "");

                            }
                            NullHandler oReaderDevRecapdetail = new NullHandler(readerdetail);
                            int nExportUPDetailID = 0;
                            if (readerdetail.Read())
                            {
                                nExportUPDetailID = oReaderDevRecapdetail.GetInt32("ExportUPDetailID");
                                sExportUPDetailIDs = sExportUPDetailIDs + oReaderDevRecapdetail.GetString("ExportUPDetailID") + ",";
                            }
                            readerdetail.Close();
                        }
                    }
                    if (sExportUPDetailIDs.Length > 0)
                    {
                        sExportUPDetailIDs = sExportUPDetailIDs.Remove(sExportUPDetailIDs.Length - 1, 1);
                    }
                    oExportUPDetail = new ExportUPDetail();
                    oExportUPDetail.ExportUPID = oExportUP.ExportUPID;
                    ExportUPDetailDA.Delete(tc, oExportUPDetail, (int)EnumDBOperation.Delete, nUserID, sExportUPDetailIDs);
                }

                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oExportUP = new ExportUP();
                    oExportUP.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oExportUP;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ExportUP oExportUP = new ExportUP();
                oExportUP.ExportUPID = id;
                DBTableReferenceDA.HasReference(tc, "ExportUP", id);
                ExportUPDA.Delete(tc, oExportUP, (int)EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exceptionif (tc != null)
                tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }


        public ExportUP Approve(ExportUP oExportUP, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                #region ExportUP
                IDataReader reader;
                reader = ExportUPDA.IUD(tc, oExportUP, (int)EnumDBOperation.Approval, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportUP = new ExportUP();
                    oExportUP = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oExportUP = new ExportUP();
                    oExportUP.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oExportUP;
        }

        public ExportUP Get(int nSUPCRID, Int64 nUserId)
        {
            ExportUP oExportUP = new ExportUP();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportUPDA.Get(tc, nSUPCRID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportUP = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oExportUP = new ExportUP();
                oExportUP.ErrorMessage = ex.Message;
                #endregion
            }

            return oExportUP;
        }

        public List<ExportUP> Gets(string sSQL, Int64 nUserID)
        {
            List<ExportUP> oExportUPs = new List<ExportUP>();
            ExportUP oExportUP = new ExportUP();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportUPDA.Gets(tc, sSQL);
                oExportUPs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oExportUP.ErrorMessage = ex.Message;
                oExportUPs.Add(oExportUP);
                #endregion
            }

            return oExportUPs;
        }

        #endregion
    }
}
