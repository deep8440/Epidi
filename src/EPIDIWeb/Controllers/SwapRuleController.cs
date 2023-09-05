using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using Epidi.Models.Model;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.Instrument;
using Epidi.Models.ViewModel.LeverageRules;
using Epidi.Models.ViewModel.QuoteMarkUpRule;
using Epidi.Models.ViewModel.RuleCondition;
using Epidi.Models.ViewModel.RuleInstrument;
using Epidi.Models.ViewModel.SwapRule;
using Epidi.Service.BDM;
using Epidi.Service.InstrumentService;
using Epidi.Service.RuleConditionService;
using Epidi.Service.SwapRuleService;
using EPIDIWeb.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using System.Text;

namespace EPIDIWeb.Controllers
{
    public class SwapRuleController : Controller
    {
        private readonly ISwapRuleService _swapRuleService;
        private readonly IInstrumentService _instrumentService;
        private readonly IBDMService _BDMService;
        private readonly IRuleConditionService _ruleConditionService;
        private readonly IMemoryCache _memoryCache;

        public SwapRuleController(ISwapRuleService swapRuleService, IInstrumentService instrumentService
            , IBDMService BDMService, IRuleConditionService ruleConditionService, IMemoryCache memoryCache)
        {
            _swapRuleService = swapRuleService;
            _instrumentService = instrumentService;
            _BDMService = BDMService;
            _ruleConditionService = ruleConditionService;
            _memoryCache = memoryCache;
        }
        public IActionResult Index()
        {
            ViewBag.Title = "SwapRule List";
            return View();
        }
        public IActionResult AddSwapRule()
        {
            return View(new SwapRuleViewModel());
        }

        [HttpPost]
        public async Task<JsonResult> Get_All()
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
            var response = await _swapRuleService.GetAllSwapRules(pageParam, search);

            totalRecord = response.Item2;
            filteredRecord = response.Item2;

            var responseData = OrderByExtension.OrderBy(response.Item1.AsQueryable(), sortColumn, sortColumnDirection).ToList();

