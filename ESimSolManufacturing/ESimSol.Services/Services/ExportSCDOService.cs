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
    public class ExportSCDOService : MarshalByRefObject, IExportSCDOService
    {
    
        #region Private functions and declaration
        private static ExportSCDO MapObject(NullHandler oReader)
        {
            ExportSCDO oExportSCDO = new ExportSCDO();
            oExportSCDO.ExportSCID = oReader.GetInt32("ExportSCID");
            oExportSCDO.ExportPIID = oReader.GetInt32("ExportPIID");
            //oExportSCDO.TextileUnit = (EnumBusinessUnitType)oReader.GetInt32("TextileUnit");
            //oExportSCDO.TextileUnitInInt = oReader.GetInt32("TextileUnit");
            oExportSCDO.PaymentType = (EnumPIPaymentType)oReader.GetInt32("PaymentType");
            oExportSCDO.PaymentTypeInInt = oReader.GetInt32("PaymentType");
            oExportSCDO.PINo = oReader.GetString("PINo");
            oExportSCDO.ReviseNo = oReader.GetInt32("ReviseNo");
            oExportSCDO.PIStatus = (EnumPIStatus)oReader.GetInt32("PIStatus");
            oExportSCDO.PIStatusInInt = oReader.GetInt32("PIStatus");
            oExportSCDO.IssueDate = oReader.GetDateTime("IssueDate");
            oExportSCDO.SCDate = oReader.GetDateTime("SCDate");
            oExportSCDO.ValidityDate = oReader.GetDateTime("ValidityDate");
            oExportSCDO.ContractorID = oReader.GetInt32("ContractorID");
            oExportSCDO.BuyerID = oReader.GetInt32("BuyerID");
            oExportSCDO.MKTEmpID = oReader.GetInt32("MKTEmpID");
            oExportSCDO.CurrencyID = oReader.GetInt32("CurrencyID");
            oExportSCDO.Qty_PI = oReader.GetDouble("Qty_PI");
            oExportSCDO.Amount_PI = oReader.GetDouble("Amount_PI");

            oExportSCDO.AdjQty = oReader.GetDouble("AdjQty");
            oExportSCDO.AdjAmount = oReader.GetDouble("AdjAmount");
            oExportSCDO.TotalAmount = oReader.GetDouble("TotalAmount");
            oExportSCDO.TotalQty = oReader.GetDouble("TotalQty");
            oExportSCDO.POQty = oReader.GetDouble("DOQty");
            oExportSCDO.LCOpenDate = oReader.GetDateTime("LCOpenDate");
            oExportSCDO.DeliveryDate = oReader.GetDateTime("AppDeliveryDate");
            oExportSCDO.Note = oReader.GetString("Note");
            oExportSCDO.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oExportSCDO.ApprovedDate = oReader.GetDateTime("ApprovedDate");
            oExportSCDO.LCID = oReader.GetInt32("LCID");
            oExportSCDO.ColorInfo = oReader.GetString("ColorInfo");
            oExportSCDO.DepthOfShade = oReader.GetString("DepthOfShade");
            //oExportSCDO.YarnCount = oReader.GetString("YarnCount");
            //oExportSCDO.DeliveryToName = oReader.GetString("DeliveryToName");
            oExportSCDO.ContractorName = oReader.GetString("ContractorName");
           // oExportSCDO.ContractorType = oReader.GetInt32("ContractorType");
            oExportSCDO.BuyerName = oReader.GetString("BuyerName");
            oExportSCDO.ContractorAddress = oReader.GetString("ContractorAddress");
            oExportSCDO.ContractorPhone = oReader.GetString("ContractorPhone");
            oExportSCDO.ContractorFax = oReader.GetString("ContractorFax");
            oExportSCDO.ContractorEmail = oReader.GetString("ContractorEmail");
            oExportSCDO.MKTPName = oReader.GetString("MKTPName");
            oExportSCDO.MKTPNickName = oReader.GetString("MKTPNickName");
            oExportSCDO.Currency = oReader.GetString("Currency");
            oExportSCDO.IsRevisePI = oReader.GetBoolean("IsRevisePI");
            oExportSCDO.ShipmentDate = oReader.GetDateTime("ShipmentDate");
            oExportSCDO.ExportLCNo = oReader.GetString("ExportLCNo");
            oExportSCDO.CurrentStatus_LC = oReader.GetInt32("CurrentStatus_LC");
            oExportSCDO.ShipmentDate = oReader.GetDateTime("ShipmentDate");
            oExportSCDO.AmendmentDate = oReader.GetDateTime("AmendmentDate");
            oExportSCDO.AmendmentNo = oReader.GetInt32("AmendmentNo");
            oExportSCDO.AmendmentRequired = oReader.GetBoolean("AmendmentRequired");
            oExportSCDO.ContractorContactPersonName = oReader.GetString("ContractorContactPersonName");
            oExportSCDO.BuyerContactPersonName = oReader.GetString("BuyerContactPersonName");

           
            return oExportSCDO;

         
        }

        public static ExportSCDO CreateObject(NullHandler oReader)
        {
            ExportSCDO oExportSCDO = new ExportSCDO();
            oExportSCDO = MapObject(oReader);            
            return oExportSCDO;
        }

        private List<ExportSCDO> CreateObjects(IDataReader oReader)
        {
            List<ExportSCDO> oExportSCDOs = new List<ExportSCDO>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportSCDO oItem = CreateObject(oHandler);
                oExportSCDOs.Add(oItem);
            }
            return oExportSCDOs;
        }

        #endregion

        #region Interface implementation
        public ExportSCDOService() { }        
        public ExportSCDO Get(int id, Int64 nUserId)
        {
            ExportSCDO oExportSCDO = new ExportSCDO();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportSCDODA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportSCDO = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get Export PI", e);
                #endregion
            }

            return oExportSCDO;
        }

        public ExportSCDO GetByPI(int nExportPIID, Int64 nUserID)
        {
            ExportSCDO oExportSCDO = new ExportSCDO();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportSCDODA.GetByPI(tc, nExportPIID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportSCDO = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get Export PI", e);
                #endregion
            }

            return oExportSCDO;
        }
        
        public List<ExportSCDO> Gets(Int64 nUserId)
        {
            List<ExportSCDO> oExportSCDOs = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportSCDODA.Gets(tc);
                oExportSCDOs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PI", e);
                #endregion
            }

            return oExportSCDOs;
        }
        public List<ExportSCDO> Gets(string sSQL, Int64 nUserId)
        {
            List<ExportSCDO> oExportSCDOs = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportSCDODA.Gets(tc, sSQL);
                oExportSCDOs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PI", e);
                #endregion
            }

            return oExportSCDOs;
        }

     
        
        #endregion
    }
}
