using ClosedXML.Excel;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.Instrument;
using Epidi.Models.ViewModel.LeverageRules;
using Epidi.Models.ViewModel.RuleCondition;
using Epidi.Models.ViewModel.TradeRule;
using Epidi.Service.BDM;
using Epidi.Service.CountryService;
using Epidi.Service.IBService;
using Epidi.Service.InstrumentService;
using Epidi.Service.LegalEntityService;
using Epidi.Service.MasterLPService;
using Epidi.Service.PositionTypeService;
using Epidi.Service.RuleConditionService;
using Epidi.Service.TradeRuleService;
using EPIDIWeb.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using System.Diagnostics.Metrics;
using System.Text;

namespace EPIDIWeb.Controllers
{
    public class TradeRuleController : Controller
    {
        private readonly ITradeRuleService _tradeRuleService;
        private readonly IIBService _IBService;
        private readonly ILegalEntityService _legalEntityService;
        private readonly ICountryService _countryService;
        private readonly IPositionTypeService _positionTypeService;
        private readonly IInstrumentService _instrumentService;
        private readonly IRuleConditionService _ruleConditionService;
        private readonly IMasterLPService _masterLPService;
        public TradeRuleController(ITradeRuleService tradeRuleService, IIBService iBService,
            ILegalEntityService legalEntityService, ICountryService countryService, IPositionTypeService positionTypeService, IInstrumentService instrumentService,
            IRuleConditionService ruleConditionService,
            IMasterLPService masterLPService)
        {
            _tradeRuleService = tradeRuleService;
            _IBService = iBService;
            _legalEntityService = legalEntityService;
            _countryService = countryService;
            _positionTypeService = positionTypeService;
            _instrumentService = instrumentService;
            _ruleConditionService = ruleConditionService;
            _masterLPService = masterLPService;
        }

