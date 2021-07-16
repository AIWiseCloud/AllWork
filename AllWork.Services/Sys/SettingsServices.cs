using AllWork.Model;
using AllWork.Model.Sys;
using System;
using System.Threading.Tasks;

namespace AllWork.Services.Sys
{
    public class SettingsServices: Base.BaseServices<Settings>,IServices.Sys.ISettingsServices
    {
        readonly IRepository.Sys.ISettingsRepository _dal;
        //依赖注入
        public SettingsServices(IRepository.Sys.ISettingsRepository settingsRepository)
        {
            _dal = settingsRepository;
        }

        public async Task<OperResult> SaveSettings(Settings model)
        {
            OperResult res = new OperResult();
            try
            {
                var instance =  _dal.QueryFirst("Select * from Settings limit 1").Result;
                //无则新增
                if (instance == null)
                {
                    var sql = @"Insert into Settings(ID,IsMaintain,ImgUrl1,Nav1,ImgUrl2,Nav2,ImgUrl3,Nav3,Notication,ShowNotice)values
(@ID,@IsMaintain,@ImgUrl1,@Nav1,@ImgUrl2,@Nav2,@ImgUrl3,@Nav3,@Notication,@ShowNotice)";
                    res.IdentityKey = "";
                    res.Status = await _dal.Execute(sql, model) > 0;
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
                    res.Status = await _dal.Execute(sql, model) > 0;
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
            return await _dal.QueryFirst("Select * from Settings limit 1");
        }
    }
}
