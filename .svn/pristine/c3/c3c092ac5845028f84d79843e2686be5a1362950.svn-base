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
    public class GRNDetailService : MarshalByRefObject, IGRNDetailService
    {
        #region Private functions and declaration
        private GRNDetail MapObject(NullHandler oReader)
        {
            GRNDetail oGRNDetail = new GRNDetail();
            oGRNDetail.GRNDetailID = oReader.GetInt32("GRNDetailID");
            oGRNDetail.GRNID = oReader.GetInt32("GRNID");
            oGRNDetail.ProductID = oReader.GetInt32("ProductID");
            oGRNDetail.VehicleModelID = oReader.GetInt32("VehicleModelID");
            oGRNDetail.ModelShortName = oReader.GetString("ModelShortName");
            oGRNDetail.TechnicalSpecification = oReader.GetString("TechnicalSpecification");
            oGRNDetail.MUnitID = oReader.GetInt32("MUnitID");
            oGRNDetail.RefQty = oReader.GetDouble("RefQty");
            oGRNDetail.ReceivedQty = oReader.GetDouble("ReceivedQty");
            oGRNDetail.UnitPrice = oReader.GetDouble("UnitPrice");
            oGRNDetail.ItemWiseLandingCost = oReader.GetDouble("ItemWiseLandingCost");

            oGRNDetail.InvoiceLandingCost = oReader.GetDouble("InvoiceLandingCost");
            oGRNDetail.LCLandingCost = oReader.GetDouble("LCLandingCost");
            oGRNDetail.Amount = oReader.GetDouble("Amount");            
            oGRNDetail.LotID = oReader.GetInt32("LotID");
            oGRNDetail.LotNo = oReader.GetString("LotNo");
            oGRNDetail.RefType = (EnumGRNType)oReader.GetInt32("RefType");
            oGRNDetail.RefTypeInt = oReader.GetInt32("RefType");
            oGRNDetail.RefObjectID = oReader.GetInt32("RefObjectID");
            oGRNDetail.StyleID = oReader.GetInt32("StyleID");
            oGRNDetail.ColorID = oReader.GetInt32("ColorID");
            oGRNDetail.SizeID = oReader.GetInt32("SizeID");
            oGRNDetail.GRNNo = oReader.GetString("GRNNo");
            oGRNDetail.MUName = oReader.GetString("MUName");
            oGRNDetail.MUSymbol = oReader.GetString("MUSymbol");
            oGRNDetail.ProductName = oReader.GetString("ProductName");
            oGRNDetail.ProductCategoryName = oReader.GetString("ProductCategoryName");
            oGRNDetail.ProductCode = oReader.GetString("ProductCode");
            oGRNDetail.StyleNo = oReader.GetString("StyleNo");
            oGRNDetail.BuyerName = oReader.GetString("BuyerName");
            oGRNDetail.ColorName = oReader.GetString("ColorName");
            oGRNDetail.SizeName = oReader.GetString("SizeName");
            oGRNDetail.ProjectName = oReader.GetString("ProjectName");
            oGRNDetail.YetToReceiveQty = oReader.GetDouble("YetToReceiveQty");
            oGRNDetail.DateMonth = oReader.GetInt32("DateMonth");
            oGRNDetail.DateYear = oReader.GetInt32("DateYear");

            oGRNDetail.GRNDate = oReader.GetDateTime("GRNDate");
            oGRNDetail.CustomerName = oReader.GetString("CustomerName");
            oGRNDetail.ImportInvoiceNo = oReader.GetString("ImportInvoiceNo");
            oGRNDetail.ImportLCNo = oReader.GetString("ImportLCNo");
            oGRNDetail.ImportLCID = oReader.GetInt32("ImportLCID");
            oGRNDetail.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oGRNDetail.Remarks = oReader.GetString("Remarks");
            oGRNDetail.RejectQty = oReader.GetDouble("RejectQty");
            oGRNDetail.ActualQty = oReader.GetDouble("ActualQty");
            oGRNDetail.WeightPerCartoon = oReader.GetDouble("WeightPerCartoon");
            oGRNDetail.ConePerCartoon = oReader.GetDouble("ConePerCartoon");
            oGRNDetail.MCDia = oReader.GetString("MCDia");
            oGRNDetail.FinishDia = oReader.GetString("FinishDia");
            oGRNDetail.GSM = oReader.GetString("GSM");
            oGRNDetail.Shade = oReader.GetString("Shade");
            oGRNDetail.Stretch_Length = oReader.GetString("Stretch_Length");
            oGRNDetail.RackID = oReader.GetInt32("RackID");
            oGRNDetail.ShelfWithRackNo = oReader.GetString("ShelfWithRackNo");
            
            return oGRNDetail;
        }

        private GRNDetail CreateObject(NullHandler oReader)
        {
            GRNDetail oGRNDetail = new GRNDetail();
            oGRNDetail = MapObject(oReader);
            return oGRNDetail;
        }

        private List<GRNDetail> CreateObjects(IDataReader oReader)
        {
            List<GRNDetail> oGRNDetail = new List<GRNDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                GRNDetail oItem = CreateObject(oHandler);
                oGRNDetail.Add(oItem);
            }
            return oGRNDetail;
        }
        #endregion

        #region Interface implementation
        public GRNDetailService() { }

        public GRNDetail Save(GRNDetail oGRNDetail, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oGRNDetail.GRNDetailID <= 0)
                {
                    reader = GRNDetailDA.InsertUpdate(tc, oGRNDetail, EnumDBOperation.Insert, nUserID, "");
                }
                else
                {
                    reader = GRNDetailDA.InsertUpdate(tc, oGRNDetail, EnumDBOperation.Update, nUserID, "");
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oGRNDetail = new GRNDetail();
                    oGRNDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save GRNDetail. Because of " + e.Message, e);
                #endregion
            }
            return oGRNDetail;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                GRNDetail oGRNDetail = new GRNDetail();
                oGRNDetail.GRNDetailID = id;
                GRNDetailDA.Delete(tc, oGRNDetail, EnumDBOperation.Delete, nUserId, "");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete GRNDetail. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public GRNDetail Get(int id, int nUserId)
        {
            GRNDetail oAccountHead = new GRNDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = GRNDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get GRNDetail", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<GRNDetail> Gets(int nGRNID, int nUserID)
        {
            List<GRNDetail> oGRNDetail = new List<GRNDetail>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = GRNDetailDA.Gets(tc, nGRNID);
                oGRNDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get GRNDetail", e);
                #endregion
            }

            return oGRNDetail;
        }
        public List<GRNDetail> Gets(int nUserID)
        {
            List<GRNDetail> oGRNDetail = new List<GRNDetail>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = GRNDetailDA.Gets(tc);
                oGRNDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get GRNDetail", e);
                #endregion
            }

            return oGRNDetail;
        }
        public List<GRNDetail> Gets(string sSQL,int nUserID)
        {
            List<GRNDetail> oGRNDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                if(sSQL=="")
                {
                sSQL = "Select * from GRNDetail where GRNDetailID in (1,2,80,272,347,370,60,45)";
                    }
                reader = GRNDetailDA.Gets(tc, sSQL);
                oGRNDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get GRNDetail", e);
                #endregion
            }

            return oGRNDetail;
        }
        public List<GRNDetail> GetsRpt_Product(int BUID, int nWorkingUnitID, int DateYear, int ReportLayout, int nUserID)
        {
            List<GRNDetail> oGRNDetails = null;
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = GRNDetailDA.GetsRpt_Product(tc, BUID, nWorkingUnitID, DateYear, ReportLayout);
                oGRNDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get GRNDetail", e);
                #endregion
            }

            return oGRNDetails;
        }
        #endregion
    }   
}