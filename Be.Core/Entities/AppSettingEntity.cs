using Be.Core.BaseEntities;

namespace Be.Core.Entities
{
    public class AppSettingEntity : AuditedEntity
    {
        public string ComputerName { get; set; }
        public string ModuleName { get; set; }
        public string SettingKey { get; set; }
        public string SettingValue { get; set; }

    }
}
