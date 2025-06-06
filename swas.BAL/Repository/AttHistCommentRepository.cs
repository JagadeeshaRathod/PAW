using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using swas.BAL.DTO;
using swas.BAL.Interfaces;
using swas.DAL;
using swas.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace swas.BAL.Repository
{

    ///Created and Reviewed by : Sub Maj Sanal
    ///Reviewed Date : 13 Nov 23
    ///Tested By :- 
    ///Tested Date : 
    ///Start
    public class AttHistCommentRepository : IAttHistComment
    {
        private readonly ApplicationDbContext _dbContext;

        //public AttHistCommentRepository(ApplicationDbContext dbContext)
        //{
        //    _dbContext = dbContext;
        //}

        /////Created and Reviewed by : Sub Maj Sanal
        /////Reviewed Date : 31 Jul 23
        //public async Task<IEnumerable<AttHistComment>> GetAllAsync()
        //{
        //    return await _dbContext.AttHistComments.ToListAsync();
        //}

        //public async Task<AttHistComment> GetByIdAsync(int attId)
        //{
        //    return await _dbContext.AttHistComments.FirstOrDefaultAsync(a => a.Attid == attId);
        //}

        //public async Task<int> AddAsync(AttHistComment attHistComment)
        //{
        //    _dbContext.AttHistComments.Add(attHistComment);
        //    await _dbContext.SaveChangesAsync();
        //    return attHistComment.Attid;
        //}

        //public async Task<int> UpdateAsync(AttHistComment attHistComment)
        //{
        //    _dbContext.Entry(attHistComment).State = EntityState.Modified;
        //    await _dbContext.SaveChangesAsync();
        //    return attHistComment.Attid;
        //}

        //public async Task<int> DeleteAsync(int attId)
        //{
        //    var attHistComment = await _dbContext.AttHistComments.FindAsync(attId);
        //    if (attHistComment != null)
        //    {
        //        _dbContext.AttHistComments.Remove(attHistComment);
        //        await _dbContext.SaveChangesAsync();
        //        return attId;
        //    }
        //    return 0;
        //}

    }


}
