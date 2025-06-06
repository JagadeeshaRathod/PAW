
using swas.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swas.BAL.Interfaces
{
    public interface IChartService
    {
        Task<List<ChartModel>> GetChartData();
        Task<List<ChartModelS>> GetChartDataS();
    }

}
