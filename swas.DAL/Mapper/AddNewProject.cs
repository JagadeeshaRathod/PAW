using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swas.DAL.Mapper
{
    [Keyless]
    public class AddNewProject
    {
        public int ProjId { get; set; }
        public string ProjName { get; set; }
        public int StakeHolderId { get; set; }
        public DateTime? InitiatedDate { get; set; }
        public string HostType { get; set; }
        public string Apptype { get; set; }
        public string? TypeofSW { get; set; }
        public string? BeingDevpInhouse { get; set; }
        public bool IsSubmited { get; set; }
        public int CurrentPslmId { get; set; }
        [NotMapped]
        public string? EncyID { get; set; }
    }
}
