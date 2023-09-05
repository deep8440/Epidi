using ClosedXML.Excel;
using DataTables.AspNet.AspNetCore;
using DocumentFormat.OpenXml.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2016.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Epidi.Models.Model;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.Instrument;
using Epidi.Models.ViewModel.Question;
using Epidi.Models.ViewModel.RuleCondition;
using Epidi.Service.BDM;
using Epidi.Service.InstrumentService;
using Epidi.Service.LegalEntityService;
using Epidi.Service.LeverageRulesService;
using Epidi.Service.RuleConditionService;
using Epidi.Service.UsersService;
using EPIDIWeb.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.POIFS.Crypt.Dsig;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Text;

namespace EPIDIWeb.Controllers
{
    public class LevarageRuleImport : Controller
    {
        private readonly ILeverageRulesService _leverageRulesService;
        private readonly IMemoryCache _memoryCache;
        private readonly IBDMService _bdmService;
        private readonly IInstrumentService _instrumentService;
        private readonly ILegalEntityService _legalEntityService;
        private readonly IUsersService _userService;
        private readonly IRuleConditionService _ruleConditionService;
        public LevarageRuleImport(ILeverageRulesService leverageRulesService, IMemoryCache memoryCache,
            IBDMService bdmService, IInstrumentService instrumentService, ILegalEntityService legalEntityService, IUsersService usersService,
            IRuleConditionService ruleConditionService)
        {
            _leverageRulesService = leverageRulesService;
            _memoryCache = memoryCache;
            _bdmService = bdmService;
            _instrumentService = instrumentService;
            _legalEntityService = legalEntityService;
            _userService = usersService;
            _ruleConditionService = ruleConditionService;
        }
        public IActionResult Index()
        {
            ViewBag.Title = "LevarageRule List";
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UploadLevarageRuleExcel(IFormFile FileUpload, LeverageRuleImport leverageRuleImport)
        {
            string filePath = string.Empty;
            var leverageRuleID = leverageRuleImport.LeverageRuleId;
            try
            {
                ResponseViewModel rvm = new ResponseViewModel();
                if (!string.IsNullOrEmpty(leverageRuleImport.LeverageName))
                {
                    rvm = _leverageRulesService.InsertLeverageRule(leverageRuleImport);
                }
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

                            List<LeverageRuleImport> gateIOInstrumentMappingViewModels = new List<LeverageRuleImport>();
                            List<LevarageBand> levarageBands = new List<LevarageBand>();

                            for (int rowNo = 1; rowNo < sheet.PhysicalNumberOfRows; rowNo++)
                            {
                                IRow row = sheet.GetRow(rowNo);
                                IRow headers = sheet.GetRow(0);
                                //for (int i = 4; i < row.Cells.Count; i++)
                                //{
                                //    if (headers.GetCell(i).ToString().Contains("Band")
                                //    || headers.GetCell(i).ToString().Contains("Levarage"))
                                //    {
                                //        levarageBands.Add(new LevarageBand()
                                //        {
                                //            BandFrom = Convert.ToDecimal(string.IsNullOrEmpty(row.GetCell(i).ToString()) ? "0" : row.GetCell(i).ToString()),
                                //            BandTo = Convert.ToDecimal(string.IsNullOrEmpty(row.GetCell(i + 1).ToString()) ? "0" : row.GetCell(i + 1).ToString()),
                                //            LeverageAmount = Convert.ToDecimal(string.IsNullOrEmpty(row.GetCell(i + 2).ToString()) ? "0" : row.GetCell(i + 2).ToString())
                                //        });
                                //        i = i + 2;
                                //    }
                                //}
                                
                                LeverageRuleImport model =new LeverageRuleImport();
                                model.Id=rowNo;
                                model.MasterInstrumentName = Convert.ToString(row.GetCell(0)) != null ? Convert.ToString(row.GetCell(0)) : "";
                                model.SymbolGroupName = Convert.ToString(row.GetCell(1)) != null ? Convert.ToString(row.GetCell(1)) : "";
                                model.Day = row.GetCell(2) != null ? Convert.ToDecimal(row.GetCell(2).ToString()) : 0;
                                model.LeverageRuleId = rvm.Id;
                                model.TimeFrom = Convert.ToString(row.GetCell(3).DateCellValue.ToString("HH:mm:ss")) != null ? Convert.ToString(row.GetCell(3).DateCellValue.ToString("HH:mm:ss")) : "";
                                model.TimeTo = Convert.ToString(row.GetCell(4).DateCellValue.ToString("HH:mm:ss")) != null ? Convert.ToString(row.GetCell(4).DateCellValue.ToString("HH:mm:ss")) : "";
                                model.Priority = row.GetCell(5) != null ? Convert.ToInt32(row.GetCell(5).NumericCellValue) : 0;
                                model.LegalEntity = row.GetCell(10) != null ? Convert.ToString(row.GetCell(10).ToString()) : "";
                                model.IsNewPosition = row.GetCell(9) != null ? Convert.ToString(row.GetCell(9)) : "";
                                
                                model.BandFrom = Convert.ToDecimal(string.IsNullOrEmpty(row.GetCell(6).ToString()) ? "0" : row.GetCell(6).ToString());
                                model.BandTo = Convert.ToDecimal(string.IsNullOrEmpty(row.GetCell(7).ToString()) ? "0" : row.GetCell(7).ToString());
                                model.LeverageAmount = Convert.ToDecimal(string.IsNullOrEmpty(row.GetCell(8).ToString()) ? "0" : row.GetCell(8).ToString());

								gateIOInstrumentMappingViewModels.Add(model);

                                //var response = _leverageRulesService.ImportLevarageRule(gateIOInstrumentMappingViewModels.FirstOrDefault());
                                //levarageBands.ForEach(x => x.LeverageId = response.Id);
                                //var res = _leverageRulesService.ImportLevarageBand(levarageBands);
                                //gateIOInstrumentMappingViewModels.Clear();
                                //levarageBands.Clear();
                            }
							

							ListToDataTableConverter converter = new ListToDataTableConverter();
							System.Data.DataTable dt = converter.ToDataTable(gateIOInstrumentMappingViewModels);
							//dt.Columns.Remove("tradeTimings");
							//dt.Columns.Remove("tradeId");
							//dt.Columns.Remove("Path");
							//dt.Columns.Remove("Image");

							var columnsToKeep = new List<string>() {"Id", "MasterInstrumentName", "SymbolGroupName", "Day", "LeverageRuleId",  "TimeFrom", "TimeTo", "Priority", "LegalEntity", "IsNewPosition",  "BandFrom", "BandTo", "LeverageAmount" };
							var toRemove = new List<DataColumn>();

                            foreach (DataColumn column in dt.Columns)
                            {
                                if (!columnsToKeep.Any(name => column.ColumnName == name))
                                {
                                    toRemove.Add(column);
                                }
                            }

                            toRemove.ForEach(col => dt.Columns.Remove(col));

							long ruleId = await _leverageRulesService.ImportLevarageRule(dt);


							return Json(new { StatusCode = 200, Id = rvm.Id, Message = "File Imported Successfully" });
                        }
                        catch (Exception ex)
                        {
                            return Json(new { StatusCode = 400, Message = ex.Message.ToString() });
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

        [HttpPost]
        public async Task<JsonResult> GetAll()
        {
            long totalRecord = 0;
            long filteredRecord = 0;
            var Draw = Request.Form["draw"].FirstOrDefault();
            PageParam pageParam = new PageParam();
            pageParam.Offset = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            pageParam.Limit = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");

            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();


            string search = Convert.ToString(Request.Form["search[value]"].FirstOrDefault() ?? "");
            var response = _leverageRulesService.GetAllLeverageRules(pageParam, search);

            totalRecord = response.Item2;
            filteredRecord = response.Item2;

            var responseData = OrderByExtension.OrderBy(response.Item1.AsQueryable(), sortColumn, sortColumnDirection).ToList();


            return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = responseData });

        }

        [ResponseCache(Duration = 60)]
        public async Task<ActionResult> AddLeverageRule(int id, int name)
         {
            try
            {
                PageParam pageParam = new PageParam();
                var model = new LeverageRuleImport();
                //var instruments = await _instrumentService.GetInstrumentListAll();
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
                var legalEntity = await _legalEntityService.GetLegalEntity_All(pageParam, "");
                ViewBag.InstrumentsDes = instruments;
                ViewBag.Instruments = JsonConvert.SerializeObject(instruments);
                ViewBag.LegalEntity = JsonConvert.SerializeObject(legalEntity.Item1);
                if (id > 0 || name != 0)
                {
                    ViewBag.RuleID = id;
                   
                    //string search = Convert.ToString(Request.Form["search"].FirstOrDefault() ?? "0");
                    model = await _leverageRulesService.GetLeverageRuleData(id);
                    var response = await _leverageRulesService.GetRuleConditionsByRuleId(id);
                    var FieldsList = _userService.GetUserFields("Users").Result.Select(p => new SelectListItem()
                    {
                        Value = p.ColumnName.ToString() + "|" + p.DataType.ToString() + "|" + p.ForeignKey.ToString(),
                        Text = p.ColumnName
                    });
                    ViewBag.FieldsStr = JsonConvert.SerializeObject(FieldsList);
                    response.ForEach(x => x.Value = x.FieldName + '|' + x.Datatype + '|' + x.ForeignKey);
                    model.RuleConditionStr = JsonConvert.SerializeObject(response);
                    model.instrumentMasters = ViewBag.InstrumentsDes;
                    model.symbolGroups = symbolgroups;
                    model.legalEntities = legalEntity.Item1;
                }

                ViewBag.InstrumentMaster = JsonConvert.SerializeObject(instruments);
                ViewBag.SymbolGroups = JsonConvert.SerializeObject(symbolgroups);
                return View(model);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost]
        public async Task<JsonResult> Get_LeverageRuleInstrumentByRuleId(int id, int InstrumentId = 0)
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
            if (id > 0)
            {
                var response = _leverageRulesService.GetLeverageRuleImports(pageParam, id, "", InstrumentId);

                totalRecord = response.Item2;
                filteredRecord = response.Item2;


                //var responseData = OrderByExtension.OrderBy(response.Item1.AsQueryable(), sortColumn, sortColumnDirection).ToList();

                return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = response.Item1 });
            }
            
