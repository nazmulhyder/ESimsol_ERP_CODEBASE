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
    
    public class ExportBillDetailService : MarshalByRefObject, IExportBillDetailService
    {
        #region Private functions and declaration
        private ExportBillDetail MapObject(NullHandler oReader)
        {
            ExportBillDetail oExportBillDetail = new ExportBillDetail();
            oExportBillDetail.ExportBillDetailID = oReader.GetInt32("ExportBillDetailID");

            oExportBillDetail.ExportBillID = oReader.GetInt32("ExportBillID");
            oExportBillDetail.ProductID = oReader.GetInt32("ProductID");
            oExportBillDetail.UnitPrice = oReader.GetDouble("UnitPrice");
            oExportBillDetail.Qty = oReader.GetDouble("Qty");
            oExportBillDetail.RateUnit = oReader.GetInt32("RateUnit");            
            oExportBillDetail.WtPerBag = oReader.GetDouble("WtPerBag");
            oExportBillDetail.NoOfBag = oReader.GetDouble("NoOfBag");
            oExportBillDetail.ExportPIDetailID = oReader.GetInt32("ExportPIDetailID");
            oExportBillDetail.ProductName = oReader.GetString("ProductName");
            oExportBillDetail.ProductCode = oReader.GetString("ProductCode");
            oExportBillDetail.MUnitID = oReader.GetInt32("MUnitID");
            oExportBillDetail.MUName = oReader.GetString("MUName");            
            oExportBillDetail.PINo = oReader.GetString("PINo");
            oExportBillDetail.HSCode = oReader.GetString("HSCode");

            oExportBillDetail.ColorName = oReader.GetString("ColorName");
            oExportBillDetail.ProductDescription = oReader.GetString("ProductDescription");
            oExportBillDetail.SizeName = oReader.GetString("SizeName");
            oExportBillDetail.ModelReferenceID = oReader.GetInt32("ModelReferenceID");
            oExportBillDetail.ModelReferenceName = oReader.GetString("ModelReferenceName");
            oExportBillDetail.Measurement = oReader.GetString("Measurement");
            oExportBillDetail.IsApplySizer = oReader.GetBoolean("IsApplySizer");
            oExportBillDetail.ColorID = oReader.GetInt32("ColorID");
            oExportBillDetail.ColorQty = oReader.GetInt32("ColorQty");

            oExportBillDetail.Currency = oReader.GetString("Currency");            
            oExportBillDetail.StyleNo = oReader.GetString("StyleNo");
            oExportBillDetail.ProductDesciption = oReader.GetString("ProductDescription");
            /// Fabric
            oExportBillDetail.Construction = oReader.GetString("Construction");
            oExportBillDetail.FabricNo = oReader.GetString("FabricNo");
            oExportBillDetail.ProcessTypeName = oReader.GetString("ProcessTypeName");
            oExportBillDetail.FabricWeaveName = oReader.GetString("FabricWeaveName");
            oExportBillDetail.FinishTypeName = oReader.GetString("FinishTypeName");
            oExportBillDetail.FabricWidth = oReader.GetString("FabricWidth");
            oExportBillDetail.ColorInfo = oReader.GetString("ColorInfo");
            oExportBillDetail.StyleRef = oReader.GetString("StyleRef");
            oExportBillDetail.StyleNo = oReader.GetString("StyleNo");

            oExportBillDetail.PIDate = oReader.GetDateTime("PIDate");            
            oExportBillDetail.VersionNo = oReader.GetInt32("VersionNo");
            oExportBillDetail.ProductCategoryID = oReader.GetInt32("ProductCategoryID");
            oExportBillDetail.ProductCategoryName = oReader.GetString("ProductCategoryName");
            oExportBillDetail.BuyerReference = oReader.GetString("BuyerReference");
            oExportBillDetail.IsDeduct = oReader.GetBoolean("IsDeduct");
            oExportBillDetail.ReferenceCaption = oReader.GetString("ReferenceCaption");
            oExportBillDetail.Shrinkage = oReader.GetString("Shrinkage");
            oExportBillDetail.Weight = oReader.GetString("Weight");

            return oExportBillDetail;
        }

        private ExportBillDetail CreateObject(NullHandler oReader)
        {
            ExportBillDetail oExportBillDetail = new ExportBillDetail();
            oExportBillDetail = MapObject(oReader);
            return oExportBillDetail;
        }

        private List<ExportBillDetail> CreateObjects(IDataReader oReader)
        {
            List<ExportBillDetail> oExportBillDetails = new List<ExportBillDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportBillDetail oItem = CreateObject(oHandler);
                oExportBillDetails.Add(oItem);
            }
            return oExportBillDetails;
        }
        #endregion

        #region Interface implementation
        public ExportBillDetailService() { }

        public string Delete(ExportBillDetail oExportBillDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ExportBillDetailDA.Delete(tc, oExportBillDetail, EnumDBOperation.Delete, nUserID);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save Bank. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }
        public ExportBillDetail SaveMultipleBills(ExportBillDetail oExportBillDetail, Int64 nUserID)
        {
            ExportBill oExportBill = new ExportBill();
            oExportBill = oExportBillDetail.ExportBill;
            List<ExportBillDetail> oExportBillDetails = new List<ExportBillDetail>();
            List<ExportBillDetail> oEBDs = new List<ExportBillDetail>();
            oExportBillDetails = oExportBillDetail.ExportBillDetails;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                int Count = 0;
                int nExportBillID = 0;
                int nCount = 0;
                #region New Code
                foreach (ExportBillDetail oItem in oExportBillDetails)
                {
                    Count++;
                    #region ExportBill Part
                    if (oItem.ExportBill != null)
                    {
                        IDataReader reader;
                        nCount = ExportBillDA.GetTotalBillCount(tc, oItem.ExportBill.ExportLCID);

                        if (oItem.ExportBill.ExportBillID <= 0)
                        {
                            oItem.ExportBill.ExportBillNo = Enum.GetName(typeof(EnumExcellColumn), (EnumExcellColumn)nCount);
                            reader = ExportBillDA.InsertUpdate(tc, oItem.ExportBill, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            reader = ExportBillDA.InsertUpdate(tc, oItem.ExportBill, EnumDBOperation.Update, nUserID);
                        }
                        NullHandler oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oExportBill = new ExportBill();
                            oExportBill = ExportBillService.CreateObject(oReader);
                            nExportBillID = oExportBill.ExportBillID;
                        }
                        if (oExportBill.ExportBillID <= 0)
                        {
                            oExportBillDetail = new ExportBillDetail();
                            oExportBillDetail.ErrorMessage = "Invalid Export Bill";
                            return oExportBillDetail;
                        }
                        reader.Close();
                    }

                    #endregion

                    if (oExportBillDetails[0].ExportBill != null)
                    {
                        oItem.ExportBillID = nExportBillID;
                    }

                    IDataReader readerdetail;
                    if (oItem.ExportBillDetailID <= 0)
                    {
                        readerdetail = ExportBillDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        readerdetail = ExportBillDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        oExportBillDetail = new ExportBillDetail();
                        oExportBillDetail = CreateObject(oReaderDetail);
                        if (Count == 1)
                        {
                            oExportBillDetail.ExportBill = oExportBill;
                        }
                    }
                    oEBDs.Add(oExportBillDetail);
                    readerdetail.Close();
                }
                #endregion
                oExportBillDetail = new ExportBillDetail();
                oExportBillDetail.ExportBillDetails = oEBDs;
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oExportBillDetail = new ExportBillDetail();
                oExportBillDetail.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oExportBillDetail;
        }       
        public ExportBillDetail Save(ExportBillDetail ExportBillDetail, Int64 nUserID)
        {
            ExportBill oExportBill = new ExportBill();
            oExportBill = ExportBillDetail.ExportBill;
            int nCount = 0;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                #region ExportBill Part
                if (oExportBill != null)
                {
                    IDataReader reader;
                    nCount = ExportBillDA.GetTotalBillCount(tc, oExportBill.ExportLCID);

                    if (oExportBill.ExportBillID <= 0)
                    {
                        oExportBill.ExportBillNo = Enum.GetName(typeof(EnumExcellColumn), (EnumExcellColumn)nCount);
                        reader = ExportBillDA.InsertUpdate(tc, oExportBill, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        reader = ExportBillDA.InsertUpdate(tc, oExportBill, EnumDBOperation.Update, nUserID);
                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oExportBill = new ExportBill();
                        oExportBill = ExportBillService.CreateObject(oReader);
                        ExportBillDetail.ExportBillID = oExportBill.ExportBillID;
                    }
                    if (oExportBill.ExportBillID <= 0)
                    {
                        ExportBillDetail = new ExportBillDetail();
                        ExportBillDetail.ErrorMessage = "Invalid ExportPI";
                        return ExportBillDetail;
                    }
                    reader.Close();
                }
                #endregion

                IDataReader readerdetail;
                if (ExportBillDetail.ExportBillDetailID <= 0)
                {
                    readerdetail = ExportBillDetailDA.InsertUpdate(tc, ExportBillDetail, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    readerdetail = ExportBillDetailDA.InsertUpdate(tc, ExportBillDetail, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReaderDetail = new NullHandler(readerdetail);
                if (readerdetail.Read())
                {
                    ExportBillDetail = new ExportBillDetail();
                    ExportBillDetail = CreateObject(oReaderDetail);
                    ExportBillDetail.ExportBill = oExportBill;
                }
                readerdetail.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExportBillDetail = new ExportBillDetail();
                ExportBillDetail.ErrorMessage = e.Message.Split('~')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to . Because of " + e.Message, e);
                #endregion
            }
            return ExportBillDetail;
        }       
        public ExportBillDetail Get(int id, Int64 nUserId)
        {
            ExportBillDetail oExportBillDetail = new ExportBillDetail();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportBillDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportBillDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ExportBillDetail", e);
                #endregion
            }

            return oExportBillDetail;
        }
        public ExportBillDetail GetPIP(int nPIProductID, Int64 nUserId)
        {
            ExportBillDetail oExportBillDetail = new ExportBillDetail();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportBillDetailDA.GetPIP(tc, nPIProductID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportBillDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ExportBillDetail", e);
                #endregion
            }

            return oExportBillDetail;
        }
    
        public List<ExportBillDetail> Gets(int nLCBillID, Int64 nUserId)
        {
            List<ExportBillDetail> oExportBillDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportBillDetailDA.Gets(tc, nLCBillID);
                oExportBillDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportBillDetails", e);
                #endregion
            }

            return oExportBillDetails;
        }
    

        public List<ExportBillDetail> GetsBySQL(string sSQL, Int64 nUserId)
        {
            List<ExportBillDetail> oExportBillDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportBillDetailDA.GetsBySQL(tc, sSQL);
                oExportBillDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportBillDetails", e);
                #endregion
            }

            return oExportBillDetails;
        }

        //public ExportBillDetail Save(ExportBillDetail ExportBillDetail, Int64 nUserID)
        //{
        //    TransactionContext tc = null;
        //    try
        //    {
        //        //ExportBill oExportBill = new ExportBill();
        //        //List<ExportBillDetail> oExportBillDetails = new 


        //        //tc = TransactionContext.Begin(true);













        //        #region ExportBill Part
        //        if (oExportBill != null)
        //        {
        //            IDataReader reader;


        //            if (oExportBill.ExportBillID <= 0)
        //            {
        //                int nCount = ExportBillDA.GetTotalBillCount(tc, oExportBill.ExportLCID);
        //                oExportBill.ExportBillNo = Enum.GetName(typeof(EnumExcellColumn), (EnumExcellColumn)nCount + 1);
        //                reader = ExportBillDA.InsertUpdate(tc, oExportBill, EnumDBOperation.Insert, nUserID);
        //            }
        //            else
        //            {
        //                reader = ExportBillDA.InsertUpdate(tc, oExportBill, EnumDBOperation.Update, nUserID);
        //            }
        //            NullHandler oReader = new NullHandler(reader);
        //            if (reader.Read())
        //            {
        //                oExportBill = new ExportBill();
        //                oExportBill = ExportBillService.CreateObject(oReader);
        //                ExportBillDetail.ExportBillID = oExportBill.ExportBillID;
        //            }
        //            if (oExportBill.ExportBillID <= 0)
        //            {
        //                ExportBillDetail = new ExportBillDetail();
        //                ExportBillDetail.ErrorMessage = "Invalid ExportPI";
        //                return ExportBillDetail;
        //            }
        //            reader.Close();
        //        }
        //        #endregion

        //        IDataReader readerdetail;
        //        if (ExportBillDetail.ExportBillDetailID <= 0)
        //        {
        //            readerdetail = ExportBillDetailDA.InsertUpdate(tc, ExportBillDetail, EnumDBOperation.Insert, nUserID);
        //        }
        //        else
        //        {
        //            readerdetail = ExportBillDetailDA.InsertUpdate(tc, ExportBillDetail, EnumDBOperation.Update, nUserID);
        //        }
        //        NullHandler oReaderDetail = new NullHandler(readerdetail);
        //        if (readerdetail.Read())
        //        {
        //            ExportBillDetail = new ExportBillDetail();
        //            ExportBillDetail = CreateObject(oReaderDetail);
        //            ExportBillDetail.ExportBill = oExportBill;
        //        }
        //        readerdetail.Close();
        //        tc.End();
        //    }
        //    catch (Exception e)
        //    {
        //        #region Handle Exception
        //        if (tc != null)
        //            tc.HandleError();

        //        ExportBillDetail = new ExportBillDetail();
        //        ExportBillDetail.ErrorMessage = e.Message.Split('~')[0];

        //        //ExceptionLog.Write(e);
        //        //throw new ServiceException("Failed to . Because of " + e.Message, e);
        //        #endregion
        //    }
        //    return ExportBillDetail;
        //}       
     
        #endregion
    }
}