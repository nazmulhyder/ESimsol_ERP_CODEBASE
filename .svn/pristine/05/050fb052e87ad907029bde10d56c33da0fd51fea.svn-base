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
    public class FabricSCReportService : MarshalByRefObject, IFabricSCReportService
    {
        #region Private functions and declaration
        private FabricSCReport MapObject(NullHandler oReader)
        {
            FabricSCReport oPIReport = new FabricSCReport();

            oPIReport.PaymentType = (EnumPIPaymentType)oReader.GetInt32("PaymentType");
            oPIReport.OrderType = oReader.GetInt32("OrderType");
            //oPIReport.SCNoFull = oReader.GetString("SCNo");     // already mapped
            oPIReport.CurrentStatus = (EnumFabricPOStatus)oReader.GetInt32("CurrentStatus");
            oPIReport.SCDate = oReader.GetDateTime("SCDate");
            oPIReport.SLNo = oReader.GetInt32("SLNo");
            oPIReport.ContractorID = oReader.GetInt32("ContractorID");
            oPIReport.BuyerID = oReader.GetInt32("BuyerID");
            oPIReport.MktAccountID = oReader.GetInt32("MktAccountID");
            oPIReport.CurrencyID = oReader.GetInt32("CurrencyID");
            oPIReport.Amount = oReader.GetDouble("Amount");
            oPIReport.Qty = oReader.GetDouble("Qty");
            oPIReport.Qty_PI = oReader.GetDouble("Qty_PI");
            oPIReport.IsInHouse = oReader.GetBoolean("IsInHouse");
            oPIReport.LDNo = oReader.GetString("LDNo");
            oPIReport.LCTermID = oReader.GetInt32("LCTermID");
            oPIReport.Note = oReader.GetString("Note");
            oPIReport.EndUse = oReader.GetString("EndUse");
            oPIReport.QualityParameters = oReader.GetString("QualityParameters");
            oPIReport.QtyTolarance = oReader.GetString("QtyTolarance");
            //oPIReport.PaymentInstruction = oReader.GetInt32("PaymentInstruction");
            oPIReport.ApprovedDate = oReader.GetDateTime("ApproveDate");

            oPIReport.ContactPersonnelID = oReader.GetInt32("ContactPersonnelID");
            oPIReport.LightSourceID = oReader.GetInt32("LightSourceID");
            oPIReport.LightSourceIDTwo = oReader.GetInt32("LightSourceIDTwo");
            oPIReport.ContractorName = oReader.GetString("ContractorName");
            oPIReport.BuyerName = oReader.GetString("BuyerName");
            oPIReport.ApproveByName = oReader.GetString("ApproveByName");
            oPIReport.PreapeByName = oReader.GetString("PreapeByName");
            oPIReport.LightSourceName = oReader.GetString("LightSourceName");
            oPIReport.LightSourceNameTwo = oReader.GetString("LightSourceNameTwo");
            oPIReport.LCTermsName = oReader.GetString("LCTermsName");
            oPIReport.ContractorAddress = oReader.GetString("ContractorAddress");
            oPIReport.BuyerAddress = oReader.GetString("BuyerAddress");
            //oPIReport.ContractorFax = oReader.GetString("ContractorFax");
            //oPIReport.ContractorEmail = oReader.GetString("ContractorEmail");
            oPIReport.MKTPName = oReader.GetString("MKTPName");
            oPIReport.MKTGroup = oReader.GetString("MKTGroup");
            oPIReport.Currency = oReader.GetString("Currency");
            oPIReport.BUID = oReader.GetInt32("BUID");
            oPIReport.ReviseNo = oReader.GetInt32("ReviseNo");
            oPIReport.SCNoFull = oReader.GetString("SCNoFull");
            oPIReport.GarmentWash = oReader.GetString("GarmentWash");
            oPIReport.BuyerCPName = oReader.GetString("BuyerCPName");
            oPIReport.FabricReceiveDate = oReader.GetDateTime("FabricReceiveDate");
            oPIReport.FabricReceiveBy = oReader.GetInt32("FabricReceiveBy");
            oPIReport.IsPrint = oReader.GetBoolean("IsPrint");
            
            ////-/// Detail -///

            oPIReport.FabricSalesContractDetailID = oReader.GetInt32("FabricSalesContractDetailID");
            //oPIReport.FabricSalesContractLogID = oReader.GetInt32("FabricSalesContractLogID");
            oPIReport.ProductID = oReader.GetInt32("ProductID");
            oPIReport.FabricID = oReader.GetInt32("FabricID");
            oPIReport.Qty = oReader.GetDouble("Qty");
            oPIReport.Qty_DO = oReader.GetDouble("Qty_DO");
            oPIReport.Qty_DC = oReader.GetDouble("Qty_DC");
            oPIReport.Qty_PI = oReader.GetDouble("Qty_PI");
            oPIReport.Qty_PRO = oReader.GetDouble("Qty_PRO");
            oPIReport.StockInHand = oReader.GetDouble("StockInHand");
            oPIReport.MUnitID = oReader.GetInt32("MUnitID");
            oPIReport.UnitPrice = oReader.GetDouble("UnitPrice");

            oPIReport.Amount = oReader.GetDouble("Amount");
            oPIReport.ProductCode = oReader.GetString("ProductCode");
            oPIReport.ProductName = oReader.GetString("ProductName");
            oPIReport.MUName = oReader.GetString("MUName");
            oPIReport.ExeNo = oReader.GetString("ExeNoFull");
            oPIReport.Currency = oReader.GetString("Currency");
            oPIReport.ColorInfo = oReader.GetString("ColorInfo");
            oPIReport.BuyerReference = oReader.GetString("BuyerReference");
            oPIReport.StyleNo = oReader.GetString("StyleNo");
            oPIReport.FabricSalesContractID = oReader.GetInt32("FabricSalesContractID");
            oPIReport.LabDipID = oReader.GetInt32("LabDipID");
            //oPIReport.ExportPIID = oReader.GetInt32("ExportPIID");

            oPIReport.Size = oReader.GetString("Size");
            //derive for Fabric
            oPIReport.FabricNo = oReader.GetString("FabricNo");
            oPIReport.FabricWidth = oReader.GetString("FabricWidth");
            oPIReport.Construction = oReader.GetString("Construction");
            oPIReport.ConstructionPI = oReader.GetString("ConstructionPI");
            oPIReport.HandLoomNo = oReader.GetString("HandLoomNo");
            oPIReport.OptionNo = oReader.GetString("OptionNo");
            oPIReport.NoOfFrame = oReader.GetInt32("NoOfFrame");
            oPIReport.WeftColor = oReader.GetString("WeftColor");
            oPIReport.SubmissionDate = oReader.GetDateTime("SubmissionDate");

            oPIReport.ProcessType = oReader.GetInt32("ProcessType");
            oPIReport.FabricWeave = oReader.GetInt32("FabricWeave");
            oPIReport.FinishType = oReader.GetInt32("FinishType");
            oPIReport.FabricDesignID = oReader.GetInt32("FabricDesignID");
            oPIReport.FabricDesignName = oReader.GetString("FabricDesignName");
            oPIReport.ProcessTypeName = oReader.GetString("ProcessTypeName");
            oPIReport.FabricWeaveName = oReader.GetString("FabricWeaveName");
            oPIReport.FinishTypeName = oReader.GetString("FinishTypeName");
            oPIReport.Weight = oReader.GetString("Weight");
            oPIReport.Shrinkage = oReader.GetString("Shrinkage");
           
            oPIReport.HLReference = oReader.GetString("HLReference");
            oPIReport.DesignPattern = oReader.GetString("DesignPattern");

            oPIReport.FabricReceiveByName = oReader.GetString("FabricReceiveByName");
            oPIReport.PINo = oReader.GetString("PINo");
            oPIReport.LabDipNo = oReader.GetString("LabDipNo");
            //oPIReport.LCNo = oReader.GetString("LCNo");
            oPIReport.LabStatus = (EnumFabricLabStatus)oReader.GetInt32("LabStatus");
            oPIReport.Status = (EnumPOState)oReader.GetInt32("Status");
            oPIReport.PreapeByID = oReader.GetInt32("Status");
            oPIReport.FNLabdipDetailID = oReader.GetInt32("FNLabdipDetailID");
            //oPIReport.PIStatus = (EnumPIStatus)oReader.GetInt32("PIStatus");
            //oPIReport.PIDate = oReader.GetDateTime("PIDate");
            oPIReport.DeliveryDate_PP = oReader.GetDateTime("DeliveryDate_PP");
            oPIReport.DeliveryDate_Full = oReader.GetDateTime("DeliveryDate_Full");
            oPIReport.Code = oReader.GetString("Code");
            
            return oPIReport;
        }

        private FabricSCReport CreateObject(NullHandler oReader)
        {
            FabricSCReport oPIReport = new FabricSCReport();
            oPIReport = MapObject(oReader);
            return oPIReport;
        }

        private List<FabricSCReport> CreateObjects(IDataReader oReader)
        {
            List<FabricSCReport> oPIReport = new List<FabricSCReport>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricSCReport oItem = CreateObject(oHandler);
                oPIReport.Add(oItem);
            }
            return oPIReport;
        }

        #endregion

        #region Interface implementation
        public FabricSCReportService() { }

        public List<FabricSCReport> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricSCReport> oPIReport = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricSCReportDA.Gets(tc, sSQL);
                oPIReport = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get vwForABP", e);
                #endregion
            }

            return oPIReport;
        }
        public List<FabricSCReport> UpdateMail(FabricSCReport oFabricSCReport, Int64 nUserID)
        {
            List<FabricSCReport> oPIReport = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricSCReportDA.UpdateMail(tc, oFabricSCReport, nUserID);
                oPIReport = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Update Mail !", e);
                #endregion
            }

            return oPIReport;
        }
        public List<FabricSCReport> Received(List<FabricSCReport> oFabricSCReports, Int64 nUserID)
        {
            FabricSCReport oFabricSCReport = new FabricSCReport();
            List<FabricSCReport> oFabrics_Return = new List<FabricSCReport>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                foreach (FabricSCReport oItem in oFabricSCReports)
                {
                    ////if (string.IsNullOrEmpty(oItem.HandLoomNo)) { oItem.HandLoomNo = ""; }
                    //FNLabDipDetailDA.Save_LDNo_FromFabric(tc, oItem, oItem.HandLoomNo, nUserID);
                    IDataReader reader = FabricSCReportDA.SetFabricExcNo(tc, oItem, nUserID);
                    //IDataReader reader = FabricDA.Received(tc, oItem, nUserID);
                    //IDataReader reader = FabricDA.Get(tc, oItem.FabricID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFabricSCReport = new FabricSCReport();
                        oFabricSCReport = CreateObject(oReader);
                        oFabrics_Return.Add(oFabricSCReport);
                    }
                    reader.Close();
                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabrics_Return = new List<FabricSCReport>();
                oFabricSCReport = new FabricSCReport();
                oFabricSCReport.ErrorMessage = e.Message.Split('~')[0];
                oFabrics_Return.Add(oFabricSCReport);

                #endregion
            }
            return oFabrics_Return;
        }
        public FabricSCReport SaveExcNo(FabricSCReport oFabricSCReport, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                //foreach (FabricSCReport oItem in oFabricSCReports)
                //{
                //    ////if (string.IsNullOrEmpty(oItem.HandLoomNo)) { oItem.HandLoomNo = ""; }
                //    //FNLabDipDetailDA.Save_LDNo_FromFabric(tc, oItem, oItem.HandLoomNo, nUserID);
                //    IDataReader reader = FabricSalesContractDetailDA.SetFabricExcNo(tc, oItem, nUserID);
                //    //IDataReader reader = FabricDA.Received(tc, oItem, nUserID);
                //    //IDataReader reader = FabricDA.Get(tc, oItem.FabricID);
                //    NullHandler oReader = new NullHandler(reader);
                //    if (reader.Read())
                //    {
                //        oFabricSCReport = new FabricSCReport();
                //        oFabricSCReport = CreateObject(oReader);
                //        oFabrics_Return.Add(oFabricSCReport);
                //    }
                //    reader.Close();
                //}
                IDataReader reader = FabricSCReportDA.SetFabricExcNo(tc, oFabricSCReport, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricSCReport = CreateObject(oReader);
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
                oFabricSCReport.ErrorMessage = e.Message;
                #endregion
            }

            return oFabricSCReport;
        }
        public FabricSCReport OperationLab(FabricSCReport oFabricSCReport, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader = FabricSCReportDA.OperationLab(tc, oFabricSCReport, eEnumDBOperation, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricSCReport = CreateObject(oReader);
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
                oFabricSCReport.ErrorMessage = e.Message;
                #endregion
            }

            return oFabricSCReport;
        }
        public FabricSCReport CreateLab(FabricSCReport oFabricSCReport, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader = FabricSCReportDA.CreateLab(tc, oFabricSCReport, eEnumDBOperation, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricSCReport = CreateObject(oReader);
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
                oFabricSCReport.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oFabricSCReport;
        }
        public FabricSCReport Get(int id, Int64 nUserId)
        {
            FabricSCReport oFabricSCReport = new FabricSCReport();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricSCReportDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricSCReport = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FabricSCReport", e);
                #endregion
            }

            return oFabricSCReport;
        }
        public FabricSCReport SaveExtra(FabricSalesContractDetail oFabricSalesContractDetail, EnumDBOperation oDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            FabricSCReport oFabricSCReport = new FabricSCReport();
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = FabricSalesContractDetailDA.InsertUpdateExtra(tc, oFabricSalesContractDetail, oDBOperation, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricSCReport = new FabricSCReport();
                    oFabricSCReport = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oFabricSCReport = new FabricSCReport();
                oFabricSCReport.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricSCReport;
        }
        #endregion
    }
}