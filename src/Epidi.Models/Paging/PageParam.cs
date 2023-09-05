using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.Paging
{
	public class PageParam
	{
		public const int MinOffset = 0, MinPage = 1;
		public const int MinLimit = 50, MinPageSize = 50;

		private int? offset;
		private int? limit;
		private int? page;
		private int? pageSize;
		private string sortDirection;
		private string sortBy;
		private int totalCount;

		/// <summary>
		/// Initializes a new instance of the <see cref="PageParam"/> class.
		/// </summary>
		public PageParam()
		{
			this.Offset = MinOffset;
			this.Limit = MinLimit;
			this.sortBy = "desc";
		}

		///// <summary>
		///// Initializes a new instance of the <see cref="PageParam"/> class.
		///// </summary>
		///// <param name="offset">The offset.</param>
		///// <param name="limit">The limit.</param>
		////public PageParam(int offset, int limit)
		////{
		////    this.Offset = offset;
		////    this.Limit = limit;
		////}

		/// <summary>
		/// Initializes a new instance of the <see cref="PageParam"/> class.
		/// </summary>
		/// <param name="pageParam">The page parameter.</param>
		public PageParam(PageParam pageParam)
		{
			this.sortBy = pageParam.SortBy;
			this.SortDirection = pageParam.SortDirection;
			if (pageParam.IsPageProvided)
			{
				this.page = pageParam.GetPage();
				this.pageSize = pageParam.GetPageSize();
			}
			else
			{
				this.offset = pageParam.GetOffset();
				this.limit = pageParam.GetLimit();
			}
		}

		/// <summary>
		/// Gets or sets the page.
		/// </summary>
		/// <value>
		/// The page.
		/// </value>
		[Range(0, int.MaxValue)]
		public int? Offset
		{
			get
			{
				if (this.IsPageProvided)
				{
					return (this.page - 1) * this.Limit;
				}

				return this.offset ?? MinOffset;
			}

			set
			{
				this.offset = value;
			}
		}

		/// <summary>
		/// Gets or sets the size of the page.
		/// </summary>
		/// <value>
		/// The size of the page.
		/// </value>
		[Range(1, int.MaxValue)]
		public int? Limit
		{
			get
			{
				if (this.IsPageProvided)
				{
					return this.PageSize;
				}

				return this.limit ?? MinLimit;
			}

			set
			{
				this.limit = value;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is offset provided.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is offset provided; otherwise, <c>false</c>.
		/// </value>
		public bool IsOffsetProvided
		{
			get
			{
				return this.offset.HasValue;
			}
		}

		/// <summary>
		/// Gets or sets the page.
		/// </summary>
		/// <value>
		/// The page.
		/// </value>
		[Range(1, int.MaxValue)]
		public int? Page
		{
			get
			{
				return this.page ?? MinPage;
			}

			set
			{
				this.page = value;
			}
		}

		/// <summary>
		/// Gets or sets the size of the page.
		/// </summary>
		/// <value>
		/// The size of the page.
		/// </value>
		[Range(1, int.MaxValue)]
		public int? PageSize
		{
			get
			{
				return this.pageSize ?? MinPageSize;
			}

			set
			{
				this.pageSize = value;
			}
		}

		/// <summary>
		/// Gets or sets the sort by.
		/// </summary>
		/// <value>
		/// The sort by field name.
		/// </value>
		public string SortBy
		{
			get
			{
				return this.sortBy;
			}
			set
			{
				this.sortBy = value;
			}
		}


		public int TotalCount
		{
			get
			{
				return this.totalCount;
			}
			set
			{
				this.totalCount = value;
			}
		}

		/// <summary>
		/// Gets or sets the sort direction.
		/// </summary>
		/// <value>
		/// The sort direction.
		/// </value>
		public string SortDirection
		{
			get
			{
				return this.sortDirection;
			}
			set
			{
				this.sortDirection = value;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is page provided.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is page provided; otherwise, <c>false</c>.
		/// </value>
		public bool IsPageProvided
		{
			get
			{
				return !this.IsOffsetProvided && this.page.HasValue;
			}
		}

		#region Methods

		/// <summary>
		/// Gets the offset.
		/// </summary>
		/// <returns>the offset.</returns>
		public int? GetOffset()
		{
			return this.offset;
		}

		/// <summary>
		/// Gets the limit.
		/// </summary>
		/// <returns>the limit.</returns>
		public int? GetLimit()
		{
			return this.limit;
		}

		/// <summary>
		/// Gets the page.
		/// </summary>
		/// <returns>the page.</returns>
		public int? GetPage()
		{
			return this.page;
		}

		/// <summary>
		/// Gets the size of the page.
		/// </summary>
		/// <returns>the page size.</returns>
		public int? GetPageSize()
		{
			return this.pageSize;
		}

		#endregion
	}
}
