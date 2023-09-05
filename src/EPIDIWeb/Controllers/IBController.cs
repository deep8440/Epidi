using DocumentFormat.OpenXml.Office2010.Excel;
using ClosedXML.Excel;
using System.Text;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.BDM;
using Epidi.Models.ViewModel.CommissionRule;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.IB;
using Epidi.Models.ViewModel.IBCommissionMarkUpSettingInstrumentWise;
using Epidi.Models.ViewModel.Instrument;
using Epidi.Models.ViewModel.Steps;
using Epidi.Notifications;
using Epidi.Repository.BDM;
using Epidi.Repository.IBRepository;
using Epidi.Service.BDM;
using Epidi.Service.CountryService;
using Epidi.Service.Helpers;
using Epidi.Service.IBCommisionMarkUpSettingInstrumentWise;
using Epidi.Service.IBLimitService;
using Epidi.Service.IBService;
using Epidi.Service.LegalEntityService;
using EPIDIWeb.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Diagnostics.Metrics;
using DocumentFormat.OpenXml.Wordprocessing;
using Epidi.Models.ViewModel.LeverageRules;
using Newtonsoft.Json;
using System.Data;
using NPOI.SS.Formula.Functions;

namespace EPIDIWeb.Controllers
{
    public class IBController : Controller
    {
        private readonly IIBService _IBServices;
        private readonly IIBCommisionMarkUpSettingInstWiseService _iIBCommisionMarkUpSettingInstWiseService;
        private readonly IBDMService _BDMService;
        private readonly ILegalEntityService _LegalEntityService;
        private readonly ICountryService _countryService;
        private readonly IIBLimitService _iIBLimitService;
        public IBController(IIBService iBServices, IIBCommisionMarkUpSettingInstWiseService iIBCommisionMarkUpSettingInstWiseService, IBDMService bDMService, ILegalEntityService legalEntityService, ICountryService countryService, IIBLimitService iIBLimitService)
        {
            _IBServices = iBServices;
            _iIBCommisionMarkUpSettingInstWiseService = iIBCommisionMarkUpSettingInstWiseService;
            _BDMService = bDMService;
            _LegalEntityService = legalEntityService;
            _countryService = countryService;
            _iIBLimitService = iIBLimitService;
        }
        public IActionResult Index(string name)
        {
            ViewBag.DetailName = name;
            return View();
        }
        //public IActionResult IBCommMarkUpSettingInstWise()
        //{
        //    return View();
        //}
        [HttpPost]
        //public JsonResult Get_All(string Id)
        //{
        //    {
        //        long totalRecord = 0;
        //        long filteredRecord = 0;
        //        var Draw = Request.Form["draw"].FirstOrDefault();
        //        PageParam pageParam = new PageParam();

        //        pageParam.Offset = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");


        //        pageParam.Limit = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
        //        string search = Convert.ToString(Request.Form["search[value]"].FirstOrDefault() ?? "");
        //        var response = _IBServices.GetAllBy_BDMID(pageParam, search, Id);

        //        totalRecord = response.Item2;
        //        filteredRecord = response.Item2;

