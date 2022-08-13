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
    public class ShipmentService : MarshalByRefObject, IShipmentService
    {
        #region Private functions and declaration

        private Shipment MapObject(NullHandler oReader)
        {
            Shipment oShipment = new Shipment();
            oShipment.ShipmentID = oReader.GetInt32("ShipmentID");
            oShipment.BUID = oReader.GetInt32("BUID");
            oShipment.BuyerID = oReader.GetInt32("BuyerID");
            oShipment.StoreID = oReader.GetInt32("StoreID");
            oShipment.ChallanNo = oReader.GetString("ChallanNo");
            oShipment.ShipmentDate = oReader.GetDateTime("ShipmentDate");
            oShipment.ShipmentMode = (EnumShipmentMode)oReader.GetInt32("ShipmentMode");
            oShipment.Remarks = oReader.GetString("Remarks");
            oShipment.TruckNo = oReader.GetString("TruckNo");
            oShipment.DriverName = oReader.GetString("DriverName");
            oShipment.DriverMobileNo = oReader.GetString("DriverMobileNo");
            oShipment.Depo = oReader.GetString("Depo");
            oShipment.Escord = oReader.GetString("Escord");
            oShipment.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oShipment.FactoryName = oReader.GetString("FactoryName");
            oShipment.SecurityLock = oReader.GetString("SecurityLock");
            oShipment.EmptyCTNQty = oReader.GetInt32("EmptyCTNQty");
            oShipment.GumTapeQty = oReader.GetInt32("GumTapeQty");

            oShipment.BuyerName = oReader.GetString("BuyerName");
            oShipment.ApproveByName = oReader.GetString("ApprovedByName");
            oShipment.StoreName = oReader.GetString("StoreName");

            return oShipment;
        }

        private Shipment CreateObject(NullHandler oReader)
        {
            Shipment oShipment = new Shipment();
            oShipment = MapObject(oReader);
            return oShipment;
        }

        private List<Shipment> CreateObjects(IDataReader oReader)
        {
            List<Shipment> oShipment = new List<Shipment>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                Shipment oItem = CreateObject(oHandler);
                oShipment.Add(oItem);
            }
            return oShipment;
        }

        #endregion

        #region Interface implementation
        public Shipment Save(Shipment oShipment, Int64 nUserID)
        {
            ShipmentDetail oShipmentDetail = new ShipmentDetail();
            Shipment oSM = new Shipment();
            oSM = oShipment;

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                #region Shipment
                IDataReader reader;
                if (oShipment.ShipmentID <= 0)
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.Shipment, EnumRoleOperationType.Add);
                    reader = ShipmentDA.InsertUpdate(tc, oShipment, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.Shipment, EnumRoleOperationType.Edit);
                    reader = ShipmentDA.InsertUpdate(tc, oShipment, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oShipment = new Shipment();
                    oShipment = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region ShipmentDetail

                if (oShipment.ShipmentID > 0)
                {
                    string sShipmentDetailIDs = "";
                    if (oSM.ShipmentDetails.Count > 0)
                    {
                        IDataReader readerdetail;
                        foreach (ShipmentDetail oSMD in oSM.ShipmentDetails)
                        {
                            oSMD.ShipmentID = oShipment.ShipmentID;
                            if (oSMD.ShipmentDetailID <= 0)
                            {
                                readerdetail = ShipmentDetailDA.InsertUpdate(tc, oSMD, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readerdetail = ShipmentDetailDA.InsertUpdate(tc, oSMD, EnumDBOperation.Update, nUserID, "");

                            }
                            NullHandler oReaderDevRecapdetail = new NullHandler(readerdetail);
                            int nShipmentDetailID = 0;
                            if (readerdetail.Read())
                            {
                                nShipmentDetailID = oReaderDevRecapdetail.GetInt32("ShipmentDetailID");
                                sShipmentDetailIDs = sShipmentDetailIDs + oReaderDevRecapdetail.GetString("ShipmentDetailID") + ",";
                            }
                            readerdetail.Close();
                        }
                    }
                    if (sShipmentDetailIDs.Length > 0)
                    {
                        sShipmentDetailIDs = sShipmentDetailIDs.Remove(sShipmentDetailIDs.Length - 1, 1);
                    }
                    oShipmentDetail = new ShipmentDetail();
                    oShipmentDetail.ShipmentID = oShipment.ShipmentID;
                    ShipmentDetailDA.Delete(tc, oShipmentDetail, EnumDBOperation.Delete, nUserID, sShipmentDetailIDs);
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
                    oShipment = new Shipment();
                    oShipment.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oShipment;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                Shipment oShipment = new Shipment();
                oShipment.ShipmentID = id;
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.Shipment, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "Shipment", id);
                ShipmentDA.Delete(tc, oShipment, EnumDBOperation.Delete, nUserId);
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

        public Shipment Approve(Shipment oShipment, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                #region Shipment
                IDataReader reader = null;
                if (oShipment.ShipmentID > 0)
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.Shipment, EnumRoleOperationType.Add);
                    reader = ShipmentDA.Approve(tc, oShipment, nUserID);
                }

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oShipment = new Shipment();
                    oShipment = CreateObject(oReader);
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
                    oShipment = new Shipment();
                    oShipment.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oShipment;
        }

        public Shipment Get(int id, Int64 nUserId)
        {
            Shipment oShipment = new Shipment();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = ShipmentDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oShipment = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get Shipment", e);
                #endregion
            }
            return oShipment;
        }

        public List<Shipment> Gets(Int64 nUserID)
        {
            List<Shipment> oShipments = new List<Shipment>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ShipmentDA.Gets(tc);
                oShipments = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                Shipment oShipment = new Shipment();
                oShipment.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oShipments;
        }

        public List<Shipment> Gets(string sSQL, Int64 nUserID)
        {
            List<Shipment> oShipments = new List<Shipment>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ShipmentDA.Gets(tc, sSQL);
                oShipments = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Shipment", e);
                #endregion
            }
            return oShipments;
        }

        #endregion
    }

}
