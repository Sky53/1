using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstraction
{
    public interface IGroupRepository
    {
        public Task<Group> GetGroupByID(long ID);
        public Task<List<Group>> GetGroups();
        public Task<long> CreateGroup(Group group);
        public Task UpdateMessage(Group group);
    }
}
