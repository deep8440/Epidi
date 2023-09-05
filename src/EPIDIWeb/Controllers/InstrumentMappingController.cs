//using DocumentFormat.OpenXml.Drawing.Charts;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.VariantTypes;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.CommissionRule;
using Epidi.Models.ViewModel.Instrument;
using Epidi.Models.ViewModel.TradeRule;
using Epidi.Service.InstrumentService;
using Epidi.Service.LPWiseInstrumentPriorityService;
using EPIDIWeb.Extensions;
using MathNet.Numerics;
using MathNet.Numerics.Distributions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace EPIDIWeb.Controllers
{
    public class InstrumentMappingController : Controller
    {
        private readonly IInstrumentService _instrumentService;
        private readonly ILPWiseInstrumentPriorityService _iLPWiseInstrumentPriorityService;
        private readonly IInstrumentSpecificationService _instrumentSpecificationService;
        public InstrumentMappingController(IInstrumentService instrumentService, ILPWiseInstrumentPriorityService iLPWiseInstrumentPriorityService,
            IInstrumentSpecificationService instrumentSpecificationService)
        {
            _instrumentService = instrumentService;
            _iLPWiseInstrumentPriorityService = iLPWiseInstrumentPriorityService;
            _instrumentSpecificationService = instrumentSpecificationService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "Instrument Mapping  List";
            ViewBag.IoInstrument = await All_GateIoInstrument();

            ViewBag.LMaxInstrument = await All_LMaxInstrument();
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> All_Instrument() { 
        
            long totalRecord;
            long filteredRecord;
            var Draw = Request.Form["draw"].FirstOrDefault();
            PageParam pageParam = new()
            {
                Offset = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0"),
                Limit = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0")
            };

            string search = Convert.ToString(Request.Form["search[value]"].FirstOrDefault() ?? "");
            var response = await _instrumentService.GetAllInstrument(pageParam, search).ConfigureAwait(false);

            totalRecord = response.Item2;
            filteredRecord = response.Item2;

            return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = response.Item1 });
        }

        [HttpPost]
        public async Task<List<GateIO>> All_GateIoInstrument()
        {
            var response = await _instrumentService.GetGateIOs().ConfigureAwait(false);
            return response.ToList();
        }
        [HttpPost]
        public async Task<List<LMax>> All_LMaxInstrument()
        {
            var response = await _instrumentService.GetLMaxes().ConfigureAwait(false);

            return response.ToList();
        }
        [HttpPost]
        public JsonResult AddInsertMapping([FromBody] List<InstrumentMasterMappingViewModel> instrumentMasterMappings)
        {
            System.Data.DataTable dataTables = new System.Data.DataTable();
            dataTables.Columns.Add("master_id", typeof(int));
            dataTables.Columns.Add("LPName", typeof(string));
            dataTables.Columns.Add("InstrumentId", typeof(int));

            instrumentMasterMappings.ForEach(x => dataTables.Rows.Add(x.master_id, x.lpName, x.instrumentId));
             _instrumentService.AddMultipleMapping(dataTables);

            return Json(new { Code = 200, Message = "Data Saved Successfully" });
        }

        [HttpPost]
        public async Task<IActionResult> UploadInstrumentsMappingExcel(IFormFile FileUpload)
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

                            List<InstrumentMappingViewModel> InstrumentsViewModels = new List<InstrumentMappingViewModel>();

                            int Id;
                            string EpidiInstrumentName;
                            string LP;
                            string LPInstrumentName;
                            string QTFrom;
                            string QTTo;
                            string Days;
                            string TTFrom;
                            string TTTo;
                            long MinValue;                            
                           // string MinValue;
                            decimal Multiplier;
                            string OnOff;
                            decimal ContractSize;
                            string ISIN;

                            
                            for (int rowNo = 1; rowNo < sheet.PhysicalNumberOfRows; rowNo++)
                            {
                                IRow row = sheet.GetRow(rowNo);

                                if (row != null)
                                {
                                    tempRowNo = rowNo;

                                    Id = row.GetCell(0) != null ? Convert.ToInt32(row.GetCell(0).ToString()) : 0;
                                    EpidiInstrumentName = row.GetCell(1) != null ? row.GetCell(1).ToString() : "";
                                    LP = row.GetCell(2) != null ? row.GetCell(2).ToString() : "";
                                    LPInstrumentName = row.GetCell(3) != null ? row.GetCell(3).ToString() : "";
                                    QTFrom = Convert.ToString(row.GetCell(4).DateCellValue.ToString("HH:mm:ss")) != null ? Convert.ToString(row.GetCell(4).DateCellValue.ToString("HH:mm:ss")) : "";
                                    QTTo = Convert.ToString(row.GetCell(5).DateCellValue.ToString("HH:mm:ss")) != null ? Convert.ToString(row.GetCell(5).DateCellValue.ToString("HH:mm:ss")) : "";

                                    TTFrom = Convert.ToString(row.GetCell(6).DateCellValue.ToString("HH:mm:ss")) != null ? Convert.ToString(row.GetCell(6).DateCellValue.ToString("HH:mm:ss")) : "";
                                    TTTo = Convert.ToString(row.GetCell(7).DateCellValue.ToString("HH:mm:ss")) != null ? Convert.ToString(row.GetCell(7).DateCellValue.ToString("HH:mm:ss")) : "";

                                    MinValue = (!String.IsNullOrEmpty(row.GetCell(8).ToString()) ? Convert.ToInt64(row.GetCell(8).ToString()) : 0);
                                    Multiplier = (!String.IsNullOrEmpty(row.GetCell(9).ToString()) ? Convert.ToDecimal(row.GetCell(9).ToString()) : 0);

                                    OnOff = row.GetCell(10) != null ? (row.GetCell(10).ToString()) : "";
                                    Days = row.GetCell(11) != null ? row.GetCell(11).ToString() : "";

                                    ICell cell = row.GetCell(12, MissingCellPolicy.CREATE_NULL_AS_BLANK);

                                    ContractSize = (!String.IsNullOrEmpty(cell.ToString()) ? Convert.ToDecimal(cell.ToString()) : 0);
                                    ISIN = row.GetCell(13) != null ? row.GetCell(13).ToString() : "";

                                    InstrumentMappingViewModel instrumentMappingViewModel = new InstrumentMappingViewModel();

                                    instrumentMappingViewModel.Id = Id;
                                    instrumentMappingViewModel.EpidiInstrumentName = EpidiInstrumentName;
                                    instrumentMappingViewModel.LP = LP;
                                    instrumentMappingViewModel.LPInstrumentName = LPInstrumentName;
                                    instrumentMappingViewModel.QTFrom = QTFrom;
                                    instrumentMappingViewModel.QTTo = QTTo;
                                    instrumentMappingViewModel.Days = Days;
                                    instrumentMappingViewModel.TTFrom = TTFrom;
                                    instrumentMappingViewModel.TTTo = TTTo;
                                    instrumentMappingViewModel.MinValue = MinValue;
                                    instrumentMappingViewModel.Multiplier = Multiplier;
                                    instrumentMappingViewModel.OnOff = OnOff;
                                    instrumentMappingViewModel.ContractSize = ContractSize;
                                    instrumentMappingViewModel.ISIN = ISIN;


                                    InstrumentsViewModels.Add(instrumentMappingViewModel);
                                }
                            }

                            ListToDataTableConverter converter = new ListToDataTableConverter();
                            System.Data.DataTable dt = converter.ToDataTable(InstrumentsViewModels);

                            await _instrumentService.AddInstrumentMapping(dt);


                            return Json(new { StatusCode = 200, Message = "File Imported Successfully" });
                        }
                        catch (Exception ex)
                        {
                            return Json(new { StatusCode = 400, Message = ex.Message.ToString()+",Line No: "+ tempRowNo });
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

        [HttpPost]
        public async Task<IActionResult> GetInstrumetntMappingDD(int MasterInstrumentId, string LPName)
        {

            var res = await _instrumentService.GetInstrument_DD_Data(MasterInstrumentId, LPName);

            return Json(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLP()
        {

            var res = await _iLPWiseInstrumentPriorityService.Get_All_LP();

            return Json(res);
        }

        [HttpPost]
        public async Task<IActionResult> OldExportInstrumentMapping()
        {
            try
            {
                PageParam pageParam = new PageParam();
                pageParam.Offset = 0;
                pageParam.Limit = 100;
                List<InstrumentSpecificationViewModel> instrumentSpecificationViewModels = new List<InstrumentSpecificationViewModel>();
                var result = await _instrumentSpecificationService.GetAllnstrumentSpecification(pageParam, "");
                instrumentSpecificationViewModels = result.Item1;
                if (instrumentSpecificationViewModels != null)
                {
                    ListToDataTableConverter listToDataTableConverter = new ListToDataTableConverter();
                    var dataTable = listToDataTableConverter.ToDataTable(instrumentSpecificationViewModels);


                    dataTable.Columns.RemoveAt(1);
                    dataTable.Columns.RemoveAt(1);
                    dataTable.Columns.RemoveAt(1);
                    dataTable.Columns.RemoveAt(1);
                    dataTable.Columns.RemoveAt(9);

                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        dataTable.Rows[i][0] = i + 1;
                    }

                    string newFilename = Guid.NewGuid() + ".xlsx";
                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "export");
                    var filePath = Path.Combine(folderPath, newFilename.ToString());


                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var workBook = wb.Worksheets.Add(dataTable, "Sheet1");
                        wb.SaveAs(filePath);
                    }
                    return Json(new { Data = filePath, StatusCode = 200, Message = "Export Instrument Mapping Is Successfully" });
                }
                return Json(new { Data = "", StatusCode = 404, Message = "Data Not Found" });
            }
            catch (Exception ex)
            {
                return Json(new { StatusCode = 400, Message = ex.Message.ToString() });
            }
        }

        //public async Task<IActionResult> ExportInstrumentMapping()
        //{
        //    PageParam pageParam = new PageParam()
        //    {
        //        Offset = 0,
        //        Limit = 1000000000
        //    };
        //    var responseData = await _instrumentSpecificationService.GetAllnstrumentSpecification(pageParam,"").ConfigureAwait(false);
        //    var response = responseData.Item1.ToList();
        //    if (response != null)
        //    {
        //        string contextType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        string fileName = "InstrumentMapping_" + DateTime.Now.ToString("MM/dd/yyyy") +".xlsx";
        //        using (var workbook = new XLWorkbook())
        //        {
        //            var worksheet = workbook.Worksheets.Add("InstrumentMapping");

        //            worksheet.Cell(1, 1).Value = "Id";
        //            worksheet.Cell(1, 2).Value = "MasterInstrumentName";
        //            worksheet.Cell(1, 3).Value = "LPName";
        //            worksheet.Cell(1, 4).Value = "LPInstrumentName";
        //            worksheet.Cell(1, 5).Value = "QTFrom";
        //            worksheet.Cell(1, 6).Value = "QTTo";
        //            worksheet.Cell(1, 7).Value = "TTFrom";
        //            worksheet.Cell(1, 8).Value = "TTTo";
        //            worksheet.Cell(1, 9).Value = "MinValue";
        //            worksheet.Cell(1, 10).Value = "Multiplier";
        //            worksheet.Cell(1, 11).Value = "OnOff";
        //            worksheet.Cell(1, 12).Value = "TradeDays";
        //            worksheet.Cell(1, 13).Value = "ContractSize";
        //            worksheet.Cell(1, 14).Value = "ISIN";

        //            worksheet.Range("A1:N1").Style.Fill.SetBackgroundColor(XLColor.Yellow);

        //            //worksheet.Range("A1:N2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

        //            for (int index = 1; index <= response.Count; index++)
        //            {
        //                worksheet.Cell(index + 1, 1).Style.NumberFormat.SetFormat("0");
        //                worksheet.Cell(index + 1, 1).Value = index;
        //                worksheet.Cell(index + 1, 2).Value = response[index - 1].MasterInstrumentName;
        //                worksheet.Cell(index + 1, 3).Value = response[index - 1].LPName;
        //                worksheet.Cell(index + 1, 4).Value = response[index - 1].LPInstrumentName;
        //                worksheet.Cell(index + 1, 5).Value = response[index - 1].QTFrom;
        //                worksheet.Cell(index + 1, 6).Value = response[index - 1].QTTo;
        //                worksheet.Cell(index + 1, 7).Value = response[index - 1].TTFrom;
        //                worksheet.Cell(index + 1, 8).Value = response[index - 1].TTTo;
        //                worksheet.Cell(index + 1, 9).Value = response[index - 1].MinValue;
        //                worksheet.Cell(index + 1, 10).Value = response[index - 1].Multiplier;
        //                worksheet.Cell(index + 1, 11).Value = response[index - 1].OnOff;
        //                worksheet.Cell(index + 1, 12).Value = response[index - 1].TradeDays;
        //                worksheet.Cell(index + 1, 13).Value = response[index - 1].ContractSize;
        //                worksheet.Cell(index + 1, 14).Value = response[index - 1].ISIN;
        //            }

        //            worksheet.Columns().AdjustToContents();
        //            using (var stream = new MemoryStream())
        //            {
        //                workbook.SaveAs(stream);
        //                var content = stream.ToArray();
        //                return File(content, contextType, fileName);
        //            }
        //        }
        //    }
        //    return Json("");
        //}


        public async Task<IActionResult> ExportInstrumentMapping()
        {
            //for (int i = 0; i < 100; i++)
            //{
            //    Thread.Sleep(100);
            //}
            //return Json(new { Data = "", StatusCode = 200, Message = "Export Instrument Mapping Is Successfully" });
            PageParam pageParam = new PageParam();
            pageParam.Offset = 0;
            //pageParam.Limit = 1000000;
            pageParam.Limit = 10000;
            List<InstrumentSpecificationViewModel> instrumentSpecificationViewModels = new List<InstrumentSpecificationViewModel>();
            var result = await _instrumentSpecificationService.GetAllnstrumentSpecification(pageParam, "");
            instrumentSpecificationViewModels = result.Item1;

            if (instrumentSpecificationViewModels != null)
            {
                //return Json(new { draw = requestModel.Draw, recordsFiltered = responseModel.FilteredCount, recordsTotal = responseModel.TotalCount, data = datavalue });
                StringBuilder stringBuilder = new StringBuilder();

                string contextType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                string fileName = "Instrument_Mapping_" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "_" + instrumentSpecificationViewModels.Count + ".xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("InstrumentMapping");
                    ////Merge and Center
                    //worksheet.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    //worksheet.Cell("A1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    //worksheet.Range("A1:N1").Merge().Value = "POI Report";
                    //worksheet.Range("A1:N2").Style.Font.Bold = true;

                    worksheet.Cell(1, 1).Value = "Id";
                    worksheet.Cell(1, 2).Value = "EpidiInstrumentName";
                    worksheet.Cell(1, 3).Value = "LPName";
                    worksheet.Cell(1, 4).Value = "LPInstrumentName";
                    worksheet.Cell(1, 5).Value = "QTFrom";
                    worksheet.Cell(1, 6).Value = "QTTo";
                    worksheet.Cell(1, 7).Value = "TTFrom";
                    worksheet.Cell(1, 8).Value = "TTTo";
                    worksheet.Cell(1, 9).Value = "MinValue";
                    worksheet.Cell(1, 10).Value = "Multiplier";
                    worksheet.Cell(1, 11).Value = "OnOff";
                    worksheet.Cell(1, 12).Value = "TradeDays";
                    worksheet.Cell(1, 13).Value = "ContractSize";
                    worksheet.Cell(1, 14).Value = "ISIN";

                    worksheet.Range("A1:N1").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                    //worksheet.Range("A1:N2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    int index = 1;
                    foreach (var viewModel in instrumentSpecificationViewModels)

                       // for (int index = 1; index <= instrumentSpecificationViewModels.Count; index++)
                    //for (int index = 1; index <= 5; index++)
                    {
                        worksheet.Cell(index + 1, 1).Style.NumberFormat.SetFormat("0");
                        worksheet.Cell(index + 1, 1).Value = index;
                        worksheet.Cell(index + 1, 2).Value = instrumentSpecificationViewModels[index - 1].MasterInstrumentName;
                        worksheet.Cell(index + 1, 3).Value = instrumentSpecificationViewModels[index - 1].LPName;
                        worksheet.Cell(index + 1, 4).Value = instrumentSpecificationViewModels[index - 1].LPInstrumentName;
                        worksheet.Cell(index + 1, 5).Value = instrumentSpecificationViewModels[index - 1].QTFrom.ToString();
                        worksheet.Cell(index + 1, 6).Value = instrumentSpecificationViewModels[index - 1].QTTo.ToString();
                        worksheet.Cell(index + 1, 7).Value = instrumentSpecificationViewModels[index - 1].TTFrom.ToString();
                        worksheet.Cell(index + 1, 8).Value = instrumentSpecificationViewModels[index - 1].TTTo.ToString();
                        worksheet.Cell(index + 1, 9).Style.NumberFormat.SetFormat("0");
                        worksheet.Cell(index + 1, 9).Value = instrumentSpecificationViewModels[index - 1].MinValue;
                        worksheet.Cell(index + 1, 10).Style.NumberFormat.SetFormat("0.00");
                        worksheet.Cell(index + 1, 10).Value = instrumentSpecificationViewModels[index - 1].Multiplier;
                        worksheet.Cell(index + 1, 11).Value = instrumentSpecificationViewModels[index - 1].OnOff;
                        worksheet.Cell(index + 1, 12).Value = instrumentSpecificationViewModels[index - 1].TradeDays;
                        worksheet.Cell(index + 1, 13).Style.NumberFormat.SetFormat("0.00");
                        worksheet.Cell(index + 1, 13).Value = instrumentSpecificationViewModels[index - 1].ContractSize;
                        worksheet.Cell(index + 1, 14).Value = instrumentSpecificationViewModels[index - 1].ISIN;
                        index++;
                    }

                    //var kcount = (datavalue.Count + 3);
                    //var total = (datavalue.Count + 2);
                    //string A = "A" + kcount;
                    //worksheet.Cell(A).Value = "Total";
                    //worksheet.Cell(A).Style.Font.Bold = true;

                    //string J = "J" + kcount;
                    //worksheet.Cell(J).FormulaA1 = "SUM(J3:J" + total + ")";
                    //worksheet.Cell(J).Style.Font.Bold = true;

                    ////string K = "K" + kcount;
                    ////worksheet.Cell(K).FormulaA1 = "SUM(K3:K" + total + ")";
                    ////worksheet.Cell(K).Style.Font.Bold = true;

                    ////string L = "L" + kcount;
                    ////worksheet.Cell(L).FormulaA1 = "SUM(L3:L" + total + ")";
                    ////worksheet.Cell(L).Style.Font.Bold = true;

                    ////string M = "M" + kcount;
                    ////worksheet.Cell(M).FormulaA1 = "SUM(M3:M" + total + ")";
                    ////worksheet.Cell(M).Style.Font.Bold = true;

                    //string N = "N" + kcount;
                    //worksheet.Cell(N).FormulaA1 = "SUM(N3:N" + total + ")";
                    //worksheet.Cell(N).Style.Font.Bold = true;

                    //string O = "O" + kcount;
                    //worksheet.Cell(O).FormulaA1 = "SUM(O3:O" + total + ")";
                    //worksheet.Cell(O).Style.Font.Bold = true;

                    //string P = "P" + kcount;
                    //worksheet.Cell(P).FormulaA1 = "SUM(P3:P" + total + ")";
                    //worksheet.Cell(P).Style.Font.Bold = true;

                    //var totalborder = (datavalue.Count + 2);
                    //string range = "A1:N" + totalborder;
                    //worksheet.Range(range).Style.Border.InsideBorder = XLBorderStyleValues.Medium;
                    //worksheet.Range(range).Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                    ////point pachhi ni value
                    //string formateJ = "J1:J" + totalborder;
                    //string formateN = "N1:N" + totalborder;
                    //string formateO = "O1:O" + totalborder;
                    //string formateP = "P1:P" + totalborder;
                    //worksheet.Range(formateJ).Style.NumberFormat.Format = "##.00";
                    //worksheet.Range(formateN).Style.NumberFormat.Format = "##.00";
                    //worksheet.Range(formateO).Style.NumberFormat.Format = "##.00";
                    //worksheet.Range(formateP).Style.NumberFormat.Format = "##.00";
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
