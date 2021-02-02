using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.AbstractionServices
{
    public interface IGroupService
    {
        public Task GetGroups();
    }
}
