﻿using ChatServer.DataAccessLayer.Model;
using ChatServer.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 using ChatServer.Exceptions;

 namespace ChatServer.DataAccessLayer.Repositories
{
    public class UserRepository
    {
        private ChatContext _chatContext { get; set; } = new ChatContext();

        public async Task<UserDto> GetUserByNameAndPassword(Message<AuthMessage> authorizationMessage)
        {
            try
            {
                var user = await _chatContext.Users
                    .Include(i => i.Groups)
                    .FirstOrDefaultAsync(w => w.Name == authorizationMessage.Body.Login && w.Pass == authorizationMessage.Body.Pass);
               
                if (user == null)
                {
                    throw new UserNotFoundException("User not founded");
                }
                var userGroupId = user.Groups.FirstOrDefault()?.Id;
                if (userGroupId != 0)
                {
                    if (userGroupId != authorizationMessage.GroupId)
                    {
                        var oldGroup = await _chatContext.Groups.FindAsync(userGroupId);
                        if (oldGroup != null)
                            user.Groups.Remove(oldGroup);
                        var newGroup = await _chatContext.Groups.FindAsync(authorizationMessage.GroupId);
                        if (newGroup != null)
                            user.Groups.Add(newGroup);
                        await _chatContext.SaveChangesAsync();
                    }
                }
                
                var message = _chatContext.BaseMessages.Where(w => w.Type == 2 && w.UserId == user.Id).ToList();
                return new UserDto
                {
                    Id = user.Id,
                    GroupId = user.Groups.FirstOrDefault()?.Id,
                    Name = user.Name,
                    Messages = message.OrderByDescending(x => x.CreateDate).Skip(0).Take(10).Select(s => s.Body).ToList()
                };
            }
            catch 
            {
                throw;
            }
        }
    }
}
