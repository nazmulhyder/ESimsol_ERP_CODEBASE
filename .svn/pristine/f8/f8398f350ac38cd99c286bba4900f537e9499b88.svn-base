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
    public class ACCostCenterService : MarshalByRefObject, IACCostCenterService
    {
        #region Private functions and declaration
        private ACCostCenter MapObject(NullHandler oReader)
        {
            ACCostCenter oACCostCenter = new ACCostCenter();
            oACCostCenter.ACCostCenterID = oReader.GetInt32("ACCostCenterID");
            oACCostCenter.Code = oReader.GetString("Code");
            oACCostCenter.Name = oReader.GetString("Name");
            oACCostCenter.Description = oReader.GetString("Description");
            oACCostCenter.ParentID = oReader.GetInt32("ParentID");
            oACCostCenter.ReferenceType = (EnumReferenceType)oReader.GetInt32("ReferenceType");
            oACCostCenter.ReferenceTypeInt = oReader.GetInt32("ReferenceType");
            oACCostCenter.ReferenceObjectID = oReader.GetInt32("ReferenceObjectID");
            oACCostCenter.ActivationDate = oReader.GetDateTime("ActivationDate");
            oACCostCenter.ExpireDate = oReader.GetDateTime("ExpireDate");
            oACCostCenter.IsActive = oReader.GetBoolean("IsActive");            
            oACCostCenter.CategoryName = oReader.GetString("CategoryName");
            oACCostCenter.CategoryCode = oReader.GetString("CategoryCode");
            oACCostCenter.IsBillRefApply = oReader.GetBoolean("IsBillRefApply");
            oACCostCenter.IsOrderRefApply = oReader.GetBoolean("IsOrderRefApply");
            oACCostCenter.NameCode = oReader.GetString("NameCode");
            oACCostCenter.BUName = oReader.GetString("BUName");
            oACCostCenter.DueAmount = oReader.GetDouble("DueAmount");
            oACCostCenter.OverDueDays = oReader.GetInt32("OverDueDays");
            oACCostCenter.CurrentBalance = oReader.GetString("CurrentBalance");
            return oACCostCenter;
        }

        private ACCostCenter CreateObject(NullHandler oReader)
        {
            ACCostCenter oACCostCenter = new ACCostCenter();
            oACCostCenter = MapObject(oReader);
            return oACCostCenter;
        }

        private List<ACCostCenter> CreateObjects(IDataReader oReader)
        {
            List<ACCostCenter> oACCostCenter = new List<ACCostCenter>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ACCostCenter oItem = CreateObject(oHandler);
                oACCostCenter.Add(oItem);
            }
            return oACCostCenter;
        }
        #endregion

        public ACCostCenter Save(ACCostCenter oACCostCenter, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                BUWiseSubLedger oBUWiseSubLedger = new BUWiseSubLedger();
                List<BUWiseSubLedger> oBUWiseSubLedgers = new List<BUWiseSubLedger>();
                List<SubledgerRefConfig> oSubledgerRefConfigs = new List<SubledgerRefConfig>();
                oBUWiseSubLedgers = oACCostCenter.BUWiseSubLedgers;
                oSubledgerRefConfigs = oACCostCenter.SubledgerRefConfigs;

                tc = TransactionContext.Begin(true);

                #region Subledger
                IDataReader reader;
                if (oACCostCenter.ACCostCenterID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.ACCostCenter, EnumRoleOperationType.Add);
                    reader = ACCostCenterDA.InsertUpdate(tc, oACCostCenter, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.ACCostCenter, EnumRoleOperationType.Edit);
                    reader = ACCostCenterDA.InsertUpdate(tc, oACCostCenter, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oACCostCenter = new ACCostCenter();
                    oACCostCenter = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region Subledger Wise Business Unit
                if (oBUWiseSubLedgers != null)
                {
                    if (oBUWiseSubLedgers.Count > 0)
                    {
                        string sBusinessUnitIDs = "";
                        foreach (BUWiseSubLedger oItem in oBUWiseSubLedgers)
                        {

                            sBusinessUnitIDs = sBusinessUnitIDs + oItem.BusinessUnitID.ToString() + ",";
                        }
                        if (sBusinessUnitIDs.Length > 0)
                        {
                            sBusinessUnitIDs = sBusinessUnitIDs.Remove(sBusinessUnitIDs.Length - 1, 1);
                        }
                        oBUWiseSubLedger.SubLedgerID = oACCostCenter.ACCostCenterID;
                        BUWiseSubLedgerDA.IUDFromCC(tc, oBUWiseSubLedger, sBusinessUnitIDs, nUserId);
                    }
                }          
                #endregion

                #region SubledgerRefConfig
                if (oSubledgerRefConfigs != null)
                {
                    if (oSubledgerRefConfigs.Count > 0)
                    {
                        string sAccountHeadIDs = "";
                        foreach (SubledgerRefConfig oItem in oSubledgerRefConfigs)
                        {
                            if (oItem.IsChecked)
                            {
                                SubledgerRefConfigDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserId);
                            }
                            sAccountHeadIDs = sAccountHeadIDs + oItem.AccountHeadID.ToString() + ",";
                        }
                        if (sAccountHeadIDs.Length > 0)
                        {
                            sAccountHeadIDs = sAccountHeadIDs.Remove(sAccountHeadIDs.Length - 1, 1);
                        }
                        SubledgerRefConfigDA.Delete(tc, sAccountHeadIDs, oACCostCenter.ACCostCenterID);
                    }
                }
                #endregion
                
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oACCostCenter = new ACCostCenter();
                oACCostCenter.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oACCostCenter;
        }

        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ACCostCenter oACCostCenter = new ACCostCenter();
                oACCostCenter.ACCostCenterID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.ACCostCenter, EnumRoleOperationType.Delete);
                ACCostCenterDA.Delete(tc, oACCostCenter, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                #endregion
            }
            return "Data Delete Successfully";
        }

        public ACCostCenter Get(int id, int nUserId)
        {
            ACCostCenter oACCostCenters = new ACCostCenter();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ACCostCenterDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oACCostCenters = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ACCostCenter", e);
                #endregion
            }

            return oACCostCenters;
        }
        public ACCostCenter GetByRef(EnumReferenceType eEnumReferenceType, int nReferenceObjectID, int nUserID)
        {
            ACCostCenter oACCostCenters = new ACCostCenter();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ACCostCenterDA.GetByRef(tc, eEnumReferenceType, nReferenceObjectID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oACCostCenters = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ACCostCenter", e);
                #endregion
            }

            return oACCostCenters;
        }

        public List<ACCostCenter> Gets(int nUserId)
        {
            List<ACCostCenter> oACCostCenters = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ACCostCenterDA.Gets(tc);
                oACCostCenters = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ACCostCenter", e);
                #endregion
            }
            return oACCostCenters;
        }

        public List<ACCostCenter> Gets(int nParentID, int nUserId)
        {
            List<ACCostCenter> oACCostCenters = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ACCostCenterDA.Gets(tc, nParentID);
                oACCostCenters = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ACCostCenter", e);
                #endregion
            }
            return oACCostCenters;
        }

        public List<ACCostCenter> Gets(string sSQL, int nUserId)
        {
            List<ACCostCenter> oACCostCenters = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ACCostCenterDA.Gets(tc, sSQL);
                oACCostCenters = CreateObjects(reader);
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

            return oACCostCenters;
        }

        public List<ACCostCenter> GetsByCodeOrName(ACCostCenter oACCostCenter, int nBUID, int nUserID)
        {
            List<ACCostCenter> oACCostCenters = new List<ACCostCenter>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ACCostCenterDA.GetsByCodeOrName(tc, oACCostCenter, nBUID);
                oACCostCenters = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oACCostCenters = new List<ACCostCenter>();
                oACCostCenter = new ACCostCenter();
                oACCostCenter.ErrorMessage = e.Message;
                oACCostCenters.Add(oACCostCenter);

                //ExceptionLog.Write(e);
                //throw new ServiceException(e.Message);
                #endregion
            }

            return oACCostCenters;
        }
        public List<ACCostCenter> GetsByCode(ACCostCenter oACCostCenter, int nUserID)
        {
            List<ACCostCenter> oACCostCenters = new List<ACCostCenter>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ACCostCenterDA.GetsByCode(tc, oACCostCenter);
                oACCostCenters = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oACCostCenters = new List<ACCostCenter>();
                oACCostCenter = new ACCostCenter();
                oACCostCenter.ErrorMessage = e.Message;
                oACCostCenters.Add(oACCostCenter);

                //ExceptionLog.Write(e);
                //throw new ServiceException(e.Message);
                #endregion
            }

            return oACCostCenters;
        }

        public List<ACCostCenter> GetsByConfigure(int nAccountHeadID, string sCCName, int nBUID, int nUserID)
        {
            List<ACCostCenter> oACCostCenters = new List<ACCostCenter>();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ACCostCenterDA.GetsByConfigure(tc, nAccountHeadID, sCCName, nBUID, nUserID);
                oACCostCenters = CreateObjects(reader);
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

            return oACCostCenters;
        }

    }
}
