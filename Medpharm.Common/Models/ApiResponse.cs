using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Text;

namespace Medpharm.Common
{
    [DataContract]
    public class BaseResponse<T>
    {
        public BaseResponse()
        {
            DataList = new List<T>();
        }

        [DataMember]
        public List<T> DataList { get; set; }

        [DataMember]
        public T Data { get; set; }

        [DataMember]
        public long ID { get; set; }

        [DataMember]
        public int Status { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public Exception Exception { get; set; }



        public BaseResponse(int status, T result, List<T> results, long id, string message, Exception exception)
        {
            Status = status;
            Data = result;
            DataList = results;
            ID = id;
            Message = message;
            Exception = exception;
            
        }
        

        public ObjectResult Send()
        {
            return new OkObjectResult(this) { StatusCode = (int?)HttpStatusCode.OK };
            //if (this.Status == StatusEnum.Success)
            //{
            //    return new OkObjectResult(this);
            //}
            //else if (this.Status == StatusEnum.Exception)
            //{
            //    return new ObjectResult(this) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            //}
            //else
            //{
            //    return new ObjectResult(this) { StatusCode = (int?)HttpStatusCode.BadRequest };
            //}
        }
    }

    //add enum here for the ref useage.
    public enum StatusEnum
    {
        Success = 1,
        Failure = 2,
        Exception = 3,
        NotFound = 4,
    }
    
    public enum GenderEnum
    {
        Male = 1,
        Female = 2,
        Other = 3,
    }
}
