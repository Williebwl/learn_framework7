using System;
using System.Collections.Generic;
using System.Linq;
using BIStudio.Framework.Domain;

using BIStudio.Framework.Tag;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BIStudio.Framework;

namespace BIFramework.Test
{
    [TestClass]
    public class TagTests
    {
        private ITag _tagService = TagService.Default;

        #region 附加测试特性

        //
        // 编写测试时，可以使用以下附加特性: 
        //
        //在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{

        //}
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //

        #endregion


        [TestMethod]
        public void FindBySpecificationTest()
        {
            var tagService = TagService.GetInstance(new Spec<Entity>(item => item is SYSTag && !string.IsNullOrEmpty((item as SYSTag).TagName)));
            AssertExtend.IsNotEmpty(tagService.FindBySpecification(_tagService.GetTags()));
        }

        [TestMethod]
        public void FindBySpecificationTest1()
        {
            var tagService = TagService.GetInstance(new Spec<Entity>(item => item is SYSTag && !string.IsNullOrEmpty((item as SYSTag).TagName)));
            Assert.IsNotNull(tagService.FindBySpecification(_tagService.GetTags().First()));
        }

        [TestMethod]
        public void GetTagDisplayLevelTest()
        {
            var item = _tagService.GetTagDisplayLevel(2);
            Assert.IsNotNull(item);
        }

        [TestMethod]
        public void GetTagNodesByGroupCodeTest()
        {
            var group = _tagService.GetTagGroups().First();

            var items = _tagService.GetTagNodesByGroupCode(group.GroupCode);
            AssertExtend.IsNotEmpty(items);

            items = _tagService.GetTagNodesByGroupCode(group.GroupCode, EnumSYSTagNodeType.TagGroup);
            AssertExtend.IsNotEmpty(items);
            items = _tagService.GetTagNodesByGroupCode(group.GroupCode, EnumSYSTagNodeType.TagGroup, true);
            AssertExtend.IsNotEmpty(items);
            items = _tagService.GetTagNodesByGroupCode(group.GroupCode, EnumSYSTagNodeType.TagGroup, false);
            AssertExtend.IsNotEmpty(items);
            items = _tagService.GetTagNodesByGroupCode(group.GroupCode, EnumSYSTagNodeType.TagGroup, false, 0);
            AssertExtend.IsNotEmpty(items);
        }

        [TestMethod]
        public void GetTagGroupsTest()
        {
            var groups = _tagService.GetTagGroups();
            AssertExtend.IsNotEmpty(groups);

            var group = groups.FirstOrDefault();
            Assert.IsNotNull(group);
            Assert.IsNotNull(group.ID);
            _tagService.GetTagGroup(group.ID.Value);
            _tagService.GetTagGroup(group.GroupCode);
        }

        [TestMethod]
        public void GetTagClassesTest()
        {
            var classes = _tagService.GetTagClasses();
            AssertExtend.IsNotEmpty(classes);
            var @class = classes.FirstOrDefault();
            Assert.IsNotNull(@class);
            Assert.IsNotNull(@class.ID);
            Assert.IsNotNull(_tagService.GetTagClass(@class.ID.Value));
            Assert.IsNotNull(_tagService.GetTagClass(@class.ClassCode));
            Assert.IsNotNull(_tagService.GetTagClass(new SYSTagClass { ClassCode = @class.ClassCode, ID = @class.ID }));
            AssertExtend.IsNotEmpty(_tagService.GetTagClasses(classes.Select(item => item.ID.Value).ToList()));
            var group = _tagService.GetTagGroup(@class.AppID.Value);
            Assert.IsNotNull(group);
            AssertExtend.IsNotEmpty(_tagService.GetTagClassesByGroupCode(group.GroupCode));
            AssertExtend.IsNotEmpty(_tagService.GetTagClassesByGroupCode(group.GroupCode, null));
        }

        [TestMethod]
        public void GetTagsTest()
        {
            var tags = _tagService.GetTags();

            AssertExtend.IsNotEmpty(tags);

            AssertExtend.IsNotEmpty(_tagService.GetTags(tags.Select(item => item.ID.Value)));
        }

