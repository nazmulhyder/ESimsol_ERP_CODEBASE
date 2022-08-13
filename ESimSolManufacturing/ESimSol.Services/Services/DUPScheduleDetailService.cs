using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    [Serializable]
    public class DUPScheduleDetailService : MarshalByRefObject, IDUPScheduleDetailService
    {
        #region Private functions and declaration
        private DUPScheduleDetail MapObject(NullHandler oReader)
        {
            DUPScheduleDetail oDUPScheduleDetail = new DUPScheduleDetail();
            oDUPScheduleDetail.DUPScheduleDetailID = oReader.GetInt32("DUPScheduleDetailID");
            oDUPScheduleDetail.DUPScheduleID = oReader.GetInt32("DUPScheduleID");
            oDUPScheduleDetail.DODID = oReader.GetInt32("DODID");
            oDUPScheduleDetail.PTUID = oReader.GetInt32("PTUID");
            oDUPScheduleDetail.ProductID = oReader.GetInt32("ProductID");
            oDUPScheduleDetail.DyeingOrderID = oReader.GetInt32("DyeingOrderID");
            oDUPScheduleDetail.Qty = oReader.GetDouble("Qty");
            oDUPScheduleDetail.PSBatchNo = oReader.GetString("PSBatchNo");
            oDUPScheduleDetail.OrderLast = oReader.GetString("OrderLast");
            oDUPScheduleDetail.Remarks = oReader.GetString("Remarks");
            oDUPScheduleDetail.DBUserID = oReader.GetInt32("DBUserID");
            oDUPScheduleDetail.LocationName = oReader.GetString("LocationName");
            oDUPScheduleDetail.MachineName = oReader.GetString("MachineName");
            oDUPScheduleDetail.StartTime = oReader.GetDateTime("StartTime");
            oDUPScheduleDetail.EndTime = oReader.GetDateTime("EndTime");
            oDUPScheduleDetail.OrderNo = oReader.GetString("OrderNo");
            oDUPScheduleDetail.ProductName = oReader.GetString("ProductName");
            oDUPScheduleDetail.ContractorName = oReader.GetString("ContractorName");
            oDUPScheduleDetail.ColorNo = oReader.GetString("ColorNo");
            oDUPScheduleDetail.ColorName = oReader.GetString("ColorName");
            oDUPScheduleDetail.OrderQty = oReader.GetDouble("OrderQty");
            oDUPScheduleDetail.BuyerName = oReader.GetString("BuyerName");
            oDUPScheduleDetail.PantonNo = oReader.GetString("PantonNo");
            oDUPScheduleDetail.DeliveryDate = oReader.GetDateTime("DeliveryDate");
            oDUPScheduleDetail.StartTime = oReader.GetDateTime("StartTime");
            oDUPScheduleDetail.EndTime = oReader.GetDateTime("EndTime");
            oDUPScheduleDetail.ScheduleNo = oReader.GetString("ScheduleNo");
            oDUPScheduleDetail.Qty = oReader.GetDouble("Qty");
            oDUPScheduleDetail.QtyPerBag = oReader.GetDouble("QtyPerBag");
            oDUPScheduleDetail.BagCount = oReader.GetDouble("BagCount");
            oDUPScheduleDetail.HankorCone =oReader.GetInt16("HankorCone");
            oDUPScheduleDetail.ContractorID = oReader.GetInt16("ContractorID");
            oDUPScheduleDetail.WorkingUnitID_Lot = oReader.GetInt16("WorkingUnitID_Lot");

            oDUPScheduleDetail.LabDipDetailID = oReader.GetInt32("LabDipDetailID");
            oDUPScheduleDetail.MachineID = oReader.GetInt32("MachineID");
            //oDUPScheduleDetail.SLMax = oReader.GetInt32("SLMax");
            //oDUPScheduleDetail.SLNo = oReader.GetInt32("SL");
            if (!string.IsNullOrEmpty(oDUPScheduleDetail.OrderLast ))
            {
                oDUPScheduleDetail.PSBatchNo = oDUPScheduleDetail.PSBatchNo + "/" + oDUPScheduleDetail.OrderLast;
            }
            //else   if (oDUPScheduleDetail.SLMax == oDUPScheduleDetail.SLNo && oDUPScheduleDetail.SLMax>1) { oDUPScheduleDetail.PSBatchNo = oDUPScheduleDetail.PSBatchNo + "/L"; }
            if (oDUPScheduleDetail.OrderQty <= oDUPScheduleDetail.Qty+0.3)
            {
                oDUPScheduleDetail.PSBatchNo = oDUPScheduleDetail.PSBatchNo + "/1";
            }
            oDUPScheduleDetail.RouteSheetNo = oReader.GetString("RouteSheetNo");
            oDUPScheduleDetail.RouteSheetID = oReader.GetInt32("RouteSheetID");
            oDUPScheduleDetail.RSState = (EnumRSState)oReader.GetInt16("RSState");
            //oDUPScheduleDetail.RouteSheetID = oReader.GetInt32("RouteSheetID");
            oDUPScheduleDetail.UsesWeight = oReader.GetString("UsesWeight");
            //oDUPScheduleDetail.RedyingForRSNo = oReader.GetString("RedyingForRSNo");
            oDUPScheduleDetail.IsInHouse = oReader.GetBoolean("IsInHouse");
            oDUPScheduleDetail.IsRequistion = oReader.GetBoolean("IsRequistion");
            oDUPScheduleDetail.BuyerRef = oReader.GetString("BuyerRef");
            oDUPScheduleDetail.ApproveLotNo = oReader.GetString("ApproveLotNo");
            oDUPScheduleDetail.HankorConeST = oReader.GetString("HankorConeST");
            oDUPScheduleDetail.DyeLoadNote = oReader.GetString("DyeLoadNote");
            oDUPScheduleDetail.DyeUnloadNote = oReader.GetString("DyeUnloadNote");

            return oDUPScheduleDetail;
        }

        private DUPScheduleDetail CreateObject(NullHandler oReader)
        {
            DUPScheduleDetail oDUPScheduleDetail = new DUPScheduleDetail();
            oDUPScheduleDetail = MapObject(oReader);
            return oDUPScheduleDetail;
        }

        private List<DUPScheduleDetail> CreateObjects(IDataReader oReader)
        {
            List<DUPScheduleDetail> oDUPScheduleDetails = new List<DUPScheduleDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DUPScheduleDetail oItem = CreateObject(oHandler);
                oDUPScheduleDetails.Add(oItem);
            }
            return oDUPScheduleDetails;
        }
        #endregion

        #region Interface implementation
        public DUPScheduleDetailService() { }



        public List<DUPScheduleDetail> Gets(int id,Int64 nUserID)
        {
            List<DUPScheduleDetail> oDUPScheduleDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUPScheduleDetailDA.Gets(tc,id);
                oDUPScheduleDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Get DUPScheduleDetails", e);
                #endregion
            }

            return oDUPScheduleDetails;
        }
    
        public List<DUPScheduleDetail> Gets(string sPSIDs,Int64 nUserID)
        {
            List<DUPScheduleDetail> oDUPScheduleDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUPScheduleDetailDA.Gets(tc, sPSIDs);
                oDUPScheduleDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Get DUPScheduleDetails", e);
                #endregion
            }

            return oDUPScheduleDetails;
        }
        public List<DUPScheduleDetail> GetsSqL(string sSQL, Int64 nUserID)
        {
            List<DUPScheduleDetail> oDUPScheduleDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUPScheduleDetailDA.GetsSqL(tc, sSQL);
                oDUPScheduleDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUPScheduleDetails because of "+e.Message.ToString());
                #endregion
            }

            return oDUPScheduleDetails;
        }
        public string Delete(DUPScheduleDetail oDUPScheduleDetail, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                DUPScheduleDetailDA.Delete(tc, oDUPScheduleDetail, EnumDBOperation.Delete, nUserId);
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
        public List<DUPScheduleDetail> Swap(List<DUPSchedule> oDUPSchedules, Int64 nUserID)
        {

            DUPScheduleDetail oDUPScheduleDetail = new DUPScheduleDetail();
            List<DUPScheduleDetail> oDUPScheduleDetails_Return = new List<DUPScheduleDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                foreach (DUPSchedule oItem in oDUPSchedules)
                {
                    IDataReader reader = DUPScheduleDetailDA.Swap(tc, oItem, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oDUPScheduleDetail = new DUPScheduleDetail();
                        oDUPScheduleDetail = CreateObject(oReader);
                        oDUPScheduleDetails_Return.Add(oDUPScheduleDetail);
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
                oDUPScheduleDetails_Return = new List<DUPScheduleDetail>();
                oDUPScheduleDetail = new DUPScheduleDetail();
                oDUPScheduleDetail.ErrorMessage = e.Message.Split('~')[0];
                oDUPScheduleDetails_Return.Add(oDUPScheduleDetail);

                #endregion
            }
            return oDUPScheduleDetails_Return;
        }

        public List<DUPScheduleDetail> Update_Requisition(DUPScheduleDetail oDUPScheduleDetail, Int64 nUserId)
        {
            List<DUPScheduleDetail> oDUPScheduleDetails = null;

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUPScheduleDetailDA.Update_Requisition(tc, oDUPScheduleDetail);
                oDUPScheduleDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Get DUPScheduleDetails", e);
                #endregion
            }

            return oDUPScheduleDetails;
        }
        public List<DUPScheduleDetail> Gets_RS(string sSQL, Int64 nUserID)
        {
            List<DUPScheduleDetail> oDUPScheduleDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUPScheduleDetailDA.Gets_RS(tc, sSQL);
                oDUPScheduleDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUPScheduleDetails because of " + e.Message.ToString());
                #endregion
            }

            return oDUPScheduleDetails;
        }
        #endregion

    }
}

