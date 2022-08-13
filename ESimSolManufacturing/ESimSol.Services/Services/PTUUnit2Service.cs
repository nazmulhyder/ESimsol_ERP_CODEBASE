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
    public class PTUUnit2Service : MarshalByRefObject, IPTUUnit2Service
    {
        #region Private functions and declaration

        private PTUUnit2 MapObject(NullHandler oReader)
        {
            PTUUnit2 oPTUUnit2 = new PTUUnit2();
            oPTUUnit2.PTUUnit2ID = oReader.GetInt32("PTUUnit2ID");
            oPTUUnit2.PTUType = (EnumPTUType)oReader.GetInt32("PTUType");
            oPTUUnit2.PTUTypeInt = oReader.GetInt32("PTUType");
            oPTUUnit2.ExportSCID = oReader.GetInt32("ExportSCID");
            oPTUUnit2.ExportSCDetailID = oReader.GetInt32("ExportSCDetailID");
            oPTUUnit2.ProductID = oReader.GetInt32("ProductID");
            oPTUUnit2.ColorID = oReader.GetInt32("ColorID");
            oPTUUnit2.PolyMUnitID = oReader.GetInt32("PolyMUnitID");
            oPTUUnit2.ExportSCQty = oReader.GetDouble("ExportSCQty");
            oPTUUnit2.ProdOrderQty = oReader.GetDouble("ProdOrderQty");
            oPTUUnit2.GraceQty = oReader.GetDouble("GraceQty");
            oPTUUnit2.ProdPipeLineQty = oReader.GetDouble("ProdPipeLineQty");
            oPTUUnit2.SubcontractQty = oReader.GetDouble("SubcontractQty");
            oPTUUnit2.ActualFinishQty = oReader.GetDouble("ActualFinishQty");
            oPTUUnit2.RejectQty = oReader.GetDouble("RejectQty");
            oPTUUnit2.DOQty = oReader.GetDouble("DOQty");
            oPTUUnit2.ChallanQty = oReader.GetDouble("ChallanQty");
            oPTUUnit2.ReturnQty = oReader.GetDouble("ReturnQty");
            oPTUUnit2.ExportPINo = oReader.GetString("ExportPINo");
            oPTUUnit2.BuyerID = oReader.GetInt32("BuyerID");
            oPTUUnit2.BuyerName = oReader.GetString("BuyerName");
            oPTUUnit2.ProductCode = oReader.GetString("ProductCode");
            oPTUUnit2.ProductName = oReader.GetString("ProductName");
            oPTUUnit2.ColorName = oReader.GetString("ColorName");
            oPTUUnit2.UnitID = oReader.GetInt32("UnitID");
            oPTUUnit2.Measurement = oReader.GetString("Measurement");
            oPTUUnit2.UnitName = oReader.GetString("UnitName");
            oPTUUnit2.UnitSymbol = oReader.GetString("UnitSymbol");
            oPTUUnit2.ModelReferenceID = oReader.GetInt32("ModelReferenceID");
            oPTUUnit2.ProductNature = (EnumProductNature)oReader.GetInt32("ProductNature");
            oPTUUnit2.ProductNatureInInt = oReader.GetInt32("ProductNature");
            oPTUUnit2.ModelReferenceName = oReader.GetString("ModelReferenceName");
            oPTUUnit2.FinishGoodsWeight = oReader.GetDouble("FinishGoodsWeight");
            oPTUUnit2.NaliWeight = oReader.GetDouble("NaliWeight");
            oPTUUnit2.FinishGoodsUnit = oReader.GetInt32("FinishGoodsUnit");
            oPTUUnit2.WeightFor = oReader.GetInt32("WeightFor");
            oPTUUnit2.FGUSymbol = oReader.GetString("FGUSymbol");
            oPTUUnit2.YetToProductionSheeteQty = oReader.GetDouble("YetToProductionSheeteQty");
            oPTUUnit2.ContractorID = oReader.GetInt32("ContractorID");
            oPTUUnit2.ContractorName = oReader.GetString("ContractorName");
            oPTUUnit2.BUID = oReader.GetInt32("BUID");
            oPTUUnit2.ReadyStockQty = oReader.GetDouble("ReadyStockQty");
            oPTUUnit2.ProductionCapacity = oReader.GetDouble("ProductionCapacity");
            oPTUUnit2.YetToDOQty = oReader.GetDouble("YetToDOQty");
            oPTUUnit2.YetToChallanQty = oReader.GetDouble("YetToChallanQty");
            oPTUUnit2.AvialableStockQty = oReader.GetDouble("AvialableStockQty");
            oPTUUnit2.PIDate = oReader.GetDateTime("PIDate");
            oPTUUnit2.UnitPrice = oReader.GetDouble("UnitPrice");
            oPTUUnit2.RateUnit = oReader.GetInt32("RateUnit");
            oPTUUnit2.BUName = oReader.GetString("BUName");
            oPTUUnit2.SizeName = oReader.GetString("SizeName");
            oPTUUnit2.StyleNo = oReader.GetString("StyleNo");
            oPTUUnit2.CurrencyID = oReader.GetInt32("CurrencyID");
            oPTUUnit2.ConversionRate = oReader.GetDouble("ConversionRate");
            oPTUUnit2.SubContractReceiveQty = oReader.GetDouble("SubContractReceiveQty");
            oPTUUnit2.SubContractReadStockQty = oReader.GetDouble("SubContractReadStockQty");
            oPTUUnit2.ConvertionValue = oReader.GetDouble("ConvertionValue");
             return oPTUUnit2;
        }

        private PTUUnit2 CreateObject(NullHandler oReader)
        {
            PTUUnit2 oPTUUnit2 = new PTUUnit2();
            oPTUUnit2 = MapObject(oReader);
            return oPTUUnit2;
        }

        private List<PTUUnit2> CreateObjects(IDataReader oReader)
        {
            List<PTUUnit2> oPTUUnit2 = new List<PTUUnit2>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PTUUnit2 oItem = CreateObject(oHandler);
                oPTUUnit2.Add(oItem);
            }
            return oPTUUnit2;
        }

        #endregion

        #region Interface implementation
        public PTUUnit2 UpdateGrace(PTUUnit2 oPTUUnit2, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = PTUUnit2DA.UpdateGrace(tc, oPTUUnit2, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPTUUnit2 = new PTUUnit2();
                    oPTUUnit2 = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oPTUUnit2 = new PTUUnit2();
                    oPTUUnit2.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oPTUUnit2;
        }

       
        public PTUUnit2 Get(int id, Int64 nUserId)
        {
            PTUUnit2 oPTUUnit2 = new PTUUnit2();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = PTUUnit2DA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPTUUnit2 = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get PTUUnit2", e);
                #endregion
            }
            return oPTUUnit2;
        }

        public List<PTUUnit2> Gets(int nPIID, int nBUID, Int64 nUserID)
        {
            List<PTUUnit2> oPTUUnit2s = new List<PTUUnit2>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = PTUUnit2DA.Gets(tc, nPIID, nBUID);
                oPTUUnit2s = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                PTUUnit2 oPTUUnit2 = new PTUUnit2();
                oPTUUnit2.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oPTUUnit2s;
        }

        public List<PTUUnit2> WaitFoSubcontractReceivePTU(int nBUID, int nProductNature, Int64 nUserID)
        {
            List<PTUUnit2> oPTUUnit2s = new List<PTUUnit2>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = PTUUnit2DA.WaitFoSubcontractReceivePTU(tc, nBUID, nProductNature);
                oPTUUnit2s = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                PTUUnit2 oPTUUnit2 = new PTUUnit2();
                oPTUUnit2.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oPTUUnit2s;
        }
        public List<PTUUnit2> Gets(string sSQL, Int64 nUserID)
        {
            List<PTUUnit2> oPTUUnit2s = new List<PTUUnit2>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = PTUUnit2DA.Gets(tc, sSQL);
                oPTUUnit2s = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PTUUnit2", e);
                #endregion
            }
            return oPTUUnit2s;
        }

        #endregion
    }

}