        [TestMethod]
        public void GetTagsByParentIDTest()
        {
            var tags = _tagService.GetTags();
            AssertExtend.IsNotEmpty(tags);
            var tag = tags.FirstOrDefault();
            Assert.IsNotNull(tag);
            AssertExtend.IsNotEmpty(_tagService.GetTagsByParentID(tag.ParentID.Value));
        }

        [TestMethod]
        public void GetTagsByGroupCodeTest()
        {
            var groups = _tagService.GetTagGroups();
            AssertExtend.IsNotEmpty(groups);
            var tag = _tagService.GetTags().FirstOrDefault();
            var @class = _tagService.GetTagClass(tag.TagClassID.Value);
            var group = groups.First(item => item.ID == @class.AppID);

            AssertExtend.IsNotEmpty(_tagService.GetTagsByGroupCode(group.GroupCode));
            AssertExtend.IsNotEmpty(_tagService.GetTagsByGroupCode(group.GroupCode, null)); ;
        }

        [TestMethod]
        public void GetTagsByClassIDTest()
        {
            var classes = _tagService.GetTagClasses();
            AssertExtend.IsNotEmpty(classes);
            var tag = _tagService.GetTags().FirstOrDefault();
            var @class = classes.FirstOrDefault(item => item.ID == tag.TagClassID);
            AssertExtend.IsNotEmpty(_tagService.GetTagsByClassID(@class.ID.Value));
            AssertExtend.IsNotEmpty(_tagService.GetTagsByClassID(@class.ID.Value, 0));
            AssertExtend.IsNotEmpty(_tagService.GetTagsByClassID(60, "CMSArticle"));
        }

        [TestMethod]
        public void GetTagsByClassCodeTest()
        {
            var classes = _tagService.GetTagClasses();
            AssertExtend.IsNotEmpty(classes);
            var tag = _tagService.GetTags().FirstOrDefault();
            var @class = classes.FirstOrDefault(item => item.ID == tag.TagClassID);
            AssertExtend.IsNotEmpty(_tagService.GetTagsByClassCode(@class.ClassCode));
            AssertExtend.IsNotEmpty(_tagService.GetTagsByClassCode(@class.ClassCode, 0));
            AssertExtend.IsNotEmpty(_tagService.GetTagsByClassCode("Common_TZLX", "CMSArticle"));
        }

        [TestMethod]
        public void GetTagTest()
        {
            var tags = _tagService.GetTags();

            AssertExtend.IsNotEmpty(tags);

            Assert.IsNotNull(_tagService.GetTag(tags.First().ID.Value));
        }

        [TestMethod]
        public void GetTagByNameTest()
        {
            var tags = _tagService.GetTags();
            AssertExtend.IsNotEmpty(tags);
            var tag = tags.FirstOrDefault(item => !string.IsNullOrEmpty(item.TagName));
            if (tag == null)
                tag = tags.First();
            Assert.IsNotNull(_tagService.GetTagByName(tag.TagClassID.Value, tag.TagName));
            var @class = _tagService.GetTagClass(tag.TagClassID.Value);
            Assert.IsNotNull(@class);
            Assert.IsNotNull(_tagService.GetTagByName(@class.ClassCode, tag.TagName));
        }

        [TestMethod]
        public void GetTagByCodeTest()
        {
            var tags = _tagService.GetTags();
            AssertExtend.IsNotEmpty(tags);
            var tag = tags.FirstOrDefault(item => !string.IsNullOrEmpty(item.TagCode));
            if (tag == null)
                tag = tags.First();
            Assert.IsNotNull(_tagService.GetTagByCode(tag.TagClassID.Value, tag.TagCode));
            var @class = _tagService.GetTagClass(tag.TagClassID.Value);
            Assert.IsNotNull(@class);
            Assert.IsNotNull(_tagService.GetTagByCode(@class.ClassCode, tag.TagCode));
        }

