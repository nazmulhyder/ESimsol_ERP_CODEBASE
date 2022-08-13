using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Linq;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class LabDipService : MarshalByRefObject, ILabDipService
    {
        #region Private functions and declaration
        private LabDip MapObject(NullHandler oReader)
        {
            LabDip oLabDip = new LabDip();
            oLabDip.LabDipID = oReader.GetInt32("LabDipID");
            oLabDip.LabdipNo = oReader.GetString("LabdipNo");
            oLabDip.ContractorID = oReader.GetInt32("ContractorID");
            oLabDip.ContactPersonnelID = oReader.GetInt32("ContactPersonnelID");
            oLabDip.RelabCount = oReader.GetInt32("RelabCount");
            oLabDip.DeliveryToID = oReader.GetInt32("DeliveryToID");
            oLabDip.DeliveryToContactPersonnelID = oReader.GetInt32("DeliveryToContactPersonnelID");
            oLabDip.DeliveryZoneID = oReader.GetInt16("DeliveryZoneID");
            oLabDip.RelabOn = oReader.GetInt32("RelabOn");
            oLabDip.PH = oReader.GetString("PH");
            oLabDip.LightSourceID = oReader.GetInt16("LightSourceID");
            oLabDip.BuyerRefNo = oReader.GetString("BuyerRefNo");
            oLabDip.LDTwistType = (EnumLDTwistType)oReader.GetInt16("LDTwistType");
            oLabDip.PriorityLevel = (EnumPriorityLevel)oReader.GetInt16("PriorityLevel");
            oLabDip.Note = oReader.GetString("Note");
            oLabDip.OrderStatus = (EnumLabdipOrderStatus)oReader.GetInt16("OrderStatus");
            oLabDip.LabDipFormat = (EnumLabdipFormat)oReader.GetInt16("LabDipFormat");
            oLabDip.OrderReferenceType = (EnumOrderType)oReader.GetInt16("OrderReferenceType");
            oLabDip.OrderReferenceID = oReader.GetInt32("OrderReferenceID");
            oLabDip.SeekingDate = oReader.GetDateTime("SeekingDate");
            oLabDip.OrderDate = oReader.GetDateTime("OrderDate");
            oLabDip.DBUserID = oReader.GetInt32("DBUserID");
            oLabDip.MktPersonID = oReader.GetInt32("MktPersonID");
            oLabDip.ISTwisted = oReader.GetBoolean("ISTwisted");
            oLabDip.ContractorName = oReader.GetString("ContractorName");
            oLabDip.DeliveryToName = oReader.GetString("DeliveryToName");
            oLabDip.MktPerson = oReader.GetString("MktPerson");
            oLabDip.LightSourceName = oReader.GetString("LightSourceName");
            oLabDip.ContractorCPName = oReader.GetString("ContractorCPName");
            oLabDip.DeliveryToCPName = oReader.GetString("DeliveryToCPName");
            oLabDip.ChallanNo = oReader.GetString("ChallanNo");
            oLabDip.FabricID = oReader.GetInt32("FabricID");
            oLabDip.OrderReferenceTypeSt = oReader.GetString("OrderReferenceTypeSt");
            oLabDip.OrderNo = oReader.GetString("OrderNo");
            oLabDip.FabricNo = oReader.GetString("FabricNo");
            oLabDip.NoOfColor = oReader.GetInt32("NoOfColor");
            oLabDip.FSCID = oReader.GetInt32("FSCID");
            oLabDip.FSCDetailID = oReader.GetInt32("FSCDetailID");
            oLabDip.ActualConstruction = oReader.GetString("ActualConstruction");
            oLabDip.Construction = oReader.GetString("Construction");
            oLabDip.ProductName = oReader.GetString("ProductName");
            oLabDip.SCNo = oReader.GetString("SCNo");
            oLabDip.OrderType = oReader.GetInt32("OrderType");
            oLabDip.NoOfColor = oReader.GetInt32("NoOfColor");
            oLabDip.LabStatus = oReader.GetInt32("LabStatus");
            oLabDip.IsInHouse = oReader.GetBoolean("IsInHouse");
            oLabDip.DeliveryNote = oReader.GetString("DeliveryNote");
            oLabDip.ProcessTypeName = oReader.GetString("ProcessTypeName");
            oLabDip.FabricWeaveName = oReader.GetString("FabricWeaveName");
            oLabDip.FinishTypeName = oReader.GetString("FinishTypeName");
            oLabDip.ExeNo = oReader.GetString("ExeNo");
            
            return oLabDip;
        }
        // LightSource, ProcessTypeName, FabricWeaveName, FinishTypeName
       
        public static LabDip CreateObject(NullHandler oReader)
        {
            LabDip oLabDip = new LabDip();
            LabDipService oService = new LabDipService();
            oLabDip = oService.MapObject(oReader);
            return oLabDip;
        }
        private List<LabDip> CreateObjects(IDataReader oReader)
        {
            List<LabDip> oLabDips = new List<LabDip>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                LabDip oItem = CreateObject(oHandler);
                oLabDips.Add(oItem);
            }
            return oLabDips;
        }
        #endregion

        #region Interface implementation
        public LabDipService() { }
        public LabDip IUD(LabDip oLabDip, int nDBOperation, int nUserID)
        {
            TransactionContext tc = null;
            List<LabDipDetail> oLabDipDetails = new List<LabDipDetail>();
            oLabDipDetails = oLabDip.LabDipDetails;
            string sLabDipDetailFabricIDS = "";
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = LabDipDA.IUD(tc, oLabDip, nDBOperation, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLabDip = new LabDip();
                    oLabDip = CreateObject(oReader);
                }
                reader.Close();

                #region Labdip Detail
                if (oLabDip.LabDipID > 0 && nDBOperation == (int)EnumDBOperation.Insert)
                {
                    oLabDipDetails = oLabDipDetails.Where(x => x.LabDipDetailID <= 0).ToList();
                    if (oLabDipDetails.Any())
                    {
                        List<LabDipDetail> oLDDs = new List<LabDipDetail>();
                        foreach (LabDipDetail oItem in oLabDipDetails)
                        {
                            oItem.LabDipID = oLabDip.LabDipID;
                            reader = LabDipDetailDA.IUD(tc, oItem, nDBOperation, nUserID);
                            oReader = new NullHandler(reader);
                            if (reader.Read())
                            {
                                var oLabdipDetail = LabDipDetailService.CreateObject(oReader);
                                oLDDs.Add(oLabdipDetail);
                            }
                            reader.Close();
                        }
                        oLabDip.LabDipDetails = oLDDs;
                    }
                }
                #endregion

                #region LabDipDetailFabric Part

                //int _oLDDetailID = 0;

                //if (oLabDipDetailFabrics != null && oLabDipDetailFabrics.Any())
                //{
                //    List<LabDipDetailFabric> oLDDFs = new List<LabDipDetailFabric>();
                //    foreach (LabDipDetailFabric oItem in oLabDipDetailFabrics)
                //    {
                //        _oLDDetailID = oItem.LabDipDetailID;

                //        IDataReader readerdetail;
                //        oItem.LabDipID = oLabDip.LabDipID;
                //        if (oItem.LDFID <= 0)
                //        {
                //            readerdetail = LabDipDetailFabricDA.IUD(tc, oItem, (int)EnumDBOperation.Insert, nUserID, "");
                //        }
                //        else
                //        {
                //            readerdetail = LabDipDetailFabricDA.IUD(tc, oItem, (int)EnumDBOperation.Update, nUserID, "");
                //        }

                //        NullHandler oReaderDetail = new NullHandler(readerdetail);
                //        if (readerdetail.Read())
                //        {
                //            LabDipDetailFabric oLabdipDetailF = new LabDipDetailFabric();
                //            oLabdipDetailF = LabDipDetailFabricService.CreateObject(oReaderDetail);
                //            oLDDFs.Add(oLabdipDetailF);
                //            sLabDipDetailFabricIDS = sLabDipDetailFabricIDS + oLabdipDetailF.LDFID+ ",";
                //        }
                //        readerdetail.Close();
                //    }
                //    oLabDip.LabDipDetailFabrics = oLDDFs;

                //    LabDipDetailFabric oLabDipDetailFabric = new LabDipDetailFabric();
                //    oLabDipDetailFabric.LabDipID = oLabDip.LabDipID;
                //    oLabDipDetailFabric.LabDipDetailID = _oLDDetailID;
                //    if (sLabDipDetailFabricIDS.Length > 0)
                //    {
                //        sLabDipDetailFabricIDS = sLabDipDetailFabricIDS.Remove(sLabDipDetailFabricIDS.Length - 1, 1);
                //    }
                //    reader = LabDipDetailFabricDA.IUD(tc, oLabDipDetailFabric, (int)EnumDBOperation.Delete, nUserID, sLabDipDetailFabricIDS);
                //    reader.Close();
                //}
                #endregion
                tc.End();
                if (nDBOperation == (int)EnumDBOperation.Delete) { oLabDip = new LabDip(); oLabDip.ErrorMessage = Global.DeleteMessage; }
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oLabDip.ErrorMessage= (ex.Message.Contains("~"))?ex.Message.Split('~')[0]:ex.Message;
                #endregion
            }
            return oLabDip;
        }
        public LabDip IUD_Fabric(LabDip oLabDip, int nDBOperation, int nUserID) 
        {
            List<LabDipDetailFabric> oLabDipDetailFabrics = new List<LabDipDetailFabric>();
            oLabDipDetailFabrics = oLabDip.LabDipDetailFabrics;

            TransactionContext tc = null;
            string sLabDipDetailFabricIDS = "";
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;

                #region LabDipDetailFabric Part

                int _oLDDetailID = oLabDip.LabDipID;

                
                if (oLabDipDetailFabrics != null && oLabDipDetailFabrics.Any())
                {
                    List<LabDipDetailFabric> oLDDFs = new List<LabDipDetailFabric>();
                    foreach (LabDipDetailFabric oItem in oLabDipDetailFabrics)
                    {
                        _oLDDetailID = oItem.LabDipDetailID;

                        IDataReader readerdetail;
                        oItem.LabDipID = oLabDip.LabDipID;
                        if (oItem.LDFID <= 0)
                        {
                            readerdetail = LabDipDetailFabricDA.IUD(tc, oItem, (int)EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = LabDipDetailFabricDA.IUD(tc, oItem, (int)EnumDBOperation.Update, nUserID, "");
                        }

                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            LabDipDetailFabric oLabdipDetailF = new LabDipDetailFabric();
                            oLabdipDetailF = LabDipDetailFabricService.CreateObject(oReaderDetail);
                            oLDDFs.Add(oLabdipDetailF);
                            sLabDipDetailFabricIDS = sLabDipDetailFabricIDS + oLabdipDetailF.LDFID + ",";
                        }
                        readerdetail.Close();
                    }
                    oLabDip.LabDipDetailFabrics = oLDDFs;

                    LabDipDetailFabric oLabDipDetailFabric = new LabDipDetailFabric();
                    oLabDipDetailFabric.LabDipID = oLabDip.LabDipID;
                    oLabDipDetailFabric.LabDipDetailID = _oLDDetailID;

                    if (sLabDipDetailFabricIDS.Length > 0)
                    {
                        sLabDipDetailFabricIDS = sLabDipDetailFabricIDS.Remove(sLabDipDetailFabricIDS.Length - 1, 1);
                    }
                    reader = LabDipDetailFabricDA.IUD(tc, oLabDipDetailFabric, (int)EnumDBOperation.Delete, nUserID, sLabDipDetailFabricIDS);
                    reader.Close();
                    tc.End();
                }
                else
                {
                    LabDipDetailFabric oLabDipDetailFabric = new LabDipDetailFabric();
                    oLabDipDetailFabric.LabDipID = oLabDip.LabDipID;
                    oLabDipDetailFabric.LabDipDetailID = oLabDip.LabDipDetails[0].LabDipDetailID;

                    if (sLabDipDetailFabricIDS.Length > 0)
                    {
                        sLabDipDetailFabricIDS = sLabDipDetailFabricIDS.Remove(sLabDipDetailFabricIDS.Length - 1, 1);
                    }
                    reader = LabDipDetailFabricDA.IUD(tc, oLabDipDetailFabric, (int)EnumDBOperation.Delete, nUserID, sLabDipDetailFabricIDS);
                    reader.Close();
                    tc.End();
                }

                #endregion
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oLabDip.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }
            return oLabDip;
        }
        public LabDip Get(int nLabDipID, int nUserID)
        {
            LabDip oLabDip = new LabDip();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = LabDipDA.Get(tc, nLabDipID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLabDip = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();

                oLabDip.ErrorMessage = e.Message;
                #endregion
            }

            return oLabDip;
        }
        public LabDip GetByFSD(int nFSCDetailID, int nUserID)
        {
            LabDip oLabDip = new LabDip();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = LabDipDA.GetByFSD(tc, nFSCDetailID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLabDip = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();

                oLabDip.ErrorMessage = e.Message;
                #endregion
            }

            return oLabDip;
        }
        public LabDip GetByFSDMap(string nFSCDetailIDs, int nUserID)
        {
            LabDip oLabDip = new LabDip();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = LabDipDA.GetByFSDMap(tc, nFSCDetailIDs);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLabDip = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();

                oLabDip.ErrorMessage = e.Message;
                #endregion
            }

            return oLabDip;
        }
        public List<LabDip> Gets(string sSQL, int nUserID)
        {
            List<LabDip> oLabDips = new List<LabDip>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LabDipDA.Gets(tc, sSQL);
                oLabDips = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                LabDip oLabDip = new LabDip();
                oLabDip.ErrorMessage = e.Message;
                oLabDips.Add(oLabDip);
                #endregion
            }

            return oLabDips;
        }
        public List<LabDip> Gets( int nUserID)
        {
            List<LabDip> oLabDips = new List<LabDip>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LabDipDA.Gets(tc);
                oLabDips = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                LabDip oLabDip = new LabDip();
                oLabDip.ErrorMessage = e.Message;
                oLabDips.Add(oLabDip);
                #endregion
            }

            return oLabDips;
        }
        public LabDip ChangeOrderStatus(LabDip oLabDip, short nNextStatus, int nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                int nLabdipDetailItem = LabDipDetailDA.LabDipDetailCount(tc, oLabDip.LabDipID);

                if (nLabdipDetailItem <= 0)
                    throw new Exception("No labdip detail item found. Please add item first.");

                if (oLabDip.OrderStatus == EnumLabdipOrderStatus.None)
                    throw new Exception("Invalid labdip status.");

                if (oLabDip.OrderStatus == EnumLabdipOrderStatus.Initialized && nNextStatus != (short)EnumLabdipOrderStatus.WaitForApprove)
                    throw new Exception("Reqired to send a request for approval .");

                if (oLabDip.OrderStatus == EnumLabdipOrderStatus.WaitForApprove && nNextStatus != (short)EnumLabdipOrderStatus.Initialized && nNextStatus != (short)EnumLabdipOrderStatus.Approve)
                    throw new Exception("Reqired to approve or undo.");

                if (oLabDip.OrderStatus == EnumLabdipOrderStatus.Approve && nNextStatus != (short)EnumLabdipOrderStatus.InLab && nNextStatus != (short)EnumLabdipOrderStatus.Cancel)
                    throw new Exception("Reqired to send in lab or cancel this request.");

                if (oLabDip.OrderStatus == EnumLabdipOrderStatus.InLab && nNextStatus != (short)EnumLabdipOrderStatus.LabdipDone && nNextStatus != (short)EnumLabdipOrderStatus.WaitForApprove)
                    throw new Exception("Reqired to send a request for Labdip done Or Undo.");

                if (oLabDip.OrderStatus == EnumLabdipOrderStatus.LabdipDone && nNextStatus != (short)EnumLabdipOrderStatus.WaitingForReceiveFromLab)
                    throw new Exception("Reqired to send a request for receive from lab.");

                if ((oLabDip.OrderStatus == EnumLabdipOrderStatus.WaitingForReceiveFromLab) && nNextStatus != (short)EnumLabdipOrderStatus.LabdipInHand)
                    throw new Exception("Reqired to send labdip at sales department.");

                if (oLabDip.OrderStatus == EnumLabdipOrderStatus.LabdipInHand && nNextStatus != (short)EnumLabdipOrderStatus.LabdipInBuyerHand)
                    throw new Exception("Reqired to the confirmation of buyer approval.");

                if (oLabDip.OrderStatus == EnumLabdipOrderStatus.LabdipInBuyerHand && nNextStatus != (short)EnumLabdipOrderStatus.BuyerApproval)
                    throw new Exception("Reqired a request, send to buyer");


                IDataReader reader = LabDipDA.ChangeOrderStatus(tc, oLabDip.LabDipID, (int)oLabDip.OrderStatus, nNextStatus, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLabDip = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();

                oLabDip.ErrorMessage = (e.Message.Contains("~")) ? e.Message.Split('~')[0] : e.Message;
                #endregion
            }

            return oLabDip;
        }
        public LabDip DirectApproval(LabDip oLabDip, short nNextStatus, int nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                int nLabdipDetailItem = LabDipDetailDA.LabDipDetailCount(tc, oLabDip.LabDipID);

                if (nLabdipDetailItem <= 0)
                    throw new Exception("No labdip detail item found. Please add item first.");

                if (oLabDip.OrderStatus == EnumLabdipOrderStatus.None)
                    throw new Exception("Invalid labdip status.");

                
                //if (oLabDip.OrderStatus == EnumLabdipOrderStatus.Approve && nNextStatus != (short)EnumLabdipOrderStatus.InLab && nNextStatus != (short)EnumLabdipOrderStatus.Cancel)
                //    throw new Exception("Reqired to send in lab or cancel this request.");

                //if (oLabDip.OrderStatus == EnumLabdipOrderStatus.InLab && nNextStatus != (short)EnumLabdipOrderStatus.LabdipDone && nNextStatus != (short)EnumLabdipOrderStatus.WaitForApprove)
                //    throw new Exception("Reqired to send a request for Labdip done Or Undo.");

                if (oLabDip.OrderStatus == EnumLabdipOrderStatus.LabdipDone && nNextStatus != (short)EnumLabdipOrderStatus.WaitingForReceiveFromLab)
                    throw new Exception("Reqired to send a request for receive from lab.");

                if ((oLabDip.OrderStatus == EnumLabdipOrderStatus.WaitingForReceiveFromLab) && nNextStatus != (short)EnumLabdipOrderStatus.LabdipInHand)
                    throw new Exception("Reqired to send labdip at sales department.");

                if (oLabDip.OrderStatus == EnumLabdipOrderStatus.LabdipInHand && nNextStatus != (short)EnumLabdipOrderStatus.LabdipInBuyerHand)
                    throw new Exception("Reqired to the confirmation of buyer approval.");

                if (oLabDip.OrderStatus == EnumLabdipOrderStatus.LabdipInBuyerHand && nNextStatus != (short)EnumLabdipOrderStatus.BuyerApproval)
                    throw new Exception("Reqired a request, send to buyer");


                IDataReader reader = LabDipDA.ChangeOrderStatus(tc, oLabDip.LabDipID, (int)oLabDip.OrderStatus, nNextStatus, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLabDip = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();

                oLabDip.ErrorMessage = (e.Message.Contains("~")) ? e.Message.Split('~')[0] : e.Message;
                #endregion
            }

            return oLabDip;
        }
        public LabDip Relab(int nLabDipID, int nUserID)
        {
            LabDip oLabDip = new LabDip();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = LabDipDA.Relab(tc, nLabDipID, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLabDip = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();

                oLabDip.ErrorMessage = (e.Message.Contains("~")) ? e.Message.Split('~')[0] : e.Message;
                #endregion
            }

            return oLabDip;
        }
        public LabDip CreateLabdipByDO(int nDyeingOrderID, int nUserID)
        {
            LabDip oLabDip = new LabDip();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                IDataReader reader = LabDipDA.CreateLabdipByDO(tc, nDyeingOrderID, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLabDip = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oLabDip = new LabDip();
                oLabDip.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }

            return oLabDip;
        }
        public LabDip Save_Challan(LabDip oLabDip, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader;
                reader = LabDipDA.IUD_LD_Challan(tc, oLabDip, (int)EnumDBOperation.Update, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLabDip = CreateObject(oReader);
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
                oLabDip.ErrorMessage = (e.Message.Contains("~")) ? e.Message.Split('~')[0] : e.Message;
                #endregion
            }

            return oLabDip;
        }
        public LabDip IUD_LD_Fabric(LabDip oLabDip, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader;
                reader = LabDipDA.IUD_LD_Fabric(tc, oLabDip,  nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLabDip = CreateObject(oReader);
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
                oLabDip.ErrorMessage = (e.Message.Contains("~")) ? e.Message.Split('~')[0] : e.Message;
                #endregion
            }

            return oLabDip;
        }

        #endregion
    }
}