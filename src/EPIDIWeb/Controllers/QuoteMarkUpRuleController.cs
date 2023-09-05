using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.IBCommissionMarkUpSettingInstrumentWise;
using Epidi.Models.ViewModel.Instrument;
using Epidi.Models.ViewModel.InstrumentMaster;
using Epidi.Models.ViewModel.LeverageRules;
using Epidi.Models.ViewModel.QuoteMarkUpRule;
using Epidi.Models.ViewModel.RuleCondition;
using Epidi.Models.ViewModel.RuleInstrument;
using Epidi.Models.ViewModel.RuleLpPriority;
using Epidi.Models.ViewModel.TradeRule;
using Epidi.Service.BDM;
using Epidi.Service.InstrumentService;
using Epidi.Service.MasterLPService;
using Epidi.Service.QuoteMarkUpRuleService;
using Epidi.Service.RuleConditionService;
using Epidi.Service.RuleInstrumentService;
using Epidi.Service.RuleLpPriorityService;
using EPIDIWeb.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using System.Diagnostics.Metrics;

namespace EPIDIWeb.Controllers
{
    public class QuoteMarkUpRuleController : Controller
    {
        private readonly IQuoteMarkUpRuleService _IQuoteMarkUpRuleService;
        private readonly IRuleLpPriorityService _ruleLpPriorityService;
        private readonly IRuleConditionService _ruleConditionService;
        private readonly IBDMService _BDMService;
        private readonly IMasterLPService _masterLPService;
        private readonly IRuleInstrumentService _ruleInstrumentService;
        private readonly IInstrumentService _instrumentService;

