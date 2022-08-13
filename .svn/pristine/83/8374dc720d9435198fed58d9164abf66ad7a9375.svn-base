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
    public class LotService : MarshalByRefObject, ILotService
    {
        #region Private functions and declaration
        public static Lot MapObject(NullHandler oReader)
        {
            Lot oLot = new Lot();
            oLot.LotID = oReader.GetInt32("LotID");
            oLot.ProductID = oReader.GetInt32("ProductID");            
            oLot.LotNo = oReader.GetString("LotNo");
            oLot.LogNo = oReader.GetString("LogNo");
            oLot.Balance = oReader.GetDouble("Balance");
            oLot.MUnitID = oReader.GetInt32("MUnitID");
            oLot.UnitPrice = oReader.GetDouble("UnitPrice");
            oLot.CurrencyID = oReader.GetInt32("CurrencyID");
            oLot.ParentLotID = oReader.GetInt32("ParentLotID");
            oLot.ProductBaseID = oReader.GetInt32("ProductBaseID");
            oLot.ParentType = (EnumTriggerParentsType)oReader.GetInt32("ParentType");
            oLot.ParentID = oReader.GetInt32("ParentID");
            oLot.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oLot.ModelReferenceID = oReader.GetInt32("ModelReferenceID");
            oLot.StyleID = oReader.GetInt32("StyleID");
            oLot.ColorID = oReader.GetInt32("ColorID");
            oLot.SizeID = oReader.GetInt32("SizeID");
            oLot.ProductCode = oReader.GetString("ProductCode");
            oLot.ProductName = oReader.GetString("ProductName");
            oLot.OperationUnitName = oReader.GetString("OperationUnitName");
            oLot.MUName = oReader.GetString("MUName");
            oLot.LocationName = oReader.GetString("LocationName");
            oLot.BUID = oReader.GetInt32("BUID");
            oLot.UnitType = (EnumWoringUnitType)oReader.GetInt32("UnitType");
            oLot.UnitTypeInInt = oReader.GetInt32("UnitType");
            oLot.StyleNo = oReader.GetString("StyleNo");
            oLot.BuyerName = oReader.GetString("BuyerName");
            oLot.ColorName = oReader.GetString("ColorName");
            oLot.SizeName = oReader.GetString("SizeName");
            oLot.ModelReferenceName = oReader.GetString("ModelReferenceName");
            oLot.Currency = oReader.GetString("Currency");
            oLot.ReportingBalance = oReader.GetDouble("ReportingBalance");
            oLot.ReportUnitSymbol = oReader.GetString("ReportUnitSymbol");
            oLot.Origin = oReader.GetString("Origin");
            oLot.ContractorID = oReader.GetInt32("ContractorID");
            oLot.ContractorName = oReader.GetString("ContractorName");
            oLot.SupplierName = oReader.GetString("SupplierName");
            oLot.AgingDays = oReader.GetInt32("AgingDays");
            oLot.LastDate = oReader.GetDateTime("LastDate");
            oLot.LotStatus = (EnumLotStatus)oReader.GetInt32("LotStatus");
            oLot.WeightPerCartoon = oReader.GetDouble("WeightPerCartoon");
            oLot.ConePerCartoon = oReader.GetDouble("ConePerCartoon");
            oLot.FinishDia = oReader.GetString("FinishDia");
            oLot.MCDia = oReader.GetString("MCDia");
            oLot.GSM = oReader.GetString("GSM");
            oLot.OrderRecapID = oReader.GetInt32("OrderRecapID");
            oLot.OrderRecapNo = oReader.GetString("OrderRecapNo");
            oLot.StoreName = oReader.GetString("StoreName");
            oLot.TotalQuantity = oReader.GetInt32("TotalQuantity");
            oLot.AlreadyShipmentQty = oReader.GetInt32("AlreadyShipmentQty");
            oLot.YetToShipmentQty = oReader.GetInt32("YetToShipmentQty");
            oLot.LocationShortName = oReader.GetString("LocationShortName");
            oLot.OUShortName = oReader.GetString("OUShortName");
            oLot.FCUnitPrice = oReader.GetDouble("FCUnitPrice");
            oLot.FCSymbol = oReader.GetString("FCSymbol");
            oLot.FCCurrencyID = oReader.GetInt32("FCCurrencyID");
            oLot.Shade = oReader.GetString("Shade");
            oLot.Stretch_Length = oReader.GetString("Stretch_Length");
            oLot.RackID = oReader.GetInt32("RackID");
            oLot.RackNo = oReader.GetString("RackNo");
            oLot.ShelfID = oReader.GetInt32("ShelfID");
            oLot.ShelfName = oReader.GetString("ShelfName");
            oLot.SupplierShortName = oReader.GetString("SupplierShortName");
            oLot.ProductCategoryID = oReader.GetInt32("ProductCategoryID");
            oLot.ProductCategoryName = oReader.GetString("ProductCategoryName");
            oLot.ProductGroupName = oReader.GetString("ProductGroupName");
            oLot.YetQty = oReader.GetDouble("YetQty");

            return oLot;
        }

        public static  Lot CreateObject(NullHandler oReader)
        {
            Lot oLot = new Lot();
            oLot = MapObject(oReader);
            return oLot;
        }

        private List<Lot> CreateObjects(IDataReader oReader)
        {
            List<Lot> oLots = new List<Lot>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                Lot oItem = CreateObject(oHandler);
                oLots.Add(oItem);
            }
            return oLots;
        }

        #endregion

        #region Interface implementation
        public LotService() { }
        public Lot UploadLot(Lot oLot, int nUserID)
        {
            TransactionContext tc = null;
           
            try
            {
                tc = TransactionContext.Begin(true);
                #region Lot
                IDataReader reader;
                if (oLot.LotID <= 0)
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "Lot", EnumRoleOperationType.Add);
                    reader = LotDA.InsertUpdateAdjLot(tc, oLot, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "Lot", EnumRoleOperationType.Edit);
                    reader = LotDA.InsertUpdateAdjLot(tc, oLot, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLot = new Lot();
                    oLot = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                tc.End();
            }

            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oLot.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oLot;
        }
        public Lot Save(Lot oLot, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
               
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oLot.LotID <= 0)
                {
                    reader = LotDA.InsertUpdate(tc, oLot, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = LotDA.InsertUpdate(tc, oLot, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLot = new Lot();
                    oLot = CreateObject(oReader);
                }
                reader.Close();

                

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to . Because of " + e.Message, e);
                oLot = new Lot();
                oLot.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oLot;
        }

        public Lot UpdateRack(Lot oLot, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
               
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oLot.LotID > 0 && oLot.RackID > 0)
                {
                    reader = LotDA.UpdateRack(tc, oLot, EnumDBOperation.Update, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oLot = new Lot();
                        oLot = CreateObject(oReader);
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

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to . Because of " + e.Message, e);
                oLot = new Lot();
                oLot.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oLot;
        }
        
        public Lot UpdateLotPrice(Lot oLot, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader = LotDA.UpdateLotPrice(tc, oLot, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLot = new Lot();
                    oLot = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oLot = new Lot();
                oLot.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oLot;
        }



        



        public Lot Get(int eParentType, int nParentID, int nWorkingUnitID, int nProductID, Int64 nUserID)
        {
            Lot oLot = new Lot();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = LotDA.Get(tc, eParentType, nParentID, nWorkingUnitID, nProductID, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLot = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get Lot", e);
                #endregion
            }

            return oLot;
        }
        public Lot Get(int nLotID, Int64 nUserID)
        {
            Lot oLot = new Lot();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = LotDA.Get(tc, nLotID, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLot = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get Lot", e);
                #endregion
            }

            return oLot;
        }
        public Lot GetByLotNo(string sLotNo, int nBUID, int nStoreId, Int64 nUserID)
        {
            List<Lot> oLots = new List<Lot>();

            TransactionContext tc = null;
            try
            {
                string sSQL = "SELECT TOP 1 * FROM View_Lot";
                string sSQL1 = "";

                #region BUID
                if (nBUID > 0)
                {
                    Global.TagSQL(ref sSQL1);
                    sSQL1 = sSQL1 + " BUID =" + nBUID;
                }
                else throw new Exception("Invalid Business Unit!");
                #endregion

                #region WorkingUnitID
                if (nStoreId > 0)
                {
                    Global.TagSQL(ref sSQL1);
                    sSQL1 = sSQL1 + " WorkingUnitID =" + nStoreId;
                }
                else throw new Exception("Invalid Store!");
                #endregion

                #region LotNo
                if (sLotNo == null) { sLotNo = ""; }
                if (sLotNo != "")
                {
                    Global.TagSQL(ref sSQL1);
                    sSQL1 = sSQL1 + " LotNo LIKE '%" + sLotNo + "%'";
                }
                else throw new Exception("Lot No !");
                #endregion

                sSQL += sSQL1;

                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = LotDA.Gets(tc, sSQL);
                oLots = CreateObjects(reader);
                reader.Close();
                tc.End();

            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Lot. ", e);
                #endregion
            }

            return oLots[0];
        }
        public Lot GetByProductID(int nProductID, bool bIsZeroBalance, Int64 nUserID)
        {
            Lot oLot = new Lot();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = LotDA.GetByProductID(tc, nProductID, bIsZeroBalance,nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLot = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get Lot", e);
                #endregion
            }

            return oLot;
        }
        public List<Lot> Gets(string sSQL, Int64 nUserId)
        {
            List<Lot> oLots = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LotDA.Gets(tc,sSQL);
                oLots = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Lot", e);
                #endregion
            }

            return oLots;
        }
        public List<Lot> GetsBy(string sProductIds, string sWorkingUnitID, Int64 nUserId)
        {
            List<Lot> oLots = new List<Lot>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);

                IDataReader reader = null;
                reader = LotDA.Gets(tc,  sProductIds,  sWorkingUnitID);
                oLots = CreateObjects(reader);
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
                #endregion
            }

            return oLots;
        }
        public List<Lot> GetsZeroBalance(string sProductIds, string sWorkingUnitID, Int64 nUserId)
        {
            List<Lot> oLots = new List<Lot>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);

                IDataReader reader = null;
                reader = LotDA.GetsZeroBalance(tc, sProductIds, sWorkingUnitID);
                oLots = CreateObjects(reader);
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
                #endregion
            }

            return oLots;
        }

        public DataSet GetsDataSet(string sSQL, Int64 nUserID)
        {
            TransactionContext tc = null;
            DataSet oDataSet = new DataSet();
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = LotDA.GetsDataSet(tc, sSQL);
                oDataSet.Load(reader, LoadOption.OverwriteChanges, new string[1]);
                reader.Close();
                tc.End();

            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DataSet", e);
                #endregion
            }
            return oDataSet;
        }
        public Lot CommitIsRunning(Lot oLot, Int64 nUserId)
        {
            Lot oAccountHead = new Lot();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                LotDA.CommitIsRunning(tc, oLot, nUserId);
                IDataReader reader = LotDA.Get(tc, oLot.LotID, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get Lot", e);
                #endregion
            }

            return oAccountHead;
        }

        public string UpdateStatus(Lot oLot, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                LotDA.UpdateStatus(tc, oLot, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message;
                #endregion
            }

            return "Success";
        }
        #endregion
    }
}