        [TestMethod]
        public void GetTagByValueTest()
        {
            var tags = _tagService.GetTags();
            AssertExtend.IsNotEmpty(tags);
            var tag = tags.FirstOrDefault(item => !string.IsNullOrEmpty(item.TagValue));
            if (tag == null)
                tag = tags.First();
            Assert.IsNotNull(_tagService.GetTagByValue(tag.TagClassID.Value, tag.TagValue));
            var @class = _tagService.GetTagClass(tag.TagClassID.Value);
            Assert.IsNotNull(@class);
            Assert.IsNotNull(_tagService.GetTagByValue(@class.ClassCode, tag.TagValue));
        }

        [TestMethod]
        public void GetTagLogsTest()
        {
            AssertExtend.IsNotEmpty(_tagService.GetTagLogs(new SYSTagLogs { TagName = "测试Name" }));
        }

        [TestMethod]
        public void GetTagLogsByTagTest()
        {
            AssertExtend.IsNotEmpty(_tagService.GetTagLogs(new SYSTag { TagName = "测试Name" }, new TCTest() { ID = 1503170947177120908 }));
        }

        [TestMethod]
        public void GetTagLogsTest2()
        {
            AssertExtend.IsNotEmpty(_tagService.GetTagLogs(new SYSTagClass() { ClassName = "手机" }, new TCTest() { ID = 1503170947177120908 }));
        }

        [TestMethod]
        public void GetTagAuthorityTest()
        {
            Assert.IsNotNull(_tagService.GetTagAuthority(7));
        }

        [TestMethod]
        public void GetTagAuthoritiesTest()
        {
            AssertExtend.IsNotEmpty(_tagService.GetTagAuthorities(new SYSTagAuthority { ID = 7 }));
        }

        [TestMethod]
        public void SetTagAuthorityTest()
        {
            var node = _tagService.GetTagNodesByGroupCode(_tagService.GetTagGroups().First().GroupCode).FirstOrDefault();
            Assert.IsNotNull(node);
            var tagAuthority = new SYSTagConferAuthorityDTO()
            {
                TargetType = EnumSYSTagAuthorityType.Dept,
                AuthorityOperate = EnumSYSTagOperate.Delete,
                AuthorityRange = EnumSYSTagRange.Current,
                Remark = "测试TagConferAuthority",
                TagDisplayLevel = EnumSYSTagDisplayLevel.System,
                TagNode = node,
                TargetContent = new List<SYSTagAuthorityDTO> { new SYSTagAuthorityDTO { AuthorityText = "测试", AuthorityValue = 0 } }

            };
            _tagService.SetTagAuthority(tagAuthority);
        }

        [TestMethod]
        public void SetTagAuthorityTest1()
        {
            var node = _tagService.GetTagNodesByGroupCode(_tagService.GetTagGroups().First().GroupCode).FirstOrDefault();
            Assert.IsNotNull(node);
            var tagAuthority = new SYSTagConferAuthorityDTO()
            {
                TargetType = EnumSYSTagAuthorityType.Dept,
                AuthorityOperate = EnumSYSTagOperate.Delete,
                AuthorityRange = EnumSYSTagRange.Current,
                Remark = "测试TagConferAuthority",
                TagDisplayLevel = EnumSYSTagDisplayLevel.System,
                TagNode = node,
                TargetContent = new List<SYSTagAuthorityDTO> { new SYSTagAuthorityDTO { AuthorityText = "测试", AuthorityValue = 0 } }

            };
            _tagService.SetTagAuthority(tagAuthority, false);
        }

        [TestMethod]
        public void FixBrokenNodesTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void FixBrokenNodesTest1()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void FixBrokenNodesTest2()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void FixBrokenNodesTest3()
        {
            throw new NotImplementedException();
        }


