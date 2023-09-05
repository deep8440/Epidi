using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.CorporateActions;
using Epidi.Models.ViewModel.Instrument;
using Epidi.Service.BDM;
using Epidi.Service.CorporateActionsService;
using Epidi.Service.IBCommisionMarkUpSettingInstrumentWise;
using EPIDIWeb.Extensions;
using MathNet.Numerics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;
using NPOI.HSSF.UserModel;

using NPOI.SS.UserModel;

using NPOI.XSSF.UserModel;
using System.Globalization;
using System.Text;

namespace EPIDIWeb.Controllers
{
    public class CorporateActionsController : Controller
    {
        private readonly ICorporateActionsService _corporateActionsService;

        public CorporateActionsController(ICorporateActionsService corporateActionsService)
        {
            _corporateActionsService = corporateActionsService;
        }
        public IActionResult Index()
        {
            ViewBag.Title = "CorporateActions List";
            return View();
        }

        [HttpPost]
        public IActionResult CorporateActionsView(int no)
        {
            if (no == 1) //Split Tab
            {
                return View("AddSplit1", new SplitViewModel());
            }
            else if (no == 2) // Reverse Split Tab
            {
                return View("AddReverseSplit", new ReverseSplitViewModel());
            }
            else if (no == 3) //Delsting Tab
            {
                return View("AddDelisting", new DelistingViewModel());
            }
            else if (no == 4) //SpintOff/Dividend
            {
                List<SelectListItem> oprationList = new List<SelectListItem>();
                ViewBag.spinOffInstrument = _corporateActionsService.Get_MasterInstrument().Result.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name });

