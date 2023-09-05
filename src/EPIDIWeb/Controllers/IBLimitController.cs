using DocumentFormat.OpenXml.Wordprocessing;
using Epidi.Models.Paging;
using ClosedXML.Excel;
using Epidi.Service.IBService;
using Microsoft.AspNetCore.Mvc;
using Epidi.Service.IBLimitService;
using Epidi.Models.ViewModel.IB;
using Epidi.Models.ViewModel.Common;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Epidi.Service.InstrumentService;
using Epidi.Models.ViewModel.Instrument;
using Epidi.Models.ViewModel.InstrumentMaster;
using Epidi.Models.ViewModel.Question;
using Epidi.Models.ViewModel.SymbolGroup;
using Epidi.Service.StepsService;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using EPIDIWeb.Extensions;
using Microsoft.EntityFrameworkCore;
using Epidi.Service.BDM;
using System.Text;

using SendGrid.Helpers.Mail;
using Epidi.Models.ViewModel.SkipRule;

namespace EPIDIWeb.Controllers
{
    public class IBLimitController : Controller
    {

        private readonly IBDMService _BDMService;
        private readonly IIBLimitService _IBLimitServices;
        private readonly IInstrumentService _instrumentService;



        public IBLimitController(IIBLimitService iBLimitServices, IInstrumentService instrumentService, IBDMService BDMService)
        {
            _IBLimitServices = iBLimitServices;
            _instrumentService = instrumentService;
            _BDMService = BDMService;

        }


        public IActionResult Index()
        {
            ViewBag.Title = "IBLimit List";

            return View();

        }

        public IActionResult AddIBLimit()
        {
            GetInstruments();
            GetAllSymbolGroupDropDown();

            var model = new IBLimitViewModel();

            return View(model);
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



        public void GetAllSymbolGroupDropDown()
        {
            List<SelectListItem> oprationList = new List<SelectListItem>();
            var FieldsList = _instrumentService.GetAllSymbolGroup().Result.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Group });
            foreach (var item in FieldsList)
            {
                oprationList.Add(new SelectListItem { Text = item.Text, Value = item.Value });
            }
            ViewBag.SymbolList = oprationList;
        }

        public long GetSymbolGroupId(int Id)
        {
            var res = _IBLimitServices.GetSymbolGroupId(Id);
            return res;
        }


        [HttpPost]
        public JsonResult Get_All() 
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
                var response = _IBLimitServices.Get_All(pageParam, search);

                totalRecord = response.Item2;
                filteredRecord = response.Item2;

                var responseData = OrderByExtension.OrderBy(response.Item1.AsQueryable(), sortColumn, sortColumnDirection).ToList();

