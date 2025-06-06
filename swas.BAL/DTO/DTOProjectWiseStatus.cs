using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swas.BAL.DTO
{
    public class DTOProjectWiseStatus
    {
       public List<StatusProject> StatusProjectlst {  get; set; }
       public List<MovProject> MovProjectlst {  get; set; }
    }
    public class StatusProject
    {
        public int StatusId { get; set; }
        public string? Status { get; set; }
        public string? StageName { get; set; }
    }
    public class MovProject
    {
        public int ProjId { get; set; }
        public int StatusId { get; set; }
        public string? ProjName { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}
