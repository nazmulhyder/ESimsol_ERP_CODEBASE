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
    public class ExportPILCMappingService : MarshalByRefObject, IExportPILCMappingService
    {
        #region Private functions and declaration
        private ExportPILCMapping MapObject(NullHandler oReader)
        {
            ExportPILCMapping oExportPILCMapping = new ExportPILCMapping();
            oExportPILCMapping.ExportPILCMappingID = oReader.GetInt32("ExportPILCMappingID");
            oExportPILCMapping.ExportPILCMappingLogID = oReader.GetInt32("ExportPILCMappingLogID");
            oExportPILCMapping.ExportPIID = oReader.GetInt32("ExportPIID");
            oExportPILCMapping.ExportLCID = oReader.GetInt32("ExportLCID");
            oExportPILCMapping.Activity = oReader.GetBoolean("Activity");
            oExportPILCMapping.Date = oReader.GetDateTime("Date");
            oExportPILCMapping.UDRecDate = oReader.GetDateTime("UDRecDate");
            oExportPILCMapping.LCReceiveDate = oReader.GetDateTime("LCReceiveDate");
            oExportPILCMapping.UDRcvType = oReader.GetInt32("UDRcvType");
            oExportPILCMapping.ReviseNo = oReader.GetInt32("ReviseNo");
            oExportPILCMapping.Flag = oReader.GetBoolean("Flag");
            oExportPILCMapping.Amount = oReader.GetDouble("Amount");
            oExportPILCMapping.PIValue = oReader.GetDouble("PIValue");
            if (oExportPILCMapping.PIValue <= 0) { oExportPILCMapping.PIValue = oExportPILCMapping.Amount;}
            oExportPILCMapping.PINo = oReader.GetString("PINo");
            oExportPILCMapping.VersionNo = oReader.GetInt32("VersionNo");
            oExportPILCMapping.Qty = oReader.GetDouble("Qty");
            oExportPILCMapping.IssueDate = oReader.GetDateTime("IssueDate");
            oExportPILCMapping.PIStatus = (EnumPIStatus)oReader.GetInt32("PIStatus");
            oExportPILCMapping.Currency = oReader.GetString("Currency");
            oExportPILCMapping.MUName = oReader.GetString("MUName");
            oExportPILCMapping.LCTermsName = oReader.GetString("LCTermsName");
            oExportPILCMapping.LCTermID = oReader.GetInt32("LCTermID");
            oExportPILCMapping.MKTPName = oReader.GetString("MKTPName");
            oExportPILCMapping.CPerson = oReader.GetString("CPerson");
            oExportPILCMapping.CPPhone = oReader.GetString("CPPhone");
            oExportPILCMapping.BuyerName = oReader.GetString("BuyerName");
            oExportPILCMapping.ReportDate = oReader.GetDateTime("ReportDate");
            
            return oExportPILCMapping;
        }

        private ExportPILCMapping CreateObject(NullHandler oReader)
        {
            ExportPILCMapping oExportPILCMapping = new ExportPILCMapping();
            oExportPILCMapping = MapObject(oReader);
            return oExportPILCMapping;
        }

        private List<ExportPILCMapping> CreateObjects(IDataReader oReader)
        {
            List<ExportPILCMapping> oExportPILCMapping = new List<ExportPILCMapping>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportPILCMapping oItem = CreateObject(oHandler);
                oExportPILCMapping.Add(oItem);
            }
            return oExportPILCMapping;
        }

        #endregion

        #region Interface implementation
        public ExportPILCMappingService() { }

  
        public ExportPILCMapping Save(ExportPILCMapping oExportPILCMapping, Int64 nUserID)
        {
            ExportLC oExportLC = new ExportLC();
            oExportLC = oExportPILCMapping.ExportLC;
            int nCount = 0;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                #region ExportBill Part
                if (oExportLC != null)
                {
                    IDataReader reader;


                    reader = ExportLCDA.InsertUpdate(tc, oExportLC, EnumDBOperation.Insert, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oExportLC = new ExportLC();
                        oExportLC = ExportLCService.CreateObject(oReader);
                        oExportPILCMapping.ExportLCID = oExportLC.ExportLCID;
                    }
                    if (oExportLC.ExportLCID <= 0)
                    {
                        oExportPILCMapping = new ExportPILCMapping();
                        oExportPILCMapping.ErrorMessage = "Invalid ExportPI";
                        return oExportPILCMapping;
                    }
                    reader.Close();
                }
                #endregion

                IDataReader readerdetail;
                if (oExportPILCMapping.ExportPILCMappingID <= 0)
                {
                    readerdetail = ExportPILCMappingDA.InsertUpdate(tc, oExportPILCMapping, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    readerdetail = ExportPILCMappingDA.InsertUpdate(tc, oExportPILCMapping, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReaderDetail = new NullHandler(readerdetail);
                if (readerdetail.Read())
                {
                    oExportPILCMapping = new ExportPILCMapping();
                    oExportPILCMapping = CreateObject(oReaderDetail);
                    oExportPILCMapping.ExportLC = oExportLC;
                }
                readerdetail.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oExportPILCMapping = new ExportPILCMapping();
                oExportPILCMapping.ErrorMessage = e.Message.Split('~')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to . Because of " + e.Message, e);
                #endregion
            }
            return oExportPILCMapping;
        }       

        public ExportPILCMapping UpdateSLHistory(ExportPILCMapping oExportPILCMapping, Int64 nUserID)
        {
            ExportPILCMapping oSLHistory = new ExportPILCMapping();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                ExportPILCMappingDA.Update(tc, oExportPILCMapping, nUserID);

                IDataReader reader = ExportPILCMappingDA.Get(tc, oExportPILCMapping.ExportPILCMappingID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSLHistory = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oSLHistory = new ExportPILCMapping();
                oSLHistory.ErrorMessage = e.Message.Split('~')[0]; 
                #endregion
            }
            return oSLHistory;
        }

        public string Delete(ExportPILCMapping oExportPILCMapping, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                ExportPILCMappingDA.Delete(tc, oExportPILCMapping, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                #endregion
                return e.Message.Split('~')[0]; 
            }
            return Global.DeleteMessage;
        }
        public string DeleteLog(ExportPILCMapping oExportPILCMapping, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                ExportPILCMappingDA.DeleteLog(tc, oExportPILCMapping, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                #endregion
                return e.Message;
            }
            return "Data delete successfully";
        }

        public List<ExportPILCMapping> GetsByLCID(int nExportLCID, Int64 nUserID)
        {
            List<ExportPILCMapping> oExportPILCMappings = new List<ExportPILCMapping>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportPILCMappingDA.GetsByLCID(tc, nExportLCID);
                oExportPILCMappings = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PILCMappings", e);
                #endregion
            }

            return oExportPILCMappings;
        }
        public List<ExportPILCMapping> GetsLogByLCID(int nExportLCLogID, Int64 nUserID)
        {
            List<ExportPILCMapping> oExportPILCMappings = new List<ExportPILCMapping>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportPILCMappingDA.GetsLogByLCID(tc, nExportLCLogID);
                oExportPILCMappings = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PILCMappings", e);
                #endregion
            }

            return oExportPILCMappings;
        }
        public List<ExportPILCMapping> GetsByEBillID(int nExportBillID, Int64 nUserID)
        {
            List<ExportPILCMapping> oExportPILCMappings = new List<ExportPILCMapping>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportPILCMappingDA.GetsByEBillID(tc, nExportBillID);
                oExportPILCMappings = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PILCMappings", e);
                #endregion
            }

            return oExportPILCMappings;
        }
        public List<ExportPILCMapping> Gets(int nExportLCID, int nVersionNo, Int64 nUserID)
        {
            List<ExportPILCMapping> oExportPILCMappings = new List<ExportPILCMapping>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportPILCMappingDA.Gets(tc, nExportLCID, nVersionNo);
                oExportPILCMappings = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PILCMappings", e);
                #endregion
            }

            return oExportPILCMappings;
        }
        public List<ExportPILCMapping> Gets(string sSQL, Int64 nUserID)
        {
            List<ExportPILCMapping> oExportPILCMappings = new List<ExportPILCMapping>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportPILCMappingDA.Gets(tc, sSQL);
                oExportPILCMappings = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Export PI LC Mappings", e);
                #endregion
            }

            return oExportPILCMappings;
        }

        public ExportPILCMapping UpdateExportPILCMapping(ExportPILCMapping oExportPILCMapping, Int64 nUserID)
        {
            ExportPILCMapping _oExportPILCMapping = new ExportPILCMapping();
            _oExportPILCMapping = oExportPILCMapping;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader readerdetail;

                readerdetail = ExportPILCMappingDA.UpdateExportPILCMapping(tc, _oExportPILCMapping, nUserID);
                
                NullHandler oReaderDetail = new NullHandler(readerdetail);
                if (readerdetail.Read())
                {
                    oExportPILCMapping = new ExportPILCMapping();
                    oExportPILCMapping = CreateObject(oReaderDetail);
                }
                readerdetail.Close();

                tc.End();
            }
            catch (Exception e)
            {
                if (tc != null)
                {
                    tc.HandleError();
                    _oExportPILCMapping = new ExportPILCMapping();
                    _oExportPILCMapping.ErrorMessage = e.Message;
                }
            }
            return _oExportPILCMapping;
        }

        #endregion
    }
}