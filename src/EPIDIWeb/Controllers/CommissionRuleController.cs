using ClosedXML.Excel;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.CommissionRule;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.LeverageRules;
using Epidi.Models.ViewModel.RuleCondition;
using Epidi.Models.ViewModel.SwapRule;
using Epidi.Service.BDM;
using Epidi.Service.CommissionRuleService;
using Epidi.Service.InstrumentService;
using Epidi.Service.RuleConditionService;
using EPIDIWeb.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using System.Text;

namespace EPIDIWeb.Controllers
{
    public class CommissionRuleController : Controller
    {
        private readonly ICommissionRuleService _commissionRuleService;
        private readonly IRuleConditionService _ruleConditionService;
        private readonly IInstrumentService _instrumentService;
        private readonly IBDMService _BDMService;
        public CommissionRuleController(ICommissionRuleService commissionRuleService,
            IRuleConditionService ruleConditionService, IInstrumentService instrumentService,
            IBDMService BDMService)
        {
            _commissionRuleService = commissionRuleService;
            _ruleConditionService = ruleConditionService;
            _instrumentService = instrumentService;
            _BDMService = BDMService;
        }
        public IActionResult Index()
        {
            ViewBag.Title = "CommissionRule List";
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Get_All()
        {
            try
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
                var response = await _commissionRuleService.GetAllCommissionRules(pageParam, search);

                totalRecord = response.Item2;
                filteredRecord = response.Item2;

                var responseData = OrderByExtension.OrderBy(response.Item1.AsQueryable(), sortColumn, sortColumnDirection).ToList();

                return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = responseData });

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public IActionResult AddCommissionRule()
        {
            GetUnitsType();
            GetWhenToApplyCombo();
            GetWhereToApplyId();
            return View(new CommissionRuleViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            ViewBag.PageTitle = "Edit Commission Rule";
            ViewBag.Method = "CommissionRule";

            GetUnitsType();
            GetWhenToApplyCombo();
            GetWhereToApplyId();
            var response = await _commissionRuleService.GetCommissionRuleById(Id);

            var ruleConditions = await _ruleConditionService.GetRuleConditionByCommissionRuleId(Id);
            response.objRuleConditionViewModel = ruleConditions;
            if (ruleConditions != null)
            {
                response.objRuleConditionViewModel.conditionRulesDtlObj = JsonConvert.SerializeObject(ruleConditions.rulesDtlViewModel);
            }
            //response.mode = "Edit";

            if (response != null)
            {
                return View("EditCommissionRule", response);
            }
            else
            {
                return View(new CommissionRuleViewModel());
            }
        }

        [HttpPost]
        public async Task<JsonResult> Get_CommissionRuleInstrumentByCommissionRuleId(int Rule_Id)
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
			var response = await _commissionRuleService.GetCommissionRuleInstrumentById(pageParam, search, Rule_Id);

			totalRecord = response.Item2;
			filteredRecord = response.Item2;

			//var responseData = OrderByExtension.OrderBy(response.Item1.AsQueryable(), sortColumn, sortColumnDirection).ToList();

			return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = response.Item1 });

        }
        [HttpPost]
        public async Task<IActionResult> AddCommissionRule(EditCommissionRuleViewModel model)
        {
            try
            {
                //model.objRuleLpPriorityViewModel = JsonConvert.DeserializeObject<List<RuleLpPriorityViewModel_Dt>>(model.objRuleLpPriorityViewModelstr);
                model.objRuleConditionViewModel = JsonConvert.DeserializeObject<List<LeverageRulesDtlViewModel>>(model.objRuleConditionViewModelstr);
                model.objRuleInstrumentViewModel = JsonConvert.DeserializeObject<List<ImportCommissionRuleInstrumentViewModel>>(model.objRuleInstrumentViewModelstr);

                ListToDataTableConverter converter = new ListToDataTableConverter();

                DataTable dtRuleInstrument = converter.ToDataTable(model.objRuleInstrumentViewModel);
                DataTable dtRuleLpPriority = new DataTable();
                DataTable dtRuleConditionsDtl = converter.ToDataTable(model.objRuleConditionViewModel);

                if (dtRuleConditionsDtl.Rows.Count > 0)
                {
                    RuleConditionViewModel ruleConditionViewModel = new RuleConditionViewModel();
                    ruleConditionViewModel.CommissionRuleId = Convert.ToInt32(model.Id);
                    ruleConditionViewModel.Id = Convert.ToInt32(dtRuleConditionsDtl.Rows[0]["RuleConditionsId"]);
                    ruleConditionViewModel.rulesDtlViewModel = model.objRuleConditionViewModel;

                    _ruleConditionService.Upsert(ruleConditionViewModel);
                }
                CommissionRuleViewModel commissionRuleViewModel = new CommissionRuleViewModel
                {
                    Id = model.Id,
                    Comment = model.Comment,
                    Priority = model.Priority,
                    TimeToApply = model.TimeToApply,
                    OrderComment = model.OrderComment,
                    Leverage = model.Leverage,
                    WhenToApplyComboId = model.WhenToApplyComboId,
                    UnitsTypeId = model.UnitsTypeId,
                    WhereToApplyId = model.WhereToApplyId,
                    PercentageValue = model.PercentageValue,
                    IsMobileView = model.IsMobileView,
                    IsBrokerCommission=model.IsBrokerCommission,

                };

                _commissionRuleService.UpdateCommissionRuleInstrument(dtRuleInstrument, commissionRuleViewModel);
                return Json(new { StatusCode = 200, Message = "Commission rule updated successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { StatusCode = 400, Message = ex.Message.ToString() });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveCommissionRule(IFormFile FileUpload, CommissionRuleViewModel model)
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

                            List<ImportCommissionRuleInstrumentViewModel> importCommissionRuleInstrumentViewModels = new List<ImportCommissionRuleInstrumentViewModel>();

                            int Id;
                            string InstrumentName;
                            string SymbolGroup;
                            decimal BuyFeePer1000Notional;
                            decimal SellFeePerNotional1000;
                            decimal BuyFeeNotional;
                            decimal SellFeeNotional;
                            decimal FeeBuyPerUnits;
                            decimal FeeSellUnits;
                            string Feecharge;
                            int DaystoApply;
                            string RoundingBuy;
                            string RoundingSell;
                            decimal DecimalCut;
                            
                            for (int rowNo = 1; rowNo < sheet.PhysicalNumberOfRows; rowNo++)
                            {
                                IRow row = sheet.GetRow(rowNo);

                                if (row != null)
                                {
                                    Id = row.GetCell(0) != null ? Convert.ToInt32(row.GetCell(0).NumericCellValue) : 0;
                                    InstrumentName = row.GetCell(1) != null ? row.GetCell(1).ToString() : "";
                                    SymbolGroup = row.GetCell(2) != null ? row.GetCell(2).ToString() : "";
                                    BuyFeePer1000Notional = row.GetCell(3) != null ? Convert.ToDecimal(row.GetCell(3).NumericCellValue) : 0;
                                    SellFeePerNotional1000 = row.GetCell(4) != null ? Convert.ToDecimal(row.GetCell(4).NumericCellValue) : 0;
                                    BuyFeeNotional = row.GetCell(5) != null ? Convert.ToDecimal(row.GetCell(5).NumericCellValue) : 0;
                                    SellFeeNotional = row.GetCell(6) != null ? Convert.ToDecimal(row.GetCell(6).NumericCellValue) : 0;
                                    FeeBuyPerUnits = row.GetCell(7) != null ? Convert.ToDecimal(row.GetCell(7).NumericCellValue) : 0;
                                    FeeSellUnits = row.GetCell(8) != null ? Convert.ToDecimal(row.GetCell(8).NumericCellValue) : 0;
                                    Feecharge = row.GetCell(9) != null ? row.GetCell(9).ToString() : "";
                                    RoundingBuy = row.GetCell(10) != null ? row.GetCell(10).ToString() : "";
                                    RoundingSell = row.GetCell(11) != null ? row.GetCell(11).ToString() : "";
                                    DecimalCut = row.GetCell(12) != null ? Convert.ToDecimal(row.GetCell(12).NumericCellValue) : 0;
                                    DaystoApply = row.GetCell(13) != null ? Convert.ToInt32(row.GetCell(13).NumericCellValue) : 0;

                                    ImportCommissionRuleInstrumentViewModel importCommissionRuleInstrumentViewModel = new ImportCommissionRuleInstrumentViewModel();

                                    importCommissionRuleInstrumentViewModel.Id = Id;
                                    importCommissionRuleInstrumentViewModel.InstrumentName = InstrumentName.Trim();
                                    importCommissionRuleInstrumentViewModel.SymbolGroup = SymbolGroup.Trim();
                                    importCommissionRuleInstrumentViewModel.BuyFeePer1000Notional = BuyFeePer1000Notional;
                                    importCommissionRuleInstrumentViewModel.SellFeePerNotional1000 = SellFeePerNotional1000;
                                    importCommissionRuleInstrumentViewModel.BuyFeeNotional = BuyFeeNotional;
                                    importCommissionRuleInstrumentViewModel.SellFeeNotional = SellFeeNotional;
                                    importCommissionRuleInstrumentViewModel.FeeBuyPerUnits = FeeBuyPerUnits;
                                    importCommissionRuleInstrumentViewModel.FeeSellUnits = FeeSellUnits;
                                    importCommissionRuleInstrumentViewModel.Feecharge = Feecharge;
                                    importCommissionRuleInstrumentViewModel.DaysToApply = DaystoApply;
                                    importCommissionRuleInstrumentViewModel.RoundingBuy = RoundingBuy;
                                    importCommissionRuleInstrumentViewModel.RoundingSell = RoundingSell;
                                    importCommissionRuleInstrumentViewModel.DecimalCut = DecimalCut;


                                    importCommissionRuleInstrumentViewModels.Add(importCommissionRuleInstrumentViewModel);
                                }
                            }

                            ListToDataTableConverter converter = new ListToDataTableConverter();
                            System.Data.DataTable dt = converter.ToDataTable(importCommissionRuleInstrumentViewModels);
                            var ruleid = await _commissionRuleService.AddCommissionRule(dt, model);


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
                else if (model != null)
                {
                    var ruleid = await _commissionRuleService.SaveCommissionRuleWithoutFile(model);
                    return Json(new { StatusCode = true, Message = "Data Saved Successfully", Data = ruleid });
                }
                else
                {
                    return Json(new { StatusCode = false, Message = "Please Add Data" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { StatusCode = 400, Message = ex.Message.ToString() });
            }
        }

        public async Task<IActionResult> ExportCommissionRuleInstrument(int Id)
        {
            PageParam pageParam = new PageParam();
            pageParam.Offset = 0;
            pageParam.Limit = 100000;
            List<CommissionRuleInstrumentViewModel> commissionRuleInstrumentViewModels = new List<CommissionRuleInstrumentViewModel>();
            var result = await _commissionRuleService.GetCommissionRuleInstrumentById(pageParam,"", Id);
            commissionRuleInstrumentViewModels = result.Item1;

            if (commissionRuleInstrumentViewModels != null)
            {
                //return Json(new { draw = requestModel.Draw, recordsFiltered = responseModel.FilteredCount, recordsTotal = responseModel.TotalCount, data = datavalue });
                StringBuilder stringBuilder = new StringBuilder();

                string contextType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                string fileName = "CommissionRuleInstrument_"+DateTime.Now.ToString("MM/dd/yyyy")+"_"+ Id+ ".xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("CommissionRuleInstrument");
                    ////Merge and Center
                    //worksheet.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    //worksheet.Cell("A1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    //worksheet.Range("A1:N1").Merge().Value = "POI Report";
                    //worksheet.Range("A1:N2").Style.Font.Bold = true;

                    worksheet.Cell(1, 1).Value = "Id";
                    worksheet.Cell(1, 2).Value = "Instrument Name";
                    worksheet.Cell(1, 3).Value = "Symbol Group";
                    worksheet.Cell(1, 4).Value = "Buy Fee Per $1000 Notional ($)";
                    worksheet.Cell(1, 5).Value = "Sell Fee Per Notional $1000 ($)";
                    worksheet.Cell(1, 6).Value = "Buy Fee (%) Notional";
                    worksheet.Cell(1, 7).Value = "Sell Fee (%) Notional";
                    worksheet.Cell(1, 8).Value = "Fee Buy Per Units ($)";
                    worksheet.Cell(1, 9).Value = "Fee Sell Units ($)";
                    worksheet.Cell(1, 10).Value = "Fee charge";
                    worksheet.Cell(1, 11).Value = "Rounding Buy";
                    worksheet.Cell(1, 12).Value = "Rounding Sell";
                    worksheet.Cell(1, 13).Value = "Decimal Cut";
                    worksheet.Cell(1, 14).Value = "Days To Apply";

                    worksheet.Range("A1:N1").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                    //worksheet.Range("A1:N2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    for (int index = 1; index <= commissionRuleInstrumentViewModels.Count; index++)
                    {
                        worksheet.Cell(index + 1, 1).Style.NumberFormat.SetFormat("0");
                        worksheet.Cell(index + 1, 1).Value = index;
                        worksheet.Cell(index + 1, 2).Value = commissionRuleInstrumentViewModels[index - 1].InstrumentName;
                        worksheet.Cell(index + 1, 3).Value = commissionRuleInstrumentViewModels[index - 1].SymbolGroup;
                        worksheet.Cell(index + 1, 4).Style.NumberFormat.SetFormat("0.00");
                        worksheet.Cell(index + 1, 4).Value = commissionRuleInstrumentViewModels[index - 1].BuyFeePer1000Notional;
                        worksheet.Cell(index + 1, 5).Style.NumberFormat.SetFormat("0.00");
                        worksheet.Cell(index + 1, 5).Value = commissionRuleInstrumentViewModels[index - 1].SellFeePerNotional1000;
                        worksheet.Cell(index + 1, 6).Style.NumberFormat.SetFormat("0.00");
                        worksheet.Cell(index + 1, 6).Value = commissionRuleInstrumentViewModels[index - 1].BuyFeeNotional;
                        worksheet.Cell(index + 1, 7).Style.NumberFormat.SetFormat("0.00");
                        worksheet.Cell(index + 1, 7).Value = commissionRuleInstrumentViewModels[index - 1].SellFeeNotional;
                        worksheet.Cell(index + 1, 8).Style.NumberFormat.SetFormat("0.00");
                        worksheet.Cell(index + 1, 8).Value = commissionRuleInstrumentViewModels[index - 1].FeeBuyPerUnits;
                        worksheet.Cell(index + 1, 9).Style.NumberFormat.SetFormat("0.00");
                        worksheet.Cell(index + 1, 9).Value = commissionRuleInstrumentViewModels[index - 1].FeeSellUnits;
                        worksheet.Cell(index + 1, 10).Value = commissionRuleInstrumentViewModels[index - 1].Feecharge;
                        worksheet.Cell(index + 1, 11).Value = commissionRuleInstrumentViewModels[index - 1].RoundingBuy;
                        worksheet.Cell(index + 1, 12).Value = commissionRuleInstrumentViewModels[index - 1].RoundingSell;
                        worksheet.Cell(index + 1, 13).Value = commissionRuleInstrumentViewModels[index - 1].DecimalCut;
                        worksheet.Cell(index + 1, 14).Value = commissionRuleInstrumentViewModels[index - 1].DaysToApply;
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

        public async Task<IActionResult> DeleteCommissionRule(int Id)
        {
            List<ResponseViewModel> responseViewModel = new List<ResponseViewModel>();
            ResponseViewModel responseViewModel1 = new ResponseViewModel();
            var res = await _commissionRuleService.DeleteCommissionRuleById(Id);
            if (res != null)
            {
                responseViewModel1.Code = 200;
                responseViewModel1.Message = "Commission Rule Deleted Successfully";
            }
            else
            {
                responseViewModel1.Code = 400;
                responseViewModel1.Message = "Commission Rule Not Deleted";
            }
            responseViewModel.Add(responseViewModel1);
            return Json(responseViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCommissionRuleInstrument(int Id, int CommissionRuleId)
        {
            ResponseViewModel responseViewModel = new ResponseViewModel();

            var res = _commissionRuleService.DeleteCommissionRuleInstrumentById(CommissionRuleId, Id);
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

        [HttpPost]
        public async Task<IActionResult> DeleteCommissonRuleConditionDtlById(int RuleConditionId, int RuleConditionDtlId)
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

        public void GetUnitsType()
        {
            var unitsType = _commissionRuleService.GetUnitsType().Result.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name });

            ViewBag.UnitsType = unitsType;
        }
        public void GetWhenToApplyCombo()
        {
            var whenToCheckCombo = _commissionRuleService.GetWhenToApplyCombo().Result.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name });

            ViewBag.WhenToApplyCombo = whenToCheckCombo;
        }

        public void GetWhereToApplyId()
        {
            var WhereToApplyId = _commissionRuleService.GetWhereToApplyId().Result.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name });

            ViewBag.WhereToApplyId = WhereToApplyId;
        }
    }
}