                return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = responseData });
            }
        }



        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> SaveIBLimit(IBLimitViewModel model)
        {
            try
            {
                ResponseViewModel responseViewModel = new ResponseViewModel();
                if (model != null)
                {
                    var res = await _IBLimitServices.Limit_Upsert(model);


                    if (res != null)
                    {
                        responseViewModel.Code = 200;
                        responseViewModel.Message = "IBLimit Saved Successfully";
                    }
                    else
                    {
                        responseViewModel.Code = 400;
                        responseViewModel.Message = "IBLimit Not Saved";
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
            var res = await _IBLimitServices.GetBy_Id(Id);
            GetInstruments();
            GetAllSymbolGroupDropDown();
            if (res != null)
            {
                return View("EditIBLimit", res);
            }
            else
            {
                return View(new IBLimitViewModel());
            }
        }


        [HttpGet]
        public IActionResult Delete(int Id)
        {
            var res = _IBLimitServices.Delete(Id);
            return Json(res);
        }




        public async Task<IActionResult> ExportIBLimit_Data()
        {
            try
            {
                try
                {
                    ListToDataTableConverter converter = new ListToDataTableConverter();
                    List<IBLimitViewModel> modelExport = new List<IBLimitViewModel>();
                    modelExport = _IBLimitServices.GetIBLimit_Data();
                    if (modelExport != null)
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        string fileName = "IBLimit_" + DateTime.Now.ToString("MM/dd/yyyy") + ".xlsx";
                        int RoWIndex = 1;
                        using (var workbook = new XLWorkbook())
                        {
                            DataTable dtIBLimitData = converter.ToDataTable(modelExport);
                            var worksheet = workbook.Worksheets.Add("IBLimit");

                            //for (int i = 1; i < dtCountryData.Columns.Count; i++)
                            //{
                            //    worksheet.Cell(1, (i + 1)).Value = dtCountryData.Columns[i].ColumnName;
                            //}

                            worksheet.Cell(1, 1).Value = "Id";
                            worksheet.Cell(1, 2).Value = "MasterInstrumentName";
                            worksheet.Cell(1, 3).Value = "SymbolName";
                            worksheet.Cell(1, 4).Value = "MarkUpPer1000";
                            worksheet.Cell(1, 5).Value = "MarkUpPer";
                            worksheet.Cell(1, 6).Value = "BuySwapPer1000";
                            worksheet.Cell(1, 7).Value = "BuySwapPer";
                            worksheet.Cell(1, 8).Value = "SellSwapPer1000";
                            worksheet.Cell(1, 9).Value = "SellSwapPer";

                            RoWIndex++;
                            for (int i = 0; i < modelExport.Count; i++)
                            {
                                //worksheet.Cell(RoWIndex, 1).Value = modelExport[i].CountryId;
                                worksheet.Cell(RoWIndex, 1).Value = modelExport[i].Id;
                                worksheet.Cell(RoWIndex, 2).Value = modelExport[i].MasterInstrumentName;
                                worksheet.Cell(RoWIndex, 3).Value = modelExport[i].SymbolName.Trim();
                                worksheet.Cell(RoWIndex, 4).Value = modelExport[i].MarkUpPer1000;
                                worksheet.Cell(RoWIndex, 5).Value = modelExport[i].MarkUpPer;
                                worksheet.Cell(RoWIndex, 6).Value = modelExport[i].BuySwapPer1000;
                                worksheet.Cell(RoWIndex, 7).Value = modelExport[i].BuySwapPer;
                                worksheet.Cell(RoWIndex, 8).Value = modelExport[i].SellSwapPer1000;
                                worksheet.Cell(RoWIndex, 9).Value = modelExport[i].SellSwapPer;
                                RoWIndex++;
                            }
                            worksheet.Columns().AdjustToContents();
                            using (var stream = new MemoryStream())
                            {
                                workbook.SaveAs(stream);
                                var content = stream.ToArray();
                                return File(content, contentType, fileName);
                            }                            
                        }
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { StatusCode = 400, Message = ex.Message });
                    throw;
                }
                return Json(new { StatusCode = 200, Message = "IBLimit data export successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { StatusCode = 400, Message = ex.Message.ToString() });
            }

        }



        [HttpPost]
        public async Task<IActionResult> UploadIBLimitData(IFormFile FileUpload)
        {
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
                            List<IBLimit_ViewModel> iBLimitViewModel = new List<IBLimit_ViewModel>();

                            int Id=0;
                            int MasterInstrumentId = 0;
                            string MasterInstrumentName="" ;
                            string SymbolName="" ;
                            int MarkUpPer1000 =0;
                            int MarkUpPer =0;
                            int BuySwapPer1000 = 0 ;
                            int BuySwapPer =0;
                            int SellSwapPer1000 = 0 ;
                            int SellSwapPer = 0 ;




                            //var FieldsList = _BDMService.Get_All_Instrument().Result.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name });

                            for (int rowNo = 1; rowNo < sheet.PhysicalNumberOfRows; rowNo++)
                            {
                                IBLimit_ViewModel model = new IBLimit_ViewModel();

                                IRow row = sheet.GetRow(rowNo);

                                    MasterInstrumentName = row.GetCell(1) != null ? Convert.ToString(row.GetCell(1)) : "";
                                    SymbolName = row.GetCell(2) != null ? Convert.ToString(row.GetCell(2)) : "";
                                    MarkUpPer1000 = row.GetCell(3) != null ? Convert.ToInt32(row.GetCell(3).NumericCellValue) : 0;
                                    MarkUpPer = row.GetCell(4) != null ? Convert.ToInt32(row.GetCell(4).NumericCellValue) : 0;
                                    BuySwapPer1000 = row.GetCell(5) != null ? Convert.ToInt32(row.GetCell(5).NumericCellValue) : 0;
                                    BuySwapPer = row.GetCell(6) != null ? Convert.ToInt32(row.GetCell(6).NumericCellValue) : 0;
                                    SellSwapPer1000 = row.GetCell(7) != null ? Convert.ToInt32(row.GetCell(7).NumericCellValue) : 0;
                                    SellSwapPer = row.GetCell(8) != null ? Convert.ToInt32(row.GetCell(8).NumericCellValue) : 0;
                                    model.MasterInstrumentName = MasterInstrumentName;
                                    model.SymbolName = SymbolName;
                                    model.MarkUpPer1000 = MarkUpPer1000;
                                    model.MarkUpPer = MarkUpPer;
                                    model.BuySwapPer1000 = BuySwapPer1000;
                                    model.BuySwapPer = BuySwapPer;
                                    model.SellSwapPer1000 = SellSwapPer1000;
                                    model.SellSwapPer = SellSwapPer;

                                    iBLimitViewModel.Add(model);
                                }
                            ListToDataTableConverter converter = new ListToDataTableConverter();
                            DataTable dt_IBLimitData = converter.ToDataTable(iBLimitViewModel);


                            int result = Convert.ToInt32(await _IBLimitServices.ImportIBLimitData_UpsertByDt(dt_IBLimitData));


                            return Json(new { StatusCode = 200, Message = "File Imported Successfully", Response = result });
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




    }
}


