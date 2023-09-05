using ClosedXML.Excel;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Instrument;
using Epidi.Models.ViewModel.Margin;
using Epidi.Service.MarginService;
using EPIDIWeb.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SendGrid;
using System.Text;

namespace EPIDIWeb.Controllers
{
    public class MarginController : Controller
    {
        private readonly IMarginService _marginService;
        public MarginController(IMarginService marginService)
        {
            _marginService = marginService;
        }
        public IActionResult Index()
        {
            ViewBag.Title = "Margin List ";
            return View();
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
            string search = Convert.ToString(Request.Form["search[value]"].FirstOrDefault() ?? "");
            var response = _marginService.Get_All(pageParam, search);

            totalRecord = response.Item2;
            filteredRecord = response.Item2;

            return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = response.Item1 });
        }
        public async Task<JsonResult> GetMarginTypes()
        {
            var res = await _marginService.GetMarginTypes();
            return Json(res);
        }

        [HttpPost]
        public async Task<JsonResult> GetInstrumentToConvertTypes()
        {
            var res = await _marginService.GetInstrumentToConvertTypes();
            return Json(res);
        }
        public IActionResult  DeleteMargin(int ID)
        {
            var res =  _marginService.DeleteMargin(ID);
            return Json(res);
        }
        public async Task<JsonResult> Margin_Upsert(MarginViewModel marginViewModel)
        {
            try
            {
                marginViewModel.marginDetailsList = JsonConvert.DeserializeObject<List<MarginDetails>>(marginViewModel.conversionDataStr);

                var res = await _marginService.Margin_Upsert(marginViewModel);
                await _marginService.MarginDetails_Upsert(marginViewModel.marginDetailsList, res.Id);
                return Json(res);
            }
            catch (Exception ex)
            {
                throw;
            }            
        }
        public async Task<JsonResult> GetInstruments()
        {
            var res = await _marginService.GetInstruments();
            return Json(res);
        }
        public async Task<JsonResult> GetSymbolGroups()
        {
            var res = await _marginService.GetSymbolGroups();
            return Json(res);
        }
        [HttpGet]
        public async Task<JsonResult> GetMarginDetails_All(int MarginId)
        {
            var response = await _marginService.GetMarginDetails_All(MarginId);
            return Json(response);
        }


        [HttpPost]
        public async Task<IActionResult> UploadMarginExcel(IFormFile FileUpload)
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
                        int tempRowNo = 0;
                        try
                        {
                            IRow HeaderRow = sheet.GetRow(0);

                            List<ImportMarginViewModel> importMarginViewModels = new List<ImportMarginViewModel>();

                            int Id;
                            string MarginType;
                            string InstrumentEnd;
                            string MasterInstrumentName;
                            string SymbolGroup;
                            string ConversionInstrumentName;
                            decimal ManualConversion;
                            string InstrumentToConvert;

                            for (int rowNo = 1; rowNo < sheet.PhysicalNumberOfRows; rowNo++)
                            {
                                IRow row = sheet.GetRow(rowNo);
                                tempRowNo = rowNo;

                                Id = row.GetCell(0) != null ? Convert.ToInt32(row.GetCell(0).NumericCellValue) : 0;
                                MarginType = row.GetCell(1) != null ? row.GetCell(1).ToString() : "";
                                InstrumentEnd = row.GetCell(2) != null ? row.GetCell(2).ToString() : "";
                                MasterInstrumentName = row.GetCell(3) != null ? row.GetCell(3).ToString() : "";
                                SymbolGroup = row.GetCell(4) != null ? row.GetCell(4).ToString() : "";
                                ConversionInstrumentName = row.GetCell(5) != null ? row.GetCell(5).ToString() : "";

                                ManualConversion = row.GetCell(6) != null ? Convert.ToDecimal(row.GetCell(6).ToString()) : 0;
                                InstrumentToConvert = row.GetCell(7) != null ? row.GetCell(7).ToString() : "";

                                
                                ImportMarginViewModel importMarginViewModel = new ImportMarginViewModel();

                                importMarginViewModel.Id = Id;
                                importMarginViewModel.MarginType = MarginType;
                                importMarginViewModel.InstrumentEnd = InstrumentEnd;
                                importMarginViewModel.MasterInstrumentName = MasterInstrumentName;
                                importMarginViewModel.SymbolGroup = SymbolGroup;
                                importMarginViewModel.ConversionInstrumentName = ConversionInstrumentName;
                                importMarginViewModel.ManualConversion = ManualConversion;
                                importMarginViewModel.InstrumentToConvert = InstrumentToConvert;

                                importMarginViewModels.Add(importMarginViewModel);
                            }

                            ListToDataTableConverter converter = new ListToDataTableConverter();
                            System.Data.DataTable dt = converter.ToDataTable(importMarginViewModels);

                            await _marginService.ImportMargin(dt);


                            return Json(new { StatusCode = 200, Message = "File Imported Successfully" });
                        }
                        catch (Exception ex)
                        {
                            return Json(new { StatusCode = 400, Message = ex.Message.ToString() + ",Line No: " + tempRowNo });
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


        public IActionResult ExportMargin()
        {
            PageParam pageParam = new PageParam();
            pageParam.Offset = 0;
            pageParam.Limit = 100;
            List<MarginViewModel> marginViewModels = new List<MarginViewModel>();
            var result =  _marginService.Get_All(pageParam, "");
            marginViewModels = result.Item1;

            if (marginViewModels != null)
            {
                //return Json(new { draw = requestModel.Draw, recordsFiltered = responseModel.FilteredCount, recordsTotal = responseModel.TotalCount, data = datavalue });
                StringBuilder stringBuilder = new StringBuilder();

                string contextType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                string fileName = "Margin_"+DateTime.Now.ToString("MM/dd/yyyy")+".xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("InstrumentMapping");
                    ////Merge and Center
                    //worksheet.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    //worksheet.Cell("A1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    //worksheet.Range("A1:N1").Merge().Value = "POI Report";
                    //worksheet.Range("A1:N2").Style.Font.Bold = true;

                    worksheet.Cell(1, 1).Value = "Id";
                    worksheet.Cell(1, 2).Value = "MrginType";
                    worksheet.Cell(1, 3).Value = "InstrumentEnd";
                    worksheet.Cell(1, 4).Value = "MasterInstrumentName";
                    worksheet.Cell(1, 5).Value = "SymbolGroup";
                    worksheet.Cell(1, 6).Value = "ConversionInstrumentName";
                    worksheet.Cell(1, 7).Value = "ManualConversion";
                    worksheet.Cell(1, 8).Value = "InstrumentToConvert";
                    
                    worksheet.Range("A1:N1").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                    //worksheet.Range("A1:N2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    for (int index = 1; index <= marginViewModels.Count; index++)
                    {
                        worksheet.Cell(index + 1, 1).Style.NumberFormat.SetFormat("0");
                        worksheet.Cell(index + 1, 1).Value = index;
                        worksheet.Cell(index + 1, 2).Value = marginViewModels[index - 1].MarginTypeName;
                        worksheet.Cell(index + 1, 3).Value = marginViewModels[index - 1].InstrumentEnd;
                        worksheet.Cell(index + 1, 4).Value = marginViewModels[index - 1].MasterInstrumentName;
                        worksheet.Cell(index + 1, 5).Value = marginViewModels[index - 1].SymbolGroupName;
                        worksheet.Cell(index + 1, 6).Value = marginViewModels[index - 1].ConversionInstrumentName;
                        worksheet.Cell(index + 1, 7).Value = marginViewModels[index - 1].ManualConversion;
                        worksheet.Cell(index + 1, 8).Value = marginViewModels[index - 1].InstrumentToConvert;
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
    }
}
