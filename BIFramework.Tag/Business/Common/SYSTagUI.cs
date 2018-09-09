using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BIStudio.Framework.Tag
{
    public static class SYSTagUI
    {
        //#region 绑定控件
        ///// <summary>
        ///// 根据TagClassCode绑定ListControl
        ///// </summary>
        ///// <param name="lc"></param>
        ///// <param name="tagClassCode"></param>
        //public static void BindByTag(this ListControl lc, string tagClassCode)
        //{
        //    ITag t = TagProvider.GetInstance();
        //    lc.DataSource = t.GetTagsByClassCode(tagClassCode);
        //    lc.DataTextField = "TagName";
        //    lc.DataValueField = "ID";
        //    lc.DataBind();
        //}
        ///// <summary>
        ///// 根据TagClassCode绑定ListControl设置默认值
        ///// </summary>
        ///// <param name="lc"></param>
        ///// <param name="tagClassCode"></param>
        ///// <param name="setEmptyItem"></param>
        //public static void BindByTag(this ListControl lc, string tagClassCode, string setEmptyItem)
        //{
        //    ITag t = TagProvider.GetInstance();
        //    lc.DataSource = t.GetTagsByClassCode(tagClassCode);
        //    lc.DataTextField = "TagName";
        //    lc.DataValueField = "ID";
        //    lc.DataBind();
        //    if (setEmptyItem != null)
        //        lc.Items.Insert(0, new ListItem(setEmptyItem, ""));
        //}
        //#endregion
    }
}
