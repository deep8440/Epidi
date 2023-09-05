using ClosedXML.Excel;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.Country;
using Epidi.Models.ViewModel.IBCommissionMarkUpSettingInstrumentWise;
using Epidi.Models.ViewModel.Instrument;
using Epidi.Models.ViewModel.TradeRule;
using Epidi.Service.CountryService;
using EPIDIWeb.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using System.Text;

namespace EPIDIWeb.Controllers
{
    public class CountryController : Controller
    {
        private readonly ICountryService _countryService;
        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;
        }
        public IActionResult Index()
        {
            ViewBag.Title = "Country List";
            ViewBag.Method = "Country";
            return View();
        }

        [HttpPost]
        public JsonResult Country_All()
        {
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
                var response = _countryService.GetAllCountriesForAdmin(pageParam, search, sortColumn, sortColumnDirection);

                totalRecord = response.Item2;
                filteredRecord = response.Item2;

                //var responseData = OrderByExtension.OrderBy(response.Item1.AsQueryable(), sortColumn, sortColumnDirection).ToList();


                return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = response.Item1 });
            }
        }
        public IActionResult AddCountry()
        {
            return View(new CountryViewModel());
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> SaveCountry(CountryViewModel model)
        {
            try
            {
                ResponseViewModel responseViewModel = new ResponseViewModel();
                if (model != null)
                {
                    var res = await _countryService.Upsert(model);
                    if (res != null)
                    {
                        responseViewModel.Code = 200;
                        responseViewModel.Message = "Country Saved Successfully";
                    }
                    else
                    {
                        responseViewModel.Code = 400;
                        responseViewModel.Message = "Country Not Saved";
                    }
                    return Json(responseViewModel);
                }
                else
                {
                    return Json("");
                }
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var response = await _countryService.GetById(Id);
            //response.FieldName =    
            if (response != null)
            {
                return View("AddCountry", response);
            }
            else
            {
                return View(new CountryViewModel());
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadCountryData(IFormFile FileUpload)
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
                            List<CountryViewModel> _countryViewModel = new List<CountryViewModel>();

                            string CountryName = "";
                            string CountryCode = "";
                            string NiceName = "";
                            string Iso3 = "";
                            int NumCode = 0;
                            int PhoneCode = 0;
                            int OnboardingSteps = 0;
                            string Nationality = "";
                            string TaxResidence = "";

                            //var FieldsList = _BDMService.Get_All_Instrument().Result.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name });

                            for (int rowNo = 1; rowNo < sheet.PhysicalNumberOfRows; rowNo++)
                            {
                                CountryViewModel model = new CountryViewModel();

                                IRow row = sheet.GetRow(rowNo);

                                CountryCode = row.GetCell(0) != null ? Convert.ToString(row.GetCell(0)) : "";
                                CountryName = row.GetCell(1) != null ? Convert.ToString(row.GetCell(1)) : "";
                                NiceName = row.GetCell(2) != null ? Convert.ToString(row.GetCell(2)) : "";
                                Iso3 = row.GetCell(3) != null ? Convert.ToString(row.GetCell(3)) : "";
                                NumCode = row.GetCell(4) != null ? Convert.ToInt32(row.GetCell(4).NumericCellValue) : 0;
                                PhoneCode = row.GetCell(5) != null ? Convert.ToInt32(row.GetCell(5).NumericCellValue) : 0;
                                OnboardingSteps = row.GetCell(6) != null ? Convert.ToInt32(row.GetCell(6).NumericCellValue) : 0;
                                Nationality = row.GetCell(7) != null ? Convert.ToString(row.GetCell(7)) : "";
                                TaxResidence = row.GetCell(8) != null ? Convert.ToString(row.GetCell(8)) : "";

                                model.CountryCode = CountryCode;
                                model.CountryName = CountryName;
                                model.NiceName = NiceName;
                                model.Iso3 = Iso3;
                                model.NumCode = NumCode;
                                model.PhoneCode = PhoneCode;
                                model.OnboardingSteps = OnboardingSteps;
                                model.Nationality = Nationality;
                                model.TaxResidence = TaxResidence;

                                _countryViewModel.Add(model);
                            }
                            ListToDataTableConverter converter = new ListToDataTableConverter();
                            DataTable dtCountryData = converter.ToDataTable(_countryViewModel);


                            int result = Convert.ToInt32(await _countryService.ImportCountryData_UpsertByDt(dtCountryData));


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
        public async Task<IActionResult> ExportCountryDataOld()
        {
            try
            {

                ListToDataTableConverter converter = new ListToDataTableConverter();
                List<CountryViewModel> modelExport = new List<CountryViewModel>();
                modelExport = await _countryService.GetCountryData();

                DataTable dtCountryData = converter.ToDataTable(modelExport);

                var workbook = new HSSFWorkbook();

                var sheet = workbook.CreateSheet("Sheet1");
                NPOI.SS.UserModel.IRow headerrow = sheet.CreateRow(0);
                ICellStyle style = workbook.CreateCellStyle();


                for (int i = 0; i < dtCountryData.Columns.Count; i++)
                {
                    ICell cell = headerrow.CreateCell(i);
                    cell.CellStyle = style;
                    cell.SetCellValue(dtCountryData.Columns[i].ColumnName);
                }

                //Below loop is fill content
                for (int i = 0; i <= dtCountryData.Rows.Count - 1; i++)
                {
                    var row = (HSSFRow)sheet.CreateRow(i + 1);

                    for (int j = 0; j <= dtCountryData.Columns.Count - 1; j++)
                    {
                        dynamic DgvValue;
                        if (dtCountryData.Columns[j].ToString() == "CountryId" || dtCountryData.Columns[j].ToString() == "NumCode" || dtCountryData.Columns[j].ToString() == "PhoneCode" || dtCountryData.Columns[j].ToString() == "OnboardingSteps")
                        {
                            DgvValue = Convert.ToInt32(dtCountryData.Rows[i][j]);
                        }
                        else
                        {
                            DgvValue = (dtCountryData.Rows[i][j] != null ? Convert.ToString(dtCountryData.Rows[i][j]) : "");
                        }
                        row.CreateCell(j).SetCellValue(DgvValue);
                        sheet.SetColumnWidth(j, 20 * 150);
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

                return Json(new { StatusCode = 200, Message = "Country data export successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { StatusCode = 400, Message = ex.Message.ToString() });
            }
        }

        public async Task<IActionResult> DeleteCountry(int Id)
        {
           
            List<ResponseViewModel> responseViewModel = new List<ResponseViewModel>();
            ResponseViewModel responseViewModel1 = new ResponseViewModel();
            var response = await _countryService.DeleteCountry(Id);
            if (response != null)
            {
                responseViewModel1.Code = 200;
                responseViewModel1.Message = "Country Deleted Successfully";
            }
            else
            {
                responseViewModel1.Code = 400;
                responseViewModel1.Message = "Country Not Deleted";
            }
            responseViewModel.Add(responseViewModel1);
            return Json(responseViewModel);
        }
        public async Task<IActionResult> ExportCountryData()
        {
            try
            { 
            ListToDataTableConverter converter = new ListToDataTableConverter();
            List<CountryViewModel> modelExport = new List<CountryViewModel>();
            modelExport = await _countryService.GetCountryData();
            if (modelExport != null)
            {
                StringBuilder stringBuilder = new StringBuilder();
                string contextType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                string fileName = "Country_"+DateTime.Now.ToString("MM/dd/yyyy")+".xlsx";
                int RoWIndex = 1;
                using (var workbook = new XLWorkbook())
                {
                    DataTable dtCountryData = converter.ToDataTable(modelExport);
                    var worksheet = workbook.Worksheets.Add("Country Codexxxxxxx");

                    //for (int i = 1; i < dtCountryData.Columns.Count; i++)
                    //{
                    //    worksheet.Cell(1, (i + 1)).Value = dtCountryData.Columns[i].ColumnName;
                    //}

                        worksheet.Cell(1, 1).Value = "CountryCode";
                        worksheet.Cell(1, 2).Value = "CountryName";
                        worksheet.Cell(1, 3).Value = "NiceName";
                        worksheet.Cell(1, 4).Value = "Iso3";
                        worksheet.Cell(1, 5).Value = "NumCode";
                        worksheet.Cell(1, 6).Value = "PhoneCode";
                        worksheet.Cell(1, 7).Value = "OnboardingSteps";
                        worksheet.Cell(1, 8).Value = "Nationality";
                        worksheet.Cell(1, 9).Value = "TaxResidence";

                        RoWIndex++;
                    for (int i = 0; i < modelExport.Count; i++)
                    {
                        //worksheet.Cell(RoWIndex, 1).Value = modelExport[i].CountryId;
                        worksheet.Cell(RoWIndex, 1).Value = modelExport[i].CountryCode;
                        worksheet.Cell(RoWIndex, 2).Value = modelExport[i].CountryName;
                        worksheet.Cell(RoWIndex, 3).Value = modelExport[i].NiceName;
                        worksheet.Cell(RoWIndex, 4).Value = modelExport[i].Iso3;
                        worksheet.Cell(RoWIndex, 5).Value = modelExport[i].NumCode;
                        worksheet.Cell(RoWIndex, 6).Value = modelExport[i].PhoneCode;
                        worksheet.Cell(RoWIndex, 7).Value = modelExport[i].OnboardingSteps;
                        worksheet.Cell(RoWIndex, 8).Value = modelExport[i].Nationality;
                        worksheet.Cell(RoWIndex, 9).Value = modelExport[i].TaxResidence;
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

    }
}
