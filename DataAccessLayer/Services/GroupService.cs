using DataAccessLayer.AbstractionServices;
using DataAccessLayer.Model;
using DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Services
{
    public class GroupService : IGroupService
    {
        public readonly GroupRepository groupRepository = new GroupRepository();
        public async Task<List<Group>> GetGroups()
        {
            var result = await groupRepository.GetGroups();

            return result;
        }
    }
}
