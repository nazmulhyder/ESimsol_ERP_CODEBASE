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
    public class RMConsumptionService : MarshalByRefObject, IRMConsumptionService
    {
        #region Private functions and declaration

        private RMConsumption MapObject(NullHandler oReader)
        {
            RMConsumption oRMConsumption = new RMConsumption();
            oRMConsumption.RMConsumptionID = oReader.GetInt32("RMConsumptionID");
            oRMConsumption.ConsumptionNo = oReader.GetString("ConsumptionNo");
            oRMConsumption.ConsumptionDate = oReader.GetDateTime("ConsumptionDate");
            oRMConsumption.BUID = oReader.GetInt32("BUID");
            oRMConsumption.Remarks = oReader.GetString("Remarks");
            oRMConsumption.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oRMConsumption.ApprovedByName = oReader.GetString("ApprovedByName");
            oRMConsumption.BUName = oReader.GetString("BUName");
            oRMConsumption.BUShortName = oReader.GetString("BUShortName");
            oRMConsumption.ConsumptionAmount = oReader.GetDouble("ConsumptionAmount");
            return oRMConsumption;
        }

        private RMConsumption CreateObject(NullHandler oReader)
        {
            RMConsumption oRMConsumption = new RMConsumption();
            oRMConsumption = MapObject(oReader);
            return oRMConsumption;
        }

        private List<RMConsumption> CreateObjects(IDataReader oReader)
        {
            List<RMConsumption> oRMConsumption = new List<RMConsumption>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RMConsumption oItem = CreateObject(oHandler);
                oRMConsumption.Add(oItem);
            }
            return oRMConsumption;
        }

        #endregion

        #region Interface implementation
        public RMConsumption Save(RMConsumption oRMConsumption, Int64 nUserID)
        {
            TransactionContext tc = null;
            string sRMConsumptionDetailIDs = "";
            List<RMConsumptionDetail> oRMConsumptionDetails = new List<RMConsumptionDetail>();
            oRMConsumptionDetails = oRMConsumption.RMConsumptionDetails;
            try
            {
                tc = TransactionContext.Begin(true);
                #region RMConsumption
                IDataReader reader;
                if (oRMConsumption.RMConsumptionID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.RMConsumption, EnumRoleOperationType.Add);
                    reader = RMConsumptionDA.InsertUpdate(tc, oRMConsumption, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.RMConsumption, EnumRoleOperationType.Edit);
                    VoucherDA.CheckVoucherReference(tc, "RMConsumption", "RMConsumptionID", oRMConsumption.RMConsumptionID);
                    reader = RMConsumptionDA.InsertUpdate(tc, oRMConsumption, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRMConsumption = new RMConsumption();
                    oRMConsumption = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region RMConsumption Detail Part
                if (oRMConsumptionDetails != null)
                {
                    foreach (RMConsumptionDetail oItem in oRMConsumptionDetails)
                    {
                        IDataReader readerdetail;
                        oItem.RMConsumptionID = oRMConsumption.RMConsumptionID;
                        if (oItem.RMConsumptionDetailID <= 0)
                        {
                            readerdetail = RMConsumptionDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = RMConsumptionDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sRMConsumptionDetailIDs = sRMConsumptionDetailIDs + oReaderDetail.GetString("RMConsumptionDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sRMConsumptionDetailIDs.Length > 0)
                    {
                        sRMConsumptionDetailIDs = sRMConsumptionDetailIDs.Remove(sRMConsumptionDetailIDs.Length - 1, 1);
                    }
                }
                RMConsumptionDetail oRMConsumptionDetail = new RMConsumptionDetail();
                oRMConsumptionDetail.RMConsumptionID = oRMConsumption.RMConsumptionID;
                RMConsumptionDetailDA.Delete(tc, oRMConsumptionDetail, EnumDBOperation.Delete, nUserID, sRMConsumptionDetailIDs);
                #endregion

                #region Get RMConsumption
                reader = RMConsumptionDA.Get(tc, oRMConsumption.RMConsumptionID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRMConsumption = new RMConsumption();
                    oRMConsumption = CreateObject(oReader);
                }
                reader.Close();
                #endregion


                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oRMConsumption = new RMConsumption();
                    oRMConsumption.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oRMConsumption;
        }
        public RMConsumption GetSuggestMaterialConsumptionDate(string sSQl, Int64 nUserID)
        {
            TransactionContext tc = null;
            RMConsumption oRMConsumption = new RMConsumption();
            try
            {
                tc = TransactionContext.Begin(true);                
                #region Get RMConsumption
                IDataReader reader;
                reader = RMConsumptionDA.Gets(tc, sSQl);
                NullHandler oReader = new NullHandler(reader);               
                if (reader.Read())
                {
                    oRMConsumption = new RMConsumption();
                    oRMConsumption.ConsumptionDate = oReader.GetDateTime("SuggestDate");
                }
                reader.Close();
                #endregion


                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oRMConsumption = new RMConsumption();
                    oRMConsumption.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oRMConsumption;
        }

        public string YetToMaterialConsumptionDate(string sSQl, Int64 nUserID)
        {
            string sConsumptionDate = "";
            TransactionContext tc = null;
            RMConsumption oRMConsumption = new RMConsumption();
            try
            {
                tc = TransactionContext.Begin(true);
                #region Get RMConsumption
                IDataReader reader;
                reader = RMConsumptionDA.Gets(tc, sSQl);
                NullHandler oHandler = new NullHandler(reader);
                while (reader.Read())
                {
                    sConsumptionDate = sConsumptionDate + oHandler.GetString("SuggestDate") + "\n";
                }
                reader.Close();
                #endregion


                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    sConsumptionDate = e.Message.Split('!')[0];
                }
                #endregion
            }
            return sConsumptionDate;
        }
        public RMConsumption Approved(RMConsumption oRMConsumption, Int64 nUserID)
        {
            TransactionContext tc = null;            
            try
            {
                tc = TransactionContext.Begin(true);                
                IDataReader reader;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.RMConsumption, EnumRoleOperationType.Approved);
                RMConsumptionDA.Approved(tc, oRMConsumption, nUserID);
                reader = RMConsumptionDA.Get(tc, oRMConsumption.RMConsumptionID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRMConsumption = new RMConsumption();
                    oRMConsumption = CreateObject(oReader);
                }
                reader.Close();              
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oRMConsumption = new RMConsumption();
                    oRMConsumption.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oRMConsumption;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                RMConsumption oRMConsumption = new RMConsumption();
                oRMConsumption.RMConsumptionID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.RMConsumption, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "RMConsumption", id);
                VoucherDA.CheckVoucherReference(tc, "RMConsumption", "RMConsumptionID", oRMConsumption.RMConsumptionID);
                RMConsumptionDA.Delete(tc, oRMConsumption, EnumDBOperation.Delete, nUserId);
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

        public RMConsumption Get(int id, Int64 nUserId)
        {
            RMConsumption oRMConsumption = new RMConsumption();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = RMConsumptionDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRMConsumption = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get RMConsumption", e);
                #endregion
            }
            return oRMConsumption;
        }

        public List<RMConsumption> Gets(Int64 nUserID)
        {
            List<RMConsumption> oRMConsumptions = new List<RMConsumption>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = RMConsumptionDA.Gets(tc);
                oRMConsumptions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                RMConsumption oRMConsumption = new RMConsumption();
                oRMConsumption.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oRMConsumptions;
        }

        public List<RMConsumption> Gets(string sSQL, Int64 nUserID)
        {
            List<RMConsumption> oRMConsumptions = new List<RMConsumption>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = RMConsumptionDA.Gets(tc, sSQL);
                oRMConsumptions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RMConsumption", e);
                #endregion
            }
            return oRMConsumptions;
        }

        #endregion
    }

}