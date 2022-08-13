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
    public class FNLabDipDetailService : MarshalByRefObject, IFNLabDipDetailService
    {
        #region Private functions and declaration
        private static FNLabDipDetail MapObject(NullHandler oReader)
        {
            FNLabDipDetail oFNLabDipDetails = new FNLabDipDetail();

            oFNLabDipDetails.FNLabDipDetailID = oReader.GetInt32("FNLabDipDetailID");
            oFNLabDipDetails.FabricID = oReader.GetInt32("FabricID");
            oFNLabDipDetails.ColorNo = oReader.GetString("ColorNo");
            oFNLabDipDetails.ColorSet = oReader.GetInt16("ColorSet");
            oFNLabDipDetails.ShadeCount = oReader.GetInt16("ShadeCount");
            oFNLabDipDetails.KnitPlyYarn = (EnumKnitPlyYarn)oReader.GetInt16("KnitPlyYarn");
            oFNLabDipDetails.ColorName = oReader.GetString("ColorName");
            oFNLabDipDetails.RefNo = oReader.GetString("RefNo");
            oFNLabDipDetails.PantonNo = oReader.GetString("PantonNo");
            oFNLabDipDetails.RGB = oReader.GetString("RGB");
            oFNLabDipDetails.Combo = oReader.GetInt16("Combo");
            oFNLabDipDetails.LotNo = oReader.GetString("LotNo");
            oFNLabDipDetails.Note = oReader.GetString("Note");
            oFNLabDipDetails.IssueDate = oReader.GetDateTime("IssueDate");
            oFNLabDipDetails.PrepareByName = oReader.GetString("PrepareByName");
            oFNLabDipDetails.LDNo = oReader.GetString("LDNo");
            oFNLabDipDetails.LabdipNo = oReader.GetString("LabdipNo");
            oFNLabDipDetails.Code = oReader.GetString("Code");
            oFNLabDipDetails.FabricNo = oReader.GetString("FabricNo");
            oFNLabDipDetails.Construction = oReader.GetString("Construction");
            oFNLabDipDetails.ProductName = oReader.GetString("ProductName");
            oFNLabDipDetails.ContractorName = oReader.GetString("ContractorName");
            oFNLabDipDetails.ReceiveBY = oReader.GetInt32("ReceiveBY");
            oFNLabDipDetails.SubmitBy = oReader.GetInt32("SubmitBy");
            oFNLabDipDetails.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oFNLabDipDetails.ReceiveBYName = oReader.GetString("ReceiveBYName");
            oFNLabDipDetails.SubmitBYName = oReader.GetString("SubmitBYName");
            oFNLabDipDetails.LightSourceID = oReader.GetInt32("LightSourceID");
            oFNLabDipDetails.LightSource = oReader.GetString("LightSource");
            oFNLabDipDetails.LightSourceTwo = oReader.GetString("LightSourceTwo");
            oFNLabDipDetails.MKTPerson = oReader.GetString("MKTPerson");
            oFNLabDipDetails.OrderName = oReader.GetString("OrderName");
            oFNLabDipDetails.StyleNo = oReader.GetString("StyleNo");
            oFNLabDipDetails.SCNoFull = oReader.GetString("SCNoFull");
            oFNLabDipDetails.StrikeOff = oReader.GetString("StrikeOff");
            oFNLabDipDetails.ApprovalNote = oReader.GetString("ApprovalNote");
            oFNLabDipDetails.ShadeApproveDate = oReader.GetDateTime("ShadeApproveDate");
            oFNLabDipDetails.ShadeID_Ap = (EnumShade)oReader.GetInt32("ShadeID_Ap");
            oFNLabDipDetails.LDStatus = (EnumLDStatus)oReader.GetInt32("LDStatus");
            oFNLabDipDetails.ContactPersonnelID = oReader.GetInt32("ContactPersonnelID");
            oFNLabDipDetails.FSCDID = oReader.GetInt32("FSCDID");

            return oFNLabDipDetails;
        }

        public static FNLabDipDetail CreateObject(NullHandler oReader)
        {
            FNLabDipDetail oFNLabDipDetail = new FNLabDipDetail();
            oFNLabDipDetail = MapObject(oReader);
            return oFNLabDipDetail;
        }

        private List<FNLabDipDetail> CreateObjects(IDataReader oReader)
        {
            List<FNLabDipDetail> oFNLabDipDetail = new List<FNLabDipDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FNLabDipDetail oItem = CreateObject(oHandler);
                oFNLabDipDetail.Add(oItem);
            }
            return oFNLabDipDetail;
        }

        #endregion

        #region Interface implementation
        public FNLabDipDetailService() { }

        public FNLabDipDetail IUD(FNLabDipDetail oFNLabDipDetail, int nDBOperation, int nUserId)
        {
            TransactionContext tc = null;
            
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                NullHandler oReader;
            

                reader = FNLabDipDetailDA.IUD(tc, oFNLabDipDetail, nDBOperation, nUserId);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNLabDipDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
                if (nDBOperation == (int)EnumDBOperation.Delete) { oFNLabDipDetail = new FNLabDipDetail(); oFNLabDipDetail.ErrorMessage = Global.DeleteMessage; }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFNLabDipDetail.ErrorMessage = (e.Message.Contains("~")) ? e.Message.Split('~')[0] : e.Message;
                #endregion
            }
            //oFNLabDipDetail.LabDip = oLabDip;
            return oFNLabDipDetail;
        }
        public FNLabDipDetail UpdateLot(FNLabDipDetail oFNLabDipDetail, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                
               FNLabDipDetailDA.UpdateLot(tc, oFNLabDipDetail, nUserID);

               IDataReader reader = FNLabDipDetailDA.Get(tc, oFNLabDipDetail.FNLabDipDetailID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNLabDipDetail = CreateObject(oReader);
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
            return oFNLabDipDetail;
        }

        public FNLabDipDetail Get(int nId, int nUserId)
        {
            FNLabDipDetail oFNLabDipDetail = new FNLabDipDetail();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FNLabDipDetailDA.Get(tc, nId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNLabDipDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFNLabDipDetail.ErrorMessage = e.Message;
                #endregion
            }

            return oFNLabDipDetail;
        }
     
        public List<FNLabDipDetail> Gets(string sSQL, int nUserId)
        {
            List<FNLabDipDetail> oFNLabDipDetails = new List<FNLabDipDetail>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FNLabDipDetailDA.Gets(tc, sSQL);
                oFNLabDipDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FNLabDipDetail oFNLabDipDetail = new FNLabDipDetail();
                oFNLabDipDetail.ErrorMessage = e.Message;
                oFNLabDipDetails.Add(oFNLabDipDetail);
                #endregion
            }

            return oFNLabDipDetails;
        }
        public List<FNLabDipDetail> GetsBy(int nFabricID, int nUserId)
        {
            List<FNLabDipDetail> oFNLabDipDetails = new List<FNLabDipDetail>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FNLabDipDetailDA.GetsBy(tc, nFabricID);
                oFNLabDipDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FNLabDipDetail oFNLabDipDetail = new FNLabDipDetail();
                oFNLabDipDetail.ErrorMessage = e.Message;
                oFNLabDipDetails.Add(oFNLabDipDetail);
                #endregion
            }

            return oFNLabDipDetails;
        }



        public List<FNLabDipDetail> IssueLDNoMultiple(List<FNLabDipDetail> oFNLabDipDetails, int nUserId)
        {
            List<FNLabDipDetail> oFNLabDipDetails_Ret = new List<FNLabDipDetail>(); 
            FNLabDipDetail oFNLabDipDetail = new FNLabDipDetail();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                NullHandler oReader;

                foreach (FNLabDipDetail oItem in oFNLabDipDetails)
                {
                    reader = FNLabDipDetailDA.Save_LDNo(tc, oItem, nUserId);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFNLabDipDetail = CreateObject(oReader);
                        oFNLabDipDetails_Ret.Add(oFNLabDipDetail);
                    }
                    reader.Close();
                }
              
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFNLabDipDetails = new List<FNLabDipDetail>();
                oFNLabDipDetail.ErrorMessage = e.Message;
                oFNLabDipDetails_Ret.Add(oFNLabDipDetail);
                #endregion
            }

            return oFNLabDipDetails_Ret;
        }

        public FNLabDipDetail Save_LDNo(FNLabDipDetail oFNLabDipDetail, int nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader = FNLabDipDetailDA.Save_LDNo(tc, oFNLabDipDetail, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNLabDipDetail = CreateObject(oReader);
                }

                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFNLabDipDetail = new FNLabDipDetail();
                oFNLabDipDetail.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oFNLabDipDetail;
        }
        public FNLabDipDetail Submited(FNLabDipDetail oFNLabDipDetail, int nUserId)
        {
            int nFNLabDipRecipeCount = 0;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                nFNLabDipRecipeCount = FNLabDipDetailDA.FNLabDipRecipeCount(tc, oFNLabDipDetail.FNLabDipDetailID);
                if (nFNLabDipRecipeCount <= 0)
                {
                    throw new Exception("Recipe Yet Not entry");
                }

                IDataReader reader = FNLabDipDetailDA.Submited(tc, oFNLabDipDetail, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNLabDipDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oFNLabDipDetail = new FNLabDipDetail();
                oFNLabDipDetail.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFNLabDipDetail;
        }
        public FNLabDipDetail Issued(FNLabDipDetail oFNLabDipDetail, int nUserId)
        {
            int nFNLabDipRecipeCount = 0;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                //nFNLabDipRecipeCount = FNLabDipDetailDA.FNLabDipRecipeCount(tc, oFNLabDipDetail.FNLabDipDetailID);
                //if (nFNLabDipRecipeCount <= 0)
                //{
                //    throw new Exception("Recipe Yet Not entry");
                //}

                IDataReader reader = FNLabDipDetailDA.Issued(tc, oFNLabDipDetail, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNLabDipDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oFNLabDipDetail = new FNLabDipDetail();
                oFNLabDipDetail.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFNLabDipDetail;
        }
        public FNLabDipDetail UpdateShade(FNLabDipDetail oFNLabDipDetail, int nUserId)
        {
            int nFNLabDipRecipeCount = 0;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                //nFNLabDipRecipeCount = FNLabDipDetailDA.FNLabDipRecipeCount(tc, oFNLabDipDetail.FNLabDipDetailID);
                //if (nFNLabDipRecipeCount <= 0)
                //{
                //    throw new Exception("Recipe Yet Not entry");
                //}

                IDataReader reader = FNLabDipDetailDA.UpdateShade(tc, oFNLabDipDetail, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNLabDipDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oFNLabDipDetail = new FNLabDipDetail();
                oFNLabDipDetail.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFNLabDipDetail;
        }

        public FNLabDipDetail UpdateColorSet(FNLabDipDetail oFNLabDipDetail, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FNLabDipDetailDA.UpdateColorSet(tc, oFNLabDipDetail);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNLabDipDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oFNLabDipDetail = new FNLabDipDetail();
                oFNLabDipDetail.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFNLabDipDetail;
        }

        #endregion
    }
}
