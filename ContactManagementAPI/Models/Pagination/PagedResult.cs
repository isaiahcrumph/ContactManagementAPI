namespace ContactManagementAPI.Models.Pagination
{
    /// <summary>
    /// Represents a paged result of items
    /// </summary>
    /// <typeparam name="T">The type of items in the result</typeparam>
    public class PagedResult<T>
    {
        /// <summary>
        /// The current page number (1-based)
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Number of items per page
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Total number of items across all pages
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Total number of pages
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// The items on the current page
        /// </summary>
        public IEnumerable<T> Data { get; set; }

        /// <summary>
        /// Indicates whether there is a previous page available
        /// </summary>
        public bool HasPrevious => PageNumber > 1;

        /// <summary>
        /// Indicates whether there is a next page available
        /// </summary>
        public bool HasNext => PageNumber < TotalPages;
    }
}