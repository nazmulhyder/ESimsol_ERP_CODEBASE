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
    public class FGQCService : MarshalByRefObject, IFGQCService
    {
        #region Private functions and declaration

        private FGQC MapObject(NullHandler oReader)
        {
            FGQC oFGQC = new FGQC();
            oFGQC.FGQCID = oReader.GetInt32("FGQCID");
            oFGQC.FGQCNo = oReader.GetString("FGQCNo");
            oFGQC.FGQCDate = oReader.GetDateTime("FGQCDate");
            oFGQC.BUID = oReader.GetInt32("BUID");
            oFGQC.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oFGQC.Remarks = oReader.GetString("Remarks");
            oFGQC.ApprovedByName = oReader.GetString("ApprovedByName");
            oFGQC.BUName = oReader.GetString("BUName");
            oFGQC.BUShortName = oReader.GetString("BUShortName");
            oFGQC.FGQCAmount = oReader.GetDouble("FGQCAmount");
            return oFGQC;
        }

        private FGQC CreateObject(NullHandler oReader)
        {
            FGQC oFGQC = new FGQC();
            oFGQC = MapObject(oReader);
            return oFGQC;
        }

        private List<FGQC> CreateObjects(IDataReader oReader)
        {
            List<FGQC> oFGQC = new List<FGQC>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FGQC oItem = CreateObject(oHandler);
                oFGQC.Add(oItem);
            }
            return oFGQC;
        }

        #endregion

        #region Interface implementation
        public FGQC Save(FGQC oFGQC, Int64 nUserID)
        {
            TransactionContext tc = null;
            string sFGQCDetailIDs = "";
            List<FGQCDetail> oFGQCDetails = new List<FGQCDetail>();
            oFGQCDetails = oFGQC.FGQCDetails;
            try
            {
                tc = TransactionContext.Begin(true);
                #region FGQC
                IDataReader reader;
                if (oFGQC.FGQCID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.FGQC, EnumRoleOperationType.Add);
                    reader = FGQCDA.InsertUpdate(tc, oFGQC, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.FGQC, EnumRoleOperationType.Edit);
                    VoucherDA.CheckVoucherReference(tc, "FGQC", "FGQCID", oFGQC.FGQCID);
                    reader = FGQCDA.InsertUpdate(tc, oFGQC, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFGQC = new FGQC();
                    oFGQC = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region FGQC Detail Part
                if (oFGQCDetails != null)
                {
                    foreach (FGQCDetail oItem in oFGQCDetails)
                    {
                        IDataReader readerdetail;
                        oItem.FGQCID = oFGQC.FGQCID;
                        if (oItem.FGQCDetailID <= 0)
                        {
                            readerdetail = FGQCDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = FGQCDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sFGQCDetailIDs = sFGQCDetailIDs + oReaderDetail.GetString("FGQCDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sFGQCDetailIDs.Length > 0)
                    {
                        sFGQCDetailIDs = sFGQCDetailIDs.Remove(sFGQCDetailIDs.Length - 1, 1);
                    }
                }
                FGQCDetail oFGQCDetail = new FGQCDetail();
                oFGQCDetail.FGQCID = oFGQC.FGQCID;
                FGQCDetailDA.Delete(tc, oFGQCDetail, EnumDBOperation.Delete, nUserID, sFGQCDetailIDs);
                #endregion

                #region Get FGQC
                reader = FGQCDA.Get(tc, oFGQC.FGQCID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFGQC = new FGQC();
                    oFGQC = CreateObject(oReader);
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
                    oFGQC = new FGQC();
                    oFGQC.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oFGQC;
        }
        public FGQC GetSuggestFGQCDate(string sSQl, Int64 nUserID)
        {
            TransactionContext tc = null;
            FGQC oFGQC = new FGQC();
            try
            {
                tc = TransactionContext.Begin(true);
                #region Get FGQC
                IDataReader reader;
                reader = FGQCDA.Gets(tc, sSQl);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFGQC = new FGQC();
                    oFGQC.FGQCDate = oReader.GetDateTime("SuggestDate");
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
                    oFGQC = new FGQC();
                    oFGQC.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oFGQC;
        }


        public FGQC Approved(FGQC oFGQC, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.FGQC, EnumRoleOperationType.Approved);
                FGQCDA.Approved(tc, oFGQC, nUserID);
                reader = FGQCDA.Get(tc, oFGQC.FGQCID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFGQC = new FGQC();
                    oFGQC = CreateObject(oReader);
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
                    oFGQC = new FGQC();
                    oFGQC.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oFGQC;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FGQC oFGQC = new FGQC();
                oFGQC.FGQCID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.FGQC, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "FGQC", id);
                VoucherDA.CheckVoucherReference(tc, "FGQC", "FGQCID", oFGQC.FGQCID);
                FGQCDA.Delete(tc, oFGQC, EnumDBOperation.Delete, nUserId);
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

        public FGQC Get(int id, Int64 nUserId)
        {
            FGQC oFGQC = new FGQC();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FGQCDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFGQC = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FGQC", e);
                #endregion
            }
            return oFGQC;
        }

        public List<FGQC> Gets(Int64 nUserID)
        {
            List<FGQC> oFGQCs = new List<FGQC>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FGQCDA.Gets(tc);
                oFGQCs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FGQC oFGQC = new FGQC();
                oFGQC.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFGQCs;
        }

        public List<FGQC> Gets(string sSQL, Int64 nUserID)
        {
            List<FGQC> oFGQCs = new List<FGQC>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FGQCDA.Gets(tc, sSQL);
                oFGQCs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FGQC", e);
                #endregion
            }
            return oFGQCs;
        }

        #endregion
    }

}