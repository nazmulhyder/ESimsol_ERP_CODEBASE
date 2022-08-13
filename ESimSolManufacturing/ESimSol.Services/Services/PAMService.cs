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
	public class PAMService : MarshalByRefObject, IPAMService
	{
		#region Private functions and declaration

		private PAM MapObject(NullHandler oReader)
		{
			PAM oPAM = new PAM();
			oPAM.PAMID = oReader.GetInt32("PAMID");
			oPAM.PAMNo = oReader.GetString("PAMNo");
			oPAM.StyleID = oReader.GetInt32("StyleID");
			oPAM.ForwardWeek = oReader.GetString("ForwardWeek");
			oPAM.Remarks = oReader.GetString("Remarks");
			oPAM.StyleNo = oReader.GetString("StyleNo");
			oPAM.BuyerName = oReader.GetString("BuyerName");
            oPAM.SessionName = oReader.GetString("SessionName");
			oPAM.ProductName = oReader.GetString("ProductName");
			oPAM.FabricName = oReader.GetString("FabricName");
            oPAM.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oPAM.ApprovedByName = oReader.GetString("ApprovedByName");
            oPAM.ApprovedDate = oReader.GetDateTime("ApprovedDate");
            oPAM.TotalQuantity = oReader.GetDouble("TotalQuantity");
            oPAM.UnitSymbol = oReader.GetString("UnitSymbol");
            oPAM.YetToRecapQty = oReader.GetDouble("YetToRecapQty");
            oPAM.MeasurementUnitID = oReader.GetInt32("MeasurementUnitID");
			return oPAM;
		}

		private PAM CreateObject(NullHandler oReader)
		{
			PAM oPAM = new PAM();
			oPAM = MapObject(oReader);
			return oPAM;
		}

		private List<PAM> CreateObjects(IDataReader oReader)
		{
			List<PAM> oPAM = new List<PAM>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				PAM oItem = CreateObject(oHandler);
				oPAM.Add(oItem);
			}
			return oPAM;
		}

		#endregion

		#region Interface implementation
			public PAM Save(PAM oPAM, Int64 nUserID)
			{
				TransactionContext tc = null;
                List<PAMDetail> oPAMDetails = new List<PAMDetail>();
                oPAMDetails = oPAM.PAMDetailLst;
                string sDetailIDs = "";
				try
				{
					tc = TransactionContext.Begin(true);
					IDataReader reader;
					if (oPAM.PAMID <= 0)
					{
						AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.PAM, EnumRoleOperationType.Add);
						reader = PAMDA.InsertUpdate(tc, oPAM, EnumDBOperation.Insert, nUserID);
					}
					else{
                        AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.PAM, EnumRoleOperationType.Edit);
						reader = PAMDA.InsertUpdate(tc, oPAM, EnumDBOperation.Update, nUserID);
					}
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
						oPAM = new PAM();
						oPAM = CreateObject(oReader);
					}
					reader.Close();

                    #region PAM Detail
                    if (oPAMDetails.Count > 0)
                    {
                        foreach (PAMDetail oItem in oPAMDetails)
                        {
                            IDataReader readerdetail;
                            oItem.PAMID = oPAM.PAMID;
                            if (oItem.PAMDetailID <= 0)
                            {
                                readerdetail = PAMDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readerdetail = PAMDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                            }
                            NullHandler oReaderDetail = new NullHandler(readerdetail);
                            if (readerdetail.Read())
                            {
                                sDetailIDs = sDetailIDs + oReaderDetail.GetString("PAMDetailID") + ",";
                            }
                            readerdetail.Close();
                        }
                        if (sDetailIDs.Length > 0)
                        {
                            sDetailIDs = sDetailIDs.Remove(sDetailIDs.Length - 1, 1);
                        }
                        PAMDetail oPAMDetail = new PAMDetail();
                        oPAMDetail.PAMID = oPAM.PAMID;
                        PAMDetailDA.Delete(tc, oPAMDetail, EnumDBOperation.Delete, nUserID, sDetailIDs);

                    }
                    #endregion

                    #region Get PAM
                    reader = PAMDA.Get(tc, oPAM.PAMID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oPAM = CreateObject(oReader);
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
						oPAM = new PAM();
						oPAM.ErrorMessage = e.Message.Split('!')[0];
					}
					#endregion
				}
				return oPAM;
			}

            public string SaveMultiPAM(PAM oPAM, Int64 nUserID)
            {
                TransactionContext tc = null;
                string sForwardWeek = "";
                PAM oTempPAM = new PAM();
                List<PAM> oPAMs = new List<PAM>();
                PAMDetail oPAMDetail = new PAMDetail();
                List<PAMDetail> oPAMDetails = new List<PAMDetail>();                                
                try
                {
                    sForwardWeek = "";                
                    foreach (PAMDetail oItem in oPAM.PAMDetailLst)
                    {
                        if (oItem.ForwardWeek != sForwardWeek)
                        {
                            oTempPAM = new PAM();
                            oTempPAM.PAMID = 0;
                            oTempPAM.PAMNo = "";
                            oTempPAM.StyleID = oItem.StyleID;
                            oTempPAM.ForwardWeek = oItem.ForwardWeek;
                            oTempPAM.Remarks = "N/A";
                            oPAMs.Add(oTempPAM); 
                        }
                        sForwardWeek = oItem.ForwardWeek;
                    }

                    tc = TransactionContext.Begin(true);
                    foreach (PAM oItem in oPAMs)
                    {                       
                        oItem.PAMID = PAMDA.GetPAMID(tc, oItem.StyleID, oItem.ForwardWeek);
                        oPAMDetails = new List<PAMDetail>();
                        foreach (PAMDetail oDetail in oPAM.PAMDetailLst)
                        {
                            if (oItem.ForwardWeek == oDetail.ForwardWeek)
                            {
                                oPAMDetail = new PAMDetail();
                                oPAMDetail.PAMDetailID = 0;
                                oPAMDetail.PAMID = oItem.PAMID;
                                oPAMDetail.ColorID = oDetail.ColorID;
                                oPAMDetail.MinQuantity = oDetail.MinQuantity;
                                oPAMDetail.Quantity = oDetail.Quantity;
                                oPAMDetail.MaxQuantity = oDetail.MaxQuantity;
                                oPAMDetail.ConfirmWeek = oDetail.ConfirmWeek;
                                oPAMDetail.DesignationWeek = oDetail.DesignationWeek;
                                oPAMDetail.WearHouseWeek = oDetail.WearHouseWeek;
                                oPAMDetail.Remarks = oDetail.Remarks;
                                oPAMDetails.Add(oPAMDetail);
                            }
                        }
                        oItem.PAMDetailLst = oPAMDetails;
                    }


                    foreach (PAM oPAMObj in oPAMs)
                    {
                        IDataReader reader;
                        if (oPAMObj.PAMID <= 0)
                        {
                            AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.PAM, EnumRoleOperationType.Add);
                            reader = PAMDA.InsertUpdate(tc, oPAMObj, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.PAM, EnumRoleOperationType.Edit);
                            reader = PAMDA.InsertUpdate(tc, oPAMObj, EnumDBOperation.Update, nUserID);
                        }
                        NullHandler oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oTempPAM = new PAM();
                            oTempPAM = CreateObject(oReader);
                        }
                        reader.Close();

                        #region PAM Detail
                        if (oPAMObj.PAMDetailLst.Count > 0)
                        {
                            foreach (PAMDetail oItem in oPAMObj.PAMDetailLst)
                            {
                                IDataReader readerdetail;
                                oItem.PAMID = oTempPAM.PAMID;
                                readerdetail = PAMDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                                readerdetail.Close();
                            }
                        }
                        #endregion
                    }

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
            //
            public PAM Revise(PAM oPAM, Int64 nUserID)
            {
                TransactionContext tc = null;
                List<PAMDetail> oPAMDetails = new List<PAMDetail>();
                oPAMDetails = oPAM.PAMDetailLst;
                string sDetailIDs = "";
                try
                {
                    tc = TransactionContext.Begin(true);
                    IDataReader reader;
                    reader = PAMDA.AccepRevise(tc, oPAM, EnumDBOperation.Update, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oPAM = new PAM();
                        oPAM = CreateObject(oReader);
                    }
                    reader.Close();

                    #region PAM Detail
                    if (oPAMDetails.Count > 0)
                    {
                        foreach (PAMDetail oItem in oPAMDetails)
                        {
                            IDataReader readerdetail;
                            oItem.PAMID = oPAM.PAMID;
                            if (oItem.PAMDetailID <= 0)
                            {
                                readerdetail = PAMDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readerdetail = PAMDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                            }
                            NullHandler oReaderDetail = new NullHandler(readerdetail);
                            if (readerdetail.Read())
                            {
                                sDetailIDs = sDetailIDs + oReaderDetail.GetString("PAMDetailID") + ",";
                            }
                            readerdetail.Close();
                        }
                        if (sDetailIDs.Length > 0)
                        {
                            sDetailIDs = sDetailIDs.Remove(sDetailIDs.Length - 1, 1);
                        }
                        PAMDetail oPAMDetail = new PAMDetail();
                        oPAMDetail.PAMID = oPAM.PAMID;
                        PAMDetailDA.Delete(tc, oPAMDetail, EnumDBOperation.Delete, nUserID, sDetailIDs);

                    }
                    #endregion
                    #region Get PAM
                    reader = PAMDA.Get(tc, oPAM.PAMID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oPAM = CreateObject(oReader);
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
                        oPAM = new PAM();
                        oPAM.ErrorMessage = e.Message.Split('!')[0];
                    }
                    #endregion
                }
                return oPAM;
            }
            public PAM Approve(PAM oPAM, Int64 nUserID)
            {
                TransactionContext tc = null;
                List<PAMDetail> oPAMDetails = new List<PAMDetail>();
                oPAMDetails = oPAM.PAMDetailLst;
                
                try
                {
                    tc = TransactionContext.Begin(true);
                    IDataReader reader;
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.PAM, EnumRoleOperationType.Approved);
                    reader = PAMDA.InsertUpdate(tc, oPAM, EnumDBOperation.Approval, nUserID);
               
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oPAM = new PAM();
                        oPAM = CreateObject(oReader);
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
                        oPAM = new PAM();
                        oPAM.ErrorMessage = e.Message.Split('!')[0];
                    }
                    #endregion
                }
                return oPAM;
            }
			public string Delete(int id, Int64 nUserId)
			{
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin(true);
					PAM oPAM = new PAM();
					oPAM.PAMID = id;
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.PAM, EnumRoleOperationType.Delete);
					DBTableReferenceDA.HasReference(tc, "PAM", id);
					PAMDA.Delete(tc, oPAM, EnumDBOperation.Delete, nUserId);
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

			public PAM Get(int id, Int64 nUserId)
			{
				PAM oPAM = new PAM();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = PAMDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oPAM = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get PAM", e);
					#endregion
				}
				return oPAM;
			}

			public List<PAM> Gets(int nStyleID, Int64 nUserID)
			{
				List<PAM> oPAMs = new List<PAM>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = PAMDA.Gets(nStyleID, tc);
					oPAMs = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					PAM oPAM = new PAM();
					oPAM.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oPAMs;
			}

			public List<PAM> Gets (string sSQL, Int64 nUserID)
			{
				List<PAM> oPAMs = new List<PAM>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = PAMDA.Gets(tc, sSQL);
					oPAMs = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get PAM", e);
					#endregion
				}
				return oPAMs;
			}

		#endregion
	}

}
