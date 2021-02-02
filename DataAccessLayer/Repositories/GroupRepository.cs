using DataAccessLayer.Abstraction;
using DataAccessLayer.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private ChatContext _chatContext { get; set; } = new ChatContext();
        public async Task<long> CreateGroup(Group group)
        {
            await _chatContext.Groups.AddAsync(group);
            await _chatContext.SaveChangesAsync();

            return group.Id;
        }

        public async Task<List<Group>> GetGroups()
        {
            var result = await _chatContext.Groups.ToListAsync();
            await _chatContext.SaveChangesAsync();

            return result;
        }

        public async Task<Group> GetGroupByID(long ID)
        {
            var result = _chatContext.Groups.Where(w => w.Id == ID).FirstOrDefault();

            return result;
        }

        public async Task UpdateMessage(Group group)
        {
            _chatContext.Groups.Update(group);
            await _chatContext.SaveChangesAsync();
        }
    }
}
