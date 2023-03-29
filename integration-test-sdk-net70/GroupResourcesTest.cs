﻿using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace integration_test_sdk_net70
{
    [TestClass]
    public class GroupResourcesTest
    {
        [TestMethod]
        public void TestGroupResources()
        {
            SmartsheetClient smartsheet = new SmartsheetBuilder().SetMaxRetryTimeout(30000).Build();
            
            long groupId = CreateGroup(smartsheet);

            UpdateGroup(smartsheet, groupId);

            AddGroupMember(smartsheet, groupId);

            GetGroup(smartsheet, groupId);

            ListGroups(smartsheet);

            DeleteGroup(smartsheet, groupId);
        }

        private static void AddGroupMember(SmartsheetClient smartsheet, long groupId)
        {
            GroupMember member = new GroupMember.AddGroupMemberBuilder("eric.yan@smartsheet.com").Build();
            IList<GroupMember> members = smartsheet.GroupResources.AddGroupMembers(groupId, new GroupMember[] { member });
            Assert.IsTrue(members.Count == 1);
            Assert.IsTrue(members[0].Email == "eric.yan@smartsheet.com");
        }

        private static void DeleteGroup(SmartsheetClient smartsheet, long groupId)
        {
            smartsheet.GroupResources.DeleteGroup(groupId);
            try
            {
                smartsheet.GroupResources.GetGroup(groupId);
                Assert.Fail("Cannot get a deleted group");
            }
            catch
            {
                //Not found.
            }
        }

        private static void ListGroups(SmartsheetClient smartsheet)
        {
            PaginatedResult<Group> groups = smartsheet.GroupResources.ListGroups(null);
            // Don't actually know that there is only one group if its a test account
            // Assert.IsTrue(groups.Data.Count == 1);
            bool found = false;
            foreach(Group group in groups.Data)
            {
                if(group.Name == "a group")
                {
                    found = true;
                    break;
                }

            }
            Assert.IsTrue(found);
        }

        private static void GetGroup(SmartsheetClient smartsheet, long groupId)
        {
            Group group = smartsheet.GroupResources.GetGroup(groupId);
            Assert.IsTrue(group.Name == "a group");
            Assert.IsTrue(group.Description == "updated desc");
        }

        private static void UpdateGroup(SmartsheetClient smartsheet, long groupId)
        {
            Group group = smartsheet.GroupResources.UpdateGroup(new Group.UpdateGroupBuilder(groupId).SetDescription("updated desc").Build());

            Assert.IsTrue(group.Name == "a group");
            Assert.IsTrue(group.Description == "updated desc");
        }

        private static long CreateGroup(SmartsheetClient smartsheet)
        {
            GroupMember member = new GroupMember.AddGroupMemberBuilder("aditi.nioding@smartsheet.com").Build();

            Group group = smartsheet.GroupResources.CreateGroup(new Group.CreateGroupBuilder("a group", "this is a group").SetMembers(new GroupMember[] { member }).Build());

            Assert.IsTrue(group.Name == "a group");
            return group.Id.Value;
        }
    }
}