        [TestMethod]
        public void SaveTagTest()
        {
            var classes = _tagService.GetTagClasses();
            AssertExtend.IsNotEmpty(classes);
            var @class = classes.FirstOrDefault();
            Assert.IsNotNull(@class);
            var tag = new SYSTag
            {
                TagClass = @class.ClassName,
                TagClassID = @class.ID,
                TagName = "测试Name",
                TagCode = "测试Code",
                TagValue = "测试Value",
                ParentID = 0,
            };
            _tagService.Save(tag);
            Assert.IsNotNull(_tagService.GetTag(tag.ID.Value));
            using (var dbcontext = BoundedContext.Create())
            {
                _tagService = TagService.GetInstance();
                _tagService.Delete(tag);
                dbcontext.Commit();

            }
            Assert.IsNull(_tagService.GetTag(tag.ID.Value).ID);
        }

        [TestMethod]
        public void SaveTagClassTest()
        {
            var group = _tagService.GetTagGroups().FirstOrDefault();
            Assert.IsNotNull(group);
            var @class = new SYSTagClass()
            {
                ClassName = "测试ClassName",
                ClassCode = "测试ClassCode",
                AppID = group.ID,
            };
            _tagService.Save(@class);
            Assert.IsNotNull(_tagService.GetTagClass(@class.ID.Value));
            _tagService.Delete(@class);
            Assert.IsNull(_tagService.GetTagClass(@class.ID.Value).ID);
        }

        [TestMethod]
        public void SaveTagGroupTest()
        {
            var group = new SYSTagGroup()
            {
                GroupName = "测试GroupName",
                GroupCode = "测试GroupCode",
            };

            _tagService.Save(group);

            Assert.IsNull(_tagService.GetTagGroup("测试GroupName"));
        }

        [TestMethod]
        public void SaveTagAuthorityTest()
        {
            var item = _tagService.GetTagAuthorities(new SYSTagAuthority { ID = 7 }).First();
            item.ID = null;
            _tagService.Save(item);
            Assert.IsNotNull(_tagService.GetTagAuthority(item.ID.Value));
            _tagService.Delete(item);
            Assert.IsNull(_tagService.GetTagAuthority(item.ID.Value).ID);
        }

        [TestMethod]
        public void SaveTagLogTest()
        {
            var target = _tagService.GetTagApplyInfos(new TCTest { ID = 1503170947177120908 }).First();

            var log = new SYSTagLogs
            {
                TagClassID = target.TagClassID,
                TagID = target.ID,
                TagName = target.TagName,
                TagClass = target.TagClass,
                Inputer = "测试",
                InputTime = DateTime.Now,
                InputIP = "192.168.1.107",
                TargetObjectID = 1503170947177120908,
                TargetObject = "张三",
            };
            _tagService.Save(new[] { log });
        }

        [TestMethod]
        public void UpdateTagSequenceTest()
        {
            _tagService.UpdateTagSequence(_tagService.GetTags().Take(20)
                 .ToDictionary(item => item.ID.HasValue ? item.ID.Value : 0,
                 item => item.Sequence.HasValue ? item.Sequence.Value : 0)); ;
        }

        [TestMethod]
        public void UpdateTagClassSequenceTest()
        {
            _tagService.UpdateTagClassSequence(
                _tagService.GetTagClasses().Take(20)
                .ToDictionary(item => item.ID.HasValue ? item.ID.Value : 0,
                item => item.Sequence.HasValue ? item.Sequence.Value : 0)); ;
        }

        [TestMethod]
        public void UpdateTagGroupSequenceTest()
        {
            _tagService.UpdateTagGroupSequence(_tagService.GetTagGroups().Take(20)
                .ToDictionary(item => item.ID.HasValue ? item.ID.Value : 0,
                item => item.Sequence.HasValue ? item.Sequence.Value : 0)); ;
        }

