using AllWork.Model.Sys;
using AllWork.IRepository.Sys;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using AllWork.Model;
using System;

namespace AllWork.Repository.Sys
{
    public class SettingsRepository:Base.BaseRepository<Settings>,ISettingsRepository
    {
        public SettingsRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<OperResult> SaveSettings(Settings model)
        {
            OperResult res = new OperResult();
            try
            {
                var instance = base.QueryFirst("Select * from Settings limit 1").Result;
                //无则新增
                if (instance == null)
                {
                    var sql = @"Insert into Settings(ID,IsMaintain,ImgUrl1,Nav1,ImgUrl2,Nav2,ImgUrl3,Nav3,Notication,ShowNotice)values
(@ID,@IsMaintain,@ImgUrl1,@Nav1,@ImgUrl2,@Nav2,@ImgUrl3,@Nav3,@Notication,@ShowNotice)";
                    res.IdentityKey = "";
                    res.Status = await base.Execute(sql, model) > 0;
                    res.ErrorMsg = "success";
                }
                else//有则修改
                {
                    var sql = @"Update Settings set ID=@ID,
IsMaintain=@IsMaintain,
ImgUrl1=@ImgUrl1,
Nav1=@Nav1,
ImgUrl2=@ImgUrl2,
Nav2=@Nav2,
ImgUrl3=@ImgUrl3,
Nav3=@Nav3,
Notication=@Notication,
ShowNotice=@ShowNotice
Where ID = @ID";
                    model.ID = instance.ID;
                    res.Status = await base.Execute(sql, model) > 0;
                }
            }
            catch (Exception ex)
            {
                res.ErrorMsg = ex.Message;
            }
            return res;
        }

        public async Task<Settings> GetSettings()
        {
            return await base.QueryFirst("Select * from Settings limit 1");
        }
    }
}
