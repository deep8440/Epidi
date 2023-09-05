using ClosedXML.Excel;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.BDM;
using Epidi.Models.ViewModel.Instrument;
using Epidi.Models.ViewModel.SkipRule;
using Epidi.Models.ViewModel.TradeRule;
using Epidi.Service.BDM;
using Epidi.Service.MathService;
using EPIDIWeb.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using System.Text;

namespace EPIDIWeb.Controllers
{
    public class MathController : Controller
    {
        private readonly IMathService _mathService;
        private readonly IBDMService _BDMService;

        public MathController(IMathService mathService, IBDMService bDMService)
        {
            _mathService = mathService;
            _BDMService = bDMService;
        }
        public IActionResult Index()
        {
            ViewBag.Title = "SkipRule List";
            GetInstruments();
            return View(new BDMViewModel());
        }
        [HttpPost]
        public JsonResult SkipRule_All()
        {
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
                var response = _mathService.GetMath(pageParam, search);

                totalRecord = response.Item2;
                filteredRecord = response.Item2;

                var responseData = OrderByExtension.OrderBy(response.Item1.AsQueryable(), sortColumn, sortColumnDirection).ToList();

                return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = responseData });
            }
        }
        [HttpPost]
        public async Task<JsonResult> AddMath(SkipRuleModelView skipRule)
        {
            skipRule.CreatedAt = DateTime.Now;
            var response = await _mathService.AddMath(skipRule);
            return Json(response);
        }
        [HttpPost]
        public async Task<JsonResult> EditMath(SkipRuleModelView skipRule)
        {
            var response = await _mathService.EditMath(skipRule);
            return Json(response);
        }
        [HttpPost]
        public async Task<JsonResult> GetMathById(int Id)
        {
            var response = await _mathService.GetMathById(Id);
            return Json(response);
        }
        [HttpGet]
        public IActionResult RemoveMath(int Id)
        {
            var res = _mathService.RemoveMath(Id);
            return Json(res);
        }


       
        public void GetInstruments()
        {
            var FieldsList = _BDMService.Get_All_Instrument().Result.Select(p => new SelectListItem()
            {
                Value = p.Id.ToString(),
                Text = p.Name
            });

            ViewBag.InstrumentList = FieldsList;
        }

        public async Task<IActionResult> ExportTradeRule()
        {
            try
            {
                PageParam pageParam = new PageParam();
                pageParam.Offset = 0;
                pageParam.Limit = 0;
                string path = "";
                long totalRecord = 0;
                long filteredRecord = 0;

                List<SkipRuleModelView> modelExport = new List<SkipRuleModelView>();
                var result = _mathService.GetMath(pageParam, "");
                modelExport = result.Item1;

                //List<InstrumentMaster> instrumentMasterViewModels = new List<InstrumentMaster>();
                //var result = await _.GetAllInstrument(pageParam, "");
                //instrumentMasterViewModels = result.Item1;

            

                if (modelExport != null)
                {
                    StringBuilder stringBuilder = new StringBuilder();

                    string contextType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    string fileName = "SkipRule_"+DateTime.Now.ToString("MM/dd/yyyy")+".xlsx";
                    int RoWIndex = 1;
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Sheet1");
                        //worksheet.Range("A1:D1").Value = "Trade Rule";
                        //RoWIndex++;
                        worksheet.Cell(RoWIndex, 1).Value = "RuleName";
                        worksheet.Cell(RoWIndex, 2).Value = "InstrumentName";
                        worksheet.Cell(RoWIndex, 3).Value = "Equation";
                        worksheet.Cell(RoWIndex, 4).Value = "IsSkip";
                        worksheet.Cell(RoWIndex, 5).Value = "Value";
                        

                        //worksheet.Row(2).Style.Fill.SetBackgroundColor(XLColor.Yellow);
                        worksheet.Range("A" + RoWIndex + ":AM" + RoWIndex).Style.Fill.SetBackgroundColor(XLColor.Yellow);

                        RoWIndex++;
                        for (int i = 0; i < modelExport.Count; i++)
                        {
                            worksheet.Cell(RoWIndex, 1).Value = modelExport[i].Name;
                            worksheet.Cell(RoWIndex, 2).Value = modelExport[i].MasterInstrumentName;
                            worksheet.Cell(RoWIndex, 3).Value = modelExport[i].Equation;
                            worksheet.Cell(RoWIndex, 4).Value = modelExport[i].IsSkip?"Yes":"No";
                            worksheet.Cell(RoWIndex, 5).Value = modelExport[i].Value;
                            
                            
                            worksheet.Range("L" + RoWIndex + ":AM" + RoWIndex).Style.Fill.SetBackgroundColor(XLColor.LightBlue);
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
                return Json(new { StatusCode = 200, Message = "Skip rule export Successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { StatusCode = 400, Message = ex.Message.ToString() });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadSkipRuleExcel(IFormFile FileUpload)
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

                            List<SkipRuleImportModelView> gateIOInstrumentMappingViewModels = new List<SkipRuleImportModelView>();

                            int id = 0;
                            int MasterInstrumentId = 0;
                            string MasterInstrumentIds = "";
                            string Name = "";
                            string Equation = "";
                            double Result = 0;
                            string IsSkip = "No";
                            float Value = 0;
                            
                            DateTime CreatedAt;
                            DateTime DeletedAt;
                            bool IsActive = true;
                            string Rulename = "";
                            string MasterInstrumentName = "";
                            string SymbolGroups = "";

                            for (int rowNo = 1; rowNo < sheet.PhysicalNumberOfRows; rowNo++)
                            {
                                IRow row = sheet.GetRow(rowNo);
                                Rulename= row.GetCell(0) != null ? Convert.ToString(row.GetCell(0)) : "";
                                MasterInstrumentName = row.GetCell(1) != null ? Convert.ToString(row.GetCell(1)) : "";
                                Equation = row.GetCell(2) != null ? row.GetCell(2).ToString() : "";
                                IsSkip = row.GetCell(3) != null ? Convert.ToString(row.GetCell(3)) : "No";
                                Value= row.GetCell(4) != null ? float.Parse(row.GetCell(4).ToString()) : 0;
                                //SymbolGroups = row.GetCell(1) != null ? row.GetCell(1).ToString() : "";
                                
                                //Result = row.GetCell(3) != null ? Convert.ToDouble(row.GetCell(3).NumericCellValue) : 0;
                                

                                SkipRuleImportModelView SkipRulemodel = new SkipRuleImportModelView();
                                SkipRulemodel.Name = Rulename;
                                SkipRulemodel.MasterInstrumentName = MasterInstrumentName;
                                //SkipRulemodel.Name = Equation + " " + Result;
                                //SkipRulemodel.SymbolGroups = SymbolGroups;
                                SkipRulemodel.Equation = Equation;                                
                                SkipRulemodel.IsSkip = IsSkip.ToUpper() == "YES" ? true : false;
                                SkipRulemodel.Value = Value;
                               
                                gateIOInstrumentMappingViewModels.Add(SkipRulemodel);
                            }

                            ListToDataTableConverter converter = new ListToDataTableConverter();
                            DataTable dt = converter.ToDataTable(gateIOInstrumentMappingViewModels);

                            await _mathService.SkipRule_InsertByImport(dt);


                            return Json(new { StatusCode = 200, Message = "File Imported Successfully" });
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

        [HttpPost]
        public async Task<IActionResult> CopyRule(int Id)
        {
            var res = await _mathService.CopyMathRule(Id);
            return Json(res);
        }



    }
}
