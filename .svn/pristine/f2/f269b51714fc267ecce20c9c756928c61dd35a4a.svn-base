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
    public class GUQCService : MarshalByRefObject, IGUQCService
    {
        #region Private functions and declaration

        private GUQC MapObject(NullHandler oReader)
        {
            GUQC oGUQC = new GUQC();
            oGUQC.GUQCID = oReader.GetInt32("GUQCID");
            oGUQC.QCBy = oReader.GetInt32("QCBy");
            oGUQC.QCDate = oReader.GetDateTime("QCDate");
            oGUQC.ApproveBy = oReader.GetInt32("ApproveBy");
            oGUQC.BUID = oReader.GetInt32("BUID");
            oGUQC.Remarks = oReader.GetString("Remarks");
            oGUQC.QCNo = oReader.GetString("QCNo");
            oGUQC.BuyerID = oReader.GetInt32("BuyerID");
            oGUQC.StoreID = oReader.GetInt32("StoreID");
            
            oGUQC.ApproveDate = oReader.GetDateTime("ApproveDate");
            oGUQC.QCByName = oReader.GetString("QCByName");
            oGUQC.ApproveByName = oReader.GetString("ApproveByName");
            oGUQC.StoreName = oReader.GetString("StoreName");
            oGUQC.BuyerName = oReader.GetString("BuyerName");
            return oGUQC;
        }

        private GUQC CreateObject(NullHandler oReader)
        {
            GUQC oGUQC = new GUQC();
            oGUQC = MapObject(oReader);
            return oGUQC;
        }

        private List<GUQC> CreateObjects(IDataReader oReader)
        {
            List<GUQC> oGUQC = new List<GUQC>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                GUQC oItem = CreateObject(oHandler);
                oGUQC.Add(oItem);
            }
            return oGUQC;
        }

        #endregion

        #region Interface implementation
        public GUQC Save(GUQC oGUQC, Int64 nUserID)
        {
            GUQCDetail oGUQCDetail = new GUQCDetail();
            GUQC oUG = new GUQC();
            oUG = oGUQC;

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                #region GUQC
                IDataReader reader;
                if (oGUQC.GUQCID <= 0)
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.GUQC, EnumRoleOperationType.Add);
                    reader = GUQCDA.InsertUpdate(tc, oGUQC, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.GUQC, EnumRoleOperationType.Edit);
                    reader = GUQCDA.InsertUpdate(tc, oGUQC, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oGUQC = new GUQC();
                    oGUQC = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region GUQCDetail

                if (oGUQC.GUQCID > 0)
                {
                    string sGUQCDetailIDs = "";
                    if (oUG.GUQCDetails.Count > 0)
                    {
                        IDataReader readerdetail;
                        foreach (GUQCDetail oDRD in oUG.GUQCDetails)
                        {
                            oDRD.GUQCID = oGUQC.GUQCID;
                            if (oDRD.GUQCDetailID <= 0)
                            {
                                readerdetail = GUQCDetailDA.InsertUpdate(tc, oDRD, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readerdetail = GUQCDetailDA.InsertUpdate(tc, oDRD, EnumDBOperation.Update, nUserID, "");

                            }
                            NullHandler oReaderDevRecapdetail = new NullHandler(readerdetail);
                            int nGUQCDetailID = 0;
                            if (readerdetail.Read())
                            {
                                nGUQCDetailID = oReaderDevRecapdetail.GetInt32("GUQCDetailID");
                                sGUQCDetailIDs = sGUQCDetailIDs + oReaderDevRecapdetail.GetString("GUQCDetailID") + ",";
                            }
                            readerdetail.Close();
                        }
                    }
                    if (sGUQCDetailIDs.Length > 0)
                    {
                        sGUQCDetailIDs = sGUQCDetailIDs.Remove(sGUQCDetailIDs.Length - 1, 1);
                    }
                    oGUQCDetail = new GUQCDetail();
                    oGUQCDetail.GUQCID = oGUQC.GUQCID;
                    GUQCDetailDA.Delete(tc, oGUQCDetail, EnumDBOperation.Delete, nUserID, sGUQCDetailIDs);
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
                    oGUQC = new GUQC();
                    oGUQC.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oGUQC;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                GUQC oGUQC = new GUQC();
                oGUQC.GUQCID = id;
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.GUQC, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "GUQC", id);
                GUQCDA.Delete(tc, oGUQC, EnumDBOperation.Delete, nUserId);
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

        public GUQC Get(int id, Int64 nUserId)
        {
            GUQC oGUQC = new GUQC();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = GUQCDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oGUQC = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get GUQC", e);
                #endregion
            }
            return oGUQC;
        }

        public List<GUQC> Gets(Int64 nUserID)
        {
            List<GUQC> oGUQCs = new List<GUQC>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = GUQCDA.Gets(tc);
                oGUQCs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                GUQC oGUQC = new GUQC();
                oGUQC.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oGUQCs;
        }

        public List<GUQC> Gets(string sSQL, Int64 nUserID)
        {
            List<GUQC> oGUQCs = new List<GUQC>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = GUQCDA.Gets(tc, sSQL);
                oGUQCs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get GUQC", e);
                #endregion
            }
            return oGUQCs;
        }

        public GUQC Approve(GUQC oGUQC, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                #region GUQC
                IDataReader reader = null;
                if (oGUQC.GUQCID > 0)
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.GUQC, EnumRoleOperationType.Add);
                    reader = GUQCDA.Approve(tc, oGUQC, nUserID);
                }
                
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oGUQC = new GUQC();
                    oGUQC = CreateObject(oReader);
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
                    oGUQC = new GUQC();
                    oGUQC.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oGUQC;
        }

        #endregion
    }

}
