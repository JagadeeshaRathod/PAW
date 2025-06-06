namespace swas.BAL.Utility
{
    ///Created and Reviewed by : Sub Maj Sanal
    ///Reviewed Date : 30 Jul 23
    ///Tested By :- 
    ///Tested Date : 
    ///Start
    public class EnumHelper
    {
        public enum Response
        {
            Success = 1,
            Default = 0,
            Error = -1,
            Mandatory = -2,
            SessionExpired = -3,
            InvalidCredentials = -4
        }
        public enum SaveData
        {
             Save = 1,
             Update = 2,
             Delete = 3,
             Duplicate = 4



        }
    }
}
