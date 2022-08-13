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
    public class YarnRequisitionService : MarshalByRefObject, IYarnRequisitionService
    {
        #region Private functions and declaration

        private YarnRequisition MapObject(NullHandler oReader)
        {
            YarnRequisition oYarnRequisition = new YarnRequisition();
            oYarnRequisition.YarnRequisitionID = oReader.GetInt32("YarnRequisitionID");
            oYarnRequisition.BUID = oReader.GetInt32("BUID");
            oYarnRequisition.RequisitionNo = oReader.GetString("RequisitionNo");
            oYarnRequisition.RequisitionDate = oReader.GetDateTime("RequisitionDate");
            oYarnRequisition.MerchandiserID = oReader.GetInt32("MerchandiserID");
            oYarnRequisition.BusinessSessionID = oReader.GetInt32("BusinessSessionID");
            oYarnRequisition.BuyerID = oReader.GetInt32("BuyerID");
            oYarnRequisition.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oYarnRequisition.Remarks = oReader.GetString("Remarks");
            oYarnRequisition.BUName = oReader.GetString("BUName");
            oYarnRequisition.BuyerName = oReader.GetString("BuyerName");
            oYarnRequisition.MerchandiserName = oReader.GetString("MerchandiserName");
            oYarnRequisition.ApprovedByName = oReader.GetString("ApprovedByName");
            oYarnRequisition.SessionName = oReader.GetString("SessionName");
            return oYarnRequisition;
        }

        private YarnRequisition CreateObject(NullHandler oReader)
        {
            YarnRequisition oYarnRequisition = new YarnRequisition();
            oYarnRequisition = MapObject(oReader);
            return oYarnRequisition;
        }

        private List<YarnRequisition> CreateObjects(IDataReader oReader)
        {
            List<YarnRequisition> oYarnRequisition = new List<YarnRequisition>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                YarnRequisition oItem = CreateObject(oHandler);
                oYarnRequisition.Add(oItem);
            }
            return oYarnRequisition;
        }

        #endregion

        #region Interface implementation
        public YarnRequisition Save(YarnRequisition oYarnRequisition, Int64 nUserID)
        {
            YarnRequisitionDetail oYarnRequisitionDetail = new YarnRequisitionDetail();
            YarnRequisitionProduct oYarnRequisitionProduct = new YarnRequisitionProduct();
            List<YarnRequisitionDetail> oYarnRequisitionDetails = new List<YarnRequisitionDetail>();
            List<YarnRequisitionProduct> oYarnRequisitionProducts = new List<YarnRequisitionProduct>();
            oYarnRequisitionDetails = oYarnRequisition.YarnRequisitionDetails;
            oYarnRequisitionProducts = oYarnRequisition.YarnRequisitionProducts;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                #region YarnRequisition
                IDataReader reader;
                if (oYarnRequisition.YarnRequisitionID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.YarnRequisition, EnumRoleOperationType.Add);
                    reader = YarnRequisitionDA.InsertUpdate(tc, oYarnRequisition, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.YarnRequisition, EnumRoleOperationType.Edit);
                    reader = YarnRequisitionDA.InsertUpdate(tc, oYarnRequisition, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oYarnRequisition = new YarnRequisition();
                    oYarnRequisition = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region YarnRequisitionDetail
                if (oYarnRequisition.YarnRequisitionID > 0)
                {
                    string sYarnRequisitionDetailIDs = "";
                    if (oYarnRequisitionDetails.Count > 0)
                    {
                        IDataReader readerdetail;
                        foreach (YarnRequisitionDetail oYRD in oYarnRequisitionDetails)
                        {
                            oYRD.YarnRequisitionID = oYarnRequisition.YarnRequisitionID;
                            if (oYRD.YarnRequisitionDetailID <= 0)
                            {
                                readerdetail = YarnRequisitionDetailDA.InsertUpdate(tc, oYRD, EnumDBOperation.Insert, "", nUserID);
                            }
                            else
                            {
                                readerdetail = YarnRequisitionDetailDA.InsertUpdate(tc, oYRD, EnumDBOperation.Update, "", nUserID);

                            }
                            NullHandler oReaderDevRecapdetail = new NullHandler(readerdetail);
                            int nYarnRequisitionDetailID = 0;
                            if (readerdetail.Read())
                            {
                                nYarnRequisitionDetailID = oReaderDevRecapdetail.GetInt32("YarnRequisitionDetailID");
                                sYarnRequisitionDetailIDs = sYarnRequisitionDetailIDs + oReaderDevRecapdetail.GetString("YarnRequisitionDetailID") + ",";
                            }
                            readerdetail.Close();
                        }
                    }
                    if (sYarnRequisitionDetailIDs.Length > 0)
                    {
                        sYarnRequisitionDetailIDs = sYarnRequisitionDetailIDs.Remove(sYarnRequisitionDetailIDs.Length - 1, 1);
                    }
                    oYarnRequisitionDetail = new YarnRequisitionDetail();
                    oYarnRequisitionDetail.YarnRequisitionID = oYarnRequisition.YarnRequisitionID;
                    YarnRequisitionDetailDA.Delete(tc, oYarnRequisitionDetail, EnumDBOperation.Delete, sYarnRequisitionDetailIDs, nUserID);
                }
                #endregion

                #region Yarn Requisition Product
                if (oYarnRequisition.YarnRequisitionID > 0)
                {
                    string sYarnRequisitionProductIDs = "";
                    if (oYarnRequisitionProducts.Count > 0)
                    {
                        IDataReader readerYarnRequisitionProduct;
                        foreach (YarnRequisitionProduct oYRP in oYarnRequisitionProducts)
                        {
                            oYRP.YarnRequisitionID = oYarnRequisition.YarnRequisitionID;
                            if (oYRP.YarnRequisitionProductID <= 0)
                            {
                                readerYarnRequisitionProduct = YarnRequisitionProductDA.InsertUpdate(tc, oYRP, EnumDBOperation.Insert, "", nUserID);
                            }
                            else
                            {
                                readerYarnRequisitionProduct = YarnRequisitionProductDA.InsertUpdate(tc, oYRP, EnumDBOperation.Update, "", nUserID);
                            }
                            NullHandler oReaderYarnRequisitionProduct = new NullHandler(readerYarnRequisitionProduct);
                            int nYarnRequisitionProductID = 0;
                            if (readerYarnRequisitionProduct.Read())
                            {
                                nYarnRequisitionProductID = oReaderYarnRequisitionProduct.GetInt32("YarnRequisitionProductID");
                                sYarnRequisitionProductIDs = sYarnRequisitionProductIDs + oReaderYarnRequisitionProduct.GetString("YarnRequisitionProductID") + ",";
                            }
                            readerYarnRequisitionProduct.Close();
                        }
                    }
                    if (sYarnRequisitionProductIDs.Length > 0)
                    {
                        sYarnRequisitionProductIDs = sYarnRequisitionProductIDs.Remove(sYarnRequisitionProductIDs.Length - 1, 1);
                    }
                    oYarnRequisitionProduct = new YarnRequisitionProduct();
                    oYarnRequisitionProduct.YarnRequisitionID = oYarnRequisition.YarnRequisitionID;
                    YarnRequisitionProductDA.Delete(tc, oYarnRequisitionProduct, EnumDBOperation.Delete, sYarnRequisitionProductIDs, nUserID);
                }
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oYarnRequisition = new YarnRequisition();
                    oYarnRequisition.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oYarnRequisition;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                YarnRequisition oYarnRequisition = new YarnRequisition();
                oYarnRequisition.YarnRequisitionID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.YarnRequisition, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "YarnRequisition", id);
                YarnRequisitionDA.Delete(tc, oYarnRequisition, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exceptionif (tc != null)
                tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }

        public YarnRequisition Get(int id, Int64 nUserId)
        {
            YarnRequisition oYarnRequisition = new YarnRequisition();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = YarnRequisitionDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oYarnRequisition = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get YarnRequisition", e);
                #endregion
            }
            return oYarnRequisition;
        }

        public List<YarnRequisition> Gets(Int64 nUserID)
        {
            List<YarnRequisition> oYarnRequisitions = new List<YarnRequisition>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = YarnRequisitionDA.Gets(tc);
                oYarnRequisitions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                YarnRequisition oYarnRequisition = new YarnRequisition();
                oYarnRequisition.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oYarnRequisitions;
        }

        public List<YarnRequisition> Gets(string sSQL, Int64 nUserID)
        {
            List<YarnRequisition> oYarnRequisitions = new List<YarnRequisition>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = YarnRequisitionDA.Gets(tc, sSQL);
                oYarnRequisitions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get YarnRequisition", e);
                #endregion
            }
            return oYarnRequisitions;
        }

        public YarnRequisition Approve(YarnRequisition oYarnRequisition, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.YarnRequisition, EnumRoleOperationType.Approved);
                reader = YarnRequisitionDA.InsertUpdate(tc, oYarnRequisition, EnumDBOperation.Approval, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oYarnRequisition = new YarnRequisition();
                    oYarnRequisition = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oYarnRequisition = new YarnRequisition();
                oYarnRequisition.ErrorMessage = e.Message.Split('~')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save YarnRequisition. Because of " + e.Message, e);
                #endregion
            }
            return oYarnRequisition;
        }

        public YarnRequisition UnApprove(YarnRequisition oYarnRequisition, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.YarnRequisition, EnumRoleOperationType.Approved);
                reader = YarnRequisitionDA.InsertUpdate(tc, oYarnRequisition, EnumDBOperation.UnApproval, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oYarnRequisition = new YarnRequisition();
                    oYarnRequisition = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oYarnRequisition = new YarnRequisition();
                oYarnRequisition.ErrorMessage = e.Message.Split('~')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save YarnRequisition. Because of " + e.Message, e);
                #endregion
            }
            return oYarnRequisition;
        }
        #endregion
    }
}
