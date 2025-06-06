using swas.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swas.BAL.Interfaces
{
    public interface INotificationRepository
    { 
        Task<int> GetNotificationCountByType(int Type , int? ToUnitId);
        Task<Notification> GetNotificationByProjId(int projId);
        Task<Notification> GetNotifByToUnitAndType(int Type, int? ToUnitId, int projId);
        Task<Notification> GetNotifByToAndFormId(int Type,int ToUnitId,int ProjId, int? FromUnitId);
        Task<List<Notification>> GetNotifExcludingToUnit(int? ToUnitId, int projId);

        Task<int> AddNotification (Notification notifications);
        Task<bool> UpdateNotification(Notification notify);

        
    }
}
