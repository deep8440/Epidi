using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.Instrument;
using Epidi.Models.ViewModel.LeverageRules;
using Epidi.Models.ViewModel.RuleCondition;
using Epidi.Models.ViewModel.SwapRule;
using Epidi.Service.BDM;
using Epidi.Service.InstrumentService;
using Epidi.Service.RuleConditionService;
using EPIDIWeb.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.Util;
using NPOI.XSSF.UserModel;
using SendGrid;
using System.Data;
using System.Text;

namespace EPIDIWeb.Controllers
{
    public class ImportInstrumentsController : Controller
    {
        private readonly IInstrumentService _instrumentService;
        private readonly IRuleConditionService _ruleConditionService;
        private readonly IBDMService _BDMService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ImportInstrumentsController(IInstrumentService instrumentService, IWebHostEnvironment webHostEnvironment,
            IRuleConditionService ruleConditionService,
            IBDMService BDMService)
        {
            _instrumentService = instrumentService;
            _ruleConditionService = ruleConditionService;
            _BDMService = BDMService;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            ViewBag.Title = "Instrument Specification List ";
            return View();
        }

        public IActionResult ImportGateIO()
        {
            return View();
        }
        public IActionResult ImportLMAX()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, int tradeId)
        {
            PageParam pageParam = new PageParam();
            pageParam.Offset = 0;
            pageParam.Limit = 1000000;
            var instruments = await _instrumentService.GetAllInstrument(pageParam, "");
            var symbolgroup = await _instrumentService.GetAllSymbolGroup();
            var tradeStatus = await _instrumentService.GetAllTradeStatus();
            ViewBag.Instruments = instruments.Item1;
            ViewBag.Symbolgroup = symbolgroup;
            ViewBag.TradeStatus = tradeStatus;
            var model = await _instrumentService.GetByID(id, tradeId);
            return PartialView("Edit", model);
        }
        public async Task<IActionResult> UpdateSpecifications(MasterInstrumentViewModel model)
        {
            var res = await _instrumentService.UpdateInstrumentMaster(model);
            return Json(res);

        }
        public async Task<IActionResult> EditSpecificationRule(int id)
        {
            //PageParam pageParam = new PageParam();
            //pageParam.Offset = 0;
            //pageParam.Limit = 1000000;
            //var instruments = await _instrumentService.GetAllInstrument(pageParam, "");
            //var symbolgroup = await _instrumentService.GetAllSymbolGroup();
            //var tradeStatus = await _instrumentService.GetAllTradeStatus();
            //ViewBag.Instruments = instruments.Item1;
            //ViewBag.Symbolgroup = symbolgroup;
            //ViewBag.TradeStatus = tradeStatus;

            var response = await _instrumentService.GetSpecificationRuleByID(id);

            var ruleConditions = await _ruleConditionService.GetRuleConditionBySpecificationRuleId(id);
            response.objRuleConditionViewModel = ruleConditions;
            if (ruleConditions != null)
            {
                response.objRuleConditionViewModel.conditionRulesDtlObj = JsonConvert.SerializeObject(ruleConditions.rulesDtlViewModel);
            }

            return PartialView("EditSpecificationRule", response);
        }


        [HttpPost]
        public async Task<JsonResult> All_Import_MasterInstrument(int Id)
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
            var response = await _instrumentService.GetAllImportMasterInstrument(Id, pageParam,search).ConfigureAwait(false);

            totalRecord = response.Item2;
            filteredRecord = response.Item2;

