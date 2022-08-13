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
    public class NOAService : MarshalByRefObject, INOAService
    {
        #region Private functions and declaration
        public static NOA MapObject(NullHandler oReader)
        {
            NOA oNOA = new NOA();
            oNOA.NOALogID = oReader.GetInt32("NOALogID");
            oNOA.NOAID = oReader.GetInt32("NOAID");
            oNOA.NOANo = oReader.GetString("NOANo");
            oNOA.NOADate = oReader.GetDateTime("NOADate");
            oNOA.PrepareBy = oReader.GetInt32("PrepareBy");
            oNOA.ApprovedByName = oReader.GetString("ApprovedByName");
            oNOA.ApproveBy = oReader.GetInt32("ApproveBy");
            oNOA.PrepareByName = oReader.GetString("PrepareByName");
            oNOA.ApproveDate = oReader.GetDateTime("ApproveDate");
            oNOA.Note = oReader.GetString("Note");
            oNOA.BUID = oReader.GetInt32("BUID");
            oNOA.BUCode = oReader.GetString("BUCode");
            oNOA.BUName = oReader.GetString("BUName");
            oNOA.ApprovalHead = oReader.GetString("ApprovalHead");
            oNOA.DiscountInPercent = oReader.GetDouble("DiscountInPercent");
            oNOA.DiscountInAmount = oReader.GetDouble("DiscountInAmount");
            oNOA.NOADetailCount = oReader.GetInt32("NOADetailCount");
            return oNOA;
        }
        public static NOA CreateObject(NullHandler oReader)
        {
            NOA oNOA = new NOA();
            oNOA = MapObject(oReader);
            return oNOA;
        }
        private List<NOA> CreateObjects(IDataReader oReader)
        {
            List<NOA> oNOA = new List<NOA>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                NOA oItem = CreateObject(oHandler);
                oNOA.Add(oItem);
            }
            return oNOA;
        }

        #endregion

        #region Function
        private List<NOAQuotation> GetNOAQuotations(int nNOADetailID, List<NOAQuotation> oNOAQuotations)
        {
            List<NOAQuotation> oTempNOAQuotations = new List<NOAQuotation>();
            foreach (NOAQuotation oItem in oNOAQuotations)
            {
                if (oItem.NOAID == nNOADetailID)
                {
                    oTempNOAQuotations.Add(oItem);
                }
            }

            return oTempNOAQuotations;
        }
        #endregion

        #region Interface implementation
        public NOA Save(NOA oNOA, int nUserId)
        {

            TransactionContext tc = null;
            List<NOADetail> oNOADetails = new List<NOADetail>();
            List<NOADetail> oTempNOADetails = new List<NOADetail>();
            List<NOAQuotation> oNOAQuotations = new List<NOAQuotation>();
            List<NOARequisition> oNOARequisitions = new List<NOARequisition>();
            NOADetail oNOADetail = new NOADetail();
            NOARequisition oNOARequisition = new NOARequisition();
            oNOADetails = oNOA.NOADetailLst;
            oNOARequisitions = oNOA.NOARequisitionList;
            string sDetailIDs = "";
            string sReqIDs = "";
            try
            {
                tc = TransactionContext.Begin(true);
                #region NOA
                IDataReader reader;
                if (oNOA.NOAID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.NOA, EnumRoleOperationType.Add);
                    reader = NOADA.InsertUpdate(tc, oNOA, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.NOA, EnumRoleOperationType.Edit);
                    reader = NOADA.InsertUpdate(tc, oNOA, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oNOA = new NOA();
                    oNOA = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region NOA Detail
                if (oNOADetails.Count > 0)
                {
                    foreach (NOADetail oItem in oNOADetails)
                    {
                        IDataReader readerdetail;
                        oItem.NOAID = oNOA.NOAID;
                        if (oItem.NOADetailID <= 0)
                        {
                            readerdetail = NOADetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserId,"");
                        }
                        else
                        {
                            readerdetail = NOADetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserId,"");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sDetailIDs = sDetailIDs + oReaderDetail.GetString("NOADetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sDetailIDs.Length > 0)
                    {
                        sDetailIDs = sDetailIDs.Remove(sDetailIDs.Length - 1, 1);
                    }
                    oNOADetail = new NOADetail();
                    oNOADetail.NOAID = oNOA.NOAID;
                    NOADetailDA.Delete(tc, oNOADetail, EnumDBOperation.Delete, nUserId, sDetailIDs); 

                }
                #endregion


                #region NOA Requisition
                if (oNOARequisitions.Count > 0)
                {
                    foreach (NOARequisition oItem in oNOARequisitions)
                    {
                        IDataReader readerdetail;
                        oItem.NOAID = oNOA.NOAID;
                        if (oItem.NOARequisitionID <= 0)
                        {
                            readerdetail = NOARequisitionDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserId, "");
                        }
                        else
                        {
                            readerdetail = NOARequisitionDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserId, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sReqIDs = sReqIDs + oReaderDetail.GetString("NOARequisitionID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sReqIDs.Length > 0)
                    {
                        sReqIDs = sReqIDs.Remove(sReqIDs.Length - 1, 1);
                    }
                    oNOARequisition = new NOARequisition();
                    oNOARequisition.NOAID = oNOA.NOAID;
                    NOARequisitionDA.Delete(tc, oNOARequisition, EnumDBOperation.Delete, nUserId, sReqIDs);

                }
                #endregion

                reader = NOADA.Get(tc, oNOA.NOAID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oNOA = CreateObject(oReader);
                }
                reader.Close();
                IDataReader readerdetails = null;
                readerdetails = NOADetailDA.Gets( tc,oNOA.NOAID);
                oNOA.NOADetailLst = NOADetailService.CreateObjects(readerdetails);
                readerdetails.Close();
                tc.End();
            }

            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oNOA = new NOA();
                oNOA.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            
            return oNOA;
        }
        public NOA AcceptRevise(NOA oNOA, Int64 nUserId)
        {

            TransactionContext tc = null;
            List<NOADetail> oNOADetails = new List<NOADetail>();
            List<NOADetail> oTempNOADetails = new List<NOADetail>();
            List<NOAQuotation> oNOAQuotations = new List<NOAQuotation>();
            List<NOARequisition> oNOARequisitions = new List<NOARequisition>();
            NOADetail oNOADetail = new NOADetail();
            NOARequisition oNOARequisition = new NOARequisition();
            oNOADetails = oNOA.NOADetailLst;
            oNOARequisitions = oNOA.NOARequisitionList;
            string sDetailIDs = "";
            string sReqIDs = "";
            try
            {
                tc = TransactionContext.Begin(true);
                #region NOA
                IDataReader reader;
                if (oNOA.NOAID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.NOA, EnumRoleOperationType.Add);
                    reader = NOADA.InsertUpdate(tc, oNOA, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.NOA, EnumRoleOperationType.Edit);
                    reader = NOADA.InsertUpdate(tc, oNOA, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oNOA = new NOA();
                    oNOA = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region NOA Detail
                if (oNOADetails.Count > 0)
                {
                    foreach (NOADetail oItem in oNOADetails)
                    {
                        IDataReader readerdetail;
                        oItem.NOAID = oNOA.NOAID;
                        if (oItem.NOADetailID <= 0)
                        {
                            readerdetail = NOADetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserId, "");
                        }
                        else
                        {
                            readerdetail = NOADetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserId, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sDetailIDs = sDetailIDs + oReaderDetail.GetString("NOADetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sDetailIDs.Length > 0)
                    {
                        sDetailIDs = sDetailIDs.Remove(sDetailIDs.Length - 1, 1);
                    }
                    oNOADetail = new NOADetail();
                    oNOADetail.NOAID = oNOA.NOAID;
                    NOADetailDA.Delete(tc, oNOADetail, EnumDBOperation.Delete, nUserId, sDetailIDs);

                }
                #endregion


                #region NOA Requisition
                if (oNOARequisitions.Count > 0)
                {
                    foreach (NOARequisition oItem in oNOARequisitions)
                    {
                        IDataReader readerdetail;
                        oItem.NOAID = oNOA.NOAID;
                        if (oItem.NOARequisitionID <= 0)
                        {
                            readerdetail = NOARequisitionDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserId, "");
                        }
                        else
                        {
                            readerdetail = NOARequisitionDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserId, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sReqIDs = sReqIDs + oReaderDetail.GetString("NOARequisitionID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sReqIDs.Length > 0)
                    {
                        sReqIDs = sReqIDs.Remove(sReqIDs.Length - 1, 1);
                    }
                    oNOARequisition = new NOARequisition();
                    oNOARequisition.NOAID = oNOA.NOAID;
                    NOARequisitionDA.Delete(tc, oNOARequisition, EnumDBOperation.Delete, nUserId, sReqIDs);

                }
                #endregion

                reader = NOADA.Get(tc, oNOA.NOAID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oNOA = CreateObject(oReader);
                }
                reader.Close();
                IDataReader readerdetails = null;
                readerdetails = NOADetailDA.Gets(tc, oNOA.NOAID);
                oNOA.NOADetailLst = NOADetailService.CreateObjects(readerdetails);
                readerdetails.Close();
                tc.End();
            }

            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oNOA = new NOA();
                oNOA.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oNOA;
        }

        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                NOA oNOA = new NOA();
                oNOA.NOAID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.NOA, EnumRoleOperationType.Delete);
                NOADA.Delete(tc, oNOA, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete NOA. Because of " + e.Message, e);
                #endregion
            }
            return "Data Delete Successfully";
        }
        public NOA Get(long id, int nUserId)
        {
            NOA oAccountHead = new NOA();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = NOADA.Get(tc, id);
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
                throw new ServiceException(e.Message);
                #endregion
            }

            return oAccountHead;
        }
        public NOA GetByLog(long id, int nUserId)
        {
            NOA oAccountHead = new NOA();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = NOADA.GetByLog(tc, id);
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
                throw new ServiceException(e.Message);
                #endregion
            }

            return oAccountHead;
        }

        public NOA Approve(NOA oNOA, int nUserId)
        {
            NOADetail oNOADetail = new NOADetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                #region Update NOA Detail
                if (oNOA.NOADetailLst.Count > 0)
                {
                    foreach (NOADetail oItem in oNOA.NOADetailLst)
                    {
                        IDataReader readerdetail;
                        readerdetail = NOADetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserId,"");
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            oNOADetail = new NOADetail();
                            oNOADetail = NOADetailService.CreateObject(oReaderDetail);
                        }
                        readerdetail.Close();
                    }
                }
                #endregion


                IDataReader reader;
                // AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "NOA", EnumRoleOperationType.Approved);
                oNOA.ApproveBy = nUserId;
                oNOA.ApproveDate = DateTime.Now;
                reader = NOADA.InsertUpdate(tc, oNOA, EnumDBOperation.Approval, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oNOA = new NOA();
                    oNOA = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oNOA = new NOA();
                oNOA.ErrorMessage = e.Message.Split('~')[0];
                //ExceptionLog.Write(e);

                //throw new ServiceException("Failed to Save NOA. Because of " + e.Message, e);
                #endregion
            }
            return oNOA;
        }

        //UndoApprove
        public NOA UndoApprove(NOA oNOA, int nUserId)
        {
            NOADetail oNOADetail = new NOADetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.NOA, EnumRoleOperationType.UnApproved);

                //NOADA.UndoApprove(tc, oNOA.NOAID);
                //reader = NOADA.Get(tc, oNOA.NOAID);
                reader = NOADA.InsertUpdate(tc, oNOA, EnumDBOperation.UnApproval, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oNOA = new NOA();
                    oNOA = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oNOA = new NOA();
                oNOA.ErrorMessage = e.Message.Split('~')[0];
                //ExceptionLog.Write(e);

                //throw new ServiceException("Failed to Save NOA. Because of " + e.Message, e);
                #endregion
            }
            return oNOA;
        }
        public NOA RequestNOARevise(NOA oNOA, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<NOADetail> oNOADetails = new List<NOADetail>();
                NOA oNOADetail = new NOA();


                oNOADetails = oNOA.NOADetailLst;


                #region NOA  part

                if (oNOA.NOAID > 0)
                {
                    IDataReader reader;
                    reader = NOADA.RequestNOARevise(tc, oNOA, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oNOA = new NOA();
                        oNOA = CreateObject(oReader);
                    }
                    reader.Close();

                #endregion



                    #region PurchaseRequisition Get
                    reader = NOADA.Get(tc, oNOA.NOAID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oNOA = CreateObject(oReader);
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
                oNOA.ErrorMessage = Message;

                #endregion
            }
            return oNOA;
        }
        public List<NOA> GetsWaitForApproval(int nUserId)
        {
            List<NOA> oNOA = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = NOADA.GetsWaitForApproval(tc);
                oNOA = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }

            return oNOA;
        }
      
        public List<NOA> Gets(string sSQL, int nUserId)
        {
            List<NOA> oNOA = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = NOADA.Gets(tc, sSQL);
                oNOA = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }

            return oNOA;
        }


        #endregion
    }   
    
    

}
