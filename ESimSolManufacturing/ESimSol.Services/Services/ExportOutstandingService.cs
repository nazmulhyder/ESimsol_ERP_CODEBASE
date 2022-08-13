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
    public class ExportOutstandingService : MarshalByRefObject, IExportOutstandingService
    {
        #region Private functions and declaration
        private ExportOutstanding MapObject(NullHandler oReader)
        {
            ExportOutstanding oExportOutstanding = new ExportOutstanding();
            oExportOutstanding.OperationStage = (EnumOperationStage)oReader.GetInt32("OperationStage");
            oExportOutstanding.OperationStageInt = oReader.GetInt32("OperationStage");
            oExportOutstanding.Qty = Math.Round(oReader.GetDouble("Qty"), 2);
            oExportOutstanding.Amount = Math.Round(oReader.GetDouble("Amount"), 2);
            oExportOutstanding.Remarks = oReader.GetString("Remarks");
            oExportOutstanding.OperationStageSt = oReader.GetString("OperationStageSt");
            return oExportOutstanding;
        }
        private ExportOutstanding CreateObject(NullHandler oReader)
        {
            ExportOutstanding oExportOutstanding = new ExportOutstanding();
            oExportOutstanding = MapObject(oReader);
            return oExportOutstanding;
        }
        private List<ExportOutstanding> CreateObjects(IDataReader oReader)
        {
            List<ExportOutstanding> oExportOutstandings = new List<ExportOutstanding>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportOutstanding oItem = CreateObject(oHandler);
                oExportOutstandings.Add(oItem);
            }
            return oExportOutstandings;
        }
        #endregion

        #region Mapping for Group Wise
        private ExportOutstanding MapObjectForGroupWise(NullHandler oReader)
        {
            ExportOutstanding oExportOutstanding = new ExportOutstanding();
            oExportOutstanding.ContractorID = oReader.GetInt32("ContractorID");
            oExportOutstanding.ContractorName = oReader.GetString("ContractorName");
            oExportOutstanding.BankID = oReader.GetInt32("BankID");
            oExportOutstanding.BankName = oReader.GetString("BankName");
            oExportOutstanding.TotalPI = oReader.GetInt32("TotalPI");
            oExportOutstanding.TotalPIQty = Math.Round(oReader.GetDouble("TotalPIQty"), 2);
            oExportOutstanding.TotalPIAmount = Math.Round(oReader.GetDouble("TotalPIAmount"), 2);
            return oExportOutstanding;
        }
        private ExportOutstanding CreateObjectForGroupWise(NullHandler oReader)
        {
            ExportOutstanding oExportOutstanding = new ExportOutstanding();
            oExportOutstanding = MapObjectForGroupWise(oReader);
            return oExportOutstanding;
        }
        private List<ExportOutstanding> CreateObjectsForGroupWise(IDataReader oReader)
        {
            List<ExportOutstanding> oExportOutstandings = new List<ExportOutstanding>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportOutstanding oItem = CreateObjectForGroupWise(oHandler);
                oExportOutstandings.Add(oItem);
            }
            return oExportOutstandings;
        }
        #endregion

        #region Mapping for Detail
        private ExportOutstanding MapObjectForDetail(NullHandler oReader)
        {
            ExportOutstanding oExportOutstanding = new ExportOutstanding();
            oExportOutstanding.BUID = oReader.GetInt32("BUID");
            oExportOutstanding.ExportPIID = oReader.GetInt32("ExportPIID");
            oExportOutstanding.PINo = oReader.GetString("PINo");
            oExportOutstanding.PIDate = oReader.GetDateTime("PIDate");
            oExportOutstanding.PartyName = oReader.GetString("PartyName");
            oExportOutstanding.PIQty = Math.Round(oReader.GetDouble("PIQty"), 2);
            oExportOutstanding.PIAmount = Math.Round(oReader.GetDouble("PIAmount"), 2);
            oExportOutstanding.DOQty = Math.Round(oReader.GetDouble("DOQty"), 2);
            oExportOutstanding.ChallanQty = Math.Round(oReader.GetDouble("ChallanQty"), 2);
            oExportOutstanding.LCNo = oReader.GetString("LCNo");
            oExportOutstanding.BillNo = oReader.GetString("BillNo");
            oExportOutstanding.LDBCNo = oReader.GetString("LDBCNo");
            oExportOutstanding.MaturityDate = oReader.GetDateTime("MaturityDate");
            oExportOutstanding.ReceiveDate = oReader.GetDateTime("ReceiveDate");
            oExportOutstanding.NegoBankName = oReader.GetString("NegoBankName");
            oExportOutstanding.ContractorID = oReader.GetInt32("ContractorID");
            oExportOutstanding.BankID = oReader.GetInt32("BankID");
            return oExportOutstanding;
        }
        private ExportOutstanding CreateObjectForDetail(NullHandler oReader)
        {
            ExportOutstanding oExportOutstanding = new ExportOutstanding();
            oExportOutstanding = MapObjectForDetail(oReader);
            return oExportOutstanding;
        }
        private List<ExportOutstanding> CreateObjectsForDetail(IDataReader oReader)
        {
            List<ExportOutstanding> oExportOutstandings = new List<ExportOutstanding>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportOutstanding oItem = CreateObjectForDetail(oHandler);
                oExportOutstandings.Add(oItem);
            }
            return oExportOutstandings;
        }
        #endregion
 

        #region Interface implementation
        public ExportOutstandingService() { }

        public List<ExportOutstanding> Gets(DateTime dFromDODate, DateTime dToDODate, int nTextileUnit, Int64 nUserId)
        {
            List<ExportOutstanding> oExportOutstandings = new List<ExportOutstanding>(); ;
            ExportOutstanding oExportOutstanding= new ExportOutstanding();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportOutstandingDA.Gets(tc, dFromDODate, dToDODate, nTextileUnit);
                oExportOutstandings = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oExportOutstanding.ErrorMessage = ex.Message;
                oExportOutstandings = new List<ExportOutstanding>();
                oExportOutstandings.Add(oExportOutstanding);
                #endregion
            }
            return oExportOutstandings;
        }
        public List<ExportOutstanding> GetsListByGroup(ExportOutstanding oExportOutstanding, Int64 nUserId)
        {
            List<ExportOutstanding> oExportOutstandings = new List<ExportOutstanding>(); ;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportOutstandingDA.GetsListByGroup(tc, oExportOutstanding);
                oExportOutstandings = CreateObjectsForGroupWise(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oExportOutstanding = new ExportOutstanding();
                oExportOutstanding.ErrorMessage = ex.Message;
                oExportOutstandings = new List<ExportOutstanding>();
                oExportOutstandings.Add(oExportOutstanding);
                #endregion
            }
            return oExportOutstandings;
        }
        public List<ExportOutstanding> GetsListChallanDetail(ExportOutstanding oExportOutstanding, Int64 nUserId)
        {
            List<ExportOutstanding> oExportOutstandings = new List<ExportOutstanding>(); ;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportOutstandingDA.GetsListChallanDetail(tc, oExportOutstanding);
                oExportOutstandings = CreateObjectsForDetail(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oExportOutstanding = new ExportOutstanding();
                oExportOutstanding.ErrorMessage = ex.Message;
                oExportOutstandings = new List<ExportOutstanding>();
                oExportOutstandings.Add(oExportOutstanding);
                #endregion
            }
            return oExportOutstandings;
        }

        #endregion
    }
}
