using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.UsersDocuments
{
    public class UsersDocumentViewModelApi
    {
        public int UserId { get; set; }
        public IFormFile? FrontDocument { get; set; }
        public IFormFile? BackDocument { get; set; }
        public IFormFile? AddressDocument { get; set; }

        public int CreatedBy { get; set; }

    }


    public class UsersDocumentViewModel
    {
        /// <summary>
        /// Gets or sets the Id value.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the UserId value.
        /// </summary>
        [FromForm(Name = "UserId")]
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the FrontDocument value.
        /// </summary>
        [FromForm(Name = "FrontDocument")]
        public IFormFile? FrontDocument { get; set; }

        /// <summary>
        /// Gets or sets the BackDocument value.
        /// </summary>
        [FromForm(Name = "BackDocument")]
        public IFormFile? BackDocument { get; set; }

        /// <summary>
        /// Gets or sets the AddressDocument value.
        /// </summary>
        [FromForm(Name = "AddressDocument")]
        public IFormFile? AddressDocument { get; set; }

        /// <summary>
        /// Gets or sets the DocumentPath value.
        /// </summary>
        public string? DocumentPath { get; set; }

        /// <summary>
        /// Gets or sets the DocumentName value.
        /// </summary>
        public string? DocumentName { get; set; }

        /// <summary>
        /// Gets or sets the CreatedAt value.
        /// </summary>
        public int? CreatedBy { get; set; }
        /// <summary>
        /// Gets or sets the UpdatedAt value.
        /// </summary>
        public int? UpdatedBy { get; set; }
        public string DocumentType { get; set; }
    }
}
