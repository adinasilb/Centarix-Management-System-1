using Microsoft.AspNetCore.Identity;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.AppData;
using System.Linq.Expressions;

namespace PrototypeWithAuth.CRUD
{
    public class NotificationsBaseProc<T1, T2> : ApplicationDbContextProc<T1> where T1:Notification<T2> where T2:NotificationStatus
    {
        public NotificationsBaseProc(ApplicationDbContext context, bool FromBase = false) : base(context)
        {
            if (!FromBase)
            {
                base.InstantiateProcs();
            }
        }

        
        public virtual async Task CreateWithoutTransactionAsync(T1 notification)
        {
            _context.Entry(notification).State = EntityState.Added;
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteWithoutTransactionAsync(List<T1> notifications)
        {
            foreach (var notification in notifications)
            {
                _context.Remove(notification);
            }
            await _context.SaveChangesAsync();          
        }


        public virtual void CreateWithoutSaveChanges(T1 notification)
        {            
            _context.Entry(notification).State = EntityState.Added;         
        }

        public virtual async Task MarkNotficationAsReadAsync(T1 notification)
        {
            notification.IsRead = true;
            _context.Update(notification);
            await _context.SaveChangesAsync();
        }
    }
}
