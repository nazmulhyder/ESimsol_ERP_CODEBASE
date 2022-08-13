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
    public class FabricClaimService : MarshalByRefObject, IFabricClaimService
    {
        #region Private functions and declaration

        private FabricClaim MapObject(NullHandler oReader)
        {
            FabricClaim oFabricClaim = new FabricClaim();
            oFabricClaim.FabricClaimID = oReader.GetInt32("FabricClaimID");
            oFabricClaim.FSCID = oReader.GetInt32("FSCID");
            oFabricClaim.ParentFSCID = oReader.GetInt32("ParentFSCID");
            oFabricClaim.Subject = oReader.GetString("Subject");
            oFabricClaim.Remarks = oReader.GetString("Remarks");
            oFabricClaim.PrepareBy = oReader.GetInt32("PrepareBy");
            oFabricClaim.CheckedBy = oReader.GetInt32("CheckedBy");
            oFabricClaim.CheckedDate = oReader.GetDateTime("CheckedDate");
            oFabricClaim.Note_Checked = oReader.GetString("Note_Checked");
            oFabricClaim.Note_Approve = oReader.GetString("Note_Approve");
            oFabricClaim.ParentSCNo = oReader.GetString("ParentSCNo");
            oFabricClaim.FabricReturnChallanID = oReader.GetInt32("FabricReturnChallanID");
            oFabricClaim.SCNoFull = oReader.GetString("SCNoFull");
            oFabricClaim.OrderName = oReader.GetString("OrderName");
            oFabricClaim.ContractorName = oReader.GetString("ContractorName");
            oFabricClaim.BuyerName = oReader.GetString("BuyerName");
            oFabricClaim.BuyerAddress = oReader.GetString("BuyerAddress");
            oFabricClaim.ContractorAddress = oReader.GetString("ContractorAddress");
            oFabricClaim.ContractorPhone = oReader.GetString("ContractorPhone");
            oFabricClaim.ContractorFax = oReader.GetString("ContractorFax");
            oFabricClaim.ContractorEmail = oReader.GetString("ContractorEmail");
            oFabricClaim.MKTPName = oReader.GetString("MKTPName");
            oFabricClaim.MKTPNickName = oReader.GetString("MKTPNickName");
            oFabricClaim.Currency = oReader.GetString("Currency");
            oFabricClaim.CPersonName = oReader.GetString("CPersonName");
            oFabricClaim.LCTermsName = oReader.GetString("LCTermsName");
            oFabricClaim.ApproveByName = oReader.GetString("ApproveByName");
            oFabricClaim.PreapeByName = oReader.GetString("PreapeByName");
            oFabricClaim.LightSourceName = oReader.GetString("LightSourceName");
            oFabricClaim.LightSourceNameTwo = oReader.GetString("LightSourceNameTwo");
            oFabricClaim.Amount = oReader.GetDouble("Amount");
            oFabricClaim.Qty = oReader.GetDouble("Qty");
            oFabricClaim.AttCount = oReader.GetInt32("AttCount");
            oFabricClaim.PINo = oReader.GetString("PINo");
            oFabricClaim.SCDate = oReader.GetDateTime("SCDate");

            return oFabricClaim;
        }

        private FabricClaim CreateObject(NullHandler oReader)
        {
            FabricClaim oFabricClaim = new FabricClaim();
            oFabricClaim = MapObject(oReader);
            return oFabricClaim;
        }

        private List<FabricClaim> CreateObjects(IDataReader oReader)
        {
            List<FabricClaim> oFabricClaim = new List<FabricClaim>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricClaim oItem = CreateObject(oHandler);
                oFabricClaim.Add(oItem);
            }
            return oFabricClaim;
        }

        #endregion

        #region Interface implementation
        public FabricClaim Save(FabricClaim oFabricClaim, Int64 nUserID)
        {
            FabricClaimDetail oFabricClaimDetail = new FabricClaimDetail();
            FabricClaim oUG = new FabricClaim();
            oUG = oFabricClaim;

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                #region FabricClaim
                IDataReader reader;
                if (oFabricClaim.FabricClaimID <= 0)
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.FabricClaim, EnumRoleOperationType.Add);
                    reader = FabricClaimDA.InsertUpdate(tc, oFabricClaim, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.FabricClaim, EnumRoleOperationType.Edit);
                    reader = FabricClaimDA.InsertUpdate(tc, oFabricClaim, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricClaim = new FabricClaim();
                    oFabricClaim = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region FabricClaimDetail

                if (oFabricClaim.FabricClaimID > 0)
                {
                    string sFabricClaimDetailIDs = "";
                    if (oUG.FabricClaimDetails.Count > 0)
                    {
                        IDataReader readerdetail;
                        foreach (FabricClaimDetail oDRD in oUG.FabricClaimDetails)
                        {
                            oDRD.FabricClaimID = oFabricClaim.FabricClaimID;
                            if (oDRD.FabricClaimDetailID <= 0)
                            {
                                readerdetail = FabricClaimDetailDA.InsertUpdate(tc, oDRD, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readerdetail = FabricClaimDetailDA.InsertUpdate(tc, oDRD, EnumDBOperation.Update, nUserID, "");

                            }
                            NullHandler oReaderDevRecapdetail = new NullHandler(readerdetail);
                            int nFabricClaimDetailID = 0;
                            if (readerdetail.Read())
                            {
                                nFabricClaimDetailID = oReaderDevRecapdetail.GetInt32("FabricClaimDetailID");
                                sFabricClaimDetailIDs = sFabricClaimDetailIDs + oReaderDevRecapdetail.GetString("FabricClaimDetailID") + ",";
                            }
                            readerdetail.Close();
                        }
                    }
                    if (sFabricClaimDetailIDs.Length > 0)
                    {
                        sFabricClaimDetailIDs = sFabricClaimDetailIDs.Remove(sFabricClaimDetailIDs.Length - 1, 1);
                    }
                    oFabricClaimDetail = new FabricClaimDetail();
                    oFabricClaimDetail.FabricClaimID = oFabricClaim.FabricClaimID;
                    FabricClaimDetailDA.Delete(tc, oFabricClaimDetail, EnumDBOperation.Delete, nUserID, sFabricClaimDetailIDs);
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
                    oFabricClaim = new FabricClaim();
                    oFabricClaim.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oFabricClaim;
        }

        public string Delete(FabricClaim oFC, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "FabricClaim", EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "FabricClaim", oFC.FabricClaimID);
                FabricClaimDA.Delete(tc, oFC, EnumDBOperation.Delete, nUserId);
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

        public FabricClaim Get(int id, Int64 nUserId)
        {
            FabricClaim oFabricClaim = new FabricClaim();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricClaimDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricClaim = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FabricClaim", e);
                #endregion
            }
            return oFabricClaim;
        }

        public List<FabricClaim> Gets(Int64 nUserID)
        {
            List<FabricClaim> oFabricClaims = new List<FabricClaim>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricClaimDA.Gets(tc);
                oFabricClaims = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FabricClaim oFabricClaim = new FabricClaim();
                oFabricClaim.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricClaims;
        }

        public List<FabricClaim> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricClaim> oFabricClaims = new List<FabricClaim>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricClaimDA.Gets(tc, sSQL);
                oFabricClaims = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricClaim", e);
                #endregion
            }
            return oFabricClaims;
        }

        #endregion
    }

}
