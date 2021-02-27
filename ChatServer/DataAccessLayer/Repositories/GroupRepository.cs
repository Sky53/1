using ChatServer.DataAccessLayer.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.DataAccessLayer.Repositories
{
    public class GroupRepository
    {
        private ChatContext _chatContext { get; set; } = new ChatContext();
        public async Task<long> CreateGroup(Group group)
        {
            await _chatContext.Groups.AddAsync(group);
            await _chatContext.SaveChangesAsync();

            return group.Id;
        }
    }
}