            return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = new List<LeverageRuleImport>() });

        }

        //public async Task<ActionResult> AddLeverageRuleData(int id, int name)
        //{
        //    try
        //    {
        //        PageParam pageParam = new PageParam();
        //        var model = new List<LeverageRuleImport>();
        //        var instruments = await _instrumentService.GetInstrumentListAll();
        //        //var instruments = await _instrumentService.GetAllInstrument(pageParam, "");
        //        var symbolgroups = await _instrumentService.GetAllSymbolGroup();
        //        var legalEntity = await _legalEntityService.GetLegalEntity_All(pageParam, "");
        //        ViewBag.Instruments = JsonConvert.SerializeObject(instruments);
        //        ViewBag.LegalEntity = JsonConvert.SerializeObject(legalEntity.Item1);
        //        if (id > 0 || name != 0)
        //        {
        //            ViewBag.RuleID = id;
        //            //string search = Convert.ToString(Request.Form["search"].FirstOrDefault() ?? "0");
        //            model = await _leverageRulesService.GetLeverageRuleImports(id, "", name);
        //            var response = await _leverageRulesService.GetRuleConditionsByRuleId(id);
        //            var FieldsList = _userService.GetUserFields("Users").Result.Select(p => new SelectListItem()
        //            {
        //                Value = p.ColumnName.ToString() + "|" + p.DataType.ToString() + "|" + p.ForeignKey.ToString(),
        //                Text = p.ColumnName
        //            });
        //            ViewBag.FieldsStr = JsonConvert.SerializeObject(FieldsList);
        //            response.ForEach(x => x.Value = x.FieldName + '|' + x.Datatype.ToString() + '|' + x.ForeignKey.ToString());
        //            if (response != null && response.Count > 0 && model.Count > 0)
        //            {
        //                model.FirstOrDefault().RuleConditionStr = JsonConvert.SerializeObject(response);  
        //            }

        //            if(model.Count > 0)
        //            {
        //                model.FirstOrDefault().instrumentMasters = instruments;
        //                model.FirstOrDefault().symbolGroups = symbolgroups;
        //                model.FirstOrDefault().legalEntities = legalEntity.Item1;
        //            }
        //        }

        //        ViewBag.SymbolGroups = JsonConvert.SerializeObject(symbolgroups);
        //        return PartialView("Controls", model);
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //}

        [HttpPost]
        public ActionResult SaveLeverageRule(LeverageRule leverageRule)
        {
            leverageRule.objleverageRuleImport = JsonConvert.DeserializeObject<List<LeverageRuleImport>>(leverageRule.objleverageRuleImportstr);
            leverageRule.objRuleconditions = JsonConvert.DeserializeObject<List<Epidi.Models.ViewModel.LeverageRules.LeverageRulesDtlViewModel>>(leverageRule.objRuleconditionsstr);
            ResponseViewModel rvm = new ResponseViewModel();
            if (!string.IsNullOrEmpty(leverageRule.RuleName))
            {
                LeverageRuleImport leverageRuleImport = new LeverageRuleImport();
                leverageRuleImport.Priority1 = leverageRule.Priority;
                leverageRuleImport.LeverageName = leverageRule.RuleName;
                leverageRuleImport.LeverageRuleId = leverageRule.RuleId;
                leverageRuleImport.LComment = leverageRule.Comment;
                rvm = _leverageRulesService.InsertLeverageRule(leverageRuleImport);
            }
            foreach (var item in leverageRule.objleverageRuleImport)
            {
                item.LeverageRuleId = leverageRule.RuleId;
                var response = _leverageRulesService.UpdateLeverage(item);
                item.LeverageId = response.Id;
                var bandresponse = _leverageRulesService.UpdateBandMapping(item);
            }

            ListToDataTableConverter converter = new ListToDataTableConverter();
            DataTable dtRuleConditionsDtl = converter.ToDataTable(leverageRule.objRuleconditions);
            if (leverageRule.objRuleconditions.Count > 0)
            {
                RuleConditionViewModel ruleConditionViewModel = new RuleConditionViewModel();
                ruleConditionViewModel.LeverageRuleId = leverageRule.RuleId;
                ruleConditionViewModel.Id = Convert.ToInt32(dtRuleConditionsDtl.Rows[0]["RuleConditionsId"]);
                ruleConditionViewModel.rulesDtlViewModel = leverageRule.objRuleconditions;

                _ruleConditionService.Upsert(ruleConditionViewModel);
            }

            return Json(new { StatusCode = 200, Message = "Saved Successfully" });
        }
        [HttpGet]


        public IActionResult RemoveRule(int Id)
        {
            var res = _leverageRulesService.RemoveRule(Id);
            return Json(res);
        }
    
        [HttpPost]
        public async Task<PartialViewResult> Edit(int id)
        {
            var instruments = await _bdmService.Get_All_Instrument();
            ViewBag.Instruments = instruments;
            var res = await _leverageRulesService.Edit(id);
            return PartialView("Edit", res);
        }
        public JsonResult SaveLeverage(LeverageRuleImport leverageRuleImport)
        {
            var response = _leverageRulesService.ImportLevarageRule(leverageRuleImport);
            var res = _leverageRulesService.ImportLevarageBand(leverageRuleImport.levarageBand);
            return Json(true);
        }
        public IActionResult ExportLevarage(int LeverageRuleId = 0)
        {
            PageParam pageParam = new PageParam();
            pageParam.Offset = 0;
            pageParam.Limit = 1000000;
            List<LeverageRuleImport> leverageRuleImports = new List<LeverageRuleImport>();
            var modelExport =  _leverageRulesService.GetLeverageRuleImports(pageParam, LeverageRuleId, "", 0);
            leverageRuleImports = modelExport.Item1;
            if (leverageRuleImports != null)
            {
                StringBuilder stringBuilder = new StringBuilder();

                string contextType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                string fileName = "LevarageRuleImport_" + DateTime.Now.ToString("MM/dd/yyyy") + ".xlsx";
                int RoWIndex = 1;
                using (var workbook = new XLWorkbook())
                {

                    var worksheet = workbook.Worksheets.Add("Sheet1");
                    //worksheet.Range("A1:D1").Value = "Trade Rule";
                    //RoWIndex++;
                    worksheet.Cell(RoWIndex, 1).Value = "InstrumentName";
                    worksheet.Cell(RoWIndex, 2).Value = "Symbol Group";
                    worksheet.Cell(RoWIndex, 3).Value = "Days";
                    worksheet.Cell(RoWIndex, 4).Value = "Trade Time From";
                    worksheet.Cell(RoWIndex, 5).Value = "Trade Time To";
                    worksheet.Cell(RoWIndex, 6).Value = "Priority";
                    worksheet.Cell(RoWIndex, 7).Value = "BandFrom";
                    worksheet.Cell(RoWIndex, 8).Value = "BandTo";
                    worksheet.Cell(RoWIndex, 9).Value = "Levarage";
                    worksheet.Cell(RoWIndex, 10).Value = "New Positions";
                    worksheet.Cell(RoWIndex, 11).Value = "Legal Entity";


                    //worksheet.Row(2).Style.Fill.SetBackgroundColor(XLColor.Yellow);
                    worksheet.Range("A" + RoWIndex + ":K" + RoWIndex).Style.Fill.SetBackgroundColor(XLColor.Yellow);
                    worksheet.Range("A" + RoWIndex + ":K" + RoWIndex).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin);
                    worksheet.Range("A" + RoWIndex + ":K" + RoWIndex).Style.Border.SetTopBorder(XLBorderStyleValues.Thin);
                    worksheet.Range("A" + RoWIndex + ":K" + RoWIndex).Style.Border.SetRightBorder(XLBorderStyleValues.Thin);
                    worksheet.Range("A" + RoWIndex + ":K" + RoWIndex).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin);

                    RoWIndex++;
                    for (int i = 0; i < leverageRuleImports.Count; i++)
                    {
                        var rangecount = leverageRuleImports.Count.ToString();
                        //worksheet.Column(3).Style.NumberFormat.SetFormat("Number");
                        //worksheet.Column(10).Style.NumberFormat.SetFormat("Number");
                        //worksheet.Column(11).Style.NumberFormat.SetFormat("Number");

                        worksheet.Cell(RoWIndex, 1).Value = leverageRuleImports[i].MasterInstrumentName;
                        worksheet.Cell(RoWIndex, 2).Value = leverageRuleImports[i].SymbolGroupName;
                        worksheet.Cell(RoWIndex, 3).Value = leverageRuleImports[i].Day;
                        worksheet.Range(string.Concat("C1:C", rangecount)).Style.NumberFormat.Format = "0";
                        worksheet.Range(string.Concat("J1:J", rangecount)).Style.NumberFormat.Format = "0";
                        worksheet.Range(string.Concat("F1:J", rangecount)).Style.NumberFormat.Format = "0";
                        worksheet.Range(string.Concat("G1:J", rangecount)).Style.NumberFormat.Format = "0";
                        worksheet.Range(string.Concat("H1:J", rangecount)).Style.NumberFormat.Format = "0";
                        worksheet.Cell(RoWIndex, 4).Value = leverageRuleImports[i].TimeFrom;
                        worksheet.Cell(RoWIndex, 5).Value = leverageRuleImports[i].TimeTo;
                        worksheet.Cell(RoWIndex, 6).Value = leverageRuleImports[i].Priority2;
                        worksheet.Cell(RoWIndex, 7).Value = leverageRuleImports[i].BandFrom;
                        worksheet.Cell(RoWIndex, 8).Value = leverageRuleImports[i].BandTo;
                        worksheet.Cell(RoWIndex, 9).Value = leverageRuleImports[i].LeverageAmount;
                        worksheet.Cell(RoWIndex, 10).Value = leverageRuleImports[i].IsNewPosition == "1" ? "Yes" : "No";
                        worksheet.Cell(RoWIndex, 11).Value = leverageRuleImports[i].LegalEntity;

                        worksheet.Range("A" + RoWIndex + ":K" + RoWIndex).Style.Fill.SetBackgroundColor(XLColor.LightBlue);
                        worksheet.Range("A" + RoWIndex + ":K" + RoWIndex).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin);
                        worksheet.Range("A" + RoWIndex + ":K" + RoWIndex).Style.Border.SetTopBorder(XLBorderStyleValues.Thin);
                        worksheet.Range("A" + RoWIndex + ":K" + RoWIndex).Style.Border.SetRightBorder(XLBorderStyleValues.Thin);
                        worksheet.Range("A" + RoWIndex + ":K" + RoWIndex).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin);
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
            else
            {
                return Json("");
            }
        }
        public async Task<IActionResult> DeleteLeverageRuleConditionById(int Id, int RuleConditionId)
        {
            ResponseViewModel responseViewModel = new ResponseViewModel();

            var res = _leverageRulesService.DeleteRuleConditionDtlById(Id, RuleConditionId);
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
        public async Task<JsonResult> GetInstrumentsDropDown()
        {
            List<SelectListItem> oprationList = new List<SelectListItem>();
            var FieldsList = _bdmService.Get_All_Instrument().Result.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name });
            //var country = _countryService.GetAllCountries().Result.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.CountryName });
            foreach (var item in FieldsList)
            {
                oprationList.Add(new SelectListItem { Text = item.Text, Value = item.Value });
            }
            return Json(oprationList);
        }
    }
}
