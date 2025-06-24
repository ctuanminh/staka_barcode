using AutoMapper;
using Be.Common.Responses;
using Be.Common.System;
using Be.Core.Entities;
using Be.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace Be.Services.System
{
    public class SystemServiceImp(IRepository repository, IMapper mapper) : ServiceResponse, ISystemService
    {
        public async Task<AppSettingEntity> AddAppSetting(AppSettingDto appSettingDto)
        {
            var setting = mapper.Map<AppSettingEntity>(appSettingDto);
            await repository.AddAsync(setting);
            await repository.SaveChangeAsync();
            return setting;
        }

        public async Task<bool> UpdateAppSetting(AppSettingDto appSettingDto)
        {
            try
            {
                var appSettingExist = await repository.FindAsync<AppSettingEntity>(appSettingDto.Id);
                var appSetting = mapper.Map(appSettingDto, appSettingExist);
                await repository.UpdateAsync(appSetting);
                await repository.SaveChangeAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<List<AppSettingDto>> GetAppSettingBuyComputer(string computerName)
        {
            var settings = await repository.GetQueryable<AppSettingEntity>()
                .Where(s => s.ComputerName == computerName
                )
                .Select(p => new AppSettingDto()
                {
                    ComputerName = p.ComputerName,
                    ModuleName = p.ModuleName,
                    SettingKey = p.SettingKey,
                    SettingValue = p.SettingValue

                })
                .ToListAsync();

            return settings;
        }
        public async Task<AppSettingDto> GetAppSetting(string computerName, string module,
            string settingKey)
        {
            var settings = await repository.GetQueryable<AppSettingEntity>()
                .Where(s => s.ComputerName == computerName
                            && s.ModuleName == module
                            && s.SettingKey == settingKey
                )
                .Select(p => new AppSettingDto()
                {
                    Id = p.Id,
                    ComputerName = p.ComputerName,
                    ModuleName = p.ModuleName,
                    SettingKey = p.SettingKey,
                    SettingValue = p.SettingValue

                })
                .FirstOrDefaultAsync();

            return settings;
        }

        public async Task<RequestEntity> AddRequest(RequestEntity requestEntity)
        {
            await repository.AddAsync(requestEntity);
            await repository.SaveChangeAsync();
            return requestEntity;
        }

        public async Task<List<RequestEntity>> GetAllRequest()
        {
            var requests = await repository.GetQueryable<RequestEntity>()
                .ToListAsync();
            return requests;
        }
    }
}
