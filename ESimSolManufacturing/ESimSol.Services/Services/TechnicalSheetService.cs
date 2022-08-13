using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
 
namespace ESimSol.Services.Services
{
    public class TechnicalSheetService : MarshalByRefObject, ITechnicalSheetService
    {
        #region Private functions and declaration
        private TechnicalSheet MapObject(NullHandler oReader)
        {
            TechnicalSheet oTechnicalSheet = new TechnicalSheet();
            oTechnicalSheet.TechnicalSheetID = oReader.GetInt32("TechnicalSheetID");
            oTechnicalSheet.StyleNo = oReader.GetString("StyleNo");
            oTechnicalSheet.BusinessSessionID = oReader.GetInt32("BusinessSessionID");
            oTechnicalSheet.DevelopmentStatus = (EnumDevelopmentStatus)oReader.GetInt32("DevelopmentStatus");
            oTechnicalSheet.BuyerID = oReader.GetInt32("BuyerID");
            oTechnicalSheet.BUID = oReader.GetInt32("BUID");
            oTechnicalSheet.ProductID = oReader.GetInt32("ProductID");
            oTechnicalSheet.SubGender =  (EnumSubGender)oReader.GetInt32("SubGender");
            oTechnicalSheet.Dept = oReader.GetInt32("Dept");
            oTechnicalSheet.DeptName = oReader.GetString("DeptName");
            oTechnicalSheet.BuyerConcernID = oReader.GetInt32("BuyerConcernID");
            oTechnicalSheet.YarnCategoryID = oReader.GetInt32("YarnCategoryID");
   
            oTechnicalSheet.GG = oReader.GetString("GG");
            oTechnicalSheet.Count = oReader.GetString("Count");
            oTechnicalSheet.SpecialFinish = oReader.GetString("SpecialFinish");
            oTechnicalSheet.BrandName = oReader.GetString("BrandName");
            oTechnicalSheet.Weight = oReader.GetString("Weight"); ;
            oTechnicalSheet.Line = oReader.GetString("Line");
            oTechnicalSheet.Designer = oReader.GetString("Designer");
            oTechnicalSheet.Story = oReader.GetString("Story");
            oTechnicalSheet.GarmentsClassID = oReader.GetInt32("GarmentsClassID");
            oTechnicalSheet.GarmentsSubClassID = oReader.GetInt32("GarmentsSubClassID");
            oTechnicalSheet.Intake = oReader.GetString("Intake");
            oTechnicalSheet.Note = oReader.GetString("Note");
            oTechnicalSheet.BrandID = oReader.GetInt32("BrandID");
            oTechnicalSheet.KnittingPattern = oReader.GetInt32("KnittingPattern");
            oTechnicalSheet.KnittingPatternName = oReader.GetString("KnittingPatternName");
            oTechnicalSheet.StyleDescription = oReader.GetString("StyleDescription");
            oTechnicalSheet.BuyerName = oReader.GetString("BuyerName");
            oTechnicalSheet.BuyerAddress = oReader.GetString("BuyerAddress");
            oTechnicalSheet.BuyerPhone = oReader.GetString("BuyerPhone");
            oTechnicalSheet.BuyerShortName = oReader.GetString("BuyerShortName");
            oTechnicalSheet.ProductCode = oReader.GetString("ProductCode");
            oTechnicalSheet.ProductName = oReader.GetString("ProductName");

            oTechnicalSheet.ConcernName = oReader.GetString("ConcernName");
            oTechnicalSheet.ConcernEmail = oReader.GetString("ConcernEmail");
            oTechnicalSheet.ClassName = oReader.GetString("ClassName");
            oTechnicalSheet.SubClassName = oReader.GetString("SubClassName");
            oTechnicalSheet.ColorRangeIDs = oReader.GetString("ColorRangeIDs");
            oTechnicalSheet.ColorRange = oReader.GetString("ColorRange");
            oTechnicalSheet.SizeRangeIDs = oReader.GetString("SizeRangeIDs");
            oTechnicalSheet.SizeRange = oReader.GetString("SizeRange");
            oTechnicalSheet.MeasurementSpecID = oReader.GetInt32("MeasurementSpecID");
            oTechnicalSheet.SampleSizeID = oReader.GetInt32("SampleSizeID");
            oTechnicalSheet.SizeClassID = oReader.GetInt32("SizeClassID");
            oTechnicalSheet.GarmentsTypeID = oReader.GetInt32("GarmentsTypeID");
            oTechnicalSheet.ShownAs = oReader.GetString("ShownAs");
            oTechnicalSheet.MSNote = oReader.GetString("MSNote");
            oTechnicalSheet.SessionName = oReader.GetString("SessionName");
            oTechnicalSheet.IsExistDevelopmentRecap = oReader.GetBoolean("IsExistDevelopmentRecap");
            oTechnicalSheet.IsExistOrderRecap = oReader.GetBoolean("IsExistOrderRecap");
            oTechnicalSheet.TSType = (EnumTSType)oReader.GetInt32("TSType");
            oTechnicalSheet.TSTypeInInt = oReader.GetInt32("TSType");
 
            oTechnicalSheet.FabConstruction = oReader.GetString("FabConstruction");
            oTechnicalSheet.Wash = oReader.GetString("Wash");
            oTechnicalSheet.FabWidth = oReader.GetString("FabWidth");

            oTechnicalSheet.FabCode = oReader.GetString("FabCode");
            oTechnicalSheet.UserName = oReader.GetString("UserName");
            oTechnicalSheet.MerchandiserID = oReader.GetInt32("MerchandiserID");
            oTechnicalSheet.MerchandiserName = oReader.GetString("MerchandiserName");
            oTechnicalSheet.TSCount = oReader.GetInt32("TSCount");
            oTechnicalSheet.ApproxQty = oReader.GetDouble("ApproxQty");
            oTechnicalSheet.DBServerDateTime = oReader.GetDateTime("DBServerDateTime");
            oTechnicalSheet.MeasurementUnitID = oReader.GetInt32("MeasurementUnitID");
            oTechnicalSheet.MUnitSymbol = oReader.GetString("MUnitSymbol");
            oTechnicalSheet.FabricDescription = oReader.GetString("FabricDescription");
            oTechnicalSheet.GSMID = oReader.GetInt32("GSMID");
            oTechnicalSheet.GSMName = oReader.GetString("GSMName");
            
            return oTechnicalSheet;
        }

