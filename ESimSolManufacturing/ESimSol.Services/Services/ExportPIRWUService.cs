using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class ExportPIRWUService : MarshalByRefObject, IExportPIRWUService
    {
        #region Private functions and declaration



        private ExportPIRWU MapObject(NullHandler oReader)
        {
            ExportPIRWU oExportPIRWU = new ExportPIRWU();
            oExportPIRWU.ExportPIDetailID = oReader.GetInt32("ExportPIDetailID");
            oExportPIRWU.FabricID = oReader.GetInt32("FabricID");
            oExportPIRWU.ExportPIID = oReader.GetInt32("ExportPIID");
            oExportPIRWU.PINo = oReader.GetString("PINo");
            oExportPIRWU.IssueDate = oReader.GetDateTime("IssueDate");
            oExportPIRWU.ContractorName = oReader.GetString("ContractorName");
            oExportPIRWU.BuyerName = oReader.GetString("BuyerName");
            oExportPIRWU.MKTPName = oReader.GetString("MKTPName");
            oExportPIRWU.ProductName = oReader.GetString("ProductName");
            oExportPIRWU.FabricNo = oReader.GetString("FabricNo");
            oExportPIRWU.PONo = oReader.GetString("FSCNo");
            oExportPIRWU.Compossion = oReader.GetString("Compossion");
            oExportPIRWU.Construction = oReader.GetString("Construction");
            oExportPIRWU.FabricTypeName = oReader.GetString("ProcessTypeName");
            oExportPIRWU.Wave = oReader.GetString("FabricWeaveName");
            oExportPIRWU.ColorInfo = oReader.GetString("ColorInfo");
            oExportPIRWU.StyleNo = oReader.GetString("StyleNo");
            oExportPIRWU.BuyerRef = oReader.GetString("BuyerRef");
            oExportPIRWU.FinishTypeName = oReader.GetString("FinishTypeName");
            oExportPIRWU.ExeNo = oReader.GetString("ExeNo");
            oExportPIRWU.Qty = oReader.GetDouble("Qty");
            oExportPIRWU.UnitPrice = oReader.GetDouble("UnitPrice");
            oExportPIRWU.Qty_PO = oReader.GetDouble("Qty_PO");
            oExportPIRWU.Qty_DO = oReader.GetDouble("Qty_DO");
            oExportPIRWU.Qty_DC = oReader.GetDouble("Qty_DC");
            oExportPIRWU.OrderSheetDetailID = oReader.GetInt32("OrderSheetDetailID");

            oExportPIRWU.SCDate = oReader.GetDateTime("SCDate");
            oExportPIRWU.ExportLCDate = oReader.GetDateTime("ExportLCDate");
            oExportPIRWU.ShipmentDate = oReader.GetDateTime("ShipmentDate");
            oExportPIRWU.ExpiryDate = oReader.GetDateTime("ExpiryDate");
            oExportPIRWU.DeliveryStartDate = oReader.GetDateTime("DeliveryStartDate");
            oExportPIRWU.DeliveryCompleteDate = oReader.GetDateTime("DeliveryCompleteDate");

            oExportPIRWU.Currency = oReader.GetString("Currency");
            oExportPIRWU.MUName = oReader.GetString("MUName");
            oExportPIRWU.ExportLCNo = oReader.GetString("ExportLCNo");
            oExportPIRWU.FabricWeave = oReader.GetString("FabricWeave");
            oExportPIRWU.PIStatus = (EnumPIStatus)oReader.GetInt32("PIStatus");
            oExportPIRWU.BankName = oReader.GetString("BankName");
            
            oExportPIRWU.ErrorMessage = "";
            return oExportPIRWU;
        }
        private ExportPIRWU CreateObject(NullHandler oReader)
        {
            ExportPIRWU oExportPIRWU = new ExportPIRWU();
            oExportPIRWU = MapObject(oReader);
            return oExportPIRWU;
        }

        private List<ExportPIRWU> CreateObjects(IDataReader oReader)
        {
            List<ExportPIRWU> oExportPIRWU = new List<ExportPIRWU>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportPIRWU oItem = CreateObject(oHandler);
                oExportPIRWU.Add(oItem);
            }
            return oExportPIRWU;
        }

        #endregion

        #region Interface implementation
        public ExportPIRWUService() { }

      

        public ExportPIRWU Get(int id, Int64 nUserId)
        {
            ExportPIRWU oExportPIRWU = new ExportPIRWU();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = ExportPIRWUDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportPIRWU = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ExportPIRWU", e);
                #endregion
            }
            return oExportPIRWU;
        }

        public List<ExportPIRWU> GetByPINo(ExportPIRWU oExportPIRWU, Int64 nUserID)
        {
            List<ExportPIRWU> oExportPIRWUs = new List<ExportPIRWU>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportPIRWUDA.GetByPINo(tc, oExportPIRWU);
                oExportPIRWUs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportPIRWUs", e);
                #endregion
            }
            return oExportPIRWUs;
        }

        public List<ExportPIRWU> Gets(Int64 nUserID)
        {
            List<ExportPIRWU> oExportPIRWUs = new List<ExportPIRWU>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportPIRWUDA.Gets(tc);
                oExportPIRWUs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportPIRWU", e);
                #endregion
            }
            return oExportPIRWUs;
        }
        public List<ExportPIRWU> Gets(string sSQL,Int64 nUserID)
        {
            List<ExportPIRWU> oExportPIRWUs = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportPIRWUDA.Gets(tc,sSQL);
                oExportPIRWUs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportPIRWU", e);
                #endregion
            }
            return oExportPIRWUs;
        }


        public List<ExportPIRWU> GetByPONo(ExportPIRWU oExportPIRWU, Int64 nUserID)
        {
            List<ExportPIRWU> oExportPIRWUs = new List<ExportPIRWU>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportPIRWUDA.GetByPONo(tc, oExportPIRWU);
                oExportPIRWUs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportPIRWUs", e);
                #endregion
            }
            return oExportPIRWUs;
        }


        public ExportPIRWU SetPO(ExportPIRWU oExportPIRWU, Int64 nUserID)
        {
            ExportPIRWU objExportPIRWU = new ExportPIRWU();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportPIRWUDA.SetPO(tc, oExportPIRWU, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    objExportPIRWU = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save PO", e);
                #endregion
            }
            return objExportPIRWU;
        }
        public ExportPIRWU RemoveDispoNo(ExportPIRWU oExportPIRWU, Int64 nUserID)
        {
            ExportPIRWU objExportPIRWU = new ExportPIRWU();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportPIRWUDA.RemoveDispoNo(tc, oExportPIRWU, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    objExportPIRWU = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save PO", e);
                #endregion
            }
            return objExportPIRWU;
        }
        #endregion
    }   
}
