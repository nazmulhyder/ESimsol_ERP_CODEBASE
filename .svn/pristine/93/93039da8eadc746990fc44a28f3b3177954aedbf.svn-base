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
    public class PFmemberService : MarshalByRefObject, IPFmemberService
    {
        #region Private functions and declaration
        private PFmember MapObject(NullHandler oReader)
        {
            PFmember oPFmember = new PFmember();
            oPFmember.PFMID = oReader.GetInt32("PFMID");
            oPFmember.PFSchemeID = oReader.GetInt32("PFSchemeID");
            oPFmember.EmployeeID = oReader.GetInt32("EmployeeID");
            oPFmember.Description = oReader.GetString("Description");
            oPFmember.PFBalance = oReader.GetDouble("PFBalance");
            oPFmember.RequestTo = oReader.GetInt32("RequestTo");
            oPFmember.RequestDate = oReader.GetDateTime("RequestDate");
            oPFmember.RequestBy = oReader.GetInt32("RequestBy");
            oPFmember.RequestByDate = oReader.GetDateTime("RequestByDate");
            oPFmember.ApproveBy = oReader.GetInt32("ApproveBy");
            oPFmember.ApproveByDate = oReader.GetDateTime("ApproveByDate");
            oPFmember.IsActive = oReader.GetBoolean("IsActive");
            oPFmember.InactiveDate = oReader.GetDateTime("InactiveDate");
            oPFmember.PFMembershipDate = oReader.GetDateTime("PFMembershipDate");

            oPFmember.PFSchemeName = oReader.GetString("PFSchemeName");
            oPFmember.EmployeeName = oReader.GetString("EmployeeName");
            oPFmember.EmployeeCode = oReader.GetString("EmployeeCode");
            oPFmember.DepartmentName = oReader.GetString("DepartmentName");
            oPFmember.DesignationName = oReader.GetString("DesignationName");
            oPFmember.RequestToName = oReader.GetString("RequestToName");
            oPFmember.RequestByName = oReader.GetString("RequestByName");
            oPFmember.ApproveByName = oReader.GetString("ApproveByName");
            oPFmember.GrossAmount = oReader.GetDouble("GrossAmount");
            
            return oPFmember;
        }

        public static PFmember CreateObject(NullHandler oReader)
        {
            PFmemberService oPFmemberService = new PFmemberService();
            PFmember oPFmember = oPFmemberService.MapObject(oReader);
            return oPFmember;
        }

        private List<PFmember> CreateObjects(IDataReader oReader)
        {
            List<PFmember> oPFmember = new List<PFmember>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PFmember oItem = CreateObject(oHandler);
                oPFmember.Add(oItem);
            }
            return oPFmember;
        }

        #endregion

        #region Interface implementation
        public PFmemberService() { }

        public PFmember IUD(PFmember oPFmember, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;

                reader = PFmemberDA.IUD(tc, oPFmember, nDBOperation, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPFmember = new PFmember();
                    oPFmember = CreateObject(oReader);
                }
                reader.Close();
                tc.End();

                if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    oPFmember = new PFmember();
                    oPFmember.ErrorMessage = Global.DeleteMessage;
                }
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oPFmember = new PFmember();
                oPFmember.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }
            return oPFmember;
        }

        public PFmember Get(int nPFMID, Int64 nUserId)
        {
            PFmember oPFmember = new PFmember();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PFmemberDA.Get(tc, nPFMID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPFmember = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oPFmember = new PFmember();
                oPFmember.ErrorMessage = "Failed to get member of the provident.";
                #endregion
            }

            return oPFmember;
        }

        public List<PFmember> Gets(string sSQL, Int64 nUserID)
        {
            List<PFmember> oPFmembers = new List<PFmember>(); ;
            PFmember oPFmember = new PFmember();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PFmemberDA.Gets(tc, sSQL);
                oPFmembers = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oPFmember = new PFmember();
                oPFmember.ErrorMessage = "Failed to get member of the provident fund.";
                oPFmembers.Add(oPFmember);
                #endregion
            }

            return oPFmembers;
        }

        #region UploadXL
        public List<PFmember> UploadXL(List<PFmember> oPFmembers, Int64 nUserID)
        {
            PFmember oTempPFmember = new PFmember();
            List<PFmember> oTempPFmembers = new List<PFmember>();
            TransactionContext tc = null;
            try
            {
                int nCount = 0;
                foreach (PFmember oItem in oPFmembers)
                {
                    tc = TransactionContext.Begin(true);
                    IDataReader reader;
                    oTempPFmember = new PFmember();
                    reader = PFmemberDA.UploadXL(tc, oItem, nUserID);
                    if (nCount < 100)
                    {
                        NullHandler oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oTempPFmember = CreateObject(oReader);
                        }
                        if (oTempPFmember.PFMID > 0)
                        {
                            oTempPFmembers.Add(oTempPFmember);
                        }
                    }
                    nCount++;
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
            return oTempPFmembers;
        }
        #endregion UploadXL

        public List<PFmember> GetsPFLedgerReport(string sSQL, Int64 nUserID)
        {
            List<PFmember> oPFmembers = new List<PFmember>(); ;
            PFmember oPFmember = new PFmember();
            TransactionContext tc = null;

            try
            {
                //tc = TransactionContext.Begin();
                //IDataReader reader = null;
                //reader = PFmemberDA.Gets(tc, sSQL);
                //oPFmembers = CreateObjects(reader);
                //reader.Close();
                //tc.End();
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = PFmemberDA.Gets(tc, sSQL);
                NullHandler oreader = new NullHandler(reader);
                while (reader.Read())
                {
                    PFmember oItem = new PFmember();
                    oItem.PFMID = oreader.GetInt32("PFMID");
                    oItem.EmployeeCode = oreader.GetString("EmployeeCode");
                    oItem.EmployeeName = oreader.GetString("EmployeeName");
                    oItem.PFBalance = oreader.GetDouble("PFBalance");
                    oItem.DepartmentName = oreader.GetString("DepartmentName");
                    oItem.DesignationName = oreader.GetString("DesignationName");
                    oItem.DateOfJoin = oreader.GetDateTime("DateOfJoin");
                    oItem.DateOfConfirmation = oreader.GetDateTime("DateOfConfirmation");
                    oItem.ApproveByDate = oreader.GetDateTime("ApproveByDate");
                    oItem.EmployeeContribution = oreader.GetDouble("EmployeeContribution");
                    oItem.IsActive = oreader.GetBoolean("IsActive");
                    oPFmembers.Add(oItem);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oPFmember = new PFmember();
                oPFmember.ErrorMessage = e.Message;
                oPFmembers.Add(oPFmember);
                #endregion
            }
            return oPFmembers;
        }
        #endregion
    }
}