using System.ComponentModel.DataAnnotations;

namespace DiscordEventBot.Model
{
    public class Role
    {
        #region Public Constructors

        public Role()
        {
        }

        public Role(ulong id) : this()
        {
            RoleId = id;
        }

        #endregion Public Constructors

        #region Public Properties

        [Key]
        public ulong RoleId { get; set; }

        #endregion Public Properties
    }
}