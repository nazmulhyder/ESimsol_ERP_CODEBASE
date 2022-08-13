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
	public class KnitDyeingProgramService : MarshalByRefObject, IKnitDyeingProgramService
	{
		#region Private functions and declaration

		private KnitDyeingProgram MapObject(NullHandler oReader)
		{
			KnitDyeingProgram oKnitDyeingProgram = new KnitDyeingProgram();
			oKnitDyeingProgram.KnitDyeingProgramID = oReader.GetInt32("KnitDyeingProgramID");
            oKnitDyeingProgram.KnitDyeingProgramLogID = oReader.GetInt32("KnitDyeingProgramLogID");
			oKnitDyeingProgram.BUID = oReader.GetInt32("BUID");
			oKnitDyeingProgram.RefNo = oReader.GetString("RefNo");
			oKnitDyeingProgram.IssueDate = oReader.GetDateTime("IssueDate");
			oKnitDyeingProgram.TechnicalSheetID = oReader.GetInt32("TechnicalSheetID");
			oKnitDyeingProgram.FabricID = oReader.GetInt32("FabricID");
			oKnitDyeingProgram.OrderQty = oReader.GetDouble("OrderQty");
			oKnitDyeingProgram.MUnitID = oReader.GetInt32("MUnitID");
            oKnitDyeingProgram.MUSymbol = oReader.GetString("MUSymbol");
			oKnitDyeingProgram.MerchandiserID = oReader.GetInt32("MerchandiserID");
            oKnitDyeingProgram.DyedType = (EnumDyedType)oReader.GetInt16("DyedType");
			oKnitDyeingProgram.ShipmentDate = oReader.GetDateTime("ShipmentDate");
			oKnitDyeingProgram.GSMID = oReader.GetInt32("GSMID");
			oKnitDyeingProgram.SrinkageTollarance = oReader.GetString("SrinkageTollarance");
			oKnitDyeingProgram.Remarks = oReader.GetString("Remarks");
			oKnitDyeingProgram.TermsAndCondition = oReader.GetString("TermsAndCondition");
			oKnitDyeingProgram.TimeOfArrivalYarn = oReader.GetDateTime("TimeOfArrivalYarn");
			oKnitDyeingProgram.TimeOfArrivalYarnNote = oReader.GetString("TimeOfArrivalYarnNote");
			oKnitDyeingProgram.TimeOfCompletionKnitting = oReader.GetDateTime("TimeOfCompletionKnitting");
			oKnitDyeingProgram.TimeOfCompletionKnittingNote = oReader.GetString("TimeOfCompletionKnittingNote");
			oKnitDyeingProgram.TimeOfCompletionDying = oReader.GetDateTime("TimeOfCompletionDying");
			oKnitDyeingProgram.TimeOfCompletionDyingNote = oReader.GetString("TimeOfCompletionDyingNote");
			oKnitDyeingProgram.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oKnitDyeingProgram.ApprovedDate = oReader.GetDateTime("ApprovedDate");
            oKnitDyeingProgram.FabricName = oReader.GetString("FabricName");
            oKnitDyeingProgram.StyleNo = oReader.GetString("StyleNo");
            oKnitDyeingProgram.BuyerName = oReader.GetString("BuyerName");
            oKnitDyeingProgram.ApprovedByName = oReader.GetString("ApprovedByName");
            oKnitDyeingProgram.GSMName = oReader.GetString("GSMName");
            oKnitDyeingProgram.ProgramType = (EnumProgramType) oReader.GetInt32("ProgramType");
            oKnitDyeingProgram.ProgramTypeInt = oReader.GetInt32("ProgramType");

            oKnitDyeingProgram.RefType = (EnumKnitDyeingProgramRefType)oReader.GetInt32("RefType");
            oKnitDyeingProgram.RefTypeInt = oReader.GetInt32("RefType");
            
            oKnitDyeingProgram.MerchandiserName = oReader.GetString("MerchandiserName");
            oKnitDyeingProgram.KnitDyeingProgramStatus = (EnumKnitDyeingProgramStatus)oReader.GetInt32("KnitDyeingProgramStatus");
            oKnitDyeingProgram.KnitDyeingProgramStatusInt = oReader.GetInt32("KnitDyeingProgramStatus");
            oKnitDyeingProgram.SessionID = oReader.GetInt32("SessionID");
            oKnitDyeingProgram.SessionName = oReader.GetString("SessionName");
            oKnitDyeingProgram.RecapOrPAMNos = oReader.GetString("RecapOrPAMNos");
            oKnitDyeingProgram.TotalReqFabricQty = oReader.GetDouble("TotalReqFabricQty");
            oKnitDyeingProgram.ORPackingPolicyCount = oReader.GetInt32("ORPackingPolicyCount");
            
			return oKnitDyeingProgram;
		}

		private KnitDyeingProgram CreateObject(NullHandler oReader)
		{
			KnitDyeingProgram oKnitDyeingProgram = new KnitDyeingProgram();
			oKnitDyeingProgram = MapObject(oReader);
			return oKnitDyeingProgram;
		}

		private List<KnitDyeingProgram> CreateObjects(IDataReader oReader)
		{
			List<KnitDyeingProgram> oKnitDyeingProgram = new List<KnitDyeingProgram>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				KnitDyeingProgram oItem = CreateObject(oHandler);
				oKnitDyeingProgram.Add(oItem);
			}
			return oKnitDyeingProgram;
		}

		#endregion

		#region Interface implementation
			public KnitDyeingProgram Save(KnitDyeingProgram oKnitDyeingProgram, Int64 nUserID)
			{
				TransactionContext tc = null;
                string sKnitDyeingYarnRequisitions = "";
                List<KnitDyeingYarnRequisition> oKnitDyeingYarnRequisitions = new List<KnitDyeingYarnRequisition>();
                KnitDyeingYarnRequisition oKnitDyeingYarnRequisition = new KnitDyeingYarnRequisition();
                oKnitDyeingYarnRequisitions = oKnitDyeingProgram.KnitDyeingYarnRequisitions;
                string sKnitDyeingProgramDetails = "";
                List<KnitDyeingProgramDetail> oKnitDyeingProgramDetails = new List<KnitDyeingProgramDetail>();
                KnitDyeingProgramDetail oKnitDyeingProgramDetail = new KnitDyeingProgramDetail();
                oKnitDyeingProgramDetails = oKnitDyeingProgram.KnitDyeingProgramDetails;
                KnitDyeingYarnConsumption oKnitDyeingYarnConsumption = new KnitDyeingYarnConsumption();
                List<KnitDyeingYarnConsumption> oKnitDyeingYarnConsumptions = new List<KnitDyeingYarnConsumption>();
                string sKnitDyeingYarnConsumptions = "";
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oKnitDyeingProgram.KnitDyeingProgramID <= 0)
					{
                        AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.KnitDyeingProgram, EnumRoleOperationType.Add);
						reader = KnitDyeingProgramDA.InsertUpdate(tc, oKnitDyeingProgram, EnumDBOperation.Insert, nUserID);
					}
					else
                    {
                        AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.KnitDyeingProgram, EnumRoleOperationType.Edit);
						reader = KnitDyeingProgramDA.InsertUpdate(tc, oKnitDyeingProgram, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oKnitDyeingProgram = new KnitDyeingProgram();
						oKnitDyeingProgram = CreateObject(oReader);
					}
					reader.Close();


                    #region KnitDyeingYarnRequisitions Part

                    foreach (KnitDyeingYarnRequisition oItem in oKnitDyeingYarnRequisitions)
                    {
                        IDataReader readerdetail;
                        oItem.KnitDyeingProgramID = oKnitDyeingProgram.KnitDyeingProgramID;
                        if (oItem.KnitDyeingYarnRequisitionID <= 0)
                        {
                            readerdetail = KnitDyeingYarnRequisitionDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID,"");
                        }
                        else
                        {
                            readerdetail = KnitDyeingYarnRequisitionDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID,"");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sKnitDyeingYarnRequisitions = sKnitDyeingYarnRequisitions + oReaderDetail.GetString("KnitDyeingYarnRequisitionID") + ",";
                        }
                        readerdetail.Close();
                    }

                    if (sKnitDyeingYarnRequisitions.Length > 0)
                    {
                        sKnitDyeingYarnRequisitions = sKnitDyeingYarnRequisitions.Remove(sKnitDyeingYarnRequisitions.Length - 1, 1);
                    }
                    oKnitDyeingYarnRequisition = new KnitDyeingYarnRequisition();
                    oKnitDyeingYarnRequisition.KnitDyeingProgramID = oKnitDyeingProgram.KnitDyeingProgramID;
                    KnitDyeingYarnRequisitionDA.Delete(tc, oKnitDyeingYarnRequisition, EnumDBOperation.Delete, nUserID, sKnitDyeingYarnRequisitions);
                    #endregion


                    #region oKnitDyeingProgramDetail Part
                    int nKnitDyeingProgramDetailID = 0;
                    foreach (KnitDyeingProgramDetail oItem in oKnitDyeingProgramDetails)
                    {
                      
                        oKnitDyeingYarnConsumptions = new List<KnitDyeingYarnConsumption>();
                        oKnitDyeingYarnConsumptions = oItem.KnitDyeingYarnConsumptions;

                        IDataReader readerdetail;
                        oItem.KnitDyeingProgramID = oKnitDyeingProgram.KnitDyeingProgramID;
                        if (oItem.KnitDyeingProgramDetailID <= 0)
                        {
                            readerdetail = KnitDyeingProgramDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = KnitDyeingProgramDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            nKnitDyeingProgramDetailID = 0;
                            nKnitDyeingProgramDetailID = oReaderDetail.GetInt32("KnitDyeingProgramDetailID");
                            sKnitDyeingProgramDetails = sKnitDyeingProgramDetails + oReaderDetail.GetString("KnitDyeingProgramDetailID") + ",";
                        }
                        readerdetail.Close();

                        #region KnitDyeingYarnConsumptions Part
                        sKnitDyeingYarnConsumptions = "";
                        foreach (KnitDyeingYarnConsumption oItemCons in oKnitDyeingYarnConsumptions)
                        {
                            IDataReader readeryarnconsumption;
                            oItemCons.KnitDyeingProgramDetailID = nKnitDyeingProgramDetailID;
                            if (oItemCons.KnitDyeingYarnConsumptionID <= 0)
                            {
                                readeryarnconsumption = KnitDyeingYarnConsumptionDA.InsertUpdate(tc, oItemCons, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readeryarnconsumption = KnitDyeingYarnConsumptionDA.InsertUpdate(tc, oItemCons, EnumDBOperation.Update, nUserID, "");
                            }
                            NullHandler oReaderYarnConsumption = new NullHandler(readeryarnconsumption);
                            if (readeryarnconsumption.Read())
                            {
                                sKnitDyeingYarnConsumptions = sKnitDyeingYarnConsumptions + oReaderYarnConsumption.GetString("KnitDyeingYarnConsumptionID") + ",";
                            }
                            readeryarnconsumption.Close();
                        }

                        if (sKnitDyeingYarnConsumptions.Length > 0)
                        {
                            sKnitDyeingYarnConsumptions = sKnitDyeingYarnConsumptions.Remove(sKnitDyeingYarnConsumptions.Length - 1, 1);
                        }
                        oKnitDyeingYarnConsumption = new KnitDyeingYarnConsumption();
                        oKnitDyeingYarnConsumption.KnitDyeingProgramDetailID = nKnitDyeingProgramDetailID;
                        KnitDyeingYarnConsumptionDA.Delete(tc, oKnitDyeingYarnConsumption, EnumDBOperation.Delete, nUserID, sKnitDyeingYarnConsumptions);
                        #endregion
                    }
                    if (sKnitDyeingProgramDetails.Length > 0)
                    {
                        sKnitDyeingProgramDetails = sKnitDyeingProgramDetails.Remove(sKnitDyeingProgramDetails.Length - 1, 1);
                    }
                    oKnitDyeingProgramDetail = new KnitDyeingProgramDetail();
                    oKnitDyeingProgramDetail.KnitDyeingProgramID = oKnitDyeingProgram.KnitDyeingProgramID;
                    KnitDyeingProgramDetailDA.Delete(tc, oKnitDyeingProgramDetail, EnumDBOperation.Delete, nUserID, sKnitDyeingProgramDetails);
                    #endregion

                    #region Get KnitDyeing Program
                    reader = KnitDyeingProgramDA.Get(tc, oKnitDyeingProgram.KnitDyeingProgramID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oKnitDyeingProgram = new KnitDyeingProgram();
                        oKnitDyeingProgram = CreateObject(oReader);
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
						oKnitDyeingProgram = new KnitDyeingProgram();
						oKnitDyeingProgram.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oKnitDyeingProgram;
			}
            public string CommitGrace(KnitDyeingProgram oKnitDyeingProgram, Int64 nUserID)
			{
				TransactionContext tc = null;                
                List<KnitDyeingYarnRequisition> oKnitDyeingYarnRequisitions = new List<KnitDyeingYarnRequisition>();
                List<KnitDyeingProgramDetail> oKnitDyeingProgramDetails = new List<KnitDyeingProgramDetail>();
                
                oKnitDyeingYarnRequisitions = oKnitDyeingProgram.KnitDyeingYarnRequisitions;
                oKnitDyeingProgramDetails = oKnitDyeingProgram.KnitDyeingProgramDetails;
				try
				{
					tc = TransactionContext.Begin(true);

                    #region KnitDyeingYarnRequisitions Part
                    foreach (KnitDyeingYarnRequisition oItem in oKnitDyeingYarnRequisitions)
                    {
                        KnitDyeingYarnRequisitionDA.CommitGrace(tc, oItem);
                    }
                    #endregion

                    #region KnitDyeingProgramDetail Part
                    foreach (KnitDyeingProgramDetail oItem in oKnitDyeingProgramDetails)
                    {                      
                        KnitDyeingProgramDetailDA.CommitGrace(tc, oItem);

                        #region KnitDyeingYarnConsumptions Part                        
                        foreach (KnitDyeingYarnConsumption oItemCons in oItem.KnitDyeingYarnConsumptions)
                        {                            
                            KnitDyeingYarnConsumptionDA.CommitGrace(tc, oItemCons);
                        }
                        #endregion
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
						return e.Message.Split('!')[0];
					}
					#endregion
				}
				return Global.SuccessMessage;
			}
        
            public KnitDyeingProgram AcceptRevise(KnitDyeingProgram oKnitDyeingProgram, Int64 nUserID)
			{
				TransactionContext tc = null;
                string sKnitDyeingYarnRequisitions = "";
                List<KnitDyeingYarnRequisition> oKnitDyeingYarnRequisitions = new List<KnitDyeingYarnRequisition>();
                KnitDyeingYarnRequisition oKnitDyeingYarnRequisition = new KnitDyeingYarnRequisition();
                oKnitDyeingYarnRequisitions = oKnitDyeingProgram.KnitDyeingYarnRequisitions;
                string sKnitDyeingProgramDetails = "";
                List<KnitDyeingProgramDetail> oKnitDyeingProgramDetails = new List<KnitDyeingProgramDetail>();
                KnitDyeingProgramDetail oKnitDyeingProgramDetail = new KnitDyeingProgramDetail();
                oKnitDyeingProgramDetails = oKnitDyeingProgram.KnitDyeingProgramDetails;
                KnitDyeingYarnConsumption oKnitDyeingYarnConsumption = new KnitDyeingYarnConsumption();
                List<KnitDyeingYarnConsumption> oKnitDyeingYarnConsumptions = new List<KnitDyeingYarnConsumption>();
                string sKnitDyeingYarnConsumptions = "";
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;

                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.KnitDyeingProgram, EnumRoleOperationType.Amendment);
                    reader = KnitDyeingProgramDA.AcceptRevise(tc, oKnitDyeingProgram,  nUserID);
			
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oKnitDyeingProgram = new KnitDyeingProgram();
						oKnitDyeingProgram = CreateObject(oReader);
					}
					reader.Close();


                    #region KnitDyeingYarnRequisitions Part

                    foreach (KnitDyeingYarnRequisition oItem in oKnitDyeingYarnRequisitions)
                    {
                        IDataReader readerdetail;
                        oItem.KnitDyeingProgramID = oKnitDyeingProgram.KnitDyeingProgramID;
                        if (oItem.KnitDyeingYarnRequisitionID <= 0)
                        {
                            readerdetail = KnitDyeingYarnRequisitionDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID,"");
                        }
                        else
                        {
                            readerdetail = KnitDyeingYarnRequisitionDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID,"");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sKnitDyeingYarnRequisitions = sKnitDyeingYarnRequisitions + oReaderDetail.GetString("KnitDyeingYarnRequisitionID") + ",";
                        }
                        readerdetail.Close();
                    }

                    if (sKnitDyeingYarnRequisitions.Length > 0)
                    {
                        sKnitDyeingYarnRequisitions = sKnitDyeingYarnRequisitions.Remove(sKnitDyeingYarnRequisitions.Length - 1, 1);
                    }
                    oKnitDyeingYarnRequisition = new KnitDyeingYarnRequisition();
                    oKnitDyeingYarnRequisition.KnitDyeingProgramID = oKnitDyeingProgram.KnitDyeingProgramID;
                    KnitDyeingYarnRequisitionDA.Delete(tc, oKnitDyeingYarnRequisition, EnumDBOperation.Delete, nUserID, sKnitDyeingYarnRequisitions);
                    #endregion

                    #region oKnitDyeingProgramDetail Part
                    int nKnitDyeingProgramDetailID = 0;
                    foreach (KnitDyeingProgramDetail oItem in oKnitDyeingProgramDetails)
                    {
                        oKnitDyeingYarnConsumptions = new List<KnitDyeingYarnConsumption>();
                        oKnitDyeingYarnConsumptions = oItem.KnitDyeingYarnConsumptions;
                        IDataReader readerdetail;
                        oItem.KnitDyeingProgramID = oKnitDyeingProgram.KnitDyeingProgramID;
                        if (oItem.KnitDyeingProgramDetailID <= 0)
                        {
                            readerdetail = KnitDyeingProgramDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = KnitDyeingProgramDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            nKnitDyeingProgramDetailID = 0;
                            nKnitDyeingProgramDetailID = oReaderDetail.GetInt32("KnitDyeingProgramDetailID");
                            sKnitDyeingProgramDetails = sKnitDyeingProgramDetails + oReaderDetail.GetString("KnitDyeingProgramDetailID") + ",";
                        }
                        readerdetail.Close();

                        #region KnitDyeingYarnConsumptions Part
                        sKnitDyeingYarnConsumptions = "";
                        foreach (KnitDyeingYarnConsumption oItemCons in oKnitDyeingYarnConsumptions)
                        {
                            IDataReader readeryarnconsumption;
                            oItemCons.KnitDyeingProgramDetailID = nKnitDyeingProgramDetailID;
                            if (oItemCons.KnitDyeingYarnConsumptionID <= 0)
                            {
                                readeryarnconsumption = KnitDyeingYarnConsumptionDA.InsertUpdate(tc, oItemCons, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readeryarnconsumption = KnitDyeingYarnConsumptionDA.InsertUpdate(tc, oItemCons, EnumDBOperation.Update, nUserID, "");
                            }
                            NullHandler oReaderYarnConsumption = new NullHandler(readeryarnconsumption);
                            if (readeryarnconsumption.Read())
                            {
                                sKnitDyeingYarnConsumptions = sKnitDyeingYarnConsumptions + oReaderYarnConsumption.GetString("KnitDyeingYarnConsumptionID") + ",";
                            }
                            readeryarnconsumption.Close();
                        }

                        if (sKnitDyeingYarnConsumptions.Length > 0)
                        {
                            sKnitDyeingYarnConsumptions = sKnitDyeingYarnConsumptions.Remove(sKnitDyeingYarnConsumptions.Length - 1, 1);
                        }
                        oKnitDyeingYarnConsumption = new KnitDyeingYarnConsumption();
                        oKnitDyeingYarnConsumption.KnitDyeingProgramDetailID = nKnitDyeingProgramDetailID;
                        KnitDyeingYarnConsumptionDA.Delete(tc, oKnitDyeingYarnConsumption, EnumDBOperation.Delete, nUserID, sKnitDyeingYarnConsumptions);
                        #endregion
                    }

                    if (sKnitDyeingProgramDetails.Length > 0)
                    {
                        sKnitDyeingProgramDetails = sKnitDyeingProgramDetails.Remove(sKnitDyeingProgramDetails.Length - 1, 1);
                    }
                    oKnitDyeingProgramDetail = new KnitDyeingProgramDetail();
                    oKnitDyeingProgramDetail.KnitDyeingProgramID = oKnitDyeingProgram.KnitDyeingProgramID;
                    KnitDyeingProgramDetailDA.Delete(tc, oKnitDyeingProgramDetail, EnumDBOperation.Delete, nUserID, sKnitDyeingProgramDetails);
                    #endregion

                    #region Production Start CAll Again
                    reader = KnitDyeingProgramDA.ProductionStart(tc, oKnitDyeingProgram, nUserID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oKnitDyeingProgram = new KnitDyeingProgram();
                        oKnitDyeingProgram = CreateObject(oReader);
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
						oKnitDyeingProgram = new KnitDyeingProgram();
						oKnitDyeingProgram.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oKnitDyeingProgram;
			}
            public KnitDyeingProgram Approve(KnitDyeingProgram oKnitDyeingProgram, Int64 nUserID)
            {
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin(true);
                    IDataReader reader;
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.KnitDyeingProgram, EnumRoleOperationType.Approved);
                    reader = KnitDyeingProgramDA.InsertUpdate(tc, oKnitDyeingProgram, EnumDBOperation.Approval, nUserID);
         
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oKnitDyeingProgram = new KnitDyeingProgram();
                        oKnitDyeingProgram = CreateObject(oReader);
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
                        oKnitDyeingProgram = new KnitDyeingProgram();
                        oKnitDyeingProgram.ErrorMessage = e.Message.Split('!')[0];
                    }
                    #endregion
                }
                return oKnitDyeingProgram;
            }

            public KnitDyeingProgram SendToFactory(KnitDyeingProgram oKnitDyeingProgram, Int64 nUserID)
            {
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin(true);
                    
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.KnitDyeingProgram, EnumRoleOperationType.SentToProduction);
                    KnitDyeingProgramDA.SendToFactory(tc, oKnitDyeingProgram, nUserID);

                    #region Get KnitDyeing Program
                    IDataReader reader;
                    reader = KnitDyeingProgramDA.Get(tc, oKnitDyeingProgram.KnitDyeingProgramID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oKnitDyeingProgram = new KnitDyeingProgram();
                        oKnitDyeingProgram = CreateObject(oReader);
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
                        oKnitDyeingProgram = new KnitDyeingProgram();
                        oKnitDyeingProgram.ErrorMessage = e.Message.Split('!')[0];
                    }
                    #endregion
                }
                return oKnitDyeingProgram;
            }
            public KnitDyeingProgram ProductionStart(KnitDyeingProgram oKnitDyeingProgram, Int64 nUserID)
            {
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin(true);
                    IDataReader reader;
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.KnitDyeingProgram, EnumRoleOperationType.ProductionProcess);
                    reader = KnitDyeingProgramDA.ProductionStart(tc, oKnitDyeingProgram, nUserID);

                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oKnitDyeingProgram = new KnitDyeingProgram();
                        oKnitDyeingProgram = CreateObject(oReader);
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
                        oKnitDyeingProgram = new KnitDyeingProgram();
                        oKnitDyeingProgram.ErrorMessage = e.Message.Split('!')[0];
                    }
                    #endregion
                }
                return oKnitDyeingProgram;
            }
			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					KnitDyeingProgram oKnitDyeingProgram = new KnitDyeingProgram();
					oKnitDyeingProgram.KnitDyeingProgramID = id;
					DBTableReferenceDA.HasReference(tc, "KnitDyeingProgram", id);
					KnitDyeingProgramDA.Delete(tc, oKnitDyeingProgram, EnumDBOperation.Delete, nUserId);
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

			public KnitDyeingProgram Get(int id, Int64 nUserId)
			{
				KnitDyeingProgram oKnitDyeingProgram = new KnitDyeingProgram();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = KnitDyeingProgramDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oKnitDyeingProgram = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get KnitDyeingProgram", e);
					#endregion
				}
				return oKnitDyeingProgram;
			}

            public KnitDyeingProgram GetLog(int LogId, Int64 nUserId)
			{
				KnitDyeingProgram oKnitDyeingProgram = new KnitDyeingProgram();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
                    IDataReader reader = KnitDyeingProgramDA.GetLog(tc, LogId);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oKnitDyeingProgram = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get KnitDyeingProgram", e);
					#endregion
				}
				return oKnitDyeingProgram;
			}
			public List<KnitDyeingProgram> Gets (string sSQL, Int64 nUserID)
			{
				List<KnitDyeingProgram> oKnitDyeingPrograms = new List<KnitDyeingProgram>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = KnitDyeingProgramDA.Gets(tc, sSQL);
					oKnitDyeingPrograms = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get KnitDyeingProgram", e);
					#endregion
				}
				return oKnitDyeingPrograms;
			}

		#endregion
	}

}