        public IActionResult Index()
        {
            ViewBag.Title = "TradeRule List";
            return View();
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
            var response = _tradeRuleService.GetAllTradeRule(pageParam, search);

            totalRecord = response.Item2;
            filteredRecord = response.Item2;

            var responseData = OrderByExtension.OrderBy(response.Item1.AsQueryable(), sortColumn, sortColumnDirection).ToList();

            return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = responseData });
        }
        public IActionResult ImportTradeRule()
        {
            return View(new TradeRuleViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> UploadTradeRule(IFormFile FileUpload, TradeRuleViewModel tradeRuleViewModel)
        {
            string filePath = string.Empty;
            var tradeRuleId = tradeRuleViewModel.Id;
            var tradeRuleName = tradeRuleViewModel.Name;
            var tradeRulePriority = tradeRuleViewModel.Priority;
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
                            List<TradeValuesViewModel> tradeValuesViewModels = new List<TradeValuesViewModel>();

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



                            //IRow row1 = sheet.GetRow(1);
                            //for (int i = 0; i <= row1.PhysicalNumberOfCells; i++)
                            //{
                            //    string values = row1.GetCell(i) != null ? Convert.ToString(row1.GetCell(i)) : "";
                            //    if (values.Contains("Value"))
                            //    {
                            //        var resp = await _tradeRuleService.CheckTradeRuleUniversalValues(values);
                            //        if (!resp)
                            //        {
                            //            return Json(new { StatusCode = 400, Message = "File not import, we did not see all universal values please add universal values" });
                            //        }

                            //    }

                            //}

                            //int counter = 18;
                            //int no = 1;
                            //for (int rowNo = 2; rowNo < sheet.PhysicalNumberOfRows; rowNo++)
                            //{
                            //    TradeValuesViewModel tradeValuesViewModel = new TradeValuesViewModel();


                            //    IRow row = sheet.GetRow(rowNo);
                            //    tradeValuesViewModel.TradeValueName = "Value"+no;
                            //    tradeValuesViewModel.TradeValues = row.GetCell(counter) != null ? Convert.ToDecimal(row.GetCell(counter).NumericCellValue) : 0;

                            //    tradeValuesViewModels.Add(tradeValuesViewModel);
                            //    counter++;
                            //    no++;
                            //}
                            IRow firstRow = sheet.GetRow(2);
                            int columnCount = 0;
                            if (firstRow != null)
                            {
                                columnCount = firstRow.LastCellNum;
                            }

                            if (!CheckUniversalValuesValidation(columnCount))
                            {
                                return Json(new { StatusCode = 400, Message = "Universal Values not match with excel file" });
                            }


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
                                // TimeTO = Convert.ToString(row.GetCell(8).DateCellValue.ToString("HH:mm:ss")) != null ? Convert.ToString(row.GetCell(8).DateCellValue.ToString("HH:mm:ss")) : "";
                                TimeTO =  row.GetCell(8) != null ? row.GetCell(8).ToString() : "";
                                //row.GetCell(8) != null ? row.GetCell(8).ToString() : "";
                                // TimeFrom = Convert.ToString(row.GetCell(9).DateCellValue.ToString("HH:mm:ss")) != null ? Convert.ToString(row.GetCell(9).DateCellValue.ToString("HH:mm:ss")) : "";
                                TimeFrom = row.GetCell(8) != null ? row.GetCell(8).ToString() : "";
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
                                await _tradeRuleService.DeleteTradeRuleInstrumentPriority(tradeRuleViewModel.Id);
                            }

                            tradeRuleViewModel.Id = Convert.ToInt32(await _tradeRuleService.TradeRule_InsertByImport(dt, tradeRuleId, tradeRuleName,tradeRulePriority));


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
        public async Task<IActionResult> Edit(int Id)
        {
            List<SelectListItem> oprationList = new List<SelectListItem>();
            List<SelectListItem> FieldsList = new List<SelectListItem>();
            PageParam pageParam = new PageParam();
            pageParam.Offset = 0;
            pageParam.Limit = 100;
            var instruments =  _instrumentService.GetInstrumentListAll().Result.Select(p => new SelectListItem() { Value = p.id.ToString(), Text = p.name }); ;
            ViewBag.InstrumentList = JsonConvert.SerializeObject(instruments);

            oprationList.Add(new SelectListItem { Text = "--Select Instrument--", Value = "0" });
            foreach (var item in instruments)
            {
                oprationList.Add(new SelectListItem { Text = item.Text, Value = item.Value });
            }
            ViewBag.Instrument = oprationList;

           
            var lpResponse = _masterLPService.GetAll(pageParam, "");
            FieldsList.Add(new SelectListItem { Text = "--Select LP--", Value = "0" });
            foreach (var item in lpResponse.Item1)
            {
                FieldsList.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }
            //var FieldsList = lpResponse.Item1.Select(p => new SelectListItem()
            //{
            //    Value = p.Id.ToString(),
            //    Text = p.Name
            //});
            ViewBag.LPList = FieldsList;

            var response = await _tradeRuleService.GetById(Id);
            var ruleConditions = await _ruleConditionService.GetRuleConditionByTradeRuleId(Id);
            response.objRuleConditionViewModel = ruleConditions;
            if (ruleConditions != null)
            {
                response.objRuleConditionViewModel.conditionRulesDtlObj = JsonConvert.SerializeObject(ruleConditions.rulesDtlViewModel);
            }
            if (response != null)
            {
                return View("EditTradeRule", response);
            }
            else
            {
                return View(new TradeStatusViewModel());
            }
        }
        //[HttpGet]
        //public async Task<JsonResult> Get_InstrumentPriorityByTradeRuleId(int Rule_Id, int lp_filter, int instrument_filter, int day_filter)
        //{
        //    var response = await _tradeRuleService.GetByInstrumentTradeRuleByRuleId(Rule_Id, lp_filter, instrument_filter, day_filter);
        //    return Json(response);
        //}

        [HttpPost]
        public async Task<JsonResult> Get_InstrumentPriorityByTradeRuleId(int Rule_Id,int InstrumentId,int LpId,string Day)
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
            var response = await _tradeRuleService.GetByInstrumentTradeRuleByRuleId(pageParam, search, Rule_Id,InstrumentId,LpId,Day);

            totalRecord = response.Item2;
            filteredRecord = response.Item2;

            //var responseData = OrderByExtension.OrderBy(response.Item1.AsQueryable(), sortColumn, sortColumnDirection).ToList();

            return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = response.Item1 });

        }
        [HttpGet]
        public JsonResult Get_TradeUniversalValues()
        {
            var response = _tradeRuleService.Get_TradeUniversalValues().Result;
            return Json(response);
        }
        public async Task<JsonResult> GetIBDropDown()
        {
            List<SelectListItem> FildsName = new List<SelectListItem>();
            PageParam pageParam = new PageParam();
            pageParam.Offset = 0;
            pageParam.Limit = 100;
            var response = _IBService.GetAll(pageParam, "");
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
        public async Task<JsonResult> GetLegalEntityDropDown()
        {
            List<SelectListItem> FildsName = new List<SelectListItem>();
            PageParam pageParam = new PageParam();
            pageParam.Offset = 0;
            pageParam.Limit = 100;
            var response = _legalEntityService.GetLegalEntity_All(pageParam, "");
            var FieldsList = response.Result.Item1.Select(p => new SelectListItem()
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
        public async Task<JsonResult> GetCountryDropDown()
        {
            List<SelectListItem> FildsName = new List<SelectListItem>();
            PageParam pageParam = new PageParam();
            pageParam.Offset = 0;
            pageParam.Limit = 100;
            var response = _countryService.GetAllCountries().Result.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.CountryName });
            foreach (var item in response)
            {
                FildsName.Add(new SelectListItem { Text = item.Text, Value = item.Value });
            }
            return Json(FildsName);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteTradeRuleInstrument(int RuleId, int RulePriorityId)
        {
            ResponseViewModel responseViewModel = new ResponseViewModel();

            var res = _tradeRuleService.DeleteTradeRuleInstrumentById(RuleId, RulePriorityId);
            if (res != null)
            {
                responseViewModel.Code = 200;
                responseViewModel.Message = "Trade Rule Deleted Successfully";
            }
            else
            {
                responseViewModel.Code = 400;
                responseViewModel.Message = "Trade Rule Not Deleted";
            }
            return Json(responseViewModel);
        }
        public async Task<JsonResult> GetPositionTypeDropDown()
        {
            List<SelectListItem> FildsName = new List<SelectListItem>();
            PageParam pageParam = new PageParam();
            pageParam.Offset = 0;
            pageParam.Limit = 100;
            var response = _positionTypeService.Get_All().Result.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Type });
            foreach (var item in response)
            {
                FildsName.Add(new SelectListItem { Text = item.Text, Value = item.Value });
            }
            return Json(FildsName);
        }
        [HttpPost]
        public async Task<IActionResult> SaveTradeRule(TradeRuleViewModelEdit model)
        {
            try
            {
                model.objRuleConditionViewModel = JsonConvert.DeserializeObject<List<LeverageRulesDtlViewModel>>(model.objRuleConditionViewModelstr);

                model.objRuleInstrumentViewModel = JsonConvert.DeserializeObject<List<TradeRuleInstrumentViewModelGet>>(model.objRuleInstrumentViewModelstr);

                ListToDataTableConverter converter = new ListToDataTableConverter();

                DataTable dtRuleInstrument = converter.ToDataTable(model.objRuleInstrumentViewModel);
                DataTable dtRuleConditionsDtl = converter.ToDataTable(model.objRuleConditionViewModel);

                if (dtRuleConditionsDtl.Rows.Count > 0)
                {
                    RuleConditionViewModel ruleConditionViewModel = new RuleConditionViewModel();
                    ruleConditionViewModel.TradeRuleId = Convert.ToInt32(model.Id);
                    ruleConditionViewModel.Id = Convert.ToInt32(dtRuleConditionsDtl.Rows[0]["RuleConditionsId"]);
                    ruleConditionViewModel.rulesDtlViewModel = model.objRuleConditionViewModel;

                    _ruleConditionService.Upsert(ruleConditionViewModel);
                }

                TradeRuleViewModel quoteMarkUpRuleViewModel = new TradeRuleViewModel
                {
                    Id = model.Id,
                    Name = model.Name,
                    Priority = model.Priority,
                };
                //         public string InstrumentName { get; set; }
                //public string SymbolGroups { get; set; }
                //public string IB { get; set; }
                //public string LegalEntity { get; set; }
                //public string CountryName { get; set; }
                //public string LPName { get; set; }
                //public string PositionType { get; set; }

                dtRuleInstrument.Columns.Remove("InstrumentName");
                dtRuleInstrument.Columns.Remove("SymbolGroups");
                dtRuleInstrument.Columns.Remove("IB");
                dtRuleInstrument.Columns.Remove("LegalEntity");
                dtRuleInstrument.Columns.Remove("CountryName");
                dtRuleInstrument.Columns.Remove("LPName");
                dtRuleInstrument.Columns.Remove("PositionType");

                _tradeRuleService.Upsert(quoteMarkUpRuleViewModel);

                _tradeRuleService.TradeRule_UpdateByDt(dtRuleInstrument);
                return Json(new { StatusCode = 200, Message = "Trade rule updated Successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { StatusCode = 400, Message = ex.Message.ToString() });
            }
        }
        private void setCellStyle(HSSFWorkbook workbook, ICell cell, bool isHeader, bool evenOddPrio)
        {
            HSSFCellStyle fCellStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            if (isHeader)
            {
                fCellStyle.FillForegroundColor = HSSFColor.LightOrange.Index;
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
        public async Task<IActionResult> ExportTradeRuleOld(TradeRuleViewModelEdit model)
        {
            try
            {
                PageParam pageParam = new PageParam();
                pageParam.Offset = 0;
                pageParam.Limit = 0;
                List<InstrumentMaster> instrumentMasterViewModels = new List<InstrumentMaster>();
                var result = await _instrumentService.GetAllInstrument(pageParam, "");
                instrumentMasterViewModels = result.Item1;

                ListToDataTableConverter converter = new ListToDataTableConverter();
                List<TradeRuleInstrumentViewModelExport> modelExport = new List<TradeRuleInstrumentViewModelExport>();
                modelExport = JsonConvert.DeserializeObject<List<TradeRuleInstrumentViewModelExport>>(model.objRuleInstrumentViewModelstr);
                DataTable dtRuleInstrument = converter.ToDataTable(modelExport);

                for (int cnt = 0; cnt < dtRuleInstrument.Rows.Count; cnt++)
                {
                    var symbolGroups = instrumentMasterViewModels.Where(a => a.name == Convert.ToString(dtRuleInstrument.Rows[cnt]["InstrumentName"]).Trim()).FirstOrDefault();
                    dtRuleInstrument.Rows[cnt]["SymbolGroups"] = symbolGroups.GroupName.ToString().Trim();
                }
                var max_priority = modelExport.Select(a => a.Priority).Distinct();

                var workbook = new HSSFWorkbook();
                var sheet = workbook.CreateSheet("Sheet1");

                //HSSFFont hFont = (HSSFFont)workbook.CreateFont();

                //hFont.FontHeightInPoints = 11;
                //hFont.FontName = "Calibri";
                ////hFont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;

                //HSSFCellStyle hStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                //hStyle.SetFont(hFont);

                // Convert datatable into json
                string JSON = JsonConvert.SerializeObject(dtRuleInstrument);

                // Convert json into SummaryClass class list  
                var items = JsonConvert.DeserializeObject<List<TradeRuleInstrumentViewModelExport>>(JSON);

                // Set column name this column name use for fetch data from list  
                var columns = new[] { "InstrumentName", "SymbolGroups", "IB", "LegalEntity", "Country", "UID", "SymbolGroups", "Day", "StartTime", "EndTime" };
                var columns_priority = new[] { "LPName", "VolumeFrom", "VolumeTo", "Coefficient", "PositionType", "MaxCoefficient", "Value1", "Value2", "Value3", "Value4", "Value5", "Value6", "Value7", "Value8", "Value9", "Value10"
                    , "Value11", "Value12", "Value13", "Value14", "Value15", "Value16", "Value17", "Value18", "Value19", "Value20", "Value21", "Value22"};

                //// Set header name this header use for set name in excel first row
                //var header = new[] { "Instrument Name", "Symbol Groups", "Day", "Time to", "Time From", "Max Spread" };
                //var header_priority = new[] { "LP", "HedgeType", "Mbid", "Mask", "Timeout" };

                //List<string> Headerlist = new List<string>();
                List<string> columnslist = new List<string>();

                foreach (var item in columns)
                {
                    columnslist.Add(item);
                }

                for (int h_cnt = 0; h_cnt < max_priority.Count(); h_cnt++)
                {
                    foreach (var item in columns_priority)
                    {
                        columnslist.Add(item);
                    }
                }

                sheet.CreateRow(0);
                var headerRow = sheet.CreateRow(1);

                //Below loop is create header  
                for (int i = 0; i < columnslist.Count(); i++)
                {
                    var cell = headerRow.CreateCell(i);
                    string headerName = columnslist[i];
                    if (columnslist[i] == "StartTime")
                    {
                        headerName = "TimeTo";
                    }
                    else if (columnslist[i] == "EndTime")
                    {
                        headerName = "TimeFrom";
                    }
                    cell.SetCellValue(headerName);
                    //sheet1.CreateRow(0).CreateCell(0).CellStyle = style1;
                    //setCellStyle(workbook, cell);
                    setCellStyle(workbook, cell, true, true);
                }
                int rowIndex = 1;
                //Below loop is fill content
                int ColCnt = 0;

                for (int i = 0; i < items.Count; i++)
                {
                    if (i > 0)
                    {
                        //if (items[i - 1].InstrumentName != items[i].InstrumentName || items[i - 1].SymbolGroups != items[i].SymbolGroups ||
                        //    items[i - 1].Day != items[i].Day ||
                        //    items[i - 1].StartTime != items[i].StartTime ||
                        //    items[i - 1].EndTime != items[i].EndTime ||
                        //    items[i - 1].IB != items[i].IB || items[i - 1].Country != items[i].Country)
                        //{
                        ColCnt = 0;
                        //prev_value = items[i].InstrumentName;
                        rowIndex++;
                        var row = sheet.CreateRow(rowIndex);

                        for (int j = 0; j < columns.Length; j++)
                        {
                            var cell = row.CreateCell(ColCnt);
                            var o = items[i];
                            //if (columns[j] == "VolumeFrom" || columns[j] == "VolumeTo" || columns[j] == "Coefficient" || columns[j] == "MaxCoeffient" || columns[j] == "Value1"
                            //    || columns[j] == "Value2" || columns[j] == "Value3" || columns[j] == "Value4" || columns[j] == "Value5" || columns[j] == "Value6"
                            //    || columns[j] == "Value7" || columns[j] == "Value8" || columns[j] == "Value9" || columns[j] == "Value10" || columns[j] == "Value11"
                            //    || columns[j] == "Value12" || columns[j] == "Value13" || columns[j] == "Value14" || columns[j] == "Value15" || columns[j] == "Value16"
                            //    || columns[j] == "Value17" || columns[j] == "Value18" || columns[j] == "Value19" || columns[j] == "Value20" || columns[j] == "Value21" || columns[j] == "Value22")
                            //{
                            //    cell.SetCellValue(Convert.ToDouble(o.GetType().GetProperty(columns[j]).GetValue(o, null)));
                            //}
                            //else
                            //{
                            cell.SetCellValue(o.GetType().GetProperty(columns[j]).GetValue(o, null).ToString());
                            //}

                            setCellStyle(workbook, cell, false, true);
                            ColCnt++;
                        }
                        for (int col = 0; col < columns_priority.Length; col++)
                        {
                            var cell = row.CreateCell(ColCnt);
                            var o = items[i];
                            if (columns_priority[col] == "VolumeFrom" || columns_priority[col] == "VolumeTo" || columns_priority[col] == "Coefficient" || columns_priority[col] == "MaxCoeffient" || columns_priority[col] == "Value1"
                                || columns_priority[col] == "Value2" || columns_priority[col] == "Value3" || columns_priority[col] == "Value4" || columns_priority[col] == "Value5" || columns_priority[col] == "Value6"
                                || columns_priority[col] == "Value7" || columns_priority[col] == "Value8" || columns_priority[col] == "Value9" || columns_priority[col] == "Value10" || columns_priority[col] == "Value11"
                                || columns_priority[col] == "Value12" || columns_priority[col] == "Value13" || columns_priority[col] == "Value14" || columns_priority[col] == "Value15" || columns_priority[col] == "Value16"
                                || columns_priority[col] == "Value17" || columns_priority[col] == "Value18" || columns_priority[col] == "Value19" || columns_priority[col] == "Value20" || columns_priority[col] == "Value21" || columns_priority[col] == "Value22")
                            {
                                cell.SetCellValue(Convert.ToDouble(o.GetType().GetProperty(columns_priority[col]).GetValue(o, null)));
                            }
                            else
                            {
                                cell.SetCellValue(o.GetType().GetProperty(columns_priority[col]).GetValue(o, null).ToString());
                            }
                            setCellStyle(workbook, cell, false, true);
                            ColCnt++;
                        }
                        //}
                        //else
                        //{
                        //    var row = sheet.GetRow(rowIndex);
                        //    for (int col = 0; col < columns_priority.Length; col++)
                        //    {
                        //        var cell = row.CreateCell(ColCnt);
                        //        var o = items[i];
                        //        if (columns[col] == "VolumeFrom" || columns[col] == "VolumeTo" || columns[col] == "Coefficient" || columns[col] == "MaxCoeffient" || columns[col] == "Value1"
                        //            || columns[col] == "Value2" || columns[col] == "Value3" || columns[col] == "Value4" || columns[col] == "Value5" || columns[col] == "Value6"
                        //            || columns[col] == "Value7" || columns[col] == "Value8" || columns[col] == "Value9" || columns[col] == "Value10" || columns[col] == "Value11"
                        //            || columns[col] == "Value12" || columns[col] == "Value13" || columns[col] == "Value14" || columns[col] == "Value15" || columns[col] == "Value16"
                        //            || columns[col] == "Value17" || columns[col] == "Value18" || columns[col] == "Value19" || columns[col] == "Value20" || columns[col] == "Value21" || columns[col] == "Value22")
                        //        {
                        //            cell.SetCellValue(Convert.ToDouble(o.GetType().GetProperty(columns_priority[col]).GetValue(o, null)));
                        //        }
                        //        else
                        //        {
                        //            cell.SetCellValue(o.GetType().GetProperty(columns_priority[col]).GetValue(o, null).ToString());
                        //        }
                        //        setCellStyle(workbook, cell, false, true);
                        //        ColCnt++;
                        //    }
                        //}
                    }
                    else
                    {
                        rowIndex++;
                        var row = sheet.CreateRow(rowIndex);

                        for (int j = 0; j < columns.Count(); j++)
                        {
                            var cell = row.CreateCell(ColCnt);
                            var o = items[i];
                            //if (columns[j] == "MaxSpread")
                            //{
                            //    cell.SetCellValue(Convert.ToDouble(o.GetType().GetProperty(columns[j]).GetValue(o, null)));
                            //}
                            //else
                            //{
                            cell.SetCellValue(o.GetType().GetProperty(columns[j]).GetValue(o, null).ToString());
                            //}

                            setCellStyle(workbook, cell, false, true);
                            ColCnt++;
                        }
                        for (int j = 0; j < columns_priority.Count(); j++)
                        {
                            var cell = row.CreateCell(ColCnt);
                            var o = items[i];
                            if (columns_priority[j] == "VolumeFrom" || columns_priority[j] == "VolumeTo" || columns_priority[j] == "Coefficient" || columns_priority[j] == "MaxCoeffient" || columns_priority[j] == "Value1"
                                || columns_priority[j] == "Value2" || columns_priority[j] == "Value3" || columns_priority[j] == "Value4" || columns_priority[j] == "Value5" || columns_priority[j] == "Value6"
                                || columns_priority[j] == "Value7" || columns_priority[j] == "Value8" || columns_priority[j] == "Value9" || columns_priority[j] == "Value10" || columns_priority[j] == "Value11"
                                || columns_priority[j] == "Value12" || columns_priority[j] == "Value13" || columns_priority[j] == "Value14" || columns_priority[j] == "Value15" || columns_priority[j] == "Value16"
                                || columns_priority[j] == "Value17" || columns_priority[j] == "Value18" || columns_priority[j] == "Value19" || columns_priority[j] == "Value20" || columns_priority[j] == "Value21" || columns_priority[j] == "Value22")
                            {
                                cell.SetCellValue(Convert.ToDouble(o.GetType().GetProperty(columns_priority[j]).GetValue(o, null)));
                            }
                            else
                            {
                                cell.SetCellValue(o.GetType().GetProperty(columns_priority[j]).GetValue(o, null).ToString());
                            }
                            setCellStyle(workbook, cell, false, true);
                            ColCnt++;
                        }
                    }
                }

                // Declare one MemoryStream variable for write file in stream  
                var stream = new MemoryStream();
                workbook.Write(stream);

                //string FilePath = "\\";
                Guid filename = Guid.NewGuid();
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "export");
                var filePath = Path.Combine(folderPath, filename.ToString() + ".xls");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                //Write to file using file stream  
                FileStream file = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write);
                stream.WriteTo(file);
                file.Close();
                stream.Close();

                return Json(new { StatusCode = 200, Message = "Export Quote Markup rule export Successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { StatusCode = 400, Message = ex.Message.ToString() });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ExportTradeRule(int RuleId)
        {
            try
            {
                PageParam pageParam = new PageParam();
                pageParam.Offset = 0;
                pageParam.Limit = 0;
                List<InstrumentMaster> instrumentMasterViewModels = new List<InstrumentMaster>();
                var result = await _instrumentService.GetAllInstrument(pageParam, "");
                instrumentMasterViewModels = result.Item1;

                var tradeUniversal = await _tradeRuleService.Get_TradeUniversalValues();

                List<TradeRuleInstrumentViewModelGet> modelExport = new List<TradeRuleInstrumentViewModelGet>();
               // modelExport = await _tradeRuleService.GetByInstrumentTradeRuleByRuleId(RuleId);

                if (modelExport != null)
                {
                    StringBuilder stringBuilder = new StringBuilder();

                    string contextType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    string fileName = "TradeRule_"+DateTime.Now.ToString("MM/dd/yyyy")+".xlsx";
                    int RoWIndex = 1;
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Sheet1");
                        worksheet.Range("A1:D1").Value = "Trade Rule";
                        RoWIndex++;
                        worksheet.Cell(RoWIndex, 1).Value = "InstrumentName";
                        worksheet.Cell(RoWIndex, 2).Value = "SymbolGroups";
                        worksheet.Cell(RoWIndex, 3).Value = "IB";
                        worksheet.Cell(RoWIndex, 4).Value = "LegalEntity";
                        worksheet.Cell(RoWIndex, 5).Value = "Country";
                        worksheet.Cell(RoWIndex, 6).Value = "UID";
                        worksheet.Cell(RoWIndex, 7).Value = "SymbolGroups";
                        worksheet.Cell(RoWIndex, 8).Value = "Day";
                        worksheet.Cell(RoWIndex, 9).Value = "TimeTo";
                        worksheet.Cell(RoWIndex, 10).Value = "TimeFrom";
                        worksheet.Cell(RoWIndex, 11).Value = "Priority";

                        worksheet.Cell(RoWIndex, 12).Value = "LPName";
                        worksheet.Cell(RoWIndex, 13).Value = "HedgeType";
                        worksheet.Cell(RoWIndex, 14).Value = "VolumeFrom";
                        worksheet.Cell(RoWIndex, 15).Value = "VolumeTo";
                        worksheet.Cell(RoWIndex, 16).Value = "Coefficient";
                        worksheet.Cell(RoWIndex, 17).Value = "PositionType";
                        worksheet.Cell(RoWIndex, 18).Value = "MaxCoefficient";

                        int Counter = 19;
                        foreach (var item in tradeUniversal)
                        {
                            worksheet.Cell(RoWIndex, Counter).Value = item.Values;
                            worksheet.Cell(RoWIndex, Counter).Style.Fill.SetBackgroundColor(XLColor.Yellow);
                            Counter++;
                        }
                        //worksheet.Cell(RoWIndex, 19).Value = "Value1";
                        //worksheet.Cell(RoWIndex, 20).Value = "Value2";
                        //worksheet.Cell(RoWIndex, 21).Value = "Value3";
                        //worksheet.Cell(RoWIndex, 22).Value = "Value4";
                        //worksheet.Cell(RoWIndex, 23).Value = "Value5";
                        //worksheet.Cell(RoWIndex, 24).Value = "Value6";
                        //worksheet.Cell(RoWIndex, 25).Value = "Value7";
                        //worksheet.Cell(RoWIndex, 26).Value = "Value8";
                        //worksheet.Cell(RoWIndex, 27).Value = "Value9";
                        //worksheet.Cell(RoWIndex, 28).Value = "Value10";
                        //worksheet.Cell(RoWIndex, 29).Value = "Value11";
                        //worksheet.Cell(RoWIndex, 30).Value = "Value12";
                        //worksheet.Cell(RoWIndex, 31).Value = "Value13";
                        //worksheet.Cell(RoWIndex, 32).Value = "Value14";
                        //worksheet.Cell(RoWIndex, 33).Value = "Value15";
                        //worksheet.Cell(RoWIndex, 34).Value = "Value16";
                        //worksheet.Cell(RoWIndex, 35).Value = "Value17";
                        //worksheet.Cell(RoWIndex, 36).Value = "Value18";
                        //worksheet.Cell(RoWIndex, 37).Value = "Value19";
                        //worksheet.Cell(RoWIndex, 38).Value = "Value20";
                        //worksheet.Cell(RoWIndex, 39).Value = "Value21";
                        //worksheet.Cell(RoWIndex, 40).Value = "Value22";

                        //worksheet.Row(2).Style.Fill.SetBackgroundColor(XLColor.Yellow);
                        worksheet.Range("A" + RoWIndex + ":R" + RoWIndex).Style.Fill.SetBackgroundColor(XLColor.Yellow);

                        RoWIndex++;
                        for (int i = 0; i < modelExport.Count; i++)
                        {
                            worksheet.Cell(RoWIndex, 1).Value = modelExport[i].InstrumentName;
                            worksheet.Cell(RoWIndex, 2).Value = modelExport[i].SymbolGroups;
                            worksheet.Cell(RoWIndex, 3).Value = modelExport[i].IB;
                            worksheet.Cell(RoWIndex, 4).Value = modelExport[i].LegalEntity;
                            worksheet.Cell(RoWIndex, 5).Value = modelExport[i].CountryName;
                            worksheet.Cell(RoWIndex, 6).Value = modelExport[i].UID;
                            worksheet.Cell(RoWIndex, 7).Value = modelExport[i].SymbolGroups;
                            worksheet.Cell(RoWIndex, 8).Value = modelExport[i].Day;
                            worksheet.Cell(RoWIndex, 9).Value = modelExport[i].StartTime;
                            worksheet.Cell(RoWIndex, 10).Value = modelExport[i].EndTime;
                            worksheet.Cell(RoWIndex, 11).Value = modelExport[i].Priority;

                            worksheet.Cell(RoWIndex, 12).Value = modelExport[i].LPName;
                            worksheet.Cell(RoWIndex, 13).Value = modelExport[i].HedgeType;
                            worksheet.Cell(RoWIndex, 14).Value = modelExport[i].VolumeFrom;
                            worksheet.Cell(RoWIndex, 15).Value = modelExport[i].VolumeTo;
                            worksheet.Cell(RoWIndex, 16).Value = modelExport[i].Coefficient;
                            worksheet.Cell(RoWIndex, 17).Value = modelExport[i].PositionType;
                            worksheet.Cell(RoWIndex, 18).Value = modelExport[i].MaxCoefficient;

                            Counter = 19;
                            foreach (var item in tradeUniversal)
                            {
                                worksheet.Cell(RoWIndex, Counter).Style.Fill.SetBackgroundColor(XLColor.LightBlue);

                                switch (item.Values)
                                {
                                    case "Value 1":
                                        worksheet.Cell(RoWIndex, Counter).Value = modelExport[i].Value1;
                                        break;
                                    case "Value 2":
                                        worksheet.Cell(RoWIndex, Counter).Value = modelExport[i].Value2;
                                        break;
                                    case "Value 3":
                                        worksheet.Cell(RoWIndex, Counter).Value = modelExport[i].Value3;
                                        break;
                                    case "Value 4":
                                        worksheet.Cell(RoWIndex, Counter).Value = modelExport[i].Value4;
                                        break;
                                    case "Value 5":
                                        worksheet.Cell(RoWIndex, Counter).Value = modelExport[i].Value5;
                                        break;
                                    case "Value 6":
                                        worksheet.Cell(RoWIndex, Counter).Value = modelExport[i].Value6;
                                        break;
                                    case "Value 7":
                                        worksheet.Cell(RoWIndex, Counter).Value = modelExport[i].Value7;
                                        break;
                                    case "Value 8":
                                        worksheet.Cell(RoWIndex, Counter).Value = modelExport[i].Value8;
                                        break;
                                    case "Value 9":
                                        worksheet.Cell(RoWIndex, Counter).Value = modelExport[i].Value9;
                                        break;
                                    case "Value 10":
                                        worksheet.Cell(RoWIndex, Counter).Value = modelExport[i].Value10;
                                        break;
                                    case "Value 11":
                                        worksheet.Cell(RoWIndex, Counter).Value = modelExport[i].Value11;
                                        break;
                                    case "Value 12":
                                        worksheet.Cell(RoWIndex, Counter).Value = modelExport[i].Value12;
                                        break;
                                    case "Value 13":
                                        worksheet.Cell(RoWIndex, Counter).Value = modelExport[i].Value13;
                                        break;
                                    case "Value 14":
                                        worksheet.Cell(RoWIndex, Counter).Value = modelExport[i].Value14;
                                        break;
                                    case "Value 15":
                                        worksheet.Cell(RoWIndex, Counter).Value = modelExport[i].Value15;
                                        break;
                                    case "Value 16":
                                        worksheet.Cell(RoWIndex, Counter).Value = modelExport[i].Value16;
                                        break;
                                    case "Value 17":
                                        worksheet.Cell(RoWIndex, Counter).Value = modelExport[i].Value17;
                                        break;
                                    case "Value 18":
                                        worksheet.Cell(RoWIndex, Counter).Value = modelExport[i].Value18;
                                        break;
                                    case "Value 19":
                                        worksheet.Cell(RoWIndex, Counter).Value = modelExport[i].Value19;
                                        break;
                                    case "Value 20":
                                        worksheet.Cell(RoWIndex, Counter).Value = modelExport[i].Value20;
                                        break;
                                    case "Value 21":
                                        worksheet.Cell(RoWIndex, Counter).Value = modelExport[i].Value21;
                                        break;
                                    case "Value 22":
                                        worksheet.Cell(RoWIndex, Counter).Value = modelExport[i].Value22;
                                        break;
                                }
                                Counter++;
                            }

                            
                            
                            
                            //worksheet.Cell(RoWIndex, 22).Value = modelExport[i].Value4;
                            //worksheet.Cell(RoWIndex, 23).Value = modelExport[i].Value5;
                            //worksheet.Cell(RoWIndex, 24).Value = modelExport[i].Value6;
                            //worksheet.Cell(RoWIndex, 25).Value = modelExport[i].Value7;
                            //worksheet.Cell(RoWIndex, 26).Value = modelExport[i].Value8;
                            //worksheet.Cell(RoWIndex, 27).Value = modelExport[i].Value9;
                            //worksheet.Cell(RoWIndex, 28).Value = modelExport[i].Value10;
                            //worksheet.Cell(RoWIndex, 29).Value = modelExport[i].Value11;
                            //worksheet.Cell(RoWIndex, 30).Value = modelExport[i].Value12;
                            //worksheet.Cell(RoWIndex, 31).Value = modelExport[i].Value13;
                            //worksheet.Cell(RoWIndex, 32).Value = modelExport[i].Value14;
                            //worksheet.Cell(RoWIndex, 33).Value = modelExport[i].Value15;
                            //worksheet.Cell(RoWIndex, 34).Value = modelExport[i].Value16;
                            //worksheet.Cell(RoWIndex, 35).Value = modelExport[i].Value17;
                            //worksheet.Cell(RoWIndex, 36).Value = modelExport[i].Value18;
                            //worksheet.Cell(RoWIndex, 37).Value = modelExport[i].Value19;
                            //worksheet.Cell(RoWIndex, 38).Value = modelExport[i].Value20;
                            //worksheet.Cell(RoWIndex, 39).Value = modelExport[i].Value21;
                            //worksheet.Cell(RoWIndex, 40).Value = modelExport[i].Value22;
                            worksheet.Range("L" + RoWIndex + ":R" + RoWIndex).Style.Fill.SetBackgroundColor(XLColor.LightBlue);
                            RoWIndex++;
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
                return Json(new { StatusCode = 200, Message = "Export Quote Markup rule export Successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { StatusCode = 400, Message = ex.Message.ToString() });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRuleConditionById(int Id, int RuleConditionId)
        {
            ResponseViewModel responseViewModel = new ResponseViewModel();

            var res = _ruleConditionService.DeleteConditionRulesDtlByTraedRuleId(Id, RuleConditionId);
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

        public bool CheckUniversalValuesValidation(int ExcelValTotalCount)
        {
            bool IsValid = true;
            try
            {
                int ExcelFixedColCnt = 18;
                int ValidValuesCnt = ExcelValTotalCount - ExcelFixedColCnt;
                var response = _tradeRuleService.Get_TradeUniversalValues();
                int TotalCount = response.Result.Count();
                if (TotalCount < ValidValuesCnt)
                {
                    IsValid = false;
                }
            }
            catch (Exception ex)
            {
                IsValid = false;
            }
            return IsValid;
        }
        
        [HttpPost]
        public async Task<IActionResult> CopyTradeRule(int Id)
        {
            var res = await _tradeRuleService.CopyTradeRule(Id);
            return Json(res);
        }
        [HttpGet]
        public IActionResult Delete(int Id)
        {
            var res = _tradeRuleService.Delete(Id);
            return Json(res);
        }



    }
}
