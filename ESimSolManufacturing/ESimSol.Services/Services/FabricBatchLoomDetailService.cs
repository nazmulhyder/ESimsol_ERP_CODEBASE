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
    public class FabricBatchLoomDetailService : MarshalByRefObject, IFabricBatchLoomDetailService
    {
        #region Private functions and declaration
        public static FabricBatchLoomDetail MapObject(NullHandler oReader)
        {
            FabricBatchLoomDetail oFabricBatchLoomDetail = new FabricBatchLoomDetail();
            oFabricBatchLoomDetail.FBLDetailID = oReader.GetInt32("FBLDetailID");
            oFabricBatchLoomDetail.FabricBatchLoomID = oReader.GetInt32("FabricBatchLoomID");
            oFabricBatchLoomDetail.ShiftStartTime = oReader.GetDateTime("ShiftStartTime");
            oFabricBatchLoomDetail.ShiftEndTime = oReader.GetDateTime("ShiftEndTime");
            oFabricBatchLoomDetail.EmployeeID = oReader.GetInt32("EmployeeID");
            oFabricBatchLoomDetail.EmployeeName = oReader.GetString("EmployeeName");
            oFabricBatchLoomDetail.Note = oReader.GetString("Note");
            oFabricBatchLoomDetail.ShiftID = oReader.GetInt32("ShiftID");
            oFabricBatchLoomDetail.ShiftName = oReader.GetString("ShiftName");
            oFabricBatchLoomDetail.Qty = oReader.GetDouble("Qty");
            oFabricBatchLoomDetail.FinishDate = oReader.GetDateTime("FinishDate");
            oFabricBatchLoomDetail.TotalFBPBreakage = oReader.GetInt32("TotalFBPBreakage");
            oFabricBatchLoomDetail.TotalNoOfBreakage = oReader.GetInt32("TotalNoOfBreakage");
            oFabricBatchLoomDetail.TotalColor = oReader.GetInt32("TotalColor");
            oFabricBatchLoomDetail.Efficiency = oReader.GetInt32("Efficiency");
            oFabricBatchLoomDetail.RPM = oReader.GetInt32("RPM");
            oFabricBatchLoomDetail.TSUID = oReader.GetInt32("TSUID");
            oFabricBatchLoomDetail.BeamNo = oReader.GetString("BeamNo");
            oFabricBatchLoomDetail.ApproveBy = oReader.GetInt32("ApproveBy");
            oFabricBatchLoomDetail.ApproveName = oReader.GetString("ApproveName");
            oFabricBatchLoomDetail.ApproveDate = oReader.GetDateTime("ApproveDate");
            oFabricBatchLoomDetail.Construction = oReader.GetString("Construction");
            oFabricBatchLoomDetail.FEONo = oReader.GetString("FEONo");
            oFabricBatchLoomDetail.MachineCode = oReader.GetString("MachineCode");
            oFabricBatchLoomDetail.BuyerName = oReader.GetString("BuyerName");
            oFabricBatchLoomDetail.Color = oReader.GetString("Color");
            oFabricBatchLoomDetail.Warp = oReader.GetInt32("Warp");
            oFabricBatchLoomDetail.Weft = oReader.GetInt32("Weft");
            oFabricBatchLoomDetail.FinishType = oReader.GetString("FinishType");
            oFabricBatchLoomDetail.FabricType = oReader.GetString("FabricType");

            return oFabricBatchLoomDetail;
        }

        public static FabricBatchLoomDetail CreateObject(NullHandler oReader)
        {
            FabricBatchLoomDetail oFabricBatchLoomDetail = new FabricBatchLoomDetail();
            oFabricBatchLoomDetail = MapObject(oReader);
            return oFabricBatchLoomDetail;
        }

        public List<FabricBatchLoomDetail> CreateObjects(IDataReader oReader)
        {
            List<FabricBatchLoomDetail> oFabricBatchLoomDetail = new List<FabricBatchLoomDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricBatchLoomDetail oItem = CreateObject(oHandler);
                oFabricBatchLoomDetail.Add(oItem);
            }
            return oFabricBatchLoomDetail;
        }

        #endregion

        public List<FabricBatchLoomDetail> Gets(int nFabricBatchLoomID, Int64 nUserID)
        {
            List<FabricBatchLoomDetail> oFabricBatchLoomDetails = new List<FabricBatchLoomDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricBatchLoomDetailDA.Gets(tc, nFabricBatchLoomID);
                oFabricBatchLoomDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Fabric Yarn Out", e);
                #endregion
            }
            return oFabricBatchLoomDetails;
        }

        public List<FabricBatchLoomDetail> GetsBySql(string sSql, Int64 nUserID)
        {
            List<FabricBatchLoomDetail> oFabricBatchLoomDetails = new List<FabricBatchLoomDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricBatchLoomDetailDA.GetsBySql(tc, sSql);
                oFabricBatchLoomDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Fabric Yarn Out", e);
                #endregion
            }
            return oFabricBatchLoomDetails;
        }

        public FabricBatchLoomDetail Get(int id, Int64 nUserId)
        {
            FabricBatchLoomDetail oFabricBatchLoomDetail = new FabricBatchLoomDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricBatchLoomDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricBatchLoomDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FabricBatchLoomDetail", e);
                #endregion
            }
            return oFabricBatchLoomDetail;
        }
        public FabricBatchLoomDetail Save(FabricBatchLoomDetail oFabricBatchLoomDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {

                tc = TransactionContext.Begin(true);

                IDataReader reader;
                if (oFabricBatchLoomDetail.FBLDetailID <= 0)
                {
                    reader = FabricBatchLoomDetailDA.IUD(tc, EnumDBOperation.Insert, oFabricBatchLoomDetail, nUserID);
                }
                else
                {
                    reader = FabricBatchLoomDetailDA.IUD(tc, EnumDBOperation.Update, oFabricBatchLoomDetail, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricBatchLoomDetail = new FabricBatchLoomDetail();
                    oFabricBatchLoomDetail = CreateObject(oReader);
                }
                else
                {
                    oFabricBatchLoomDetail.ErrorMessage = "Invalid fabric batch production batch man !!!";
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricBatchLoomDetail = new FabricBatchLoomDetail();
                oFabricBatchLoomDetail.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricBatchLoomDetail;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricBatchLoomDetail oFabricBatchLoomDetail = new FabricBatchLoomDetail();
                oFabricBatchLoomDetail.FBLDetailID = id;
                FabricBatchLoomDetailDA.Delete(tc, EnumDBOperation.Delete, oFabricBatchLoomDetail, nUserId);
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
        public string MultipleApprove(String FBLDetailIDS, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricBatchLoomDetailDA.MultipleApprove(tc, FBLDetailIDS, nUserId);
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
            return "Success";
        }
        public string MultipleDelete(String FBLDetailIDS, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricBatchLoomDetailDA.MultipleDelete(tc, FBLDetailIDS, nUserId);
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

    }


}
