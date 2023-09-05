using Epidi.Models.Paging;
using Epidi.Models.ViewModel.BDM;
using Epidi.Models.ViewModel.BDMCommisionMarkUpSettingInstrumentWise;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.IBCommissionMarkUpSettingInstrumentWise;
using Epidi.Models.ViewModel.TradeRule;
using Epidi.Service.BDM;
using Epidi.Service.IBCommisionMarkUpSettingInstrumentWise;
using EPIDIWeb.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;

namespace EPIDIWeb.Controllers
{
    public class BDMController : Controller
    {
        private readonly IBDMService _BDMService;
        private readonly IIBCommisionMarkUpSettingInstWiseService _iCommissionSetting;
        //private readonly IIn

         public BDMController(IBDMService bDMService, IIBCommisionMarkUpSettingInstWiseService iCommissionSetting)
        {
            _BDMService = bDMService;
            _iCommissionSetting = iCommissionSetting;
        }

        public IActionResult Index()
        {
            ViewBag.Title = "BDM List";
            return View();
        }
        public IActionResult AddBDM()
        {
            GetInstruments();
            return View(new BDMViewModel());
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


                pageParam.Limit = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
                string search = Convert.ToString(Request.Form["search[value]"].FirstOrDefault() ?? "");
                var response = _iCommissionSetting.GetAll_Commission(pageParam, search);

                totalRecord = response.Item2;
                filteredRecord = response.Item2;

                return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = response.Item1 });
            }
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> SaveBDM(BDMViewModel model)
        {
            try
            {
                ResponseViewModel responseViewModel = new ResponseViewModel();
                if (model != null)
                {
                    var res = await _BDMService.Upsert(model);
                    if (res != null)
                    {
                        responseViewModel.Code = 200;
                        responseViewModel.Message = "BDM Saved Successfully";
                    }
                    else
                    {
                        responseViewModel.Code = 400;
                        responseViewModel.Message = "BDM Not Saved";
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
        public void GetInstruments()
        {
            var FieldsList = _BDMService.Get_All_Instrument().Result.Select(p => new SelectListItem()
            {
                Value = p.Id.ToString(),
                Text = p.Name
            });

            ViewBag.InstrumentList = FieldsList;
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var response = await _BDMService.GetById(Id);
            //response.FieldName =    
            GetInstruments();
            if (response != null)
            {
                return View("AddBDM", response);
            }
            else
            {
                return View(new BDMViewModel());
            }
        }
        public async Task<IActionResult> IBCommision(int IBID, string IBName = "")
        {
            //GetInstruments();
            var model = new CommisionMarkUpSetting();
            model.IbId = IBID;
            model.IbName = IBName;
            //if (IBID > 0)
            //{
            //    model.IbId = IBID;
            //    var response = _iCommissionSetting.GetIBMCommision_IBID(IBID);
            //    if (response != null)
            //    {
            //        return View("IBCommision", response);
            //    }
            //    else
            //    {
            //        if (IBName != "")
            //        {
            //            model.IbName = IBName;
            //        }
            //        goto CallBlankModel;
            //    }
            //}
            //else
            //{
            //    goto CallBlankModel;
            //}
            //////goto statement is jump statement
            //CallBlankModel:
            return View("IBCommision", model);
        }


        [HttpPost]
        public async Task<IActionResult> SaveBDMCommision(BDMCommisionMarkUpSettingInstrumentWiseViewModel model)
        {
            try
            {
                ResponseViewModel responseViewModel = new ResponseViewModel();
                if (model != null)
                {
                    var res = await _BDMService.SaveBDMCommision(model);
                    if (res != null)
                    {
                        responseViewModel.Code = 200;
                        responseViewModel.Message = "BDM Saved Successfully";
                    }
                    else
                    {
                        responseViewModel.Code = 400;
                        responseViewModel.Message = "BDM Not Saved";
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
        [HttpPost]
        public async Task<IActionResult> SaveIBInstrumentCommission(CommisionMarkUpSetting model)
        {
            try
            {
                List<CommisionMarkUpSetting> commisionMarkUpSetting = new List<CommisionMarkUpSetting>();
                commisionMarkUpSetting = JsonConvert.DeserializeObject<List<CommisionMarkUpSetting>>(model.objCommissionString);

                ListToDataTableConverter converter = new ListToDataTableConverter();

                DataTable dtRuleInstrument = converter.ToDataTable(commisionMarkUpSetting);
                dtRuleInstrument.Columns.Remove("BdmName");
                dtRuleInstrument.Columns.Remove("IbNo");
                dtRuleInstrument.Columns.Remove("IbName");
                dtRuleInstrument.Columns.Remove("objCommissionString");

                _iCommissionSetting.CommisionMarkUpSetting_UpsertByDt(dtRuleInstrument);
                return Json(new { StatusCode = 200, Message = "IB Commission updated Successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { StatusCode = 400, Message = ex.Message.ToString() });
            }
        }
        [HttpGet]
        public async Task<JsonResult> Get_CommissionInstrumentWise(int Ib_Id, int instrument_filter)
        {
            var response = await _iCommissionSetting.GetIBMCommision_IBID(Ib_Id, instrument_filter);
            return Json(response);
        }

        [HttpPost]
        public async Task<IActionResult> UploadBDMInstrument(IFormFile FileUpload, CommisionMarkUpSetting reqModel)
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
                            List<CommisionMarkUpSetting> _commisionMarkUpSetting = new List<CommisionMarkUpSetting>();

                            int id = 0;
                            int MasterInstrumentalId = 0;
                            double MarkUpPer1000 = 0;
                            int IBID = reqModel.IbId;
                            double MarkUpPer = 0;
                            double BuySwapPer1000 = 0;
                            double BuySwapPer = 0;
                            double SellSwapPer1000 = 0;
                            double SellSwapPer = 0;
                            double BDMID = 0; 

                            var FieldsList = _BDMService.Get_All_Instrument().Result.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name });

                            for (int rowNo = 1; rowNo < sheet.PhysicalNumberOfRows; rowNo++)
                            {
                                CommisionMarkUpSetting model = new CommisionMarkUpSetting();

                                IRow row = sheet.GetRow(rowNo);
                                
                                if (FieldsList.Where(x => x.Text == Convert.ToString(row.GetCell(0))).Any())
                                {
                                    var masterInstrumentId = FieldsList.Where(x => x.Text == Convert.ToString(row.GetCell(0))).FirstOrDefault().Value;
                                    MasterInstrumentalId = masterInstrumentId != null ? Convert.ToInt32(masterInstrumentId) : 0;
                                }
                                else 
                                {
                                    MasterInstrumentalId = 0;
                                }

                                MarkUpPer1000 = row.GetCell(1) != null ? Convert.ToInt32(row.GetCell(1).NumericCellValue) : 0;
                                MarkUpPer = row.GetCell(2) != null ? Convert.ToInt32(row.GetCell(2).NumericCellValue) : 0;
                                BuySwapPer1000 = row.GetCell(3) != null ? Convert.ToInt32(row.GetCell(3).NumericCellValue) : 0;
                                BuySwapPer = row.GetCell(4) != null ? Convert.ToInt32(row.GetCell(4).NumericCellValue) : 0;
                                SellSwapPer1000 = row.GetCell(5) != null ? Convert.ToInt32(row.GetCell(5).NumericCellValue) : 0;
                                SellSwapPer = row.GetCell(6) != null ? Convert.ToInt32(row.GetCell(6).NumericCellValue) : 0;

                                model.MasterInstrumentalId = MasterInstrumentalId;
                                model.MarkUpPer1000 = MarkUpPer1000;
                                model.MarkUpPer = MarkUpPer;
                                model.BuySwapPer1000 = BuySwapPer1000;
                                model.BuySwapPer = BuySwapPer;
                                model.SellSwapPer1000 = SellSwapPer1000;
                                model.SellSwapPer = SellSwapPer;
                                model.IbId = IBID;

                                _commisionMarkUpSetting.Add(model);
                            }
                            ListToDataTableConverter converter = new ListToDataTableConverter();
                            DataTable dtRuleInstrument = converter.ToDataTable(_commisionMarkUpSetting);
                            dtRuleInstrument.Columns.Remove("BdmName");
                            dtRuleInstrument.Columns.Remove("IbNo");
                            dtRuleInstrument.Columns.Remove("IbName");
                            dtRuleInstrument.Columns.Remove("objCommissionString");

                            int result = Convert.ToInt32(await _iCommissionSetting.CommisionMarkUpSetting_UpsertByDt(dtRuleInstrument));


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
        public async Task<IActionResult> ExportBDMCommission(CommisionMarkUpSetting model)
        {
            try
            {
                PageParam pageParam = new PageParam();
                pageParam.Offset = 0;
                pageParam.Limit = 100;

                ListToDataTableConverter converter = new ListToDataTableConverter();
                List<CommisionMarkUpSetting> modelExport = new List<CommisionMarkUpSetting>();
                modelExport = JsonConvert.DeserializeObject<List<CommisionMarkUpSetting>>(model.objCommissionString);

                DataTable dtCommisionInstrument = converter.ToDataTable(modelExport);
                dtCommisionInstrument.Columns.Remove("BdmName");
                dtCommisionInstrument.Columns.Remove("IbNo");
                dtCommisionInstrument.Columns.Remove("IbName");
                dtCommisionInstrument.Columns.Remove("objCommissionString");


                // Convert datatable into json  
                string JSON = JsonConvert.SerializeObject(dtCommisionInstrument);

                // Convert json into SummaryClass class list  
                var items = JsonConvert.DeserializeObject<List<CommisionMarkUpSettingExport>>(JSON);
                DataTable dtCommisionInt = converter.ToDataTable(items);
                 
                var workbook = new HSSFWorkbook();
                 
                var sheet = workbook.CreateSheet("Sheet1");
                NPOI.SS.UserModel.IRow headerrow = sheet.CreateRow(0);
                ICellStyle style = workbook.CreateCellStyle();

                for (int i = 0; i < dtCommisionInt.Columns.Count; i++)
                {
                    ICell cell = headerrow.CreateCell(i);
                    cell.CellStyle = style;
                    cell.SetCellValue(dtCommisionInt.Columns[i].ColumnName);
                }
                //Below loop is fill content
                var FieldsList = _BDMService.Get_All_Instrument().Result.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name });

                for (int i = 0; i <= dtCommisionInt.Rows.Count-1; i++)
                {
                    var row = (HSSFRow)sheet.CreateRow(i+1);

                    for (int j = 0; j <= dtCommisionInt.Columns.Count-1; j++)
                    {
                        dynamic DgvValue;
                        if (j == 0)
                        {
                            if (FieldsList.Where(x => x.Value == dtCommisionInstrument.Rows[i][1].ToString()).Any())
                            {
                                DgvValue = FieldsList.Where(x => x.Value == dtCommisionInstrument.Rows[i][1].ToString()).FirstOrDefault().Text;
                                DgvValue = DgvValue != null ? DgvValue : "";
                            }
                            else
                            {
                                DgvValue = "";
                            }
                        }
                        else
                        {
                            DgvValue = Convert.ToInt64(dtCommisionInt.Rows[i][j]);
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

                return Json(new { StatusCode = 200, Message = "BDM Instrument export Successfully" });
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
