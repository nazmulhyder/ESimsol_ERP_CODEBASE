using System;
using System.Data;
using ESimSol.BusinessObjects;
using ICS.Core;
using ICS.Core.Utility;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Reflection;
using System.Linq;

namespace ESimSol.Reports
{
    public class ESimSolSignature
    {
        public static PdfPTable GetSignature(float nTableWidth, object oDataObject, List<SignatureSetup> oSignatureSetups, float nBlankRowHeight)
        {
            iTextSharp.text.Font _oFontStyle;
            PdfPCell _oPdfPCell;
            int nSignatureCount = oSignatureSetups.Count;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            if (nSignatureCount <= 0)
            {
                #region Blank Table
                PdfPTable oPdfPTable = new PdfPTable(1);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;                
                oPdfPTable.SetWidths(new float[] { nTableWidth });

                if (nBlankRowHeight <= 0)
                {
                    nBlankRowHeight = 10f;
                }
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.FixedHeight = nBlankRowHeight; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                return oPdfPTable;
                #endregion
            }
            else
            {
                PdfPTable oPdfPTable = new PdfPTable(nSignatureCount);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

                int nColumnCount = -1;
                float[] columnArray = new float[nSignatureCount];
                foreach (SignatureSetup oItem in oSignatureSetups)
                {
                    nColumnCount++;
                    columnArray[nColumnCount] = nTableWidth / nSignatureCount;
                }
                oPdfPTable.SetWidths(columnArray);

                #region Blank Row
                if (nBlankRowHeight > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Colspan = nSignatureCount; _oPdfPCell.FixedHeight = nBlankRowHeight; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                }
                #endregion
                
                nColumnCount = 0;                
                foreach (SignatureSetup oItem in oSignatureSetups)
                {
                    if (nSignatureCount == 1)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(" " + GetDisplayData(oDataObject, oItem.DisplayDataColumn) + "\n__________________\n" + oItem.SignatureCaption, _oFontStyle));
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    }
                    else if (nSignatureCount == 2)
                    {
                        if (nColumnCount == 0)
                        {

                            _oPdfPCell = new PdfPCell(new Phrase(" "+GetDisplayData(oDataObject, oItem.DisplayDataColumn) + "\n__________________\n" + oItem.SignatureCaption, _oFontStyle));
                            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        }
                        else
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(GetDisplayData(oDataObject, oItem.DisplayDataColumn) + "\n__________________\n" + oItem.SignatureCaption+" ", _oFontStyle));
                            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        }
                    }
                    else
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(GetDisplayData(oDataObject, oItem.DisplayDataColumn) + "\n__________________\n" + oItem.SignatureCaption, _oFontStyle));
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    }
                    nColumnCount++;
                }
                return oPdfPTable;
            }
        }
        private static string GetDisplayData(object oDataObject, string sPropertyName)
        {
            string sDisplayData = "";
            if (sPropertyName == "")
            {
                return sDisplayData;
            }

            PropertyInfo prop = oDataObject.GetType().GetProperty(sPropertyName, BindingFlags.Public | BindingFlags.Instance);
            if (prop != null)
            {
                sDisplayData = Convert.ToString(prop.GetValue(oDataObject, null));
            }
            return sDisplayData;
        }

        //Saurove Dash
        //Note: In Approve head setup Ref Column Name shuld be ID (Sample:It is used in PurchaseOrder module of mithela)
        public static PdfPTable GetDynamicSignatureWithImageAndDesignation(object oDataObject, List<ApprovalHead> oApprovalHeads, float nBlankRowHeight, bool bIsShowDesignation, bool bIsShowSignatureImage)
        {
            iTextSharp.text.Font _oFontStyle;
            PdfPCell _oPdfPCell;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            if (oApprovalHeads.Count() <= 0)
            {
                #region Blank Table
                PdfPTable oPdfPTable = new PdfPTable(1);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 100 });

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.FixedHeight = nBlankRowHeight; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                return oPdfPTable;
                #endregion
            }
            else
            {
                oApprovalHeads = GetFinalList(oDataObject, oApprovalHeads);

                PdfPTable oPdfPTable = new PdfPTable((oApprovalHeads.Count()*2)+1);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

                #region Set Widths
                int nColumnCount = 0;
                if (oApprovalHeads.Count() == 1)
                {
                    oPdfPTable.SetWidths(new float[] { 35f, 30f, 35f });
                }
                else if (oApprovalHeads.Count() == 2)
                {
                    oPdfPTable.SetWidths(new float[] { 5f, 25f, 41f, 25f, 5f });
                }
                else if (oApprovalHeads.Count() == 3)
                {
                    oPdfPTable.SetWidths(new float[] { 5f, 24f, 10f, 24f, 10f, 24f, 5f });
                }
                else
                {
                    float[] columnArray = new float[(oApprovalHeads.Count() * 2) + 1];
                    columnArray[nColumnCount++] = 5f;
                    foreach (ApprovalHead oItem in oApprovalHeads)
                    {
                        columnArray[nColumnCount++] = 90 / oApprovalHeads.Count();
                        columnArray[nColumnCount++] = 5f;
                    }
                    oPdfPTable.SetWidths(columnArray);
                }
                #endregion

                #region Blank Row
                if (nBlankRowHeight > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Colspan = (oApprovalHeads.Count()*2)+1; _oPdfPCell.FixedHeight = nBlankRowHeight; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                }
                #endregion

                iTextSharp.text.Image _oImag;
                if (bIsShowSignatureImage)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    foreach (ApprovalHead oItem in oApprovalHeads)
                    {
                        if (oItem.SignatureImage == null)
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Border = 0;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);                    
                        }
                        else
                        {
                            _oImag = iTextSharp.text.Image.GetInstance(oItem.SignatureImage, System.Drawing.Imaging.ImageFormat.Jpeg);
                            _oImag.ScaleAbsolute(45f, 35f); _oPdfPCell = new PdfPCell(_oImag); _oPdfPCell.Border = 0;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            oPdfPTable.AddCell(_oPdfPCell);
                        }

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Border = 0;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    }
                }

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                foreach (ApprovalHead oItem in oApprovalHeads)
                {
                    Phrase oPhrase = new Phrase();
                    Chunk oChunk2 = new Chunk(oItem.RefColName, _oFontStyle);
                    oPhrase.Add(oChunk2);
                    if (bIsShowDesignation)
                    {
                        _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL);
                        if (!string.IsNullOrEmpty(oItem.DesignationName))
                        {
                            Chunk oChunk3 = new Chunk("\n" + oItem.DesignationName, _oFontStyle);
                            oPhrase.Add(oChunk3);
                        }
                        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                    }
                    _oPdfPCell = new PdfPCell(oPhrase); _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                }
                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                foreach (ApprovalHead oItem in oApprovalHeads)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Name, _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 1;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                }

                return oPdfPTable;
            }
        }

        public static List<ApprovalHead> GetFinalList(object oDataObject, List<ApprovalHead> oApprovalHeads)
        {
            foreach (ApprovalHead oAH in oApprovalHeads)
            {
                oAH.RefID = Convert.ToInt32(GetDBInfo(oDataObject, oAH.RefColName));
            }

            List<User> oUsers = new List<User>();
            oUsers = ESimSol.BusinessObjects.User.GetsBySql("SELECT * FROM Users WHERE UserID IN (" + string.Join(",", oApprovalHeads.Select(x => x.RefID)) + ")", -9);
            if (oUsers.Count > 0)
            {
                List<Employee> oEmployees = new List<Employee>();
                string sSQL = "SELECT EE.DesignationName, UU.UserID AS EmployeeID, UU.UserName AS Name FROM Users AS UU INNER JOIN View_Employee AS EE ON EE.EmployeeID=UU.EmployeeID WHERE EE.EmployeeID IN (" + string.Join(",", oUsers.Select(x => x.EmployeeID)) + ")";
                oEmployees = Employee.Gets(sSQL, -9);
                List<AttachDocument> oAttachDocuments = new List<AttachDocument>();
                //sSQL = "SELECT EE.DesignationName, UU.UserID AS EmployeeID, UU.UserName AS Name FROM Users AS UU INNER JOIN View_Employee AS EE ON EE.EmployeeID=UU.EmployeeID WHERE EE.EmployeeID IN (" + string.Join(",", oUsers.Select(x => x.EmployeeID)) + ")";
                oAttachDocuments = AttachDocument.Gets("SELECT * FROM AttachDocument WHERE AttachDocumentID in (SELECT HH.AttachDocumentID FROM AttachDocument AS HH WHERE HH.RefType=" + (int)EnumAttachRefType.UserSignature + " AND HH.RefID IN (" + string.Join(",", oUsers.Select(x => x.UserID)) + "))", -9);
                foreach (ApprovalHead oAH in oApprovalHeads)
                {
                    oAH.RefColName = oUsers.Where(x => x.UserID == oAH.RefID).Select(x => x.UserName).FirstOrDefault();
                    //Here EmployeeID === UserID
                    //oAH.RefColName = oEmployees.Where(x => x.EmployeeID == oAH.RefID).Select(x => x.Name).FirstOrDefault();                    
                    oAH.DesignationName = oEmployees.Where(x => x.EmployeeID == oAH.RefID).Select(x => x.DesignationName).FirstOrDefault();
                }
                foreach (ApprovalHead oAH in oApprovalHeads)
                {
                    oAH.SignatureImage = ApprovalHead.GetImage(oAttachDocuments.Where(x => x.RefID == oAH.RefID).FirstOrDefault());
                }
            }
            else
            {
                foreach (ApprovalHead oAH in oApprovalHeads)
                {
                    oAH.RefColName = "";
                }
            }
            

            return oApprovalHeads;
        }

        public static string GetDBInfo(object obj, string propName)
        {
            try
            {
                return Convert.ToString(obj.GetType().GetProperty(propName).GetValue(obj, null));
            }
            catch
            {
                return "0";
            }
        }



    }
}