            return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = responseData });
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            ViewBag.PageTitle = "Edit Swap Rule";
            ViewBag.Method = "SwapRule";

            if (!_memoryCache.TryGetValue("InstrumentsCache", out var instruments))
            {
                instruments = await _instrumentService.GetInstrumentListAll();
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30),
                };
                _memoryCache.Set("InstrumentsCache", instruments, cacheEntryOptions);
            }
            // var instruments = await _instrumentService.GetAllInstrument(pageParam, "");
            var symbolgroups = await _instrumentService.GetAllSymbolGroup();
            ViewBag.InstrumentsDes = instruments;
            ViewBag.Instruments = JsonConvert.SerializeObject(instruments);

            ViewBag.SymbolGroups = JsonConvert.SerializeObject(symbolgroups);

            var response = await _swapRuleService.GetSwapRuleById(Id);

            var ruleConditions = await _ruleConditionService.GetRuleConditionBySwapRuleId(Id);
            response.objRuleConditionViewModel = ruleConditions;
            if (ruleConditions != null)
            {
                response.objRuleConditionViewModel.conditionRulesDtlObj = JsonConvert.SerializeObject(ruleConditions.rulesDtlViewModel);
            }
            //response.mode = "Edit";

            if (response != null)
            {
                return View("EditSwapRule", response);
            }
            else
            {
                return View(new SwapRuleViewModel());
            }
        }

        [HttpPost]
        public async Task<JsonResult> Get_SwapRuleInstrumentBySwapRuleId(int Rule_Id)
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


            var response = await _swapRuleService.GetSwapRuleInstrumentById(pageParam, search, Rule_Id);

            totalRecord = response.Item2;
            filteredRecord = response.Item2;

            return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = response.Item1 });

            //   return Json(response);
        }

        [HttpPost]
        public async Task<IActionResult> SaveSwapRule(IFormFile FileUpload, SwapRuleViewModel model)
        {
            string filePath = string.Empty;
            try
            {
                if (FileUpload != null)
                {
                    if (Request.Form["Id"].FirstOrDefault() != "")
                    {
                        model.Id = Convert.ToInt32(Request.Form["Id"].FirstOrDefault());
                    }
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

                            List<ImportSwapRuleInstrumentViewModel> importSwapRuleInstrumentViewModels = new List<ImportSwapRuleInstrumentViewModel>();

                            int Id;
                            string InstrumentName;
                            string SymbolGroup;
                            decimal BuySwapPer1000Notional;
                            decimal SellSwapPer1000Notional;
                            decimal BuySwapNotional;
                            decimal SellSwapNotional;
                            int TrippleSwapsDay;
                            string DaystoApply;
                            string RoundingBuy;
                            string RoundingSell;
                            decimal DecimalCut;
                            long Priority;

                            for (int rowNo = 1; rowNo < sheet.PhysicalNumberOfRows; rowNo++)
                            {
                                IRow row = sheet.GetRow(rowNo);

                                if (row != null)
                                {
                                    Id = row.GetCell(0) != null ? Convert.ToInt32(row.GetCell(0).NumericCellValue) : 0;
                                    InstrumentName = row.GetCell(1) != null ? row.GetCell(1).ToString() : "";
                                    SymbolGroup = row.GetCell(2) != null ? row.GetCell(2).ToString() : "";
                                    BuySwapPer1000Notional = row.GetCell(3) != null ? Convert.ToDecimal(row.GetCell(3).NumericCellValue) : 0;
                                    SellSwapPer1000Notional = row.GetCell(4) != null ? Convert.ToDecimal(row.GetCell(4).NumericCellValue) : 0;
                                    BuySwapNotional = row.GetCell(5) != null ? Convert.ToDecimal(row.GetCell(5).NumericCellValue) : 0;
                                    SellSwapNotional = row.GetCell(6) != null ? Convert.ToDecimal(row.GetCell(6).NumericCellValue) : 0;
                                    TrippleSwapsDay = row.GetCell(7) != null ? Convert.ToInt32(row.GetCell(7).NumericCellValue) : 0;
                                    DaystoApply = row.GetCell(8) != null ? row.GetCell(8).ToString() : "";
                                    RoundingBuy = row.GetCell(9) != null ? row.GetCell(9).ToString() : "";
                                    RoundingSell = row.GetCell(10) != null ? row.GetCell(10).ToString() : "";
                                    DecimalCut = row.GetCell(11) != null ? Convert.ToDecimal(row.GetCell(11).NumericCellValue) : 0;
                                    Priority = row.GetCell(12) != null ? Convert.ToInt64(row.GetCell(12).NumericCellValue) : 0;


                                    ImportSwapRuleInstrumentViewModel importSwapRuleInstrument = new ImportSwapRuleInstrumentViewModel();

                                    importSwapRuleInstrument.Id = Id;
                                    importSwapRuleInstrument.InstrumentName = InstrumentName;
                                    importSwapRuleInstrument.SymbolGroup = SymbolGroup;
                                    importSwapRuleInstrument.BuySwapPer1000Notional = BuySwapPer1000Notional;
                                    importSwapRuleInstrument.SellSwapPer1000Notional = SellSwapPer1000Notional;
                                    importSwapRuleInstrument.BuySwapNotional = BuySwapNotional;
                                    importSwapRuleInstrument.SellSwapNotional = SellSwapNotional;
                                    importSwapRuleInstrument.TrippleSwapsDay = TrippleSwapsDay;
                                    importSwapRuleInstrument.DaystoApply = DaystoApply;
                                    importSwapRuleInstrument.RoundingBuy = RoundingBuy;
                                    importSwapRuleInstrument.RoundingSell = RoundingSell;
                                    importSwapRuleInstrument.DecimalCut = DecimalCut;
                                    importSwapRuleInstrument.Priority = Priority;


                                    importSwapRuleInstrumentViewModels.Add(importSwapRuleInstrument);
                                }
                            }

                            ListToDataTableConverter converter = new ListToDataTableConverter();
                            System.Data.DataTable dt = converter.ToDataTable(importSwapRuleInstrumentViewModels);
                            var ruleid = await _swapRuleService.AddSwapRule(dt, model);


                            return Json(new { StatusCode = 200, Message = "File Imported Successfully", Data = ruleid });
                        }
                        catch (Exception ex)
                        {
                            return Json(new { StatusCode = 400, Message = ex.Message.ToString() });
                        }
                        finally
                        {
                            try
                            {
                                //System.IO.File.Delete(filePath);
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


        public async Task<IActionResult> ExportSwapInstrument(int Id)
        {
            PageParam pageParam = new PageParam();
            pageParam.Offset = 0;
            pageParam.Limit = 100;
            List<SwapRuleInstrumentViewModel> swapRuleInstrumentViewModels = new List<SwapRuleInstrumentViewModel>();
            var result = await _swapRuleService.GetSwapRuleInstrumentById(pageParam, "", Id);
            swapRuleInstrumentViewModels = result.Item1;

            if (swapRuleInstrumentViewModels != null)
            {
                //return Json(new { draw = requestModel.Draw, recordsFiltered = responseModel.FilteredCount, recordsTotal = responseModel.TotalCount, data = datavalue });
                StringBuilder stringBuilder = new StringBuilder();

                string contextType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                string fileName = "SwapRule_" + DateTime.Now.ToString("MM/dd/yyyy") + ".xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("SwapRuleInstrument");
                    ////Merge and Center
                    //worksheet.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    //worksheet.Cell("A1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    //worksheet.Range("A1:N1").Merge().Value = "POI Report";
                    //worksheet.Range("A1:N2").Style.Font.Bold = true;

                    worksheet.Cell(1, 1).Value = "Id";
                    worksheet.Cell(1, 2).Value = "Instrument Name";
                    worksheet.Cell(1, 3).Value = "Symbol Group";
                    worksheet.Cell(1, 4).Value = "Buy Swap Per $1000 Notional ($)";
                    worksheet.Cell(1, 5).Value = "Sell Swap Per Notional $1000 ($)";
                    worksheet.Cell(1, 6).Value = "Buy Swap (%) Notional";
                    worksheet.Cell(1, 7).Value = "Sell Swap (%) Notional";
                    worksheet.Cell(1, 8).Value = "Tripple Swaps Day";
                    worksheet.Cell(1, 9).Value = "Days to Apply";
                    worksheet.Cell(1, 10).Value = "Rounding Buy";
                    worksheet.Cell(1, 11).Value = "Rounding Sell";
                    worksheet.Cell(1, 12).Value = "Decimal Cut";
                    worksheet.Cell(1, 13).Value = "Priority";

                    worksheet.Range("A1:N1").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                    //worksheet.Range("A1:N2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    for (int index = 1; index <= swapRuleInstrumentViewModels.Count; index++)
                    {
                        worksheet.Cell(index + 1, 1).Style.NumberFormat.SetFormat("0");
                        worksheet.Cell(index + 1, 1).Value = index;
                        worksheet.Cell(index + 1, 2).Value = swapRuleInstrumentViewModels[index - 1].InstrumentName;
                        worksheet.Cell(index + 1, 3).Value = swapRuleInstrumentViewModels[index - 1].SymbolGroup;
                        worksheet.Cell(index + 1, 4).Value = swapRuleInstrumentViewModels[index - 1].BuySwapPer1000Notional;
                        worksheet.Cell(index + 1, 5).Value = swapRuleInstrumentViewModels[index - 1].SellSwapPer1000Notional;
                        worksheet.Cell(index + 1, 6).Value = swapRuleInstrumentViewModels[index - 1].BuySwapNotional;
                        worksheet.Cell(index + 1, 7).Value = swapRuleInstrumentViewModels[index - 1].SellSwapNotional;
                        worksheet.Cell(index + 1, 8).Value = swapRuleInstrumentViewModels[index - 1].TrippleSwapsDay;
                        worksheet.Cell(index + 1, 9).Value = swapRuleInstrumentViewModels[index - 1].DaystoApply;
                        worksheet.Cell(index + 1, 10).Value = swapRuleInstrumentViewModels[index - 1].RoundingBuy;
                        worksheet.Cell(index + 1, 11).Value = swapRuleInstrumentViewModels[index - 1].RoundingSell;
                        worksheet.Cell(index + 1, 12).Value = swapRuleInstrumentViewModels[index - 1].DecimalCut;
                        worksheet.Cell(index + 1, 13).Value = swapRuleInstrumentViewModels[index - 1].Priority;
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

        [HttpPost]
        public async Task<IActionResult> AddSwapRule(EditSwapRuleViewModel model)
        {
            try
            {
                //model.objRuleLpPriorityViewModel = JsonConvert.DeserializeObject<List<RuleLpPriorityViewModel_Dt>>(model.objRuleLpPriorityViewModelstr);
                model.objRuleConditionViewModel = JsonConvert.DeserializeObject<List<LeverageRulesDtlViewModel>>(model.objRuleConditionViewModelstr);
                model.objRuleInstrumentViewModel = JsonConvert.DeserializeObject<List<SwapRuleInstrumentViewModel>>(model.objRuleInstrumentViewModelstr);

                ListToDataTableConverter converter = new ListToDataTableConverter();


                DataTable dtRuleInstrument = converter.ToDataTable(model.objRuleInstrumentViewModel.Where(x => x.Id != 0).ToList());

                DataTable dtRuleInstrumentAdd = converter.ToDataTable(model.objRuleInstrumentViewModel.Where(x => x.Id == 0).ToList());
                DataTable dtRuleLpPriority = new DataTable();
                DataTable dtRuleConditionsDtl = converter.ToDataTable(model.objRuleConditionViewModel);


                //foreach (var item in model.objRuleInstrumentViewModel)
                //{
                //    item.Id = model.Id;
                //    var response = _swapRuleService.UpdateSwapRuleNew(item);
                //    item.Id = response.Id;
                //  //  var bandresponse = _leverageRulesService.UpdateBandMapping(item);
                //}



                if (dtRuleConditionsDtl.Rows.Count > 0)
                {
                    RuleConditionViewModel ruleConditionViewModel = new RuleConditionViewModel();
                    ruleConditionViewModel.SwapRuleId = Convert.ToInt32(model.Id);
                    ruleConditionViewModel.Id = Convert.ToInt32(dtRuleConditionsDtl.Rows[0]["RuleConditionsId"]);
                    ruleConditionViewModel.rulesDtlViewModel = model.objRuleConditionViewModel;

                    _ruleConditionService.Upsert(ruleConditionViewModel);
                }
                SwapRuleViewModel swapRuleViewModel = new SwapRuleViewModel
                {
                    Id = model.Id,
                    RuleName = model.RuleName,
                    Comment = model.Comment,
                    Priority = model.Priority,
                    TimeToApply = model.TimeToApply,
                };


                //foreach (var item in model.objRuleInstrumentViewModel)
                //{

                //}
                dtRuleInstrument.Columns.Remove("InstrumentName");
                dtRuleInstrument.Columns.Remove("SymbolGroup");

                foreach (DataRow dr in dtRuleInstrument.Rows) // search whole table
                {
                    dr["SymbolGroupId"] = "0";

                }

                dtRuleInstrumentAdd.Columns.Remove("InstrumentName");
                dtRuleInstrumentAdd.Columns.Remove("SymbolGroup");

                _swapRuleService.UpdateSwapRuleInstrument(dtRuleInstrument, swapRuleViewModel);

                _swapRuleService.AddSwapRuleNew(dtRuleInstrumentAdd, swapRuleViewModel);
                return Json(new { StatusCode = 200, Message = "Swap rule updated successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { StatusCode = 400, Message = ex.Message.ToString() });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSwapRuleInstrument(int Id, int SwapRuleId)
        {
            ResponseViewModel responseViewModel = new ResponseViewModel();

            var res = _swapRuleService.DeleteSwapRuleInstrumentById(Id, SwapRuleId);
            if (res != null)
            {
                responseViewModel.Code = 200;
                responseViewModel.Message = "Swap Rule Deleted Successfully";
            }
            else
            {
                responseViewModel.Code = 400;
                responseViewModel.Message = "Swap Rule Not Deleted";
            }
            return Json(responseViewModel);
        }


        public async Task<IActionResult> DeleteSwapRule(int Id)
        {
            List<ResponseViewModel> responseViewModel = new List<ResponseViewModel>();
            ResponseViewModel responseViewModel1 = new ResponseViewModel();
            var res = await _swapRuleService.DeleteSwapRuleById(Id);
            if (res != null)
            {
                responseViewModel1.Code = 200;
                responseViewModel1.Message = "Swap Rule Deleted Successfully";
            }
            else
            {
                responseViewModel1.Code = 400;
                responseViewModel1.Message = "Swap Rule Not Deleted";
            }
            responseViewModel.Add(responseViewModel1);
            return Json(responseViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteSwapRuleConditionDtlById(int RuleConditionId, int RuleConditionDtlId)
        {
            ResponseViewModel responseViewModel = new ResponseViewModel();

            var res = _ruleConditionService.DeleteSwapRuleConditionDtlById(RuleConditionId, RuleConditionDtlId);
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
        public async Task<JsonResult> GetAllSymbolGroupDropDown()
        {
            List<SelectListItem> oprationList = new List<SelectListItem>();
            var FieldsList = _instrumentService.GetAllSymbolGroup().Result.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Group });
            foreach (var item in FieldsList)
            {
                oprationList.Add(new SelectListItem { Text = item.Text, Value = item.Value });
            }
            return Json(oprationList);
        }

        [HttpPost]
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
        [HttpPost]
        public async Task<IActionResult> CopyRule(int Id)
        {
            var res = await _swapRuleService.CopyRule(Id);
            return Json(res);
        }

    }
}
