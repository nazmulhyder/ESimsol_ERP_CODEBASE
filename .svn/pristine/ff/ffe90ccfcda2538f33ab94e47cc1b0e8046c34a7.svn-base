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


    public class PartsRequisitionService : MarshalByRefObject, IPartsRequisitionService
    {
        #region Private functions and declaration
        private PartsRequisition MapObject(NullHandler oReader)
        {
            PartsRequisition oPartsRequisition = new PartsRequisition();
            oPartsRequisition.PartsRequisitionID = oReader.GetInt32("PartsRequisitionID");
            oPartsRequisition.RequisitionNo = oReader.GetString("RequisitionNo");
            oPartsRequisition.BUID = oReader.GetInt32("BUID");
            oPartsRequisition.ServiceOrderID = oReader.GetInt32("ServiceOrderID");
            oPartsRequisition.VehicleRegID = oReader.GetInt32("VehicleRegID");
            oPartsRequisition.PRType = (EnumPRequisutionType)oReader.GetInt32("PRType");
            oPartsRequisition.PRTypeInt = oReader.GetInt32("PRType");
            oPartsRequisition.RequisitionBy = oReader.GetInt32("RequisitionBy");
            oPartsRequisition.PRStatus = (EnumCRStatus)oReader.GetInt32("PRStatus");
            oPartsRequisition.PRStatusInt = oReader.GetInt32("PRStatus");
            oPartsRequisition.IssueDate = oReader.GetDateTime("IssueDate");
            oPartsRequisition.StoreID = oReader.GetInt32("StoreID");
            oPartsRequisition.CustomerName = oReader.GetString("CustomerName");
            oPartsRequisition.Remarks = oReader.GetString("Remarks");
            oPartsRequisition.Note = oReader.GetString("Note");
            oPartsRequisition.DeliveryBy = oReader.GetInt32("DeliveryBy");
            oPartsRequisition.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oPartsRequisition.StoreCode = oReader.GetString("StoreCode");
            oPartsRequisition.StoreName = oReader.GetString("StoreName");
            oPartsRequisition.ServiceOrderNo = oReader.GetString("ServiceOrderNo");
            oPartsRequisition.ChassisNo = oReader.GetString("ChassisNo");
            oPartsRequisition.EngineNo = oReader.GetString("EngineNo");
            oPartsRequisition.VehicleRegNo = oReader.GetString("VehicleRegNo");
            oPartsRequisition.ModelNo = oReader.GetString("ModelNo");
            oPartsRequisition.RequisitionByName = oReader.GetString("RequisitionByName");
            oPartsRequisition.ApprovedByName = oReader.GetString("ApprovedByName");
            oPartsRequisition.DeliveryByName = oReader.GetString("DeliveryByName");
            oPartsRequisition.Amount = oReader.GetDouble("Amount");
            oPartsRequisition.PartsRequisitionLogID = oReader.GetInt32("PartsRequisitionLogID");
            
            return oPartsRequisition;
        }

        private PartsRequisition CreateObject(NullHandler oReader)
        {
            PartsRequisition oPartsRequisition = new PartsRequisition();
            oPartsRequisition = MapObject(oReader);
            return oPartsRequisition;
        }

        private List<PartsRequisition> CreateObjects(IDataReader oReader)
        {
            List<PartsRequisition> oPartsRequisition = new List<PartsRequisition>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PartsRequisition oItem = CreateObject(oHandler);
                oPartsRequisition.Add(oItem);
            }
            return oPartsRequisition;
        }

        #endregion

        #region Interface implementation

        public PartsRequisition Save(PartsRequisition oPartsRequisition, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<PartsRequisitionDetail> oPartsRequisitionDetails = new List<PartsRequisitionDetail>();
            string sPartsRequisitionDetailIDs = "";
            oPartsRequisitionDetails = oPartsRequisition.PartsRequisitionDetails;

            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oPartsRequisition.PartsRequisitionID <= 0)
                {

                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.PartsRequisition, EnumRoleOperationType.Add);
                    reader = PartsRequisitionDA.InsertUpdate(tc, oPartsRequisition, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.PartsRequisition, EnumRoleOperationType.Edit);
                    reader = PartsRequisitionDA.InsertUpdate(tc, oPartsRequisition, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPartsRequisition = new PartsRequisition();
                    oPartsRequisition = CreateObject(oReader);
                }
                reader.Close();

                #region CR Detail Part
                if (oPartsRequisitionDetails != null)
                {
                    foreach (PartsRequisitionDetail oItem in oPartsRequisitionDetails)
                    {
                        IDataReader readerdetail;
                        oItem.PartsRequisitionID = oPartsRequisition.PartsRequisitionID;
                        if (oItem.PartsRequisitionDetailID <= 0)
                        {
                            readerdetail = PartsRequisitionDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = PartsRequisitionDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sPartsRequisitionDetailIDs = sPartsRequisitionDetailIDs + oReaderDetail.GetString("PartsRequisitionDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sPartsRequisitionDetailIDs.Length > 0)
                    {
                        sPartsRequisitionDetailIDs = sPartsRequisitionDetailIDs.Remove(sPartsRequisitionDetailIDs.Length - 1, 1);
                    }
                    PartsRequisitionDetail oPartsRequisitionDetail = new PartsRequisitionDetail();
                    oPartsRequisitionDetail.PartsRequisitionID = oPartsRequisition.PartsRequisitionID;
                    PartsRequisitionDetailDA.Delete(tc, oPartsRequisitionDetail, EnumDBOperation.Delete, nUserID, sPartsRequisitionDetailIDs);

                }

                #endregion

                #region Again Get CR
                reader = PartsRequisitionDA.Get(tc, oPartsRequisition.PartsRequisitionID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPartsRequisition = CreateObject(oReader);
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

                oPartsRequisition = new PartsRequisition();
                oPartsRequisition.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oPartsRequisition;
        }

        public PartsRequisition Delivery(PartsRequisition oPartsRequisition, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<PartsRequisitionDetail> oPartsRequisitionDetails = new List<PartsRequisitionDetail>();
            oPartsRequisitionDetails = oPartsRequisition.PartsRequisitionDetails;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.PartsRequisition, EnumRoleOperationType.Edit);
                reader = PartsRequisitionDA.InsertUpdate(tc, oPartsRequisition, EnumDBOperation.Update, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPartsRequisition = new PartsRequisition();
                    oPartsRequisition = CreateObject(oReader);
                }
                reader.Close();

                #region CR Detail Part
                if (oPartsRequisitionDetails != null)
                {
                    foreach (PartsRequisitionDetail oItem in oPartsRequisitionDetails)
                    {
                        IDataReader readerdetail;
                        oItem.PartsRequisitionID = oPartsRequisition.PartsRequisitionID;
                        readerdetail = PartsRequisitionDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {

                        }
                        readerdetail.Close();
                    }
                }

                #endregion

                #region Delivery Effect
                oPartsRequisition.PRActionType = EnumCRActionType.StockOut;
                oPartsRequisition.PRStatus = EnumCRStatus.Approve;
                reader = PartsRequisitionDA.ChangeStatus(tc, oPartsRequisition, EnumDBOperation.Insert, nUserID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPartsRequisition = CreateObject(oReader);
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

                oPartsRequisition = new PartsRequisition();
                oPartsRequisition.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oPartsRequisition;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                PartsRequisition oPartsRequisition = new PartsRequisition();
                oPartsRequisition.PartsRequisitionID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.PartsRequisition, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "PartsRequisition", id);
                PartsRequisitionDA.Delete(tc, oPartsRequisition, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete PartsRequisition. Because of " + e.Message, e);
                #endregion
            }
            return "Delete successfully";
        }

        public PartsRequisition Get(int id, Int64 nUserId)
        {
            PartsRequisition oPartsRequisition = new PartsRequisition();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PartsRequisitionDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPartsRequisition = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get PartsRequisition", e);
                #endregion
            }

            return oPartsRequisition;
        }

        public PartsRequisition GetLog(int id, Int64 nUserId)
        {
            PartsRequisition oPartsRequisition = new PartsRequisition();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PartsRequisitionDA.GetLog(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPartsRequisition = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get PartsRequisition", e);
                #endregion
            }

            return oPartsRequisition;
        }

        public PartsRequisition ChangeStatus(PartsRequisition oPartsRequisition, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oPartsRequisition.PRActionType == EnumCRActionType.Approve)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.PartsRequisition, EnumRoleOperationType.Approved);
                }

                reader = PartsRequisitionDA.ChangeStatus(tc, oPartsRequisition, EnumDBOperation.Insert, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPartsRequisition = new PartsRequisition();
                    oPartsRequisition = CreateObject(oReader);
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
                oPartsRequisition.ErrorMessage = Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save PartsRequisitionDetail. Because of " + e.Message, e);
                #endregion
            }
            return oPartsRequisition;
        }
        public List<PartsRequisition> Gets(Int64 nUserID)
        {
            List<PartsRequisition> oPartsRequisition = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PartsRequisitionDA.Gets(tc);
                oPartsRequisition = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PartsRequisition", e);
                #endregion
            }

            return oPartsRequisition;
        }

        public List<PartsRequisition> Gets(string sSQL, Int64 nUserID)
        {
            List<PartsRequisition> oPartsRequisition = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PartsRequisitionDA.Gets(tc, sSQL);
                oPartsRequisition = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PartsRequisition", e);
                #endregion
            }

            return oPartsRequisition;
        }
        public PartsRequisition AcceptPartsRequisitionRevise(PartsRequisition oPartsRequisition, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<PartsRequisitionDetail> oPartsRequisitionDetails = new List<PartsRequisitionDetail>();
                PartsRequisitionDetail oPartsRequisitionDetail = new PartsRequisitionDetail();
                oPartsRequisitionDetails = oPartsRequisition.PartsRequisitionDetails;
                string sPartsRequisitionDetailIDs = "";

                if (oPartsRequisition.PartsRequisitionID > 0)
                {
                    #region PartsRequisition part
                    IDataReader reader;
                    reader = PartsRequisitionDA.AcceptPartsRequisitionRevise(tc, oPartsRequisition, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oPartsRequisition = new PartsRequisition();
                        oPartsRequisition = CreateObject(oReader);
                    }
                    reader.Close();

                    #endregion

                    #region PartsRequisition Detail Detail Part
                    if (oPartsRequisitionDetails != null)
                    {
                        foreach (PartsRequisitionDetail oItem in oPartsRequisitionDetails)
                        {
                            IDataReader readerdetail;
                            oItem.PartsRequisitionID = oPartsRequisition.PartsRequisitionID;
                            if (oItem.PartsRequisitionDetailID <= 0)
                            {
                                readerdetail = PartsRequisitionDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readerdetail = PartsRequisitionDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                            }
                            NullHandler oReaderDetail = new NullHandler(readerdetail);
                            if (readerdetail.Read())
                            {
                                sPartsRequisitionDetailIDs = sPartsRequisitionDetailIDs + oReaderDetail.GetString("PartsRequisitionDetailID") + ",";
                            }
                            readerdetail.Close();
                        }
                        if (sPartsRequisitionDetailIDs.Length > 0)
                        {
                            sPartsRequisitionDetailIDs = sPartsRequisitionDetailIDs.Remove(sPartsRequisitionDetailIDs.Length - 1, 1);
                        }
                        oPartsRequisitionDetail = new PartsRequisitionDetail();
                        oPartsRequisitionDetail.PartsRequisitionID = oPartsRequisition.PartsRequisitionID;
                        PartsRequisitionDetailDA.Delete(tc, oPartsRequisitionDetail, EnumDBOperation.Delete, nUserID, sPartsRequisitionDetailIDs);
                    }

                    #endregion

                    #region PartsRequisition Get
                    reader = PartsRequisitionDA.Get(tc, oPartsRequisition.PartsRequisitionID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oPartsRequisition = CreateObject(oReader);
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
                oPartsRequisition.ErrorMessage = Message;

                #endregion
            }
            return oPartsRequisition;
        }
        #endregion
    }
}