        [TestMethod]
        public void ApplyTest()
        {
            var info = new TCTest { Name = "张三" };
            using (var dbContext = BoundedContext.Create())
            {
                _tagService = TagService.GetInstance();
                var testBo = dbContext.Resolve<TCTestDapperBO>();
                //使用Domain.Repository插入数据
                testBo.Add(info);
                var classes = _tagService.GetTagClasses();
                AssertExtend.IsNotEmpty(classes);
                var @class = classes.FirstOrDefault();
                Assert.IsNotNull(@class);

                var tag = new SYSTag
                {
                    TagClass = @class.ClassName,
                    TagClassID = @class.ID,
                    TagName = "测试Name",
                    TagCode = "测试Code",
                    TagValue = "测试Value",
                    ParentID = 0,
                };
                _tagService.Save(tag);
                _tagService.Apply(tag, info);
                dbContext.Commit();
            }
            AssertExtend.IsNotEmpty(_tagService.GetTagApplyInfos(info));
        }

        [TestMethod]
        public void ApplyListTagTest()
        {
            var info = new TCTest { Name = "张三" };
            using (var dbContext = BoundedContext.Create())
            {
                _tagService = TagService.GetInstance();
                var testBo = dbContext.Resolve<TCTestDapperBO>();
                //使用Domain.Repository插入数据
                testBo.Add(info);
                var classes = _tagService.GetTagClasses();
                AssertExtend.IsNotEmpty(classes);
                var @class = classes.FirstOrDefault();
                Assert.IsNotNull(@class);

                var tagsList = new List<SYSTag>();

                for (int i = 0; i < 10; i++)
                {
                    var tag = new SYSTag
                    {
                        TagClass = @class.ClassName,
                        TagClassID = @class.ID,
                        TagName = "测试Name" + i,
                        TagCode = "测试Code" + i,
                        TagValue = "测试Value" + i,
                        ParentID = 0,
                    };
                    _tagService.Save(tag);
                    tagsList.Add(tag);
                }
                _tagService.Apply(tagsList, info);
                dbContext.Commit();
            }
            AssertExtend.IsNotEmpty(_tagService.GetTagApplyInfos(info));
        }

        [TestMethod]
        public void ApplyIdListTest()
        {
            var info = new TCTest { Name = "张三" };
            using (var dbContext = BoundedContext.Create())
            {
                _tagService = TagService.GetInstance();
                var testBo = dbContext.Resolve<TCTestDapperBO>();
                //使用Domain.Repository插入数据
                testBo.Add(info);
                var classes = _tagService.GetTagClasses();
                AssertExtend.IsNotEmpty(classes);
                var @class = classes.FirstOrDefault();
                Assert.IsNotNull(@class);

                var tagsList = new List<SYSTag>();

                for (int i = 0; i < 10; i++)
                {
                    var tag = new SYSTag
                    {
                        TagClass = @class.ClassName,
                        TagClassID = @class.ID,
                        TagName = "测试Name" + i,
                        TagCode = "测试Code" + i,
                        TagValue = "测试Value" + i,
                        ParentID = 0,
                    };
                    _tagService.Save(tag);
                    tagsList.Add(tag);
                }
                _tagService.Apply(tagsList.Select(item => item.ID.Value), info);
            }
        }

        [TestMethod]
        public void ApplyByEntityListTest()
        {
            var infos = new List<TCTest>();
            for (int i = 0; i < 5; i++)
            {
                infos.Add(new TCTest { Name = "张三" + i });

            }
            using (var dbContext = BoundedContext.Create())
            {
                _tagService = TagService.GetInstance();
                var testBo = dbContext.Resolve<TCTestDapperBO>();
                //使用Domain.Repository插入数据
                infos.ForEach(item => testBo.Add(item));
                var classes = _tagService.GetTagClasses();
                AssertExtend.IsNotEmpty(classes);
                var @class = classes.FirstOrDefault();
                Assert.IsNotNull(@class);

                var tag = new SYSTag
                {
                    TagClass = @class.ClassName,
                    TagClassID = @class.ID,
                    TagName = "测试Name",
                    TagCode = "测试Code",
                    TagValue = "测试Value",
                    ParentID = 0,
                };
                _tagService.Save(tag);
                _tagService.Apply(tag, infos);
                dbContext.Commit();
            }
            AssertExtend.IsNotEmpty(_tagService.GetTagApplyInfos(infos.First()));
        }

