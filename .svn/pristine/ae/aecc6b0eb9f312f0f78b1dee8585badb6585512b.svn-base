using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;

using System.Web.Script.Serialization;
using ESimSol.Reports;
using ICS.Core.Utility;
using System.Drawing.Printing;
//using Microsoft.Office.Interop.Word;
using System.Reflection;


namespace ESimSolFinancial.Controllers
{
    public class BankController : Controller
    {
        #region Declaration
        Bank _oBank = new Bank();
        List<Bank> _oBanks = new List<Bank>();
        string _sErrorMessage = "";
        #endregion

        #region Functions


        #region New Code
        public ActionResult ViewBanks(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            //this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByDBObjectAndUser("'Bank', 'BankBranch'", (int)Session[SessionInfo.currentUserID], ((User)Session[SessionInfo.CurrentUser]).UserID));

            _oBanks = new List<Bank>();
            _oBanks = Bank.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.AccountTypes = Enum.GetValues(typeof(EnumBankAccountType)).Cast<EnumBankAccountType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            return View(_oBanks);
        }
        public ActionResult ViewBank(int id, double ts)
        {
            _oBank = new Bank();
            if (id > 0)
            {
                _oBank = _oBank.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            return View(_oBank);
        }

        public ActionResult ViewBankBranchs(int id, double ts)
        {
            _oBank = new Bank();
            if (id > 0)
            {
                _oBank = _oBank.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oBank.BankBranchs = BankBranch.GetsByBank(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            return View(_oBank);
        }

        public ActionResult ViewBankBranch(int id, int nid, double ts)
        {
            BankBranch oBankBranch = new BankBranch();
            List<BankBranchDept> oBankBranchDepts = new List<BankBranchDept>();
            if (id > 0)
            {
                oBankBranch = BankBranch.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBankBranch.BankBranchBUs = BankBranchBU.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
            {
                oBankBranch.BankID = nid;
            }
            List<EnumObject> oEnumOperationalDepts = EnumObject.jGets(typeof(EnumOperationalDept));
            foreach (EnumObject oItem in oEnumOperationalDepts)
            {
                BankBranchDept oBankBranchDept = new BankBranchDept();
                oBankBranchDept.BankBranchID = id;
                oBankBranchDept.OperationalDept = (EnumOperationalDept)oItem.id;
                oBankBranchDept.OperationalDeptInInt = oItem.id;
                oBankBranchDept.OperationalDeptName = oItem.Value;
                if (oBankBranchDept.OperationalDeptInInt > 0)
                {
                    oBankBranchDepts.Add(oBankBranchDept);
                }
            }
            ViewBag.OperationalDepts = oBankBranchDepts;
            ViewBag.BankBranchDepts = BankBranchDept.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            
            return View(oBankBranch);
        }
        public ActionResult ViewBankAccounts(int id)
        {

            BankBranch oBankBranch = new BankBranch();
            if (id > 0)
            {
                oBankBranch = BankBranch.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBankBranch.BankAccounts = BankAccount.GetsByBankBranch(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            return View(oBankBranch);
        }

        public ActionResult ViewBankAccount(int id, int nid)
        {
            BankAccount oBankAccount = new BankAccount();
            if (id > 0)
            {
                oBankAccount = oBankAccount.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
            {
                BankBranch oBankBranch = new BankBranch();
                oBankBranch = BankBranch.Get(nid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBankAccount.BankID = oBankBranch.BankID;
                oBankAccount.BankBranchID = oBankBranch.BankBranchID;
            }
            return View(oBankAccount);
        }
        public ActionResult ViewBankPersonnels(int id)
        {
            BankBranch oBankBranch = new BankBranch();
            if (id > 0)
            {
                oBankBranch = BankBranch.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBankBranch.BankPersonnels = BankPersonnel.GetsByBankBranch(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            return View(oBankBranch);
        }

        public ActionResult ViewBankPersonnel(int id, int nid)
        {
            BankPersonnel oBankPersonnel = new BankPersonnel();
            if (id > 0)
            {
                oBankPersonnel = oBankPersonnel.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
            {
                BankBranch oBankBranch = new BankBranch();
                oBankBranch = BankBranch.Get(nid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBankPersonnel.BankID = oBankBranch.BankID;
                oBankPersonnel.BankBranchID = oBankBranch.BankBranchID;
            }
            return View(oBankPersonnel);
        }

        [HttpPost]
        public JsonResult Save(Bank oBank)
        {
            _oBank = new Bank();
            try
            {
                _oBank = oBank;
                _oBank = _oBank.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oBank = new Bank();
                _oBank.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBank);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(Bank oBank)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oBank.Delete(oBank.BankID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetPIAddress(Bank oBank)
        {
            string ip = "";
            try
            {
                ip = Request.UserHostAddress;
            }
            catch (Exception ex)
            {
                ip = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(ip);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Get(Bank oBank)
        {
            _oBank = new Bank();
            try
            {
                if (oBank.BankID > 0)
                {
                    _oBank = _oBank.Get(oBank.BankID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                _oBank = new Bank();
                _oBank.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBank);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Gets(Bank oBank)
        {
            string sSQL = "";
            oBank.Name = oBank.Name == null ? "" : oBank.Name;
            sSQL = "SELECT * FROM View_Bank WHERE Name LIKE '%" + oBank.Name + "%'";
            _oBanks = new List<Bank>();
            _oBanks = Bank.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);


            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBanks);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

        #endregion



        #region Old Code

        private bool ValidateInput(Bank oBank)
        {
            if (string.IsNullOrEmpty(oBank.Name))
            {
                _sErrorMessage = "Please enter Bank Name";
                return false;
            }
            if (string.IsNullOrEmpty(oBank.ShortName))
            {
                _sErrorMessage = "Please insert Bank Initial";
                return false;
            }
            return true;
        }
        #endregion


        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                Bank oBank = new Bank();
                sFeedBackMessage = oBank.Delete(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Gets()
        {
            List<Bank> oBanks = new List<Bank>();
            Bank oBank = new Bank();
            oBank.Name = "-- Select Bank --";
            oBanks.Add(oBank);
            oBanks.AddRange(Bank.Gets(((User)Session[SessionInfo.CurrentUser]).UserID));

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oBanks);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        //public ActionResult PrintBanks()
        //{
        //    //_oBank = new Bank();
        //    //_oBank.Banks = Bank.Gets((int)Session[SessionInfo.currentUserID]); //new JavaScriptSerializer().Deserialize<List<Bank>>(DataCollection["txtBankCollectionList"]);
        //    //Company oCompany = new Company();
        //    //oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    //oCompany.CompanyLogo = GetCompanyLogo(oCompany);
        //    //_oBank.Company = oCompany;

        //    //string Messge = "Bank List";
        //    //rptBanks oReport = new rptBanks();
        //    //byte[] abytes = oReport.PrepareReport(_oBank, Messge);
        //    //return File(abytes, "application/pdf");

        //    //string filePath = Server.MapPath("~/Content/test.pdf");
        //    //new PrintingExample(filePath);

        //    Document document = PrepareDocument();            
        //    var filePath = Server.MapPath("../Content/test.doc");            
        //    Save(document, filePath);


        //    byte[] bytes = System.IO.File.ReadAllBytes(filePath);
        //    return new FileStreamResult(new MemoryStream(bytes), "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
        //}

        //private void Save(Document oDoc, object filePath)
        //{
        //    object missing = Missing.Value;
        //    oDoc.SaveAs(ref filePath, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);

        //    object saveChanges = WdSaveOptions.wdSaveChanges;
        //    object originalFormat = WdOriginalFormat.wdWordDocument;            
        //    object routeDocument = true;
        //    oDoc.Close(ref saveChanges, ref originalFormat, ref routeDocument);
        //}

        //public Document PrepareDocument()
        //{
        //    object missing = Missing.Value;

        //    //Start Word and create a new document.
        //    Application application;
        //    Document document;
        //    application = new Application { Visible = false };
        //    document = application.Documents.Add(ref missing, ref missing, ref missing, ref missing);
        //    document.PageSetup.PaperSize = WdPaperSize.wdPaperA4;
        //    document.PageSetup.LeftMargin = application.InchesToPoints(0.5f);
        //    document.PageSetup.RightMargin= application.InchesToPoints(0.5f);
        //    document.PageSetup.TopMargin = application.InchesToPoints(0.5f);
        //    document.PageSetup.BottomMargin = application.InchesToPoints(0.5f);
        //    Fill(document);
        //    application.ActiveDocument.Protect(Microsoft.Office.Interop.Word.WdProtectionType.wdAllowOnlyReading, ref missing, ref missing, ref missing, ref missing);
        //    return document;
        //}
        
        //private void Fill(Document document)
        //{
        //    object missing = Missing.Value;
        //    object endOfDoc = "\\endofdoc"; /* \endofdoc is a predefined bookmark */


        //    Paragraph oPara1;
        //    oPara1 = document.Content.Paragraphs.Add(ref missing);
        //    oPara1.Range.Text = "Heading 1";
        //    oPara1.Range.Font.Bold = 1;
        //    oPara1.Format.SpaceAfter = 24;    //24 pt spacing after paragraph.
        //    oPara1.Range.InsertParagraphAfter();


        //    //Insert a paragraph at the end of the document.
        //    Paragraph oPara2;
        //    object oRng = document.Bookmarks.get_Item(ref endOfDoc).Range;
        //    oPara2 = document.Content.Paragraphs.Add(ref oRng);
        //    oPara2.Range.Text = "বাংলাদেশ কুমিল্লা প্রারম্ভিক";
        //    oPara2.Format.SpaceAfter = 6;
        //    oPara2.Range.InsertParagraphAfter();

        //    //Insert another paragraph.
        //    Paragraph oPara3;
        //    oRng = document.Bookmarks.get_Item(ref endOfDoc).Range;
        //    oPara3 = document.Content.Paragraphs.Add(ref oRng);
        //    oPara3.Range.Text = "আলোচিত পুলিশ সুপার বাবুল আক্তার স্বেচ্ছায় পদত্যাগপত্র জমা দেননি বলে দাবি করেছেন তাঁর শ্বশুর সাবেক পুলিশ কর্মকর্তা মোশাররফ হোসেন। গতকাল মঙ্গলবার তিনি অভিযোগ করেন, বাবুল চাইলেও তাঁকে কাজে যোগ দিতে দেওয়া হয়নি। এদিকে বাবুলের পদত্যাগপত্র নিয়ে এত দিন পুলিশের পক্ষ থেকে রাখঢাক করা হলেও স্বরাষ্ট্রমন্ত্রী আসাদুজ্জামান খান কামাল খান প্রথম আলোকে বলেছেন, বাবুল আক্তারের পদত্যাগপত্র এসেছে। এর আইনগত প্রক্রিয়া শুরু হয়েছে। নিয়ম অনুসারে যা যা করার দরকার, করা হচ্ছে। এ ব্যাপারে জানতে বাবুল আক্তারের সঙ্গে যোগাযোগ করা হলে তিনি কোনো মন্তব্য করতে রাজি হননি। তাঁর শ্বশুর মোশাররফ হোসেন বলেন, বাবুল পদত্যাগ করেছেন বলে যা বলা হচ্ছে, তা ঠিক নয়। বাবুল চাকরি ছাড়তে চাননি। তিনি বলেন, পুলিশের একজন পদস্থ কর্মকর্তা কীভাবে, কোথায় বসে, কার মাধ্যমে পদত্যাগপত্র দিচ্ছেন, সেটা একটা বড় ব্যাপার। জিজ্ঞাসাবাদের নামে বাবুলকে নেওয়া হয়েছিল ২৪ জুন রাতে। ওই দিন তাঁর কাছ থেকে পদত্যাগপত্রে সই নেওয়া হয়। সেটা ছিল শুক্রবার। মোশাররফ হোসেনের প্রশ্ন, ছুটির দিনে একজন কর্মকর্তা কী করে পদত্যাগ করবেন? তিনি বলেন, বাবুল যদি পদত্যাগ করেন, তাহলে এত দিন সেটা কোথায় ছিল? কারণ, পুলিশের পক্ষ থেকে বারবার বলা হচ্ছিল, বাবুল চাকরিতেই আছেন। পুলিশ সদর দপ্তরের একজন কর্মকর্তা প্রথম আলোকে বলেন, গত ৫ জুন স্ত্রী মাহমুদা খানম মিতু খুনের পর ৩ আগস্ট বাবুল আক্তার তাঁর কর্মস্থল পুলিশ সদর দপ্তরে গিয়ে লিখিতভাবে কাজে যোগ দিতে চান। কিন্তু দায়িত্বপ্রাপ্ত কর্মকর্তারা তাঁকে জানিয়ে দেন, তাঁকে আর কাজে যোগদান করতে দেওয়া সম্ভব নয়। পরদিন ৪ আগস্ট বাবুল লিখিতভাবে যোগদানপত্র জমা দেন। এতে তিনি বলেন, স্ত্রী খুন হওয়ার পর দুই সন্তানের দেখাশোনার জন্য কর্মকর্তাদের পরামর্শমতো তিনি শ্বশুরবাড়িতে অবস্থান করছিলেন। সেখান থেকে দুই সন্তানকে নিয়মিত চিকিৎসকের কাছেও নেওয়া হচ্ছে। অনুপস্থিতির সময়টা ছুটি হিসেবে নিয়ে তাঁকে যোগ দেওয়ার সুযোগ চান বাবুল। পুলিশ সদর দপ্তরে এই আবেদনের ব্যাপারে দায়িত্বশীল কোনো কর্মকর্তা কোনো মতামত দেননি বলে জানা গেছে। বাবুলের শ্বশুর বলেন, চাকরি চলে গেলে বাবুল নিরাপত্তাহীন হয়ে যাবেন। তিনি বিভিন্ন ব্যক্তি ও গোষ্ঠীর আক্রোশের শিকার হতে পারেন। চট্টগ্রাম থেকে ঢাকায় পুলিশ সদর দপ্তরে বদলি হয়ে আসার দুই দিন পর গত ৫ জুন সকালে চট্টগ্রামের ও আর নিজাম রোডে সন্ত্রাসীদের গুলিতে নিহত হন বাবুলের স্ত্রী মাহমুদা খানম। এ ঘটনায় বাবুল বাদী হয়ে অজ্ঞাতপরিচয় তিনজনের বিরুদ্ধে পাঁচলাইশ থানায় মামলা করেন। শুরুতে জঙ্গিদের সন্দেহ করা হয়েছিল। কিন্তু পরে কয়েকজনকে গ্রেপ্তার করে জিজ্ঞাসাবাদ করা হলে পুলিশের ধারণা পাল্টে যায়। এরপর জিজ্ঞাসাবাদের কথা বলে গত ২৪ জুন গভীর রাতে খিলগাঁও ভূঁইয়াপাড়ার শ্বশুরবাড়ি থেকে বাবুলকে ডিবি কার্যালয়ে নেওয়া হয়। ১৫ ঘণ্টা জিজ্ঞাসাবাদের পর আবার তাঁকে শ্বশুরবাড়িতে পৌঁছে দেওয়া হয়। ওই জিজ্ঞাসাবাদের সময় পদত্যাগপত্রে সই করেন বাবুল। বেশ কিছুদিন সেই পদত্যাগপত্র পুলিশ সদর দপ্তরে থাকার পর কিছুদিন আগে মন্ত্রণালয়ে পাঠানো হয়।";
        //    oPara3.Alignment = WdParagraphAlignment.wdAlignParagraphJustify;
        //    oPara3.Range.Font.Bold = 0;
        //    oPara3.Format.SpaceAfter = 24;
        //    oPara3.Range.InsertParagraphAfter();

        //    float aaa = oPara3.Range.get_Information(Microsoft.Office.Interop.Word.WdInformation.wdVerticalPositionRelativeToPage);

        //    float nPageHeight = document.PageSetup.PageHeight;
        //    float n =  document.PageSetup.LinesPage;

        //    document.Words.Last.InsertBreak(Microsoft.Office.Interop.Word.WdBreakType.wdPageBreak);

        //    //Insert another paragraph.
        //    Paragraph oPara4;
        //    oRng = document.Bookmarks.get_Item(ref endOfDoc).Range;
        //    oPara4 = document.Content.Paragraphs.Add(ref oRng);
        //    oPara4.Range.Text = "This is a sentence of normal text. Now here is a table:";
        //    oPara4.Range.Font.Bold = 0;
        //    oPara4.Format.SpaceAfter = 24;
        //    oPara4.Range.InsertParagraphAfter();


        //    //Insert a 3 x 5 table, fill it with data, and make the first row
        //    //bold and italic.
        //    Table oTable;
        //    Range wrdRng = document.Bookmarks.get_Item(ref endOfDoc).Range;
        //    oTable = document.Tables.Add(wrdRng, 3, 5, ref missing, ref missing);
        //    oTable.Range.ParagraphFormat.SpaceAfter = 6;          
        //    int r, c;
        //    string strText;
        //    for (r = 1; r <= 3; r++)
        //        for (c = 1; c <= 5; c++)
        //        {
        //            strText = "r" + r + "c" + c;
        //            oTable.Cell(r, c).Range.Text = strText;
        //        }
        //    oTable.Rows[1].Range.Font.Bold = 1;
        //    oTable.Rows[1].Range.Font.Italic = 1;
        //}

    }
}
