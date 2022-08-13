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
    public class DUDeliveryChallanPackService : MarshalByRefObject, IDUDeliveryChallanPackService
    {
        #region Private functions and declaration

        private DUDeliveryChallanPack MapObject(NullHandler oReader)
        {
            DUDeliveryChallanPack oDUDeliveryChallanPack = new DUDeliveryChallanPack();
            oDUDeliveryChallanPack.DUDeliveryChallanPackID = oReader.GetInt32("DUDeliveryChallanPackID");
            oDUDeliveryChallanPack.DUDeliveryChallanDetailID = oReader.GetInt32("DUDeliveryChallanDetailID");
            oDUDeliveryChallanPack.DUDeliveryChallanID = oReader.GetInt32("DUDeliveryChallanID");
            oDUDeliveryChallanPack.RouteSheetPackingID = oReader.GetInt32("RouteSheetPackingID");
            oDUDeliveryChallanPack.QTY = oReader.GetDouble("QTY");
            oDUDeliveryChallanPack.BagWeight = oReader.GetDouble("BagWeight");
            oDUDeliveryChallanPack.LotNo = oReader.GetString("LotNo");
            oDUDeliveryChallanPack.LogNo = oReader.GetString("LogNo");
            oDUDeliveryChallanPack.Balance = oReader.GetDouble("Balance");
            oDUDeliveryChallanPack.LotUnitPrice = oReader.GetDouble("LotUnitPrice");
            oDUDeliveryChallanPack.LotMUnitID = oReader.GetInt32("LotMUnitID");
            oDUDeliveryChallanPack.ColorName = oReader.GetString("ColorName");
            oDUDeliveryChallanPack.Shade = oReader.GetInt32("Shade");
            oDUDeliveryChallanPack.MUnit = oReader.GetString("MUnit");
            oDUDeliveryChallanPack.BagNo = oReader.GetInt32("BagNo");
            oDUDeliveryChallanPack.PackingWeight = oReader.GetDouble("PackingWeight");
            return oDUDeliveryChallanPack;
        }

        private DUDeliveryChallanPack CreateObject(NullHandler oReader)
        {
            DUDeliveryChallanPack oDUDeliveryChallanPack = new DUDeliveryChallanPack();
            oDUDeliveryChallanPack = MapObject(oReader);
            return oDUDeliveryChallanPack;
        }

        private List<DUDeliveryChallanPack> CreateObjects(IDataReader oReader)
        {
            List<DUDeliveryChallanPack> oDUDeliveryChallanPack = new List<DUDeliveryChallanPack>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DUDeliveryChallanPack oItem = CreateObject(oHandler);
                oDUDeliveryChallanPack.Add(oItem);
            }
            return oDUDeliveryChallanPack;
        }

        #endregion

        #region Interface implementation
        public DUDeliveryChallanPack Save(DUDeliveryChallanPack oDUDeliveryChallanPack, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oDUDeliveryChallanPack.DUDeliveryChallanPackID <= 0)
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "DUDeliveryChallanPack", EnumRoleOperationType.Add);
                    reader = DUDeliveryChallanPackDA.InsertUpdate(tc, oDUDeliveryChallanPack, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "DUDeliveryChallanPack", EnumRoleOperationType.Edit);
                    reader = DUDeliveryChallanPackDA.InsertUpdate(tc, oDUDeliveryChallanPack, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUDeliveryChallanPack = new DUDeliveryChallanPack();
                    oDUDeliveryChallanPack = CreateObject(oReader);
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
                    oDUDeliveryChallanPack = new DUDeliveryChallanPack();
                    oDUDeliveryChallanPack.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oDUDeliveryChallanPack;
        }

        public List<DUDeliveryChallanPack> SavePackingDetails(List<DUDeliveryChallanPack> oDUDeliveryChallanPacks, Int64 nUserID)
        {
            DUDeliveryChallanPack oDUDeliveryChallanPack = new DUDeliveryChallanPack();
            List<DUDeliveryChallanPack> oDUDCPs = new List<DUDeliveryChallanPack>();
            double nTotalQty = 0;
            double nTotalBagQty = 0;
            string sPackIDs = "";
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                foreach (DUDeliveryChallanPack oCHP in oDUDeliveryChallanPacks)
                {
                    IDataReader reader;
                    if (oCHP.DUDeliveryChallanPackID <= 0)
                    {
                        //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "DUDeliveryChallanPack", EnumRoleOperationType.Add);
                        reader = DUDeliveryChallanPackDA.InsertUpdate(tc, oCHP, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "DUDeliveryChallanPack", EnumRoleOperationType.Edit);
                        reader = DUDeliveryChallanPackDA.InsertUpdate(tc, oCHP, EnumDBOperation.Update, nUserID);
                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oDUDeliveryChallanPack = new DUDeliveryChallanPack();
                        oDUDeliveryChallanPack = CreateObject(oReader);
                        oDUDCPs.Add(oDUDeliveryChallanPack);
                        nTotalQty += oDUDeliveryChallanPack.QTY;
                        nTotalBagQty += oDUDeliveryChallanPack.BagWeight;
                        sPackIDs += oDUDeliveryChallanPack.DUDeliveryChallanPackID + ",";
                    }
                    reader.Close();
                }
                if (sPackIDs.Length > 0) sPackIDs = sPackIDs.Substring(0, sPackIDs.Length - 1);
                DUDeliveryChallanPackDA.DeletePacks(tc, oDUDCPs[0].DUDeliveryChallanDetailID, sPackIDs);

                DUDeliveryChallanDetail oDUDeliveryChallanDetail = new DUDeliveryChallanDetail();
                DUDeliveryChallanDetailDA.UpdateQty(tc, nTotalQty, nTotalBagQty, oDUDCPs[0].DUDeliveryChallanDetailID);
                
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oDUDeliveryChallanPack = new DUDeliveryChallanPack();
                    oDUDeliveryChallanPack.ErrorMessage = e.Message.Split('!')[0];
                    oDUDCPs.Add(oDUDeliveryChallanPack);
                }
                #endregion
            }
            return oDUDCPs;
        }

        public string Delete(DUDeliveryChallanPack oDUDeliveryChallanPack, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "DUDeliveryChallanPack", EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "DUDeliveryChallanPack", oDUDeliveryChallanPack.DUDeliveryChallanPackID);
                DUDeliveryChallanPackDA.Delete(tc, oDUDeliveryChallanPack, EnumDBOperation.Delete, nUserId);
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

        public DUDeliveryChallanPack Get(int id, Int64 nUserId)
        {
            DUDeliveryChallanPack oDUDeliveryChallanPack = new DUDeliveryChallanPack();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = DUDeliveryChallanPackDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUDeliveryChallanPack = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get DUDeliveryChallanPack", e);
                #endregion
            }
            return oDUDeliveryChallanPack;
        }

        public List<DUDeliveryChallanPack> Gets(Int64 nUserID)
        {
            List<DUDeliveryChallanPack> oDUDeliveryChallanPacks = new List<DUDeliveryChallanPack>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = DUDeliveryChallanPackDA.Gets(tc);
                oDUDeliveryChallanPacks = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                DUDeliveryChallanPack oDUDeliveryChallanPack = new DUDeliveryChallanPack();
                oDUDeliveryChallanPack.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oDUDeliveryChallanPacks;
        }

        public List<DUDeliveryChallanPack> Gets(string sSQL, Int64 nUserID)
        {
            List<DUDeliveryChallanPack> oDUDeliveryChallanPacks = new List<DUDeliveryChallanPack>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = DUDeliveryChallanPackDA.Gets(tc, sSQL);
                oDUDeliveryChallanPacks = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUDeliveryChallanPack", e);
                #endregion
            }
            return oDUDeliveryChallanPacks;
        }

        #endregion
    }

}
