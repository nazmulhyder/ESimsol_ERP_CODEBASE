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
    public class FabricRequisitionService : MarshalByRefObject, IFabricRequisitionService
    {
        #region Private functions and declaration
        private FabricRequisition MapObject(NullHandler oReader)
        {
            FabricRequisition oFabricRequisition = new FabricRequisition();
            oFabricRequisition.FabricRequisitionID = oReader.GetInt32("FabricRequisitionID");
            oFabricRequisition.RequisitionType = oReader.GetInt32("RequisitionType");
            oFabricRequisition.ReqNo = oReader.GetString("ReqNo");
            oFabricRequisition.ReqDate = oReader.GetDateTime("ReqDate");
            oFabricRequisition.BUID = oReader.GetInt32("BUID");
            oFabricRequisition.IssueStoreID = oReader.GetInt32("IssueStoreID");
            oFabricRequisition.ReceiveStoreID = oReader.GetInt32("ReceiveStoreID");
            oFabricRequisition.Note = oReader.GetString("Note");
            oFabricRequisition.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oFabricRequisition.DisburseBy = oReader.GetInt32("DisburseBy");
            oFabricRequisition.ReceivedBy = oReader.GetInt32("ReceivedBy");
            oFabricRequisition.ApproveDate = oReader.GetDateTime("ApproveDate");
            oFabricRequisition.DisburseDate = oReader.GetDateTime("DisburseDate");
            oFabricRequisition.ReceiveDate = oReader.GetDateTime("ReceiveDate");
            oFabricRequisition.IssueStoreName = oReader.GetString("IssueStoreName");
            oFabricRequisition.ReceiveStoreName = oReader.GetString("ReceiveStoreName");
            oFabricRequisition.ApprovedByName = oReader.GetString("ApprovedByName");
            oFabricRequisition.DisburseByName = oReader.GetString("DisburseByName");
            oFabricRequisition.ReceivedByName = oReader.GetString("ReceivedByName");

            return oFabricRequisition;
        }

        private FabricRequisition CreateObject(NullHandler oReader)
        {
            FabricRequisition oFabricRequisition = new FabricRequisition();
            oFabricRequisition = MapObject(oReader);
            return oFabricRequisition;
        }

        private List<FabricRequisition> CreateObjects(IDataReader oReader)
        {
            List<FabricRequisition> oFabricRequisition = new List<FabricRequisition>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricRequisition oItem = CreateObject(oHandler);
                oFabricRequisition.Add(oItem);
            }
            return oFabricRequisition;
        }

        #endregion

        #region Interface implementation
        public FabricRequisition Save(FabricRequisition oFabricRequisition, Int64 nUserID)
        {
            FabricRequisition oUG = new FabricRequisition();
            oUG = oFabricRequisition;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFabricRequisition.FabricRequisitionID <= 0)
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "FabricRequisition", EnumRoleOperationType.Add);
                    reader = FabricRequisitionDA.InsertUpdate(tc, oFabricRequisition, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "FabricRequisition", EnumRoleOperationType.Edit);
                    reader = FabricRequisitionDA.InsertUpdate(tc, oFabricRequisition, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricRequisition = new FabricRequisition();
                    oFabricRequisition = CreateObject(oReader);
                }
                reader.Close();

                #region FabricRequisitionDetail

                if (oFabricRequisition.FabricRequisitionID > 0)
                {
                    string sFabricRequisitionDetailIDs = "";
                    if (oUG.FabricRequisitionDetails.Count > 0)
                    {
                        IDataReader readerdetail;
                        foreach (FabricRequisitionDetail oDRD in oUG.FabricRequisitionDetails)
                        {
                            oDRD.FabricRequisitionID = oFabricRequisition.FabricRequisitionID;
                            if (oDRD.FabricRequisitionDetailID <= 0)
                            {
                                readerdetail = FabricRequisitionDetailDA.InsertUpdate(tc, oDRD, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readerdetail = FabricRequisitionDetailDA.InsertUpdate(tc, oDRD, EnumDBOperation.Update, nUserID, "");

                            }
                            NullHandler oReaderDevRecapdetail = new NullHandler(readerdetail);
                            int nFabricRequisitionDetailID = 0;
                            if (readerdetail.Read())
                            {
                                nFabricRequisitionDetailID = oReaderDevRecapdetail.GetInt32("FabricRequisitionDetailID");
                                sFabricRequisitionDetailIDs = sFabricRequisitionDetailIDs + oReaderDevRecapdetail.GetString("FabricRequisitionDetailID") + ",";
                            }
                            readerdetail.Close();
                        }
                    }
                    if (sFabricRequisitionDetailIDs.Length > 0)
                    {
                        sFabricRequisitionDetailIDs = sFabricRequisitionDetailIDs.Remove(sFabricRequisitionDetailIDs.Length - 1, 1);
                    }
                    FabricRequisitionDetail oFabricRequisitionDetail = new FabricRequisitionDetail();
                    oFabricRequisitionDetail.FabricRequisitionID = oFabricRequisition.FabricRequisitionID;
                    FabricRequisitionDetailDA.Delete(tc, oFabricRequisitionDetail, EnumDBOperation.Delete, nUserID, sFabricRequisitionDetailIDs);
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
                    oFabricRequisition = new FabricRequisition();
                    oFabricRequisition.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oFabricRequisition;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricRequisition oFabricRequisition = new FabricRequisition();
                oFabricRequisition.FabricRequisitionID = id;
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "FabricRequisition", EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "FabricRequisition", id);
                FabricRequisitionDA.Delete(tc, oFabricRequisition, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exceptionif (tc != null)
                tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return "Data delete successfully";
        }

        public FabricRequisition Get(int id, Int64 nUserId)
        {
            FabricRequisition oFabricRequisition = new FabricRequisition();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricRequisitionDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricRequisition = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FabricRequisition", e);
                #endregion
            }
            return oFabricRequisition;
        }

        public List<FabricRequisition> Gets(Int64 nUserID)
        {
            List<FabricRequisition> oFabricRequisitions = new List<FabricRequisition>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricRequisitionDA.Gets(tc);
                oFabricRequisitions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FabricRequisition oFabricRequisition = new FabricRequisition();
                oFabricRequisition.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricRequisitions;
        }

        public List<FabricRequisition> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricRequisition> oFabricRequisitions = new List<FabricRequisition>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricRequisitionDA.Gets(tc, sSQL);
                oFabricRequisitions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricRequisition", e);
                #endregion
            }
            return oFabricRequisitions;
        }

        public FabricRequisition ChangeStatus(FabricRequisition oFabricRequisition, int nUserId)
        {
            oFabricRequisition.ApprovedBy = nUserId;
            oFabricRequisition.ApproveDate = DateTime.Now;
            oFabricRequisition.DisburseBy = nUserId;
            oFabricRequisition.DisburseDate = DateTime.Now;

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricRequisitionDA.ChangeStatus(tc, oFabricRequisition);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricRequisition = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oFabricRequisition = new FabricRequisition();
                oFabricRequisition.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricRequisition;
        }

        public FabricRequisition Receive(FabricRequisition oFabricRequisition, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricRequisitionDA.InsertUpdate(tc, oFabricRequisition, EnumDBOperation.Receive, nUserId);
                //IDataReader reader = FabricRequisitionDA.Receive(tc, oFabricRequisition);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricRequisition = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oFabricRequisition = new FabricRequisition();
                oFabricRequisition.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricRequisition;
        }

        #endregion
    }

}
