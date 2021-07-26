using System.ComponentModel.DataAnnotations;

namespace DiscordEventBot.Model
{
    public class AttendeeGroupTemplate
    {
        #region Public Properties

        [Key]
        public ulong GroupTemplateID { get; set; }

        public int? MaxCapacity { get; set; }

        public string Name { get; set; }

        #endregion Public Properties
    }
}