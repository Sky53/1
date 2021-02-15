using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.AbstractionServices
{
    public interface IGroupService
    {
        public Task<List<Group>> GetGroups();
    }
}
