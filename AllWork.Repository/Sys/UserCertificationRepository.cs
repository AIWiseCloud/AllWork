using AllWork.IRepository.Sys;
using AllWork.Model.User;
using AllWork.Repository.Base;
using System.Threading.Tasks;

namespace AllWork.Repository.Sys
{
    public class UserCertificationRepository:BaseRepository<UserCertification>,IUserCertificationRepository
    {
        //提交用户验证
        public async Task<bool> SaveUserCertification(UserCertification userCertification)
        {
            var instance = await base.QueryFirst("Select * from UserCertification Where UnionId = @UnionId", userCertification);
            if (instance == null)
            {
                var insertSql = @"Insert UserCertification (UnionId,Name,AuthState, PhoneNumber, CertificateId,CertificateFront,CertificateBack,CorpName,CorpAddress,OpenUserId, SalesMan)
values
(@UnionId,@Name,@AuthState,@PhoneNumber, @CertificateId,@CertificateFront,@CertificateBack,@CorpName,@CorpAddress,@OpenUserId,@SalesMan)";
                return await base.Execute(insertSql, userCertification) > 0;
            }
            else
            {
                var updateSql = "Update UserCertification set Name = @Name,AuthState = @AuthState, PhoneNumber = @PhoneNumber,CertificateId = @CertificateId,CertificateFront = @CertificateFront,CertificateBack = @CertificateBack,CorpName = @CorpName,CorpAddress = @CorpAddress, OpenUserId = @OpenUserId,SalesMan = @SalesMan Where UnionId = @UnionId";
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

        public async Task<CorpCertification> GetCorpCertification(string unionId)
        {
            var sql = "Select * from CorpCertification Where UnionId = @UnionId";
            var res = await base.QueryFirst<CorpCertification>(sql, new { UnionId = unionId });
            return res;
        }

        public async Task<bool> SaveCorpCertification(CorpCertification corpCertification)
        {
            var instance = await base.QueryFirst("Select * from CorpCertification Where UnionId = @UnionId", corpCertification);
            string sql;
            if (instance == null)
            {
                sql = @"Insert CorpCertification (UnionId,Name,AuthState,PhoneNumber, CertificateId,CertificateFront,CorpName,CorpAddress,OpenUserId, SalesMan)
values
(@UnionId,@Name,@AuthState,@PhoneNumber, @CertificateId,@CertificateFront,@CorpName,@CorpAddress, @OpenUserId,@SalesMan)";
            }
            else
            {
                sql = "Update CorpCertification set Name = @Name,AuthState = @AuthState,PhoneNumber = @PhoneNumber, CertificateId = @CertificateId,CertificateFront = @CertificateFront,CorpName = @CorpName,CorpAddress = @CorpAddress, OpenUserId,SalesMan = @SalesMan Where UnionId = @UnionId";
                
            }
            return await base.Execute(sql, corpCertification) > 0;
        }

    }
}
