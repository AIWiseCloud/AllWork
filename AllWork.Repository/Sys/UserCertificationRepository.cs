using AllWork.IRepository.Sys;
using AllWork.Model.RequestParams;
using AllWork.Model.User;
using AllWork.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;
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
                var insertSql = @"Insert UserCertification (UnionId,Name,AuthState, PhoneNumber, CertificateId,CertificateFront,CertificateBack,CorpName,CorpAddress)
values
(@UnionId,@Name,@AuthState,@PhoneNumber, @CertificateId,@CertificateFront,@CertificateBack,@CorpName,@CorpAddress)";
                return await base.Execute(insertSql, userCertification) > 0;
            }
            else
            {
                var updateSql = "Update UserCertification set Name = @Name,AuthState = @AuthState, PhoneNumber = @PhoneNumber,CertificateId = @CertificateId,CertificateFront = @CertificateFront,CertificateBack = @CertificateBack,CorpName = @CorpName,CorpAddress = @CorpAddress Where UnionId = @UnionId";
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
                sql = @"Insert CorpCertification (UnionId,Name,AuthState,PhoneNumber, CertificateId,CertificateFront,CorpName,CorpAddress)
values
(@UnionId,@Name,@AuthState,@PhoneNumber, @CertificateId,@CertificateFront,@CorpName,@CorpAddress)";
            }
            else
            {
                sql = "Update CorpCertification set Name = @Name,AuthState = @AuthState,PhoneNumber = @PhoneNumber, CertificateId = @CertificateId,CertificateFront = @CertificateFront,CorpName = @CorpName,CorpAddress = @CorpAddress Where UnionId = @UnionId";
                
            }
            return await base.Execute(sql, corpCertification) > 0;
        }

        //分页查询认证信息
        public async Task<Tuple<IEnumerable<UserCertification>, int>> QueryCertification(UserCertificationParams userCertificationParams)
        {
            //(1) sql公共部分
            var sqlpub = new StringBuilder(" from CertificationView a Where (1=1)");
            if (!string.IsNullOrEmpty(userCertificationParams.Keywords))
            {
                sqlpub.AppendFormat(" and (UnionId = @UnionId or Name like '%{0}%' or PhoneNumber like '%{0}%' ) ", userCertificationParams.Keywords);
            }
            if (!string.IsNullOrEmpty(userCertificationParams.StartDate) && !string.IsNullOrEmpty(userCertificationParams.EndDate))
            {
                sqlpub.Append(" and  a.CreateDate between @StartDate and @EndDate ");
            }
            if (userCertificationParams.CertificateType != -1)
            {
                sqlpub.Append(" and CertificateType = @CertificateType ");
            }
            //(2) 固定排序
            string sqlorder = " Order by CreateDate desc ";
            //(3) 求记录数
            var sql1 = "Select count(a.UnionId)  " + sqlpub.ToString();
            //(4) 分页获取数据
            var sql2 = " Select * " + sqlpub.ToString() + sqlorder + " limit @Skip, @PageSize ";
            //(5) 合并sql, 
            var sql = sql1 + ";" + sql2;
            //(6) 执行查询
            var res = await base.QueryPagination(sql, new
            {
                UnionId = userCertificationParams.Keywords,
                userCertificationParams.CertificateType,
                userCertificationParams.StartDate,
                userCertificationParams.EndDate,
                userCertificationParams.PageModel.Skip,
                userCertificationParams.PageModel.PageSize
            });
            return res;
        }

        public async Task<bool> AuditCertificationInfo(string unionId, int certificateType, int authState, string reason = "")
        {
            string sql;
            if (certificateType == 0)
            {
                sql = "Update UserCertification set AuthState = @AuthState Where UnionId = @UnionId";
            }
            else
            {
                sql = "Update CorpCertification set AuthState = @AuthState Where UnionId = @UnionId";
            }
            var res = await base.Execute(sql, new { UnionId = unionId, AuthState = authState });
            return res > 0;
        }

    }
}
