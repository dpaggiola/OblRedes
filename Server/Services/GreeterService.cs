using System;
using System.Threading.Tasks;
using DataAccess;
using Domain;
using Grpc.Core;
using IDataAccess;
using Microsoft.Extensions.Logging;


namespace Server
{
    public class GreeterService : Greeter.GreeterBase
    {
        public override Task<Reply> AddUser(AddUserRequest request, ServerCallContext context)
        {
            string message;
            try
            {
                IClientDataAccess clientDataAccess = new ClientDataAccess();
                clientDataAccess.RegisterUser(request.Username, request.Password);
                message = "Registro exitoso " + request.Username;
            }
            catch (System.Exception e)
            {
                message = e.Message;
            }

            return Task.FromResult(new Reply
            {
                Message = message
            });
        }

        public override Task<Reply> DeleteUser(Username request, ServerCallContext context)
        {
            string message;
            try
            {
                IClientDataAccess clientDataAccess = new ClientDataAccess();
                clientDataAccess.DeleteUser(request.Username_);
                message = "Borrado exitoso " + request.Username_;
            }
            catch (System.Exception e)
            {
                message = e.Message;
            }

            return Task.FromResult(new Reply
            {
                Message = message
            });
        }

        public override Task<Reply> UpdateUser(UpdateUserRequest request, ServerCallContext context)
        {
            string message;
            try
            {
                IClientDataAccess clientDataAccess = new ClientDataAccess();
                clientDataAccess.UpdateUser(request.OldUsername, request.Username, request.Password);
                message = "Actualizacion exitosa del usuario " + request.Username;
            }
            catch (System.Exception e)
            {
                message = e.Message;
            }

            return Task.FromResult(new Reply
            {
                Message = message
            });
        }
    }
}