        public QuoteMarkUpRuleController(IQuoteMarkUpRuleService iQuoteMarkUpRuleService, IRuleConditionService ruleConditionService,
            IRuleLpPriorityService ruleLpPriorityService, IBDMService iBDMService, IMasterLPService masterLPService,
            IRuleInstrumentService ruleInstrumentService, IInstrumentService instrumentService)
        {
            _IQuoteMarkUpRuleService = iQuoteMarkUpRuleService;
            _BDMService = iBDMService;
            _masterLPService = masterLPService;
            _ruleLpPriorityService = ruleLpPriorityService;
            _ruleConditionService = ruleConditionService;
            _ruleInstrumentService = ruleInstrumentService;
            _instrumentService = instrumentService;
        }
        public IActionResult Index()
        {
            ViewBag.Title = "QuoteMarkUpRule List ";
            return View();
        }
        public IActionResult AddQuoteMarkUpRule()
        {
            GetInstruments();
            GetMarketLP();
            return View(new QuoteMarkUpRuleViewModel());
        }
        public IActionResult EditQuoteMarkUpRule()
        {
            GetInstruments();
            GetMarketLP();
            return View(new QuoteMarkUpRuleViewModel());
        }
        public IActionResult QuoteMarkUpRuleImport()
        {
            return View(new QuoteMarkUpRuleViewModel());
        }
        [HttpPost]
        public JsonResult Get_All()
        {
            long totalRecord = 0;
            long filteredRecord = 0;
            var Draw = Request.Form["draw"].FirstOrDefault();
            PageParam pageParam = new PageParam();

            pageParam.Offset = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");

            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            pageParam.Limit = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            string search = Convert.ToString(Request.Form["search[value]"].FirstOrDefault() ?? "");
            var response = _IQuoteMarkUpRuleService.GetAll(pageParam, search);

            totalRecord = response.Item2;
            filteredRecord = response.Item2;

            var responseData = OrderByExtension.OrderBy(response.Item1.AsQueryable(), sortColumn, sortColumnDirection).ToList();

            return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = responseData });
        }

        //[ValidateAntiForgeryToken]
        //[HttpPost]
        //public async Task<IActionResult> SaveQuoteMarkUpRule(QuoteMarkUpRuleViewModel model)
        //{
        //    try
        //    {
        //        ResponseViewModel responseViewModel = new ResponseViewModel();
        //        if (model != null)
        //        {
        //            var res = await _IQuoteMarkUpRuleService.Upsert(model);
        //            model.objRuleLpPriorityViewModel.RuleId = Convert.ToInt32(res.Id);
        //            var res1 = await _ruleLpPriorityService.Upsert(model.objRuleLpPriorityViewModel);
        //            if (res != null)
        //            {
        //                responseViewModel.Code = 200;
        //                responseViewModel.Message = "QuoteMarkUpRule Saved Successfully";
        //                responseViewModel.Id = Convert.ToInt32(res.Id);
        //            }
        //            else
        //            {
        //                responseViewModel.Code = 400;
        //                responseViewModel.Message = "QuoteMarkUpRule Not Saved";
        //            }
        //            return Json(responseViewModel);
        //        }
        //        else
        //        {
        //            return Json("");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(ex);
        //    }
        //}
        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var response = await _IQuoteMarkUpRuleService.GetById(Id);
            var ruleLP = await _ruleLpPriorityService.GetByRuleId(Id);
            var ruleConditions = await _ruleConditionService.GetByRuleId(Id);
            response.objRuleConditionViewModel = ruleConditions;
            if (ruleConditions != null)
            {
                response.objRuleConditionViewModel.conditionRulesDtlObj = JsonConvert.SerializeObject(ruleConditions.rulesDtlViewModel);
            }
            response.mode = "Edit";
            GetInstruments();
            GetMarketLP();

            if (response != null)
            {
                return View("AddQuoteMarkUpRule", response);
            }
            else
            {
                return View(new QuoteMarkUpRuleViewModel());
            }
        }
        public async Task<IActionResult> UploadTradeRule(IFormFile FileUpload, TradeRuleViewModel tradeRuleViewModel)
        {
            string filePath = string.Empty;
            try
            {
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
                        if (fileExt.Equals("xls"))
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

                            List<TradeRuleViewModelImport> _ruleInstrumentViewModel = new List<TradeRuleViewModelImport>();

                            int id = 0;
                            string MasterInstrumentName = "";
                            string SymbolGroups = "";
                            string IBName = "";
                            string LegalEntity = "";
                            string Country = "";
                            string UID = "";
                            int MasterInstrumentId = 0;
                            string Days = "";
                            string TimeTO;
                            string TimeFrom;

                            bool AutoupdateLPPriorityUp = false;

                            int ruleInstrumentIdCounter = 0;
                            for (int rowNo = 2; rowNo < sheet.PhysicalNumberOfRows; rowNo++)
                            {
                                IRow row = sheet.GetRow(rowNo);

                                MasterInstrumentName = row.GetCell(0) != null ? Convert.ToString(row.GetCell(0)) : "";
                                SymbolGroups = row.GetCell(1) != null ? row.GetCell(1).ToString() : "";
                                IBName = row.GetCell(2) != null ? row.GetCell(2).ToString() : "";
                                LegalEntity = row.GetCell(3) != null ? row.GetCell(3).ToString() : ""; ////Time To
                                Country = row.GetCell(4) != null ? row.GetCell(4).ToString() : "";////Time from
                                UID = row.GetCell(5) != null ? row.GetCell(5).ToString() : "";
                                Days = row.GetCell(7) != null ? row.GetCell(7).ToString() : "";
                                TimeTO = Convert.ToString(row.GetCell(8).DateCellValue.ToString("HH:mm:ss")) != null ? Convert.ToString(row.GetCell(8).DateCellValue.ToString("HH:mm:ss")) : "";
                                //row.GetCell(8) != null ? row.GetCell(8).ToString() : "";
                                TimeFrom = Convert.ToString(row.GetCell(9).DateCellValue.ToString("HH:mm:ss")) != null ? Convert.ToString(row.GetCell(9).DateCellValue.ToString("HH:mm:ss")) : "";
								//row.GetCell(9) != null ? row.GetCell(9).ToString() : "";

								TradeRuleViewModelImport model;
								ruleInstrumentIdCounter++;

								if (Convert.ToString(row.GetCell(10)) != "")
								{
									model = new TradeRuleViewModelImport();
									model.RuleId = tradeRuleViewModel.Id;
									model.RuleInstrumentIdCounter = ruleInstrumentIdCounter;
									model.Name = tradeRuleViewModel.Name.Trim();
									model.Priority = tradeRuleViewModel.Priority;
									model.MasterInstrumentName = MasterInstrumentName.Trim();
									model.SymbolGroups = SymbolGroups.Trim();
									model.Day = Days;
									model.TimeFrom = TimeFrom;
									model.TimeTo = TimeTO;
									model.LegalEntity = LegalEntity;
									model.CountryName = Country;
									model.Uid = UID;
									model.IBName = IBName;

									model.PriorityNo = row.GetCell(10) != null ? Convert.ToInt32(row.GetCell(10).NumericCellValue) : 0;
									model.LPName = row.GetCell(11) != null ? Convert.ToString(row.GetCell(11)).Trim() : "";
									model.HedgeType = row.GetCell(12) != null ? Convert.ToString(row.GetCell(12)).Trim() : "";
									model.VolumeFrom = row.GetCell(13) != null ? Convert.ToInt32(row.GetCell(13).NumericCellValue) : 0;
									model.VolumeTo = row.GetCell(14) != null ? Convert.ToInt32(row.GetCell(14).NumericCellValue) : 0;
									model.Coefficient = row.GetCell(15) != null ? Convert.ToDecimal(row.GetCell(15).NumericCellValue) : 0;
									model.PositionType = row.GetCell(16) != null ? Convert.ToString(row.GetCell(16)) : "";
									model.MaxCoefficient = row.GetCell(17) != null ? Convert.ToDecimal(row.GetCell(17).NumericCellValue) : 0;
									model.Value1 = row.GetCell(18) != null ? Convert.ToDecimal(row.GetCell(18).NumericCellValue) : 0;
									model.Value2 = row.GetCell(19) != null ? Convert.ToDecimal(row.GetCell(19).NumericCellValue) : 0;
									model.Value3 = row.GetCell(20) != null ? Convert.ToDecimal(row.GetCell(20).NumericCellValue) : 0;
									model.Value4 = row.GetCell(21) != null ? Convert.ToDecimal(row.GetCell(21).NumericCellValue) : 0;
									model.Value5 = row.GetCell(22) != null ? Convert.ToDecimal(row.GetCell(22).NumericCellValue) : 0;
									model.Value6 = row.GetCell(23) != null ? Convert.ToDecimal(row.GetCell(23).NumericCellValue) : 0;
									model.Value7 = row.GetCell(24) != null ? Convert.ToDecimal(row.GetCell(24).NumericCellValue) : 0;
									model.Value8 = row.GetCell(25) != null ? Convert.ToDecimal(row.GetCell(25).NumericCellValue) : 0;
									model.Value9 = row.GetCell(26) != null ? Convert.ToDecimal(row.GetCell(26).NumericCellValue) : 0;
									model.Value10 = row.GetCell(27) != null ? Convert.ToDecimal(row.GetCell(27).NumericCellValue) : 0;
									model.Value11 = row.GetCell(28) != null ? Convert.ToDecimal(row.GetCell(28).NumericCellValue) : 0;
									model.Value12 = row.GetCell(29) != null ? Convert.ToDecimal(row.GetCell(29).NumericCellValue) : 0;
									model.Value13 = row.GetCell(30) != null ? Convert.ToDecimal(row.GetCell(30).NumericCellValue) : 0;
									model.Value14 = row.GetCell(31) != null ? Convert.ToDecimal(row.GetCell(31).NumericCellValue) : 0;
									model.Value15 = row.GetCell(32) != null ? Convert.ToDecimal(row.GetCell(32).NumericCellValue) : 0;
									model.Value16 = row.GetCell(33) != null ? Convert.ToDecimal(row.GetCell(33).NumericCellValue) : 0;
									model.Value17 = row.GetCell(34) != null ? Convert.ToDecimal(row.GetCell(34).NumericCellValue) : 0;
									model.Value18 = row.GetCell(35) != null ? Convert.ToDecimal(row.GetCell(35).NumericCellValue) : 0;
									model.Value19 = row.GetCell(36) != null ? Convert.ToDecimal(row.GetCell(36).NumericCellValue) : 0;
									model.Value20 = row.GetCell(37) != null ? Convert.ToDecimal(row.GetCell(37).NumericCellValue) : 0;
									model.Value21 = row.GetCell(38) != null ? Convert.ToDecimal(row.GetCell(38).NumericCellValue) : 0;
									model.Value22 = row.GetCell(39) != null ? Convert.ToDecimal(row.GetCell(39).NumericCellValue) : 0;
									//model.Value23 = row.GetCell(Counter + 30) != null ? Convert.ToDecimal(row.GetCell(Counter + 30).NumericCellValue) : 0;
									//model.Value24 = row.GetCell(Counter + 31) != null ? Convert.ToDecimal(row.GetCell(Counter + 31).NumericCellValue) : 0;
									//model.Value25 = row.GetCell(Counter + 32) != null ? Convert.ToDecimal(row.GetCell(Counter + 32).NumericCellValue) : 0;
									//model.Value26 = row.GetCell(Counter + 33) != null ? Convert.ToDecimal(row.GetCell(Counter + 33).NumericCellValue) : 0;
									//model.Value27 = row.GetCell(Counter + 34) != null ? Convert.ToDecimal(row.GetCell(Counter + 34).NumericCellValue) : 0;
									//model.Value28 = row.GetCell(Counter + 35) != null ? Convert.ToDecimal(row.GetCell(Counter + 35).NumericCellValue) : 0;
								}
								else
								{
									break;
								}
								_ruleInstrumentViewModel.Add(model);
							}
                            ListToDataTableConverter converter = new ListToDataTableConverter();
                            DataTable dt = converter.ToDataTable(_ruleInstrumentViewModel);

                            if (tradeRuleViewModel.Id > 0)
                            {
                                ////delete
                                //await _tradeRuleService.DeleteTradeRuleInstrumentPriority(tradeRuleViewModel.Id);
                            }

                            //tradeRuleViewModel.Id = Convert.ToInt32(await _tradeRuleService.TradeRule_InsertByImport(dt));


                            return Json(new { StatusCode = 200, Message = "File Imported Successfully", Response = tradeRuleViewModel });
                        }
                        catch (Exception ex)
                        {
                            return Json(new { StatusCode = 400, Message = ex.Message.ToString() });
                        }
                        finally
                        {
                            try
                            {
                                System.IO.File.Delete(filePath);
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                else
                {
                    return Json(new { StatusCode = false, Message = "Please select File" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { StatusCode = 400, Message = ex.Message.ToString() });
            }
        }

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            var res = _IQuoteMarkUpRuleService.Delete(Id);
            return Json(res);
        }
        [HttpPost]
        public async Task<IActionResult> CopyRule(int Id)
        {
            var res = await _IQuoteMarkUpRuleService.CopyRule(Id);
            return Json(res);
        }
        private void GetInstruments()
        {
            var FieldsList = _BDMService.Get_All_Instrument().Result.Select(p => new SelectListItem()
            {
                Value = p.Id.ToString(),
                Text = p.Name.ToString()
            });
           // ViewBag.InstrumentList = FieldsList;
            ViewBag.InstrumentList = new SelectList(FieldsList, "Value", "Text");

        }
        public async Task<JsonResult> GetInstrumentsDropDown()
        {
            List<SelectListItem> oprationList = new List<SelectListItem>();
            var FieldsList = _BDMService.Get_All_Instrument().Result.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name });
            //var country = _countryService.GetAllCountries().Result.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.CountryName });
            foreach (var item in FieldsList)
            {
                oprationList.Add(new SelectListItem { Text = item.Text, Value = item.Value });
            }
            return Json(oprationList);
        }
        private void GetMarketLP()
        {
            PageParam pageParam = new PageParam();
            pageParam.Offset = 0;
            pageParam.Limit = 100;
            var response = _masterLPService.GetAll(pageParam, "");
            var FieldsList = response.Item1.Select(p => new SelectListItem()
            {
                Value = p.Id.ToString(),
                Text = p.Name
            });
            ViewBag.MarketLPDropDown = FieldsList;
        }
        public async Task<JsonResult> GetMarketLPDropDown()
        {
            List<SelectListItem> FildsName = new List<SelectListItem>();
            PageParam pageParam = new PageParam();
            pageParam.Offset = 0;
            pageParam.Limit = 100;
            var response = _masterLPService.GetAll(pageParam, "");
            var FieldsList = response.Item1.Select(p => new SelectListItem()
            {
                Value = p.Id.ToString(),
                Text = p.Name
            });
            foreach (var item in FieldsList)
            {
                FildsName.Add(new SelectListItem { Text = item.Text, Value = item.Value });
            }
            return Json(FildsName);
        }
        [HttpPost]
        public async Task<IActionResult> UploadQuoteMarkupRule(IFormFile FileUpload, QuoteMarkUpRuleViewModel quoteMarkUpRuleViewModel)
        {
            string filePath = string.Empty;
            try
            {
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
                        if (fileExt.Equals("xls"))
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
                            IRow HeaderRow = sheet.GetRow(1);

                            List<QuoteMarkUpRuleViewModelImport> _ruleInstrumentViewModel = new List<QuoteMarkUpRuleViewModelImport>();

                            int id = 0;
                            int MasterInstrumentId = 0;
                            string Days = "";
                            double MaxSpread = 0;
                            string StartTime = "";
                            string EndTime = "";
                            string MasterInstrumentName = "";
                            string SymbolGroups = "";
                            bool AutoupdateLPPriorityUp = false;

                            int ruleInstrumentIdCounter = 0;

                            if (sheet.PhysicalNumberOfRows > 2)
                            {
                                for (int rowNo = 2; rowNo < sheet.PhysicalNumberOfRows; rowNo++)
                                {
                                    IRow row = sheet.GetRow(rowNo);

                                    MasterInstrumentName = row.GetCell(0) != null ? Convert.ToString(row.GetCell(0)) : "";
                                    SymbolGroups = row.GetCell(1) != null ? row.GetCell(1).ToString() : "";
                                    Days = row.GetCell(2) != null ? row.GetCell(2).ToString() : "";
                                    // StartTime = Convert.ToString(row.GetCell(3).DateCellValue.ToString("HH:mm:ss")) != null ? Convert.ToString(row.GetCell(3).DateCellValue.ToString("HH:mm:ss")) : "";

                                    //row.GetCell(3) != null ? row.GetCell(3).ToString() : ""; ////Time To
                                    StartTime = row.GetCell(3) != null ? row.GetCell(3).ToString() : ""; ////Time To
                                                                                                         // EndTime = Convert.ToString(row.GetCell(4).DateCellValue.ToString("HH:mm:ss")) != null ? Convert.ToString(row.GetCell(4).DateCellValue.ToString("HH:mm:ss")) : "";
                                                                                                         //row.GetCell(4) != null ? row.GetCell(4).ToString() : "";////Time from
                                    EndTime = row.GetCell(4) != null ? row.GetCell(4).ToString() : "";////Time from
                                    MaxSpread = row.GetCell(5) != null ? Convert.ToDouble(row.GetCell(5).NumericCellValue) : 0;

                                    //var intList = Days.Select(digit => int.Parse(digit.ToString()));
                                    int counter = 9, priority = 1;
                                    QuoteMarkUpRuleViewModelImport model;
                                    for (int i = 6; i <= counter; i = i + 4)
                                    {
                                        if (i == row.Cells.Count)
                                        {
                                            break;
                                        }
                                        model = new QuoteMarkUpRuleViewModelImport();
                                        model.LPName = row.GetCell(i) != null ? Convert.ToString(row.GetCell(i)).Trim() : "";
                                        if (model.LPName == "")
                                        {
                                            break;
                                        }
                                        model.RuleName = quoteMarkUpRuleViewModel.Name.Trim();
                                        model.Priority = priority;
                                        model.AutoupdateLPPriorityUp = quoteMarkUpRuleViewModel.AutoupdateLPPriorityUp;
                                        model.MasterInstrumentName = MasterInstrumentName.Trim();
                                        model.SymbolGroups = SymbolGroups.Trim();
                                        model.Day = Days;
                                        model.FromTime = StartTime;
                                        model.ToTime = EndTime;
                                        model.MaxSpread = MaxSpread;
                                        model.RuleInstrumentIdCounter = rowNo;
                                        model.LPName = row.GetCell(i) != null ? Convert.ToString(row.GetCell(i)).Trim() : "";
                                        model.Mbid = row.GetCell(i + 1) != null ? Convert.ToDouble(row.GetCell(i + 1).NumericCellValue) : 0;
                                        model.Mask = row.GetCell(i + 2) != null ? Convert.ToDouble(row.GetCell(i + 2).NumericCellValue) : 0;
                                        model.Timeout = row.GetCell(i + 3) != null ? Convert.ToInt64(row.GetCell(i + 3).NumericCellValue) : 0;
                                        counter = counter + 4;
                                        priority++;
                                        _ruleInstrumentViewModel.Add(model);
                                    }
                                    priority = 1;
                                    //foreach (var item in intList)
                                    //{

                                    //    ruleInstrumentIdCounter++;

                                    //    //GET Priority list
                                    //    int Counter = 5;
                                    //    for (int i = 1; i <= 5; i++)
                                    //    {
                                    //        if (Convert.ToString(row.GetCell(Counter + 1)) != "")
                                    //        {
                                    //            model = new QuoteMarkUpRuleViewModelImport();
                                    //            model.RuleInstrumentIdCounter = ruleInstrumentIdCounter;



                                    //            model.PriorityNo = i;
                                    //            model.LPName = row.GetCell(Counter + 1) != null ? Convert.ToString(row.GetCell(Counter + 1)).Trim() : "";
                                    //            model.Mbid = row.GetCell(Counter + 2) != null ? Convert.ToDouble(row.GetCell(Counter + 2).NumericCellValue) : 0;
                                    //            model.Mask = row.GetCell(Counter + 3) != null ? Convert.ToDouble(row.GetCell(Counter + 3).NumericCellValue) : 0;
                                    //            model.Timeout = row.GetCell(Counter + 4) != null ? Convert.ToInt64(row.GetCell(Counter + 4).NumericCellValue) : 0;
                                    //            Counter = Counter + 4;

                                    //        }
                                    //        else
                                    //        {
                                    //            break;
                                    //        }
                                    //        _ruleInstrumentViewModel.Add(model);
                                    //    }
                                    //    //model.ruleLpPriorities = GetPriorityFromExcel(row);

                                    //}
                                }

                                ListToDataTableConverter converter = new ListToDataTableConverter();
                                DataTable dt = converter.ToDataTable(_ruleInstrumentViewModel);


                                quoteMarkUpRuleViewModel.Id = await _IQuoteMarkUpRuleService.QuoteMarkUpRule_InsertByImport(dt, Convert.ToInt32(quoteMarkUpRuleViewModel.Id));


                                return Json(new { StatusCode = 200, Message = "File Imported Successfully", Response = quoteMarkUpRuleViewModel });
                            }
                            else
                            {
                                return Json(new { StatusCode = false, Message = "Not able found data in file.", Response = quoteMarkUpRuleViewModel });
                            }
                        }
                        catch (Exception ex)
                        {
                            return Json(new { StatusCode = 400, Message = ex.Message.ToString() });
                        }
                        //finally
                        //{
                        //    try
                        //    {
                        //        System.IO.File.Delete(filePath);
                        //    }
                        //    catch
                        //    {
                        //    }
                        //}
                    }
                }
                else
                {
                    return Json(new { StatusCode = false, Message = "Please select File" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { StatusCode = 400, Message = ex.Message.ToString() });
            }
        }
        public List<RuleLpPriority> GetPriorityFromExcel(IRow row)
        {
            List<RuleLpPriority> ruleLpPriority = new List<RuleLpPriority>();
            try
            {
                int Counter = 5;
                for (int i = 1; i <= 5; i++)
                {
                    if (Convert.ToString(row.GetCell(Counter + 1)) != "")
                    {
                        RuleLpPriority Priority = new RuleLpPriority()
                        {
                            PriorityNo = i,
                            LPName = row.GetCell(Counter + 1) != null ? Convert.ToString(row.GetCell(Counter + 1)) : "",
                            Mbid = row.GetCell(Counter + 2) != null ? Convert.ToInt32(row.GetCell(Counter + 2).NumericCellValue) : 0,
                            Mask = row.GetCell(Counter + 3) != null ? Convert.ToInt32(row.GetCell(Counter + 3).NumericCellValue) : 0,
                            Timeout = row.GetCell(Counter + 4) != null ? Convert.ToInt64(row.GetCell(Counter + 4).NumericCellValue) : 0,
                        };
                        Counter = Counter + 4;
                        ruleLpPriority.Add(Priority);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ruleLpPriority;
        }
        [HttpPost]
        public async Task<IActionResult> DeleteRuleConditionById(int Id, int RuleConditionId)
        {
            ResponseViewModel responseViewModel = new ResponseViewModel();

            var res = _ruleConditionService.DeleteConditionRulesDtlById(Id, RuleConditionId);
            if (res != null)
            {
                responseViewModel.Code = 200;
                responseViewModel.Message = "Rule Condition Deleted Successfully";
            }
            else
            {
                responseViewModel.Code = 400;
                responseViewModel.Message = "Rule Condition Not Deleted";
            }
            return Json(responseViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteLpRuleById(int Id, int RuleId)
        {
            ResponseViewModel responseViewModel = new ResponseViewModel();

            var res = _ruleLpPriorityService.Delete(Id);
            if (res != null)
            {
                responseViewModel.Code = 200;
                responseViewModel.Message = "Rule Condition Deleted Successfully";
            }
            else
            {
                responseViewModel.Code = 400;
                responseViewModel.Message = "Rule Condition Not Deleted";
            }
            return Json(responseViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteQuotemarkUpRuleInstrument(int Id, int RuleId, int tLP_ID)
        {
            ResponseViewModel responseViewModel = new ResponseViewModel();

            var res = _ruleConditionService.DeleteQuotemarkUpRuleInstrument(Id, RuleId, tLP_ID);
            if (res != null)
            {
                responseViewModel.Code = 200;
                responseViewModel.Message = "QuoteMarkUp Rule Deleted Successfully";
            }
            else
            {
                responseViewModel.Code = 400;
                responseViewModel.Message = "QuoteMarkUp Rule Not Deleted";
            }
            return Json(responseViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SaveQuoteMarkUpRule(QuoteMarkUpRuleViewModelEdit model)
        {
            try
            {
                //model.objRuleLpPriorityViewModel = JsonConvert.DeserializeObject<List<RuleLpPriorityViewModel_Dt>>(model.objRuleLpPriorityViewModelstr);
                model.objRuleConditionViewModel = JsonConvert.DeserializeObject<List<LeverageRulesDtlViewModel>>(model.objRuleConditionViewModelstr);
                model.objRuleInstrumentViewModel = JsonConvert.DeserializeObject<List<RuleInstrumentLPViewModel>>(model.objRuleInstrumentViewModelstr);

                ListToDataTableConverter converter = new ListToDataTableConverter();

                DataTable dtRuleInstrument = converter.ToDataTable(model.objRuleInstrumentViewModel);
                DataTable dtRuleLpPriority = new DataTable();
                DataTable dtRuleConditionsDtl = converter.ToDataTable(model.objRuleConditionViewModel);

                if (dtRuleConditionsDtl.Rows.Count > 0)
                {
                    RuleConditionViewModel ruleConditionViewModel = new RuleConditionViewModel();
                    ruleConditionViewModel.RuleId = Convert.ToInt32(model.Id);
                    ruleConditionViewModel.Id = Convert.ToInt32(dtRuleConditionsDtl.Rows[0]["RuleConditionsId"]);
                    ruleConditionViewModel.rulesDtlViewModel = model.objRuleConditionViewModel;

                    _ruleConditionService.Upsert(ruleConditionViewModel);
                }
                QuoteMarkUpRuleViewModel quoteMarkUpRuleViewModel = new QuoteMarkUpRuleViewModel
                {
                    Id = model.Id,
                    Name = model.Name,
                    Priority = model.Priority,
                    AutoupdateLPPriorityUp = model.AutoupdateLPPriorityUp,
                };
                _IQuoteMarkUpRuleService.Upsert(quoteMarkUpRuleViewModel);
                dtRuleInstrument.Columns.Remove("InstrumentName");
                _IQuoteMarkUpRuleService.QuoteMarkUpRule_UpdateByDt(dtRuleInstrument, dtRuleLpPriority, dtRuleConditionsDtl);
                return Json(new { StatusCode = 200, Message = "Quote Markup rule updated Successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { StatusCode = 400, Message = ex.Message.ToString() });
            }
        }
        [HttpPost]
        public async Task<JsonResult> Get_InstrumentLpPriorityByRuleId(int Rule_Id, int lp_filter, int instrument_filter, int day_filter)
        {
            long totalRecord = 0;
            long filteredRecord = 0;
            var Draw = Request.Form["draw"].FirstOrDefault();
            PageParam pageParam = new PageParam();

            pageParam.Offset = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");

            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            pageParam.Limit = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            string search = Convert.ToString(Request.Form["search[value]"].FirstOrDefault() ?? "");
            var response = await _ruleInstrumentService.GetByInstrumentLPRuleByRuleId(pageParam, search, Rule_Id, lp_filter, instrument_filter, day_filter);
            totalRecord = response.Item2;
            filteredRecord = response.Item2;

            //var responseData = OrderByExtension.OrderBy(response.Item1.AsQueryable(), sortColumn, sortColumnDirection).ToList();

            return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = response.Item1 });
        }

        public async Task<JsonResult> Get_InstrumentByRuleId(int Rule_Id)
        {
            List<SelectListItem> oprationList = new List<SelectListItem>();
            var response = await _ruleInstrumentService.Get_InstrumentByRuleId(Rule_Id);
            if (response != null)
                oprationList.Add(new SelectListItem { Text = response.Name, Value = Convert.ToString(response.Id) });
            return Json(oprationList);
        }
        private void setCellStyle(HSSFWorkbook workbook, ICell cell, bool isHeader, bool evenOddPrio)
        {
            HSSFCellStyle fCellStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            if (isHeader)
            {
                fCellStyle.FillForegroundColor = HSSFColor.LightOrange.Index;
            }
            else
            {
                fCellStyle.FillForegroundColor = HSSFColor.White.Index;
            }
            fCellStyle.FillPattern = FillPattern.SolidForeground;

            HSSFFont ffont = (HSSFFont)workbook.CreateFont();
            //ffont.FontHeight = 20 * 20;
            ffont.FontHeightInPoints = 11;
            ffont.FontName = "Calibri";
            fCellStyle.SetFont(ffont);

            //fCellStyle.VerticalAlignment = VerticalAlignment.Center;
            //fCellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;

            cell.CellStyle = fCellStyle;
        }
        [HttpPost]
        public async Task<IActionResult> ExportQuoteMarkUpRule(int Id)
        {
            try
            {
                PageParam pageParam = new PageParam();
                pageParam.Offset = 0;
                pageParam.Limit = 100;
                string path = "";

                List<RuleInstrumentLPViewModelExport> modelExport = new List<RuleInstrumentLPViewModelExport>();
                var result  = await _ruleInstrumentService.GetExPortQuoteMarkUpRule_Data(Id);
                modelExport = result;


                //  List<InstrumentMaster> instrumentMasterViewModels = new List<InstrumentMaster>();
                //   var result = await _instrumentService.GetAllInstrument(pageParam, "");


              //  if (ruleInstrumentLPViewModelExport != null)

                    if (result.Count() > 0)
                {
                //   modelExport = result.Item1;

                    ListToDataTableConverter converter = new ListToDataTableConverter();
                   // List<RuleInstrumentLPViewModelExport> modelExport = new List<RuleInstrumentLPViewModelExport>();
                   // modelExport = JsonConvert.DeserializeObject<List<RuleInstrumentLPViewModelExport>>(model.objRuleInstrumentViewModelstr);
                    DataTable dtRuleInstrument = converter.ToDataTable(modelExport);

                    if (dtRuleInstrument.Rows.Count == 0)
                    {
                        return Json(new { StatusCode = 400, Message = "No Records Found." });
                    }

                //    for (int cnt = 0; cnt < dtRuleInstrument.Rows.Count; cnt++)
                //    {
                ////        var symbolGroups = instrumentMasterViewModels.Where(a => a.name.Trim() == Convert.ToString(dtRuleInstrument.Rows[cnt]["InstrumentName"]).Trim()).FirstOrDefault();
                //        dtRuleInstrument.Rows[cnt]["SymbolGroups"] = symbolGroups.GroupName != null ? symbolGroups.GroupName.ToString().Trim() : "";
                //    }
                   var max_priority = modelExport.Select(a => a.Priority).Distinct();
                    string contextType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    //string fileName = "QuoteMarkUpRule_" + DateTime.Now.ToString("MM/dd/yyyy") + ".xlsx";
                    //var workbook = new XLWorkbook();
                    using (var workbook = new XLWorkbook())
                    {

                        var sheet = workbook.Worksheets.Add("Sheet1");

                        //HSSFFont hFont = (HSSFFont)workbook.CreateFont();

                        //hFont.FontHeightInPoints = 11;
                        //hFont.FontName = "Calibri";
                        ////hFont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;

                        //HSSFCellStyle hStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                        //hStyle.SetFont(hFont);

                        // Convert datatable into json  
                        string JSON = JsonConvert.SerializeObject(dtRuleInstrument);

                        // Convert json into SummaryClass class list  
                        var items = JsonConvert.DeserializeObject<List<RuleInstrumentLPViewModelExport>>(JSON);

                        // Set column name this column name use for fetch data from list  
                        var columns = new[] { "InstrumentName", "SymbolGroups", "Day", "StartTime", "EndTime", "MaxSpread" };
                        var columns_priority = new[] { "LPName", "Mbid", "Mask", "Timeout" };

                        //// Set header name this header use for set name in excel first row
                        var header = new[] { "Instrument Name", "Symbol Groups", "Day", "Time to", "Time From", "Max Spread" };
                        var header_priority = new[] { "LPName", "Mbid", "Mask", "Timeout" };

                        List<string> Headerlist = new List<string>();
                        List<string> columnslist = new List<string>();
                        foreach (var item in header)
                        {
                            Headerlist.Add(item);
                        }
                        foreach (var item in columns)
                        {
                            columnslist.Add(item);
                        }

                        for (int h_cnt = 0; h_cnt < max_priority.Count(); h_cnt++)
                        {

                            foreach (var item in header_priority)
                            {
                                
                                Headerlist.Add(item);
                            }
                            foreach (var item in columns_priority)
                            {
                                columnslist.Add(item);
                            }
                        }

                        //sheet.Rows() .Row(0);
                        var headerRow = sheet.Row(2);


                        sheet.Range("A1:C1").Merge().SetValue("Days");
                        sheet.Range("D1:F1").Merge().SetValue("Trade Time ");



                        for (int i = 1; i <= 10; i++)
                        {
                           

                            var priority_check = $"Priority{i}";
                            int startColumn = 7 + (i - 1) * 4;
                            int endColumn = startColumn + 3;

                            // sheet.Range($"G1:K7").Merge();
                            // sheet.Range($"G1:K7").SetValue(priority_check);
                            foreach (var item in header_priority)
                            {

                                Headerlist.Add(item);
                            }
                            sheet.Range(1, startColumn, 1, endColumn).Merge();
                            sheet.Range(1, startColumn, 1, endColumn).SetValue(priority_check);
                        }


                        //for (int i = 0; i <= 10; i++)
                        //{
                        //    for (int k = 7; k < 50; k = k + 3)
                        //    {
                        //        var priority_check = $"Priority+{i}";
                        //        sheet.Range("G1:K7").Merge().SetValue(priority_check);
                        //    }
                        //}
                        //sheet.Range("G1:J1").Merge().SetValue("Priority1");
                        //sheet.Range("k1:N1").Merge().SetValue("Priority2");
                        //sheet.Range("O1:R1").Merge().SetValue("Priority3");
                        //sheet.Range("S1:V1").Merge().SetValue("Priority4");
                        //sheet.Range("W1:Z1").Merge().SetValue("Priority5");
                        //sheet.Range("AA1:AD1").Merge().SetValue("Priority6");
                        //sheet.Range("AE1:I1").Merge().SetValue("Priority7");








                        //hcell.SetValue("Days");

                        int rowIndex = 2;
                        //Below loop is create header  
                        for (int i = 0; i < Headerlist.Count(); i++)
                        {
                            var cell = headerRow.Cell(i + 1);
                            cell.SetValue(Headerlist[i]);
                            //sheet1.CreateRow(0).CreateCell(0).CellStyle = style1;
                            //setCellStyle(workbook, cell);
                            sheet.Cell(rowIndex, i + 1).Value = Headerlist[i];
                            sheet.Row(rowIndex).Style.Fill.SetBackgroundColor(XLColor.Yellow);
                        }

                        //Below loop is fill content
                        int ColCnt = 1;

                        for (int i = 0; i < items.Count; i++)
                        {
                            if (i > 0)
                            {
                                if (items[i - 1].InstrumentName != items[i].InstrumentName || items[i - 1].SymbolGroups != items[i].SymbolGroups ||
                                    items[i - 1].Day != items[i].Day ||
                                    items[i - 1].StartTime != items[i].StartTime ||
                                    items[i - 1].EndTime != items[i].EndTime ||
                                    items[i - 1].MaxSpread != items[i].MaxSpread)
                                {
                                    ColCnt = 1;
                                    //prev_value = items[i].InstrumentName;
                                    rowIndex++;
                                    //var row = sheet.Row(rowIndex);

                                    for (int j = 0; j < columns.Length; j++)
                                    {
                                        //var cell = row.Cell(ColCnt);
                                        var o = items[i];
                                        if (columns[j] == "MaxSpread")
                                        {
                                            sheet.Cell(rowIndex, ColCnt).Value = Convert.ToString(o.GetType().GetProperty(columns[j]).GetValue(o, null)).ToString();
                                            //cell.SetValue(Convert.ToDouble(o.GetType().GetProperty(columns[j]).GetValue(o, null)));
                                        }
                                        else if (columns[j] == "StartTime" || columns[j] == "EndTime")
                                        {
                                            sheet.Cell(rowIndex, ColCnt).Value = Convert.ToDateTime(o.GetType().GetProperty(columns[j]).GetValue(o, null).ToString());
                                        }
                                        else
                                        {
                                            sheet.Cell(rowIndex, ColCnt).Value = Convert.ToString(o.GetType().GetProperty(columns[j]).GetValue(o, null)).ToString();
                                            //cell.SetValue(o.GetType().GetProperty(columns[j]).GetValue(o, null).ToString());
                                        }

                                        //setCellStyle(workbook, cell, false, true);
                                        ColCnt++;
                                    }
                                    for (int col = 0; col < columns_priority.Length; col++)
                                    {
                                        //var cell = row.Cell(ColCnt);
                                        var o = items[i];

                                        if (columns_priority[col] == "LPName" || columns_priority[col] == "Mbid" || columns_priority[col] == "Mask" || columns_priority[col] == "Timeout")
                                        {
                                            

                                                sheet.Cell(rowIndex, ColCnt).Value = Convert.ToString(o.GetType().GetProperty(columns_priority[col]).GetValue(o, null)).ToString();
                                                //cell.SetValue(Convert.ToDouble(o.GetType().GetProperty(columns_priority[col]).GetValue(o, null)));
                                           
                                            }
                                        else
                                        {
                                            sheet.Cell(rowIndex, ColCnt).Value = Convert.ToString(o.GetType().GetProperty(columns_priority[col]).GetValue(o, null)).ToString();
                                            //cell.SetValue(o.GetType().GetProperty(columns_priority[col]).GetValue(o, null).ToString());
                                        }
                                        //setCellStyle(workbook, cell, false, true);
                                        ColCnt++;
                                    }
                                }
                                else
                                {
                                    //var row = sheet.Row(rowIndex);
                                    for (int col = 0; col < columns_priority.Length; col++)
                                    {
                                        //var cell = row.Cell(ColCnt);
                                        var o = items[i];
                                        
                                        if (columns_priority[col] == "LPName" || columns_priority[col] == "Mbid" || columns_priority[col] == "Mask" || columns_priority[col] == "Timeout")
                                        {
                                           

                                                sheet.Cell(rowIndex, ColCnt).Value = Convert.ToString(o.GetType().GetProperty(columns_priority[col]).GetValue(o, null)).ToString();
                                                //cell.SetValue(Convert.ToDouble(o.GetType().GetProperty(columns_priority[col]).GetValue(o, null)));
                                            
                                            }
                                        else
                                        {
                                            sheet.Cell(rowIndex, ColCnt).Value = Convert.ToString( o.GetType().GetProperty(columns_priority[col]).GetValue(o, null)).ToString();
                                            //cell.SetValue(o.GetType().GetProperty(columns_priority[col]).GetValue(o, null).ToString());
                                        }
                                        //setCellStyle(workbook, cell, false, true);
                                        ColCnt++;
                                    }
                                }
                            }
                            else
                            {
                                rowIndex++;
                                //var row = sheet.Row(rowIndex);

                                for (int j = 0; j < columns.Count(); j++)
                                {
                                    //var cell = row.Cell(ColCnt);
                                    var o = items[i];
                                    if (columns[j] == "MaxSpread")
                                    {
                                        if (o.MaxSpread != null)
                                        {
                                            sheet.Cell(rowIndex, ColCnt).Value = Convert.ToDouble(o.GetType().GetProperty(columns[j]).GetValue(o, null));
                                        }
                                    }
                                    else
                                    {
                                        if (columns[j] == "InstrumentName")
                                        {
                                            if (o.InstrumentName != null)
                                            {
                                                sheet.Cell(rowIndex, ColCnt).Value = o.GetType().GetProperty(columns[j]).GetValue(o, null).ToString();
                                            }
                                            //cell.SetValue(o.GetType().GetProperty(columns[j]).GetValue(o, null).ToString());
                                        }
                                        else if (columns[j] == "SymbolGroups")
                                        {
                                            if (o.SymbolGroups != null)
                                            {
                                                sheet.Cell(rowIndex, ColCnt).Value = o.GetType().GetProperty(columns[j]).GetValue(o, null).ToString();
                                            }
                                        }
                                        else if (columns[j] == "Day")
                                        {
                                            if (o.Day != null)
                                            {
                                                sheet.Cell(rowIndex, ColCnt).Value = o.GetType().GetProperty(columns[j]).GetValue(o, null).ToString();
                                            }
                                        }
                                        else if (columns[j] == "StartTime")
                                        {
                                            if (o.StartTime != null)
                                            {
                                                sheet.Cell(rowIndex, ColCnt).Value = Convert.ToDateTime(o.GetType().GetProperty(columns[j]).GetValue(o, null).ToString());
                                            }
                                        }
                                        else if (columns[j] == "EndTime")
                                        {
                                            if (o.EndTime != null)
                                            {
                                                sheet.Cell(rowIndex, ColCnt).Value = Convert.ToDateTime(o.GetType().GetProperty(columns[j]).GetValue(o, null).ToString());
                                            }
                                        }
                                    }

                                    //setCellStyle(workbook, cell, false, true);
                                    ColCnt++;
                                }
                                for (int j = 0; j < columns_priority.Count(); j++)
                                {
                                    //var cell = row.Cell(ColCnt);
                                    var o = items[i];
                                    if (columns_priority[j] == "LPName" || columns_priority[j] == "Mbid" || columns_priority[j] == "Mask" || columns_priority[j] == "Timeout")
                                    {
                                       

                                            sheet.Cell(rowIndex, ColCnt).Value = Convert.ToString(o.GetType().GetProperty(columns_priority[j]).GetValue(o, null)).ToString();
                                            //cell.SetValue(Convert.ToDouble(o.GetType().GetProperty(columns_priority[j]).GetValue(o, null)));
                                       
                                        }
                                    else
                                    {

                                        sheet.Cell(rowIndex, ColCnt).Value = Convert.ToString (o.GetType().GetProperty(columns_priority[j]).GetValue(o, null)).ToString();
                                        //cell.SetValue(o.GetType().GetProperty(columns_priority[j]).GetValue(o, null).ToString());
                                    }
                                    //setCellStyle(workbook, cell, false, true);
                                    ColCnt++;
                                }
                            }
                        }

                        // Declare one MemoryStream variable for write file in stream  
                        var stream = new MemoryStream();
                        workbook.SaveAs(stream);

                        //sheet.Columns().AdjustToContents();
                        //using (var stream = new MemoryStream())
                        //{
                        //    workbook.SaveAs(stream);
                        //    var content = stream.ToArray();
                        //    return File(content, contextType, fileName);
                        //}
                        //string FilePath = "\\";
                        string filename = "QuoteMarkUpRule_" + DateTime.Now.ToString("MMddyyyy HH:mm");
                        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "export");
                        var filePath = Path.Combine(folderPath, filename.ToString() + ".xlsx");
                        path = filename.ToString();
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }
                        //using (var stream = new MemoryStream())
                        //{
                        //    workbook.SaveAs(stream);
                        //    var content = stream.ToArray();
                        //    return File(content, contextType, fileName);
                        //}
                        //Write to file using file stream  
                        FileStream file = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write);
                        stream.WriteTo(file);
                        file.Close();
                        stream.Close();
                    }


                    return Json(new { StatusCode = 200, FileName = path + ".xlsx", Message = "Export Quote Markup rule export Successfully" });
                }
                else
                {
                    return Json(new { StatusCode = 400, Message = "No Records Found." });

                }

              
                
            }
            catch (Exception ex)
            {
                return Json(new { StatusCode = 400, Message = ex.Message.ToString() });
            }
        }
        public FileResult Download(string Filename)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "export");
            var filePath = Path.Combine(folderPath, Filename);
            FileInfo fileInfo = new FileInfo(filePath);
            var bytes = System.IO.File.ReadAllBytes(fileInfo.FullName);
            return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, Filename);

        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuoteMarkUpRule(QuoteMarkUpRuleViewModel model)
        {
            try
            {
                _IQuoteMarkUpRuleService.Upsert(model);
                return Json(new { StatusCode = 200, Message = "Quote Markup rule updated Successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { StatusCode = 400, Message = ex.Message.ToString() });
            }
        }
    }
}
