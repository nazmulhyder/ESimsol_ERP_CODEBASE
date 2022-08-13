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
	public class JobService : MarshalByRefObject, IJobService
	{
		#region Private functions and declaration

		private Job MapObject(NullHandler oReader)
		{
			Job oJob = new Job();
			oJob.JobID = oReader.GetInt32("JobID");
            oJob.JobNo = oReader.GetString("JobNo");
			oJob.IssueDate = oReader.GetDateTime("IssueDate");
			oJob.TechnicalSheetID = oReader.GetInt32("TechnicalSheetID");
            oJob.BUID = oReader.GetInt32("BUID");
			oJob.Remarks = oReader.GetString("Remarks");
			oJob.StyleNo = oReader.GetString("StyleNo");
            oJob.MerchandiserID = oReader.GetInt32("MerchandiserID");
			oJob.MerchandiserName = oReader.GetString("MerchandiserName");
            oJob.BusinessSessionID = oReader.GetInt32("BusinessSessionID");
			oJob.SessionName = oReader.GetString("SessionName");
            oJob.BuyerID = oReader.GetInt32("BuyerID");
			oJob.BuyerName = oReader.GetString("BuyerName");
            oJob.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oJob.ApprovedByName = oReader.GetString("ApprovedByName");
            oJob.IsTAPExist = oReader.GetBoolean("IsTAPExist");
            oJob.IsTAPApproved = oReader.GetBoolean("IsTAPApproved");
			return oJob;
		}

		private Job CreateObject(NullHandler oReader)
		{
			Job oJob = new Job();
			oJob = MapObject(oReader);
			return oJob;
		}

		private List<Job> CreateObjects(IDataReader oReader)
		{
			List<Job> oJob = new List<Job>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				Job oItem = CreateObject(oHandler);
				oJob.Add(oItem);
			}
			return oJob;
		}

		#endregion

		#region Interface implementation
			public Job Save(Job oJob, Int64 nUserID)
			{
                JobDetail oJobDetail = new JobDetail();
                Job oUG = new Job();
                oUG = oJob;

				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);

                    #region Job
                    IDataReader reader;
                    if (oJob.JobID <= 0)
                    {
                        AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.Job, EnumRoleOperationType.Add);
                        reader = JobDA.InsertUpdate(tc, oJob, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.Job, EnumRoleOperationType.Edit);
                        reader = JobDA.InsertUpdate(tc, oJob, EnumDBOperation.Update, nUserID);
                    }
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oJob = new Job();
						oJob = CreateObject(oReader);
					}
					reader.Close();
                    #endregion

                    #region JobDetail

                    if (oJob.JobID > 0)
                    {
                        string sJobDetailIDs = "";
                        if (oUG.JobDetails.Count > 0)
                        {
                            IDataReader readerdetail;
                            foreach (JobDetail oDRD in oUG.JobDetails)
                            {
                                oDRD.JobID = oJob.JobID;
                                if (oDRD.JobDetailID <= 0)
                                {
                                    readerdetail = JobDetailDA.InsertUpdate(tc, oDRD, EnumDBOperation.Insert, nUserID, "");
                                }
                                else
                                {
                                    readerdetail = JobDetailDA.InsertUpdate(tc, oDRD, EnumDBOperation.Update, nUserID, "");

                                }
                                NullHandler oReaderDevRecapdetail = new NullHandler(readerdetail);
                                int nJobDetailID = 0;
                                if (readerdetail.Read())
                                {
                                    nJobDetailID = oReaderDevRecapdetail.GetInt32("JobDetailID");
                                    sJobDetailIDs = sJobDetailIDs + oReaderDevRecapdetail.GetString("JobDetailID") + ",";
                                }
                                readerdetail.Close();
                            }
                        }
                        if (sJobDetailIDs.Length > 0)
                        {
                            sJobDetailIDs = sJobDetailIDs.Remove(sJobDetailIDs.Length - 1, 1);
                        }
                        oJobDetail = new JobDetail();
                        oJobDetail.JobID = oJob.JobID;
                        JobDetailDA.Delete(tc, oJobDetail, EnumDBOperation.Delete, nUserID, sJobDetailIDs);
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
						oJob = new Job();
						oJob.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oJob;
			}
            public Job Approve(Job oJob, Int64 nUserID)
            {
                JobDetail oJobDetail = new JobDetail();
                Job oUG = new Job();
                oUG = oJob;

                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin(true);

                    #region Job
                    IDataReader reader;
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.Job, EnumRoleOperationType.Approved);
                    reader = JobDA.InsertUpdate(tc, oJob, EnumDBOperation.Approval, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oJob = new Job();
                        oJob = CreateObject(oReader);
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
                        oJob = new Job();
                        oJob.ErrorMessage = e.Message.Split('!')[0];
                    }
                    #endregion
                }
                return oJob;
            }
            public Job UndoApprove(Job oJob, Int64 nUserID)
            {
                Job oUG = new Job();
                oUG = oJob;
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin(true);
                    #region Job
                    JobDA.UndoApprove(tc, oJob.JobID);
                    IDataReader reader = JobDA.Get(tc, oJob.JobID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oJob = new Job();
                        oJob = CreateObject(oReader);
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
                        oJob = new Job();
                        oJob.ErrorMessage = e.Message.Split('!')[0];
                    }
                    #endregion
                }
                return oJob;
            }
			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					Job oJob = new Job();
					oJob.JobID = id;
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.Job, EnumRoleOperationType.Delete);
					DBTableReferenceDA.HasReference(tc, "Job", id);
					JobDA.Delete(tc, oJob, EnumDBOperation.Delete, nUserId);
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

			public Job Get(int id, Int64 nUserId)
			{
				Job oJob = new Job();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = JobDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oJob = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get Job", e);
					#endregion
				}
				return oJob;
			}

			public List<Job> Gets(int buid, Int64 nUserID)
			{
				List<Job> oJobs = new List<Job>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
                    reader = JobDA.Gets(buid, tc);
					oJobs = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					Job oJob = new Job();
					oJob.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oJobs;
			}

			public List<Job> Gets (string sSQL, Int64 nUserID)
			{
				List<Job> oJobs = new List<Job>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = JobDA.Gets(tc, sSQL);
					oJobs = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get Job", e);
					#endregion
				}
				return oJobs;
			}

		#endregion
	}

}
