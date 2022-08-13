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
    public class PerformanceIncentiveMemberService : MarshalByRefObject, IPerformanceIncentiveMemberService
    {
        #region Private functions and declaration
        private PerformanceIncentiveMember MapObject(NullHandler oReader)
        {
            PerformanceIncentiveMember oPerformanceIncentiveMember = new PerformanceIncentiveMember();

            oPerformanceIncentiveMember.PIMemberID = oReader.GetInt32("PIMemberID");
            oPerformanceIncentiveMember.PIID = oReader.GetInt32("PIID");
            oPerformanceIncentiveMember.EmployeeID = oReader.GetInt32("EmployeeID");
            oPerformanceIncentiveMember.Rate = oReader.GetDouble("Rate");
            oPerformanceIncentiveMember.ApproveBy = oReader.GetInt32("ApproveBy");
            oPerformanceIncentiveMember.ApproveByDate = oReader.GetDateTime("ApproveByDate");
            oPerformanceIncentiveMember.InactiveBy = oReader.GetInt32("InactiveBy");
            oPerformanceIncentiveMember.InactiveByDate = oReader.GetDateTime("InactiveByDate");
            oPerformanceIncentiveMember.EncryptPIMemberID = Global.Encrypt(oPerformanceIncentiveMember.PIMemberID.ToString());
            oPerformanceIncentiveMember.InactiveByName = oReader.GetString("InactiveByName");
            oPerformanceIncentiveMember.ApproveByName = oReader.GetString("ApproveByName");
            oPerformanceIncentiveMember.EmployeeName = oReader.GetString("EmployeeName");
            oPerformanceIncentiveMember.EmployeeCode = oReader.GetString("EmployeeCode");
            oPerformanceIncentiveMember.PICode = oReader.GetString("PICode");
            oPerformanceIncentiveMember.PIName = oReader.GetString("PIName");
            return oPerformanceIncentiveMember;

        }

        private PerformanceIncentiveMember CreateObject(NullHandler oReader)
        {
            PerformanceIncentiveMember oPerformanceIncentiveMember = MapObject(oReader);
            return oPerformanceIncentiveMember;
        }

        private List<PerformanceIncentiveMember> CreateObjects(IDataReader oReader)
        {
            List<PerformanceIncentiveMember> oPerformanceIncentiveMembers = new List<PerformanceIncentiveMember>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PerformanceIncentiveMember oItem = CreateObject(oHandler);
                oPerformanceIncentiveMembers.Add(oItem);
            }
            return oPerformanceIncentiveMembers;
        }

        #endregion

        #region Interface implementation
        public PerformanceIncentiveMemberService() { }

        public List<PerformanceIncentiveMember>  IUD(List<PerformanceIncentiveMember> oPerformanceIncentiveMembers, int nDBOperation, Int64 nUserID)
        {
            List<PerformanceIncentiveMember> oPIMs = new List<PerformanceIncentiveMember>();
            PerformanceIncentiveMember oPIM = new PerformanceIncentiveMember();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                foreach (PerformanceIncentiveMember oItem in oPerformanceIncentiveMembers)
                {
                    IDataReader reader;
                    reader = PerformanceIncentiveMemberDA.IUD(tc, oItem, nUserID, nDBOperation);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oPIM = new PerformanceIncentiveMember();
                        oPIM = CreateObject(oReader);
                    }
                    if (oPIM.PIMemberID > 0) { oPIMs.Add(oPIM); }
                    reader.Close();
                }
                if (nDBOperation == (int)EnumDBOperation.Delete) { oPIM.ErrorMessage = Global.DeleteMessage; oPIMs.Add(oPIM); }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                throw new ServiceException(e.Message);
                #endregion
            }
            return oPIMs;
        }


        public PerformanceIncentiveMember Get(int nPIID, Int64 nUserId)
        {
            PerformanceIncentiveMember oPerformanceIncentiveMember = new PerformanceIncentiveMember();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PerformanceIncentiveMemberDA.Get(nPIID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPerformanceIncentiveMember = CreateObject(oReader);
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

                oPerformanceIncentiveMember.ErrorMessage = e.Message;
                #endregion
            }

            return oPerformanceIncentiveMember;
        }

        public PerformanceIncentiveMember Get(string sSQL, Int64 nUserId)
        {
            PerformanceIncentiveMember oPerformanceIncentiveMember = new PerformanceIncentiveMember();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PerformanceIncentiveMemberDA.Get(sSQL, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPerformanceIncentiveMember = CreateObject(oReader);
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

                oPerformanceIncentiveMember.ErrorMessage = e.Message;
                #endregion
            }

            return oPerformanceIncentiveMember;
        }

        public List<PerformanceIncentiveMember> Gets(Int64 nUserID)
        {
            List<PerformanceIncentiveMember> oPerformanceIncentiveMember = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PerformanceIncentiveMemberDA.Gets(tc);
                oPerformanceIncentiveMember = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PerformanceIncentiveMember", e);
                #endregion
            }
            return oPerformanceIncentiveMember;
        }

        public List<PerformanceIncentiveMember> Gets(string sSQL, Int64 nUserID)
        {
            List<PerformanceIncentiveMember> oPerformanceIncentiveMember = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PerformanceIncentiveMemberDA.Gets(sSQL, tc);
                oPerformanceIncentiveMember = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PerformanceIncentiveMember", e);
                #endregion
            }
            return oPerformanceIncentiveMember;
        }

        #region Activity
        public PerformanceIncentiveMember InActive(int nPIID, Int64 nUserId)
        {
            PerformanceIncentiveMember oPerformanceIncentiveMember = new PerformanceIncentiveMember();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PerformanceIncentiveMemberDA.InActive(nPIID, nUserId, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPerformanceIncentiveMember = CreateObject(oReader);
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
                oPerformanceIncentiveMember.ErrorMessage = e.Message;
                #endregion
            }

            return oPerformanceIncentiveMember;
        }


        #endregion

        #region Approve
        public List<PerformanceIncentiveMember> Approve(string sPMIIDs, Int64 nUserId)
        {
            List<PerformanceIncentiveMember> oPerformanceIncentiveMembers = new List<PerformanceIncentiveMember>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = PerformanceIncentiveMemberDA.Approve(sPMIIDs,nUserId, tc);
                oPerformanceIncentiveMembers = CreateObjects(reader);
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
                //oPerformanceIncentiveMember.ErrorMessage = e.Message;
                #endregion
            }

            return oPerformanceIncentiveMembers;
        }


        #endregion

        #region UploadXL
        public List<PerformanceIncentiveMember> UploadPIMXL(List<PerformanceIncentiveMember> oPerformanceIncentiveMembers, Int64 nUserID)
        {
            PerformanceIncentiveMember oTempPIM = new PerformanceIncentiveMember();
            List<PerformanceIncentiveMember> oPIMs = new List<PerformanceIncentiveMember>();
            TransactionContext tc = null;
            try
            {
                foreach (PerformanceIncentiveMember oItem in oPerformanceIncentiveMembers)
                {
                    tc = TransactionContext.Begin(true);
                    IDataReader reader;
                    oTempPIM = new PerformanceIncentiveMember();
                    reader = PerformanceIncentiveMemberDA.Upload_PIMember(tc, oItem, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oTempPIM = CreateObject(oReader);
                    }
                    if (oTempPIM.PIMemberID>0){oPIMs.Add(oTempPIM);}
                    reader.Close();
                    tc.End();
                }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                throw new ServiceException(e.Message.Split('!')[0]);
                #endregion
            }
            return oPIMs;
        }
        #endregion UploadXl

        #endregion


    }
}
