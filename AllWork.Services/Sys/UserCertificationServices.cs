using AllWork.IRepository.Sys;
using AllWork.IServices.Sys;
using AllWork.Model.Sys;
using System.Threading.Tasks;

namespace AllWork.Services.Sys
{
    public class UserCertificationServices:Base.BaseServices<UserCertification>,IUserCertificationServices
    {
        readonly IUserCertificationRepository _dal;
        public UserCertificationServices(IUserCertificationRepository userCertificationRepository)
        {
            _dal = userCertificationRepository;
        }

        //提交用户验证
        public async Task<bool> SaveUserCertification(UserCertification userCertification)
        {
            var instance = await _dal.QueryFirst("Select * from UserCertification Where UnionId = @UserId", userCertification);
            if (instance == null)
            {
                var insertSql = "Insert UserCertification (UnionId,Name,AuthType,AuthState,CertificateId,CertificateFront,CertificateBack,CorpName,CorpAddress,SalesMan)values(@UnionId,@Name,@AuthType,@AuthState,@CertificateId,@CertificateFront,@CertificateBack,@CorpName,@CorpAddress,@SalesMan)";
                return await _dal.Execute(insertSql, userCertification) > 0;
            }
            else
            {
                var updateSql = "Update UserCertification set UnionId = @UnionId,Name = @Name,AuthType = @AuthType,AuthState = @AuthState,CertificateId = @CertificateId,CertificateFront = @CertificateFront,CertificateBack = @CertificateBack,CorpName = @CorpName,CorpAddress = @CorpAddress,SalesMan = @SalesMan Where UnionId = @UnionId";
                return await _dal.Execute(updateSql, userCertification) > 0;
            }
        }

        //获取用户验证信息
        public async Task<UserCertification> GetUserCertification(string unionId)
        {
            var sql = "Select * from UserCertification Where UnionId = @UnionId";
            var res = await _dal.QueryFirst(sql, new { UnionId = unionId });
            return res;
        }
    }
}
