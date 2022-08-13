using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class FabricSalesContractDetailService : MarshalByRefObject, IFabricSalesContractDetailService
    {
        #region Private functions and declaration
        private FabricSalesContractDetail MapObject(NullHandler oReader)
        {
            FabricSalesContractDetail oFabricSalesContractDetail = new FabricSalesContractDetail();
            oFabricSalesContractDetail.FabricSalesContractDetailID = oReader.GetInt32("FabricSalesContractDetailID");
            //oFabricSalesContractDetail.FabricSalesContractLogID = oReader.GetInt32("FabricSalesContractLogID");
            oFabricSalesContractDetail.ProductID = oReader.GetInt32("ProductID");
            oFabricSalesContractDetail.FabricID = oReader.GetInt32("FabricID");
            oFabricSalesContractDetail.Qty = oReader.GetDouble("Qty");
            oFabricSalesContractDetail.MUnitID = oReader.GetInt32("MUnitID");
            oFabricSalesContractDetail.UnitPrice = oReader.GetDouble("UnitPrice");
            oFabricSalesContractDetail.Amount = oReader.GetDouble("Amount");
            oFabricSalesContractDetail.ProductCode = oReader.GetString("ProductCode");
            oFabricSalesContractDetail.ProductName = oReader.GetString("ProductName");
            oFabricSalesContractDetail.MUName = oReader.GetString("MUName");
            oFabricSalesContractDetail.Currency = oReader.GetString("Currency");
            oFabricSalesContractDetail.ColorInfo = oReader.GetString("ColorInfo");
            oFabricSalesContractDetail.BuyerReference = oReader.GetString("BuyerReference");
            oFabricSalesContractDetail.StyleNo = oReader.GetString("StyleNo");
            oFabricSalesContractDetail.FabricSalesContractID = oReader.GetInt32("FabricSalesContractID");
            oFabricSalesContractDetail.ExeNo = oReader.GetString("ExeNoFull");
            oFabricSalesContractDetail.Size = oReader.GetString("Size");
            oFabricSalesContractDetail.LabDipNo = oReader.GetString("LabdipNo");
            oFabricSalesContractDetail.BuyerName = oReader.GetString("BuyerName");
            oFabricSalesContractDetail.IsBWash = oReader.GetBoolean("IsBWash");
            oFabricSalesContractDetail.PantonNo = oReader.GetString("PantonNo");
            oFabricSalesContractDetail.YarnType = oReader.GetString("YarnType");
             //derive for Fabric
            //oFabricSalesContractDetail.ColorName = oReader.GetString("ColorName");
            oFabricSalesContractDetail.FabricNo = oReader.GetString("FabricNo");
            oFabricSalesContractDetail.OptionNo = oReader.GetString("OptionNo");
            oFabricSalesContractDetail.FabricWidth = oReader.GetString("FabricWidth");
            oFabricSalesContractDetail.Construction = oReader.GetString("Construction");
            oFabricSalesContractDetail.ConstructionPI = oReader.GetString("ConstructionPI");
            oFabricSalesContractDetail.ProcessType = oReader.GetInt32("ProcessType");
            oFabricSalesContractDetail.FabricWeave = oReader.GetInt32("FabricWeave");
            oFabricSalesContractDetail.FinishType = oReader.GetInt32("FinishType");
            oFabricSalesContractDetail.FabricDesignID = oReader.GetInt32("FabricDesignID");
            oFabricSalesContractDetail.FabricDesignName = oReader.GetString("FabricDesignName");
            oFabricSalesContractDetail.ProcessTypeName = oReader.GetString("ProcessTypeName");
            oFabricSalesContractDetail.FabricWeaveName = oReader.GetString("FabricWeaveName");
            oFabricSalesContractDetail.FinishTypeName = oReader.GetString("FinishTypeName");
            oFabricSalesContractDetail.Note = oReader.GetString("Note");
            oFabricSalesContractDetail.PINo = oReader.GetString("PINo");
            oFabricSalesContractDetail.HLReference = oReader.GetString("HLReference");
            oFabricSalesContractDetail.DesignPattern = oReader.GetString("DesignPattern");
            oFabricSalesContractDetail.IsProduction = oReader.GetBoolean("IsProduction");
          
            oFabricSalesContractDetail.Qty_PI = oReader.GetDouble("Qty_PI");
            oFabricSalesContractDetail.Status = (EnumPOState)oReader.GetInt16("Status");
            oFabricSalesContractDetail.SCDetailTypeInt = oReader.GetInt32("SCDetailType");
            oFabricSalesContractDetail.SCDetailType = (EnumSCDetailType)oReader.GetInt16("SCDetailType");
            oFabricSalesContractDetail.FNLabdipDetailID = oReader.GetInt32("FNLabdipDetailID");
            oFabricSalesContractDetail.ShadeID = oReader.GetInt32("ShadeID");
            oFabricSalesContractDetail.LDNo = oReader.GetString("LDNo");
            oFabricSalesContractDetail.Shrinkage = oReader.GetString("Shrinkage");
            oFabricSalesContractDetail.Weight = oReader.GetString("Weight");
            oFabricSalesContractDetail.SLNo = oReader.GetInt32("SLNo");
            //oFabricSalesContractDetail.IsDeduct = oReader.GetBoolean("IsDeduct");
            oFabricSalesContractDetail.SCNo = oReader.GetString("SCNo");
            //oFabricSalesContractDetail.DispoNo = oReader.GetString("DispoNo");
            oFabricSalesContractDetail.FabricID = oReader.GetInt32("FabricID");
            oFabricSalesContractDetail.RawFabricRcvQty = oReader.GetDouble("RawFabricRcvQty");
            oFabricSalesContractDetail.OrderQty = oReader.GetDouble("OrderQty");
            oFabricSalesContractDetail.PlannedQty = oReader.GetDouble("PlannedQty");
            oFabricSalesContractDetail.BatchQty = oReader.GetDouble("BatchQty");
            oFabricSalesContractDetail.DeliveredQty = oReader.GetDouble("DeliveredQty");
            oFabricSalesContractDetail.Balance = oReader.GetDouble("Balance");
            oFabricSalesContractDetail.ProcessType = oReader.GetInt32("ProcessType");
            //oFabricSalesContractDetail.DispoQty = oReader.GetDouble("DispoQty");
            oFabricSalesContractDetail.SCNoFull = oReader.GetString("SCNoFull");
            oFabricSalesContractDetail.ShadeCount = oReader.GetInt16("ShadeCount");
            oFabricSalesContractDetail.PantonNo = oReader.GetString("PantonNo");
            oFabricSalesContractDetail.Code = oReader.GetString("Code");
            oFabricSalesContractDetail.ExeNoFull = oReader.GetString("ExeNoFull");
            oFabricSalesContractDetail.ContractorName = oReader.GetString("ContractorName");
            
            return oFabricSalesContractDetail;
        }
        private FabricSalesContractDetail CreateObject(NullHandler oReader)
        {
            FabricSalesContractDetail oFabricSalesContractDetail = new FabricSalesContractDetail();
            oFabricSalesContractDetail = MapObject(oReader);
            return oFabricSalesContractDetail;
        }      
        private List<FabricSalesContractDetail> CreateObjects(IDataReader oReader)
        {
            List<FabricSalesContractDetail> oFabricSalesContractDetails = new List<FabricSalesContractDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricSalesContractDetail oItem = CreateObject(oHandler);
                oFabricSalesContractDetails.Add(oItem);
            }
            return oFabricSalesContractDetails;
        }
        #endregion

        #region Interface implementation
        public FabricSalesContractDetailService() { }
      
        public FabricSalesContractDetail Get(int id, Int64 nUserId)
        {
            FabricSalesContractDetail oFabricSalesContractDetail = new FabricSalesContractDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricSalesContractDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricSalesContractDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FabricSalesContractDetail", e);
                #endregion
            }

            return oFabricSalesContractDetail;
        }
        public List<FabricSalesContractDetail> Gets(int nFabricSalesContractID, Int64 nUserId)
        {
            List<FabricSalesContractDetail> oFabricSalesContractDetails = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricSalesContractDetailDA.Gets(tc, nFabricSalesContractID);
                oFabricSalesContractDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PIDetail", e);
                #endregion
            }

            return oFabricSalesContractDetails;
        }
        public List<FabricSalesContractDetail> Gets(Int64 nUserId)
        {
            List<FabricSalesContractDetail> oFabricSalesContractDetail = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricSalesContractDetailDA.Gets(tc);
                oFabricSalesContractDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PIDetail", e);
                #endregion
            }

            return oFabricSalesContractDetail;
        }
        public List<FabricSalesContractDetail> Gets(string sSQL, Int64 nUserId)
        {
            List<FabricSalesContractDetail> oFabricSalesContractDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricSalesContractDetailDA.Gets(tc, sSQL);
                oFabricSalesContractDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricSalesContractDetail", e);
                #endregion
            }

            return oFabricSalesContractDetail;
        }
        public List<FabricSalesContractDetail> GetsReport(string sSQL, Int64 nUserId)
        {
            List<FabricSalesContractDetail> oFabricSalesContractDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricSalesContractDetailDA.GetsReport(tc, sSQL);
                oFabricSalesContractDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricSalesContractDetail", e);
                #endregion
            }

            return oFabricSalesContractDetail;
        }
        public List<FabricSalesContractDetail> GetsLog(int nFabricSalesContractLogID, Int64 nUserId)
        {
            List<FabricSalesContractDetail> oFabricSalesContractDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricSalesContractDetailDA.GetsLog(tc, nFabricSalesContractLogID);
                oFabricSalesContractDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PIDetails", e);
                #endregion
            }

            return oFabricSalesContractDetails;
        }
        public List<FabricSalesContractDetail> Save_UpdateDispoNo(List<FabricSalesContractDetail> oFabricSalesContractDetails, Int64 nUserID)
        {
            FabricSalesContractDetail oFabricSalesContractDetail = new FabricSalesContractDetail();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                foreach (FabricSalesContractDetail oitem in oFabricSalesContractDetails)
                {
                    FabricSalesContractDetailDA.Save_UpdateDispoNo(tc, oitem);
                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricSalesContractDetails =new List<FabricSalesContractDetail>();
               
                #endregion
            }

            return oFabricSalesContractDetails;

        }

        public List<FabricSalesContractDetail> SetFabricExcNo(List<FabricSalesContractDetail> oFabricSalesContractDetails, Int64 nUserID)
        {
            List<FabricSalesContractDetail> oFSCDs = new List<FabricSalesContractDetail>();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader = null;
                foreach (FabricSalesContractDetail oitem in oFabricSalesContractDetails)
                {
                    if (oitem.ExeNo.Trim() != "")
                    {
                        reader = FabricSalesContractDetailDA.SetHandLoomNo(tc, oitem.FabricSalesContractDetailID, oitem.ExeNo, oitem.OptionNo);
                        NullHandler oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oFSCDs.Add(CreateObject(oReader));
                        }
                        reader.Close();
                    }
                    else
                    {
                        oFSCDs.Add(oitem);
                    }
                   
                    
                }
               
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                var oFSCD = new FabricSalesContractDetail();
                oFSCD.ErrorMessage = (e.Message.Contains('~') ? e.Message.Split('~')[0] : e.Message);
                throw new Exception(oFSCD.ErrorMessage);
                #endregion
            }

            return oFSCDs;

        }
        public List<FabricSalesContractDetail> SetFabricExcNo(FabricSalesContract oFabricSalesContract, Int64 nUserID)
        {
            List<FabricSalesContractDetail> oFSCDs = new List<FabricSalesContractDetail>();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                List<FabricSalesContractDetail> oFabricSalesContractDetails = new List<FabricSalesContractDetail>();
                oFabricSalesContractDetails = oFabricSalesContract.FabricSalesContractDetails;
                foreach (FabricSalesContractDetail oitem in oFabricSalesContractDetails)
                {
                    if (!String.IsNullOrEmpty(oitem.ExeNo))
                    {
                        FabricSalesContractDetailDA.Save_UpdateDispoNo(tc, oitem);
                    }
                }
                reader = null;
                //reader = FabricSalesContractDetailDA.SetFabricExcNo(tc, oFabricSalesContract.FabricSalesContractID, 0,oFabricSalesContract.ErrorMessage, nUserID);
                oFSCDs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                var oFSCD = new FabricSalesContractDetail();
                oFSCD.ErrorMessage = (e.Message.Contains('~') ? e.Message.Split('~')[0] : e.Message);
                throw new Exception(oFSCD.ErrorMessage);
                #endregion
            }

            return oFSCDs;

        }
        public FabricSalesContractDetail SaveProcess(FabricSalesContractDetail oFabricSalesContractDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<FNExecutionOrderProcess> oFNExecutionOrderProcessList = new List<FNExecutionOrderProcess>();
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                oFNExecutionOrderProcessList = oFabricSalesContractDetail.FNExecutionOrderProcessList;
                foreach (FNExecutionOrderProcess oItem in oFNExecutionOrderProcessList)
                {
                    oItem.FNExOID = oFabricSalesContractDetail.FabricSalesContractDetailID;
                    if (oItem.FNExOProcessID <= 0)
                    {
                        reader = FNExecutionOrderProcessDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        reader = FNExecutionOrderProcessDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {

                    }
                    reader.Close();
                }
                reader = FNExecutionOrderProcessDA.Gets(oFabricSalesContractDetail.FabricSalesContractDetailID, tc);
                FNExecutionOrderProcessService oFNExecutionOrderProcessService = new FNExecutionOrderProcessService();
                oFabricSalesContractDetail.FNExecutionOrderProcessList = oFNExecutionOrderProcessService.CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oFabricSalesContractDetail = new FabricSalesContractDetail();
                    oFabricSalesContractDetail.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oFabricSalesContractDetail;
        }
        public FabricSalesContractDetail OrderHold(FabricSalesContractDetail oFabricSalesContractDetail, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricSalesContractDetailDA.OrderHold(tc, oFabricSalesContractDetail, EnumDBOperation.Insert, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricSalesContractDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oFabricSalesContractDetail = new FabricSalesContractDetail();
                oFabricSalesContractDetail.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricSalesContractDetail;
        }
        public List<FabricSalesContractDetail> SetHandLoomNo(List<FabricSalesContractDetail> oFabricSalesContractDetails, Int64 nUserID)
        {
            List<FabricSalesContractDetail> oFSCDs = new List<FabricSalesContractDetail>();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader = null;
                foreach (FabricSalesContractDetail oitem in oFabricSalesContractDetails)
                {
                    if (oitem.ExeNo.Trim() != "")
                    {
                        reader = FabricSalesContractDetailDA.SetHandLoomNo(tc, oitem.FabricSalesContractDetailID, oitem.ExeNo, oitem.OptionNo);
                        NullHandler oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oFSCDs.Add(CreateObject(oReader));
                        }
                        reader.Close();
                    }          
                    else
                    {
                        oFSCDs.Add(oitem);
                    }
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                var oFSCD = new FabricSalesContractDetail();
                oFSCD.ErrorMessage = (e.Message.Contains('~') ? e.Message.Split('~')[0] : e.Message);
                throw new Exception(oFSCD.ErrorMessage);
                #endregion
            }

            return oFSCDs;

        }
        public FabricSalesContractDetail SaveLDNo(FabricSalesContractDetail oFabricSalesContractDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = FabricSalesContractDetailDA.SaveLDNo(tc, oFabricSalesContractDetail);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricSalesContractDetail = new FabricSalesContractDetail();
                    oFabricSalesContractDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save FabricSalesContractDetail. Because of " + e.Message, e);
                #endregion
            }
            return oFabricSalesContractDetail;
        }

        public FabricSalesContractDetail UpdateStatus(FabricSalesContractDetail oFabricSalesContractDetail, Int64 nUserId)
        {
            FabricSalesContractDetail oFSCD = new FabricSalesContractDetail();
            oFSCD = oFabricSalesContractDetail;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                #region History
                FabricSCHistory oFabricSCHistory = new FabricSCHistory();
                oFabricSCHistory.FabricSCHistoryID = 0;
                oFabricSCHistory.FabricSCID = oFSCD.FabricSalesContractID;
                oFabricSCHistory.FabricSCDetailID = oFSCD.FabricSalesContractDetailID;
                oFabricSCHistory.FSCDStatus = oFSCD.Status;
                oFabricSCHistory.FSCDStatus_Prv = oFSCD.PreviousStatus;
                IDataReader readerdetail;
                readerdetail = FabricSCHistoryDA.InsertUpdate(tc, oFabricSCHistory, EnumDBOperation.Insert, (int)nUserId);
                readerdetail.Close();
                #endregion

                IDataReader reader = FabricSalesContractDetailDA.UpdateStatus(tc, oFabricSalesContractDetail);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricSalesContractDetail = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oFabricSalesContractDetail = new FabricSalesContractDetail();
                oFabricSalesContractDetail.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricSalesContractDetail;
        }

      
        #endregion
    }
}