        private TechnicalSheet CreateObject(NullHandler oReader)
        {
            TechnicalSheet oTechnicalSheet = new TechnicalSheet();
            oTechnicalSheet = MapObject(oReader);
            return oTechnicalSheet;
        }

        private List<TechnicalSheet> CreateObjects(IDataReader oReader)
        {
            List<TechnicalSheet> oTechnicalSheet = new List<TechnicalSheet>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                TechnicalSheet oItem = CreateObject(oHandler);
                oTechnicalSheet.Add(oItem);
            }
            return oTechnicalSheet;
        }

        private bool ColorExists(int nColorID, List<TechnicalSheetColor> oTechnicalSheetColors)
        {
            foreach (TechnicalSheetColor oItem in oTechnicalSheetColors)
            {
                if (oItem.ColorCategoryID == nColorID)
                {
                    return true;
                }
            }
            return false;
        }

        private bool SizeExists(int nSizeID, List<TechnicalSheetSize> oTechnicalSheetSizes)
        {
            foreach (TechnicalSheetSize oItem in oTechnicalSheetSizes)
            {
                if (oItem.SizeCategoryID== nSizeID)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Interface implementation
        public TechnicalSheetService() { }

        public TechnicalSheet Save(TechnicalSheet oTechnicalSheet, Int64 nUserID)
        {
            #region Declaration
            List<TechnicalSheetColor> oTechnicalSheetColors = new List<TechnicalSheetColor>();
            TechnicalSheetColor oTechnicalSheetColor = new TechnicalSheetColor();
            List<TechnicalSheetSize> oTechnicalSheetSizes = new List<TechnicalSheetSize>();
            List<TechnicalSheetShipment> oTechnicalSheetShipments = new List<TechnicalSheetShipment>();
            TechnicalSheetSize oTechnicalSheetSize = new TechnicalSheetSize();
            TechnicalSheetShipment oTechnicalSheetShipment = new TechnicalSheetShipment();
            List<TechnicalSheetColor> oTempTechnicalSheetColors = new List<TechnicalSheetColor>();
            List<TechnicalSheetSize> oTempTechnicalSheetSizes= new List<TechnicalSheetSize>();
            List<TechnicalSheetShipment> oTempTechnicalSheetShipments = new List<TechnicalSheetShipment>();
            List<OrderRecapYarn> oOrderRecapYarns = new List<OrderRecapYarn>();
            #endregion

            oTechnicalSheetColors = oTechnicalSheet.TechnicalSheetColors;
            oTechnicalSheetSizes = oTechnicalSheet.TechnicalSheetSizes;
            oTechnicalSheetShipments = oTechnicalSheet.TechnicalSheetShipments;
            oOrderRecapYarns = oTechnicalSheet.OrderRecapYarns;
            TransactionContext tc = null; string sTempString = ""; string sTechnicalSheetShipmentIDs = "", sOrderRecapYarnIDs="";
            oTechnicalSheet.ErrorMessage = "";
            try
            {
                tc = TransactionContext.Begin(true);
                #region Technical Sheet Part
                IDataReader reader;
                if (oTechnicalSheet.TechnicalSheetID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.TechnicalSheet, EnumRoleOperationType.Add);
                    reader = TechnicalSheetDA.InsertUpdate(tc, oTechnicalSheet, EnumDBOperation.Insert, nUserID);                    
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.TechnicalSheet, EnumRoleOperationType.Edit);
                    reader = TechnicalSheetDA.InsertUpdate(tc, oTechnicalSheet, EnumDBOperation.Update, nUserID);                  
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTechnicalSheet = new TechnicalSheet();
                    oTechnicalSheet = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region Technical Sheet Color Part
                oTechnicalSheetColor = new TechnicalSheetColor();
                oTechnicalSheetColor.TechnicalSheetID = oTechnicalSheet.TechnicalSheetID;
                TechnicalSheetColorDA.Delete(tc, oTechnicalSheetColor, EnumDBOperation.Delete, nUserID);
                foreach (TechnicalSheetColor oItem in oTechnicalSheetColors)
                {                    
                    oItem.TechnicalSheetID = oTechnicalSheet.TechnicalSheetID;
                    reader = TechnicalSheetColorDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                    NullHandler oReaderDetail = new NullHandler(reader);
                    if(reader.Read())
                    {
                        oTechnicalSheetColor = new TechnicalSheetColor();                        
                        oTechnicalSheetColor.ColorCategoryID = oReaderDetail.GetInt32("ColorCategoryID");
                        oTempTechnicalSheetColors.Add(oTechnicalSheetColor);
                    }
                    reader.Close();
                }

                

                #endregion

                #region Technical Sheet Size Part
                oTechnicalSheetSize = new TechnicalSheetSize();
                oTechnicalSheetSize.TechnicalSheetID = oTechnicalSheet.TechnicalSheetID;
                TechnicalSheetSizeDA.Delete(tc, oTechnicalSheetSize, EnumDBOperation.Delete, nUserID);
                foreach (TechnicalSheetSize oItem in oTechnicalSheetSizes)
                {                    
                    oItem.TechnicalSheetID = oTechnicalSheet.TechnicalSheetID;
                    reader = TechnicalSheetSizeDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                    NullHandler oReaderDetail = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oTechnicalSheetSize = new TechnicalSheetSize();
                        oTechnicalSheetSize.SizeCategoryID = oReaderDetail.GetInt32("SizeCategoryID");
                        oTempTechnicalSheetSizes.Add(oTechnicalSheetSize);
                    }
                    reader.Close();
                }
                #endregion
                #region Technical Sheet Shipment Part

                if (oTechnicalSheetShipments != null)
                {
                    foreach (TechnicalSheetShipment oItem in oTechnicalSheetShipments)
                    {
                        IDataReader readerdetail;
                        oItem.TechnicalSheetID = oTechnicalSheet.TechnicalSheetID;
                        if (oItem.TechnicalSheetShipmentID <= 0)
                        {
                            readerdetail = TechnicalSheetShipmentDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = TechnicalSheetShipmentDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderTSShipment = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            oTechnicalSheetShipment = new TechnicalSheetShipment();
                            oTechnicalSheetShipment = TechnicalSheetShipmentService.CreateObject(oReaderTSShipment);
                            oTempTechnicalSheetShipments.Add(oTechnicalSheetShipment);
                            sTechnicalSheetShipmentIDs = sTechnicalSheetShipmentIDs + oReaderTSShipment.GetString("TechnicalSheetShipmentID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sTechnicalSheetShipmentIDs.Length > 0)
                    {
                        sTechnicalSheetShipmentIDs = sTechnicalSheetShipmentIDs.Remove(sTechnicalSheetShipmentIDs.Length - 1, 1);
                    }
                    oTechnicalSheetShipment = new TechnicalSheetShipment();
                    oTechnicalSheetShipment.TechnicalSheetID = oTechnicalSheet.TechnicalSheetID;
                    TechnicalSheetShipmentDA.Delete(tc, oTechnicalSheetShipment, EnumDBOperation.Delete, nUserID, sTechnicalSheetShipmentIDs);

                }
                #endregion
                #region Style Yarn Part

                if (oOrderRecapYarns != null)
                {
                    foreach (OrderRecapYarn oItem in oOrderRecapYarns)
                    {
                        IDataReader readerdetail;
                        oItem.RefObjectID = oTechnicalSheet.TechnicalSheetID;
                        oItem.RefType = EnumRecapRefType.TechnicalSheet;
                        if (oItem.OrderRecapYarnID <= 0)
                        {
                            readerdetail = OrderRecapYarnDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = OrderRecapYarnDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderTSShipment = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sOrderRecapYarnIDs = sOrderRecapYarnIDs + oReaderTSShipment.GetString("OrderRecapYarnID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sOrderRecapYarnIDs.Length > 0)
                    {
                        sOrderRecapYarnIDs = sOrderRecapYarnIDs.Remove(sOrderRecapYarnIDs.Length - 1, 1);
                    }
                    OrderRecapYarn oOrderRecapYarn = new OrderRecapYarn();
                    oOrderRecapYarn.RefObjectID = oTechnicalSheet.TechnicalSheetID;
                    oOrderRecapYarn.RefType = EnumRecapRefType.TechnicalSheet;
                    OrderRecapYarnDA.Delete(tc, oOrderRecapYarn, EnumDBOperation.Delete, nUserID, sOrderRecapYarnIDs);

                }
                #endregion
                reader = TechnicalSheetDA.Get(tc, oTechnicalSheet.TechnicalSheetID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTechnicalSheet = new TechnicalSheet();
                    oTechnicalSheet = CreateObject(oReader);
                }
                reader.Close();        
                tc.End();
                oTechnicalSheet.TechnicalSheetShipments = oTechnicalSheetShipments;
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oTechnicalSheet.ErrorMessage = e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save TechnicalSheet. Because of " + e.Message, e);
                #endregion
            }
            return oTechnicalSheet;
        }

        public TechnicalSheet UpdateStatus(int nTechnicalSheetID, int nDevelopmentStatus, ApprovalRequest _oApprovalRequest, Int64 nUserID)
        {
            TransactionContext tc = null;
            TechnicalSheet oTechnicalSheet = new TechnicalSheet();
            oTechnicalSheet.ErrorMessage = "";
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.TechnicalSheet, EnumRoleOperationType.Approved);
                reader = TechnicalSheetDA.UpdateStatus(tc, nTechnicalSheetID, nDevelopmentStatus, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTechnicalSheet = new TechnicalSheet();
                    oTechnicalSheet = CreateObject(oReader);
                }
                reader.Close();
                if (nDevelopmentStatus == (int)EnumDevelopmentStatus.RequestForApproved)
                {
                    IDataReader ApprovalRequestreader;
                    ApprovalRequestreader = ApprovalRequestDA.InsertUpdate(tc, _oApprovalRequest, EnumDBOperation.Insert, nUserID);
                    if (ApprovalRequestreader.Read())
                    {

                    }
                    ApprovalRequestreader.Close();
                }


                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oTechnicalSheet.ErrorMessage = e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save TechnicalSheet. Because of " + e.Message, e);
                #endregion
            }
            return oTechnicalSheet;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                TechnicalSheet oTechnicalSheet = new TechnicalSheet();
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.TechnicalSheet, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "TechnicalSheet", id);
                oTechnicalSheet.TechnicalSheetID = id;
                TechnicalSheetDA.Delete(tc, oTechnicalSheet, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete TechnicalSheet. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }
                
        public TechnicalSheet Get(int id, Int64 nUserId)
        {
            TechnicalSheet oAccountHead = new TechnicalSheet();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = TechnicalSheetDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get TechnicalSheet", e);
                #endregion
            }

            return oAccountHead;
        }
        public TechnicalSheet GetByStyleNo(string StyleNo, Int64 nUserId)
        {
            TechnicalSheet oAccountHead = new TechnicalSheet();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = TechnicalSheetDA.GetByStyleNo(tc, StyleNo);
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
                throw new ServiceException("Failed to Get TechnicalSheet", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<TechnicalSheet> WaitForApproval(Int64 nUserID)
        {
            List<TechnicalSheet> oTechnicalSheet = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TechnicalSheetDA.WaitForApproval(tc);
                oTechnicalSheet = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TechnicalSheet", e);
                #endregion
            }

            return oTechnicalSheet;
        }

        public List<TechnicalSheet> BUWiseGets(int BUID, string DevelopmentStatus, Int64 nUserID)
        {
            List<TechnicalSheet> oTechnicalSheet = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TechnicalSheetDA.BUWiseGets(BUID, DevelopmentStatus, tc);
                oTechnicalSheet = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TechnicalSheet", e);
                #endregion
            }

            return oTechnicalSheet;
        }

        public List<TechnicalSheet> Gets(string sSQL, Int64 nUserID)
        {
            List<TechnicalSheet> oTechnicalSheet = null;
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = TechnicalSheetDA.Gets(tc, sSQL);
                oTechnicalSheet = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TechnicalSheet", e);
                #endregion
            }

            return oTechnicalSheet;
        }

        public List<TechnicalSheet> Gets_Report(int id, Int64 nUserID)
        {
            List<TechnicalSheet> oTechnicalSheet = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TechnicalSheetDA.Gets_Report(tc, id);
                oTechnicalSheet = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TechnicalSheet", e);
                #endregion
            }

            return oTechnicalSheet;
        }
        #endregion
    }
}
