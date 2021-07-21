using AllWork.IRepository.Sys;
using AllWork.Model.Sys;
using AllWork.Repository.Base;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace AllWork.Repository.Sys
{
    public class UserCertificationRepository:BaseRepository<UserCertification>,IUserCertificationRepository
    {
        public UserCertificationRepository(IConfiguration configuration) : base(configuration)
        {
        }

        //提交用户验证
        public async Task<bool> SaveUserCertification(UserCertification userCertification)
        {
            var instance = await base.QueryFirst("Select * from UserCertification Where UnionId = @UserId", userCertification);
            if (instance == null)
            {
                var insertSql = "Insert UserCertification (UnionId,Name,AuthType,AuthState,CertificateId,CertificateFront,CertificateBack,CorpName,CorpAddress,SalesMan)values(@UnionId,@Name,@AuthType,@AuthState,@CertificateId,@CertificateFront,@CertificateBack,@CorpName,@CorpAddress,@SalesMan)";
                return await base.Execute(insertSql, userCertification) > 0;
            }
            else
            {
                var updateSql = "Update UserCertification set UnionId = @UnionId,Name = @Name,AuthType = @AuthType,AuthState = @AuthState,CertificateId = @CertificateId,CertificateFront = @CertificateFront,CertificateBack = @CertificateBack,CorpName = @CorpName,CorpAddress = @CorpAddress,SalesMan = @SalesMan Where UnionId = @UnionId";
                return await base.Execute(updateSql, userCertification) > 0;
            }
        }

        //获取用户验证信息
        public async Task<UserCertification> GetUserCertification(string unionId)
        {
            var sql = "Select * from UserCertification Where UnionId = @UnionId";
            var res = await base.QueryFirst(sql, new { UnionId = unionId });
            return res;
        }

    }
}
