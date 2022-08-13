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
    public class LabDipDetailService : MarshalByRefObject, ILabDipDetailService
    {
        #region Private functions and declaration
        private static LabDipDetail MapObject(NullHandler oReader)
        {
            LabDipDetail oLabDipDetail = new LabDipDetail();

            oLabDipDetail.LabDipDetailID = oReader.GetInt32("LabDipDetailID");
            oLabDipDetail.LabDipID = oReader.GetInt32("LabDipID");
            oLabDipDetail.ProductID = oReader.GetInt32("ProductID");
            oLabDipDetail.ParentID = oReader.GetInt32("ParentID");
            oLabDipDetail.ColorSet = oReader.GetInt16("ColorSet");
            oLabDipDetail.ShadeCount = oReader.GetInt16("ShadeCount");
            oLabDipDetail.KnitPlyYarn = (EnumKnitPlyYarn)oReader.GetInt16("KnitPlyYarn");
            oLabDipDetail.ColorName = oReader.GetString("ColorName");
            oLabDipDetail.RefNo = oReader.GetString("RefNo");
            oLabDipDetail.PantonNo = oReader.GetString("PantonNo");
            oLabDipDetail.PantonPageNo = oReader.GetString("PantonPageNo");
            oLabDipDetail.RGB = oReader.GetString("RGB");
            oLabDipDetail.ColorNo = oReader.GetString("ColorNoFull");
            oLabDipDetail.Combo = oReader.GetInt16("Combo");
            oLabDipDetail.LotNo = oReader.GetString("LotNo");
            oLabDipDetail.ColorCreateBy = oReader.GetInt32("ColorCreateBy");
            oLabDipDetail.SubmitBy = oReader.GetInt32("SubmitBy");
            oLabDipDetail.ColorCreateDate = oReader.GetDateTime("ColorCreateDate");
            oLabDipDetail.OrderDate = oReader.GetDateTime("OrderDate");
            oLabDipDetail.DBUserID = oReader.GetInt32("DBUserID");
            oLabDipDetail.TwistedGroup = oReader.GetInt32("TwistedGroup");
            oLabDipDetail.Gauge = oReader.GetInt32("Gauge");
            oLabDipDetail.ProductCode = oReader.GetString("ProductCode");
            oLabDipDetail.ProductName = oReader.GetString("ProductName");
            oLabDipDetail.FabricNo = oReader.GetString("FabricNo");
            oLabDipDetail.LabdipNo = oReader.GetString("LabdipNo");
            oLabDipDetail.OrderNo = oReader.GetString("OrderNo");
            oLabDipDetail.OrderTypeSt = oReader.GetString("OrderTypeSt");
            oLabDipDetail.OrderStatus = (EnumLabdipOrderStatus)oReader.GetInt16("OrderStatus");
            oLabDipDetail.LDNo = oReader.GetString("LDNo");
            oLabDipDetail.LabdipColorID = oReader.GetInt32("LabdipColorID");
            oLabDipDetail.LabdipChallanID = oReader.GetInt32("LabdipChallanID");
            oLabDipDetail.WarpWeftType = (EnumWarpWeft)oReader.GetInt16("WarpWeftType");
            oLabDipDetail.ColorCode = oReader.GetString("ColorCode");
            oLabDipDetail.Construction = oReader.GetString("Construction");
            oLabDipDetail.ContractorID = oReader.GetInt32("ContractorID");
            oLabDipDetail.ContractorName = oReader.GetString("ContractorName");
            oLabDipDetail.ContractorAddress = oReader.GetString("ContractorAddress");
            oLabDipDetail.ParentProduct = oReader.GetString("ParentProduct");

            oLabDipDetail.IsInHouse = oReader.GetBoolean("IsInHouse");
            if (string.IsNullOrEmpty(oLabDipDetail.OrderTypeSt))
            {
                if (oLabDipDetail.IsInHouse) { oLabDipDetail.OrderTypeSt = "In House"; }
                else oLabDipDetail.OrderTypeSt = "Out Side";
            }
            return oLabDipDetail;
        }

        public static LabDipDetail CreateObject(NullHandler oReader)
        {
            LabDipDetail oLabDipDetail = new LabDipDetail();
            oLabDipDetail = MapObject(oReader);
            return oLabDipDetail;
        }

        private List<LabDipDetail> CreateObjects(IDataReader oReader)
        {
            List<LabDipDetail> oLabDipDetail = new List<LabDipDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                LabDipDetail oItem = CreateObject(oHandler);
                oLabDipDetail.Add(oItem);
            }
            return oLabDipDetail;
        }

        #endregion

        #region Interface implementation
        public LabDipDetailService() { }

        public LabDipDetail IUD(LabDipDetail oLabDipDetail, int nDBOperation, int nUserId)
        {
            LabDip oLabDip = new LabDip();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                NullHandler oReader;
                if (nDBOperation == (int)EnumDBOperation.Insert && oLabDipDetail.LabDipID == 0)
                {
                    if (oLabDipDetail.LabDip != null)
                    {
                        reader = LabDipDA.IUD(tc, oLabDipDetail.LabDip, nDBOperation, nUserId);
                        oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oLabDip = LabDipService.CreateObject(oReader);
                        }
                        reader.Close();
                        oLabDipDetail.LabDipID = oLabDip.LabDipID;
                    }
                    else { throw new Exception("No labdip information found to save."); }
                }

                reader = LabDipDetailDA.IUD(tc, oLabDipDetail, nDBOperation, nUserId);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLabDipDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
                if (nDBOperation == (int)EnumDBOperation.Delete) { oLabDipDetail = new LabDipDetail(); oLabDipDetail.ErrorMessage = Global.DeleteMessage; }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oLabDip = new LabDip();
                oLabDipDetail.ErrorMessage = (e.Message.Contains("~")) ? e.Message.Split('~')[0] : e.Message;
                #endregion
            }
            oLabDipDetail.LabDip = oLabDip;
            return oLabDipDetail;
        }

        public LabDipDetail Revise(LabDipDetail oLabDipDetail, int nDBOperation, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                NullHandler oReader;
                
                reader = LabDipDetailDA.IUD(tc, oLabDipDetail, nDBOperation, nUserId);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLabDipDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oLabDipDetail.ErrorMessage = (e.Message.Contains("~")) ? e.Message.Split('~')[0] : e.Message;
                #endregion
            }
            return oLabDipDetail;
        }
        public LabDipDetail UpdateLot(LabDipDetail oLabDipDetail, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                
               LabDipDetailDA.UpdateLot(tc, oLabDipDetail, nUserID);

               IDataReader reader = LabDipDetailDA.Get(tc, oLabDipDetail.LabDipDetailID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLabDipDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save BLLot", e);
                #endregion
            }
            return oLabDipDetail;
        }

        public LabDipDetail Get(int nId, int nUserId)
        {
            LabDipDetail oLabDipDetail = new LabDipDetail();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = LabDipDetailDA.Get(tc, nId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLabDipDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oLabDipDetail.ErrorMessage = e.Message;
                #endregion
            }

            return oLabDipDetail;
        }
     
        public List<LabDipDetail> Gets(string sSQL, int nUserId)
        {
            List<LabDipDetail> oLabDipDetails = new List<LabDipDetail>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LabDipDetailDA.Gets(tc, sSQL);
                oLabDipDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                LabDipDetail oLabDipDetail = new LabDipDetail();
                oLabDipDetail.ErrorMessage = e.Message;
                oLabDipDetails.Add(oLabDipDetail);
                #endregion
            }

            return oLabDipDetails;
        }

        public LabDipDetail IssueColor(int nLabDipDetailID, int nUserId)
        {
            LabDipDetail oLabDipDetail = new LabDipDetail();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader = LabDipDetailDA.IssueColor(tc, nLabDipDetailID, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLabDipDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oLabDipDetail.ErrorMessage = e.Message;
                #endregion
            }

            return oLabDipDetail;
        }

        public List<LabDipDetail> IssueColorMultiple(int[] LabDipDetailIDs, int nUserId)
        {

            List<LabDipDetail> oLabDipDetails = new List<LabDipDetail>();
            LabDipDetail oLabDipDetail = new LabDipDetail();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                NullHandler oReader;

                foreach (int nLabDipDetailID in LabDipDetailIDs)
                {
                    reader = LabDipDetailDA.IssueColor(tc, nLabDipDetailID, nUserId);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oLabDipDetail = CreateObject(oReader);
                        oLabDipDetails.Add(oLabDipDetail);
                    }
                    reader.Close();
                }
              
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oLabDipDetails = new List<LabDipDetail>();
                oLabDipDetail.ErrorMessage = e.Message;
                oLabDipDetails.Add(oLabDipDetail);
                #endregion
            }

            return oLabDipDetails;
        }
        
        public List<LabDipDetail> MakeTwistedGroup(string sLabDipDetailID, int nLabDipID, int nTwistedGroup, int nParentID, int nDBOperation, int nUserID)
        {
            List<LabDipDetail> oLabDipDetails = new List<LabDipDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LabDipDetailDA.MakeTwistedGroup(tc, sLabDipDetailID, nLabDipID, nTwistedGroup, nParentID, nDBOperation, nUserID);
                oLabDipDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                LabDipDetail oLabDipDetail = new LabDipDetail();
                oLabDipDetail.ErrorMessage = e.Message;
                oLabDipDetails.Add(oLabDipDetail);
                #endregion
            }

            return oLabDipDetails;
        }
        public LabDipDetail Save_ColorNo(LabDipDetail oLabDipDetail, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader = LabDipDetailDA.Save_ColorNo(tc, oLabDipDetail, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLabDipDetail = CreateObject(oReader);
                }

                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oLabDipDetail = new LabDipDetail();
                oLabDipDetail.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oLabDipDetail;

        }
        public LabDipDetail Save_PantonNo(LabDipDetail oLabDipDetail, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader = LabDipDetailDA.Save_PantonNo(tc, oLabDipDetail, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLabDipDetail = CreateObject(oReader);
                }

                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oLabDipDetail = new LabDipDetail();
                oLabDipDetail.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oLabDipDetail;

        }
        public LabDipDetail LabDip_Receive_Submit(LabDipDetail oLabDipDetail, int nBDOperation, int nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader = LabDipDetailDA.LabDip_Receive_Submit(tc, oLabDipDetail, nBDOperation,nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLabDipDetail = CreateObject(oReader);
                }

                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oLabDipDetail = new LabDipDetail();
                oLabDipDetail.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oLabDipDetail;

        }
        #endregion
    }
}
