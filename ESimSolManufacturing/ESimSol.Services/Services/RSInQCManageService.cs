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
    public class RSInQCManageService : MarshalByRefObject, IRSInQCManageService
    {
        #region Private functions and declaration
        private RSInQCManage MapObject(NullHandler oReader)
        {
            RSInQCManage oRSInQCManage = new RSInQCManage();
            oRSInQCManage.RSInQCDetailID = oReader.GetInt32("RSInQCDetailID");
            oRSInQCManage.RouteSheetID = oReader.GetInt32("RouteSheetID");
            oRSInQCManage.RSInQCSetupID = oReader.GetInt32("RSInQCSetupID");
            oRSInQCManage.ManageDate = oReader.GetDateTime("ManageDate");
            oRSInQCManage.ProductID = oReader.GetInt32("ProductID");
            oRSInQCManage.MUnitID = oReader.GetInt32("MUnitID");
            oRSInQCManage.ManagedLotID = oReader.GetInt32("ManagedLotID");
         
            oRSInQCManage.Qty = oReader.GetDouble("Qty");
            oRSInQCManage.QtyRS = oReader.GetDouble("QtyRS");
            oRSInQCManage.Note = oReader.GetString("Note");
            oRSInQCManage.QCSetupName = oReader.GetString("QCSetupName");
            oRSInQCManage.YarnType = (EnumYarnType)oReader.GetInt16("YarnType");
            oRSInQCManage.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oRSInQCManage.ManagedLotID = oReader.GetInt32("ManagedLotID");
            oRSInQCManage.RouteSheetNo = oReader.GetString("RouteSheetNo");
            oRSInQCManage.ProductName = oReader.GetString("ProductName");
            oRSInQCManage.ProductCode = oReader.GetString("ProductCode");
            oRSInQCManage.OrderType = oReader.GetInt32("OrderType");
            oRSInQCManage.OrderNo = oReader.GetString("OrderNo");
            oRSInQCManage.OrderTypeSt = oReader.GetString("OrderTypeSt");
            oRSInQCManage.DyeingType = oReader.GetString("DyeingType");
            oRSInQCManage.NoCode = oReader.GetString("NoCode");
            oRSInQCManage.WUName = oReader.GetString("WUName");
            return oRSInQCManage;
        }

        private RSInQCManage CreateObject(NullHandler oReader)
        {
            RSInQCManage oRSInQCManage = MapObject(oReader);
            return oRSInQCManage;
        }

        private List<RSInQCManage> CreateObjects(IDataReader oReader)
        {
            List<RSInQCManage> oRSInQCManage = new List<RSInQCManage>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RSInQCManage oItem = CreateObject(oHandler);
                oRSInQCManage.Add(oItem);
            }
            return oRSInQCManage;
        }

        #endregion

        #region Interface implementation
        public RSInQCManageService() { }


        public List<RSInQCManage> IUD(List<RSInQCManage> oRSInQCManages, Int64 nUserID)
        {

            RSInQCManage oRSInQCManage = new RSInQCManage();
            List<RSInQCManage> oRSInQCManages_Return = new List<RSInQCManage>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                foreach (RSInQCManage oItem in oRSInQCManages)
                {
                    IDataReader readerdetail;
                    readerdetail = RSInQCManageDA.IUD(tc, oItem, nUserID, (int)EnumDBOperation.Insert);

                    NullHandler oReader = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        oRSInQCManage = new RSInQCManage();
                        oRSInQCManage = CreateObject(oReader);
                        oRSInQCManages_Return.Add(oRSInQCManage);
                    }
                    readerdetail.Close();
                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oRSInQCManage = new RSInQCManage();
                oRSInQCManage.ErrorMessage = e.Message.Split('~')[0];
                oRSInQCManages_Return.Add(oRSInQCManage);

                #endregion
            }
            return oRSInQCManages_Return;
        }

        public RSInQCManage Get(int nRSInQCManageID, Int64 nUserId)
        {
            RSInQCManage oRSInQCManage = new RSInQCManage();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = RSInQCManageDA.Get(nRSInQCManageID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRSInQCManage = CreateObject(oReader);
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
                oRSInQCManage.ErrorMessage = e.Message;
                #endregion
            }

            return oRSInQCManage;
        }
        public List<RSInQCManage> Gets(string sSQL, Int64 nUserID)
        {
            List<RSInQCManage> oRSInQCManages = new List<RSInQCManage>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RSInQCManageDA.Gets(sSQL, tc);
                oRSInQCManages = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                RSInQCManage oRSInQCManage = new RSInQCManage();
                oRSInQCManage.ErrorMessage = e.Message;
                oRSInQCManages = new List<RSInQCManage>();
                oRSInQCManages.Add(oRSInQCManage);
                #endregion
            }
            return oRSInQCManages;
        }

        #endregion
    }
}
