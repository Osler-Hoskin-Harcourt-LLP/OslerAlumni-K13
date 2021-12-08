using CMS.DocumentEngine;

namespace ECA.Content.Extensions
{
    public static class TreeNodeExtensions
    {
        public static T ToPageType<T>(
            this TreeNode page)
            where T : TreeNode, new()
        {
            if (page == null)
            {
                return null;
            }

            if (typeof(T) == page.GetType())
            {
                return (T)page;
            }

            var ds = page.GetDataSet();

            if ((ds == null) || (ds.Tables.Count < 1))
            {
                return null;
            }

            var dt = ds.Tables[0];

            if (dt.Rows.Count < 1)
            {
                return null;
            }

            return TreeNode.New<T>(
                page.NodeClassName,
                dt.Rows[0]);
        }
    }
}
