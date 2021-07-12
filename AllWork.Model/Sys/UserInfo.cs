using System;
using System.Collections.Generic;
using System.Text;

namespace AllWork.Model.Sys
{
    public class UserInfo:BaseModel
    {
        public string UnionId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string Gender { get; set; }
        public int Lock { get; set; }
        public int AuthState { get; set; }
        public string CertificateId { get; set; }
        public string CertificateFront { get; set; }
        public string CertificateBack { get; set; }
        public string CorpName { get; set; }
        public string CorpAddress { get; set; }
        public string SalesMan { get; set; }
        public string Roles { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
