using System;
using System.Threading.Tasks;
using Dexter.Attributes.Methods;
using Dexter.Databases.Levels;
using Dexter.Enums;
using Dexter.Extensions;
using Discord;
using Discord.Commands;

namespace Dexter.Commands
{
    public partial class LevelingCommands
    {

        /// <summary>
        /// Toggles XP debugging.
        /// </summary>

        [Command("debugleveling")]
        [Alias("debuglevels", "debugxp")]
        [RequireModerator]

        public async Task ToggleXPDebug()
        {
            await Context.Channel.SendMessageAsync(LevelingService.ToggleDebugging());
        }

    }
}
