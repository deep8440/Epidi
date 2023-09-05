using ClosedXML.Excel;
using DataTables.AspNet.AspNetCore;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Instrument;
using Epidi.Service.InstrumentService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;
using System.Text;

namespace EPIDIWeb.Controllers
{
    public class InstrumentSpecificationController : Controller
    {
        private readonly IInstrumentSpecificationService _instrumentSpecificationService;
        public InstrumentSpecificationController(IInstrumentSpecificationService instrumentSpecificationService)
        {
            _instrumentSpecificationService = instrumentSpecificationService;
        }
        public IActionResult Index()
        {
            ViewBag.Title = "LP Instrument Specification List ";
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> AllInstrumentSpecification()
        {
            try
            {
                long totalRecord;
                long filteredRecord;
                var Draw = Request.Form["draw"].FirstOrDefault();
                PageParam pageParam = new()
                {
                    Offset = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0"),
                    Limit = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0")
                };

                string search = Convert.ToString(Request.Form["search[value]"].FirstOrDefault() ?? "");
                var response = await _instrumentSpecificationService.GetAllnstrumentSpecification(pageParam, search).ConfigureAwait(false);

                totalRecord = response.Item2;
                filteredRecord = response.Item2;

                return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = response.Item1 });

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditInstrumentSpecification(EditInstrumentSpecificationViewModel model)
        {
            try
            {
                if (model != null)
                {
                    GetTradeDays();
                    var response = await _instrumentSpecificationService.GetInstrumentSpecification(model).ConfigureAwait(false);
                    return View("EditInstrumentSpecification", response);
                }
                else
                {
                    return View("EditInstrumentSpecification", new InstrumentSpecificationViewModel());
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost]
        public async Task<JsonResult> UpdateInstrumentSpecification(InstrumentSpecificationViewModel model)
        {
            try
            {
                var response = _instrumentSpecificationService.UpdateInstrumentSpecification(model);

                return Json(response);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void GetTradeDays()
        {
            var tradeDays = new List<SelectListItem>
                    {
                        new SelectListItem{ Value = "1", Text = "Sunday"},
                        new SelectListItem{ Value = "2", Text = "Monday"},
                        new SelectListItem{ Value = "3", Text = "Tuesday"},
                        new SelectListItem{ Value = "4", Text = "Wednesday"},
                        new SelectListItem{ Value = "5", Text = "Thursday"},
                        new SelectListItem{ Value = "6", Text = "Friday"},
                        new SelectListItem{ Value = "7", Text = "Saturday"}
                    };

            ViewBag.DayList = tradeDays;
        }
        public async Task<IActionResult> ExportInstrumentSpecification(int totalRecords)
        {
            PageParam pageParam = new PageParam()
            {
                Offset = 0,
                Limit = totalRecords
            };
            List<InstrumentSpecificationViewModel> instrumentSpecificationViewModels = new List<InstrumentSpecificationViewModel>();
            var responseData = await _instrumentSpecificationService.GetAllnstrumentSpecification(pageParam, "").ConfigureAwait(false);
            //var response = responseData.Item1.ToList();
            instrumentSpecificationViewModels = responseData.Item1;
            if (instrumentSpecificationViewModels != null)
            {
                //List<InstrumentSpecificationViewModel> instrumentSpecificationViewModels = new List<InstrumentSpecificationViewModel>();
                //var result = await _instrumentSpecificationService.GetAllnstrumentSpecification(pageParam, "");
                //instrumentSpecificationViewModels = result.Item1;

                //    if (instrumentSpecificationViewModels != null)
                //    {
                //        //return Json(new { draw = requestModel.Draw, recordsFiltered = responseModel.FilteredCount, recordsTotal = responseModel.TotalCount, data = datavalue });
                //        StringBuilder stringBuilder = new StringBuilder();

                //        string contextType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //        string fileName = "Instrument_Mapping_" + DateTime.Now.ToString("MM/dd/yyyy") + "_" + instrumentSpecificationViewModels.Count + ".xlsx";
                //        using (var workbook = new XLWorkbook())
                //        {
                //            var worksheet = workbook.Worksheets.Add("InstrumentMapping");
                //            ////Merge and Center
                //            //worksheet.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                //            //worksheet.Cell("A1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                //            //worksheet.Range("A1:N1").Merge().Value = "POI Report";
                //            //worksheet.Range("A1:N2").Style.Font.Bold = true;

                //            worksheet.Cell(1, 1).Value = "Instrument Name";
                //            worksheet.Cell(1, 2).Value = "LPName";
                //            worksheet.Cell(1, 3).Value = "LPInstrumentName";
                //            worksheet.Cell(1, 7).Value = "TTFrom";
                //            worksheet.Cell(1, 5).Value = "TTTo";
                //            worksheet.Cell(1, 6).Value = "TradeDays";
                //            worksheet.Cell(1, 7).Value = "QTFrom";
                //            worksheet.Cell(1, 8).Value = "QTTo";                    
                //            worksheet.Cell(1, 9).Value = "MinValue";
                //            worksheet.Cell(1, 10).Value = "ISIN";
                //            worksheet.Cell(1, 11).Value = "ContractSize";

                //            worksheet.Range("A1:N1").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                //            //worksheet.Range("A1:N2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                //            for (int index = 1; index <= instrumentSpecificationViewModels.Count; index++)
                //            //for (int index = 1; index <= 5; index++)
                //            {
                //                worksheet.Cell(index + 1, 1).Value = instrumentSpecificationViewModels[index - 1].MasterInstrumentName;
                //                worksheet.Cell(index + 1, 2).Value = instrumentSpecificationViewModels[index - 1].LPName;
                //                worksheet.Cell(index + 1, 3).Value = instrumentSpecificationViewModels[index - 1].LPInstrumentName;

                //                worksheet.Cell(index + 1, 4).Value = instrumentSpecificationViewModels[index - 1].TTFrom.ToString();
                //                worksheet.Cell(index + 1, 5).Value = instrumentSpecificationViewModels[index - 1].TTTo.ToString();
                //                worksheet.Cell(index + 1, 6).Value = instrumentSpecificationViewModels[index - 1].TradeDays;

                //                worksheet.Cell(index + 1, 7).Value = instrumentSpecificationViewModels[index - 1].QTFrom.ToString();
                //                worksheet.Cell(index + 1, 8).Value = instrumentSpecificationViewModels[index - 1].QTTo.ToString();

                //                worksheet.Cell(index + 1, 9).Style.NumberFormat.SetFormat("0");
                //                worksheet.Cell(index + 1, 9).Value = instrumentSpecificationViewModels[index - 1].MinValue;
                //                worksheet.Cell(index + 1, 10).Value = instrumentSpecificationViewModels[index - 1].ISIN;

                //                worksheet.Cell(index + 1, 11).Style.NumberFormat.SetFormat("0.00");
                //                worksheet.Cell(index + 1, 11).Value = instrumentSpecificationViewModels[index - 1].ContractSize;                        
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
                //    else
                //    {
                //        return Json("");
                //    }
                //}

                //string fileName = "Instrument_Mapping_" + DateTime.Now.ToString("MM/dd/yyyy") + "_" + instrumentSpecificationViewModels.Count + ".xlsx";
                string fileName = "Instrument_Specification_" + "_" + DateTime.Now.ToString("MM/dd/yyyy") + ".xlsx";
                var workbook = new System.Data.DataTable("InstrumentMapping");

                workbook.Columns.Add("Instrument Name", typeof(string));
                workbook.Columns.Add("LPName", typeof(string));
                workbook.Columns.Add("LPInstrumentName", typeof(string));
                workbook.Columns.Add("TTFrom", typeof(string));
                workbook.Columns.Add("TTTo", typeof(string));
                workbook.Columns.Add("TradeDays", typeof(string));
                workbook.Columns.Add("QTFrom", typeof(string));
                workbook.Columns.Add("QTTo", typeof(string));
                workbook.Columns.Add("MinValue", typeof(string));
                workbook.Columns.Add("ISIN", typeof(string));
                workbook.Columns.Add("ContractSize", typeof(string));

                int i = 1;
                foreach (var item in instrumentSpecificationViewModels)
                {
                    workbook.Rows.Add(item.MasterInstrumentName, item.LPName, item.LPInstrumentName, item.TTFrom, item.TTTo, item.TradeDays, item.QTFrom, item.QTTo, item.MinValue, item.ISIN, item.ContractSize);
                    i++;
                }

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(workbook);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }
            }
            return Json("");
        }


        public IActionResult DeleteInstrumentSpecification(int Id)
        {
            var res = _instrumentSpecificationService.DeleteInstrumentSpecification(Id);
            return Json(res);
        }

    }
}
