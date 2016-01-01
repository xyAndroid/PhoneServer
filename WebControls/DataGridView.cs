using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eSchool.Web.UI.WebControls
{
    /// <summary>
    /// 提供对 GridView 功能扩展的控件。
    /// </summary>
    [ToolboxData("<{0}:DataGridView runat=server></{0}:DataGridView>")]
    public class DataGridView : System.Web.UI.WebControls.GridView
    {
        private Dictionary<string, int> _colsOrders = null;

        /// <summary>
        /// 设置数据列的顺序。
        /// </summary>
        /// <param name="orders">表示数据列名称和顺序号的字典。</param>
        public virtual void SetColumnsOrder(Dictionary<string, int> orders)
        {
            _colsOrders = orders;
        }

        /// <summary>
        /// 创建用来构建控件层次结构的列字段集。
        /// </summary>
        /// <param name="dataSource">数据源。</param>
        /// <param name="useDataSource">指示是否使用数据源。</param>
        /// <returns>列字段集。</returns>
        protected override System.Collections.ICollection CreateColumns(PagedDataSource dataSource, bool useDataSource)
        {
            System.Collections.ICollection cols =
                base.CreateColumns(dataSource, useDataSource);

            if (_colsOrders == null)
            {
                return cols;
            }

            List<DataControlField> tmp =
                cols.Cast<DataControlField>().ToList();

            List<DataControlField> r = new List<DataControlField>();

            foreach (var entry in _colsOrders.OrderBy(p => p.Value))
            {
                if (entry.Value < 0) continue;

                r.Add(
                    tmp.First(p => p.HeaderText.Equals(entry.Key)));
            }

            foreach (var f in tmp)
            {
                if (r.Exists(p => p.HeaderText.Equals(f.HeaderText))) continue;

                r.Add(f);
            }

            return r;
        }
    }
}
