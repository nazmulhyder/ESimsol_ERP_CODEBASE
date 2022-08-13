using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing;using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
namespace ESimSol.Reports
{
	public class rptKnitDyeingBatchGrayChallans 
	{
		#region Declaration

Document _oDocument;
		iTextSharp.text.Font _oFontStyle;
		PdfPTable _oPdfPTable = new PdfPTable(5);
		PdfPCell _oPdfPCell;
		iTextSharp.text.Image _oImag;
		MemoryStream _oMemoryStream = new MemoryStream();
		KnitDyeingBatchGrayChallan _oKnitDyeingBatchGrayChallan = new KnitDyeingBatchGrayChallan();
		List<KnitDyeingBatchGrayChallan> _oKnitDyeingBatchGrayChallans = new  List<KnitDyeingBatchGrayChallan>();
Company _oCompany = new Company();
		string _sMessage = "";
		#endregion

		#region Prepare Functions

		#endregion

		#region Report Header

		#endregion

		#region Report Body

		#endregion

	}

}
