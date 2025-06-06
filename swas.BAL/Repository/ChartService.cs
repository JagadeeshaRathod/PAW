using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using swas.BAL.Interfaces;
using swas.DAL;
using swas.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swas.BAL.Repository
{
    public class ChartService : IChartService
    {
        private readonly ApplicationDbContext _dbContext;

        public ChartService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        ///Developer Name :- Sub Maj M Sanal Kumar

        ///Revised on :- 19,20 & 26, 27 Aug 2023  ---    pie chart incorrect data shown fixed by using sp json
        /// Revised on :- 07& 08 Oct  2023  ---   month order and chart error fixed and tested


        public async Task<List<ChartModelS>> GetChartDataS()
        {
            try
            {
                var chartModels = await _dbContext.Set<ChartModelS>()
                    .FromSqlRaw("EXEC getMonthInOutSummary")
                    .ToListAsync();

                return chartModels;
            }
            catch (Exception ex)
            {
                string ss = ex.Message;
                return new List<ChartModelS>();
            }
        }

        public async Task<List<ChartModel>> GetChartData()
        {
            try
            {
                var chartModels = await _dbContext.Set<ChartModel>()
                    .FromSqlRaw("EXEC getAppTypeCounts")
                    .ToListAsync();

                return chartModels;
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                // Log or handle the exception
                return new List<ChartModel>();
            }
        }


    }

}
