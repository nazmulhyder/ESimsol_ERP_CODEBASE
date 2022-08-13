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
    class ExportUPDetailService : MarshalByRefObject, IExportUPDetailService
    {
        #region Private functions and declaration
        private ExportUPDetail MapObject(NullHandler oReader)
        {
            ExportUPDetail oExportUPDetail = new ExportUPDetail();
            oExportUPDetail.ExportUPDetailID = oReader.GetInt32("ExportUPDetailID");
            oExportUPDetail.ExportUPID = oReader.GetInt32("ExportUPID");
            oExportUPDetail.ExportUDID = oReader.GetInt32("ExportUDID");
            oExportUPDetail.DateofUDReceive = oReader.GetDateTime("DateofUDReceive");
            oExportUPDetail.ContractPersonalID = oReader.GetInt32("ContractPersonalID");
            oExportUPDetail.Note = oReader.GetString("Note");
            oExportUPDetail.ExportUDNo = oReader.GetString("ExportUDNo");
            oExportUPDetail.ApplicantName = oReader.GetString("ApplicantName");
            oExportUPDetail.ExportLCNo = oReader.GetString("ExportLCNo");
            oExportUPDetail.LCOpeningDate = oReader.GetDateTime("LCOpeningDate");
            oExportUPDetail.Qty = oReader.GetDouble("Qty");
            oExportUPDetail.Amount = oReader.GetDouble("Amount");
            oExportUPDetail.UDReceiveDate = oReader.GetDateTime("UDReceiveDate");
            oExportUPDetail.ANo = oReader.GetInt32("ANo");
            oExportUPDetail.UPNo = oReader.GetString("UPNo");
            oExportUPDetail.ExportLCID = oReader.GetInt32("ExportLCID");
            oExportUPDetail.YetToUPAmount = oReader.GetDouble("YetToUPAmount");

            return oExportUPDetail;
        }

        private ExportUPDetail CreateObject(NullHandler oReader)
        {
            ExportUPDetail oExportUPDetails = MapObject(oReader);
            return oExportUPDetails;
        }

        private List<ExportUPDetail> CreateObjects(IDataReader oReader)
        {
            List<ExportUPDetail> oExportUPDetails = new List<ExportUPDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportUPDetail oItem = CreateObject(oHandler);
                oExportUPDetails.Add(oItem);
            }
            return oExportUPDetails;
        }

        #endregion

        #region Interface implementation
        public ExportUPDetailService() { }

        public ExportUPDetail IUD(ExportUPDetail oExportUPDetail, int nDBOperation, Int64 nUserID)
        {
            ExportUP oExportUP = new ExportUP();
            TransactionContext tc = null;
            IDataReader reader;
            NullHandler oReader;
            try
            {
                tc = TransactionContext.Begin(true);

                if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update)
                {
                    if (oExportUPDetail.ExportUPID <= 0 && oExportUPDetail.ExportUP !=null)
                    {
                        reader = ExportUPDA.IUD(tc, oExportUPDetail.ExportUP, nDBOperation, nUserID);
                        oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oExportUP = new ExportUP();
                            oExportUP = ExportUPService.CreateObject(oReader);
                        }
                        reader.Close();
                    }
                    oExportUPDetail.ExportUPID = (oExportUPDetail.ExportUPID > 0) ? oExportUPDetail.ExportUPID : oExportUP.ExportUPID;
                    oExportUPDetail.Note = (oExportUPDetail.Note == null) ? "" : oExportUPDetail.Note;
                    reader = ExportUPDetailDA.IUD(tc, oExportUPDetail, nDBOperation, nUserID,"");
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oExportUPDetail = new ExportUPDetail();
                        oExportUPDetail = CreateObject(oReader);
                    }
                    reader.Close();
                }
                else if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    reader = ExportUPDetailDA.IUD(tc, oExportUPDetail, nDBOperation, nUserID,"");
                    oReader = new NullHandler(reader);
                    reader.Close();
                    oExportUPDetail.ErrorMessage = Global.DeleteMessage;
                }

                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oExportUPDetail = new ExportUPDetail();
                oExportUPDetail.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion

            }
            oExportUPDetail.ExportUP = oExportUP;
            return oExportUPDetail;
        }

        public ExportUPDetail Get(int nSUPCRID, Int64 nUserId)
        {
            ExportUPDetail oExportUPDetail = new ExportUPDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportUPDetailDA.Get(tc, nSUPCRID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportUPDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oExportUPDetail = new ExportUPDetail();
                oExportUPDetail.ErrorMessage = ex.Message;
                #endregion
            }

            return oExportUPDetail;
        }

        public List<ExportUPDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<ExportUPDetail> oExportUPDetails = new List<ExportUPDetail>();
            ExportUPDetail oExportUPDetail = new ExportUPDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportUPDetailDA.Gets(tc, sSQL);
                oExportUPDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oExportUPDetail.ErrorMessage = ex.Message;
                oExportUPDetails.Add(oExportUPDetail);
                #endregion
            }

            return oExportUPDetails;
        }

        #endregion
    }
}
