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
    public class LeaveHeadService : MarshalByRefObject, ILeaveHeadService
    {
        #region Private functions and declaration
        private LeaveHead MapObject(NullHandler oReader)
        {
            LeaveHead oLeaveHead = new LeaveHead();
            oLeaveHead.LeaveHeadID = oReader.GetInt32("LeaveHeadID");
            oLeaveHead.Code = oReader.GetInt32("Code");
            oLeaveHead.Name = oReader.GetString("Name");
            oLeaveHead.NameInBangla = oReader.GetString("NameInBangla");
            oLeaveHead.Description = oReader.GetString("Description");
            oLeaveHead.TotalDay = oReader.GetInt32("TotalDay");
            oLeaveHead.RequiredFor = (EnumLeaveRequiredFor)oReader.GetInt16("RequiredFor");
            oLeaveHead.IsActive = oReader.GetBoolean("IsActive");
            oLeaveHead.ShortName = oReader.GetString("ShortName");
            oLeaveHead.IsLWP = oReader.GetBoolean("IsLWP");
            //oLeaveHead.IsHRApproval = oReader.GetBoolean("IsHRApproval");
            return oLeaveHead;
        }

        private LeaveHead CreateObject(NullHandler oReader)
        {
            LeaveHead oLeaveHead = MapObject(oReader);
            return oLeaveHead;
        }

        private List<LeaveHead> CreateObjects(IDataReader oReader)
        {
            List<LeaveHead> oLeaveHead = new List<LeaveHead>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                LeaveHead oItem = CreateObject(oHandler);
                oLeaveHead.Add(oItem);
            }
            return oLeaveHead;
        }

        #endregion

        #region Interface implementation
        public LeaveHeadService() { }

        public LeaveHead Save(LeaveHead oLeaveHead, Int64 nUserID)
        {
            LeaveHead objLeaveHead = new LeaveHead();
            LHRule oLHRule = new LHRule();
            LHRuleDetail oLHRuleDetail = new LHRuleDetail();
            objLeaveHead = oLeaveHead;
            TransactionContext tc = null;
            string sLHRuleIDs = "";
            string sLHRuleDetailIDs = "";
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oLeaveHead.LeaveHeadID <= 0)
                {
                    reader = LeaveHeadDA.InsertUpdate(tc, oLeaveHead, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = LeaveHeadDA.InsertUpdate(tc, oLeaveHead, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLeaveHead = new LeaveHead();
                    oLeaveHead = CreateObject(oReader);
                }
                reader.Close();

                IDataReader readerLHRule;
                foreach (LHRule obj in objLeaveHead.LHRules)
                {
                    obj.LeaveHeadID = oLeaveHead.LeaveHeadID;
                    if (obj.LHRuleID <= 0)
                    {
                        readerLHRule = LHRuleDA.InsertUpdate(tc, obj, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        readerLHRule = LHRuleDA.InsertUpdate(tc, obj, EnumDBOperation.Update, nUserID);
                    }
                    NullHandler oReaderLHRule = new NullHandler(readerLHRule);
                    if (readerLHRule.Read())
                    {
                        oLHRule = new LHRule();
                        oLHRule = LHRuleService.CreateObject(oReaderLHRule);
                        sLHRuleIDs = sLHRuleIDs + oReaderLHRule.GetString("LHRuleID") + ",";
                    }
                    readerLHRule.Close();
                    
                    IDataReader readerLHRuleDetail;
                    foreach (LHRuleDetail objRuleDetail in obj.LHRuleDetails)
                    {
                        objRuleDetail.LHRuleID = oLHRule.LHRuleID;
                        if (objRuleDetail.LHRuleDetailID <= 0)
                        {
                            readerLHRuleDetail = LHRuleDetailDA.InsertUpdate(tc, objRuleDetail, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            readerLHRuleDetail = LHRuleDetailDA.InsertUpdate(tc, objRuleDetail, EnumDBOperation.Update, nUserID);
                        }
                        NullHandler oReaderLHRuleDetail = new NullHandler(readerLHRuleDetail);
                        if (readerLHRuleDetail.Read())
                        {
                            oLHRuleDetail = new LHRuleDetail();
                            oLHRuleDetail = LHRuleDetailService.CreateObject(oReaderLHRuleDetail);
                            sLHRuleDetailIDs = sLHRuleDetailIDs + oReaderLHRuleDetail.GetString("LHRuleDetailID") + ",";
                        }
                        readerLHRuleDetail.Close();
                    }
                    LHRuleDetail oNewLHRuleDetail = new LHRuleDetail();
                    oNewLHRuleDetail.LHRuleID = oLHRule.LHRuleID;
                    if (sLHRuleDetailIDs.Length > 0)
                    {
                        sLHRuleDetailIDs = sLHRuleDetailIDs.Remove(sLHRuleDetailIDs.Length - 1, 1);
                    }
                    if (oNewLHRuleDetail.LHRuleID>0)
                    {
                        LHRuleDetailDA.Delete(tc, oNewLHRuleDetail, EnumDBOperation.Delete, nUserID, sLHRuleDetailIDs);
                    }
                }
                LHRule oNewLHRule = new LHRule();
                oNewLHRule.LeaveHeadID = oLeaveHead.LeaveHeadID;
                if (sLHRuleIDs.Length > 0)
                {
                    sLHRuleIDs = sLHRuleIDs.Remove(sLHRuleIDs.Length - 1, 1);
                }
                LHRuleDA.Delete(tc, oNewLHRule, EnumDBOperation.Delete, nUserID, sLHRuleIDs);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Save LeaveHead. Because of " + e.Message, e);
                #endregion
            }
            return oLeaveHead;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                LeaveHead oLeaveHead = new LeaveHead();
                oLeaveHead.LeaveHeadID = id;
                LeaveHeadDA.Delete(tc, oLeaveHead, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete LeaveHead. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public LeaveHead Get(int id, Int64 nUserId)
        {
            LeaveHead aLeaveHead = new LeaveHead();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = LeaveHeadDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    aLeaveHead = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get LeaveHead", e);
                #endregion
            }

            return aLeaveHead;
        }

        public List<LeaveHead> Gets(Int64 nUserID)
        {
            List<LeaveHead> oLeaveHead = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LeaveHeadDA.Gets(tc);
                oLeaveHead = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LeaveHead", e);
                #endregion
            }

            return oLeaveHead;
        }

        public string ChangeActiveStatus(LeaveHead oLeaveHead, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                LeaveHeadDA.ChangeActiveStatus(tc, oLeaveHead);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete Product. Because of " + e.Message, e);
                #endregion
            }
            return "Update sucessfully";
        }

        public List<LeaveHead> Gets(string sSQL, Int64 nUserID)
        {
            List<LeaveHead> oLeaveHeads = new List<LeaveHead>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LeaveHeadDA.Gets(tc, sSQL);
                oLeaveHeads = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LeaveHead", e);
                #endregion
            }

            return oLeaveHeads;
        }

        #endregion
    }
}