        //        return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = response.Item1 });
        //    }
        //}
        [HttpPost]
        public JsonResult Get_AllIB_Comm()
        {
            {
                long totalRecord = 0;
                long filteredRecord = 0;
                var Draw = Request.Form["draw"].FirstOrDefault();
                PageParam pageParam = new PageParam();

                pageParam.Offset = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");


                pageParam.Limit = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
                string search = Convert.ToString(Request.Form["search[value]"].FirstOrDefault() ?? "");
                var response = _iIBCommisionMarkUpSettingInstWiseService.GetAll(pageParam, search);

                totalRecord = response.Item2;
                filteredRecord = response.Item2;

                return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = response.Item1 });
            }
        }
        public void GetBDMList()
        {
            PageParam pageParam = new PageParam();
            pageParam.Offset = 1;
            pageParam.Limit = 100;
            var response = _BDMService.GetAll(pageParam, "");

            var FieldsList = response.Item1.Select(p => new SelectListItem()
            {
                Value = p.Id.ToString(),
                Text = p.Name
            });

            ViewBag.BDMDropDown = FieldsList;
        }


        public async void GetLegalEntityList()
        {
            PageParam pageParam = new PageParam();
            pageParam.Offset = 1;
            pageParam.Limit = 100;
            var response = _LegalEntityService.GetLegalEntity_All(pageParam, "");

            var FieldsList = (await response.ConfigureAwait(false)).Item1.Select(p => new SelectListItem()
            {
                Value = p.Id.ToString(),
                Text = p.Name
            });

            ViewBag.LegalEntityDropDown = FieldsList;
        }


        public IActionResult AddIB()
        {
            GetBDMListALL();
            GetIBParentList();
            GetLegalEntityListALL();
            GetCountryListAll();
            IBViewModel iBViewModel = new IBViewModel();
            iBViewModel.ParentCommissionPercentageRebate = 3; // Default value
            iBViewModel.BDMCommissionPercentage = 1; // Default value
            return View(iBViewModel);
        }
        //public IActionResult AddIB(int BDMId)
        //{
        //    IBViewModel model = new IBViewModel();
        //    model.BDMId = BDMId;
        //    GetIBParentList();
        //    return View("AddIB", model);
        //}
        public void GetIBParentList()
        {
            PageParam pageParam = new PageParam();
            pageParam.Offset = 0;
            pageParam.Limit = 100;
            var response = _IBServices.GetAll(pageParam, "");
            var FieldsList = response.Item1.Select(p => new SelectListItem()
            {
                Value = p.Id.ToString(),
                Text = p.Name
            });
            ViewBag.IBParentDropDown = FieldsList;
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> SaveIB(IBViewModel model)
        {
            try
            {
                ResponseViewModel responseViewModel = new ResponseViewModel();
                if (model != null)
                {

                    var res = await _IBServices.Upsert(model);
                    if (res != null)
                    {
                        responseViewModel.Code = 200;
                        responseViewModel.Message = "IB Saved Successfully";
                    }
                    else
                    {
                        responseViewModel.Code = 400;
                        responseViewModel.Message = "IB Not Saved";
                    }

                    if (model.File != null)
                    {
                        IWorkbook workbook;
                        var supportedTypes = new[] { "xls", "xlsx" };
                        var fileExt = Path.GetExtension(model.File.FileName);
                        fileExt = fileExt.Replace(".", "");
                        if (!supportedTypes.Contains(fileExt))
                        {
                            return Json(new { StatusCode = 400, Message = "Please Select valid file." });
                        }
                        else
                        {
                            if (fileExt.Equals("-xls"))
                            {
                                //FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                                workbook = new HSSFWorkbook(model.File.OpenReadStream(), true);
                            }
                            else
                            {
                                workbook = new XSSFWorkbook(model.File.OpenReadStream());
                            }

                            ISheet sheet = workbook.GetSheetAt(0);
                            try
                            {
                                IRow HeaderRow = sheet.GetRow(0);

                                string Instrument;
                                decimal MarkUpPer100;
                                decimal MarkUpPer;
                                decimal BuySwapPer1000;
                                decimal BuySwapPer;
                                decimal SellSwapPer1000;
                                decimal SellSwapPer;
                                int IbId;
                                var myList = new List<string>();
                                for (int rowNo = 1; rowNo < sheet.PhysicalNumberOfRows; rowNo++)
                                {
                                    IRow row = sheet.GetRow(rowNo);
                                    if (row != null)
                                    {
                                        Instrument = row.GetCell(0) != null ? Convert.ToString(row.GetCell(0)) : "";
                                        MarkUpPer100 = row.GetCell(1) != null ? Convert.ToDecimal(row.GetCell(1).ToString()) : 0;
                                        MarkUpPer = row.GetCell(2) != null ? Convert.ToDecimal(row.GetCell(2).ToString()) : 0;
                                        BuySwapPer1000 = row.GetCell(3) != null ? Convert.ToDecimal(row.GetCell(3).ToString()) : 0;
                                        BuySwapPer = row.GetCell(4) != null ? Convert.ToDecimal(row.GetCell(4).ToString()) : 0;
                                        SellSwapPer1000 = row.GetCell(5) != null ? Convert.ToDecimal(row.GetCell(5).ToString()) : 0;
                                        SellSwapPer = row.GetCell(6) != null ? Convert.ToDecimal(row.GetCell(6).ToString()) : 0;
                                        IbId = res.Id;
                                        Tuple<string, decimal, decimal, decimal, decimal, decimal, decimal, Tuple<int>> arguments = new Tuple<string, decimal, decimal, decimal, decimal, decimal, decimal, Tuple<int>>
                                            (Instrument, MarkUpPer100, MarkUpPer, BuySwapPer1000, BuySwapPer, SellSwapPer1000, SellSwapPer, new Tuple<int>(IbId));

                                        List<IBLimitViewModel> modelExport = new List<IBLimitViewModel>();
                                        modelExport = _iIBLimitService.GetIBLimit_All_Data();
                                        bool allcheck = true;

                                        var duplicate = modelExport.Find(o => o.MasterInstrumentName == Instrument);
                                        if (duplicate != null)
                                        {
                                            string iBCheckMessege = "Instrument :- " + Instrument;
                                            if (MarkUpPer100 > duplicate.MarkUpPer1000)
                                            {
                                                allcheck = false;
                                                iBCheckMessege = string.Format(iBCheckMessege + Environment.NewLine + "IB MarkUpPer 1000 is more than the IBLimit MarkUpPer 1000");
                                            }
                                            if (MarkUpPer > duplicate.MarkUpPer)
                                            {
                                                allcheck = false;
                                                iBCheckMessege = string.Format(iBCheckMessege + Environment.NewLine + "IB MarkUpPer is more than the IBLimit MarkUpPer");
                                            }
                                            if (BuySwapPer1000 > duplicate.BuySwapPer1000)
                                            {
                                                allcheck = false;
                                                iBCheckMessege = string.Format(iBCheckMessege + Environment.NewLine + "IB BuySwapPer1000 is more than the IBLimit BuySwapPer1000");
                                            }
                                            if (BuySwapPer > duplicate.BuySwapPer)
                                            {
                                                allcheck = false;
                                                iBCheckMessege = string.Format(iBCheckMessege + Environment.NewLine + "IB BuySwapPer is more than the IBLimit BuySwapPer");
                                            }
                                            if (SellSwapPer1000 > duplicate.SellSwapPer1000)
                                            {
                                                allcheck = false;
                                                iBCheckMessege = string.Format(iBCheckMessege + Environment.NewLine + "IB SellSwapPer1000 is more than the IBLimit SellSwapPer1000");
                                            }
                                            if (SellSwapPer > duplicate.SellSwapPer)
                                            {
                                                allcheck = false;
                                                iBCheckMessege = string.Format(iBCheckMessege + Environment.NewLine + "IB SellSwapPer is more than the IBLimit SellSwapPer");
                                            }
                                            iBCheckMessege = iBCheckMessege.Replace("\r\n", "\\n");
                                            myList.Add(iBCheckMessege);
                                        }
                                        if (allcheck)
                                        {
                                            var response = await _IBServices.Import(arguments, model.BDMId);
                                        }

                                    }

                                }
                                TempData["IBDupMessege"] = myList;
                                return Json(responseViewModel);
                            }
                            catch (Exception ex)
                            {

                                return Json("");
                            }
                        }

                        return Json(responseViewModel);
                    }
                }
                else
                {
                    return Json("");
                }
                return Json(responseViewModel);
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            ViewBag.PageTitle = "Edit IB";
            ViewBag.Method = "IB";

            var response = await _IBServices.GetById(Id);
            GetBDMListALL();
            GetLegalEntityListALL();
            GetIBParentList();
            GetCountryListAll();

            if (response != null)
            {
                return View("EditIB", response);
            }
            else
            {
                return View(new IBViewModel());
            }


        }

        [HttpGet]
        public async Task<JsonResult> Get_IBCommitionId(int Rule_Id)
        {
            var response = await _IBServices.GetIBCommitionById(Rule_Id);
            return Json(response);
        }

        public async Task<JsonResult> GetInstruments()
        {
            List<SelectListItem> oprationList = new List<SelectListItem>();
            //var FieldsList = _BDMService.Get_All_Instrument().Result.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name });
            var FieldsList = _BDMService.Get_All_Instrument().Result.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name });
            //var country = _countryService.GetAllCountries().Result.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.CountryName });
            foreach (var item in FieldsList)
            {
                oprationList.Add(new SelectListItem { Text = item.Text, Value = item.Value });
            }
            return Json(oprationList);
        }


        //public void GetInstrumentss()
        //{
        //    var FieldsList = _BDMService.Get_All_Instrument().Result.Select(p => new SelectListItem()
        //    {
        //        Value = p.Id.ToString(),
        //        Text = p.Name
        //    });

        //    ViewBag.InstrumentList = FieldsList;
        //}
        //public Task<IActionResult> IBCommMarkUpSettingInstWise(int IBID, string IBName, int BDMID)
        //{
        //    GetInstruments();
        //    var model = new IBCommisionMarkUpSettingInstrumentWiseViewModel();
        //    if (IBID > 0)
        //    {
        //        var response = _iIBCommisionMarkUpSettingInstWiseService.GetIBMCommision_IBID(IBID,BDMID);
        //        if (response != null)
        //        {
        //            return View("IBCommMarkUpSettingInstWise", response);
        //        }
        //        else
        //        {
        //            if (IBName != "")
        //            {
        //                model.ib_name = IBName;
        //                model.BDMID = BDMID;
        //            }
        //            goto CallBlankModel;
        //        }
        //    }
        //    else
        //    {
        //        goto CallBlankModel;
        //    }

        //////goto statement is jump statement
        //CallBlankModel:
        //    return View("IBCommMarkUpSettingInstWise", model);
        //}
        [HttpPost]
        public async Task<IActionResult> SaveIBMCommision(IBCommisionMarkUpSettingInstrumentWiseViewModel model)
        {
            try
            {
                ResponseViewModel responseViewModel = new ResponseViewModel();
                if (model != null)
                {
                    var res = await _iIBCommisionMarkUpSettingInstWiseService.Upsert(model);
                    if (res != null)
                    {
                        responseViewModel.Code = 200;
                        responseViewModel.Message = "BI Saved Successfully";
                    }
                    else
                    {
                        responseViewModel.Code = 400;
                        responseViewModel.Message = "BI Not Saved";
                    }
                    return Json(responseViewModel);
                }
                else
                {
                    return Json("");
                }
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }
        [HttpPost]
        public async Task<JsonResult> SaveIB_new(IBViewModel model, IFormFile FileUpload)
        {
            try
            {
                var response = await _IBServices.Upsert(model);
                FileUpload = model.File;
                List<IBCommisionMarkUpSettingInstrumentWiseViewModel> IBModel = new List<IBCommisionMarkUpSettingInstrumentWiseViewModel>();
                if (FileUpload != null)
                {
                    IWorkbook workbook;
                    var supportedTypes = new[] { "xls", "xlsx" };
                    var fileExt = Path.GetExtension(FileUpload.FileName);
                    fileExt = fileExt.Replace(".", "");
                    if (!supportedTypes.Contains(fileExt))
                    {
                        return Json(new { StatusCode = 400, Message = "Please Select valid file." });
                    }
                    else
                    {
                        if (fileExt.Equals("-xls"))
                        {
                            //FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                            workbook = new HSSFWorkbook(FileUpload.OpenReadStream(), true);
                        }
                        else
                        {
                            workbook = new XSSFWorkbook(FileUpload.OpenReadStream());
                        }

                        ISheet sheet = workbook.GetSheetAt(0);

                        try
                        {
                            IRow HeaderRow = sheet.GetRow(0);



                            string Name;
                            decimal markupper1000;
                            decimal markupper;
                            decimal Buyswapper1000;
                            decimal Buyswapper;
                            decimal Sellswapper1000;
                            decimal Sellswapper;
                            var myList = new List<string>();

                            for (int rowNo = 1; rowNo < sheet.PhysicalNumberOfRows; rowNo++)
                            {
                                IRow row = sheet.GetRow(rowNo);


                                Name = row.GetCell(0) != null ? row.GetCell(0).ToString() : "";
                                markupper1000 = row.GetCell(1) != null ? Convert.ToDecimal(row.GetCell(1).ToString()) : 0;
                                markupper = row.GetCell(2) != null ? Convert.ToDecimal(row.GetCell(2).ToString()) : 0;
                                Buyswapper1000 = row.GetCell(3) != null ? Convert.ToDecimal(row.GetCell(3).ToString()) : 0;
                                Buyswapper = row.GetCell(4) != null ? Convert.ToDecimal(row.GetCell(4).ToString()) : 0;
                                Sellswapper1000 = row.GetCell(5) != null ? Convert.ToDecimal(row.GetCell(5).ToString()) : 0;
                                Sellswapper = row.GetCell(6) != null ? Convert.ToDecimal(row.GetCell(6).ToString()) : 0;


                                IBCommisionMarkUpSettingInstrumentWiseViewModel IB = new IBCommisionMarkUpSettingInstrumentWiseViewModel();
                                IB.InstrumentalName = Name;
                                IB.MarkUpPer = Convert.ToDouble(markupper);
                                IB.MarkUpPer1000 = Convert.ToDouble(markupper1000);
                                IB.BuySwapPer = Convert.ToDouble(Buyswapper);
                                IB.BuySwapPer1000 = Convert.ToDouble(Buyswapper1000);
                                IB.SellSwapPer = Convert.ToDouble(Sellswapper);
                                IB.SellSwapPer1000 = Convert.ToDouble(Sellswapper1000);
                                IB.BDMID = response.BDMId;
                                IB.IBID = response.Id;
                                //IB.IBID = response.ParentIBId;
                                List<IBLimitViewModel> modelExport = new List<IBLimitViewModel>();
                                modelExport = _iIBLimitService.GetIBLimit_All_Data();
                                bool allcheck = true;

                                var duplicate = modelExport.Find(o => o.MasterInstrumentName == Name);
                                if (duplicate != null)
                                {
                                    string iBCheckMessege = "Instrument :- " + Name;
                                    if (IB.MarkUpPer1000 > duplicate.MarkUpPer1000)
                                    {
                                        allcheck = false;
                                        iBCheckMessege = string.Format(iBCheckMessege + Environment.NewLine + "IB MarkUpPer 1000 is more than the IBLimit MarkUpPer 1000");
                                    }
                                    if (IB.MarkUpPer > duplicate.MarkUpPer)
                                    {
                                        allcheck = false;
                                        iBCheckMessege = string.Format(iBCheckMessege + Environment.NewLine + "IB MarkUpPer is more than the IBLimit MarkUpPer");
                                    }
                                    if (IB.BuySwapPer1000 > duplicate.BuySwapPer1000)
                                    {
                                        allcheck = false;
                                        iBCheckMessege = string.Format(iBCheckMessege + Environment.NewLine + "IB BuySwapPer1000 is more than the IBLimit BuySwapPer1000");
                                    }
                                    if (IB.BuySwapPer > duplicate.BuySwapPer)
                                    {
                                        allcheck = false;
                                        iBCheckMessege = string.Format(iBCheckMessege + Environment.NewLine + "IB BuySwapPer is more than the IBLimit BuySwapPer");
                                    }
                                    if (IB.SellSwapPer1000 > duplicate.SellSwapPer1000)
                                    {
                                        allcheck = false;
                                        iBCheckMessege = string.Format(iBCheckMessege + Environment.NewLine + "IB SellSwapPer1000 is more than the IBLimit SellSwapPer1000");
                                    }
                                    if (IB.SellSwapPer > duplicate.SellSwapPer)
                                    {
                                        allcheck = false;
                                        iBCheckMessege = string.Format(iBCheckMessege + Environment.NewLine + "IB SellSwapPer is more than the IBLimit SellSwapPer");
                                    }
                                    iBCheckMessege = iBCheckMessege.Replace("\r\n", "\\n");
                                    myList.Add(iBCheckMessege);
                                }
                                if (allcheck)
                                {
                                    var res = await _iIBCommisionMarkUpSettingInstWiseService.Upsert(IB);
                                }

                            }



                            TempData["IBUpdateDupMessege"] = myList;
                            return Json(new { StatusCode = 200, Message = "File Imported Successfully" });
                        }
                        catch (Exception ex)
                        {
                            return Json(new { StatusCode = 400, Message = ex.Message.ToString() });
                        }

                    }
                }
                else
                {
                    ListToDataTableConverter converter = new ListToDataTableConverter();

                    model.objiBCommisionMarkUpSettingInstrument = JsonConvert.DeserializeObject<List<IBCommisionMarkUpSettingInstrumentWiseViewModel>>(model.objIBCommissionMarkUpSettingInstrumentWise);
                    DataTable dtInstrumentsDtl = converter.ToDataTable(model.objiBCommisionMarkUpSettingInstrument);
                    for (int cnt = 0; cnt < dtInstrumentsDtl.Rows.Count; cnt++)
                    {
                        IBCommisionMarkUpSettingInstrumentWiseViewModel IB = new IBCommisionMarkUpSettingInstrumentWiseViewModel();
                        IB.InstrumentalName = Convert.ToString(dtInstrumentsDtl.Rows[cnt]["InstrumentalName"]).Trim();
                        IB.MarkUpPer = Convert.ToDouble(dtInstrumentsDtl.Rows[cnt]["MarkUpPer"]);
                        IB.MarkUpPer1000 = Convert.ToDouble(dtInstrumentsDtl.Rows[cnt]["MarkUpPer1000"]);
                        IB.BuySwapPer = Convert.ToDouble(dtInstrumentsDtl.Rows[cnt]["BuySwapPer"]);
                        IB.BuySwapPer1000 = Convert.ToDouble(dtInstrumentsDtl.Rows[cnt]["BuySwapPer1000"]);
                        IB.SellSwapPer = Convert.ToDouble(dtInstrumentsDtl.Rows[cnt]["SellSwapPer"]);
                        IB.SellSwapPer1000 = Convert.ToDouble(dtInstrumentsDtl.Rows[cnt]["SellSwapPer1000"]);
                        IB.BDMID = response.BDMId;
                        IB.IBID = response.Id;
                        var res = await _iIBCommisionMarkUpSettingInstWiseService.Upsert(IB);
                    }

                }
                return Json(new { StatusCode = 200, Message = "IB Updated Successfully" });
            }
            catch
            {
                return Json(null);
            }
            return Json("");
        }
        public IActionResult IBListing()
        {
            GetBDMListALL();
            GetLegalEntityListALL();
            GetIBParentList();
            GetCountryListAll();
            ViewBag.IBDuplicateMessege = Convert.ToString(TempData["IBDuplicateMessege"]);
            ViewBag.IBUpdateMessege = Convert.ToString(TempData["IBUpdateMessege"]);
            ViewBag.IBValidationMessege = Convert.ToString(TempData["IBValidationMessege"]);
            ViewBag.IBAddMessege = Convert.ToString(TempData["IBAddMessege"]);
            TempData["IBDupMessege"] = TempData["IBDupMessege"] as string[];
            TempData["IBUpdateDupMessege"] = TempData["IBUpdateDupMessege"] as string[];
            // ViewBag.IBDupMessege = TempData["IBDupMessege"].ToString();
            return View();
        }


        public void GetBDMListALL()
        {
            var DropdownlistBDM = _BDMService.GetBDMListALL().Result.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name });

            ViewBag.DropdownlistBDM = DropdownlistBDM;
        }

        public void GetLegalEntityListALL()
        {
            var DropdownlistLegalEntity = _LegalEntityService.GetLegalEntityListALL().Result.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name });

            ViewBag.DropdownlistLegalEntity = DropdownlistLegalEntity;
        }

        public void GetCountryListAll()
        {
            var DropdownlistCountry = _countryService.GetCountryListAll().Result.Select(p => new SelectListItem() { Value = p.CountryId.ToString(), Text = p.CountryName });

            ViewBag.DropdownlistCountry = DropdownlistCountry;


        }

        [HttpPost]
        //public JsonResult Get_All(string Id)
        //{
        //    {
        //        long totalRecord = 0;
        //        long filteredRecord = 0;
        //        var Draw = Request.Form["draw"].FirstOrDefault();
        //        PageParam pageParam = new PageParam();

        //        pageParam.Offset = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");


        //        pageParam.Limit = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
        //        string search = Convert.ToString(Request.Form["search[value]"].FirstOrDefault() ?? "");
        //        var response = _IBServices.GetAllBy_BDMID(pageParam, search, Id);

        //        totalRecord = response.Item2;
        //        filteredRecord = response.Item2;

        //        return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = response.Item1 });
        //    }
        //}

        public JsonResult GetAll(string Id)
        {
            {
                long totalRecord = 0;
                long filteredRecord = 0;
                var Draw = Request.Form["draw"].FirstOrDefault();
                PageParam pageParam = new PageParam();

                pageParam.Offset = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");


                pageParam.Limit = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
                string search = Convert.ToString(Request.Form["search[value]"].FirstOrDefault() ?? "");
                var response = _IBServices.GetAll(pageParam, search);

                totalRecord = response.Item2;
                filteredRecord = response.Item2;

                return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = response.Item1 });
            }
        }

        [HttpGet]
        public IActionResult DeleteIB(int Id)
        {
            var res = _IBServices.DeleteIB(Id);
            return Json(res);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteIBRuleInstrument(int IBId, int MasterInstrumentalID)
        {
            ResponseViewModel responseViewModel = new ResponseViewModel();

            var res = _IBServices.DeleteIBRuleInstrumentById(IBId, MasterInstrumentalID);
            if (res != null)
            {
                responseViewModel.Code = 200;
                responseViewModel.Message = "Commission Rule Deleted Successfully";
            }
            else
            {
                responseViewModel.Code = 400;
                responseViewModel.Message = "Commission Rule Not Deleted";
            }
            return Json(responseViewModel);
        }

        [HttpGet]

        public async Task<IActionResult> EditIBRuleInstrumentById(int IBId, int MasterInstrumentalID)
        {
            var EditIBInstrument = await _IBServices.GetIBRuleInstrumentById(IBId, MasterInstrumentalID);
            GetInstruments();
            if (EditIBInstrument != null)
            {
                return View("EditIBRuleInstrument", EditIBInstrument);
            }
            else
            {
                return View(new IBViewModel());
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> SaveIBCommissionInstrument(IBCommissionInstrument model)
        {

            ResponseViewModel responseViewModel = new ResponseViewModel();

            var res = _IBServices.SaveIBCommissionInstrument(model);
            if (res != null)
            {
                responseViewModel.Code = 200;
                responseViewModel.Message = "IB Commission Instrument Update Successfully";
            }
            else
            {
                responseViewModel.Code = 400;
                responseViewModel.Message = "IB Commission Instrument Not Updated";
            }
            TempData["IBUpdateMessege"] = "IB Commission Instrument Update Successfully ...";
            return Redirect("IBListing");
        }

        public async Task<IActionResult> ExportIBInstrument(int Id)
        {
            PageParam pageParam = new PageParam();
            pageParam.Offset = 0;
            pageParam.Limit = 100;
            List<IBCommisionMarkUpSettingInstrumentWiseViewModel> iBCommisionMarkUpSettingInstrumentWiseViewModel = new List<IBCommisionMarkUpSettingInstrumentWiseViewModel>();
            var result = await _IBServices.GetIBCommitionById(Id);
            iBCommisionMarkUpSettingInstrumentWiseViewModel = result;

            if (iBCommisionMarkUpSettingInstrumentWiseViewModel != null)
            {
                //return Json(new { draw = requestModel.Draw, recordsFiltered = responseModel.FilteredCount, recordsTotal = responseModel.TotalCount, data = datavalue });
                StringBuilder stringBuilder = new StringBuilder();

                string contextType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                string fileName = "IBInstrument_" + DateTime.Now.ToString("MM/dd/yyyy") + "_" + Id + ".xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("IBInstrument");

                    worksheet.Cell(1, 1).Value = "Instrument Name";
                    worksheet.Cell(1, 2).Value = "MarkUpPer1000";
                    worksheet.Cell(1, 3).Value = "MarkUpPer";
                    worksheet.Cell(1, 4).Value = "BuySwapPer1000";
                    worksheet.Cell(1, 5).Value = "BuySwapPer";
                    worksheet.Cell(1, 6).Value = "SellSwapPer1000";
                    worksheet.Cell(1, 7).Value = "SellSwapPer";

                    worksheet.Range("A1:N1").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                    //worksheet.Range("A1:N2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    for (int index = 1; index <= iBCommisionMarkUpSettingInstrumentWiseViewModel.Count; index++)
                    {
                        //worksheet.Cell(index + 1, 1).Style.NumberFormat.SetFormat("0");
                        //worksheet.Cell(index + 1, 1).Value = index;
                        worksheet.Cell(index + 1, 1).Value = iBCommisionMarkUpSettingInstrumentWiseViewModel[index - 1].InstrumentalName;
                        worksheet.Cell(index + 1, 2).Style.NumberFormat.SetFormat("0.00");
                        worksheet.Cell(index + 1, 2).Value = iBCommisionMarkUpSettingInstrumentWiseViewModel[index - 1].MarkUpPer1000;
                        worksheet.Cell(index + 1, 3).Style.NumberFormat.SetFormat("0.00");
                        worksheet.Cell(index + 1, 3).Value = iBCommisionMarkUpSettingInstrumentWiseViewModel[index - 1].MarkUpPer;
                        worksheet.Cell(index + 1, 4).Style.NumberFormat.SetFormat("0.00");
                        worksheet.Cell(index + 1, 4).Value = iBCommisionMarkUpSettingInstrumentWiseViewModel[index - 1].BuySwapPer1000;
                        worksheet.Cell(index + 1, 5).Style.NumberFormat.SetFormat("0.00");
                        worksheet.Cell(index + 1, 5).Value = iBCommisionMarkUpSettingInstrumentWiseViewModel[index - 1].BuySwapPer;
                        worksheet.Cell(index + 1, 6).Style.NumberFormat.SetFormat("0.00");
                        worksheet.Cell(index + 1, 6).Value = iBCommisionMarkUpSettingInstrumentWiseViewModel[index - 1].SellSwapPer1000;
                        worksheet.Cell(index + 1, 7).Style.NumberFormat.SetFormat("0.00");
                        worksheet.Cell(index + 1, 7).Value = iBCommisionMarkUpSettingInstrumentWiseViewModel[index - 1].SellSwapPer;
                    }

                    worksheet.Columns().AdjustToContents();
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content, contextType, fileName);
                    }
                }
            }
            else
            {
                return Json("");
            }
        }
        [HttpGet]

        public async Task<IActionResult> AddInstrumentRule(int Id)
        {
            GetInstruments();
            IBCommissionInstrument iBCommissionInstrument = new IBCommissionInstrument();
            iBCommissionInstrument.IBID = Id;
            return View("AddIBInstrumentRule", iBCommissionInstrument);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> SaveIBInstrumentRule(IBCommissionInstrument model)
        {

            ResponseViewModel responseViewModel = new ResponseViewModel();

            var checkres = await _IBServices.CheckIBRule(model.IBID, model.MasterInstrumentId);
            if (checkres != true)
            {
                List<IBLimitViewModel> modelExport = new List<IBLimitViewModel>();
                modelExport = _iIBLimitService.GetIBLimit_All_Data();
                bool allcheck = true;

                var duplicate = modelExport.Find(o => o.MasterInstrumentId == model.MasterInstrumentId);
                if (duplicate != null)
                {
                    string iBCheckMessege = "";
                    if (model.MarkUpPer1000 > duplicate.MarkUpPer1000)
                    {
                        allcheck = false;
                        iBCheckMessege = string.Format(iBCheckMessege + Environment.NewLine + "IB MarkUpPer 1000 is more than the IBLimit MarkUpPer 1000");
                    }
                    if (model.MarkUpPer > duplicate.MarkUpPer)
                    {
                        allcheck = false;
                        iBCheckMessege = string.Format(iBCheckMessege + Environment.NewLine + "IB MarkUpPer is more than the IBLimit MarkUpPer");
                    }
                    if (model.BuySwapPer1000 > duplicate.BuySwapPer1000)
                    {
                        allcheck = false;
                        iBCheckMessege = string.Format(iBCheckMessege + Environment.NewLine + "IB BuySwapPer1000 is more than the IBLimit BuySwapPer1000");
                    }
                    if (model.BuySwapPer > duplicate.BuySwapPer)
                    {
                        allcheck = false;
                        iBCheckMessege = string.Format(iBCheckMessege + Environment.NewLine + "IB BuySwapPer is more than the IBLimit BuySwapPer");
                    }
                    if (model.SellSwapPer1000 > duplicate.SellSwapPer1000)
                    {
                        allcheck = false;
                        iBCheckMessege = string.Format(iBCheckMessege + Environment.NewLine + "IB SellSwapPer1000 is more than the IBLimit SellSwapPer1000");
                    }
                    if (model.SellSwapPer > duplicate.SellSwapPer)
                    {
                        allcheck = false;
                        iBCheckMessege = string.Format(iBCheckMessege + Environment.NewLine + "IB SellSwapPer is more than the IBLimit SellSwapPer");
                    }
                    iBCheckMessege = iBCheckMessege.Replace("\r\n", "\\n");
                    if (allcheck == false)
                    {
                        TempData["IBDuplicateMessege"] = iBCheckMessege;
                        return RedirectToAction("IBListing");
                    }
                }
                else
                {
                    var res = _IBServices.SaveIBInstrumentRule(model);
                    if (res != null)
                    {
                        responseViewModel.Code = 200;
                        responseViewModel.Message = "IB Commission Instrument Update Successfully";
                    }
                    else
                    {
                        responseViewModel.Code = 400;
                        responseViewModel.Message = "IB Commission Instrument Not Updated";
                    }
                    TempData["IBAddMessege"] = "IB Commission Instrument Add Successfully ...";
                    return Redirect("IBListing");
                }
            }
            TempData["IBDuplicateMessege"] = "Add another one IB Instrument Rule when the IB Instrument Rule is already there ...";
            return RedirectToAction("IBListing");
        }
    }

}
