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

    public class PurchaseRequisitionService : MarshalByRefObject, IPurchaseRequisitionService
    {
        #region Private functions and declaration
        private PurchaseRequisition MapObject(NullHandler oReader)
        {
            PurchaseRequisition oPurchaseRequisition = new PurchaseRequisition();
            oPurchaseRequisition.PRID = oReader.GetInt32("PRID");
            oPurchaseRequisition.ApprovalSequence = oReader.GetInt32("ApprovalSequence");
            oPurchaseRequisition.DepartmentID = oReader.GetInt32("DepartmentID");
            oPurchaseRequisition.FinishByID = oReader.GetInt32("FinishByID");
            oPurchaseRequisition.CancelByID = oReader.GetInt32("CancelByID");


            oPurchaseRequisition.RequisitionByName = oReader.GetString("RequisitionByName");
            oPurchaseRequisition.RequisitionByCode = oReader.GetString("RequisitionByCode");
            oPurchaseRequisition.EmployeeDesignationType = (EnumEmployeeDesignationType)oReader.GetInt32("EmployeeDesignationType");
            oPurchaseRequisition.PriortyLevel = (EnumPriortyLevel)oReader.GetInt32("PriortyLevel");
            oPurchaseRequisition.PriortyLevelInt = oReader.GetInt32("PriortyLevel");
     
            oPurchaseRequisition.PRNo = oReader.GetString("PRNo");
            oPurchaseRequisition.DepartmentName = oReader.GetString("DepartmentName");
            oPurchaseRequisition.FinishByName = oReader.GetString("FinishByName");
            oPurchaseRequisition.CancelByName = oReader.GetString("CancelByName");
            oPurchaseRequisition.PRDate = oReader.GetDateTime("PRDate");
            oPurchaseRequisition.RequirementDate = oReader.GetDateTime("RequirementDate");
            oPurchaseRequisition.RequisitionBy = oReader.GetInt32("RequisitionBy");
            oPurchaseRequisition.ApproveBy = oReader.GetInt32("ApproveBy");
            oPurchaseRequisition.Note = oReader.GetString("Note");
            oPurchaseRequisition.IDNo = oReader.GetString("IDNo");
            oPurchaseRequisition.BUID = oReader.GetInt32("BUID"); 
            oPurchaseRequisition.BUCode = oReader.GetString("BUCode");
            oPurchaseRequisition.BUName = oReader.GetString("BUName");
            oPurchaseRequisition.PrepareByName = oReader.GetString("PrepareByName");
            oPurchaseRequisition.ApprovedByName = oReader.GetString("ApprovedByName");
            oPurchaseRequisition.ApprovalStatus = oReader.GetString("ApprovalStatus");
            oPurchaseRequisition.Status = oReader.GetInt32("Status");
            oPurchaseRequisition.TotalDetail = oReader.GetInt32("TotalDetail");
            oPurchaseRequisition.TotalConfirm = oReader.GetInt32("TotalConfirm");
            if ( Convert.ToDateTime(oPurchaseRequisition.RequirementDate.ToString("dd MMM yyyy")) == DateTime.Today)
            {
                oPurchaseRequisition.RequiremenStatus = 1;
            }
            else if (oPurchaseRequisition.RequirementDate < DateTime.Today)
            {
                oPurchaseRequisition.RequiremenStatus = 2;
            }
            
            return oPurchaseRequisition;
        }

        private PurchaseRequisition CreateObject(NullHandler oReader)
        {
            PurchaseRequisition oPurchaseRequisition = new PurchaseRequisition();
            oPurchaseRequisition = MapObject(oReader);
            return oPurchaseRequisition;
        }

        private List<PurchaseRequisition> CreateObjects(IDataReader oReader)
        {
            List<PurchaseRequisition> oPurchaseRequisition = new List<PurchaseRequisition>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PurchaseRequisition oItem = CreateObject(oHandler);
                oPurchaseRequisition.Add(oItem);
            }
            return oPurchaseRequisition;
        }

        #endregion

        #region Interface implementation
        public PurchaseRequisitionService() { }

        public PurchaseRequisition Save(PurchaseRequisition oPurchaseRequisition, Int64 nUserID)
        {
            List<PurchaseRequisitionDetail> oPurchaseRequisitionDetails = new List<PurchaseRequisitionDetail>();
            oPurchaseRequisitionDetails = oPurchaseRequisition.PurchaseRequisitionDetails;
            string sPurchaseRequisitionDetailIDs = "";
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oPurchaseRequisition.PRID <= 0)
                {

                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.PurchaseRequisition, EnumRoleOperationType.Add);
                    reader = PurchaseRequisitionDA.InsertUpdate(tc, oPurchaseRequisition, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.PurchaseRequisition, EnumRoleOperationType.Edit);
                    reader = PurchaseRequisitionDA.InsertUpdate(tc, oPurchaseRequisition, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPurchaseRequisition = new PurchaseRequisition();
                    oPurchaseRequisition = CreateObject(oReader);
                }
                reader.Close();
                #region Buyer Yarn Detail Part
                if (oPurchaseRequisitionDetails != null)
                {
                    foreach (PurchaseRequisitionDetail oItem in oPurchaseRequisitionDetails)
                    {
                        IDataReader readerdetail;
                        oItem.PRID = oPurchaseRequisition.PRID;
                        oItem.Note = oItem.Note == null ? "" : oItem.Note;
                        if (oItem.PRDetailID <= 0)
                        {
                            readerdetail = PurchaseRequisitionDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = PurchaseRequisitionDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sPurchaseRequisitionDetailIDs = sPurchaseRequisitionDetailIDs + oReaderDetail.GetString("PRDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sPurchaseRequisitionDetailIDs.Length > 0)
                    {
                        sPurchaseRequisitionDetailIDs = sPurchaseRequisitionDetailIDs.Remove(sPurchaseRequisitionDetailIDs.Length - 1, 1);
                    }
                    PurchaseRequisitionDetail oPurchaseRequisitionDetail = new PurchaseRequisitionDetail();
                    oPurchaseRequisitionDetail.PRID = oPurchaseRequisition.PRID;
                    PurchaseRequisitionDetailDA.Delete(tc, oPurchaseRequisitionDetail, EnumDBOperation.Delete, nUserID, sPurchaseRequisitionDetailIDs);

                }

                IDataReader readerdetails = null;
                readerdetails = PurchaseRequisitionDetailDA.Gets(oPurchaseRequisition.PRID, tc);
                PurchaseRequisitionDetailService obj = new PurchaseRequisitionDetailService();
                oPurchaseRequisition.PurchaseRequisitionDetails = obj.CreateObjects(readerdetails);
                readerdetails.Close();

                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oPurchaseRequisition = new PurchaseRequisition();
                oPurchaseRequisition.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oPurchaseRequisition;
        }

        #region Revise Requisition

        public PurchaseRequisition AcceptRevise(PurchaseRequisition oPurchaseRequisition, Int64 nUserID)
        {
            List<PurchaseRequisitionDetail> oPurchaseRequisitionDetails = new List<PurchaseRequisitionDetail>();
            oPurchaseRequisitionDetails = oPurchaseRequisition.PurchaseRequisitionDetails;
            string sPurchaseRequisitionDetailIDs = "";
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.PurchaseRequisition, EnumRoleOperationType.Edit);
                oPurchaseRequisition.Status = (int)EnumPurchaseRequisitionStatus.Initialized;
                reader = PurchaseRequisitionDA.UpdateForRevise(tc, oPurchaseRequisition, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPurchaseRequisition = new PurchaseRequisition();
                    oPurchaseRequisition = CreateObject(oReader);
                }
                reader.Close();
                #region PurchaseRequisitionDetail Part
                if (oPurchaseRequisitionDetails != null)
                {
                    foreach (PurchaseRequisitionDetail oItem in oPurchaseRequisitionDetails)
                    {
                        IDataReader readerdetail;
                        oItem.PRID = oPurchaseRequisition.PRID;
                        oItem.Note = oItem.Note == null ? "" : oItem.Note;
                        if (oItem.PRDetailID <= 0)
                        {
                            readerdetail = PurchaseRequisitionDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = PurchaseRequisitionDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sPurchaseRequisitionDetailIDs = sPurchaseRequisitionDetailIDs + oReaderDetail.GetString("PRDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sPurchaseRequisitionDetailIDs.Length > 0)
                    {
                        sPurchaseRequisitionDetailIDs = sPurchaseRequisitionDetailIDs.Remove(sPurchaseRequisitionDetailIDs.Length - 1, 1);
                    }
                    PurchaseRequisitionDetail oPurchaseRequisitionDetail = new PurchaseRequisitionDetail();
                    oPurchaseRequisitionDetail.PRID = oPurchaseRequisition.PRID;
                    PurchaseRequisitionDetailDA.Delete(tc, oPurchaseRequisitionDetail, EnumDBOperation.Delete, nUserID, sPurchaseRequisitionDetailIDs);

                }

                IDataReader readerdetails = null;
                readerdetails = PurchaseRequisitionDetailDA.Gets(oPurchaseRequisition.PRID, tc);
                PurchaseRequisitionDetailService obj = new PurchaseRequisitionDetailService();
                oPurchaseRequisition.PurchaseRequisitionDetails = obj.CreateObjects(readerdetails);
                readerdetails.Close();

                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oPurchaseRequisition = new PurchaseRequisition();
                oPurchaseRequisition.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oPurchaseRequisition;
        }
        
        public PurchaseRequisition UndoRequestRevise(PurchaseRequisition oPurchaseRequisition, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                oPurchaseRequisition.Status = (int)EnumPurchaseRequisitionStatus.Approve;
                reader = PurchaseRequisitionDA.UndoRequestRevise(tc, oPurchaseRequisition, nUserID);

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPurchaseRequisition = new PurchaseRequisition();
                    oPurchaseRequisition = CreateObject(oReader);
                }
                reader.Close();



                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oPurchaseRequisition = new PurchaseRequisition();
                oPurchaseRequisition.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oPurchaseRequisition;
        }

        public PurchaseRequisition RequestRequisitionRevise(PurchaseRequisition oPurchaseRequisition, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<PurchaseRequisitionDetail> oPurchaseRequisitionDetails = new List<PurchaseRequisitionDetail>();
                PurchaseRequisitionDetail oPurchaseRequisitionDetail = new PurchaseRequisitionDetail();


                oPurchaseRequisitionDetails = oPurchaseRequisition.PurchaseRequisitionDetails;

                string sPurchaseRequisitionDetailIDs = "";

                #region PurchaseRequisition  part

                if (oPurchaseRequisition.PRID > 0)
                {
                    IDataReader reader;
                    oPurchaseRequisition.Status = (int)EnumPurchaseRequisitionStatus.RequestRevise;
                    reader = PurchaseRequisitionDA.RequestRequisitionRevise(tc, oPurchaseRequisition, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oPurchaseRequisition = new PurchaseRequisition();
                        oPurchaseRequisition = CreateObject(oReader);
                    }
                    reader.Close();

                #endregion

                    //#region PurchaseRequisition Detail Part
                    //if (oPurchaseRequisitionDetails != null)
                    //{
                    //    foreach (PurchaseRequisitionDetail oItem in oPurchaseRequisitionDetails)
                    //    {
                    //        IDataReader readerdetail;
                    //        oItem.PRID = oPurchaseRequisition.PRID;
                    //        if (oItem.PRDetailID <= 0)
                    //        {
                    //            readerdetail = PurchaseRequisitionDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                    //        }
                    //        else
                    //        {
                    //            readerdetail = PurchaseRequisitionDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                    //        }
                    //        NullHandler oReaderDetail = new NullHandler(readerdetail);
                    //        if (readerdetail.Read())
                    //        {
                    //            sPurchaseRequisitionDetailIDs = sPurchaseRequisitionDetailIDs + oReaderDetail.GetString("PRDetailID") + ",";
                    //        }
                    //        readerdetail.Close();
                    //    }
                    //    if (sPurchaseRequisitionDetailIDs.Length > 0)
                    //    {
                    //        sPurchaseRequisitionDetailIDs = sPurchaseRequisitionDetailIDs.Remove(sPurchaseRequisitionDetailIDs.Length - 1, 1);
                    //    }
                    //    oPurchaseRequisitionDetail = new PurchaseRequisitionDetail();
                    //    oPurchaseRequisitionDetail.PRID = oPurchaseRequisition.PRID;
                    //    PurchaseRequisitionDetailDA.Delete(tc, oPurchaseRequisitionDetail, EnumDBOperation.Delete, nUserID, sPurchaseRequisitionDetailIDs);

                    //}

                    //#endregion



                    #region PurchaseRequisition Get
                    reader = PurchaseRequisitionDA.Get(tc, oPurchaseRequisition.PRID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oPurchaseRequisition = CreateObject(oReader);
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
                oPurchaseRequisition.ErrorMessage = Message;

                #endregion
            }
            return oPurchaseRequisition;
        }
        #endregion

        public PurchaseRequisition Approved(PurchaseRequisition oPurchaseRequisition, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                oPurchaseRequisition.Status = (int)EnumPurchaseRequisitionStatus.Approve;
                // AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "PurchaseRequisition", EnumRoleOperationType.Edit);
                reader = PurchaseRequisitionDA.InsertUpdate(tc, oPurchaseRequisition, EnumDBOperation.Approval, nUserID);

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPurchaseRequisition = new PurchaseRequisition();
                    oPurchaseRequisition = CreateObject(oReader);
                }
                reader.Close();
               


                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oPurchaseRequisition = new PurchaseRequisition();
                oPurchaseRequisition.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oPurchaseRequisition;
        }

        public PurchaseRequisition Finish(PurchaseRequisition oPurchaseRequisition, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;


                oPurchaseRequisition.Status = (int)EnumPurchaseRequisitionStatus.Finish;
                // AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "PurchaseRequisition", EnumRoleOperationType.Edit);
                reader = PurchaseRequisitionDA.Finish(tc, oPurchaseRequisition, nUserID);

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPurchaseRequisition = new PurchaseRequisition();
                    oPurchaseRequisition = CreateObject(oReader);
                }
                reader.Close();



                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oPurchaseRequisition = new PurchaseRequisition();
                oPurchaseRequisition.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oPurchaseRequisition;
        }

        public PurchaseRequisition Cancel(PurchaseRequisition oPurchaseRequisition, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;


                oPurchaseRequisition.Status = (int)EnumPurchaseRequisitionStatus.Cancel;
                // AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "PurchaseRequisition", EnumRoleOperationType.Edit);
                reader = PurchaseRequisitionDA.Cancel(tc, oPurchaseRequisition, nUserID);

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPurchaseRequisition = new PurchaseRequisition();
                    oPurchaseRequisition = CreateObject(oReader);
                }
                reader.Close();



                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oPurchaseRequisition = new PurchaseRequisition();
                oPurchaseRequisition.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oPurchaseRequisition;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                PurchaseRequisition oPurchaseRequisition = new PurchaseRequisition();
                oPurchaseRequisition.PRID = id;
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "PurchaseRequisition", EnumRoleOperationType.Delete);
                //DBTableReferenceDA.HasReference(tc, "PurchaseRequisition", id);
                PurchaseRequisitionDA.Delete(tc, oPurchaseRequisition, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return "deleted";
        }
        

        public PurchaseRequisition Get(int id, Int64 nUserId)
        {
            PurchaseRequisition oPurchaseRequisition = new PurchaseRequisition();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PurchaseRequisitionDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPurchaseRequisition = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get PurchaseRequisition", e);
                #endregion
            }

            return oPurchaseRequisition;
        }


        public List<PurchaseRequisition> GetsBy(string nStatus, Int64 nUserID)
        {
            List<PurchaseRequisition> oPurchaseRequisition = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PurchaseRequisitionDA.GetsBy(tc, nStatus);
                oPurchaseRequisition = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PurchaseRequisition", e);
                #endregion
            }

            return oPurchaseRequisition;
        }

        //GetsByBU
        public List<PurchaseRequisition> GetsByBU(int nbuid, Int64 nUserID)
        {
            List<PurchaseRequisition> oPurchaseRequisition = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PurchaseRequisitionDA.GetsByBU(tc, nbuid);
                oPurchaseRequisition = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PurchaseRequisition", e);
                #endregion
            }

            return oPurchaseRequisition;
        }
        public List<PurchaseRequisition> Gets(string sSQL, Int64 nUserID)
        {
            List<PurchaseRequisition> oPurchaseRequisitions = new List<PurchaseRequisition>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PurchaseRequisitionDA.Gets(tc, sSQL);
                oPurchaseRequisitions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PurchaseRequisition", e);
                #endregion
            }

            return oPurchaseRequisitions;
        }

        #endregion
    }   
    
    
}
