using Epidi.API.Common;
using Epidi.Models.Model;
using Epidi.Models.Utility;
using Epidi.Models.ViewModel.UsersDocuments;
using Epidi.Service.Helpers;
using Epidi.Service.UsersDocumentsService;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using System;
using System.Net;


namespace Epidi.API.Controllers.UsersDocuments
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class UsersDocumentsController : ControllerBase
	{
		private readonly IUsersDocumentsService _usersDocumentsService;
		private readonly IWebHostEnvironment _environment;
		private readonly IConfiguration _configuration;


		public UsersDocumentsController(IUsersDocumentsService usersDocumentsService, IWebHostEnvironment environment, IConfiguration iConfig)
		{
			_configuration = iConfig;
			_usersDocumentsService = usersDocumentsService;
			_environment = environment;
		}

		[AllowAnonymous]
		[HttpPost]
		[Produces("application/json")]
		[ProducesResponseType((int)HttpStatusCode.Forbidden)]
		[ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
		[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
		[ProducesResponseType(typeof(BaseResponse<UsersDocumentViewModel>), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> Post([FromForm] UsersDocumentViewModelApi req)
		{
			BaseResponse<UsersDocumentViewModel> response = new BaseResponse<UsersDocumentViewModel>();
			UsersDocumentViewModel usersDocumentViewModel = new UsersDocumentViewModel();
			//Doc Name: Front,Back,Address

			var accountName = this._configuration.GetSection("DocumentUploadCred")["BlobAccountName"];
			var accountKey = this._configuration.GetSection("DocumentUploadCred")["BlobAccountKey"];
			var containerName = this._configuration.GetSection("DocumentUploadCred")["Blobcontainername"];
			var fileUrl = this._configuration.GetSection("DocumentUploadCred")["FileUrl"];
			var cloudStorageAccount = new CloudStorageAccount(new StorageCredentials(accountName, accountKey), true);
			var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
			var cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);
			


			if (req.FrontDocument.Length > 0)
			{
				var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(req.FrontDocument.FileName.ToString());

				await using var memoryStream = new MemoryStream();
				await req.FrontDocument.CopyToAsync(memoryStream);
				var streamBytes = memoryStream.ToArray();

				using (Stream stream = new MemoryStream(streamBytes))
				{
					await cloudBlockBlob.UploadFromStreamAsync(stream);
				}

				usersDocumentViewModel.UserId = req.UserId;
				usersDocumentViewModel.DocumentPath = fileUrl+ req.FrontDocument.FileName.ToString();
				usersDocumentViewModel.DocumentType = "Front";
				usersDocumentViewModel.DocumentName = req.FrontDocument.FileName;
				usersDocumentViewModel.CreatedBy = req.CreatedBy;
				response.ResponseData = await _usersDocumentsService.Upsert(usersDocumentViewModel);
			}
			if (req.BackDocument.Length > 0)
			{

				var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(req.BackDocument.FileName.ToString());

				await using var memoryStream = new MemoryStream();
				await req.BackDocument.CopyToAsync(memoryStream);
				var streamBytes = memoryStream.ToArray();

				using (Stream stream = new MemoryStream(streamBytes))
				{
					await cloudBlockBlob.UploadFromStreamAsync(stream);
				}

				usersDocumentViewModel.UserId = req.UserId;
				usersDocumentViewModel.DocumentPath = fileUrl + req.BackDocument.FileName;
				usersDocumentViewModel.DocumentType = "Back";
				usersDocumentViewModel.DocumentName = req.BackDocument.FileName;
				usersDocumentViewModel.CreatedBy = req.CreatedBy;
				response.ResponseData = await _usersDocumentsService.Upsert(usersDocumentViewModel);

			}
			if (req.AddressDocument.Length > 0)
			{
				var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(req.AddressDocument.FileName.ToString());

				await using var memoryStream = new MemoryStream();
				await req.AddressDocument.CopyToAsync(memoryStream);
				var streamBytes = memoryStream.ToArray();

				using (Stream stream = new MemoryStream(streamBytes))
				{
					await cloudBlockBlob.UploadFromStreamAsync(stream);
				}
				usersDocumentViewModel.UserId = req.UserId;
				usersDocumentViewModel.DocumentPath = fileUrl + req.AddressDocument.FileName;
				usersDocumentViewModel.DocumentType = "Address";
				usersDocumentViewModel.DocumentName = req.AddressDocument.FileName;
				usersDocumentViewModel.CreatedBy = req.CreatedBy;
				response.ResponseData = await _usersDocumentsService.Upsert(usersDocumentViewModel);
			}
			response.ResponseCode = (int)HttpStatusCode.Created;
			response.ResponseMessage = string.Format(General.AccountCreated, "UsersDocuments");
			return StatusCode(response.ResponseCode, response);
		}

		[AllowAnonymous]
		[HttpGet("{UserId}")]
		[Produces("application/json")]
		[ProducesResponseType((int)HttpStatusCode.Forbidden)]
		[ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
		[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
		[ProducesResponseType(typeof(BaseResponse<UsersDocumentViewModel>), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> FetchbyUserId(int UserId)
		{
			BaseResponse<List<UsersDocumentViewModel>> response = new BaseResponse<List<UsersDocumentViewModel>>();
			response.ResponseCode = (int)HttpStatusCode.OK;
			string baseUrl = _configuration.GetSection("FileUpload").GetSection("BaseUrl").Value;
			response.ResponseData = await _usersDocumentsService.GetByUserId(UserId);
			if (response.ResponseData != null)
			{
				if (response.ResponseData.Count > 0)
				{
					for (int i = 0; i < response.ResponseData.Count; i++)
					{
						response.ResponseData[i].DocumentPath = Path.Combine(baseUrl, response.ResponseData[i].DocumentPath);
					}
				}
			}
			response.ResponseMessage = "Users Document Fetch Successfully";
			return StatusCode(response.ResponseCode, response);
		}


	}
}
