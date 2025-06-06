using Grpc.Core;
using swas.BAL.DTO;
using swas.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swas.BAL.Interfaces
{
    ///Created and Reviewed by : Sub Maj M Sanal
    ///Reviewed Date : 31 Jul 23
    ///Tested By :- 
    ///Tested Date : 
    ///Start
    public interface IUnitRepository
    { 
        Task<int> Save(UnitDtl Db);
        bool CheckUserExist(string UserName); /*, string susno*/
        Task<int> del(UnitDtl Db);
        Task<List<UnitDtl>> GetAllUnitAsync();

        Task<List<DTOUnitMapping>> GetallUnitwithmap();

        Task<List<DTOUnitMapping>> GetallUnitwithmap1(int StageId ,int StatusId);


        Task<List<UnitDtl>> GetAllStakeHolderComment();


        //bool GetFindUnitAsync(string UserName);

        Task<UnitDtl> GetUnitDtl(int unitid);
        Task<int> GetIdCalendar();

        Task<UnitDtl> GetUnitDtlwithname(string UnitName);
        Task<UnitDtl> GetUnitDtls(int unitid);


    }
}
