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
    public class GRNService : MarshalByRefObject, IGRNService
    {
        #region Private functions and declaration
        private GRN MapObject(NullHandler oReader)
        {
            GRN oGRN = new GRN();
            oGRN.GRNID = oReader.GetInt32("GRNID");
            oGRN.GRNNo = oReader.GetString("GRNNo");
            oGRN.GRNDate = oReader.GetDateTime("GRNDate");
            oGRN.GLDate = oReader.GetDateTime("GLDate");
            oGRN.GRNType = (EnumGRNType)oReader.GetInt16("GRNType");
            oGRN.GRNTypeInt = oReader.GetInt16("GRNType");
            oGRN.GRNStatus = (EnumGRNStatus)oReader.GetInt16("GRNStatus");
            oGRN.GRNStatusInt = oReader.GetInt16("GRNStatus");
            oGRN.BUID = oReader.GetInt32("BUID");
            oGRN.ContractorID = oReader.GetInt32("ContractorID");
            oGRN.RefObjectID = oReader.GetInt32("RefObjectID");
            oGRN.Remarks = oReader.GetString("Remarks");
            oGRN.StoreNameWithoutLocation = oReader.GetString("StoreNameWithoutLocation");
            oGRN.CurrencyID = oReader.GetInt32("CurrencyID");
            oGRN.ConversionRate = oReader.GetDouble("ConversionRate");
            oGRN.ApproveBy = oReader.GetInt32("ApproveBy");
            oGRN.ApproveDate = oReader.GetDateTime("ApproveDate");
            oGRN.ContractorName = oReader.GetString("VendorName");
            oGRN.ContractorPhone = oReader.GetString("VendorPhone");
            oGRN.Address = oReader.GetString("VendorAddress");
            oGRN.ConShortName = oReader.GetString("VendorShortName");
            oGRN.Currency = oReader.GetString("CSymbol");
            oGRN.Amount = oReader.GetDouble("Amount");
            oGRN.PrepareByName = oReader.GetString("PrepareByName");
            oGRN.ApproveByName = oReader.GetString("ApproveByName");
            oGRN.MRFNo = oReader.GetString("MRFNo");
            oGRN.RefObjectNo = oReader.GetString("RefObjectNo");
            oGRN.RefObjectDate = oReader.GetDateTime("RefObjectDate");
            oGRN.RefObjectAmount = oReader.GetInt32("RefObjectAmount");           
            oGRN.BUName = oReader.GetString("BUName");
            oGRN.BUCode = oReader.GetString("BUCode");
            oGRN.ReceivedBy = oReader.GetInt32("ReceivedBy");
            oGRN.ReceivedDateTime = oReader.GetDateTime("ReceivedDateTime");
            oGRN.ReceivedByName = oReader.GetString("ReceivedByName");
            oGRN.ChallanNo = oReader.GetString("ChallanNo");
            oGRN.ChallanDate = oReader.GetDateTime("ChallanDate");
            oGRN.StoreID = oReader.GetInt32("StoreID");
            oGRN.StoreName = oReader.GetString("StoreName");
            oGRN.GatePassNo = oReader.GetString("GatePassNo");
            oGRN.ImportLCNo = oReader.GetString("ImportLCNo");
            oGRN.VehicleNo = oReader.GetString("VehicleNo");
            oGRN.IsWillVoucherEffect = oReader.GetBoolean("IsWillVoucherEffect");
            oGRN.RefAmount = oReader.GetDouble("RefAmount");
            oGRN.RefDisCount = oReader.GetDouble("RefDisCount");
            oGRN.RefServiceCharge = oReader.GetDouble("RefServiceCharge");
            oGRN.TotalQty = oReader.GetDouble("TotalQty");
            oGRN.MUName = oReader.GetString("MUName");
            oGRN.MRIRNo = oReader.GetString("MRIRNo");
            oGRN.MRIRDate = oReader.GetDateTime("MRIRDate");
            return oGRN;
        }

        private GRN CreateObject(NullHandler oReader)
        {
            GRN oGRN = new GRN();
            oGRN = MapObject(oReader);
            return oGRN;
        }

        private List<GRN> CreateObjects(IDataReader oReader)
        {
            List<GRN> oGRN = new List<GRN>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                GRN oItem = CreateObject(oHandler);
                oGRN.Add(oItem);
            }
            return oGRN;
        }

        #endregion

        #region Interface implementation
     

        public GRN Save(GRN oGRN, int nUserID)
        {
            string sGRNDetailIDs = "";
            TransactionContext tc = null;
            List<GRNDetail> oGRNDetails = new List<GRNDetail>();            
            oGRNDetails = oGRN.GRNDetails;
            try
            {
                tc = TransactionContext.Begin(true);

                #region GRN Part
                IDataReader reader;
                if (oGRN.GRNID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.GRN , EnumRoleOperationType.Add);
                    reader = GRNDA.InsertUpdate(tc, oGRN, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.GRN, EnumRoleOperationType.Edit);
                    VoucherDA.CheckVoucherReference(tc, "GRN", "GRNID", oGRN.GRNID);
                    reader = GRNDA.InsertUpdate(tc, oGRN, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oGRN = new GRN();
                    oGRN = CreateObject(oReader);
                }
                reader.Close();
                #endregion
                
                #region GRN Detail Part
                if (oGRNDetails != null)
                {
                    foreach (GRNDetail oItem in oGRNDetails)
                    {
                        IDataReader readerdetail;
                        oItem.GRNID = oGRN.GRNID;                                              
                        oItem.Amount = (oItem.ReceivedQty * oItem.UnitPrice);
                        if (oItem.GRNDetailID <= 0)
                        {
                            readerdetail = GRNDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = GRNDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sGRNDetailIDs = sGRNDetailIDs + oReaderDetail.GetString("GRNDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sGRNDetailIDs.Length > 0)
                    {
                        sGRNDetailIDs = sGRNDetailIDs.Remove(sGRNDetailIDs.Length - 1, 1);
                    }
                }
                GRNDetail oGRNDetail = new GRNDetail();
                oGRNDetail.GRNID = oGRN.GRNID;
                GRNDetailDA.Delete(tc, oGRNDetail, EnumDBOperation.Delete, nUserID, sGRNDetailIDs);
                #endregion

                #region Get GRN
                reader = GRNDA.Get(tc, oGRN.GRNID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oGRN = new GRN();
                    oGRN = CreateObject(oReader);
                }
                reader.Close();
                #endregion
                
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Save GRN. Because of " + e.Message, e);
                #endregion
            }
            return oGRN;
        }
        public GRN Approve(GRN oGRN, int nUserID)
        {
            TransactionContext tc = null;
            List<GRNDetail> oGRNDetails = new List<GRNDetail>();
            oGRNDetails = oGRN.GRNDetails;
            try
            {
                tc = TransactionContext.Begin(true);

                #region Approve Part
                IDataReader reader;
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "GRN", EnumRoleOperationType.Edit);
                reader = GRNDA.Approve(tc, oGRN, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oGRN = new GRN();
                    oGRN = CreateObject(oReader);
                }
                reader.Close();
                #endregion



                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Save GRN. Because of " + e.Message, e);
                #endregion
            }
            return oGRN;
        }

        public GRN UndoApprove(GRN oGRN, int nUserID)
        {
            TransactionContext tc = null;
            List<GRNDetail> oGRNDetails = new List<GRNDetail>();

            try
            {
                tc = TransactionContext.Begin(true);

                #region GRN Part
                IDataReader reader;
                reader = GRNDA.UndoApprove(tc, oGRN, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oGRN = new GRN();
                    oGRN = CreateObject(oReader);
                }
                reader.Close();
                #endregion



                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Save GRN. Because of " + e.Message, e);
                #endregion
            }
            return oGRN;
        }
        
        public GRN Receive(GRN oGRN, int nUserID)
        {
            string sGRNDetailIDs = "";
            TransactionContext tc = null;
            List<GRNDetail> oGRNDetails = new List<GRNDetail>();
            oGRNDetails = oGRN.GRNDetails;            
            try
            {
                tc = TransactionContext.Begin(true);
                

                #region GRN Detail Part
                if (oGRNDetails != null)
                {
                    foreach (GRNDetail oItem in oGRNDetails)
                    {
                        IDataReader readerdetail;
                        oItem.GRNID = oGRN.GRNID;
                        oItem.Amount = (oItem.ReceivedQty * oItem.UnitPrice);
                        if (oItem.GRNDetailID <= 0)
                        {
                            readerdetail = GRNDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = GRNDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sGRNDetailIDs = sGRNDetailIDs + oReaderDetail.GetString("GRNDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sGRNDetailIDs.Length > 0)
                    {
                        sGRNDetailIDs = sGRNDetailIDs.Remove(sGRNDetailIDs.Length - 1, 1);
                    }
                }
                GRNDetail oGRNDetail = new GRNDetail();
                oGRNDetail.GRNID = oGRN.GRNID;
                GRNDetailDA.Delete(tc, oGRNDetail, EnumDBOperation.Delete, nUserID, sGRNDetailIDs);
                #endregion

                #region GRN Received Part
                IDataReader reader;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.GRN, EnumRoleOperationType.Received);
                reader = GRNDA.Receive(tc, oGRN, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oGRN = new GRN();
                    oGRN = CreateObject(oReader);
                }
                reader.Close();
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                
                oGRN = new GRN();
                oGRN.ErrorMessage = e.Message;
                #endregion
            }
            return oGRN;
        }


        public GRN UpdateMRIRInfo(GRN oGRN, int nUserID)
        {
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);

                GRNDA.UpdateMRIRInfo(tc, oGRN);
                IDataReader reader = GRNDA.Get(tc, oGRN.GRNID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oGRN = new GRN();
                    oGRN = CreateObject(oReader);
                }
                reader.Close();
                #endregion



                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Save GRN. Because of " + e.Message, e);
                #endregion
            }
            return oGRN;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                GRN oGRN = new GRN();
                oGRN.GRNID = id;
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "GRN", EnumRoleOperationType.Delete);
                VoucherDA.CheckVoucherReference(tc, "GRN", "GRNID", oGRN.GRNID);
                GRNDA.Delete(tc, oGRN, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete GRN. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }
        public GRN UpdateVoucherEffect(GRN oGRN, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                GRNDA.UpdateVoucherEffect(tc, oGRN);
                IDataReader reader;
                reader = GRNDA.Get(tc, oGRN.GRNID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oGRN = new GRN();
                    oGRN = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oGRN = new GRN();
                oGRN.ErrorMessage = e.Message.Split('~')[0];

                #endregion
            }

            return oGRN;

        }
        public GRN Get(int id, int nUserId)
        {
            GRN oAccountHead = new GRN();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = GRNDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get GRN", e);
                #endregion
            }

            return oAccountHead;
        }



        public List<GRN> Gets(int nUserID)
        {
            List<GRN> oGRN = new List<GRN>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = GRNDA.Gets(tc);
                oGRN = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get GRN", e);
                #endregion
            }

            return oGRN;
        }
        public List<GRN> Gets(string sSQL,int nUserID)
        {
            List<GRN> oGRN = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
              
                reader = GRNDA.Gets(tc, sSQL);
                oGRN = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get GRN", e);
                #endregion
            }

            return oGRN;
        }

    }   
}