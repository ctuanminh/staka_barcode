using Be.Common.System;
using Be.Core.Entities;

namespace Be.Services.System
{
    public interface ISystemService
    {
        /// <summary>
        /// Synchronizes the application settings.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<AppSettingEntity> AddAppSetting(AppSettingDto appSettingDto);
        /// <summary>
        /// Synchronizes the system logs.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<bool> UpdateAppSetting(AppSettingDto appSettingDto);
        /// <summary>
        /// Checks for system updates.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<List<AppSettingDto>> GetAppSettingBuyComputer(string computerName);
        Task<AppSettingDto> GetAppSetting(string computerName, string module, string settingKey);

        Task<RequestEntity> AddRequest(RequestEntity requestEntity);
        Task<List<RequestEntity>> GetAllRequest();

    }
}
