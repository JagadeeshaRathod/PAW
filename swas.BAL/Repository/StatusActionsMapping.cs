using swas.BAL.Interfaces;
using swas.DAL.Models;
using swas.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using swas.BAL.DTO;
using swas.BAL.Helpers;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Grpc.Core;

namespace swas.BAL.Repository
{
    public class StatusActionsMapping : GenericRepositoryDL<TrnStatusActionsMapping>, IStatusActionsMapping
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly ApplicationDbContext _DBContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDataProtector _dataProtector;
        public StatusActionsMapping(ApplicationDbContext context , IHttpContextAccessor httpContextAccessor ) : base(context)
        {
            _dbContext = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TrnStatusActionsMapping> GetByActionsAndStatusAsync(int StatusActionsMappingId,int actionsId, int statusId)
        {
            if(StatusActionsMappingId > 0)
            {
                return await _context.TrnStatusActionsMapping
                .FirstOrDefaultAsync(usm => usm.ActionsId == actionsId && usm.StatusId == statusId && usm.StatusActionsMappingId == StatusActionsMappingId);
            }
            else
            {
                return await _context.TrnStatusActionsMapping.FirstOrDefaultAsync(usm => usm.ActionsId == actionsId && usm.StatusId == statusId );
            }
            
        }
    }
}
