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
    public class ExportSCDetailService : MarshalByRefObject, IExportSCDetailService
    {
        #region Private functions and declaration
        private ExportSCDetail MapObject(NullHandler oReader)
        {
            ExportSCDetail oExportSCDetail = new ExportSCDetail();
            oExportSCDetail.ExportSCDetailID = oReader.GetInt32("ExportSCDetailID");
            oExportSCDetail.ExportPIDetailID = oReader.GetInt32("ExportPIDetailID");
            oExportSCDetail.ExportSCID = oReader.GetInt32("ExportSCID");
            oExportSCDetail.ProductID = oReader.GetInt32("ProductID");
            oExportSCDetail.Qty = oReader.GetDouble("Qty");
            oExportSCDetail.MUnitID = oReader.GetInt32("MUnitID");
            oExportSCDetail.UnitPrice = oReader.GetDouble("UnitPrice");
            oExportSCDetail.Amount = oReader.GetDouble("Amount");
            oExportSCDetail.ProductCode = oReader.GetString("ProductCode");
            oExportSCDetail.ProductName = oReader.GetString("ProductName");
            oExportSCDetail.MUName = oReader.GetString("MUName");
            oExportSCDetail.ProductionType = oReader.GetInt32("ProductionType");
            
            oExportSCDetail.ColorInfo = oReader.GetString("ColorInfo");
            oExportSCDetail.BuyerRef = oReader.GetString("BuyerRef");
            oExportSCDetail.StyleNo = oReader.GetString("StyleNo");
            oExportSCDetail.POQty = oReader.GetDouble("POQty");
            oExportSCDetail.DOQty = oReader.GetDouble("DOQty");

            oExportSCDetail.ColorID = oReader.GetInt32("ColorID");
            oExportSCDetail.ModelReferenceID = oReader.GetInt32("ModelReferenceID");
            oExportSCDetail.OrderSheetDetailID = oReader.GetInt32("OrderSheetDetailID");
            oExportSCDetail.Measurement = oReader.GetString("Measurement");
            oExportSCDetail.PolyMUnitID = oReader.GetInt32("PolyMUnitID");
            oExportSCDetail.ProductDescription = oReader.GetString("ProductDescription");
            oExportSCDetail.ColorQty = oReader.GetInt32("ColorQty");
            oExportSCDetail.ColorName = oReader.GetString("ColorName");
            oExportSCDetail.ModelReferenceName = oReader.GetString("ModelReferenceName");
            oExportSCDetail.OrderSheetID = oReader.GetInt32("OrderSheetID");
            oExportSCDetail.IsApplySizer = oReader.GetBoolean("IsApplySizer");
            oExportSCDetail.YetToProductionOrderQty = oReader.GetDouble("YetToProductionOrderQty");
            oExportSCDetail.YetToDeliveryOrderQty = oReader.GetDouble("YetToDeliveryOrderQty");
            oExportSCDetail.SizeName = oReader.GetString("SizeName");
            oExportSCDetail.OverQty = oReader.GetDouble("OverQty");
            oExportSCDetail.TotalQty = oReader.GetDouble("TotalQty");
            oExportSCDetail.DyeingType = oReader.GetInt16("DyeingType");
            oExportSCDetail.BagCount = oReader.GetInt32("BagCount");
            oExportSCDetail.IsBuyerYarn = oReader.GetBoolean("IsBuyerYarn");
            oExportSCDetail.IsBuyerDyes = oReader.GetBoolean("IsBuyerDyes");
            oExportSCDetail.IsBuyerChemical = oReader.GetBoolean("IsBuyerChemical");
            return oExportSCDetail;
        }
        private ExportSCDetail CreateObject(NullHandler oReader)
        {
            ExportSCDetail oExportSCDetail = new ExportSCDetail();
            oExportSCDetail = MapObject(oReader);
            return oExportSCDetail;
        }      
        private List<ExportSCDetail> CreateObjects(IDataReader oReader)
        {
            List<ExportSCDetail> oExportSCDetails = new List<ExportSCDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportSCDetail oItem = CreateObject(oHandler);
                oExportSCDetails.Add(oItem);
            }
            return oExportSCDetails;
        }
        #endregion

        #region Interface implementation
        public ExportSCDetailService() { }
        public ExportSCDetail Save(ExportSCDetail oExportSCDetail, Int64 nUserID)
        {
            ExportSC oExportSC=new ExportSC();
            TransactionContext tc = null;
            List<ExportSCDetail> oEPIDetails = new List<ExportSCDetail>();

            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                NullHandler oReader;
                #region ExportSC Part
                if (oExportSC != null)
                {
                    reader = ExportSCDA.InsertUpdate(tc, oExportSC, EnumDBOperation.Insert, nUserID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oExportSC = new ExportSC();
                        oExportSC = ExportSCService.CreateObject(oReader);
                        oExportSCDetail.ExportSCID = oExportSC.ExportSCID;                        
                    }                    
                    if (oExportSC.ExportSCID <= 0)
                    {
                        oExportSCDetail = new ExportSCDetail();
                        oExportSCDetail.ErrorMessage = "Invalid ExportSC";
                        return oExportSCDetail;
                    }
                    reader.Close();
                }
                #endregion

                if (oExportSCDetail.ExportSCDetails.Count() > 0)
                {
                    ExportSCDetail oEPIDteail = new ExportSCDetail();
                    foreach (ExportSCDetail oItem in oExportSCDetail.ExportSCDetails)
                    {
                        if (oItem.ExportSCDetailID <= 0)
                        {
                            if (oExportSCDetail.ExportSCID > 0) 
                            {
                                oItem.ExportSCID = oExportSCDetail.ExportSCID; 
                            }
                            reader = ExportSCDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID,"");
                            oReader = new NullHandler(reader);
                            if (reader.Read())
                            {
                                oEPIDteail = new ExportSCDetail();
                                oEPIDteail = CreateObject(oReader);
                                oEPIDetails.Add(oEPIDteail);
                            }
                            reader.Close();
                        }
                    }
                }

                else
                {
                    IDataReader readerdetail;
                    if (oExportSCDetail.ExportSCDetailID <= 0)
                    {
                        readerdetail = ExportSCDetailDA.InsertUpdate(tc, oExportSCDetail, EnumDBOperation.Insert, nUserID,"");
                    }
                    else
                    {
                        readerdetail = ExportSCDetailDA.InsertUpdate(tc, oExportSCDetail, EnumDBOperation.Update, nUserID,"");
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        oExportSCDetail = new ExportSCDetail();
                        oExportSCDetail = CreateObject(oReaderDetail);
                    }
                    readerdetail.Close();
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oExportSCDetail = new ExportSCDetail();
                oExportSCDetail.ErrorMessage = e.Message;

                oExportSC = new ExportSC();
                oEPIDetails = new List<ExportSCDetail>();

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to . Because of " + e.Message, e);
                #endregion
            }

         
            oExportSCDetail.ExportSCDetails = oEPIDetails;

            return oExportSCDetail;
        }       
        public string Delete(ExportSCDetail oExportSCDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ExportSCDetailDA.Delete(tc, oExportSCDetail, EnumDBOperation.Delete, nUserID,"");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save Bank. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }
        public ExportSCDetail Get(int id, Int64 nUserId)
        {
            ExportSCDetail oExportSCDetail = new ExportSCDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportSCDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportSCDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ExportSCDetail", e);
                #endregion
            }

            return oExportSCDetail;
        }
        public List<ExportSCDetail> Gets(int nExportLCID, Int64 nUserId)
        {
            List<ExportSCDetail> oExportSCDetail = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportSCDetailDA.Gets(tc , nExportLCID);
                oExportSCDetail = CreateObjects(reader);
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

            return oExportSCDetail;
        }
        public List<ExportSCDetail> GetsByESCID(int nExportSCID, Int64 nUserId)
        {
            List<ExportSCDetail> oExportSCDetail = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportSCDetailDA.GetsByESCID(tc, nExportSCID);
                oExportSCDetail = CreateObjects(reader);
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

            return oExportSCDetail;
        }
        public List<ExportSCDetail> Gets(Int64 nUserId)
        {
            List<ExportSCDetail> oExportSCDetail = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportSCDetailDA.Gets(tc);
                oExportSCDetail = CreateObjects(reader);
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

            return oExportSCDetail;
        }
        public List<ExportSCDetail> GetsByPI(int nExportPIID,Int64 nUserId)
        {
            List<ExportSCDetail> oExportSCDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportSCDetailDA.GetsByPI(tc, nExportPIID);
                oExportSCDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportSCDetail", e);
                #endregion
            }

            return oExportSCDetail;
        }
        public List<ExportSCDetail> Gets(string sSQL, Int64 nUserId)
        {
            List<ExportSCDetail> oExportSCDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportSCDetailDA.Gets(tc, sSQL);
                oExportSCDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportSCDetail", e);
                #endregion
            }

            return oExportSCDetail;
        }
        public List<ExportSCDetail> GetsLog(int nExportPIID, Int64 nUserId)
        {
            List<ExportSCDetail> oExportSCDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportSCDetailDA.GetsLog(tc, nExportPIID);
                oExportSCDetails = CreateObjects(reader);
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

            return oExportSCDetails;
        }
        #endregion
    }
}
