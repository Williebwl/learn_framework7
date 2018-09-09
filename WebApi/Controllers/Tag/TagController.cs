using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BIStudio.Framework;
using BIStudio.Framework.Data;
using BIStudio.Framework.Domain;
using BIStudio.Framework.UI.Models;
using BIStudio.Framework.Tag;

namespace WebApi.Controllers.Tag
{
    public partial class TagController
    {
        [HttpGet]
        public IList<TagVM> GetTagByTagClass(string tagClass)
        {
            var q = from d in _tagRepository.Entities
                    from b in _tagClassRepository.Entities
                    where b.ClassCode == tagClass
                    && d.TagClassID == b.ID
                    orderby d.Path, d.Sequence
                    select d;

            return q.Map<SYSTag, TagVM>().ToList();
        }

        [HttpGet]
        public IList<SmartTreeVM> GetModuleSmartTree(long id)
        {
            #region sql

            var sql = @"WITH    a AS ( SELECT   ID ,
                                                GroupCode AS TagCode ,
                                                GroupName AS TagName ,
                                                0 AS ParentID ,
                                                0 AS Layer ,
                                                RIGHT('00000000000000000' + CAST(ID AS VARCHAR(30)),
                                                      16) + ',' [PATH]
                                       FROM     SYSTagGroup tg
                                       WHERE    tg.ID = @GroupID
                                     ),
                                b AS ( SELECT   tc.ID ,
                                                tc.ClassCode ,
                                                tc.ClassName ,
                                                a.ID AS ParentID ,
                                                1 AS Layer ,
                                                a.[PATH] + RIGHT('00000000000000000'
                                                                 + CAST(tc.ID AS VARCHAR(30)), 16)
                                                + ',' [PATH]
                                       FROM     SYSTagClass tc ,
                                                a
                                       WHERE    tc.TagGroupID = a.ID
                                     ),
                                c AS ( SELECT   t.ID ,
                                                t.TagCode ,
                                                t.TagName ,
                                                b.ID AS ParentID ,
                                                2 AS Layer ,
                                                [PATH] = CAST(b.[PATH] + RIGHT('00000000000000000'
                                                                               + CAST(t.ID AS VARCHAR(30)),
                                                                               16) + ',' AS VARCHAR(150))
                                       FROM     SYSTag t ,
                                                b
                                       WHERE    t.TagClassID = b.ID
                                                AND t.ParentID = 0
                                       UNION ALL
                                       SELECT   tg.ID ,
                                                tg.TagCode ,
                                                tg.TagName ,
                                                tg.ParentID ,
                                                3 AS Layer ,
                                                [PATH] = CAST(c.[PATH] + RIGHT('00000000000000000'
                                                                               + CAST(tg.ID AS VARCHAR(30)),
                                                                               16) + ',' AS VARCHAR(150))
                                       FROM     SYSTag tg ,
                                                c
                                       WHERE    tg.ParentID = c.ID
                                     )
                            SELECT  *
                            FROM    a
                            UNION ALL
                            SELECT  *
                            FROM    b
                            UNION ALL
                            SELECT  *
                            FROM    c
                            ORDER BY [PATH]";

            #endregion sql

            var isfirst = true;
            return this._repository.GetAll(new Spec<SYSTag>(DBBuilder.Define(sql, new { GroupID = id }, null, null, false))).Select(d => new SmartTreeVM
            {
                Text = d.TagName,
                Value = d.ID,
                ID = d.ID,
                ParentID = d.ParentID,
                Path = d.Path,
                Layer = d.Layer,
                IsExpand = isfirst ? !(isfirst = d.Layer.GetValueOrDefault(0) != 0) : isfirst
            }).ToArray();
        }

        public override TagVM Post(TagVM vm)
        {
            SYSTag pInfo = null;

            try
            {
                if (vm.ParentID > 0) pInfo = _repository.Get(vm.ParentID.Value);

                var info = vm.Map<TagVM, SYSTag>();

                if (_repository.Add(info))
                {
                    if (pInfo != null && pInfo.ID > 0)
                    {
                        info.Layer = pInfo.Layer.Value + 1;
                        info.Path = pInfo.Path + info.ID.ToString() + ",";
                    }
                    else
                    {
                        info.Layer = 1;
                        info.Path = "0," + info.ID.ToString() + ",";
                    }

                    return _repository.Modify(info) ? _repository.Get(info.ID.Value).Map<SYSTag, TagVM>() : default(TagVM);
                }
            }
            catch { }

            return default(TagVM);
        }


        public override TagVM Put(long id, TagVM vm)
        {
            SYSTag pInfo = null;

            try
            {
                if (vm.ParentID > 0) pInfo = _repository.Get(vm.ParentID.Value);

                var info = vm.Map<TagVM, SYSTag>();
                info.ID = id;

                if (pInfo != null && pInfo.ID > 0)
                {
                    info.Layer = pInfo.Layer.Value + 1;
                    info.Path = pInfo.Path + info.ID.ToString() + ",";
                }
                else
                {
                    info.Layer = 1;
                    info.Path = "0," + info.ID.ToString() + ",";
                }

                return _repository.Modify(info) ? _repository.Get(info.ID.Value).Map<SYSTag, TagVM>() : default(TagVM);
            }
            catch { }

            return default(TagVM);
        }
    }
}