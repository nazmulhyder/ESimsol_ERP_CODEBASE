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
    public class RouteSheetDetailService : MarshalByRefObject, IRouteSheetDetailService
    {
        #region Private functions and declaration
        private RouteSheetDetail MapObject(NullHandler oReader)
        {
            RouteSheetDetail oRouteSheetDetail = new RouteSheetDetail();
            oRouteSheetDetail.RouteSheetDetailID = oReader.GetInt32("RouteSheetDetailID");
            oRouteSheetDetail.RouteSheetID = oReader.GetInt32("RouteSheetID");
            oRouteSheetDetail.ProcessID = oReader.GetInt32("ProcessID");
            oRouteSheetDetail.ParentID = oReader.GetInt32("ParentID");
            oRouteSheetDetail.IsDyesChemical = oReader.GetBoolean("IsDyesChemical");
            oRouteSheetDetail.TempTime = oReader.GetString("TempTime");
            oRouteSheetDetail.GL = oReader.GetDouble("GL");
            oRouteSheetDetail.Percentage = oReader.GetDouble("Percentage");
            oRouteSheetDetail.DAdjustment = oReader.GetDouble("DAdjustment");
            oRouteSheetDetail.Note = oReader.GetString("Note");
            oRouteSheetDetail.BatchManID = oReader.GetInt32("BatchManID");
            oRouteSheetDetail.Equation = oReader.GetString("Equation");
            oRouteSheetDetail.Sequence = oReader.GetInt16("Sequence");
            oRouteSheetDetail.ForCotton = oReader.GetBoolean("ForCotton");
            oRouteSheetDetail.SupportMaterial = oReader.GetBoolean("SupportMaterial");
            oRouteSheetDetail.TotalQty = oReader.GetDouble("TotalQty");
            oRouteSheetDetail.AddOneQty = oReader.GetDouble("AddOneQty");
            oRouteSheetDetail.AddTwoQty = oReader.GetDouble("AddTwoQty");
            oRouteSheetDetail.AddThreeQty = oReader.GetDouble("AddThreeQty");
            oRouteSheetDetail.ReturnQty = oReader.GetDouble("ReturnQty");
            oRouteSheetDetail.TotalQtyLotID = oReader.GetInt32("TotalQtyLotID");
            oRouteSheetDetail.TotalQtyLotNo = oReader.GetString("TotalQtyLotNo");
            oRouteSheetDetail.AddOneLotID = oReader.GetInt32("AddOneLotID");
            oRouteSheetDetail.AddOneLotNo = oReader.GetString("AddOneLotNo");
            oRouteSheetDetail.AddTwoLotID = oReader.GetInt32("AddTwoLotID");
            oRouteSheetDetail.AddTwoLotNo = oReader.GetString("AddTwoLotNo");
            oRouteSheetDetail.AddThreeLotID = oReader.GetInt32("AddThreeLotID");
            oRouteSheetDetail.AddThreeLotNo = oReader.GetString("AddThreeLotNo");
            oRouteSheetDetail.ReturnLotID = oReader.GetInt32("ReturnLotID");
            oRouteSheetDetail.ReturnLotNo = oReader.GetString("ReturnLotNo");

            oRouteSheetDetail.ProcessName = oReader.GetString("ProcessName");
            oRouteSheetDetail.BatchEmpName = oReader.GetString("BatchEmpName");
            oRouteSheetDetail.AddOnePercentage = oReader.GetDouble("AddOnePercentage");
            oRouteSheetDetail.AddTwoPercentage = oReader.GetDouble("AddTwoPercentage");
            oRouteSheetDetail.AddThreePercentage = oReader.GetDouble("AddThreePercentage");
            oRouteSheetDetail.ProductCode = oReader.GetString("ProductCode");
            oRouteSheetDetail.ProductCategoryName = oReader.GetString("ProductCategoryName");
            oRouteSheetDetail.SuggestLotID = oReader.GetInt32("SuggestLotID");
            oRouteSheetDetail.SuggestLotNo = oReader.GetString("SuggestLotNo");
            oRouteSheetDetail.Balance = oReader.GetDouble("Balance");
            oRouteSheetDetail.PCategoryID = oReader.GetInt32("PCategoryID");
            oRouteSheetDetail.WUID = oReader.GetInt32("WUID");
            oRouteSheetDetail.RecipeCalType = (EnumDyeingRecipeType)oReader.GetInt16("RecipeCalType");
            oRouteSheetDetail.ProductType = (EnumProductNature)oReader.GetInt16("ProductType");
            return oRouteSheetDetail;
        }
        public static RouteSheetDetail CreateObject(NullHandler oReader)
        {
            RouteSheetDetail oRouteSheetDetail = new RouteSheetDetail();
            RouteSheetDetailService oService = new RouteSheetDetailService();
            oRouteSheetDetail = oService.MapObject(oReader);
            return oRouteSheetDetail;
        }
        public static List<RouteSheetDetail> CreateObjects(IDataReader oReader)
        {
            List<RouteSheetDetail> oRouteSheetDetails = new List<RouteSheetDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RouteSheetDetail oItem = CreateObject(oHandler);
                oRouteSheetDetails.Add(oItem);
            }
            return oRouteSheetDetails;
        }

        #endregion

        #region Interface implementation
        public RouteSheetDetailService() { }

        public RouteSheetDetail IUD(RouteSheetDetail oRouteSheetDetail, int nDBOperation, long nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                NullHandler oReader;

                //#region Total Qty Calulation

                //RouteSheet oRouteSheet = new RouteSheet();
                //reader = RouteSheetDA.Get(tc, oRouteSheetDetail.RouteSheetID);
                //oReader = new NullHandler(reader);
                //if (reader.Read())
                //{
                //    oRouteSheet = new RouteSheet();
                //    oRouteSheet = RouteSheetService.CreateObject(oReader);
                //}
                //reader.Close();

                //double nLiquor = (oRouteSheetDetail.ForCotton) ? oRouteSheet.TtlCotton : oRouteSheet.TtlLiquire;
                ////if (oRouteSheet.TtlLiquire > 0)
                ////{
                //    if (oRouteSheetDetail.GL > 0)
                //    {
                //        oRouteSheetDetail.TotalQty = (( oRouteSheet.Qty*0.454)*nLiquor * oRouteSheetDetail.GL) / 1000;
                //    }
                //    else if (oRouteSheetDetail.Percentage > 0)
                //    {
                //        if (oRouteSheetDetail.DAdjustment != 0)
                //        {
                //            oRouteSheetDetail.TotalQty = ((oRouteSheetDetail.Percentage + (oRouteSheetDetail.Percentage * oRouteSheetDetail.DAdjustment / 100)) * 10 * (oRouteSheet.Qty * 0.454)) / 1000;
                //        }
                //        else
                //        {
                //            oRouteSheetDetail.TotalQty = (oRouteSheetDetail.Percentage * 10 * oRouteSheet.Qty*0.454) / 1000;
                //        }
                //    }
                //    else { oRouteSheetDetail.TotalQty = 0; }
                

                //#endregion
  

                reader = RouteSheetDetailDA.IUD(tc, oRouteSheetDetail, nDBOperation, nUserID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRouteSheetDetail = new RouteSheetDetail();
                    oRouteSheetDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();

                if (nDBOperation == (int)EnumDBOperation.Delete) { oRouteSheetDetail = new RouteSheetDetail(); oRouteSheetDetail.ErrorMessage = Global.DeleteMessage; }
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oRouteSheetDetail.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }
            return oRouteSheetDetail;
        }
        public List<RouteSheetDetail> Update(List<RouteSheetDetail> oRouteSheetDetails, Int64 nUserID)
        {
            RouteSheetDetail oRouteSheetDetail = new RouteSheetDetail();
            List<RouteSheetDetail> oRouteSheetDetails_Return = new List<RouteSheetDetail>();
            int nRouteSheetID = 0;
            double nLiquor = 0;
            if (oRouteSheetDetails != null)
            {
                nRouteSheetID = oRouteSheetDetails[0].RouteSheetID;
            }
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                NullHandler oReader;
                RouteSheet oRouteSheet = new RouteSheet();
                reader = RouteSheetDA.Get(tc, nRouteSheetID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRouteSheet = new RouteSheet();
                    oRouteSheet = RouteSheetService.CreateObject(oReader);
                }
                reader.Close();

                foreach (RouteSheetDetail oItem in oRouteSheetDetails)
                {
                    IDataReader readerdetail;
                    nLiquor = (oItem.ForCotton) ? oRouteSheet.TtlCotton : oRouteSheet.TtlLiquire;
                    
                    if (oItem.GL > 0)
                    {
                        oItem.TotalQty = ((oRouteSheet.Qty * 0.454)*nLiquor * oItem.GL) / 1000;
                    }
                    else if (oItem.Percentage > 0)
                    {
                        if (oItem.DAdjustment != 0)
                        {
                            oItem.TotalQty = ((oItem.Percentage + (oItem.Percentage * oItem.DAdjustment / 100)) * 10 * (oRouteSheet.Qty * 0.454)) / 1000;
                        }
                        else
                        {
                            oItem.TotalQty = (oItem.Percentage * 10 * oRouteSheet.Qty * 0.454) / 1000;
                        }
                    }
                    else { oItem.TotalQty = 0; }

                    readerdetail = RouteSheetDetailDA.IUD(tc, oItem, (int)EnumDBOperation.Update, nUserID);
                    
                    NullHandler oReaderTwo = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        oRouteSheetDetail = new RouteSheetDetail();
                        oRouteSheetDetail = CreateObject(oReaderTwo);
                        oRouteSheetDetails_Return.Add(oRouteSheetDetail);
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

                oRouteSheetDetails_Return = new List<RouteSheetDetail>();
                oRouteSheetDetail = new RouteSheetDetail();
                oRouteSheetDetail.ErrorMessage = e.Message.Split('~')[0];
                oRouteSheetDetails_Return.Add(oRouteSheetDetail);

                #endregion
            }
            return oRouteSheetDetails_Return;
        }
        public RouteSheetDetail Get(int nRouteSheetDetailID, long nUserID)
        {
            RouteSheetDetail oRouteSheetDetail = new RouteSheetDetail();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = RouteSheetDetailDA.Get(tc, nRouteSheetDetailID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRouteSheetDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();

                oRouteSheetDetail.ErrorMessage = e.Message;
                #endregion
            }

            return oRouteSheetDetail;
        }

        public List<RouteSheetDetail> Gets(string sSQL, long nUserID)
        {
            List<RouteSheetDetail> oRouteSheetDetails = new List<RouteSheetDetail>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RouteSheetDetailDA.Gets(tc, sSQL);
                oRouteSheetDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                RouteSheetDetail oRouteSheetDetail = new RouteSheetDetail();
                oRouteSheetDetail.ErrorMessage = e.Message;
                oRouteSheetDetails.Add(oRouteSheetDetail);
                #endregion
            }

            return oRouteSheetDetails;
        }
        public List<RouteSheetDetail> Gets(int nRSID, long nUserID)
        {
            List<RouteSheetDetail> oRouteSheetDetails = new List<RouteSheetDetail>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RouteSheetDetailDA.Gets(tc, nRSID);
                oRouteSheetDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                RouteSheetDetail oRouteSheetDetail = new RouteSheetDetail();
                oRouteSheetDetail.ErrorMessage = e.Message;
                oRouteSheetDetails.Add(oRouteSheetDetail);
                #endregion
            }

            return oRouteSheetDetails;
        }

        public List<RouteSheetDetail> IUDTemplate(int nDSID, int nRSID, long nUserID)
        {
            List<RouteSheetDetail> oRouteSheetDetails = new List<RouteSheetDetail>();
            RouteSheetDetail oRouteSheetDetail = new RouteSheetDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                if (RouteSheetDetailDA.GetDyeChemicalOut(tc, nRSID) > 0)
                    throw new Exception("Already dyes chemical out.");

                IDataReader reader = null;
                reader = RouteSheetDetailDA.IUDTemplate(tc, nDSID, nRSID, nUserID);
                oRouteSheetDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oRouteSheetDetail.ErrorMessage = e.Message.Contains("!")? e.Message.Split('!')[0]: e.Message;
                oRouteSheetDetails.Add(oRouteSheetDetail);
                #endregion
            }

            return oRouteSheetDetails;
        }
        public List<RouteSheetDetail> IUDTemplateCopyFromRS(int nRSID_CopyFrom, int nRSID, long nUserID)
        {
            List<RouteSheetDetail> oRouteSheetDetails = new List<RouteSheetDetail>();
            RouteSheetDetail oRouteSheetDetail = new RouteSheetDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                if (RouteSheetDetailDA.GetDyeChemicalOut(tc, nRSID) > 0)
                    throw new Exception("Already dyes chemical out.");

                IDataReader reader = null;
                reader = RouteSheetDetailDA.IUDTemplateCopyFromRS(tc, nRSID_CopyFrom, nRSID, nUserID);
                oRouteSheetDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oRouteSheetDetail.ErrorMessage = e.Message.Contains("!") ? e.Message.Split('!')[0] : e.Message;
                oRouteSheetDetails.Add(oRouteSheetDetail);
                #endregion
            }

            return oRouteSheetDetails;
        }
        public List<RouteSheetDetail> DyeChemicalOut_All(List<RouteSheetDetail> oRouteSheetDetails, Int64 nUserID)
        {
            RouteSheetDetail oRouteSheetDetail = new RouteSheetDetail();
            List<RouteSheetDetail> oRouteSheetDetails_Return = new List<RouteSheetDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                IDataReader reader;
                foreach (RouteSheetDetail oItem in oRouteSheetDetails)
                {
                    short nAddFor = 0;
                    if (oItem.OutState <= 0)
                    {
                        nAddFor = this.DyesChemicalOutFor(oItem);
                    }
                    else
                    {
                        nAddFor = (short)oItem.OutState;
                    }

                    if (nAddFor < 0) { throw new Exception("Invalid request, refresh & try with another."); }

                    int nLotID = 0;
                    if (nAddFor == 0)
                        nLotID = oItem.TotalQtyLotID;
                    else if (nAddFor == 1 && oItem.AddOneQty>0)
                        nLotID = oItem.AddOneLotID;
                    else if (nAddFor == 2 && oItem.AddTwoQty > 0)
                        nLotID = oItem.AddTwoLotID;
                    else if (nAddFor == 3 && oItem.AddThreeQty > 0)
                        nLotID = oItem.AddThreeLotID;
                    else if (nAddFor == 4 && oItem.ReturnQty> 0)
                        nLotID = oItem.ReturnLotID;

                    if (nLotID > 0)
                    {
                        reader = RouteSheetDetailDA.DyeChemicalOut(tc, oItem, nLotID, nAddFor, nUserID);

                        NullHandler oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oRouteSheetDetail = new RouteSheetDetail();
                            oRouteSheetDetail = CreateObject(oReader);
                            oRouteSheetDetails_Return.Add(oRouteSheetDetail);
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
                oRouteSheetDetails_Return = new List<RouteSheetDetail>();
                oRouteSheetDetail = new RouteSheetDetail();
                oRouteSheetDetail.ErrorMessage = e.Message.Split('~')[0];
                oRouteSheetDetails_Return.Add(oRouteSheetDetail);

                #endregion
            }
            return oRouteSheetDetails_Return;
        }
        public List<RouteSheetDetail> DyeChemicalOut_All_V2(List<RSDetailAdditonal> oRSDetailAdditonals, Int64 nUserID)
        {
            RouteSheetDetail oRouteSheetDetail = new RouteSheetDetail();
            List<RouteSheetDetail> oRouteSheetDetails_Return = new List<RouteSheetDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                IDataReader reader;
                foreach (RSDetailAdditonal oItem in oRSDetailAdditonals)
                {
                    short nAddFor = 0;
                    //if (oItem.OutState <= 0)
                    //{
                    //    nAddFor = this.DyesChemicalOutFor(oItem);
                    //}
                    //else
                    //{
                    //    nAddFor = (short)oItem.OutState;
                    //}

                    //if (nAddFor < 0) { throw new Exception("Invalid request, refresh & try with another."); }

                    int nLotID = 0;
                    //nLotID = oItem.SuggestLotID;

                    reader = RouteSheetDetailDA.DyeChemicalOut_V2(tc, oItem,  nUserID);

                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oRouteSheetDetail = new RouteSheetDetail();
                        oRouteSheetDetail = CreateObject(oReader);
                        oRouteSheetDetails_Return.Add(oRouteSheetDetail);
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
                oRouteSheetDetails_Return = new List<RouteSheetDetail>();
                oRouteSheetDetail = new RouteSheetDetail();
                oRouteSheetDetail.ErrorMessage = e.Message.Split('~')[0];
                oRouteSheetDetails_Return.Add(oRouteSheetDetail);

                #endregion
            }
            return oRouteSheetDetails_Return;
        }

        public List<RouteSheetDetail> DyeChemicalOut_All_Return(List<RouteSheetDetail> oRouteSheetDetails, Int64 nUserID)
        {
            RouteSheetDetail oRouteSheetDetail = new RouteSheetDetail();
            List<RouteSheetDetail> oRouteSheetDetails_Return = new List<RouteSheetDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                IDataReader reader;
                foreach (RouteSheetDetail oItem in oRouteSheetDetails)
                {
                    short nAddFor = 0;
                    //if (oItem.OutState <= 0)
                    //{
                    //    nAddFor = this.DyesChemicalOutFor(oItem);
                    //}
                    //else
                    //{
                    //    nAddFor = (short)oItem.OutState;
                    //}

                    //if (nAddFor < 0) { throw new Exception("Invalid request, refresh & try with another."); }

                    int nLotID = 0;
                    nLotID = oItem.TotalQtyLotID;

                    reader = RouteSheetDetailDA.DyeChemicalOut_Return(tc, oItem, nLotID, nAddFor, nUserID);

                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oRouteSheetDetail = new RouteSheetDetail();
                        oRouteSheetDetail = CreateObject(oReader);
                        oRouteSheetDetails_Return.Add(oRouteSheetDetail);
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
                oRouteSheetDetails_Return = new List<RouteSheetDetail>();
                oRouteSheetDetail = new RouteSheetDetail();
                oRouteSheetDetail.ErrorMessage = e.Message.Split('~')[0];
                oRouteSheetDetails_Return.Add(oRouteSheetDetail);

                #endregion
            }
            return oRouteSheetDetails_Return;
        }

        public RouteSheetDetail Update_RSDetail(RouteSheetDetail oRouteSheetDetail, long nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                NullHandler oReader;

                reader = RouteSheetDetailDA.Update_RSDetail(tc, oRouteSheetDetail);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRouteSheetDetail = new RouteSheetDetail();
                    oRouteSheetDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oRouteSheetDetail.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }
            return oRouteSheetDetail;
        }
     

        public RouteSheetDetail DyeChemicalOut(RouteSheetDetail oRouteSheetDetail, long nUserID)
        {
            TransactionContext tc = null;
            try
            {
                short nAddFor = this.DyesChemicalOutFor(oRouteSheetDetail);
                if (nAddFor < 0) { throw new Exception("Invalid request, refresh & try with another."); }

                int nLotID = 0;
                if (nAddFor == 0)
                    nLotID = oRouteSheetDetail.TotalQtyLotID;
                else if (nAddFor == 1)
                    nLotID = oRouteSheetDetail.AddOneLotID;
                else if (nAddFor == 2)
                    nLotID = oRouteSheetDetail.AddTwoLotID;
                else if (nAddFor == 3)
                    nLotID = oRouteSheetDetail.AddThreeLotID;
                else if (nAddFor == 4)
                    nLotID = oRouteSheetDetail.ReturnLotID;

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                NullHandler oReader;
                reader = RouteSheetDetailDA.DyeChemicalOut(tc, oRouteSheetDetail, nLotID, nAddFor, nUserID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRouteSheetDetail = new RouteSheetDetail();
                    oRouteSheetDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oRouteSheetDetail = new RouteSheetDetail();
                oRouteSheetDetail.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }
            return oRouteSheetDetail;
        }

        private short DyesChemicalOutFor(RouteSheetDetail oRouteSheetDetail)
        {

            if (oRouteSheetDetail.TotalQtyLotID > 0 && oRouteSheetDetail.AddOneLotID <= 0 && oRouteSheetDetail.ReturnLotID<=0)
            {
                return 0;
            }
            else if (oRouteSheetDetail.AddOneLotID > 0 && oRouteSheetDetail.AddTwoLotID <= 0 && oRouteSheetDetail.ReturnLotID <= 0)
            {
                return 1;
            }
            else if (oRouteSheetDetail.AddTwoLotID > 0 && oRouteSheetDetail.AddThreeLotID <= 0 && oRouteSheetDetail.ReturnLotID <= 0)
            {
                return 2;
            }
            if (oRouteSheetDetail.AddThreeLotID > 0 && oRouteSheetDetail.ReturnLotID<=0)
            {
                return 3;
            }
            if (oRouteSheetDetail.ReturnLotID > 0 && string.IsNullOrEmpty(oRouteSheetDetail.ReturnLotNo))
            {
                return 4;
            }
            else
            {
                return -1;
            }
        }

        public RouteSheetDetail RefreshSequence(RouteSheetDetail oRouteSheetDetail, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                if (oRouteSheetDetail.children.Count > 0)
                {
                    foreach (RouteSheetDetail oItem in oRouteSheetDetail.children)
                    {
                        if (oItem.RouteSheetDetailID > 0 && oItem.Sequence > 0)
                        {
                            RouteSheetDetailDA.UpdateSequence(tc, oItem);
                        }
                    }
                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oRouteSheetDetail = new RouteSheetDetail();
                oRouteSheetDetail.ErrorMessage = e.Message;
                #endregion
            }
            return oRouteSheetDetail;
        }
        #endregion
    }
}