using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using swas.BAL.DTO;


namespace swas.BAL.Utility
{
    ///Created and Reviewed by : Sub Maj Sanal
    ///Reviewed Date : 10 Aug 23
    ///Tested By :- 
    ///Tested Date : 
    ///Start
    public class HandlerSessionMW
    {
        private readonly RequestDelegate _next;

        public HandlerSessionMW(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ISession session)
        {
            // Create an object to be stored in session

            var myLogin = new Login();
            
            // Serialize the object to JSON
            string serializedObject = JsonConvert.SerializeObject(myLogin);

            // Store the serialized object in session
            session.SetString("MyLogin", serializedObject);

            await _next(context);
        }
    }
}