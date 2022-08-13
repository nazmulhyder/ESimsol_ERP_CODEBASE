using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class RSInQCDetailService : MarshalByRefObject, IRSInQCDetailService
    {
        #region Private functions and declaration
        private RSInQCDetail MapObject(NullHandler oReader)
        {
            RSInQCDetail oRSInQCDetail = new RSInQCDetail();
            oRSInQCDetail.RSInQCDetailID = oReader.GetInt32("RSInQCDetailID");
            oRSInQCDetail.RouteSheetID = oReader.GetInt32("RouteSheetID");
            oRSInQCDetail.RSInQCSetupID = oReader.GetInt32("RSInQCSetupID");
            oRSInQCDetail.ManageDate = oReader.GetDateTime("ManageDate");
            oRSInQCDetail.ProductID = oReader.GetInt32("ProductID");
            oRSInQCDetail.MUnitID = oReader.GetInt32("MUnitID");
            oRSInQCDetail.ManagedLotID = oReader.GetInt32("ManagedLotID");
            oRSInQCDetail.Qty = oReader.GetDouble("Qty");
            oRSInQCDetail.Note = oReader.GetString("Note");
            oRSInQCDetail.QCSetupName = oReader.GetString("QCSetupName");
            oRSInQCDetail.YarnType = (EnumYarnType)oReader.GetInt16("YarnType");
            oRSInQCDetail.DyeingOrderDetailID = oReader.GetInt32("DyeingOrderDetailID");
            return oRSInQCDetail;
        }

        private RSInQCDetail CreateObject(NullHandler oReader)
        {
            RSInQCDetail oRSInQCDetail = MapObject(oReader);
            return oRSInQCDetail;
        }

        private List<RSInQCDetail> CreateObjects(IDataReader oReader)
        {
            List<RSInQCDetail> oRSInQCDetail = new List<RSInQCDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RSInQCDetail oItem = CreateObject(oHandler);
                oRSInQCDetail.Add(oItem);
            }
            return oRSInQCDetail;
        }

        #endregion

        #region Interface implementation
        public RSInQCDetailService() { }

        public RSInQCDetail IUD(RSInQCDetail oRSInQCDetail, int nDBOperation, Int64 nUserID)
        {

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = RSInQCDetailDA.IUD(tc, oRSInQCDetail, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRSInQCDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
                if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    oRSInQCDetail = new RSInQCDetail();
                    oRSInQCDetail.ErrorMessage = Global.DeleteMessage;
                }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oRSInQCDetail = new RSInQCDetail(); 
                oRSInQCDetail.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oRSInQCDetail;
        }
        public List<RSInQCDetail> SaveAll(List<RSInQCDetail> oRSInQCDetails, Int64 nUserID)
        {
            RSInQCDetail oRSInQCDetail = new RSInQCDetail();
            List<RSInQCDetail> oRSInQCDetails_Return = new List<RSInQCDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                foreach (RSInQCDetail oItem in oRSInQCDetails)
                {
                    if (oItem.RouteSheetID > 0) 
                    {
                        if (oItem.RSInQCDetailID <= 0)
                        {
                            reader = RSInQCDetailDA.IUD(tc, oItem, nUserID, (int)EnumDBOperation.Insert);
                        }
                        else
                        {
                            reader = RSInQCDetailDA.IUD(tc, oItem, nUserID, (int)EnumDBOperation.Update);
                        }
                        NullHandler oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oRSInQCDetail = new RSInQCDetail();
                            oRSInQCDetail = CreateObject(oReader);
                            oRSInQCDetails_Return.Add(oRSInQCDetail);
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
                oRSInQCDetails_Return = new List<RSInQCDetail>();
                oRSInQCDetail = new RSInQCDetail();
                oRSInQCDetail.ErrorMessage = e.Message.Split('~')[0];
                oRSInQCDetails_Return.Add(oRSInQCDetail);

                #endregion
            }
            return oRSInQCDetails_Return;
        }
        public RSInQCDetail Get(int nRSInQCDetailID, Int64 nUserId)
        {
            RSInQCDetail oRSInQCDetail = new RSInQCDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = RSInQCDetailDA.Get(nRSInQCDetailID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRSInQCDetail = CreateObject(oReader);
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
                oRSInQCDetail.ErrorMessage = e.Message;
                #endregion
            }

            return oRSInQCDetail;
        }
        public List<RSInQCDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<RSInQCDetail> oRSInQCDetails = new List<RSInQCDetail>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RSInQCDetailDA.Gets(sSQL, tc);
                oRSInQCDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                RSInQCDetail oRSInQCDetail = new RSInQCDetail();
                oRSInQCDetail.ErrorMessage = e.Message;
                oRSInQCDetails = new List<RSInQCDetail>();
                oRSInQCDetails.Add(oRSInQCDetail);
                #endregion
            }
            return oRSInQCDetails;
        }

        #endregion
    }
}