        [TestMethod]
        public void UnApplyTest()
        {
            var info = new TCTest { Name = "张三" };
            var classes = _tagService.GetTagClasses();
            AssertExtend.IsNotEmpty(classes);
            var @class = classes.FirstOrDefault();
            Assert.IsNotNull(@class);
            var tag = new SYSTag
            {
                TagClass = @class.ClassName,
                TagClassID = @class.ID,
                TagName = "测试Name",
                TagCode = "测试Code",
                TagValue = "测试Value",
                ParentID = 0,
            };
            using (var dbContext = BoundedContext.Create())
            {
                _tagService = TagService.GetInstance();
                var testBo = dbContext.Resolve<TCTestDapperBO>();
                //使用Domain.Repository插入数据
                testBo.Add(info);

                _tagService.Save(tag);
                _tagService.Apply(tag, info);
                dbContext.Commit();
            }
            AssertExtend.IsNotEmpty(_tagService.GetTagApplyInfos(info));
            _tagService = TagService.GetInstance();
            _tagService.UnApply(info, tag);
        }

        [TestMethod]
        public void UnApplyByClassInfoTest()
        {
            ApplyTest();
            var classes = _tagService.GetTagClasses();
            AssertExtend.IsNotEmpty(classes);
            var @class = classes.FirstOrDefault();
            var info = new TCTest { Name = "张三" };
            _tagService.UnApply(info, @class);

            AssertExtend.IsNullOrEmpty(_tagService.GetTagApplyInfos(info, @class)); ;
        }

        [TestMethod]
        public void UnApplyByIdListTest()
        {
            ApplyTest();
            var classes = _tagService.GetTagClasses();
            AssertExtend.IsNotEmpty(classes);
            var @class = classes.FirstOrDefault();
            var info = new TCTest { Name = "张三" };
            _tagService.UnApply(info, new List<long> { @class.ID.Value });

            AssertExtend.IsNullOrEmpty(_tagService.GetTagApplyInfos(info, @class)); ;
            AssertExtend.IsNullOrEmpty(_tagService.GetTagApplyInfos(info, _tagService.GetTagGroup(@class.AppID.Value)));
        }

        [TestMethod]
        public void UnApplyByFroupTest()
        {
            ApplyTest();
            var classes = _tagService.GetTagClasses();
            AssertExtend.IsNotEmpty(classes);
            var @class = classes.FirstOrDefault();
            var info = new TCTest { Name = "张三" };
            var group = _tagService.GetTagGroup(@class.AppID.Value);
            _tagService.UnApply(info, group);
            AssertExtend.IsNullOrEmpty(_tagService.GetTagApplyInfos(info, group));
        }

        [TestMethod]
        public void UnApplyByEntityTest()
        {
            var info = new TCTest { Name = "张三" };
            _tagService.UnApply(info);
            AssertExtend.IsNullOrEmpty(_tagService.GetTagApplies(info));
        }

        [TestMethod]
        public void CreateSearchTest()
        {
            Assert.IsNotNull(_tagService.CreateSearch("TCTest")); ;
        }

        [TestMethod]
        public void CreateSearchByIdsTest()
        {
            Assert.IsNotNull(_tagService.CreateSearch("TCTest", _tagService.GetTags().Select(item => item.ID.Value).ToArray())); ;
        }

        [TestMethod]
        public void CreateSearchByMathcTypeTest()
        {
            Assert.IsNotNull(_tagService.CreateSearch("TCTest", EnumSYSTagSearch.Fuzzy, _tagService.GetTags().Select(item => item.ID.Value).ToArray())); ;
        }

        [TestMethod]
        public void CreateTargetTest()
        {
            Assert.IsNotNull(_tagService.CreateTarget("TCTest")); ;
        }

        [TestMethod]
        public void GetSpecificationTest()
        {
            Assert.IsNotNull(_tagService.GetSpecification<TCTest>(EnumSYSTagOperate.Create));
        }
        
    }
}