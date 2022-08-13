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
    public class FabricPatternDetailService : MarshalByRefObject, IFabricPatternDetailService
    {
        #region Private functions and declaration
        private FabricPatternDetail MapObject(NullHandler oReader)
        {
            FabricPatternDetail oFPDetail = new FabricPatternDetail();
            oFPDetail.FPDID = oReader.GetInt32("FPDID");
            oFPDetail.FPID = oReader.GetInt32("FPID");
            oFPDetail.IsWarp = oReader.GetBoolean("IsWarp");
            oFPDetail.ProductID = oReader.GetInt32("ProductID");
            oFPDetail.ColorName = oReader.GetString("ColorNameLD");
            oFPDetail.ColorNo = oReader.GetString("ColorNo");
            oFPDetail.PantonNo = oReader.GetString("PantonNo");
            oFPDetail.EndsCount = oReader.GetInt32("EndsCount");
            oFPDetail.LabDipDetailID = oReader.GetInt32("LabDipDetailID");
            oFPDetail.TwistedGroup = oReader.GetInt32("TwistedGroup");
            oFPDetail.TwistedGroupInt = oReader.GetInt32("TwistedGroupInt");
            oFPDetail.ProductCode = oReader.GetString("ProductCode");
            oFPDetail.ProductName = oReader.GetString("ProductName");
            oFPDetail.ProductShortName = oReader.GetString("ProductShortName");
            oFPDetail.Value = oReader.GetDouble("Value");
            oFPDetail.SetNo = oReader.GetInt32("SetNo");
            oFPDetail.SLNo = oReader.GetInt32("SLNo");
            
            oFPDetail.GroupNo = oReader.GetInt32("GroupNo");
            return oFPDetail;
        }

        public FabricPatternDetail CreateObject(NullHandler oReader)
        {
            FabricPatternDetail oFabricPatternDetail = new FabricPatternDetail();
            oFabricPatternDetail = MapObject(oReader);
            return oFabricPatternDetail;
        }

        private List<FabricPatternDetail> CreateObjects(IDataReader oReader)
        {
            List<FabricPatternDetail> oFabricPatternDetails = new List<FabricPatternDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricPatternDetail oItem = CreateObject(oHandler);
                oFabricPatternDetails.Add(oItem);
            }
            return oFabricPatternDetails;
        }
        #endregion

        #region Interface implementatio
        public FabricPatternDetailService() { }

        public FabricPatternDetail IUD(FabricPatternDetail oFPDetail, int nDBOperation, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                string sFPDetailIds = oFPDetail.FPDetailIds;
                IDataReader reader;
                NullHandler oReader;
                if (nDBOperation == (int)EnumDBOperation.Insert && oFPDetail.FPID == 0) {

                    if (oFPDetail.FP != null)
                    {
                        reader = FabricPatternDA.IUD(tc, oFPDetail.FP, nDBOperation, nUserId);
                        oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oFPDetail.FPID = Convert.ToInt32(reader["FPID"]);
                        }
                        reader.Close();
                    }
                    else { throw new Exception("No fabric pattern information found to save."); }
                }

                reader = FabricPatternDetailDA.IUD(tc, oFPDetail, nDBOperation, nUserId);

                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFPDetail = new FabricPatternDetail();
                    oFPDetail = CreateObject(oReader);
                }
                if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    oFPDetail = new FabricPatternDetail();
                    oFPDetail.ErrorMessage = Global.DeleteMessage;
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFPDetail = new FabricPatternDetail();
                oFPDetail.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFPDetail;
        }

        public FabricPatternDetail Get(int nFPID, Int64 nUserId)
        {
            FabricPatternDetail oFPDetail = new FabricPatternDetail();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricPatternDetailDA.Get(tc, nFPID, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFPDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFPDetail = new FabricPatternDetail();
                oFPDetail.ErrorMessage = "Failed to get information.";
                #endregion
            }

            return oFPDetail;
        }

        public List<FabricPatternDetail> Gets(int nFPID, Int64 nUserId)
        {
            List<FabricPatternDetail> oFPDetails = new List<FabricPatternDetail>();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricPatternDetailDA.Gets(tc, nFPID, nUserId);
                oFPDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFPDetails = new List<FabricPatternDetail>();
                #endregion
            }

            return oFPDetails;
        }
        public List<FabricPatternDetail> Gets(string sSQL, Int64 nUserId)
        {
            List<FabricPatternDetail> oFPDetails = new List<FabricPatternDetail>();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricPatternDetailDA.Gets(tc, sSQL, nUserId);
                oFPDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFPDetails = new List<FabricPatternDetail>();
                #endregion
            }

            return oFPDetails;
        }
        public FabricPatternDetail SavePatternRepeat(FabricPatternDetail oFabricPatternDetail, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                FabricPatternDetailDA.SavePatternRepeat(tc, oFabricPatternDetail, nUserId);
                reader = FabricPatternDetailDA.Gets(tc, oFabricPatternDetail.FPID, nUserId);
                oFabricPatternDetail.FabricPatternDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFabricPatternDetail = new FabricPatternDetail();
                oFabricPatternDetail.ErrorMessage = e.Message;
                #endregion
            }

            return oFabricPatternDetail;
        }
        public FabricPatternDetail CopyFromWarp(FabricPatternDetail oFabricPatternDetail, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                int nFPID = oFabricPatternDetail.FPID;
                FabricPatternDetailDA.CopyFromWarp(tc, oFabricPatternDetail, nUserId);
                string sSQL = "SELECT * FROM View_FabricPatternDetail WHERE FPID = " + oFabricPatternDetail.FPID + " AND IsWarp=0  Order by SLNo";
                reader = FabricPatternDetailDA.Gets(tc, sSQL, nUserId);
                oFabricPatternDetail.FabricPatternDetails = CreateObjects(reader);
                oFabricPatternDetail.FPID = nFPID;
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFabricPatternDetail = new FabricPatternDetail();
                oFabricPatternDetail.ErrorMessage = e.Message;
                #endregion
            }

            return oFabricPatternDetail;
        }
        public List<FabricPatternDetail> MakeTwistedGroup(string sFabricPatternDetailID, int nLabDipID, int nTwistedGroup,  int nDBOperation, int nUserID)
        {
            List<FabricPatternDetail> oFabricPatternDetails = new List<FabricPatternDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricPatternDetailDA.MakeTwistedGroup(tc, sFabricPatternDetailID, nLabDipID, nTwistedGroup, nDBOperation, nUserID);
                oFabricPatternDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FabricPatternDetail oFabricPatternDetail = new FabricPatternDetail();
                oFabricPatternDetail.ErrorMessage = e.Message;
                oFabricPatternDetails.Add(oFabricPatternDetail);
                #endregion
            }

            return oFabricPatternDetails;
        }
       
        #endregion
    }
}