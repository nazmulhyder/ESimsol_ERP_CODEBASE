using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class SalesCommissionService : MarshalByRefObject, ISalesCommissionService
    {
    
        #region Private functions and declaration
        private static SalesCommission MapObject(NullHandler oReader)
        {
            SalesCommission oSalesCommission = new SalesCommission();
            oSalesCommission.SalesCommissionID = oReader.GetInt32("SalesCommissionID");
            oSalesCommission.ExportPIID = oReader.GetInt32("ExportPIID");
            //oSalesCommission.BUID = oReader.GetInt32("BUID");
            //oSalesCommission.Status = (EnumPIPaymentType)oReader.GetInt32("Status");
            oSalesCommission.Status =(EnumLSalesCommissionStatus) oReader.GetInt16("Status");
            oSalesCommission.CPName = oReader.GetString("CPName");
            oSalesCommission.ContractorName = oReader.GetString("ContractorName");
            oSalesCommission.ContractorID = oReader.GetInt32("ContractorID");
            oSalesCommission.CurrencyID = oReader.GetInt32("CurrencyID");
            oSalesCommission.CommissionAmount = oReader.GetDouble("CommissionAmount");
            oSalesCommission.ContactPersonnelID = oReader.GetInt32("ContactPersonnelID");
            oSalesCommission.Percentage = oReader.GetDouble("Percentage");
            oSalesCommission.Activity = oReader.GetBoolean("Activity");
            oSalesCommission.CommissionOn = oReader.GetInt16("CommissionOn");
            oSalesCommission.ContractNo = oReader.GetString("ContractNo");
            //oSalesCommission.ContractorEmail = oReader.GetString("ContractorEmail");
            //oSalesCommission.MKTPName = oReader.GetString("MKTPName");
            oSalesCommission.Currency = oReader.GetString("Currency");
            oSalesCommission.Percentage_Maturity = oReader.GetDouble("Percentage_Maturity");
            oSalesCommission.ApproveByName = oReader.GetString("ApproveByName");
            oSalesCommission.RequestedByName = oReader.GetString("RequestedByName");
 
            
            return oSalesCommission;

         
        }

        public static SalesCommission CreateObject(NullHandler oReader)
        {
            SalesCommission oSalesCommission = new SalesCommission();
            oSalesCommission = MapObject(oReader);            
            return oSalesCommission;
        }

        private List<SalesCommission> CreateObjects(IDataReader oReader)
        {
            List<SalesCommission> oSalesCommissions = new List<SalesCommission>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SalesCommission oItem = CreateObject(oHandler);
                oSalesCommissions.Add(oItem);
            }
            return oSalesCommissions;
        }

        #endregion

        #region Interface implementation
        public SalesCommissionService() { }        
        public SalesCommission Get(int id, Int64 nUserId)
        {
            SalesCommission oSalesCommission = new SalesCommission();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = SalesCommissionDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSalesCommission = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get Export PI", e);
                #endregion
            }

            return oSalesCommission;
        }
    
        public SalesCommission Save(SalesCommission oSalesCommission, Int64 nUserID)
        {
            TransactionContext tc = null;
        
            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oSalesCommission.SalesCommissionID <= 0)
                {
                    reader = SalesCommissionDA.InsertUpdate(tc, oSalesCommission, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = SalesCommissionDA.InsertUpdate(tc, oSalesCommission, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSalesCommission = new SalesCommission();
                    oSalesCommission = CreateObject(oReader);
                }
                reader.Close();

                //#region Terms & Condition Part
                //if (oSalesCommissionDetails != null)
                //{
                //    foreach (SalesCommissionDetail oItem in oSalesCommissionDetails)
                //    {
                //        if (oItem.Qty > 0)
                //        {
                //            IDataReader readertnc;
                //            oItem.SalesCommissionID = oSalesCommission.SalesCommissionID;
                //            if (oItem.SalesCommissionDetailID <= 0)
                //            {
                //                readertnc = SalesCommissionDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                //            }
                //            else
                //            {
                //                readertnc = SalesCommissionDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                //            }
                //            NullHandler oReaderTNC = new NullHandler(readertnc);

                //            if (readertnc.Read())
                //            {
                //                sSalesCommissionDetaillIDs = sSalesCommissionDetaillIDs + oReaderTNC.GetString("SalesCommissionDetailID") + ",";
                //            }
                //            readertnc.Close();
                //        }
                //    }

              
                //}
                //#endregion
                                
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to . Because of " + e.Message, e);
                oSalesCommission = new SalesCommission();
                oSalesCommission.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oSalesCommission;
        }

        public List<SalesCommission> SaveAll(List<SalesCommission> oSalesCommissions, Int64 nUserID)
        {

            SalesCommission oSalesCommission = new SalesCommission();
            List<SalesCommission> oSalesCommissions_Return = new List<SalesCommission>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                foreach (SalesCommission oItem in oSalesCommissions)
                {
                    if (oItem.SalesCommissionID <= 0)
                    {
                        
                        reader = SalesCommissionDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                      
                        reader = SalesCommissionDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                    }
                    //IDataReader reader = SalesCommissionDA.Get(tc, oItem.SalesCommissionID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oSalesCommission = new SalesCommission();
                        oSalesCommission = CreateObject(oReader);
                        oSalesCommissions_Return.Add(oSalesCommission);
                    }
                    reader.Close();
                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oSalesCommissions_Return = new List<SalesCommission>();
                oSalesCommission = new SalesCommission();
                oSalesCommission.ErrorMessage = e.Message.Split('~')[0];
                oSalesCommissions_Return.Add(oSalesCommission);

                #endregion
            }
            return oSalesCommissions_Return;
        }
        public string Delete(SalesCommission oSalesCommission, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                SalesCommissionDA.Delete(tc, oSalesCommission, EnumDBOperation.Delete, nUserID);                
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
            return Global.DeleteMessage;
        }
 
        public SalesCommission Approved(SalesCommission oSalesCommission, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = SalesCommissionDA.InsertUpdate(tc, oSalesCommission, EnumDBOperation.Approval, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSalesCommission = new SalesCommission();
                    oSalesCommission = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oSalesCommission = new SalesCommission();
                oSalesCommission.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oSalesCommission;
        }

        public List<SalesCommission> ApproveAll(List<SalesCommission> oSalesCommissions, Int64 nUserID)
        {

            SalesCommission oSalesCommission = new SalesCommission();
            List<SalesCommission> oSalesCommissions_Return = new List<SalesCommission>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                foreach (SalesCommission oItem in oSalesCommissions)
                {
                    if (oItem.SalesCommissionID >0)
                    {

                        reader = SalesCommissionDA.InsertUpdate(tc, oItem, EnumDBOperation.Approval, nUserID);
                        NullHandler oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oSalesCommission = new SalesCommission();
                            oSalesCommission = CreateObject(oReader);
                            oSalesCommissions_Return.Add(oSalesCommission);
                        }
                        reader.Close();
                    }
                    
                   
                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oSalesCommission = new SalesCommission();
                oSalesCommission.ErrorMessage = e.Message.Split('~')[0];
                oSalesCommissions_Return.Add(oSalesCommission);

                #endregion
            }
            return oSalesCommissions_Return;
        }

        public List<SalesCommission> RequestedAll(List<SalesCommission> oSalesCommissions, Int64 nUserID)
        {

            SalesCommission oSalesCommission = new SalesCommission();
            List<SalesCommission> oSalesCommissions_Return = new List<SalesCommission>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                foreach (SalesCommission oItem in oSalesCommissions)
                {

                    if (oItem.SalesCommissionID > 0)
                    {
                        reader = SalesCommissionDA.InsertUpdate(tc, oItem, EnumDBOperation.Request, nUserID);
                        NullHandler oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oSalesCommission = new SalesCommission();
                            oSalesCommission = CreateObject(oReader);
                            oSalesCommissions_Return.Add(oSalesCommission);
                        }
                        reader.Close();
                    }
                   
                        
              
                    
                  


                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oSalesCommission = new SalesCommission();
                oSalesCommission.ErrorMessage = e.Message.Split('~')[0];
                oSalesCommissions_Return.Add(oSalesCommission);

                #endregion
            }
            return oSalesCommissions_Return;
        }

 
        public List<SalesCommission> Gets(int nExportPIID,Int64 nUserId)
        {
            List<SalesCommission> oSalesCommissions = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SalesCommissionDA.Gets(tc, nExportPIID);
                oSalesCommissions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PI", e);
                #endregion
            }

            return oSalesCommissions;
        }
        public List<SalesCommission> Gets(string sSQL, Int64 nUserId)
        {
            List<SalesCommission> oSalesCommissions = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SalesCommissionDA.Gets(tc, sSQL);
                oSalesCommissions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PI", e);
                #endregion
            }

            return oSalesCommissions;
        }
     
     
     
        #endregion
    }
}
