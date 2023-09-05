using ClosedXML.Excel;
using System.Text;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Instrument;
using Epidi.Models.ViewModel.Question;
using Epidi.Models.ViewModel.TradeRule;
using Epidi.Service.TradeRuleUniversalValuesService;
using EPIDIWeb.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace EPIDIWeb.Controllers
{
    public class TradeRuleUniversalValuesController : Controller
    {
        private readonly ITradeRuleUniversalValuesService _tradeRuleUniversalValuesService;
        public TradeRuleUniversalValuesController(ITradeRuleUniversalValuesService tradeRuleUniversalValuesService)
        {
            _tradeRuleUniversalValuesService = tradeRuleUniversalValuesService;
        }
        public IActionResult Index()
        {

            ViewBag.Title = "TradeRule UniversalValues List";
            ViewBag.Method = "TradeRule UniversalValues";

            PageParam pageParam = new PageParam();
            pageParam.Offset = 0;

            pageParam.Limit = 1000000;
            string search = "";
            GetActionsDefintiions();
            GetWhenToCheckList();
            GetFunctionsList();
            GetTablesList();
			var response = _tradeRuleUniversalValuesService.GetAll_TradeRuleUniversalValues(pageParam, search);


            return View(response.Item1);
        }

        [HttpPost]
        public JsonResult GetAll_TradeRuleUniversalValues()
        {
                long totalRecord = 0;
                long filteredRecord = 0;
                var Draw = Request.Form["draw"].FirstOrDefault();
                PageParam pageParam = new PageParam();
                pageParam.Offset = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
                
                pageParam.Limit = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
                string search = Convert.ToString(Request.Form["search[value]"].FirstOrDefault() ?? "");
                var response = _tradeRuleUniversalValuesService.GetAll_TradeRuleUniversalValues(pageParam, search);

                totalRecord = response.Item2;
                filteredRecord = response.Item2;

                return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = response.Item1 });
            
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult SaveTradeRuleUniversalValues(List<TradeRuleUniversalValuesViewModel> model)
        {
            try
            {

                if (model != null)
                {
                    ListToDataTableConverter converter = new ListToDataTableConverter();
                    System.Data.DataTable dt = converter.ToDataTable(model);
                    var res = _tradeRuleUniversalValuesService.SaveTradeRuleUniversalValues(dt);
                    return Json(new { StatusCode = 200, Message = "Data Saved Successfully"});
                }
                else
                {
                    return Json(new { StatusCode = 200, Message = "Data Not Saved Successfully" });
                }
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        [HttpPost]
        public IActionResult ImportTradeRuleUniversalValuesExcel(IFormFile FileUpload)
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

                            List<TradeRuleUniversalValuesViewModel> tradeRuleUniversalValuesViewModels = new List<TradeRuleUniversalValuesViewModel>();

                            int Id;
                            int Priority;
                            string Values;
                            string Formulas;
                            string Function;
                            string ActionDefination;
                            string Table;
                            string WhenToCheck;
                            string Comment;
                            string Action;
                            
                            for (int rowNo = 1; rowNo < sheet.PhysicalNumberOfRows; rowNo++)
                            {
                                IRow row = sheet.GetRow(rowNo);

                                if (row != null)
                                {
                                   Id = row.GetCell(0) != null ? Convert.ToInt32(row.GetCell(0).NumericCellValue) : 0;
                                    Priority = row.GetCell(1) != null ? Convert.ToInt32(row.GetCell(1).NumericCellValue) : 0;
                                    Values = row.GetCell(2) != null ? row.GetCell(2).ToString() : "";
                                    Formulas = row.GetCell(3) != null ? row.GetCell(3).ToString() : "";
                                    Function = row.GetCell(5) != null ? row.GetCell(5).ToString() : "";
                                    ActionDefination = row.GetCell(4) != null ? row.GetCell(4).ToString() : "";
                                    Table = row.GetCell(6) != null ? row.GetCell(6).ToString() : "";
                                    WhenToCheck = row.GetCell(7) != null ? row.GetCell(7).ToString() : "";
                                    Comment = row.GetCell(8) != null ? row.GetCell(8).ToString() : "";
									Action = row.GetCell(9) != null ? row.GetCell(9).ToString() : "";

									TradeRuleUniversalValuesViewModel tradeRuleUniversalValuesViewModel = new TradeRuleUniversalValuesViewModel();

                                    tradeRuleUniversalValuesViewModel.Id = Id;
                                    tradeRuleUniversalValuesViewModel.Priority = Priority;
                                    tradeRuleUniversalValuesViewModel.Values = Values;
                                    tradeRuleUniversalValuesViewModel.Formulas = Formulas;
                                    tradeRuleUniversalValuesViewModel.Function = Function;
                                    tradeRuleUniversalValuesViewModel.ActionDefinationName = ActionDefination;
                                    tradeRuleUniversalValuesViewModel.Table = Table;
                                    tradeRuleUniversalValuesViewModel.WhenToCheck = WhenToCheck;
                                    tradeRuleUniversalValuesViewModel.Comment = Comment;
                                    tradeRuleUniversalValuesViewModel.Action = Action;



									tradeRuleUniversalValuesViewModels.Add(tradeRuleUniversalValuesViewModel);
                                }
                            }

                            ListToDataTableConverter converter = new ListToDataTableConverter();
                            System.Data.DataTable dt = converter.ToDataTable(tradeRuleUniversalValuesViewModels);
                            
                            var ruleid =  _tradeRuleUniversalValuesService.SaveTradeRuleUniversalValues(dt);

                            return Json(new { StatusCode = 200, Message = "File Imported Successfully", RuleId = ruleid });

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

        public IActionResult AddTradeRuleUniversalValues()
        {
            GetActionsDefintiions();
            GetWhenToCheckList();
            GetFunctionsList();
            GetTablesList();
            return View(new TradeRuleUniversalValuesViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var resp = await _tradeRuleUniversalValuesService.GetTradeRuleUniversalValues_ById(Id);
            if (resp != null)
            {
                GetActionsDefintiions();
				GetWhenToCheckList();
				GetFunctionsList();
				GetTablesList();
				return View("AddTradeRuleUniversalValues", resp);
            }
            else
            {
                return View(new TradeRuleUniversalValuesViewModel());
            }
        }

        [HttpGet]
        public IActionResult TradeRuleUniversalValuesDelete(int Id)
        {
            var res = _tradeRuleUniversalValuesService.TradeRuleUniversalValuesDelete(Id);
            return Json(res);
        }

        public void GetActionsDefintiions()
        {
            var resp = _tradeRuleUniversalValuesService.GetActionsDefintiions().Result
                .Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name });

            ViewBag.ActionsDefintiions = resp;
        }

		public void GetWhenToCheckList()
		{
			var resp = _tradeRuleUniversalValuesService.GetWhenToCheckList().Result
				.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name });

			ViewBag.WhenToCheck = resp;
		}

		public void GetFunctionsList()
		{
			var resp = _tradeRuleUniversalValuesService.GetFunctionsList().Result
				.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name });

			ViewBag.Functions = resp;
		}

		public void GetTablesList()
		{
			var resp = _tradeRuleUniversalValuesService.GetTablesList().Result
				.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name });

			ViewBag.Tables = resp;
		}

		public async Task<IActionResult> ExportTradeRuleUniversalValuesExcel()
		{
			var response = await _tradeRuleUniversalValuesService.ExportTradeRuleUniversalValues().ConfigureAwait(false);

			if (response != null)
			{
				//return Json(new { draw = requestModel.Draw, recordsFiltered = responseModel.FilteredCount, recordsTotal = responseModel.TotalCount, data = datavalue });
				StringBuilder stringBuilder = new StringBuilder();

				string contextType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
				string fileName = "TradeRuleUniversalValues" + "_" + DateTime.Now.ToString("MM/dd/yyyy") + ".xlsx";
				using (var workbook = new XLWorkbook())
				{
					var worksheet = workbook.Worksheets.Add("TraduRuleUniversalValues");

					worksheet.Cell(1, 1).Value = "Id";
					worksheet.Cell(1, 2).Value = "Priority";
					worksheet.Cell(1, 3).Value = "Values";
					worksheet.Cell(1, 4).Value = "Formulas";
					worksheet.Cell(1, 5).Value = "ActionDefination";
					worksheet.Cell(1, 6).Value = "Function";
					worksheet.Cell(1, 7).Value = "Table";
					worksheet.Cell(1, 8).Value = "WhenToCheck";
					worksheet.Cell(1, 9).Value = "Comment";
					worksheet.Cell(1, 10).Value = "Action";

					worksheet.Range("A1:J1").Style.Fill.SetBackgroundColor(XLColor.Yellow);

					//worksheet.Range("A1:N2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

					for (int index = 1; index <= response.Count; index++)
					{
						worksheet.Cell(index + 1, 1).Style.NumberFormat.SetFormat("0");
						worksheet.Cell(index + 1, 1).Value = index;
						worksheet.Cell(index + 1, 2).Value = response[index - 1].Priority;
						worksheet.Cell(index + 1, 3).Value = response[index - 1].Values;
						worksheet.Cell(index + 1, 4).Value = response[index - 1].Formulas;
						worksheet.Cell(index + 1, 5).Value = response[index - 1].ActionDefinationName;
						worksheet.Cell(index + 1, 6).Value = response[index - 1].Function;
						worksheet.Cell(index + 1, 7).Value = response[index - 1].Table;
						worksheet.Cell(index + 1, 8).Value = response[index - 1].WhenToCheck;
						worksheet.Cell(index + 1, 9).Value = response[index - 1].Comment;
						worksheet.Cell(index + 1, 10).Value = response[index - 1].Action;
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
	}
}
