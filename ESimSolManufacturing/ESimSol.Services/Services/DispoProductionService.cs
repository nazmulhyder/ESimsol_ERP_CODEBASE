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
    public class DispoProductionService : MarshalByRefObject, IDispoProductionService
    {
        #region Private functions and declaration
        private DispoProduction MapObject(NullHandler oReader)
        {
            DispoProduction oPIReport = new DispoProduction();

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
            oPIReport.IsInHouse = oReader.GetBoolean("IsInHouse");
            oPIReport.LDNo = oReader.GetString("LDNo");
            oPIReport.LCTermID = oReader.GetInt32("LCTermID");
            oPIReport.Note = oReader.GetString("Note");
            oPIReport.EndUse = oReader.GetString("EndUse");
            oPIReport.QualityParameters = oReader.GetString("QualityParameters");
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
            oPIReport.ProductID = oReader.GetInt32("ProductID");
            oPIReport.FabricID = oReader.GetInt32("FabricID");
            oPIReport.StockInHand = oReader.GetDouble("StockInHand");
            oPIReport.MUnitID = oReader.GetInt32("MUnitID");
            oPIReport.UnitPrice = oReader.GetDouble("UnitPrice");

            oPIReport.Amount = oReader.GetDouble("Amount");
            oPIReport.ProductCode = oReader.GetString("ProductCode");
            oPIReport.ProductName = oReader.GetString("ProductName");
            oPIReport.MUName = oReader.GetString("MUName");
            oPIReport.ExeNo = oReader.GetString("ExeNo");
            oPIReport.Currency = oReader.GetString("Currency");
            oPIReport.ColorInfo = oReader.GetString("ColorInfo");
            oPIReport.BuyerReference = oReader.GetString("BuyerReference");
            oPIReport.StyleNo = oReader.GetString("StyleNo");
            oPIReport.FabricSalesContractID = oReader.GetInt32("FabricSalesContractID");
            oPIReport.FEOSID = oReader.GetInt32("FEOSID");
            oPIReport.LabDipID = oReader.GetInt32("LabDipID");
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
            oPIReport.LabStatus = (EnumFabricLabStatus)oReader.GetInt32("LabStatus");
            oPIReport.Status = (EnumPOState)oReader.GetInt32("Status");
            oPIReport.PreapeByID = oReader.GetInt32("Status");
            oPIReport.FNLabdipDetailID = oReader.GetInt32("FNLabdipDetailID");
            oPIReport.DeliveryDate_PP = oReader.GetDateTime("DeliveryDate_PP");
            oPIReport.DeliveryDate_Full = oReader.GetDateTime("DeliveryDate_Full");
            oPIReport.Code = oReader.GetString("Code");

            oPIReport.Qty = oReader.GetDouble("Qty");
            oPIReport.QtyDispo = oReader.GetDouble("QtyDispo");
            oPIReport.QtyWarp = oReader.GetDouble("QtyWarp");
            oPIReport.QtySizing = oReader.GetDouble("QtySizing");
            oPIReport.QtyWeaving = oReader.GetDouble("QtyWeaving");
            oPIReport.GreyRecd = oReader.GetDouble("GreyRecd");
            oPIReport.StoreRcvQty = oReader.GetDouble("StoreRcvQty");
            oPIReport.DCQty = oReader.GetDouble("DCQty");
            oPIReport.RCQty = oReader.GetDouble("RCQty");
            oPIReport.StockInHand = oReader.GetDouble("StockInHand");

            return oPIReport;
        }

        private DispoProduction CreateObject(NullHandler oReader)
        {
            DispoProduction oPIReport = new DispoProduction();
            oPIReport = MapObject(oReader);
            return oPIReport;
        }

        private List<DispoProduction> CreateObjects(IDataReader oReader)
        {
            List<DispoProduction> oPIReport = new List<DispoProduction>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DispoProduction oItem = CreateObject(oHandler);
                oPIReport.Add(oItem);
            }
            return oPIReport;
        }

        #endregion

        #region Interface implementation
        public DispoProductionService() { }

        public List<DispoProduction> Gets(string sSQL, Int64 nUserID)
        {
            List<DispoProduction> oPIReport = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = DispoProductionDA.Gets(tc, sSQL);
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
        public List<DispoProduction> UpdateMail(DispoProduction oDispoProduction, Int64 nUserID)
        {
            List<DispoProduction> oPIReport = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = DispoProductionDA.UpdateMail(tc, oDispoProduction, nUserID);
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
        public List<DispoProduction> Received(List<DispoProduction> oDispoProductions, Int64 nUserID)
        {
            DispoProduction oDispoProduction = new DispoProduction();
            List<DispoProduction> oFabrics_Return = new List<DispoProduction>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                foreach (DispoProduction oItem in oDispoProductions)
                {
                    ////if (string.IsNullOrEmpty(oItem.HandLoomNo)) { oItem.HandLoomNo = ""; }
                    //FNLabDipDetailDA.Save_LDNo_FromFabric(tc, oItem, oItem.HandLoomNo, nUserID);
                    IDataReader reader = DispoProductionDA.SetFabricExcNo(tc, oItem, nUserID);
                    //IDataReader reader = FabricDA.Received(tc, oItem, nUserID);
                    //IDataReader reader = FabricDA.Get(tc, oItem.FabricID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oDispoProduction = new DispoProduction();
                        oDispoProduction = CreateObject(oReader);
                        oFabrics_Return.Add(oDispoProduction);
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
                oFabrics_Return = new List<DispoProduction>();
                oDispoProduction = new DispoProduction();
                oDispoProduction.ErrorMessage = e.Message.Split('~')[0];
                oFabrics_Return.Add(oDispoProduction);

                #endregion
            }
            return oFabrics_Return;
        }
        public DispoProduction SaveExcNo(DispoProduction oDispoProduction, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                //foreach (DispoProduction oItem in oDispoProductions)
                //{
                //    ////if (string.IsNullOrEmpty(oItem.HandLoomNo)) { oItem.HandLoomNo = ""; }
                //    //FNLabDipDetailDA.Save_LDNo_FromFabric(tc, oItem, oItem.HandLoomNo, nUserID);
                //    IDataReader reader = FabricSalesContractDetailDA.SetFabricExcNo(tc, oItem, nUserID);
                //    //IDataReader reader = FabricDA.Received(tc, oItem, nUserID);
                //    //IDataReader reader = FabricDA.Get(tc, oItem.FabricID);
                //    NullHandler oReader = new NullHandler(reader);
                //    if (reader.Read())
                //    {
                //        oDispoProduction = new DispoProduction();
                //        oDispoProduction = CreateObject(oReader);
                //        oFabrics_Return.Add(oDispoProduction);
                //    }
                //    reader.Close();
                //}
                IDataReader reader = DispoProductionDA.SetFabricExcNo(tc, oDispoProduction, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDispoProduction = CreateObject(oReader);
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
                oDispoProduction.ErrorMessage = e.Message;
                #endregion
            }

            return oDispoProduction;
        }
        public DispoProduction OperationLab(DispoProduction oDispoProduction, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader = DispoProductionDA.OperationLab(tc, oDispoProduction, eEnumDBOperation, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDispoProduction = CreateObject(oReader);
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
                oDispoProduction.ErrorMessage = e.Message;
                #endregion
            }

            return oDispoProduction;
        }
        public DispoProduction CreateLab(DispoProduction oDispoProduction, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader = DispoProductionDA.CreateLab(tc, oDispoProduction, eEnumDBOperation, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDispoProduction = CreateObject(oReader);
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
                oDispoProduction.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oDispoProduction;
        }
        public DispoProduction Get(int id, Int64 nUserId)
        {
            DispoProduction oDispoProduction = new DispoProduction();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DispoProductionDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDispoProduction = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get DispoProduction", e);
                #endregion
            }

            return oDispoProduction;
        }
        public DispoProduction SaveExtra(FabricSalesContractDetail oFabricSalesContractDetail, EnumDBOperation oDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            DispoProduction oDispoProduction = new DispoProduction();
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = FabricSalesContractDetailDA.InsertUpdateExtra(tc, oFabricSalesContractDetail, oDBOperation, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDispoProduction = new DispoProduction();
                    oDispoProduction = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oDispoProduction = new DispoProduction();
                oDispoProduction.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oDispoProduction;
        }
        #endregion
    }
}