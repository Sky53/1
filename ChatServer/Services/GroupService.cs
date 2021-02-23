using ChatServer.Repositories;
using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.Services
{
    public class GroupService
    {
        public readonly GroupRepository groupRepository = new GroupRepository();
        public async Task<List<Group>> GetGroups()
        {
            var result = await groupRepository.GetGroups();

            return result;
        }
    }
}
