namespace OnePrivateNavigation.Common.Models
{
    /// <summary>
    /// 分页请求基类
    /// </summary>
    public class PagedRequest
    {
        private int _pageSize = 10;
        private int _pageIndex = 1;

        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value <= 0 ? 10 : value;
        }

        /// <summary>
        /// 页码（从1开始）
        /// </summary>
        public int PageIndex
        {
            get => _pageIndex;
            set => _pageIndex = value <= 0 ? 1 : value;
        }

        /// <summary>
        /// 跳过的记录数
        /// </summary>
        public int Skip => (PageIndex - 1) * PageSize;

        /// <summary>
        /// 排序字段
        /// </summary>
        public string? OrderBy { get; set; }

        /// <summary>
        /// 是否降序
        /// </summary>
        public bool IsDescending { get; set; }

        /// <summary>
        /// 搜索关键词
        /// </summary>
        public string? Keyword { get; set; }
    }
}