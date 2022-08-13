using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;

namespace ESimSol.Services.Services
{
    public class ExportUDService : MarshalByRefObject, IExportUDService
    {
        #region Private functions and declaration

        private ExportUD MapObject(NullHandler oReader)
        {
            ExportUD oExportUD = new ExportUD();
            oExportUD.ExportUDID = oReader.GetInt32("ExportUDID");
            oExportUD.ExportLCID = oReader.GetInt32("ExportLCID");
            oExportUD.ANo = oReader.GetInt32("ANo");
            oExportUD.Amount = oReader.GetDouble("Amount");
            oExportUD.UDNo = oReader.GetString("UDNo");
            oExportUD.UDReceiveDate = oReader.GetDateTime("UDReceiveDate");
            oExportUD.ReceiveBYID = oReader.GetInt32("ReceiveBYID");
            oExportUD.ReceiveFrom = oReader.GetString("ReceiveFrom");
            oExportUD.ContractNo = oReader.GetString("ContractNo");
            oExportUD.Note = oReader.GetString("Note");
            oExportUD.ExportLCNo = oReader.GetString("ExportLCNo");
            oExportUD.LCOpeningDate = oReader.GetDateTime("LCOpeningDate");
            oExportUD.ApplicantName = oReader.GetString("ApplicantName");
            oExportUD.AUDNo = oReader.GetString("AUDNo");
            oExportUD.ADate = oReader.GetDateTime("ADate");
            oExportUD.YetToUPAmount = oReader.GetDouble("YetToUPAmount");

            return oExportUD;
        }

        private ExportUD CreateObject(NullHandler oReader)
        {
            ExportUD oExportUD = new ExportUD();
            oExportUD = MapObject(oReader);
            return oExportUD;
        }

        private List<ExportUD> CreateObjects(IDataReader oReader)
        {
            List<ExportUD> oExportUD = new List<ExportUD>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportUD oItem = CreateObject(oHandler);
                oExportUD.Add(oItem);
            }
            return oExportUD;
        }

        #endregion

        #region Interface implementation
        public ExportUD Save(ExportUD oExportUD, Int64 nUserID)
        {
            ExportUDDetail oExportUDDetail = new ExportUDDetail();
            ExportUD oUG = new ExportUD();
            oUG = oExportUD;

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                #region ExportUD
                IDataReader reader;
                if (oExportUD.ADate == DateTime.MinValue || oExportUD.ADate == null) oExportUD.ADate = null;
                if (oExportUD.ExportUDID <= 0)
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ExportUD, EnumRoleOperationType.Add);
                    reader = ExportUDDA.InsertUpdate(tc, oExportUD, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ExportUD, EnumRoleOperationType.Edit);
                    reader = ExportUDDA.InsertUpdate(tc, oExportUD, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportUD = new ExportUD();
                    oExportUD = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region ExportUDDetail

                if (oExportUD.ExportUDID > 0)
                {
                    string sExportUDDetailIDs = "";
                    if (oUG.ExportUDDetails.Count > 0)
                    {
                        IDataReader readerdetail;
                        foreach (ExportUDDetail oDRD in oUG.ExportUDDetails)
                        {
                            oDRD.ExportUDID = oExportUD.ExportUDID;
                            if (oDRD.ExportUDDetailID <= 0)
                            {
                                readerdetail = ExportUDDetailDA.InsertUpdate(tc, oDRD, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readerdetail = ExportUDDetailDA.InsertUpdate(tc, oDRD, EnumDBOperation.Update, nUserID, "");

                            }
                            NullHandler oReaderDevRecapdetail = new NullHandler(readerdetail);
                            int nExportUDDetailID = 0;
                            if (readerdetail.Read())
                            {
                                nExportUDDetailID = oReaderDevRecapdetail.GetInt32("ExportUDDetailID");
                                sExportUDDetailIDs = sExportUDDetailIDs + oReaderDevRecapdetail.GetString("ExportUDDetailID") + ",";
                            }
                            readerdetail.Close();
                        }
                    }
                    if (sExportUDDetailIDs.Length > 0)
                    {
                        sExportUDDetailIDs = sExportUDDetailIDs.Remove(sExportUDDetailIDs.Length - 1, 1);
                    }
                    oExportUDDetail = new ExportUDDetail();
                    oExportUDDetail.ExportUDID = oExportUD.ExportUDID;
                    ExportUDDetailDA.Delete(tc, oExportUDDetail, EnumDBOperation.Delete, nUserID, sExportUDDetailIDs);
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
                    oExportUD = new ExportUD();
                    oExportUD.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oExportUD;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ExportUD oExportUD = new ExportUD();
                oExportUD.ExportUDID = id;
                if (oExportUD.ADate == DateTime.MinValue || oExportUD.ADate == null) oExportUD.ADate = null;
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.ExportUD, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "ExportUD", id);
                ExportUDDA.Delete(tc, oExportUD, EnumDBOperation.Delete, nUserId);
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

        public ExportUD Get(int id, Int64 nUserId)
        {
            ExportUD oExportUD = new ExportUD();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = ExportUDDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportUD = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ExportUD", e);
                #endregion
            }
            return oExportUD;
        }

        public List<ExportUD> Gets(Int64 nUserID)
        {
            List<ExportUD> oExportUDs = new List<ExportUD>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportUDDA.Gets(tc);
                oExportUDs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExportUD oExportUD = new ExportUD();
                oExportUD.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oExportUDs;
        }

        public List<ExportUD> Gets(string sSQL, Int64 nUserID)
        {
            List<ExportUD> oExportUDs = new List<ExportUD>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportUDDA.Gets(tc, sSQL);
                oExportUDs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportUD", e);
                #endregion
            }
            return oExportUDs;
        }

        #endregion
    }

}
