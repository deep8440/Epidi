using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Epidi.Models.Model;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Country;
using Epidi.Service.CountryService;
using Epidi.Service.InstrumentService;
using Epidi.Service.OrdersService;
using Epidi.Service.UsersService;
using EPIDIWeb.Extensions;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using System.Text;

namespace EPIDIWeb.Controllers
{
    public class Orders : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IInstrumentService _instrumentService;

        private readonly IUsersService _usersService;
        private readonly ICountryService _countryService;
        public Orders(IOrderService orderService, IInstrumentService instrumentService, IUsersService usersService, ICountryService countryService)
        {
            _orderService = orderService;
            _instrumentService = instrumentService;
            _usersService = usersService;
            _countryService = countryService;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "Orders List";
            PageParam pageParam = new PageParam();
            pageParam.Limit = 10000;
            pageParam.Offset = 0;
            var instruments = await _instrumentService.GetAllInstrument(pageParam, "");
            var users = _usersService.Users_GetAll(pageParam, "");
            var countries = await _countryService.GetAllCountries();

            ViewBag.Users = users.Item1;
            ViewBag.Countries = countries;
            ViewBag.Instruments = instruments.Item1;
            return View();
        }
        [HttpPost]
        public IActionResult GetOrders(string search1)
        {
            long totalRecord = 0;
            long filteredRecord = 0;
            var Draw = Request.Form["draw"].FirstOrDefault();
            PageParam pageParam = new PageParam();
            pageParam.Offset = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");

            var searchCriteria = search1.Split(',');
            int MasterInstrumentId = Convert.ToInt32(searchCriteria[0]);
            DateTime? opentiming = string.IsNullOrEmpty(searchCriteria[1]) ? null : Convert.ToDateTime(searchCriteria[1]);
            DateTime? closeTiming = string.IsNullOrEmpty(searchCriteria[2]) ? null : Convert.ToDateTime(searchCriteria[2]);
            int UserId = string.IsNullOrEmpty(searchCriteria[2]) ? 0 : Convert.ToInt32(searchCriteria[3]);
            int CountryId = string.IsNullOrEmpty(searchCriteria[2]) ? 0 : Convert.ToInt32(searchCriteria[4]);
            decimal Price = string.IsNullOrEmpty(searchCriteria[2]) ? 0 : Convert.ToDecimal(searchCriteria[5]);
            decimal SpecificPrice = string.IsNullOrEmpty(searchCriteria[2]) ? 0 : Convert.ToDecimal(searchCriteria[6]);
            string search = Convert.ToString(Request.Form["search[value]"].FirstOrDefault() ?? "");
            pageParam.Limit = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            var response = _orderService.GetOrders(pageParam, MasterInstrumentId, opentiming, closeTiming, UserId, CountryId, Price, SpecificPrice, search);
            totalRecord = response.Item2;
            filteredRecord = response.Item2;
            return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = response.Item1 });

        }
        [HttpPost]
        public IActionResult RemoveOrder(int ID)
        {
            var res = _orderService.removeOrders(ID);
            return Json(res);
        }
        [HttpPost]
        public IActionResult UpdateFields(string FieldName, string FieldValue, int ID)
        {
            var res = _orderService.updateFields(FieldName, FieldValue, ID);
            return Json(res);
        }
        public async Task<IActionResult> ExportOrderData()
        {
            try
            {
                ListToDataTableConverter converter = new ListToDataTableConverter();
                List<Order> modelExport = new List<Order>();
                modelExport = await _orderService.GetAllOrders();
                if (modelExport != null)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    string contextType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    string fileName = "Order_" + DateTime.Now.ToString("MM/dd/yyyy") + ".xlsx";
                    int RoWIndex = 1;
                    using (var workbook = new XLWorkbook())
                    {
                        DataTable dtCountryData = converter.ToDataTable(modelExport);
                        var worksheet = workbook.Worksheets.Add("Orders");

                        //for (int i = 1; i < dtCountryData.Columns.Count; i++)
                        //{
                        //    worksheet.Cell(1, (i + 1)).Value = dtCountryData.Columns[i].ColumnName;
                        //}

                        worksheet.Cell(1, 1).Value = "Order";
                        worksheet.Cell(1, 2).Value = "ClientId";
                        worksheet.Cell(1, 3).Value = "Trading Account";
                        worksheet.Cell(1, 4).Value = "Order Id";
                        worksheet.Cell(1, 5).Value = "Side";
                        worksheet.Cell(1, 6).Value = "Equity Name";
                        worksheet.Cell(1, 7).Value = "Open DateTime";
                        worksheet.Cell(1, 8).Value = "Legal Entity";
                        worksheet.Cell(1, 9).Value = "Open Price";
                        worksheet.Cell(1, 10).Value = "Open Units";
                        worksheet.Cell(1, 11).Value = "SL";
                        worksheet.Cell(1, 12).Value = "TP";
                        worksheet.Cell(1, 13).Value = "Close Price";
                        worksheet.Cell(1, 14).Value = "Swaps";
                        worksheet.Cell(1, 15).Value = "Commision";
                        worksheet.Cell(1, 16).Value = "Profit";
                        worksheet.Cell(1, 17).Value = "Leverage";
                        worksheet.Cell(1, 18).Value = "Comment";

                        RoWIndex++;
                        for (int i = 0; i < modelExport.Count; i++)
                        {
                            //worksheet.Cell(RoWIndex, 1).Value = modelExport[i].CountryId;
                            worksheet.Cell(RoWIndex, 1).Value = modelExport[i].Orders;
                            worksheet.Cell(RoWIndex, 2).Value = modelExport[i].UserId;
                            worksheet.Cell(RoWIndex, 3).Value = modelExport[i].TradingAccount;
                            worksheet.Cell(RoWIndex, 4).Value = modelExport[i].OrderId;
                            worksheet.Cell(RoWIndex, 5).Value = modelExport[i].Side;
                            worksheet.Cell(RoWIndex, 6).Value = modelExport[i].EquityName;
                            worksheet.Cell(RoWIndex, 7).Value = modelExport[i].OpenTiming;
                            worksheet.Cell(RoWIndex, 8).Value = modelExport[i].LegalEntity;
                            worksheet.Cell(RoWIndex, 9).Value = modelExport[i].OpenPrice;
                            worksheet.Cell(RoWIndex, 10).Value = modelExport[i].OpenUnits;
                            worksheet.Cell(RoWIndex, 11).Value = modelExport[i].SL;
                            worksheet.Cell(RoWIndex, 12).Value = modelExport[i].TP;
                            worksheet.Cell(RoWIndex, 13).Value = modelExport[i].ClosePrice;
                            worksheet.Cell(RoWIndex, 14).Value = modelExport[i].Swap;
                            worksheet.Cell(RoWIndex, 15).Value = modelExport[i].Commision;
                            worksheet.Cell(RoWIndex, 16).Value = modelExport[i].Profit;
                            worksheet.Cell(RoWIndex, 17).Value = modelExport[i].Leverage;
                            worksheet.Cell(RoWIndex, 18).Value = modelExport[i].CommentOfPosition;
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
                return Json(new { StatusCode = 200, Message = "Country data export successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { StatusCode = 400, Message = ex.Message.ToString() });
            }

        }
        [HttpPost]
        public async Task<IActionResult> UploadOrderData(IFormFile FileUpload)
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
                            List<Order> _orderViewModel = new List<Order>();

                            //var FieldsList = _BDMService.Get_All_Instrument().Result.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name });

                            for (int rowNo = 1; rowNo < sheet.PhysicalNumberOfRows; rowNo++)
                            {
                                Order model = new Order();

                                IRow row = sheet.GetRow(rowNo);

                                model.Orders = row.GetCell(0) != null ? Convert.ToString(row.GetCell(0)) : "";
                                model.UserId = row.GetCell(1) != null ? Convert.ToInt32(row.GetCell(1).NumericCellValue) : 0;
                                model.TradingAccount = row.GetCell(2) != null ? Convert.ToString(row.GetCell(2)) : "";
                                model.OrderId = row.GetCell(3) != null ? Convert.ToInt32(row.GetCell(3).NumericCellValue) : 0;
                                model.Side = row.GetCell(4) != null ? Convert.ToString(row.GetCell(4)) : "";
                                model.EquityName = row.GetCell(5) != null ? Convert.ToString(row.GetCell(5)) : "";
                                model.OpenTimingstr = Convert.ToString(row.GetCell(6).DateCellValue.ToString("yyyyMMdd HH:mm:ss")) != null ? Convert.ToString(row.GetCell(6).DateCellValue.ToString("yyyyMMdd HH:mm:ss")) : "";
                                model.LegalEntity = row.GetCell(7) != null ? Convert.ToString(row.GetCell(7)) : "";
                                model.OpenPrice = Convert.ToDecimal(string.IsNullOrEmpty(row.GetCell(8).ToString()) ? "0" : row.GetCell(8).ToString());

                                model.OpenUnits = Convert.ToDecimal(string.IsNullOrEmpty(row.GetCell(9).ToString()) ? "0" : row.GetCell(9).ToString());
                                model.SL = Convert.ToDecimal(string.IsNullOrEmpty(row.GetCell(10).ToString()) ? "0" : row.GetCell(10).ToString());
                                model.TP = Convert.ToDecimal(string.IsNullOrEmpty(row.GetCell(11).ToString()) ? "0" : row.GetCell(11).ToString());
                                model.ClosePrice = Convert.ToDecimal(string.IsNullOrEmpty(row.GetCell(12).ToString()) ? "0" : row.GetCell(12).ToString());
                                model.Swap = Convert.ToDecimal(string.IsNullOrEmpty(row.GetCell(13).ToString()) ? "0" : row.GetCell(13).ToString());
                                model.Commision = Convert.ToDecimal(string.IsNullOrEmpty(row.GetCell(14).ToString()) ? "0" : row.GetCell(14).ToString());
                                model.Profit = Convert.ToDecimal(string.IsNullOrEmpty(row.GetCell(15).ToString()) ? "0" : row.GetCell(15).ToString());
                                model.Leverage = Convert.ToDecimal(string.IsNullOrEmpty(row.GetCell(16).ToString()) ? "0" : row.GetCell(16).ToString());
                                model.CommentOfPosition = row.GetCell(17) != null ? Convert.ToString(row.GetCell(17)) : "";

                                _orderViewModel.Add(model);
                            }
                            ListToDataTableConverter converter = new ListToDataTableConverter();
                            DataTable dtOrderData = converter.ToDataTable(_orderViewModel);
                            dtOrderData.Columns.Remove("OpenTiming");

                            int result = Convert.ToInt32(await _orderService.ImportOrderData_UpsertByDt(dtOrderData));


                            return Json(new { StatusCode = 200, Message = "File Imported Successfully", Response = result });
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
        public IActionResult UpdateGridChanges(int OrderId,double Closeprice,double OpenUnits,double OpenPrice)
        {
            try
            {
                var res = _orderService.updateGridFields(OrderId, Closeprice, OpenUnits, OpenPrice);
                return Json(new { StatusCode = 200, Message = "Order data update successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { StatusCode = 400, Message = ex.Message.ToString() });
            }
            
        }
    }
}
