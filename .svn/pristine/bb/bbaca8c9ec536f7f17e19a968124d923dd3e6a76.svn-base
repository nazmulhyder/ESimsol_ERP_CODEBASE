using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{


    public class SparePartsRequisitionService : MarshalByRefObject, ISparePartsRequisitionService
    {
        #region Private functions and declaration
        private SparePartsRequisition MapObject(NullHandler oReader)
        {
            SparePartsRequisition oSparePartsRequisition = new SparePartsRequisition();
            oSparePartsRequisition.SparePartsRequisitionID = oReader.GetInt32("SparePartsRequisitionID");
            oSparePartsRequisition.RequisitionNo = oReader.GetString("RequisitionNo");
            oSparePartsRequisition.BUID = oReader.GetInt32("BUID");
            oSparePartsRequisition.SPStatus = (EnumSPStatus)oReader.GetInt32("SPStatus");
            oSparePartsRequisition.SPStatusInt = oReader.GetInt32("SPStatus");
            oSparePartsRequisition.IssueDate = oReader.GetDateTime("IssueDate");
            oSparePartsRequisition.CRID = oReader.GetInt32("CRID");
            oSparePartsRequisition.Remarks = oReader.GetString("Remarks");
            oSparePartsRequisition.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oSparePartsRequisition.ApprovedByName = oReader.GetString("ApprovedByName");
            oSparePartsRequisition.RequisitionBy = oReader.GetInt32("RequisitionBy");
            oSparePartsRequisition.RequisitionByName = oReader.GetString("RequisitionByName");
            oSparePartsRequisition.DeliveryByName = oReader.GetString("DeliveryByName");
            oSparePartsRequisition.CapitalResourceName = oReader.GetString("CapitalResourceName");
            oSparePartsRequisition.SparePartsRequisitionLogID = oReader.GetInt32("SparePartsRequisitionLogID");

            return oSparePartsRequisition;
        }

        private SparePartsRequisition CreateObject(NullHandler oReader)
        {
            SparePartsRequisition oSparePartsRequisition = new SparePartsRequisition();
            oSparePartsRequisition = MapObject(oReader);
            return oSparePartsRequisition;
        }

        private List<SparePartsRequisition> CreateObjects(IDataReader oReader)
        {
            List<SparePartsRequisition> oSparePartsRequisition = new List<SparePartsRequisition>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SparePartsRequisition oItem = CreateObject(oHandler);
                oSparePartsRequisition.Add(oItem);
            }
            return oSparePartsRequisition;
        }

        #endregion

        #region Interface implementation

        public SparePartsRequisition Save(SparePartsRequisition oSparePartsRequisition, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<SparePartsRequisitionDetail> oSparePartsRequisitionDetails = new List<SparePartsRequisitionDetail>();
            string sSparePartsRequisitionDetailIDs = "";
            oSparePartsRequisitionDetails = oSparePartsRequisition.SparePartsRequisitionDetails;

            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oSparePartsRequisition.SparePartsRequisitionID <= 0)
                {

                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.SparePartsRequisition, EnumRoleOperationType.Add);
                    reader = SparePartsRequisitionDA.InsertUpdate(tc, oSparePartsRequisition, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.SparePartsRequisition, EnumRoleOperationType.Edit);
                    VoucherDA.CheckVoucherReference(tc, "SparePartsRequisition", "SparePartsRequisitionID", oSparePartsRequisition.SparePartsRequisitionID);
                    reader = SparePartsRequisitionDA.InsertUpdate(tc, oSparePartsRequisition, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSparePartsRequisition = new SparePartsRequisition();
                    oSparePartsRequisition = CreateObject(oReader);
                }
                reader.Close();

                #region CR Detail Part
                if (oSparePartsRequisitionDetails != null)
                {
                    foreach (SparePartsRequisitionDetail oItem in oSparePartsRequisitionDetails)
                    {
                        IDataReader readerdetail;
                        oItem.SparePartsRequisitionID = oSparePartsRequisition.SparePartsRequisitionID;
                        if (oItem.SparePartsRequisitionDetailID <= 0)
                        {
                            readerdetail = SparePartsRequisitionDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = SparePartsRequisitionDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sSparePartsRequisitionDetailIDs = sSparePartsRequisitionDetailIDs + oReaderDetail.GetString("SparePartsRequisitionDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sSparePartsRequisitionDetailIDs.Length > 0)
                    {
                        sSparePartsRequisitionDetailIDs = sSparePartsRequisitionDetailIDs.Remove(sSparePartsRequisitionDetailIDs.Length - 1, 1);
                    }
                    SparePartsRequisitionDetail oSparePartsRequisitionDetail = new SparePartsRequisitionDetail();
                    oSparePartsRequisitionDetail.SparePartsRequisitionID = oSparePartsRequisition.SparePartsRequisitionID;
                    SparePartsRequisitionDetailDA.Delete(tc, oSparePartsRequisitionDetail, EnumDBOperation.Delete, nUserID, sSparePartsRequisitionDetailIDs);

                }

                #endregion

                #region Again Get CR
                reader = SparePartsRequisitionDA.Get(tc, oSparePartsRequisition.SparePartsRequisitionID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSparePartsRequisition = CreateObject(oReader);
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

                oSparePartsRequisition = new SparePartsRequisition();
                oSparePartsRequisition.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oSparePartsRequisition;
        }

        //public SparePartsRequisition Delivery(SparePartsRequisition oSparePartsRequisition, Int64 nUserID)
        //{
        //    TransactionContext tc = null;
        //    List<SparePartsRequisitionDetail> oSparePartsRequisitionDetails = new List<SparePartsRequisitionDetail>();
        //    oSparePartsRequisitionDetails = oSparePartsRequisition.SparePartsRequisitionDetails;
        //    try
        //    {
        //        tc = TransactionContext.Begin(true);
        //        IDataReader reader;
        //        AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.SparePartsRequisition, EnumRoleOperationType.Edit);
        //        reader = SparePartsRequisitionDA.InsertUpdate(tc, oSparePartsRequisition, EnumDBOperation.Update, nUserID);
        //        NullHandler oReader = new NullHandler(reader);
        //        if (reader.Read())
        //        {
        //            oSparePartsRequisition = new SparePartsRequisition();
        //            oSparePartsRequisition = CreateObject(oReader);
        //        }
        //        reader.Close();

        //        #region CR Detail Part
        //        if (oSparePartsRequisitionDetails != null)
        //        {
        //            foreach (SparePartsRequisitionDetail oItem in oSparePartsRequisitionDetails)
        //            {
        //                IDataReader readerdetail;
        //                oItem.SparePartsRequisitionID = oSparePartsRequisition.SparePartsRequisitionID;
        //                readerdetail = SparePartsRequisitionDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
        //                NullHandler oReaderDetail = new NullHandler(readerdetail);
        //                if (readerdetail.Read())
        //                {

        //                }
        //                readerdetail.Close();
        //            }
        //        }

        //        #endregion

        //        #region Delivery Effect
        //        oSparePartsRequisition.SPStatus = EnumSPStatus.Approve;
        //        reader = SparePartsRequisitionDA.ChangeStatus(tc, oSparePartsRequisition, EnumDBOperation.Insert, nUserID);
        //        oReader = new NullHandler(reader);
        //        if (reader.Read())
        //        {
        //            oSparePartsRequisition = CreateObject(oReader);
        //        }
        //        reader.Close();

        //        #endregion

        //        tc.End();
        //    }
        //    catch (Exception e)
        //    {
        //        #region Handle Exception
        //        if (tc != null)
        //            tc.HandleError();

        //        oSparePartsRequisition = new SparePartsRequisition();
        //        oSparePartsRequisition.ErrorMessage = e.Message.Split('!')[0];
        //        #endregion
        //    }
        //    return oSparePartsRequisition;
        //}
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                SparePartsRequisition oSparePartsRequisition = new SparePartsRequisition();
                oSparePartsRequisition.SparePartsRequisitionID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.SparePartsRequisition, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "SparePartsRequisition", id);
                VoucherDA.CheckVoucherReference(tc, "SparePartsRequisition", "SparePartsRequisitionID", oSparePartsRequisition.SparePartsRequisitionID);
                SparePartsRequisitionDA.Delete(tc, oSparePartsRequisition, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete SparePartsRequisition. Because of " + e.Message, e);
                #endregion
            }
            return "Delete successfully";
        }

        public SparePartsRequisition Get(int id, Int64 nUserId)
        {
            SparePartsRequisition oSparePartsRequisition = new SparePartsRequisition();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = SparePartsRequisitionDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSparePartsRequisition = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get SparePartsRequisition", e);
                #endregion
            }

            return oSparePartsRequisition;
        }

        public SparePartsRequisition GetLog(int id, Int64 nUserId)
        {
            SparePartsRequisition oSparePartsRequisition = new SparePartsRequisition();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = SparePartsRequisitionDA.GetLog(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSparePartsRequisition = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get SparePartsRequisition", e);
                #endregion
            }

            return oSparePartsRequisition;
        }

        public SparePartsRequisition ChangeStatus(SparePartsRequisition oSparePartsRequisition, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader = null;
                if (oSparePartsRequisition.SPStatus == EnumSPStatus.Approve)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.SparePartsRequisition, EnumRoleOperationType.Approved);
                }

                if (oSparePartsRequisition.SPStatus==EnumSPStatus.RequestForApproval)
                {
                    reader = SparePartsRequisitionDA.InsertUpdate(tc, oSparePartsRequisition, EnumDBOperation.Request, nUserID);
                }
                else if (oSparePartsRequisition.SPStatus == EnumSPStatus.Approve)
                {
                    reader = SparePartsRequisitionDA.InsertUpdate(tc, oSparePartsRequisition, EnumDBOperation.Approval, nUserID);
                }
                else if (oSparePartsRequisition.SPStatus == EnumSPStatus.InStore)
                {
                    reader = SparePartsRequisitionDA.InsertUpdate(tc, oSparePartsRequisition, EnumDBOperation.Start, nUserID);
                }
                else if (oSparePartsRequisition.SPStatus == EnumSPStatus.PartialDisverse)
                {
                    reader = SparePartsRequisitionDA.InsertUpdate(tc, oSparePartsRequisition, EnumDBOperation.Settlement, nUserID);
                }
                else if (oSparePartsRequisition.SPStatus == EnumSPStatus.Disburse)
                {
                    reader = SparePartsRequisitionDA.InsertUpdate(tc, oSparePartsRequisition, EnumDBOperation.Disburse, nUserID);
                }

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSparePartsRequisition = new SparePartsRequisition();
                    oSparePartsRequisition = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                oSparePartsRequisition.ErrorMessage = Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save SparePartsRequisitionDetail. Because of " + e.Message, e);
                #endregion
            }
            return oSparePartsRequisition;
        }
        public List<SparePartsRequisition> Gets(Int64 nUserID)
        {
            List<SparePartsRequisition> oSparePartsRequisition = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SparePartsRequisitionDA.Gets(tc);
                oSparePartsRequisition = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SparePartsRequisition", e);
                #endregion
            }

            return oSparePartsRequisition;
        }

        public List<SparePartsRequisition> Gets(string sSQL, Int64 nUserID)
        {
            List<SparePartsRequisition> oSparePartsRequisition = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SparePartsRequisitionDA.Gets(tc, sSQL);
                oSparePartsRequisition = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SparePartsRequisition", e);
                #endregion
            }

            return oSparePartsRequisition;
        }
        public SparePartsRequisition AcceptSparePartsRequisitionRevise(SparePartsRequisition oSparePartsRequisition, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<SparePartsRequisitionDetail> oSparePartsRequisitionDetails = new List<SparePartsRequisitionDetail>();
                SparePartsRequisitionDetail oSparePartsRequisitionDetail = new SparePartsRequisitionDetail();
                oSparePartsRequisitionDetails = oSparePartsRequisition.SparePartsRequisitionDetails;
                string sSparePartsRequisitionDetailIDs = "";

                if (oSparePartsRequisition.SparePartsRequisitionID > 0)
                {
                    #region SparePartsRequisition part
                    IDataReader reader;
                    reader = SparePartsRequisitionDA.AcceptSparePartsRequisitionRevise(tc, oSparePartsRequisition, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oSparePartsRequisition = new SparePartsRequisition();
                        oSparePartsRequisition = CreateObject(oReader);
                    }
                    reader.Close();

                    #endregion

                    #region SparePartsRequisition Detail Detail Part
                    if (oSparePartsRequisitionDetails != null)
                    {
                        foreach (SparePartsRequisitionDetail oItem in oSparePartsRequisitionDetails)
                        {
                            IDataReader readerdetail;
                            oItem.SparePartsRequisitionID = oSparePartsRequisition.SparePartsRequisitionID;
                            if (oItem.SparePartsRequisitionDetailID <= 0)
                            {
                                readerdetail = SparePartsRequisitionDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readerdetail = SparePartsRequisitionDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                            }
                            NullHandler oReaderDetail = new NullHandler(readerdetail);
                            if (readerdetail.Read())
                            {
                                sSparePartsRequisitionDetailIDs = sSparePartsRequisitionDetailIDs + oReaderDetail.GetString("SparePartsRequisitionDetailID") + ",";
                            }
                            readerdetail.Close();
                        }
                        if (sSparePartsRequisitionDetailIDs.Length > 0)
                        {
                            sSparePartsRequisitionDetailIDs = sSparePartsRequisitionDetailIDs.Remove(sSparePartsRequisitionDetailIDs.Length - 1, 1);
                        }
                        oSparePartsRequisitionDetail = new SparePartsRequisitionDetail();
                        oSparePartsRequisitionDetail.SparePartsRequisitionID = oSparePartsRequisition.SparePartsRequisitionID;
                        SparePartsRequisitionDetailDA.Delete(tc, oSparePartsRequisitionDetail, EnumDBOperation.Delete, nUserID, sSparePartsRequisitionDetailIDs);
                    }

                    #endregion

                    #region SparePartsRequisition Get
                    reader = SparePartsRequisitionDA.Get(tc, oSparePartsRequisition.SparePartsRequisitionID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oSparePartsRequisition = CreateObject(oReader);
                    }
                    reader.Close();
                }
                    #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                oSparePartsRequisition.ErrorMessage = Message;

                #endregion
            }
            return oSparePartsRequisition;
        }

        public SparePartsRequisition UpdateVoucherEffect(SparePartsRequisition oSparePartsRequisition, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                SparePartsRequisitionDA.UpdateVoucherEffect(tc, oSparePartsRequisition);
                IDataReader reader;
                reader = SparePartsRequisitionDA.Get(tc, oSparePartsRequisition.SparePartsRequisitionID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSparePartsRequisition = new SparePartsRequisition();
                    oSparePartsRequisition = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oSparePartsRequisition = new SparePartsRequisition();
                oSparePartsRequisition.ErrorMessage = e.Message.Split('~')[0];

                #endregion
            }

            return oSparePartsRequisition;

        }
        #endregion
    }
}