                return View("AddSpinOff", new SpinoffViewModel());
            }
            else if (no == 5) //Devidend in $ tab
            {
                List<SelectListItem> oprationList = new List<SelectListItem>();
                ViewBag.divedendInstrument = _corporateActionsService.Get_MasterInstrument().Result.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name }).ToList();

                return View("AddDividend", new DividendViewModel());
            }
            else if (no == 6) //Rename Tab
            {
                return View("AddRenameOfStock", new RenameOfStockViewModel());
            }
            return View("AddSplit", new SplitViewModel());
        }

        public async Task<JsonResult> GetInstrumentsDropDown()
        {
            List<SelectListItem> oprationList = new List<SelectListItem>();
            var FieldsList = _corporateActionsService.Get_MasterInstrument().Result.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name }).ToList();

            return Json(FieldsList);
        }


        [HttpGet]
        public async Task<JsonResult> GetStockDetailsByInstrumentId(long InstrumentId)
        {

            var response = await _corporateActionsService.GetStockDetails(InstrumentId);

            return Json(response);

        }

        public async Task<IActionResult> SaveNewEquityStockDetails(long OldInstrumentId, long NewInstrumentId, string OrderIds)
        {
            try
            {
                if (OrderIds != null)
                {
                    List<StockDetailsViewModel> response = await _corporateActionsService.SaveNewEquityStockDetails(OldInstrumentId, NewInstrumentId, OrderIds);
                    if (response.Count > 0)
                    {
                        return Json(new { StatusCode = 200, Message = "New equity stock details inserted successfully" });
                    }
                    else
                    {
                        return Json(new { StatusCode = 400, Message = "No data inserted" });
                    }
                }
                else
                {
                    return Json(new { StatusCode = 400, Message = "Please select anyone row" });
                }
                
            }
            catch (Exception ex)
            {
                return Json(new { StatusCode = 400, Message = ex.Message.ToString() });
            }

        }

        public async Task<IActionResult> SaveDelstingComment(string OrderIds)
        {
            try
            {
                
                if (OrderIds != null)
                {

                    List<StockDetailsViewModel> response = await _corporateActionsService.SaveDelstingComment(OrderIds);
                    if (response.Count > 0)
                    {
                        return Json(new { StatusCode = 200, Message = "Delsting comment added succesfully" });
                    }
                    else
                    {
                        return Json(new { StatusCode = 400, Message = "No data inserted" });
                    }
                    return Json(new { StatusCode = 400, Message = "No data inserted" });
                }
                else
                {
                    return Json(new { StatusCode = 400, Message = "Please select anyone row" });
                }

            }
            catch (Exception ex)
            {
                return Json(new { StatusCode = 400, Message = ex.Message.ToString() });
            }

        }

        [HttpGet]
        public async Task<JsonResult> GetCalculatedSplitOrderDetails(SplitViewModel splitViewModel)
        {
            var response = await _corporateActionsService.GetCalculatedSplitOrderDetails(splitViewModel);

            return Json(response);
        }

        [HttpGet]
        public async Task<JsonResult> GetCalculatedDelistingOrderDetails(DelistingViewModel delistingViewModel)
        {
            var response = await _corporateActionsService.GetCalculatedDelistingOrderDetails(delistingViewModel);

            return Json(response);
        }


        [HttpGet]
        public async Task<JsonResult> GetCalculatedReverseSplitOrderDetails(SplitViewModel splitViewModel)
        {
            var decimalValueCount = splitViewModel.MDU;
            var response = await _corporateActionsService.GetCalculatedReverseSplitOrderDetails(splitViewModel);

            foreach (var item in response)
            {
                decimal OpenPriceAfterSplitDecimal = Decimal.Round(Convert.ToDecimal(item.OpenPriceAfterSplit), decimalValueCount);
                

                item.OpenPriceAfterSplit = OpenPriceAfterSplitDecimal.ToString();

                item.SplitAdjustment = "$" + item.SplitAdjustment;


            }
                return Json(response);
        }

        [HttpGet]
        public JsonResult GetCalculatedSpinOffOrderDetails(SpinoffViewModel spinoffViewModel)
        {
            var response = _corporateActionsService.GetCalculatedSpinOffOrderDetails(spinoffViewModel);

            return Json(response);
        }

        [HttpPost]
        public IActionResult GetDivedendDetails(string InstrumentId, string FromDate)
        {
            try
            {
                var response = _corporateActionsService.GetDivedendDetails(InstrumentId, FromDate);
                return View("DividendList",response);
            }
            catch (Exception ex)
            {
                return Json(new { StatusCode = 400, Message = ex.Message.ToString() });
            }

        }

        [HttpPost]
        public IActionResult UpdateDividendDetails(List<DividendDetailsViewModel> response)
        {
            try
            {
                if (response.Count > 0)
                {
                    ListToDataTableConverter converter = new ListToDataTableConverter();
                    System.Data.DataTable dt = converter.ToDataTable(response);
                    dt.Columns.Remove("InstrumentId");
                    dt.Columns.Remove("SymbolGroupName");
                    var res = _corporateActionsService.AddUpdateDividenMaster(dt);
                    return Json(new { StatusCode = 200, Message = "Details updated successfully" });
                }
                else
                {
                    return Json(new { StatusCode = 400, Message = "Not found any order" });
                }

            }
            catch (Exception ex)
            {
                return Json(new { StatusCode = 400, Message = ex.Message.ToString() });
            }

        }


        [HttpPost]
        public async Task<IActionResult> AddUpdateSplitValue(List<AddSplitViewModel> model)
        {
            try
            {
                if (model.Count > 0)
                {
                    ListToDataTableConverter converter = new ListToDataTableConverter();
                    System.Data.DataTable dt = converter.ToDataTable(model);

                    var response = await _corporateActionsService.AddUpdateSplitOrder(dt);

                    return Json(new { StatusCode = 200, Message = "Order updated successfully" });

                }
                else
                {
                    return Json(new { StatusCode = 400, Message = "Not found any order" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { StatusCode = 400, Message = ex.Message.ToString() });
            }

        }

        [HttpPost]
        public async Task<IActionResult> AddUpdateReverseSplitValue(List<AddSplitViewModel> model)
        {
            try
            {
                if (model.Count > 0)
                {
                    ListToDataTableConverter converter = new ListToDataTableConverter();
                    System.Data.DataTable dt = converter.ToDataTable(model);

                    var response = await _corporateActionsService.AddUpdateReverseSplitOrder(dt);

                    return Json(new { StatusCode = 200, Message = "Order updated successfully" });

                }
                else
                {
                    return Json(new { StatusCode = 400, Message = "Not found any order" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { StatusCode = 400, Message = ex.Message.ToString() });
            }

        }

        [HttpPost]
        public async Task<IActionResult> AddUpdateSpinOffValue(List<SpinOffOrderUpdateViewModel> model, string SelectedOrderid)
        {
            try
            {
                if (model.Count > 0)
                {
                    string[] OrderIds = SelectedOrderid.Split(',');
                    ListToDataTableConverter converter = new ListToDataTableConverter();
                    
                    for (int i = 0; i < model.Count; i++)
                    {
                        for (int j = 0; j < OrderIds.Length; j++)
                        {
                            if (model[i].OrderId == Convert.ToInt32(OrderIds[j]) )
                            {
                                List<SpinOffOrderUpdateViewModel> spinoffmodel = new List<SpinOffOrderUpdateViewModel>();
                                spinoffmodel.Add(model[i]);
                                System.Data.DataTable dt = converter.ToDataTable(spinoffmodel);
                                var response = await _corporateActionsService.AddUpdateSpinOffOrder(dt);
                            }
                        }
                    }
                    return Json(new { StatusCode = 200, Message = "Order updated successfully" });
                }
                else
                {
                    return Json(new { StatusCode = 400, Message = "Not found any order" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { StatusCode = 400, Message = ex.Message.ToString() });
            }

        }

        [HttpPost]
        public async Task<IActionResult> UploadDividendExcel(IFormFile FileUpload)
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

                            List<DividendDetailsViewModel> dividendDetailsViewModels = new List<DividendDetailsViewModel>();

                            string InstrumentName;
                            string TimeTo;
                            string TaxApplyBuyLeverageEqulTo1;
                            string TaxApplyBuyLeverageGreaterThen1;
                            string TaxApplySell;
                            decimal DividendPerUnit;
                            decimal DividendPerUnitSell;
                            decimal DividendPerUnitBuyLeverageGreaterThen1;
                            decimal DividendPerUnitBuyLeverageEqualTo1;
                            string ExDividendDate;
                            

                            for (int rowNo = 1; rowNo < sheet.PhysicalNumberOfRows; rowNo++)
                            {
                                IRow row = sheet.GetRow(rowNo);
                                tempRowNo = rowNo;

                                InstrumentName = row.GetCell(0) != null ? row.GetCell(0).ToString() : "";
                                TimeTo = Convert.ToString(row.GetCell(1).DateCellValue.ToString("HH:mm:ss")) != null ? Convert.ToString(row.GetCell(1).DateCellValue.ToString("HH:mm:ss")) : "";
                                TaxApplyBuyLeverageEqulTo1 = row.GetCell(2) != null ? row.GetCell(2).ToString() : "";
                                TaxApplyBuyLeverageGreaterThen1 = row.GetCell(3) != null ? row.GetCell(3).ToString() : "";
                                TaxApplySell = row.GetCell(4) != null ? row.GetCell(4).ToString() : "";
                                DividendPerUnit = row.GetCell(5) != null ? Convert.ToDecimal(row.GetCell(5).NumericCellValue) : 0;
                                DividendPerUnitSell = row.GetCell(6) != null ? Convert.ToDecimal(row.GetCell(5).NumericCellValue) : 0;
                                DividendPerUnitBuyLeverageGreaterThen1 = row.GetCell(7) != null ? Convert.ToDecimal(row.GetCell(7).NumericCellValue) : 0;
                                DividendPerUnitBuyLeverageEqualTo1 = row.GetCell(8) != null ? Convert.ToDecimal(row.GetCell(8).NumericCellValue) : 0;
                                ExDividendDate = Convert.ToString(row.GetCell(9).DateCellValue.ToString("yyyy-MM-dd")) != null ? Convert.ToString(row.GetCell(9).DateCellValue.ToString("yyyy-MM-dd")) : "";


                                DividendDetailsViewModel dividendDetailsView = new DividendDetailsViewModel();

                                
                                dividendDetailsView.InstrumentName = InstrumentName;
                                dividendDetailsView.TimeTo = TimeSpan.Parse(TimeTo) ;
                                dividendDetailsView.TaxApplyBuyLeverageEqulTo1 = TaxApplyBuyLeverageEqulTo1;
                                dividendDetailsView.TaxApplyBuyLeverageGreaterThen1 = TaxApplyBuyLeverageGreaterThen1;
                                dividendDetailsView.TaxApplySell = TaxApplySell;
                                dividendDetailsView.DividendPerUnit = DividendPerUnit;
                                dividendDetailsView.DividendPerUnitSell = DividendPerUnitSell;
                                dividendDetailsView.DividendPerUnitBuyLeverageGreaterThen1 = DividendPerUnitBuyLeverageGreaterThen1;
                                dividendDetailsView.DividendPerUnitBuyLeverageEqualTo1 = DividendPerUnitBuyLeverageEqualTo1;
                                dividendDetailsView.ExDividendDate = ExDividendDate;
                                //dividendDetailsView.ExDividendDate = Convert.ToDateTime("06-06-24 09:54:54 AM");


                                dividendDetailsViewModels.Add(dividendDetailsView);
                            }

                            ListToDataTableConverter converter = new ListToDataTableConverter();
                            System.Data.DataTable dt = converter.ToDataTable(dividendDetailsViewModels);

                            dt.Columns.Remove("InstrumentId");
                            dt.Columns.Remove("SymbolGroupName");
                            var res = await _corporateActionsService.AddUpdateDividenMaster(dt);

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

        [HttpPost]
        public IActionResult ExportDividendDetailsExcel()
        {
            var res = _corporateActionsService.ExportDivedendDetails();


            if (res.Count > 0)
            {
                //return Json(new { draw = requestModel.Draw, recordsFiltered = responseModel.FilteredCount, recordsTotal = responseModel.TotalCount, data = datavalue });
                StringBuilder stringBuilder = new StringBuilder();

                string contextType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                string fileName = "DividendDetails"+"_" + DateTime.Now.ToString("MM/dd/yyyy") + ".xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("DividendDetails");

                    worksheet.Cell(1, 1).Value = "InstrumentName";
                    worksheet.Cell(1, 2).Value = "Time to Apply";
                    worksheet.Cell(1, 3).Value = "Tax Apply Buy (Leverage=1)";
                    worksheet.Cell(1, 4).Value = "Tax Apply Buy (Leverage>1)";
                    worksheet.Cell(1, 5).Value = "Tax Apply Sell";
                    worksheet.Cell(1, 6).Value = "Dividend per Unit, $";
                    worksheet.Cell(1, 7).Value = "Dividend per Unit, Sell";
                    worksheet.Cell(1, 8).Value = "Dividend per Unit, Buy (Leverage>1)";
                    worksheet.Cell(1, 9).Value = "Dividend per Unit, Buy (Leverage=1)";
                    worksheet.Cell(1, 10).Value = "Ex Dividend Date";
                    

                    worksheet.Range("A1:N1").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                    //worksheet.Range("A1:N2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    for (int index = 1; index <= res.Count; index++)
                    {
                        worksheet.Cell(index + 1, 1).Value = res[index - 1].InstrumentName;
                        worksheet.Cell(index + 1, 2).Value = res[index - 1].TimeTo;
                        worksheet.Cell(index + 1, 3).Value = res[index - 1].TaxApplyBuyLeverageEqulTo1;
                        worksheet.Cell(index + 1, 4).Value = res[index - 1].TaxApplyBuyLeverageGreaterThen1;
                        worksheet.Cell(index + 1, 5).Value = res[index - 1].TaxApplySell;
                        worksheet.Cell(index + 1, 6).Value = res[index - 1].DividendPerUnit;
                        worksheet.Cell(index + 1, 7).Value = res[index - 1].DividendPerUnitSell;
                        worksheet.Cell(index + 1, 8).Value = res[index - 1].DividendPerUnitBuyLeverageGreaterThen1;
                        worksheet.Cell(index + 1, 9).Value = res[index - 1].DividendPerUnitBuyLeverageEqualTo1;
                        worksheet.Cell(index + 1, 10).Style.DateFormat.Format = "dd/mm/yyyy";
                        worksheet.Cell(index + 1, 10).Value = Convert.ToDateTime(res[index - 1].ExDividendDate);
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
            return Json("");
        }

        [HttpPost]
        public IActionResult GetDivedendOrderDetails(int InstrumentId, string FromDate,string ToDate)
        {
            try
            {
                var response = _corporateActionsService.GetDivedendOrderDetails(InstrumentId, FromDate, ToDate);
                return PartialView("DividendOrdersDetails", response);
            }
            catch (Exception ex)
            {
                return Json(new { StatusCode = 400, Message = ex.Message.ToString() });
            }

        }
        [HttpPost]
        public IActionResult FilterDivedendOrderDetails(int InstrumentId, string FromDate, string ToDate)
        {
            try
            {
                var response = _corporateActionsService.GetDivedendOrderDetails(InstrumentId, FromDate, ToDate);
                return PartialView("DividendOrdersAppendToTable", response);
            }
            catch (Exception ex)
            {
                return Json(new { StatusCode = 400, Message = ex.Message.ToString() });
            }

        }

        [HttpPost]
        public IActionResult ExportDivedendOrderDetails(int InstrumentId, string FromDate, string ToDate)
        {
            try
            {
                var response = _corporateActionsService.GetDivedendOrderDetails(InstrumentId, FromDate, ToDate);
                if (response.Count > 0)
                {
                    //return Json(new { draw = requestModel.Draw, recordsFiltered = responseModel.FilteredCount, recordsTotal = responseModel.TotalCount, data = datavalue });
                    StringBuilder stringBuilder = new StringBuilder();

                    string contextType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    string fileName = "DividendOrdersDetails" + "_" + DateTime.Now.ToString("MM/dd/yyyy") + ".xlsx";
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("DividendDetails");

                        worksheet.Cell(1, 1).Value = "Client ID";
                        worksheet.Cell(1, 2).Value = "Trading Account";
                        worksheet.Cell(1, 3).Value = "Order ID";
                        worksheet.Cell(1, 4).Value = "Side";
                        worksheet.Cell(1, 5).Value = "Equity Name";
                        worksheet.Cell(1, 6).Value = "Open Date Time";
                        worksheet.Cell(1, 7).Value = "Legal Entity";
                        worksheet.Cell(1, 8).Value = "Leverage";
                        worksheet.Cell(1, 9).Value = "Open Units";
                        worksheet.Cell(1, 10).Value = "Open Price";
                        worksheet.Cell(1, 11).Value = "Dividend per Unit, Sell";
                        worksheet.Cell(1, 12).Value = "Dividend per Unit, Buy (Leverage>1)";
                        worksheet.Cell(1, 13).Value = "Dividend per Unit, Buy (Leverage=1)";
                        worksheet.Cell(1, 14).Value = "Ex Dividend Date";
                        worksheet.Cell(1, 15).Value = "Dividend Integer";
                        worksheet.Cell(1, 16).Value = "Dividend Decimal";
                        worksheet.Cell(1, 17).Value = "Dividend Total";


                        worksheet.Range("A1:Q1").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                        //worksheet.Range("A1:N2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        for (int index = 1; index <= response.Count; index++)
                        {
                            worksheet.Cell(index + 1, 1).Value = response[index - 1].ClientId;
                            worksheet.Cell(index + 1, 2).Value = response[index - 1].TradingAccount;
                            worksheet.Cell(index + 1, 3).Value = response[index - 1].OrderId;
                            worksheet.Cell(index + 1, 4).Value = response[index - 1].SideName;
                            worksheet.Cell(index + 1, 5).Value = response[index - 1].EquityName;
                            worksheet.Cell(index + 1, 6).Value = response[index - 1].OpenDateTiming;
                            worksheet.Cell(index + 1, 7).Value = response[index - 1].LegalEntity;
                            worksheet.Cell(index + 1, 8).Value = response[index - 1].Leverage;
                            worksheet.Cell(index + 1, 9).Value = response[index - 1].OpenUnits;
                            worksheet.Cell(index + 1, 10).Value = response[index - 1].OpenPrice;
                            worksheet.Cell(index + 1, 11).Value = response[index - 1].DividendPerUnitSell;
                            worksheet.Cell(index + 1, 12).Value = response[index - 1].DividendPerUnitBuyLeverageGreaterThen1;
                            worksheet.Cell(index + 1, 13).Value = response[index - 1].DividendPerUnitBuyLeverageEqualTo1;
                            worksheet.Cell(index + 1, 14).Style.DateFormat.Format = "dd/mm/yyyy";
                            worksheet.Cell(index + 1, 14).Value = Convert.ToDateTime(response[index - 1].ExDividendDate);
                            worksheet.Cell(index + 1, 15).Value = response[index - 1].DividendInteger;
                            worksheet.Cell(index + 1, 16).Value = response[index - 1].DividendDecimal;
                            worksheet.Cell(index + 1, 17).Value = response[index - 1].DividendTotal;


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
                return Json("");
            }
            catch (Exception ex)
            {
                return Json(new { StatusCode = 400, Message = ex.Message.ToString() });
            }

        }

    }
}
