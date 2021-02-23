﻿using DataAccessLayer;
using DataAccessLayer.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.Repositories
{
    public class MessageRepository
    {
        private ChatContext _chatContext { get; set; } = new ChatContext();
        public async Task<Guid> CreateMessage(BaseMessage message)
        {
            message.GroupId = message.GroupId == 0 ? null : message.GroupId;
            await _chatContext.BaseMessages.AddAsync(message);
            await _chatContext.SaveChangesAsync();

            return message.Id;
        }

        public async Task DeleteMessage(BaseMessage user)
        {
            _chatContext.BaseMessages.Remove(user);
            await _chatContext.SaveChangesAsync();
        }

        public async Task<List<BaseMessage>> GetMessagesForGroup(long id)
        {
            var result = await _chatContext.BaseMessages.ToListAsync();
            await _chatContext.SaveChangesAsync();

            return id == 0 ? result : result.Where(w => w.GroupId == id).ToList();
        }

        public async Task UpdateMessage(BaseMessage message)
        {
            _chatContext.BaseMessages.Update(message);
            await _chatContext.SaveChangesAsync();
        }
    }
}