using System;
using System.Collections.Generic;
using System.Text;

namespace Yongrong.Model.Int
{
    public class PageBean
    {
        /// <summary>
        /// 当前页编号 从1开始编号
        /// </summary>
        public int Index { get; set; } = 1;

        /// <summary>
        /// 单页记录数
        /// </summary>
        public int Size { get; set; } = 20;

        /// <summary>
        /// 总页数
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 记录总数
        /// </summary>
        public int Row { get; set; }

        public PageBean()
        {
        }

        public void CopyPage(PageBean rsp)
        {
            if (null != rsp)
            {
                rsp.Index = this.Index;
                rsp.Count = this.Count;
                rsp.Size = this.Size;
                rsp.Row = this.Row;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CheckPage()
        {
            if (0 >= this.Size)
            {
                this.Size = 20;
            }

            /*if (100 < this.Size)
            {
                this.Size = 100;
            }*/

            if (0 >= this.Index)
            {
                this.Index = 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SumPageCount()
        {
            CheckPage();

            this.Count = this.Row / this.Size;

            if (0 != (this.Row % this.Size))
            {
                this.Count++;
            }

            if (this.Index > this.Count)
            {
                this.Index = this.Count;
            }

            if (0 >= this.Index)
            {
                this.Index = 1;
            }
        }
    }
}
