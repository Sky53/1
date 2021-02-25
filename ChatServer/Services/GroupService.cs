using ChatServer.DataAccessLayer.Model;
using ChatServer.Repositories;
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
