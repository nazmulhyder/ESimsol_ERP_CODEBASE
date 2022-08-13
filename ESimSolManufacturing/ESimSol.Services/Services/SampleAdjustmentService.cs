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
	public class SampleAdjustmentService : MarshalByRefObject, ISampleAdjustmentService
	{
		#region Private functions and declaration

		private SampleAdjustment MapObject(NullHandler oReader)
		{
			SampleAdjustment oSampleAdjustment = new SampleAdjustment();
			oSampleAdjustment.SampleAdjustmentID = oReader.GetInt32("SampleAdjustmentID");
			oSampleAdjustment.AdjustmentNo = oReader.GetString("AdjustmentNo");
            oSampleAdjustment.SANNo = oReader.GetString("SANNo");
			oSampleAdjustment.BUID = oReader.GetInt32("BUID");
			oSampleAdjustment.SANID = oReader.GetInt32("SANID");
			oSampleAdjustment.ContractorID = oReader.GetInt32("ContractorID");
			oSampleAdjustment.CurrencyID = oReader.GetInt32("CurrencyID");
			oSampleAdjustment.IssueDate = oReader.GetDateTime("IssueDate");
            oSampleAdjustment.SANAdjust = oReader.GetDouble("SANAdjust");
            oSampleAdjustment.AlreadyAdjust = oReader.GetDouble("AlreadyAdjust");
            oSampleAdjustment.RemainingAdjust = oReader.GetDouble("RemainingAdjust");
            oSampleAdjustment.WaitingAdjust = oReader.GetDouble("WaitingAdjust");
            oSampleAdjustment.AdjustAmount = oReader.GetDouble("AdjustAmount");
			oSampleAdjustment.Remarks = oReader.GetString("Remarks");
			oSampleAdjustment.ApprovedBy = oReader.GetInt32("ApprovedBy");
			oSampleAdjustment.ContractorName = oReader.GetString("ContractorName");
			oSampleAdjustment.CurrencyName = oReader.GetString("CurrencyName");
            oSampleAdjustment.ApprovedByName = oReader.GetString("ApprovedByName");
			return oSampleAdjustment;
		}

		private SampleAdjustment CreateObject(NullHandler oReader)
		{
			SampleAdjustment oSampleAdjustment = new SampleAdjustment();
			oSampleAdjustment = MapObject(oReader);
			return oSampleAdjustment;
		}

		private List<SampleAdjustment> CreateObjects(IDataReader oReader)
		{
			List<SampleAdjustment> oSampleAdjustment = new List<SampleAdjustment>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				SampleAdjustment oItem = CreateObject(oHandler);
				oSampleAdjustment.Add(oItem);
			}
			return oSampleAdjustment;
		}

		#endregion

		#region Interface implementation
			public SampleAdjustment Save(SampleAdjustment oSampleAdjustment, Int64 nUserID)
			{
				TransactionContext tc = null;
                List<SampleAdjustmentDetail> oSampleAdjustmentDetails = new List<SampleAdjustmentDetail>();
                SampleAdjustmentDetail oSampleAdjustmentDetail = new SampleAdjustmentDetail();
                oSampleAdjustmentDetails = oSampleAdjustment.SampleAdjustmentDetails;
                string sSampleAdjustmentDetailIDs = "";
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oSampleAdjustment.SampleAdjustmentID <= 0)
					{
						AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.SampleAdjustment, EnumRoleOperationType.Add);
						reader = SampleAdjustmentDA.InsertUpdate(tc, oSampleAdjustment, EnumDBOperation.Insert, nUserID);
					}
					else{
						AuthorizationRoleDA.CheckUserPermission(tc, nUserID,  EnumModuleName.SampleAdjustment, EnumRoleOperationType.Edit);
                        VoucherDA.CheckVoucherReference(tc, "SampleAdjustment", "SampleAdjustmentID", oSampleAdjustment.SampleAdjustmentID);
						reader = SampleAdjustmentDA.InsertUpdate(tc, oSampleAdjustment, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oSampleAdjustment = new SampleAdjustment();
						oSampleAdjustment = CreateObject(oReader);
					}
					reader.Close();

                    #region SampleAdjustment Detail Part
                    if (oSampleAdjustmentDetails != null)
                    {
                        foreach (SampleAdjustmentDetail oItem in oSampleAdjustmentDetails)
                        {
                            IDataReader readerdetail;
                            oItem.SampleAdjustmentID = oSampleAdjustment.SampleAdjustmentID;
                            if (oItem.SampleAdjustmentDetailID <= 0)
                            {
                                readerdetail = SampleAdjustmentDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readerdetail = SampleAdjustmentDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                            }
                            NullHandler oReaderDetail = new NullHandler(readerdetail);
                            if (readerdetail.Read())
                            {
                                sSampleAdjustmentDetailIDs = sSampleAdjustmentDetailIDs + oReaderDetail.GetString("SampleAdjustmentDetailID") + ",";
                            }
                            readerdetail.Close();
                        }
                        if (sSampleAdjustmentDetailIDs.Length > 0)
                        {
                            sSampleAdjustmentDetailIDs = sSampleAdjustmentDetailIDs.Remove(sSampleAdjustmentDetailIDs.Length - 1, 1);
                        }
                        oSampleAdjustmentDetail = new SampleAdjustmentDetail();
                        oSampleAdjustmentDetail.SampleAdjustmentID = oSampleAdjustment.SampleAdjustmentID;
                        SampleAdjustmentDetailDA.Delete(tc, oSampleAdjustmentDetail, EnumDBOperation.Delete, nUserID, sSampleAdjustmentDetailIDs);
                    }

                    #endregion

                    #region SampleAdjustment Get
                    reader = SampleAdjustmentDA.Get(tc, oSampleAdjustment.SampleAdjustmentID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oSampleAdjustment = CreateObject(oReader);
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
						oSampleAdjustment = new SampleAdjustment();
						oSampleAdjustment.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oSampleAdjustment;
			}

            public SampleAdjustment Approve(SampleAdjustment oSampleAdjustment, Int64 nUserID)
            {
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin(true);
                    IDataReader reader;
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.SampleAdjustment, EnumRoleOperationType.Approved);
                    reader = SampleAdjustmentDA.InsertUpdate(tc, oSampleAdjustment, EnumDBOperation.Approval, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oSampleAdjustment = new SampleAdjustment();
                        oSampleAdjustment = CreateObject(oReader);
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
                        oSampleAdjustment = new SampleAdjustment();
                        oSampleAdjustment.ErrorMessage = e.Message.Split('!')[0];
                    }
                    #endregion
                }
                return oSampleAdjustment;
            }

            public SampleAdjustment UnApprove(SampleAdjustment oSampleAdjustment, Int64 nUserID)
            {
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin(true);
                    IDataReader reader;
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.SampleAdjustment, EnumRoleOperationType.Approved);
                    reader = SampleAdjustmentDA.InsertUpdate(tc, oSampleAdjustment, EnumDBOperation.UnApproval, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oSampleAdjustment = new SampleAdjustment();
                        oSampleAdjustment = CreateObject(oReader);
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
                        oSampleAdjustment = new SampleAdjustment();
                        oSampleAdjustment.ErrorMessage = e.Message.Split('!')[0];
                    }
                    #endregion
                }
                return oSampleAdjustment;
            }
			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					SampleAdjustment oSampleAdjustment = new SampleAdjustment();
					oSampleAdjustment.SampleAdjustmentID = id;
					AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.SampleAdjustment, EnumRoleOperationType.Delete);
					DBTableReferenceDA.HasReference(tc, "SampleAdjustment", id);
                    VoucherDA.CheckVoucherReference(tc, "SampleAdjustment", "SampleAdjustmentID", oSampleAdjustment.SampleAdjustmentID);
					SampleAdjustmentDA.Delete(tc, oSampleAdjustment, EnumDBOperation.Delete, nUserId);
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

			public SampleAdjustment Get(int id, Int64 nUserId)
			{
				SampleAdjustment oSampleAdjustment = new SampleAdjustment();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = SampleAdjustmentDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oSampleAdjustment = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get SampleAdjustment", e);
					#endregion
				}
				return oSampleAdjustment;
			}

            public List<SampleAdjustment> Process(int BUID, Int64 nUserID)
			{
				List<SampleAdjustment> oSampleAdjustments = new List<SampleAdjustment>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
                    reader = SampleAdjustmentDA.Process(BUID, tc);
					oSampleAdjustments = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					SampleAdjustment oSampleAdjustment = new SampleAdjustment();
					oSampleAdjustment.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oSampleAdjustments;
			}

			public List<SampleAdjustment> Gets (string sSQL, Int64 nUserID)
			{
				List<SampleAdjustment> oSampleAdjustments = new List<SampleAdjustment>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = SampleAdjustmentDA.Gets(tc, sSQL);
					oSampleAdjustments = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get SampleAdjustment", e);
					#endregion
				}
				return oSampleAdjustments;
			}

            public List<SampleAdjustment> Gets(Int64 nUserID)
            {
                List<SampleAdjustment> oSampleAdjustments = new List<SampleAdjustment>();
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin();
                    IDataReader reader = null;
                    reader = SampleAdjustmentDA.Gets(tc);
                    oSampleAdjustments = CreateObjects(reader);
                    reader.Close();
                    tc.End();
                }
                catch (Exception e)
                {
                    #region Handle Exception
                    if (tc != null) ;
                    tc.HandleError();
                    ExceptionLog.Write(e);
                    throw new ServiceException("Failed to Get SampleAdjustment", e);
                    #endregion
                }
                return oSampleAdjustments;
            }

		#endregion
	}

}
