using System;
using System.IO;
using System.Data;
using System.Xml;
using System.Collections;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace ICS.Core.Utility
{
    public class Global
    {
        
        
        //for WCF Server Session
        private static Guid _CurrentSession = Guid.Empty;
        public static Guid CurrentSession
        {
            get { return _CurrentSession; }
            set { _CurrentSession = value; }
        }

        //current User
        private static object _oCurrentUser = null;
        public static object CurrentUser
        {
            get { return _oCurrentUser; }
        }

        //set current user
        public static void SetCurrentUser(object oCurrentUser)
        {
            _oCurrentUser = oCurrentUser;
        }
        //constaints
        private static string _sDBDateTime = " getdate() ";
        public static string DBDateTime
        {
            get { return _sDBDateTime; }
        }
        public static string SessionParamSetMessage
        {
            get { return "Successful"; }
        }

        public static string DeleteMessage
        {
            get { return "Deleted"; }
        }
        public static string SuccessMessage
        {
            get { return "Success"; }
        }
        public static string SystemSendingEmailAddress
        {
            get { return "esimsol.service@gmail.com"; }
        }
        public static string SystemSendingEmailPassword
        {
            get { return "ics@2007"; }
        }
        public static string SystemSendingEmailDisplayName
        {
            get { return "Infocrat Solutions Ltd."; }
        }
        public static string SystemSendingEmailHost
        {
            get { return "smtp.gmail.com"; }
        }
        public static Regex StringPattern
        {
            get {
                return new Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);
            }
        }
        //for A4 Size paper
        private static double _nLabPageUsed = 0;
        public static double LabPageUsed
        {
            get { return _nLabPageUsed; }
            set { _nLabPageUsed = value; }
        }

        private static object _oWorkingUnits = null;
        public static object WorkingUnits
        {
            get { return _oWorkingUnits; }
            set { _oWorkingUnits = value; }
        }

        private static object _oWorkingUnitsAll = null;
        public static object WorkingUnitsAll
        {
            get { return _oWorkingUnitsAll; }
            set { _oWorkingUnitsAll = value; }
        }

        private static object _oOperationLocation = null;
        public static object OperationLocation
        {
            get { return _oOperationLocation; }
            set { _oOperationLocation = value; }
        }

        private static bool _bBarCodeFontInstalled = false;
        public static bool BarCodeFontInstalled
        {
            get { return _bBarCodeFontInstalled; }
            set { _bBarCodeFontInstalled = value; }
        }

        #region GetInt
        public static int GetInt(string sMask, char cSplitChar)
        {
            string str = sMask;
            string[] sTokens = str.Split(cSplitChar);
            str = "";
            foreach (string sItem in sTokens)
            {
                str = str + sItem;
            }
            if (str.Length < 1) return 0;
            return Convert.ToInt32(str);
        }
        #endregion
        
        #region Declartion/Constructor
        private static HierarchyTree _oMenuTree;
        private static NodeItem _oSelectedMenu;
        private static HierarchyTree _oMenuTreeWeb;
        private static NodeItem _oSelectedMenuWeb;
        public static int _nItemNotValid = -13;
        public Global() { }

        #region Excel Related Functions
        /// <summary>
        /// Added by fahim0abir on date 15 FEB 2016
        /// for calculating the column name of an excel sheet i.e. "A","B","AB","XFD" etc.
        /// </summary>
        /// <param name="nColumn"></param>
        /// <returns>Excel Sheet Column Name in String</returns>
        public static string GetExcelColumnName(int nColumn)
        {
            int nRemainder = nColumn;
            string sColumn = String.Empty;
            int nMod;

            while (nRemainder > 0)
            {
                nMod = (nRemainder - 1) % 26;
                sColumn = Convert.ToChar(65 + nMod).ToString() + sColumn;
                nRemainder = (int)((nRemainder - nMod) / 26);
            }

            return sColumn;
        }
        /// <summary>
        /// Added by fahim0abir on date 15 FEB 2016
        /// for calculating the cell name of an excel sheet i.e. "A1","B45","AB78","XFD222" etc.
        /// </summary>
        /// <param name="nColumn"></param>
        /// <param name="nRow"></param>
        /// <returns>Excel Sheet Cell Name in String</returns>
        public static string GetExcelCellName(int nRow, int nColumn)
        {
            int nRemainder = nColumn;
            string sColumn = String.Empty;
            int nMod;

            while (nRemainder > 0)
            {
                nMod = (nRemainder - 1) % 26;
                sColumn = Convert.ToChar(65 + nMod).ToString() + sColumn;
                nRemainder = (int)((nRemainder - nMod) / 26);
            }

            return sColumn + nRow.ToString();
        }
        #endregion
        #endregion

        #region Menu Display Items
        public static HierarchyTree MenuTree()
        {
            return _oMenuTree;
        }
        public static NodeItem SelectedMenu
        {
            get { return _oSelectedMenu; }
            set { _oSelectedMenu = value; }
        }
        /// <summary>
        /// Jakaria
        /// </summary>
        #region Menu Decleration
        public static void BuildMenuTree()
        {
            _oMenuTree = new HierarchyTree();

            HierarchyTree oTemp = new HierarchyTree();
            oTemp.BeginGroup("M0", "Administrative Privilege", true);
            oTemp.MenuItem("M0.1", "Organizational Information", true);
            oTemp.MenuItem("M0.2", "User Management", true);
            oTemp.MenuItem("M0.4", "Buyer Factory", true);
            oTemp.EndGroup();

            oTemp.BeginGroup("M1", "Basic Entry", true);
            oTemp.MenuItem("M1.4", "Employee", true);
            oTemp.MenuItem("M1.9", "Bank", true);
            oTemp.MenuItem("M1.10", "WorkingUnit Setup", true);
            oTemp.MenuItem("M1.11", "Yarn Product Name", true);
            oTemp.MenuItem("M1.12", "Yarn Name Mapping", true);
            oTemp.MenuItem("M1.13", "Yarn Format Configure", true);
            oTemp.MenuItem("M1.14", "Currency", true);

            oTemp.EndGroup();

            oTemp.BeginGroup("M5", "Agent Operation", true);
            oTemp.MenuItem("M5.1", "Agent", true);
            oTemp.MenuItem("M5.2", "BL Lot Approval", true);
            oTemp.MenuItem("M5.3", "Agent Lots", true);
            oTemp.MenuItem("M5.4", "Agent Ledger", true);
            oTemp.MenuItem("M5.5", "Agent Challan", true);
            oTemp.MenuItem("M5.6", "Agent Bill", true);
            oTemp.MenuItem("M5.7", "Agent Lot Information", true);
            oTemp.MenuItem("M5.8", "Agent Challan For Disburse", true);
            oTemp.MenuItem("M5.9", "Agent Challan For Receive", true);


            oTemp.EndGroup();

            oTemp.BeginGroup("M4", "Utilities", true);
            oTemp.MenuItem("M4.1", "UpLoad EWYDLs", true);
            oTemp.MenuItem("M4.13", "Debit Notes", true);
            oTemp.MenuItem("M4.20", "Job Order Memos(Production Order Sheet)", true);
            oTemp.MenuItem("M4.21", "View Order", true);
            oTemp.MenuItem("M4.22", "Search Samples", true);
            oTemp.MenuItem("M4.23", "Debit Notes For Admin", true);
            oTemp.MenuItem("M4.26", "Job Authorization", true);
            oTemp.MenuItem("M4.27", "Search Invoice", true);
            oTemp.MenuItem("M4.28", "Manage Contrctors", true);
            oTemp.MenuItem("M4.29", "Manage Contact Personnels", true);
            oTemp.MenuItem("M4.30", "Manage Yarn [Route Sheet wise]", true);
            oTemp.MenuItem("M4.32", "Transfer to Available store[From Finishing]", true);
            oTemp.MenuItem("M4.33", "Job Status(prodution and delivery-ColorWise)", true);
            oTemp.MenuItem("M4.34", "Import BLs", true);
            oTemp.MenuItem("M4.35", "Search Sample", true);
            oTemp.MenuItem("M4.36", "Search Order Details (Extended)", true);
            oTemp.MenuItem("M4.37", "PTU Distribution Manage", true);
            oTemp.MenuItem("M4.38", "Production Grace History", true);
            oTemp.MenuItem("M4.39", "PTU Extended Search", true);

            oTemp.EndGroup();

            oTemp.BeginGroup("M80", "Account", true);
            oTemp.MenuItem("M80.1", "Cost Head", true);
            oTemp.MenuItem("M80.2", "Commission Payment", true);
            oTemp.MenuItem("M80.3", "Commission Log", true);
            oTemp.MenuItem("M80.4", "PI Commission", true);
            oTemp.MenuItem("M80.5", "Export LC Wise Commission", true);
            oTemp.BeginGroup("M80.50", "Reports & View", true);
            oTemp.MenuItem("M80.50.1", "Production Report with value", true);
            oTemp.MenuItem("M80.50.2", "Production Report with value(simple)", true);
            oTemp.MenuItem("M80.50.4", "Delivery Report ->Accounts(Dyed yarn)", true);
            oTemp.MenuItem("M80.50.5", "Dye Chemical Stock Report (Accounts)", true);
            oTemp.MenuItem("M80.50.6", "Inventory Report-Yarn (Inventory transaction)", true);
            oTemp.MenuItem("M80.50.7", "Inventory Report (Chemical/Dye)", true);
            oTemp.MenuItem("M80.50.9", "Inventory Report (Product Wise)", true);
            oTemp.MenuItem("M80.50.10", "Inventory Report -Account", true);

            oTemp.EndGroup();
            oTemp.EndGroup();

            #region Trade Finance
            oTemp.BeginGroup("M70", "Trade Finance", true);

            oTemp.BeginGroup("M70.1", "Operational Basics", true);
            oTemp.MenuItem("M70.1.1", "Cost Heads", true);
            oTemp.MenuItem("M70.1.2", "Commercial Doc", true);
            oTemp.MenuItem("M70.1.3", "Head of Expendecer", true);

            oTemp.EndGroup();

            #region Import LC
            oTemp.BeginGroup("M70.50", "Import", true);
            oTemp.MenuItem("M70.50.1", "Purchase Orders", true);
            oTemp.MenuItem("M70.50.2", "Import LC", true);
            oTemp.MenuItem("M70.50.3", "Invoices(import bulk raw yarn/dye/chemical)", true);
            oTemp.MenuItem("M70.50.4", "Invoices", true);
            oTemp.MenuItem("M70.50.5", "Invoices(For Production)", true);
            oTemp.MenuItem("M70.50.6", "Invoice Details", true);
            #region Reports & Views
            oTemp.BeginGroup("M70.53", "Report & Views", true);
            oTemp.MenuItem("M70.53.1", "Search Invoice ", true);
            oTemp.MenuItem("M70.53.2", "DSS Report ", true);
            oTemp.EndGroup();
            #endregion
            oTemp.EndGroup();
            #endregion

            #region Export
            oTemp.BeginGroup("M70.51", "Export", true);
            oTemp.MenuItem("M70.51.1", "Master LC", true);
            oTemp.MenuItem("M70.51.2", "Export LC", true);
            oTemp.MenuItem("M70.51.3", "Export Bills(Admin)", true);
            oTemp.MenuItem("M70.51.4", "Export Bills(For Bank Operation )", true);
            oTemp.MenuItem("M70.51.5", "Export Bills(For Payment )", true);
            oTemp.MenuItem("M70.51.6", "Export Bills(For Acceptance Doc Received)", true);
            oTemp.MenuItem("M70.51.7", "Export Bills(MKT Tracking)", true);
            oTemp.MenuItem("M70.51.8", "Non LC Payment", true);
            #region Reports & Views
            oTemp.BeginGroup("M70.54", "Report & Views", true);
            oTemp.MenuItem("M70.54.1", "Export LC Reports", true);
            oTemp.MenuItem("M70.54.2", "DSS Report For Export", true);
            oTemp.EndGroup();
            #endregion

            oTemp.EndGroup();
            #endregion
            #region Bill Operation(in Bank)
            oTemp.BeginGroup("M70.52", "Bill Operation(in Bank)", true);
            oTemp.MenuItem("M70.52.1", "Installment", true);
            oTemp.MenuItem("M70.52.2", "Interest Rate", true);
            oTemp.MenuItem("M70.52.3", "LDBC", true);
            oTemp.MenuItem("M70.52.4", "LDBP", true);
            oTemp.MenuItem("M70.52.5", "Loan Report", true);
            oTemp.MenuItem("M70.52.6", "LTR", true);
            oTemp.MenuItem("M70.52.7", "PAD", true);
            oTemp.MenuItem("M70.52.8", "Process Interest", true);
            oTemp.MenuItem("M70.52.9", "Reconcile With Cash", true);
            oTemp.EndGroup();
            #endregion

            oTemp.EndGroup();

            #endregion

            #region Sales Process
            oTemp.BeginGroup("M51", "Sales Process", true);

            #region Basic Entry
            oTemp.BeginGroup("M51.1", "Basic Entry", true);
            oTemp.MenuItem("M51.1.1", "Buyer/Factory[M1.5]", true);
            oTemp.EndGroup();
            #endregion

            #region Operations
            oTemp.BeginGroup("M51.2", "Operations", true);
            oTemp.BeginGroup("M51.2.1", "Lab-dip order", true);
            oTemp.MenuItem("M51.2.1.1", "Issue lab-dip order memo", true);
            oTemp.MenuItem("M51.2.1.2", "Accept lab-dip order memo", true);
            oTemp.MenuItem("M51.2.1.3", "Lab-dip orders priority", true);
            oTemp.MenuItem("M51.2.1.4", "Lab-dip orders Currection", true);
            oTemp.EndGroup();

            oTemp.BeginGroup("M51.2.2", "Sample order", true);
            oTemp.MenuItem("M51.2.2.1", "Issue sample order memo", true);
            oTemp.MenuItem("M51.2.2.2", "Accept sample order memo", true);
            oTemp.EndGroup();

            oTemp.EndGroup();
            #endregion

            #region Lab-dip & Sample order management
            oTemp.BeginGroup("M51.3", "Lab-dip & Sample order for management", true);
            oTemp.MenuItem("M51.3.1", "Sample Requisition", true);
            oTemp.MenuItem("M51.3.2", "Take party approval(reply)", true);
            oTemp.MenuItem("M51.3.3", "Update order status", true);
            oTemp.EndGroup();
            #endregion

            #region Reports/View
            oTemp.BeginGroup("M51.4", "Reports/View", true);
            oTemp.MenuItem("M51.4.1", "Various view of Lab-dips", true);
            oTemp.MenuItem("M51.4.2", "Reports on Lab-dips", true);
            oTemp.MenuItem("M51.4.3", "Various view of Samples", true);
            oTemp.MenuItem("M51.4.4", "Reports on Samples", true);
            oTemp.EndGroup();
            #endregion

            #region Sales Contract
            oTemp.BeginGroup("M51.5", "Sales Contract", true);
            oTemp.MenuItem("M51.5.6.2", "Update party accounts (PL)", true);
            oTemp.MenuItem("M51.5.6.3", "Forword to PI desk with adjustments", true);

            oTemp.MenuItem("M51.5.7.1", "Various view by PL(Party Ledger)", true);
            oTemp.MenuItem("M51.5.7.2", "Reports on sales contract", true);
            oTemp.EndGroup();
            #endregion

            #region Re-dyeing
            oTemp.BeginGroup("M51.6", "Re-dyeing", true);
            oTemp.MenuItem("M51.6.1", "Re-dyeing contract", true);
            oTemp.MenuItem("M51.6.2", "Re-dyeing terms and conditions(adjustments)", true);
            oTemp.MenuItem("M51.6.3", "Receive re-dyeing yarn", true);
            oTemp.MenuItem("M51.6.4", "Process for re-dyeing", true);
            oTemp.MenuItem("M51.6.5", "Update re-dyeing status(after Dyeing)", true);
            oTemp.BeginGroup("M51.6.6", "Re-dyeing", true);
            oTemp.MenuItem("M51.6.6.1", "Re-dyeing reports", true);
            oTemp.EndGroup();
            oTemp.EndGroup();
            #endregion

            oTemp.EndGroup();
            #endregion

            #region Perty Ledger
            oTemp.BeginGroup("M52", "Party Ledger", true);

            oTemp.MenuItem("M52.1", "Sample rate entry", true);
            oTemp.MenuItem("M52.2", "Exchange rate entry", true);
            oTemp.MenuItem("M52.3", "View Party Ledger", true);
            oTemp.MenuItem("M52.4", "Party Ledger", true);
            oTemp.MenuItem("M52.5", "Product Status", true);
            oTemp.MenuItem("M52.6", "Search Order Details (Extended)", true);
            oTemp.MenuItem("M52.7", "PTU Extended Search", true);
            oTemp.EndGroup();

            #endregion

            #region Sales Contract & Order Processing
            oTemp.BeginGroup("M54", "Sales Contract & Order Processing ", true);
            oTemp.MenuItem("M54.1", "Sales Contract Print Setup", true);
            oTemp.MenuItem("M54.2", "Sales Contract ->In Admin", true);
            oTemp.MenuItem("M54.3", "Sales Contract ->Marketing", true);
            oTemp.MenuItem("M54.4", "Change Sales Contract", true);
            oTemp.MenuItem("M54.5", "Dyeing Order Issue", true);
            oTemp.MenuItem("M54.6", "Dyeing Order Authorization Admin", true);
            oTemp.MenuItem("M54.7", "Job order Issue(Send To Production)", true);
            oTemp.MenuItem("M54.8", "Job Activation Admin", true);
            oTemp.MenuItem("M54.9", "Short Claims", true);
            oTemp.BeginGroup("M54.10", "Reports & View", true);
            oTemp.MenuItem("M54.10.1", "New Dyeing Order Issue Report", true);
            oTemp.EndGroup();
            oTemp.EndGroup();
            #endregion

            #region Finishing & Delivery
            oTemp.BeginGroup("M57", "Finishing & Delivery", true);

            oTemp.BeginGroup("M57.1", "Finishing", true);
            oTemp.MenuItem("M57.1.1", "Hydro & Drier", true);
            oTemp.MenuItem("M57.1.5", "Sub-Finishing store-receive yarn", true);
            oTemp.MenuItem("M57.1.2", "Packing", true);
            oTemp.MenuItem("M57.1.3", "Packet - Finishing In", true);
            oTemp.MenuItem("M57.1.4", "Finishing store-receive yarn", true);
            oTemp.EndGroup();

            oTemp.BeginGroup("M57.2", "Delivery", true);
            oTemp.MenuItem("M57.2.6", "Delivery Order Authority", true);
            oTemp.MenuItem("M57.2.1", "Delivery order", true);
            oTemp.MenuItem("M57.2.7", "Delivery order for night shift", true);
            oTemp.MenuItem("M57.2.5", "Delivery order-2nd level authority", true);
            oTemp.MenuItem("M57.2.9", "Delivery order-2nd level authority (for night shift)", true);
            oTemp.MenuItem("M57.2.2", "Approve delivery order", true);
            oTemp.MenuItem("M57.2.10", "Production Tracing Unit", true);
            oTemp.MenuItem("M57.2.3", "Challan", true);
            oTemp.MenuItem("M57.2.8", "Challan for over night authorisation", true);
            oTemp.MenuItem("M57.2.4", "Received Challan", true);
            oTemp.MenuItem("M57.2.11", "Challan Advice", true);
            oTemp.EndGroup();

            oTemp.BeginGroup("M57.3", "Reports & View", true);
            oTemp.MenuItem("M57.3.1", "...", true);
            oTemp.EndGroup();
            oTemp.EndGroup();
            #endregion

            #region Inventory
            oTemp.BeginGroup("M59", "Inventory", true);

            oTemp.BeginGroup("M59.1", "Basic Entry", true);
            oTemp.MenuItem("M59.1.1", "Yarn Categories", true);
            oTemp.MenuItem("M59.1.2", "Dyes", true);
            oTemp.MenuItem("M59.1.3", "Chemicals", true);
            oTemp.MenuItem("M59.1.4", "Machineries & Others", true);
            oTemp.MenuItem("M59.1.5", "Inventory Yarn Name Mapping", true);
            oTemp.EndGroup();

            #region Inventory Operation
            oTemp.BeginGroup("M59.2", "Inventory Opearations", true);
            oTemp.MenuItem("M59.2.10", "Raw yarn lots", true);
            oTemp.MenuItem("M59.2.13", "Soft Winding", true);
            oTemp.MenuItem("M59.2.1", "Invoices(raw yarn in)", true);
            oTemp.MenuItem("M59.2.12", "Invoices/Bills(Sample)", true);
            oTemp.MenuItem("M59.2.3", "Raw yarns out", true);
            oTemp.MenuItem("M59.2.4", "Local party yarn in", true);
            oTemp.MenuItem("M59.2.5", "Local party yarn out", true);
            oTemp.MenuItem("M59.2.7", "Product Adjustment", true);
            oTemp.MenuItem("M59.2.2", "Sample yarn in", true);

            oTemp.MenuItem("M59.2.11", "Dye/Chemical lots", true);
            oTemp.MenuItem("M59.2.9", "Invoices(Dye(s)/Chemical In)", true);
            oTemp.MenuItem("M59.2.6", "Chemicals & dyes out (-> Route Sheet)", true);
            oTemp.MenuItem("M59.2.8", "Chemicals & dyes out (-> Requisition)", true);
            oTemp.MenuItem("M59.2.14", "Chemicals & dyes Adjustment", true);

            #region storeToStoreTransfer
            oTemp.BeginGroup("M59.2.20", "Store to Store Transfer", true);

            //Yarn Part (Raw Yarn + Dyed Yarn)
            oTemp.MenuItem("M59.2.20.1", "Yarn internal challan [issue/disburse/receive]", true);// issue+authorise Yarn Requisition
            oTemp.MenuItem("M59.2.20.4", "Yarn external transfer [issue/disburse/receive]", true);// issue+authorise
            oTemp.MenuItem("M59.2.20.5", "Yarn Internal transfer [Delivery to RecycleStore]", true);// issue+authorise
            oTemp.MenuItem("M59.2.20.60", "Yarn Internal transfer [Available to Delivery]", true);// issue+authorise

            //Dye
            oTemp.MenuItem("M59.2.20.40", "Dye internal challan [issue/disburse/receive]", true);// issue+authorise
            oTemp.MenuItem("M59.2.20.20", "Dye external transfer[issue/disburse/receive]", true);// issue+authorise [it can be receive or disburse]
            oTemp.MenuItem("M59.2.20.23", "Dye consumption requisition issue (lab use/ machine wash/ buyer support treatments)", true);//issue+authorise
            oTemp.MenuItem("M59.2.20.24", "Dye consumption requisition disburse", true);

            //Chemical
            oTemp.MenuItem("M59.2.20.10", "Chemical internal challan [issue/disburse/receive]", true);// issue+authorise
            oTemp.MenuItem("M59.2.20.31", "Chemical external transfer [issue/disburse/receive]", true);// issue+authorise [it can be receive or disburse]
            oTemp.MenuItem("M59.2.20.34", "Chemical consumption requisition issue \n(lab use/ machine wash/ buyer support treatments)", true);//issue+authorise
            oTemp.MenuItem("M59.2.20.35", "Chemical consumption requisition disburse", true);
            //oTemp.MenuItem("M59.2.20.11", "Chemical Requisition Slip-> Admin", true);
            oTemp.EndGroup();
            #endregion
            oTemp.EndGroup();
            #endregion

            #region Reports & view
            oTemp.BeginGroup("M59.3", "Reports & Views", true);
            oTemp.MenuItem("M59.3.1", "Report Setups", true);
            oTemp.MenuItem("M59.3.2", "Inventory", true);
            oTemp.MenuItem("M59.3.3", "Product", true);
            oTemp.MenuItem("M59.3.4", "LC Wise Report", true);
            oTemp.MenuItem("M59.3.5", "Re-dyeing yarn stock", true);
            oTemp.MenuItem("M59.3.6", "Rejected yarn report", true);
            oTemp.MenuItem("M59.3.7", "Yarn Store", true);
            #region Dye Chemical Stock
            oTemp.BeginGroup("M59.3.9", "Dye Chemical Report", true);
            oTemp.MenuItem("M59.3.9.1", "Dye Chemical Stock Report", true);
            oTemp.MenuItem("M59.3.9.2", "Dye Chemical Stock In Out Report", true);
            oTemp.MenuItem("M59.3.9.3", "Dye Chemical In Out In Lab", true);
            oTemp.EndGroup();
            #endregion

            oTemp.MenuItem("M59.3.11", "Configurable yarn report", true);
            oTemp.MenuItem("M59.3.12", "Inventory summary", true);


            oTemp.EndGroup();
            #endregion

            oTemp.EndGroup();
            #endregion

            #region Laboratory
            oTemp.BeginGroup("M60", "Laboratory", true);
            oTemp.MenuItem("M60.3", "Lab Capacity", true);
            oTemp.MenuItem("M60.1", "Labdips", true);
            oTemp.MenuItem("M60.4", "SampleLabdips", true);
            oTemp.MenuItem("M60.2", "Route Sheets", true);

            oTemp.BeginGroup("M60.10", "Reports & Views", true);
            oTemp.MenuItem("M60.10.1", "EWYDL Color", true);
            oTemp.MenuItem("M60.10.2", "EWYDL Color All", true);
            oTemp.MenuItem("M60.10.3", "EWYDL Color All(for lab)", true);
            oTemp.MenuItem("M60.10.4", "Lab Monitoring", true);
            oTemp.EndGroup();
            oTemp.EndGroup();
            #endregion

            #region Production
            oTemp.BeginGroup("M61", "Production", true);
            oTemp.BeginGroup("M61.0", "Basic Entry & Setup", true);
            oTemp.MenuItem("M61.0.1", "Dye Machines", true);
            oTemp.MenuItem("M61.0.4", "Drier Hydro Machines", true);
            oTemp.MenuItem("M61.0.2", "Route Sheet Dye Chemical Setup(RSDCS)", true);
            oTemp.MenuItem("M61.0.3", "Collect Orphans [DyeChemicalSetup(RSTSFJ)]", true);
            oTemp.EndGroup();

            oTemp.BeginGroup("M61.15", "Production Planning & Maintenance", true);
            oTemp.MenuItem("M61.15.2", "Route Sheet Job wise distribution", true);
            oTemp.MenuItem("M61.15.3", "Route Sheet Cancel ->Accept", true);
            oTemp.MenuItem("M61.15.4", "Route Sheet Cancel", true);
            oTemp.MenuItem("M61.15.5", "Dyed yarn color", true);
            oTemp.EndGroup();

            oTemp.MenuItem("M61.5", "Dye/Chemicals requisitions (floor)", true);
            oTemp.MenuItem("M61.4", "Route Sheets->Floor", true);
            oTemp.EndGroup();
            #endregion

            #region Factory Monitor
            oTemp.BeginGroup("M62", "Factory Monitor", true);
            oTemp.MenuItem("M62.1", "Production Tracking", true);
            oTemp.MenuItem("M62.2", "Inventory Tracking", true);
            oTemp.MenuItem("M62.3", "Dying Master", true);
            oTemp.MenuItem("M62.4", "Delivery & Challan Monitor", true);
            oTemp.EndGroup();
            #endregion

            _oMenuTree = oTemp;
        }
        #endregion
        #endregion

        #region Web Menu Display Items
        public static HierarchyTree MenuTreeWeb()
        {
            return _oMenuTreeWeb;
        }
        public static NodeItem SelectedMenuWeb
        {
            get { return _oSelectedMenuWeb; }
            set { _oSelectedMenuWeb = value; }
        }
        #region Menu Decleration Web
        public static void BuildMenuTreeBasic()
        {
            _oMenuTree = new HierarchyTree();
            HierarchyTree oTemp = new HierarchyTree();
            #region Common
            oTemp.BeginGroup("M0", "ESimSol", true);
            oTemp.MenuItem("M0.0", "Home", true);
            oTemp.MenuItem("M0.1", "About us", true);
            oTemp.MenuItem("M0.2", "Contact us", true);
            oTemp.MenuItem("M0.100", "infocrat solution", true);
            oTemp.EndGroup();
            #endregion
            _oMenuTree = oTemp;
        }
        public static void BuildMenuTreeWeb()
        {
            _oMenuTreeWeb = new HierarchyTree();
            HierarchyTree oTemp = new HierarchyTree();
            #region Administrative Privilege
            oTemp.BeginGroup("WM0", "Administrative Privilege", true);
            oTemp.MenuItem("WM0.1", "Organizational Information", true);
            oTemp.MenuItem("WM0.2", "User Management", true);
            oTemp.MenuItem("WM0.3", "Bussiness Permission", true);
            oTemp.EndGroup();
            #endregion

            #region Lab-dip & Sample Orders
            oTemp.BeginGroup("WM1", "Lab-dip & Sample Orders", true);
            oTemp.BeginGroup("WM1.0", "Order Process", true);
            oTemp.MenuItem("WM1.0.0", "Issue lab-dip order", true);
            oTemp.MenuItem("WM1.0.1", "Accept lab-dip order", true);
            oTemp.MenuItem("WM1.0.2", "Issue sample order", true);
            oTemp.MenuItem("WM1.0.3", "Issue Dyeing order", true);
            oTemp.MenuItem("WM1.0.4", "Issue Job order", true);
            oTemp.EndGroup();

            oTemp.BeginGroup("WM1.1", "Reports/View", true);
            oTemp.MenuItem("WM1.1.0", "Lab-dip Orders Reports", true);
            oTemp.MenuItem("WM1.1.1", "Sample Orders Reports", true);
            oTemp.EndGroup();

            oTemp.BeginGroup("WM1.2", "Delivery & Finising", true);
            oTemp.MenuItem("WM1.2.0", "Approve Delivery Orders", true);
            oTemp.MenuItem("WM1.2.1", "Approve PTU", true);
            oTemp.EndGroup();

            oTemp.EndGroup();
            #endregion

            #region Authority
            oTemp.BeginGroup("WM2", "Authority & Approval", true);
            oTemp.BeginGroup("WM2.0", "Delivery & Finising", true);
            oTemp.MenuItem("WM2.0.1", "Approve Delivery Orders", true);
            oTemp.MenuItem("WM2.0.2", "Approve Job Order", true);
            oTemp.MenuItem("WM2.0.3", "Approve Dyeing Order", true);
            oTemp.MenuItem("WM2.0.4", "Approve Internal Challan", true);
            oTemp.EndGroup();

            oTemp.BeginGroup("WM2.1", "Reports", true);
            oTemp.MenuItem("WM2.1.1", "LC Bill Wise Delivery", true);
            oTemp.EndGroup();

            oTemp.EndGroup();
            #endregion
            _oMenuTreeWeb = oTemp;
        }
        #endregion
        #endregion

        #region Date Related functuion
        #region First date of month/year
        /// <summary>
        /// This function return the first date of month of param date
        /// </summary>
        /// <param name="forDate">A valid date</param>
        /// <returns>Return the date of month</returns>
        public static DateTime FirstDateOfMonth(DateTime forDate)
        {
            DateTime dFDOfMonth = new DateTime(forDate.Year, forDate.Month, 1);
            return dFDOfMonth;
        }
        public static DateTime FirstDateOfMonth(int year, int month)
        {
            DateTime dFDOfMonth = new DateTime(year, month, 1);
            return dFDOfMonth;
        }

        public static DateTime FirstDateOfYear(DateTime forDate)
        {
            DateTime dFDOfYear = new DateTime(forDate.Year, 1, 1);
            return dFDOfYear;
        }
        public static DateTime FirstDateOfYear(int year)
        {
            DateTime dFDOfYear = new DateTime(year, 1, 1);
            return dFDOfYear;
        }
        #endregion
        #region Last date of month/year
        public static DateTime LastDateOfYear(DateTime forDate)
        {
            DateTime dFDOfYear = new DateTime(forDate.Year, 12, 31);
            return dFDOfYear;
        }

        public static DateTime LastDateOfYear(int year)
        {
            DateTime dFDOfYear = new DateTime(year, 12, 31);
            return dFDOfYear;
        }

        public static DateTime LastDateOfMonth(DateTime forDate)
        {
            DateTime dLDOfMonth = new DateTime(forDate.Year, forDate.Month, 1);
            dLDOfMonth = dLDOfMonth.AddMonths(1);
            dLDOfMonth = dLDOfMonth.AddDays(-1);
            return dLDOfMonth;
        }
        public static DateTime LastDateOfMonth(int year, int month)
        {
            DateTime dLDOfMonth = new DateTime(year, month, 1);
            dLDOfMonth = dLDOfMonth.AddMonths(1);
            dLDOfMonth = dLDOfMonth.AddDays(-1);
            return dLDOfMonth;
        }
        #endregion

        #region Date Difference function
        /// <summary>
        /// This function return the integer value in accordance with stringformat
        /// </summary>
        /// <param name="Date1">Subtract from this value and must be datetime</param>
        /// <param name="Date2">Subtract this value and must be datetime</param>
        /// <param name="Interval">There are three types of format 1=d/D return difference in days, 2="m/M return difference in month, 3=y/Y return difference in years//   </param>
        /// <returns></returns>
        public static int DateDiff(string Interval, DateTime Date1, DateTime Date2)
        {
            int difVale = 0;
            DateTime startDate, endDate;

            if (Date1 > Date2)
            {
                endDate = Date1;
                startDate = Date2;
            }
            else
            {
                startDate = Date1;
                endDate = Date2;
            }
            switch (Interval)
            {
                case "D":
                case "d":
                    for (int nYear = startDate.Year; nYear < endDate.Year; nYear++)
                    {
                        difVale += new DateTime(nYear, 12, 31).DayOfYear;
                    }
                    difVale += endDate.DayOfYear - startDate.DayOfYear;
                    break;

                case "M":
                case "m":
                    difVale = endDate.Year - startDate.Year;
                    difVale = difVale * 12;
                    difVale += endDate.Month - startDate.Month;
                    break;
                case "Y":
                case "y":
                    difVale = endDate.Year - startDate.Year;
                    break;
            }
            if (Date1 > Date2)
            {
                difVale = -difVale;
            }
            return difVale;
        }
        #endregion

        /// <summary>
        /// This function return Timespan of two date(time)
        /// </summary>
        /// <param name="Date1">Subtract from this value and must be datetime</param>
        /// <param name="Date2">Subtract this value and must be datetime</param>        
        /// <returns></returns>
        public static TimeSpan TimeDifference(DateTime Date1, DateTime Date2)
        {
            DateTime fromDate, toDate;
            fromDate = Date1;
            toDate = Date2;

            if (DateTime.Compare(Date1, Date2) < 0)
            {
                fromDate = Date1; toDate = Date2;
            }
            if (DateTime.Compare(Date1, Date2) > 0)
            {
                fromDate = Date2; toDate = Date1;
            }

            TimeSpan tsReturn = toDate - fromDate;

            return tsReturn;
        }
        #endregion

        #region Left/Right/Mid  function
        public static string Left(string intputString, int Length)
        {
            string retStr = "";
            if (Length < intputString.Length)
            {
                retStr = intputString.Substring(0, Length);
            }
            else
            {
                retStr = intputString;
            }
            return retStr;
        }

        public static string Right(string intputString, int Length)
        {
            string retStr = "";
            if (Length < intputString.Length && Length > 0)
            {
                retStr = intputString.Substring((intputString.Length - Length), Length);
            }
            else
            {
                retStr = intputString;
            }
            return retStr;
        }
        public static string Mid(string intputString, int Start, int Length)
        {
            string retStr = "";

            if ((Start + Length) < intputString.Length)
            {
                retStr = intputString.Substring(Start, Length);
            }
            else if (Start < intputString.Length)
            {
                retStr = intputString.Substring(Start);
            }
            else
            {
                retStr = intputString;
            }
            return retStr;
        }
        public static string Mid(string intputString, int Start)
        {
            string retStr = "";

            if (Start < intputString.Length)
            {
                retStr = intputString.Substring(Start);
            }
            else
            {
                retStr = intputString;
            }
            return retStr;
        }
        #endregion

        #region Taka/Amount in words
        private static string HundredWords(int inputValue)
        {
            string hundredWords = "", numStr = "", pos1 = "", pos2 = "", pos3 = "";
            string[] digits = new string[10] { "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" };
            string[] teens = new string[10] { "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
            string[] tens = new string[10] { "", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

            numStr = Right(inputValue.ToString("000"), 3);
            if (Left(numStr, 1) != "0")
            {
                pos1 = digits.GetValue(Convert.ToInt32(Left(numStr, 1))) + " Hundred";
            }
            else
            {
                pos1 = "";
            }

            numStr = Right(numStr, 2);
            if (Left(numStr, 1) == "1")
            {
                pos2 = Convert.ToString(teens.GetValue(Convert.ToInt32(Right(numStr, 1))));
                pos3 = "";
            }
            else
            {
                pos2 = Convert.ToString(tens.GetValue(Convert.ToInt32(Left(numStr, 1))));
                pos3 = Convert.ToString(digits.GetValue(Convert.ToInt32(Right(numStr, 1))));
            }
            hundredWords = pos1;
            if (hundredWords != "")
            {
                if (pos2 != "")
                {
                    hundredWords = hundredWords + " ";
                }
            }
            hundredWords = hundredWords + pos2;

            if (hundredWords != "")
            {
                if (pos3 != "")
                {
                    hundredWords = hundredWords + " ";
                }
            }
            hundredWords = hundredWords + pos3;

            return hundredWords;
        }

        private static string InWords(double inputValue, string beforeDecimal, string afterDecimal)
        {
            int commaCount = 0, digitCount = 0;
            string sign = "", takaWords = "", numStr = "", taka = "", paisa = "", pow = "";
            string[] pows = new string[3] { "Crore", "Thousand", "Lac" };

            if (inputValue < 0)
            {
                sign = "Minus";
                inputValue = Math.Abs(inputValue);
            }

            numStr = inputValue.ToString("0.00");
            paisa = HundredWords(Convert.ToInt32(Right(numStr, 2)));

            if (paisa != "")
            {
                paisa = paisa.Substring(0, 1).ToUpper() + paisa.Substring(1);
                paisa = afterDecimal + " " + paisa;
            }

            numStr = Left(numStr, numStr.Length - 3);
            taka = HundredWords(Convert.ToInt32(Right(numStr, 3)));

            if (numStr.Length <= 3)
            {
                numStr = "";
            }
            else
            {
                numStr = Left(numStr, numStr.Length - 3);
            }

            commaCount = 1;
            if (numStr != "")
            {
                do
                {
                    if (commaCount % 3 == 0)
                    {
                        digitCount = 3;
                    }
                    else
                    {
                        digitCount = 2;
                    }

                    pow = HundredWords(Convert.ToInt32(Right(numStr, digitCount)));
                    if (pow != "")
                    {
                        if (Convert.ToString(inputValue).Length > 10)
                        {
                            //pow = pow + " " + pows.GetValue(commaCount % 3) + " crore ";//By Abdullah
                            pow = pow + " " + pows.GetValue(commaCount % 3);
                        }
                        else
                        {
                            pow = pow + " " + pows.GetValue(commaCount % 3);
                        }
                    }
                    if (taka != "")
                    {
                        if (pow != "")
                        {
                            pow = pow + " ";
                        }
                    }

                    taka = pow + taka;
                    if (numStr.Length <= digitCount)
                    {
                        numStr = "";
                    }
                    else
                    {
                        numStr = Left(numStr, numStr.Length - digitCount);
                    }
                    commaCount = commaCount + 1;

                }
                while (numStr != "");
            }

            if (taka != "")
            {
                taka = taka.Substring(0, 1).ToUpper() + taka.Substring(1);
                taka = beforeDecimal + " " + taka;
            }
            takaWords = taka;

            if (takaWords != "")
            {
                if (paisa != "")
                {
                    takaWords = takaWords + " and ";
                }
            }
            takaWords = takaWords + paisa;

            if (takaWords == "")
            {
                takaWords = beforeDecimal + " Zero";
            }
            takaWords = sign + takaWords + " Only";
            return takaWords;
        }

        public static string AmountInWords(decimal inputValue, string beforeDecimal, string afterDecimal)
        {
            return InWords(Convert.ToDouble(inputValue), beforeDecimal, afterDecimal);
        }

        public static string AmountInWords(float inputValue, string beforeDecimal, string afterDecimal)
        {
            return InWords(Convert.ToDouble(inputValue), beforeDecimal, afterDecimal);
        }

        public static string AmountInWords(double inputValue, string beforeDecimal, string afterDecimal)
        {
            return InWords(inputValue, beforeDecimal, afterDecimal);
        }

        public static string TakaWords(decimal inputValue)
        {
            return TakaWords(Convert.ToDouble(inputValue));
        }

        public static string TakaWords(float inputValue)
        {
            return TakaWords(Convert.ToDouble(inputValue));
        }

        public static string TakaWords(double inputValue)
        {
            return InWords(inputValue, "Taka", "Paisa");
        }        
        public static string EuroWords(double inputValue)
        {
            return InWords(inputValue, "Euro", "Cent");
        }

        public static string DollarWords(double inputValue)
        {
            return InWords(inputValue, "Dollar", "Cent");
        }

        public static string PoundWords(double inputValue)
        {
            return InWords(inputValue, "Pound", "Cent");
        }
        public static string DollarWords(decimal inputValue)
        {
            return DollarWords(Convert.ToDouble(inputValue));
        }

        public static string DollarWords(float inputValue)
        {
            return DollarWords(Convert.ToDouble(inputValue));
        }

        #endregion

        #region RoundOff
        public static double RoundOff(decimal inputValue)
        {
            return Convert.ToDouble(Math.Round(inputValue));
        }
        public static double RoundOff(decimal inputValue, int digits)
        {
            return RoundOff(Convert.ToDouble(inputValue), digits);
        }

        public static double RoundOff(double inputValue, int digits)
        {
            double rmndr = 0.0, retVal = 0.0;
            if (digits < 0)
            {
                retVal = Math.Pow(10, Math.Abs(digits));
                rmndr = ((inputValue % retVal) / retVal);
                if (rmndr >= 0.5)
                {
                    rmndr = 1 * retVal;
                }
                else
                {
                    rmndr = 0;
                }
                retVal = (((inputValue - (inputValue % retVal)) / retVal) * retVal) + rmndr;
            }
            else
            {
                retVal = Math.Round(inputValue, digits);
            }
            return retVal;
        }
        public static double RoundOff(double inputValue)
        {
            return Math.Round(inputValue);
        }

        public static double RoundOff(float inputValue, int digits)
        {
            return RoundOff(Convert.ToDouble(inputValue), digits);
        }
        public static double RoundOff(float inputValue)
        {
            return Math.Round(inputValue);
        }
        #endregion

        #region Taka Format
        public static string TakaFormat(decimal inputValue)
        {
            return TakaFormat(Convert.ToDouble(inputValue));
        }
        public static string TakaFormat(float inputValue)
        {
            return TakaFormat(Convert.ToDouble(inputValue));
        }
        public static string TakaFormat(double inputValue)
        {
            int commaCount = 1, digitCount = 0;
            string sign = "", numStr = "", takaFormat = "";

            if (inputValue < 0)
            {
                sign = "-";
                inputValue = (-inputValue);
            }

            numStr = inputValue.ToString("0.00");
            takaFormat = Right(numStr, 6);
            if ((numStr.Length) <= 6)
            {
                numStr = "";
            }
            else
            {
                numStr = Left(numStr, numStr.Length - 6);
            }

            if (numStr == "")
            {
                //takaFormat = takaFormat;
                return takaFormat;
            }

            do
            {
                if (commaCount % 3 == 0)
                {
                    digitCount = 3;
                }
                else
                {
                    digitCount = 2;
                }

                takaFormat = Right(numStr, digitCount) + "," + takaFormat;

                if (numStr.Length <= digitCount)
                {
                    numStr = "";
                }
                else
                {
                    numStr = Left(numStr, numStr.Length - digitCount);
                }

                commaCount = commaCount + 1;
            }
            while (numStr != "");

            takaFormat = sign + takaFormat;

            return takaFormat;
        }
        #endregion

        #region Million Format
        public static string MillionFormat(decimal inputValue, char sepChar, char decChar)
        {
            return MillionFormat(Convert.ToDouble(inputValue), sepChar, decChar, 2);
        }
        public static string MillionFormat(decimal inputValue)
        {
            return MillionFormat(Convert.ToDouble(inputValue));
        }

        public static string MillionFormat(decimal inputValue, int nDecimalPoint)
        {
            return MillionFormat(Convert.ToDouble(inputValue), nDecimalPoint);
        }
        public static string MillionFormat(float inputValue, int nDecimalPoint)
        {
            return MillionFormat(Convert.ToDouble(inputValue), nDecimalPoint);
        }
        public static string MillionFormat(double inputValue, int nDecimalPoint)
        {
            return MillionFormat(inputValue, ',', '.', nDecimalPoint);
        }
        public static string MillionFormat(float inputValue, char sepChar, char decChar)
        {
            return MillionFormat(Convert.ToDouble(inputValue), sepChar, decChar, 2);
        }
        public static string MillionFormat(float inputValue)
        {
            return MillionFormat(Convert.ToDouble(inputValue));
        }
        public static string MillionFormat(double inputValue, char sepChar, char decChar, int nDecimalPoint)
        {

            int commaCount = 1, digitCount = 3, nDP = nDecimalPoint;
            string sign = "", numStr = "", milFormat = "";

            if (inputValue < 0)
            {
                sign = "-";
                inputValue = (-inputValue);
            }

            switch (nDP)
            {
                case 0:
                    numStr = Convert.ToString(Math.Round(inputValue));
                    break;
                case 1:
                    numStr = inputValue.ToString("0.0");
                    break;
                case 2:
                    numStr = inputValue.ToString("0.00");
                    break;
                case 3:
                    numStr = inputValue.ToString("0.000");
                    break;
                case 4:
                    numStr = inputValue.ToString("0.0000");
                    break;
                case 5:
                    numStr = inputValue.ToString("0.00000");
                    break;
                case 6:
                    numStr = inputValue.ToString("0.000000");
                    break;
                case 7:
                    numStr = inputValue.ToString("0.0000000");
                    break;
                case 8:
                    numStr = inputValue.ToString("0.00000000");
                    break;
                case 9:
                    numStr = inputValue.ToString("0.000000000");
                    break;
                case 10:
                    numStr = inputValue.ToString("0.0000000000");
                    break;
                default:
                    numStr = inputValue.ToString("0.00");
                    break;
            }

            milFormat = Right(numStr, 6);
            if ((numStr.Length) <= 6)
            {
                numStr = "";
            }
            else
            {
                numStr = Left(numStr, numStr.Length - 6);
            }

            if (numStr == "")
            {
                //milFormat=milFormat;
                if (milFormat == "0" && nDecimalPoint == 0)
                {
                    return "-";
                }
                else
                {
                    return sign + milFormat;
                }
            }

            do
            {
                milFormat = Right(numStr, digitCount) + "," + milFormat;

                if (numStr.Length <= digitCount)
                {
                    numStr = "";
                }
                else
                {
                    numStr = Left(numStr, numStr.Length - digitCount);
                }
                commaCount = commaCount + 1;
            }
            while (numStr != "");
            milFormat = sign + milFormat;

            milFormat = milFormat.Replace('.', '|');
            milFormat = milFormat.Replace(',', sepChar);
            milFormat = milFormat.Replace('|', decChar);

            return milFormat;
        }
        public static string MillionFormat(double inputValue)
        {
            return MillionFormat(inputValue, ',', '.', 2);
        }
        public static string MillionFormat_Round(double inputValue)
        {
            //return MillionFormat(inputValue, ',', '.', 0);
            inputValue = Math.Round(inputValue);
            int commaCount = 1, digitCount = 3, nDP = 2;
            string sign = "", numStr = "", milFormat = "";

            if (inputValue < 0)
            {
                sign = "-";
                inputValue = (-inputValue);
            }

            switch (nDP)
            {
                case 0:
                    numStr = Convert.ToString(Math.Round(inputValue));
                    break;
                case 1:
                    numStr = inputValue.ToString("0.0");
                    break;
                case 2:
                    numStr = inputValue.ToString("0.00");
                    break;
                case 3:
                    numStr = inputValue.ToString("0.000");
                    break;
                case 4:
                    numStr = inputValue.ToString("0.0000");
                    break;
                case 5:
                    numStr = inputValue.ToString("0.00000");
                    break;
                case 6:
                    numStr = inputValue.ToString("0.000000");
                    break;
                case 7:
                    numStr = inputValue.ToString("0.0000000");
                    break;
                case 8:
                    numStr = inputValue.ToString("0.00000000");
                    break;
                case 9:
                    numStr = inputValue.ToString("0.000000000");
                    break;
                case 10:
                    numStr = inputValue.ToString("0.0000000000");
                    break;
                default:
                    numStr = inputValue.ToString("0.00");
                    break;
            }

            milFormat = Right(numStr, 6);
            if ((numStr.Length) <= 6)
            {
                numStr = "";
            }
            else
            {
                numStr = Left(numStr, numStr.Length - 6);
            }

            if (numStr == "")
            {
                //milFormat=milFormat;
                if (milFormat == "0.00")
                {
                    return "-";
                }
                else
                {
                    //  milFormat = milFormat.Remove(3);
                    return sign + milFormat;
                }
            }

            do
            {
                milFormat = Right(numStr, digitCount) + "," + milFormat;

                if (numStr.Length <= digitCount)
                {
                    numStr = "";
                }
                else
                {
                    numStr = Left(numStr, numStr.Length - digitCount);
                }
                commaCount = commaCount + 1;
            }
            while (numStr != "");
            milFormat = sign + milFormat;

            milFormat = milFormat.Replace('.', '|');
            //milFormat = milFormat.Replace(',', sepChar);
            //milFormat = milFormat.Replace('|', decChar);
            milFormat = milFormat.Remove(milFormat.Length - 3, 3);
            return milFormat;

        }

        public static string MillionFormatActualDigit(double inputValue)
        {
            string sReturnValue = "";
            sReturnValue = inputValue.ToString("#,##0.####;(#,##0.####)");
            return sReturnValue;
        }

        public static string MillionFormatRound(double inputValue, int decimalPlace)
        {
            inputValue = Math.Round(inputValue, decimalPlace);
            return MillionFormat(inputValue, decimalPlace);
        }
        #endregion

        #region Sentance case
        public static string MakeItSentence(string inputString)
        {
            char a;
            int i = 0;

            for (i = 0; i < inputString.Length; i++)
            {
                a = Convert.ToChar(inputString.Substring(i, 1));
                if (Char.IsUpper(a))
                {
                    if (i != 0 && inputString.Substring(i - 1, 1) != " ")
                    {
                        inputString = inputString.Insert(i, " ");
                    }
                }
            }
            return inputString;
        }

        #endregion

        #region GetEnumValue
        public static int GetEnumValue(Type enumType, string stringValue)
        {
            foreach (object value in Enum.GetValues(enumType))
            {
                if (value.ToString() == stringValue)
                    return (int)value;
            }
            return 0;
        }
        #endregion

        #region IsNumeric
        public static bool IsNumeric(object obj)
        {
            try
            {
                double nTemp = Convert.ToDouble(obj);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region IsInteger
        public static bool IsInteger(object obj)
        {
            try
            {
                double nTemp = Convert.ToInt32(obj);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region GetYard
        /// <summary>
        /// Convert Meter into Yard
        /// </summary>
        /// <param name="nMeter"></param>
        /// <param name="nDigit"></param>
        /// <returns>Returns Yard value of Meter</returns>
        public static double GetYard(double nMeter, int nDigit)
        {
            if (nDigit > 8) nDigit = 8;
            nMeter = nMeter * 1.0936132983;
            // nKG = nKG *2.2046244201837775;
            return Math.Round(nMeter, nDigit);
        }
        #endregion

        #region GetMeter
       /// <summary>
       /// Convert Yard into Meter
       /// </summary>
       /// <param name="nYard"></param>
       /// <param name="nDigit"></param>
       /// <returns>Returns Meter value of Yard</returns>
        public static double GetMeter(double nYard, int nDigit)
        {
            if (nDigit > 8) nDigit = 8;
            nYard = nYard / 1.0936132983;
            // nKG = nKG *2.2046244201837775;
            return Math.Round(nYard, nDigit);
        }
        #endregion

        #region GetLBS
        /// <summary>
        /// Convert KG into LBS
        /// </summary>
        /// <param name="nKG"></param>
        /// /// <param name="nDigit"></param>
        /// <returns>Retrun LBS value of KG</returns>
        public static double GetLBS(double nKG, int nDigit)
        {
            if (nDigit > 8) nDigit = 8;
            nKG = nKG * 2.2046226218;
            // nKG = nKG *2.2046244201837775;
            return Math.Round(nKG, nDigit);
        }
        #endregion

        #region GetKG
        /// <summary>
        /// Convert LBS into KG
        /// </summary>
        /// <param name="nKG"></param>
        /// /// <param name="nDigit"></param>
        /// <returns>Retrun KG value of LBS</returns>
        public static double GetKG(double nLBS, int nDigit)
        {
            if (nDigit > 10) nDigit = 10;
            //nLBS = nLBS * 0.45359237001003542909395360718511; //Add By ICS
            // nLBS = nLBS * (1 /2.2046226218);
            nLBS = nLBS * 0.4535970244035199; // Add By Baly
            // nLBS = nLBS * (1 /2.2046); // Add By Baly
            return Math.Round(nLBS, nDigit);
        }
        #endregion

        #region GetFormattedCode
        public static string GetFormattedCode(string sFormat, object oValue)
        {
            string sReturn;
            sReturn = sFormat.Insert(sFormat.Length - oValue.ToString().Length, oValue.ToString());
            sReturn = sReturn.ToString().Substring(0, sFormat.Length);
            return sReturn;
        }
        #endregion

        #region GetFormattedCode
        public static string GetDigitsOnly(string sValue)
        {
            string sTemp = "";
            foreach (char c in sValue)
            {
                if (Char.IsDigit(c))
                    sTemp += c.ToString();
            }
            return sTemp;
        }
        #endregion

        #region TagSQL
        /// <summary>
        /// first make the search query from an empty string. then add the selection query.
        /// Adds ' WHERE ' to an empty string and ' AND ' to others
        /// </summary>
        /// <param name="sSQL"></param>
        public static void TagSQL(ref string sSQL)
        {
            if (sSQL.Length == 0) { sSQL = " WHERE "; }
            else { sSQL = sSQL + " AND "; }
        }
        public static void TagSQL(ref string sSQL, string sTextLikeFieldName, string sTextLikeVale)
        {
            // this is static mamber below lines of codes repating
            if (sSQL.Length == 0) { sSQL = " WHERE "; }
            else { sSQL = sSQL + " AND "; }

            sTextLikeVale = sTextLikeVale.Trim();
            sSQL = sSQL + " " + sTextLikeFieldName + " like '%" + sTextLikeVale + "%'";
        }
        /// <summary>
        /// first make the search query from an empty string. then add the selection query.
        /// Adds ' WHERE ' to an empty string. if not empty then ' OR ' if second parameter is true or ' AND ' if second parameter is false.
        /// </summary>
        /// <param name="sSQL"></param>
        /// <param name="bTagOr"></param>
        public static void TagSQL(ref string sSQL, bool bTagOr)
        {
            if (sSQL.Length == 0)
                sSQL = " WHERE ";
            else
                if (bTagOr)
                    sSQL = sSQL + " OR ";
                else
                    sSQL = sSQL + " AND ";
        }
        #endregion

        #region Password related function
        public static string Encrypt(string sText)
        {
            int i = 0;
            string sEncrypt = "", sKey = "cel.abracadabra";
            char cTextChar, cKeyChar;
            char[] cTextData, cKey;

            //Save Length of Pass
            sText = (char)(sText.Length) + sText;

            //Pad Password with space upto 10 Characters
            if (sText.Length < 10)
            {
                sText = sText + sText.PadRight((10 - sText.Length), ' ');
            }
            cTextData = sText.ToCharArray();

            //Make the key big enough
            while (sKey.Length < sText.Length)
            {
                sKey = sKey + sKey;
            }
            sKey = sKey.Substring(0, sText.Length);
            cKey = sKey.ToCharArray();

            //Encrypting Data
            for (i = 0; i < sText.Length; i++)
            {
                cTextChar = (char)cTextData.GetValue(i);
                cKeyChar = (char)cKey.GetValue(i);
                sEncrypt = sEncrypt + IntToHex((int)(cTextChar) ^ (int)(cKeyChar));
            }

            return sEncrypt;
        }

        public static string Decrypt(string sText)
        {
            int j = 0, i = 0, nLen = 0;
            string sTextByte = "", sDecrypt = "", sKey = "cel.abracadabra";
            char[] cTextData, cKey;
            char cTextChar, cKeyChar;

            //Taking Lenght, half of Encrypting data  
            nLen = sText.Length / 2;

            //Making key is big Enough
            while (sKey.Length < nLen)
            {
                sKey = sKey + sKey;
            }
            sKey = sKey.Substring(0, nLen);
            cKey = sKey.ToCharArray();
            cTextData = sText.ToCharArray();

            //Decripting data
            for (i = 0; i < nLen; i++)
            {
                sTextByte = "";
                for (j = i * 2; j < (i * 2 + 2); j++)
                {
                    sTextByte = sTextByte + cTextData.GetValue(j).ToString();
                }
                cTextChar = (char)HexToInt(sTextByte);
                cKeyChar = (char)cKey.GetValue(i);
                sDecrypt = sDecrypt + (char)((int)(cKeyChar) ^ (int)(cTextChar));
            }

            //Taking real password
            cTextData = sDecrypt.ToCharArray();
            sDecrypt = "";
            i = (int)(char)cTextData.GetValue(0);
            for (j = 1; j <= i; j++)
            {
                sDecrypt = sDecrypt + cTextData.GetValue(j).ToString();
            }

            return sDecrypt;
        }

        private static string IntToHex(int nIntData)
        {
            return Convert.ToString(nIntData, 16).PadLeft(2, '0');
        }

        private static int HexToInt(string sHexData)
        {
            return Convert.ToInt32(sHexData, 16);
        }
        #endregion

        #region Status Bar
        public class Status
        {
            private static ToolStripStatusLabel _statusBar;
            private static Stack _statusList = null;

            public Status() { }

            public static void RegisterControl(ToolStripStatusLabel statusControl)
            {
                _statusBar = statusControl;
                _statusList = new Stack();
            }
            public static void SetStatus(string statusString)
            {
                if (_statusList == null) return;
                _statusList.Push(statusString);
                _statusBar.Text = statusString;
                Cursor.Current = Cursors.WaitCursor;
            }

            public static void UpdateStatus(string statusString)
            {
                Cursor.Current = Cursors.WaitCursor;
                _statusBar.Text = statusString;
            }

            public static void ResetStatus()
            {
                if (_statusList == null) return;
                if (_statusList.Count > 0)
                {
                    _statusList.Pop();
                }
                if (_statusList.Count > 0)
                {
                    _statusBar.Text = (string)_statusList.Peek();
                }
                else
                {
                    _statusBar.Text = "Ready";
                    Cursor.Current = Cursors.Default;
                }
            }
        }
        #endregion

        #region Cheek Valid Mail Address, Mail Send 
        public static bool IsValidMail(string sEmail)
        {
            try
            {
                var address = new System.Net.Mail.MailAddress(sEmail);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool MailSend(string Subject, string BodyInformation, List<string> EmailTo, List<string> EmailToCC, List<Attachment> oAttachments, string sSendingEmail = "esimsol.service@gmail.com", string sSendingEmailPassword = "ics@2007", string sDisplayName = "Infocrat Solutions Ltd.", string sMailHostName = "smtp.gmail.com", string sPortNumber = "587", bool bSSLRequired = true)
        {
            try
            {
                MailMessage oMM = new MailMessage();
                foreach (string oItem in EmailTo)
                {
                    oMM.To.Add(oItem);
                }
                if (EmailToCC.Count() > 0) { foreach (string oItem in EmailToCC) { oMM.CC.Add(oItem.ToLower()); } }
                //oMM.From = new MailAddress("esimsol.service@gmail.com", "Infocrat Solutions Ltd.");
                oMM.From = new MailAddress(sSendingEmail, sDisplayName);
                oMM.Subject = Subject;
                oMM.IsBodyHtml = true;
                oMM.Body = BodyInformation;
                if (oAttachments.Count() > 0) { foreach (Attachment oItem in oAttachments) { oMM.Attachments.Add(oItem); } }
                
                SmtpClient oSmtpClient = new SmtpClient();
                oSmtpClient.Port = Convert.ToInt32(sPortNumber); //port (SSL): 465  && port (TSL): 587                
                oSmtpClient.Host = sMailHostName; //"smtp.gmail.com";
                oSmtpClient.EnableSsl = bSSLRequired;
                oSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                oSmtpClient.UseDefaultCredentials = false;                
                oSmtpClient.Credentials = new System.Net.NetworkCredential(sSendingEmail, sSendingEmailPassword); //new System.Net.NetworkCredential("esimsol.service@gmail.com", "ics@2007");                
                oSmtpClient.Send(oMM);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        public static string MinInHourMin(int TimeInMinute)
        {
            string S = "";
            if (TimeInMinute > 0)
            {
                if (TimeInMinute / 60 >= 1) { S += ((TimeInMinute - TimeInMinute % 60) / 60).ToString() + "h "; }
                if (TimeInMinute % 60 != 0) { S += (TimeInMinute % 60).ToString() + "m"; }
                return S;
            }
            else return "-";
        }

        public static string EnumerationFormatter(string text)
        {
            char[] arr = text.ToArray();

            text = "";
            foreach (char ch in arr)
            {
                text += (char.IsUpper(ch)) ? " " + ch.ToString() : ch.ToString();
            }

            return text;
        }

        public static string GetDuration(DateTime dtFrom, DateTime dtTo)
        {
            TimeSpan tSpan = dtTo - dtFrom;

            var nYear = Math.Truncate(tSpan.TotalDays / 365);
            var nMonth = Math.Truncate((tSpan.TotalDays % 365) / 30);
            var nDays = Math.Truncate((tSpan.TotalDays % 365) % 30);

            return string.Format("{0} Year, {1} Month, {2} Day", nYear, nMonth, nDays);
        }

        public static string DefaultEncodeValue()
        {
            return Convert.ToBase64String(Encoding.Default.GetBytes("efxmoode "));
        }

        public static string GetEncodeValue(string text)
        {
            text = "efxmoode " + text;
            return Convert.ToBase64String(Encoding.Default.GetBytes(text));
        }
   
        public static string GetDecodeValue(string text)
        {
            try
            {
                Decoder decoder = Encoding.UTF8.GetDecoder();
                byte[] bytes = Convert.FromBase64String(text);
                int count = decoder.GetCharCount(bytes, 0, bytes.Count());
                char[] arr = new char[count];
                decoder.GetChars(bytes, 0, bytes.Count(), arr, 0);
                return new string(arr);
            }
            catch
            {
                return "";
            }
        
        }

        public static string GetActualToken(string token)
        {
            return GetDecodeValue(token.Replace(Global.DefaultEncodeValue(), ""));
        }
   
        public static string DateSQLGenerator(string fieldName, short compareValue, DateTime dtFrom, DateTime dtTo, bool IsDateTime)
        {
            string sSQL = (IsDateTime)? fieldName : "Convert(date, " + fieldName+ ")";
            if (compareValue == 1) // Eqaul
            {
                if (IsDateTime)
                    sSQL = sSQL + " = '" + dtFrom.ToString("dd MMM yyyy hh:mm")+"'";
                else
                    sSQL = sSQL + " = '" + dtFrom.ToString("dd MMM yyyy") + "'";
            }

            else if (compareValue == 2) // Not Equal
            {
                if (IsDateTime)
                    sSQL = sSQL + " != '" + dtFrom.ToString("dd MMM yyyy hh:mm") + "'";
                else
                    sSQL = sSQL + " != '" + dtFrom.ToString("dd MMM yyyy") + "'";
            }
            else if (compareValue == 3) // Greater Than
            {
                if (IsDateTime)
                    sSQL = sSQL + " > '" + dtFrom.ToString("dd MMM yyyy hh:mm") + "'";
                else
                    sSQL = sSQL + " > '" + dtFrom.ToString("dd MMM yyyy") + "'";
            }
            
            else if (compareValue == 4) // Smaller Than
            {
                if (IsDateTime)
                    sSQL = sSQL + " < '" + dtFrom.ToString("dd MMM yyyy hh:mm") + "'";
                else
                    sSQL = sSQL + " < '" + dtFrom.ToString("dd MMM yyyy") + "'";
            }
            else if (compareValue == 5) // Between
            {
                if (IsDateTime)
                    sSQL = sSQL + " Between '" + dtFrom.ToString("dd MMM yyyy hh:mm")  + "' And '" + dtTo.ToString("dd MMM yyyy hh:mm") + "'";
                else
                    sSQL = sSQL + " Between '" + dtFrom.ToString("dd MMM yyyy")  + "' And '" + dtTo.ToString("dd MMM yyyy") + "'";
            }
            else if (compareValue == 6) // Not Between
            {
                if (IsDateTime)
                    sSQL = sSQL + " Not Between '" + dtFrom.ToString("dd MMM yyyy hh:mm") + "' And ''" + dtTo.ToString("dd MMM yyyy hh:mm") + "'";
                else
                    sSQL = sSQL + " Not Between '" + dtFrom.ToString("dd MMM yyyy")  + "' And '" + dtTo.ToString("dd MMM yyyy") + "'";
            }
            return sSQL;
        }

        public static string CapitalSpilitor(string text)
        {
            char[] arr = text.ToArray();

            text = "";
            foreach (char ch in arr)
            {

                text += (char.IsUpper(ch)) ? " " + ch.ToString() : ch.ToString();
            }
            
            return text.Trim();
        }
        #region Qty or amount return
        public static string GetValue(double nVal)
        {
            if (nVal < 0) return "(" + Global.MillionFormat(nVal * (-1)) + ")";
            else if (nVal == 0) return "-";
            else return Global.MillionFormat(nVal);
        }
        #endregion
    }
}