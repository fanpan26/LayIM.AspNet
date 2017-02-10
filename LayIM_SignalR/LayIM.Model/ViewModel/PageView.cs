using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.Model.ViewModel
{
    /// <summary>
    /// 分页结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageView<T>
    {

        public PageView()
        {
            
        }
        public PageView(int pageIndex,int pageSize) : base()
        {
            _pageIndex = pageIndex > 0 ? pageIndex : 1;
            _pageSize = pageSize > 0 ? pageSize : 20;
        }

        private int _pageIndex;
        private int _pageSize;
        private int _totalPageCount = 0;
        private int _totalCount = 0;
        public int TotalCount
        {
            get { return _totalCount; }
            set
            {
                _totalCount = value;
                _totalPageCount = Convert.ToInt32(Math.Ceiling(TotalCount * 1.0 / PageSize));
                _pageIndex = _pageIndex > _totalPageCount ? _totalPageCount : _pageIndex;
            }
        }
        public int PageIndex
        {
            get
            {
                return _pageIndex;
            }
            set { _pageIndex = value; }
        }
        public int PageSize { get { return _pageSize; } set { _pageSize = value; } }
        public int TotalPageCount
        {
            get
            {
                return _totalPageCount;
            }
        }
        public IEnumerable<T> List { get; set; }
    }
}