            return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = response.Item1 });
        }
        [HttpPost]
        public async Task<JsonResult> All_SpecificationRule()
        {
            long totalRecord;
            long filteredRecord;
            var Draw = Request.Form["draw"].FirstOrDefault();
            PageParam pageParam = new()
            {
                Offset = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0"),
                Limit = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0")
            };

            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();


            string search = Convert.ToString(Request.Form["search[value]"].FirstOrDefault() ?? "");
            var response = await _instrumentService.GetAllSpecificationRule(pageParam, search).ConfigureAwait(false);

            totalRecord = response.Item2;
            filteredRecord = response.Item2;

            var responseData = OrderByExtension.OrderBy(response.Item1.AsQueryable(), sortColumn, sortColumnDirection).ToList();


            return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = responseData });
        }

        [HttpPost]
        public async Task<JsonResult> All_Import_GATEIO()
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
            var response = await _instrumentService.GetAllImportGATEIO(pageParam, search).ConfigureAwait(false);

            totalRecord = response.Item2;
            filteredRecord = response.Item2;

            return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = response.Item1 });
        }

        [HttpPost]
        public async Task<JsonResult> All_Import_LMAX()
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
            var response = await _instrumentService.GetAllImportLMax(pageParam, search).ConfigureAwait(false);

            totalRecord = response.Item2;
            filteredRecord = response.Item2;

            return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = response.Item1 });
        }

        [HttpPost]
        public async Task<IActionResult> UploadGateIOInstrumentsExcel(IFormFile FileUpload)
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

                            List<GateIOInstrumentMappingViewModel> gateIOInstrumentMappingViewModels = new List<GateIOInstrumentMappingViewModel>();

                            string Name;
                            int tradeStatus;
                            string Base;
                            string quote;
                            decimal fee;
                            decimal minBaseAmount;
                            decimal minQuoteAmount;
                            decimal amountPrecision;
                            decimal precision;
                            decimal sellStart;
                            decimal buyStart;

                            for (int rowNo = 1; rowNo < sheet.PhysicalNumberOfRows; rowNo++)
                            {
                                IRow row = sheet.GetRow(rowNo);

                                tradeStatus = row.GetCell(0) != null ? Convert.ToInt32(row.GetCell(0).NumericCellValue) : 0;
                                Name = row.GetCell(1) != null ? row.GetCell(1).ToString() : "";
                                Base = row.GetCell(2) != null ? row.GetCell(2).ToString() : "";
                                quote = row.GetCell(3) != null ? row.GetCell(3).ToString() : "";
                                fee = row.GetCell(4) != null ? Convert.ToDecimal(row.GetCell(4).NumericCellValue) : 0;
                                minBaseAmount = row.GetCell(5) != null ? Convert.ToDecimal(row.GetCell(5).NumericCellValue) : 0;
                                minQuoteAmount = row.GetCell(6) != null ? Convert.ToDecimal(row.GetCell(6).NumericCellValue) : 0;
                                amountPrecision = row.GetCell(7) != null ? Convert.ToDecimal(row.GetCell(7).NumericCellValue) : 0;
                                precision = row.GetCell(8) != null ? Convert.ToDecimal(row.GetCell(8).NumericCellValue) : 0;
                                sellStart = row.GetCell(9) != null ? Convert.ToDecimal(row.GetCell(9).NumericCellValue) : 0;
                                buyStart = row.GetCell(10) != null ? Convert.ToDecimal(row.GetCell(10).NumericCellValue) : 0;

                                GateIOInstrumentMappingViewModel gateIO = new GateIOInstrumentMappingViewModel();
                                gateIO.TradeStatus = tradeStatus;
                                gateIO.Name = Name;
                                gateIO.Base = Base;
                                gateIO.Quote = quote;
                                gateIO.Fee = fee;
                                gateIO.MinBaseAmount = minBaseAmount;
                                gateIO.MinQuoteAmount = minQuoteAmount;
                                gateIO.AmountPrecision = amountPrecision;
                                gateIO.Precision = precision;
                                gateIO.SellStart = sellStart;
                                gateIO.BuyStart = buyStart;
                                gateIOInstrumentMappingViewModels.Add(gateIO);
                                
                            }

                            ListToDataTableConverter converter = new ListToDataTableConverter();
                            DataTable dt = converter.ToDataTable(gateIOInstrumentMappingViewModels);

                            await _instrumentService.AddGateIOInstrument(dt);


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
        public async Task<IActionResult> UploadLMaxInstrumentsExcel(IFormFile LMAXUpload)
        {
            string filePath = string.Empty;
            try
            {
                if (LMAXUpload != null)
                {
                    IWorkbook workbook;
                    var supportedTypes = new[] { "xls", "xlsx" };
                    var fileExt = Path.GetExtension(LMAXUpload.FileName);
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
                            workbook = new HSSFWorkbook(LMAXUpload.OpenReadStream(), true);
                        }
                        else
                        {
                            workbook = new XSSFWorkbook(LMAXUpload.OpenReadStream());
                        }

                        ISheet sheet = workbook.GetSheetAt(0);

                        try
                        {
                            IRow HeaderRow = sheet.GetRow(0);

                            List<LMaxInstrumentsViewModel> lMaxInstrumentsViewModels = new List<LMaxInstrumentsViewModel>();

                            string symbol;
                            long securityid;
                            string currency;
                            string UOM;
                            string assetClass;
                            decimal QtyIncrement;
                            decimal priceIncrement;
                            bool tickerEnabled;

                            for (int rowNo = 1; rowNo < sheet.PhysicalNumberOfRows; rowNo++)
                            {
                                IRow row = sheet.GetRow(rowNo);


                                symbol = row.GetCell(0) != null ? row.GetCell(0).ToString() : "";
                                securityid = row.GetCell(1) != null ? Convert.ToInt64(row.GetCell(1).NumericCellValue) : 0;
                                currency = row.GetCell(2) != null ? row.GetCell(2).ToString() : "";
                                UOM = row.GetCell(3) != null ? row.GetCell(3).ToString() : "";
                                assetClass = row.GetCell(4) != null ? row.GetCell(4).ToString() : "";
                                QtyIncrement = row.GetCell(5) != null ? Convert.ToDecimal(row.GetCell(5).NumericCellValue) : 0;
                                priceIncrement = row.GetCell(6) != null ? Convert.ToDecimal(row.GetCell(6).NumericCellValue) : 0;
                                int te = row.GetCell(7) != null ? Convert.ToInt32(row.GetCell(7).NumericCellValue) : 0;
                                if (te == 1)
                                    tickerEnabled = true;
                                else
                                    tickerEnabled = false;


                                LMaxInstrumentsViewModel lMax = new LMaxInstrumentsViewModel();

                                lMax.Symbol = symbol;
                                lMax.Securityid = securityid;
                                lMax.Currency = currency;
                                lMax.UOM = UOM;
                                lMax.AssetClass = assetClass;
                                lMax.QtyIncrement = QtyIncrement;
                                lMax.PriceIncrement = priceIncrement;
                                lMax.TickerEnabled = tickerEnabled;

                                lMaxInstrumentsViewModels.Add(lMax);
                            }

                            ListToDataTableConverter converter = new ListToDataTableConverter();
                            DataTable dt = converter.ToDataTable(lMaxInstrumentsViewModels);


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
        public async Task<IActionResult> UploadMasterInstrumentsExcel(IFormFile FileUpload, InstrumentSpecificationRule instrumentSpecificationRule)
        {
            string filePath = string.Empty;
            try
            {
                if (FileUpload != null)
                {
                    if (Request.Form["Id"].FirstOrDefault() != "")
                    {
                        instrumentSpecificationRule.Id = Convert.ToInt32(Request.Form["Id"].FirstOrDefault());
                    }

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

                            List<MasterInstrumentViewModel> masterInstrumentViewModels = new List<MasterInstrumentViewModel>();

                            int Id;
                            string Comment;
                            int Priority;
                            string InstrumentName;
                            string SymbolGroup;
                            string QTFrom;
                            string QTTo;
                            string Day;
                            string TTFrom;
                            string TTTo;
                            string SymbolDenomination;
                            string TradeStatus;
                            decimal AverageSpread;
                            decimal Decimals;
                            string UnitDescription;
                            int ZerosTobeGrouped;
                            
                            //string imagesPath = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, "img");
                            //grab the image file
                            //imagesPath = System.IO.Path.Combine(imagesPath, "image.png");
                            ////create an image from the path
                            //System.Drawing.Image image = System.Drawing.Image.FromFile(imagesPath);

                            byte[] imageBytes = new byte[0];
                            var pictures = workbook.GetAllPictures();
                            
                            
                            foreach (var item in pictures)
                            {
                                var pic = (XSSFPictureData)item;
                                imageBytes = pic.Data;

                                //img = item;
                                //InputStream inputStream = new FileInputStream((Stream)item);
                                //imageBytes = IOUtils.ToByteArray(inputStream);
                                //MemoryStream ms = new MemoryStream();
                                //pull the memory stream from the image (I need this for the byte array later)
                                //image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                            }
                            //if (instrumentSpecificationRule.Comment != "" && instrumentSpecificationRule.Priority > 0 && sheet.PhysicalNumberOfRows > 0)
                            //{
                            Comment = instrumentSpecificationRule.Comment;
                            Priority = instrumentSpecificationRule.Priority;

                            for (int rowNo = 1; rowNo < sheet.PhysicalNumberOfRows; rowNo++)
                            {
                                IRow row = sheet.GetRow(rowNo);

                                if (row != null)
                                {
                                    Id = row.GetCell(0) != null ? Convert.ToInt32(row.GetCell(0).ToString()) : 0;
                                    InstrumentName = row.GetCell(1) != null ? row.GetCell(1).ToString() : "";
                                    SymbolGroup = row.GetCell(2) != null ? row.GetCell(2).ToString() : "";
                                    QTFrom = Convert.ToString(row.GetCell(3).DateCellValue.ToString("HH:mm:ss")) != null ? Convert.ToString(row.GetCell(3).DateCellValue.ToString("HH:mm:ss")) : "";
                                    QTTo = Convert.ToString(row.GetCell(4).DateCellValue.ToString("HH:mm:ss")) != null ? Convert.ToString(row.GetCell(4).DateCellValue.ToString("HH:mm:ss")) : "";
                                    Day = row.GetCell(5) != null ? row.GetCell(5).ToString() : "";
                                    TTFrom = Convert.ToString(row.GetCell(6).DateCellValue.ToString("HH:mm:ss")) != null ? Convert.ToString(row.GetCell(6).DateCellValue.ToString("HH:mm:ss")) : "";
                                    TTTo = Convert.ToString(row.GetCell(7).DateCellValue.ToString("HH:mm:ss")) != null ? Convert.ToString(row.GetCell(7).DateCellValue.ToString("HH:mm:ss")) : "";
                                    SymbolDenomination = row.GetCell(8) != null ? row.GetCell(8).ToString() : "";
                                    TradeStatus = row.GetCell(9) != null ? row.GetCell(9).ToString() : "";
                                    

                                    if (Convert.ToString(row.GetCell(10)) != "")
                                    {
                                        AverageSpread = row.GetCell(10) != null ? Convert.ToDecimal(row.GetCell(10).ToString()) : 0;
                                    }
                                    else
                                    {
                                        AverageSpread = 0;
                                    }
                                    if (Convert.ToString(row.GetCell(11)) != "")
                                    {
                                        Decimals = row.GetCell(11) != null ? Convert.ToDecimal(row.GetCell(11).ToString()) : 0;
                                    }
                                    else
                                    {
                                        Decimals = 0;
                                    }

                                    UnitDescription = row.GetCell(12) != null ? row.GetCell(12).ToString() : "";
                                    if (Convert.ToString(row.GetCell(13)) != "")
                                    {
                                        ZerosTobeGrouped = row.GetCell(13) != null ? Convert.ToInt32(row.GetCell(13).ToString()) : 0;
                                    }
                                    else
                                    {
                                        ZerosTobeGrouped = 0;
                                    }
                                    //var pic = (XSSFPictureData)row.GetCell(14);
                                    //byte[] data = pic.Data;

                                    MasterInstrumentViewModel instrumentMappingViewModel = new MasterInstrumentViewModel();

                                    instrumentMappingViewModel.Id = Id;
                                    instrumentMappingViewModel.InstrumentName = InstrumentName;
                                    instrumentMappingViewModel.ZerosTobeGrouped = ZerosTobeGrouped;
                                    instrumentMappingViewModel.SymbolGroup = SymbolGroup;
                                    instrumentMappingViewModel.QTFrom = QTFrom;
                                    instrumentMappingViewModel.QTTo = QTTo;
                                    instrumentMappingViewModel.Day = Day;
                                    instrumentMappingViewModel.TTFrom = TTFrom;
                                    instrumentMappingViewModel.TTTo = TTTo;
                                    instrumentMappingViewModel.SymbolDenomination = SymbolDenomination;
                                    instrumentMappingViewModel.TradeStatus = TradeStatus;
                                    instrumentMappingViewModel.AverageSpread = AverageSpread;
                                    instrumentMappingViewModel.Decimals = Decimals;
                                    instrumentMappingViewModel.UnitDescription = UnitDescription;

                                    masterInstrumentViewModels.Add(instrumentMappingViewModel);
                                }
                            }

                            ListToDataTableConverter converter = new ListToDataTableConverter();
                            System.Data.DataTable dt = converter.ToDataTable(masterInstrumentViewModels);
                            dt.Columns.Remove("tradeTimings");
                            dt.Columns.Remove("tradeId");
                            dt.Columns.Remove("Path");
                            dt.Columns.Remove("Image");
                            var ruleid = await _instrumentService.AddMasterInstrumentMapping(dt, instrumentSpecificationRule);

                            return Json(new { StatusCode = 200, Message = "File Imported Successfully", RuleId = ruleid });
                            //}

                            //return Json(new { StatusCode = 400, Message = "File Imported failed, validation" });


                        }
                        catch (Exception ex)
                        {
                            return Json(new { StatusCode = 400, Message = ex.Message.ToString() });
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

        //[HttpPost]
        //public async Task<IActionResult> SaveSpecificationRuleMasterInstruments(string objRuleInstrumentViewModelstr)
        //{
        //    List<MasterInstrumentViewModel> masterInstrumentViewModels = new List<MasterInstrumentViewModel>();
        //    model.objRuleInstrumentViewModel = JsonConvert.DeserializeObject<List<UpdateSpecificationRuleInstrumentViewModel>>(model.objRuleInstrumentViewModelstr);

        //    MasterInstrumentViewModel instrumentMappingViewModel = new MasterInstrumentViewModel();

        //    instrumentMappingViewModel.Id = Id;
        //    instrumentMappingViewModel.InstrumentName = InstrumentName;
        //    instrumentMappingViewModel.ZerosTobeGrouped = ZerosTobeGrouped;
        //    instrumentMappingViewModel.SymbolGroup = SymbolGroup;
        //    instrumentMappingViewModel.QTFrom = QTFrom;
        //    instrumentMappingViewModel.QTTo = QTTo;
        //    instrumentMappingViewModel.Day = Day;
        //    instrumentMappingViewModel.TTFrom = TTFrom;
        //    instrumentMappingViewModel.TTTo = TTTo;
        //    instrumentMappingViewModel.SymbolDenomination = SymbolDenomination;
        //    instrumentMappingViewModel.TradeStatus = TradeStatus;
        //    instrumentMappingViewModel.AverageSpread = AverageSpread;
        //    instrumentMappingViewModel.Decimals = Decimals;
        //    instrumentMappingViewModel.UnitDescription = UnitDescription;
        //    //instrumentMappingViewModel.Path = imagesPath;

        //    masterInstrumentViewModels.Add(instrumentMappingViewModel);
        //}
        [HttpPost]
        public async Task<IActionResult> UpdateSpecificationRule(IFormFile logoUploader, SpecificationRuleViewModel model)
        {
            try
            {
                //model.objRuleLpPriorityViewModel = JsonConvert.DeserializeObject<List<RuleLpPriorityViewModel_Dt>>(model.objRuleLpPriorityViewModelstr);
                model.objRuleConditionViewModel = JsonConvert.DeserializeObject<List<LeverageRulesDtlViewModel>>(model.objRuleConditionViewModelstr);
                
                if (!string.IsNullOrEmpty(model.objRuleInstrumentViewModelstr))
                {
                    model.objRuleInstrumentViewModel = JsonConvert.DeserializeObject<List<UpdateSpecificationRuleInstrumentViewModel>>(model.objRuleInstrumentViewModelstr);
                }
                if (logoUploader != null)
                {
                    string foderName = "InstrumentLogoUpload";
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, foderName);
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string filePath = Path.Combine(uploadsFolder, logoUploader.FileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        logoUploader.CopyTo(fileStream);
                    }

                    foreach (var item in model.objRuleInstrumentViewModel)
                    {
                        item.Logo = "/" + foderName + "/" + logoUploader.FileName;
                    }
                }

                
                ListToDataTableConverter converter = new ListToDataTableConverter();

                DataTable dtRuleInstrument = new DataTable();
                if (model.objRuleInstrumentViewModel != null && model.objRuleInstrumentViewModel.Count > 0)
                {
                     dtRuleInstrument = converter.ToDataTable(model.objRuleInstrumentViewModel);
                }
                DataTable dtRuleLpPriority = new DataTable();
                DataTable dtRuleConditionsDtl = converter.ToDataTable(model.objRuleConditionViewModel);

                if (dtRuleConditionsDtl.Rows.Count > 0)
                {
                    RuleConditionViewModel ruleConditionViewModel = new RuleConditionViewModel();
                    ruleConditionViewModel.SpecificationRuleId = Convert.ToInt32(model.Id);
                    ruleConditionViewModel.Id = Convert.ToInt32(dtRuleConditionsDtl.Rows[0]["RuleConditionsId"]);
                    ruleConditionViewModel.rulesDtlViewModel = model.objRuleConditionViewModel;

                    _ruleConditionService.Upsert(ruleConditionViewModel);
                }
                InstrumentSpecificationRule instrumentSpecificationRule = new InstrumentSpecificationRule
                {
                    Id = model.Id,
                    Comment = model.Comment,
                    Priority = model.Priority,
                };

                _instrumentService.UpdateSpecificationRuleInstrument(dtRuleInstrument, instrumentSpecificationRule);
                return Json(new { StatusCode = 200, Message = "Specification rule updated successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { StatusCode = 400, Message = ex.Message.ToString() });
            }
        }

        [HttpPost]
        public IActionResult DeleteSpecificationRuleInstrument(int SpecificationRuleId, int InstrumentId)
        {
            ResponseViewModel responseViewModel = new ResponseViewModel();

            var res = _instrumentService.DeleteSpecificationRuleInstrumentById(SpecificationRuleId, InstrumentId);
            if (res != null)
            {
                responseViewModel.Code = 200;
                responseViewModel.Message = "Specification Rule Instrument Deleted Successfully";
            }
            else
            {
                responseViewModel.Code = 400;
                responseViewModel.Message = "Specification Rule Instrument Not Deleted";
            }
            return Json(responseViewModel);
        }

        public async Task<IActionResult> DeleteSpecificationRule(int Id)
        {
            List<ResponseViewModel> responseViewModel = new List<ResponseViewModel>();
            ResponseViewModel responseViewModel1 = new ResponseViewModel();
            var res = await _instrumentService.DeleteSpecificationRuleById(Id);
            if (res != null)
            {
                responseViewModel1.Code = 200;
                responseViewModel1.Message = "Specification Rule Deleted Successfully";
            }
            else
            {
                responseViewModel1.Code = 400;
                responseViewModel1.Message = "Specification Rule Not Deleted";
            }
            responseViewModel.Add(responseViewModel1);
            return Json(responseViewModel);
        }

        [HttpPost]
        public IActionResult DeleteSpecificationRuleConditionDtlById(int RuleConditionId, int RuleConditionDtlId)
        {
            ResponseViewModel responseViewModel = new ResponseViewModel();

            var res = _ruleConditionService.DeleteSwapRuleConditionDtlById(RuleConditionId, RuleConditionDtlId);
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
        public JsonResult GetTradeStatus()
        {
            var tradeStatus = _instrumentService.GetAllTradeStatus().Result.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Status });

            return Json(tradeStatus);
        }
        public JsonResult GetSymbolGroup()
        {
            var symbolGroup = _instrumentService.GetAllSymbolGroup().Result.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Group });

            return Json(symbolGroup);
        }
        public JsonResult GetAllInstrument()
        {
            List<SelectListItem> oprationList = new List<SelectListItem>();
            var FieldsList = _BDMService.Get_All_Instrument().Result.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name });
            //var country = _countryService.GetAllCountries().Result.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.CountryName });
            foreach (var item in FieldsList)
            {
                oprationList.Add(new SelectListItem { Text = item.Text, Value = item.Value });
            }
            return Json(oprationList);
        }
        public IActionResult ImportSpecificationRule()
        {
            return View(new InstrumentSpecificationRule());
        }

        [HttpPost]
        public async Task<IActionResult> ExportInstrumentExcel(int RuleId)
        {
            try
            {
                PageParam pageParam = new()
                {
                    Offset = 0,
                    Limit = 1000000000
                };
                var responseData = await _instrumentService.GetAllImportMasterInstrument(RuleId, pageParam, "").ConfigureAwait(false);
                var response = responseData.Item1?.ToList();

                if (response == null || response.Count == 0)
                {
                    return Json("No data available for export.");
                }

                //string contextType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //string fileName = $"InstrumentSpecifications{RuleId}_{DateTime.Now.ToString("MM/dd/yyyy")}.xlsx";

                using (var workbook = new XLWorkbook())
                {
                    string contextType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    string fileName = $"InstrumentSpecifications{RuleId}_{DateTime.Now.ToString("MM/dd/yyyy")}.xlsx";
                    var worksheet = workbook.Worksheets.Add("InstrumentSpecification");

                    // ... (Header setup and data population logic)

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content, contextType, fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                // logger.LogError(ex, "An error occurred while exporting instrument data.");

                // Return an error message to the user
                return Content("An error occurred while exporting instrument data. Please try again later.");
            }
        }


    }
}
