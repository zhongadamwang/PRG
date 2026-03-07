namespace Sanjel.RequestManagement.Repositories.Common;

/// <summary>
/// Represents a paged result set with entities and pagination metadata.
/// </summary>
/// <typeparam name="T">Entity type.</typeparam>
public class PagedResult<T>
{
	/// <summary>
	/// Gets or sets the entities in the current page.
	/// </summary>
	public IList<T> Items { get; set; } = new List<T>();

	/// <summary>
	/// Gets or sets total number of entities available (across all pages).
	/// </summary>
	public int TotalCount { get; set; }

	/// <summary>
	/// Gets or sets current page number (1-based).
	/// </summary>
	public int PageNumber { get; set; }

	/// <summary>
	/// Gets or sets number of items per page.
	/// </summary>
	public int PageSize { get; set; }

	/// <summary>
	/// Gets total number of pages available.
	/// </summary>
	public int TotalPages => this.PageSize > 0 ? (int)Math.Ceiling((double)this.TotalCount / this.PageSize) : 0;

	/// <summary>
	/// Gets a value indicating whether whether there is a previous page available.
	/// </summary>
	public bool HasPreviousPage => this.PageNumber > 1;

	/// <summary>
	/// Gets a value indicating whether whether there is a next page available.
	/// </summary>
	public bool HasNextPage => this.PageNumber < this.TotalPages;

	/// <summary>
	/// Gets index of the first item in the current page (1-based).
	/// </summary>
	public int StartIndex => this.PageSize > 0 ? ((this.PageNumber - 1) * this.PageSize) + 1 : 0;

	/// <summary>
	/// Gets index of the last item in the current page (1-based).
	/// </summary>
	public int EndIndex => Math.Min(this.StartIndex + this.PageSize - 1, this.TotalCount);
}
