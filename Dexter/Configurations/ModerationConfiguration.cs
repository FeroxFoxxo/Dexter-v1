﻿using Dexter.Abstractions;
using System.Collections.Generic;

namespace Dexter.Configurations {

    /// <summary>
    /// The ModerationConfiguration relates to the logging of reactions etc to a specified logging channel and to other moderator commands.
    /// </summary>
    
    public class ModerationConfiguration : JSONConfig {

        /// <summary>
        /// The DISABLED REACTION CHANNELS field details which channels the logging of reactions will not occur in.
        /// This is so we do not log extraneous reaction removals from channels like the suggestions or roles channels. 
        /// </summary>
        
        public ulong[] DisabledReactionChannels { get; set; }

        /// <summary>
        /// The WEBHOOK CHANNEL specifies the snowflake ID of the channel in which the moderation channel shall log to.
        /// </summary>
        
        public ulong WebhookChannel { get; set; }

        /// <summary>
        /// The WEBHOOK NAME specifies the name of the webhook that will be instantiated in the given moderation channel. 
        /// </summary>
        
        public string WebhookName { get; set; }

        /// <summary>
        /// The role ID for the "Happy Borkday" role.
        /// </summary>

        public ulong BorkdayRoleID { get; set; }

        /// <summary>
        /// The role ID for the "Happy Borkday (Staff edition)" role.
        /// </summary>

        public ulong StaffBorkdayRoleID { get; set; }

        /// <summary>
        /// Amount of time to grant the role for (generally 24 hours), in seconds.
        /// </summary>

        public int SecondsOfBorkday { get; set; }

        /// <summary>
        /// The role ID for the "Muted" role.
        /// </summary>

        public ulong MutedRoleID { get; set; }

        /// <summary>
        /// The numerical ID of the #rules-and-info channel.
        /// </summary>

        public ulong RulesAndInfoChannel { get; set; }

        /// <summary>
        /// The maximum amount of points any Dexter Profile may hold at any given moment.
        /// </summary>

        public short MaxPoints { get; set; }

        /// <summary>
        /// The amount of time to wait between each time a user would regain a point in the Dexter Profile.
        /// </summary>

        public int SecondsTillPointIncrement { get; set; }
        
        /// <summary>
        /// The numerical ID of the #staff-bots channel.
        /// </summary>

        public ulong StaffBotsChannel { get; set; }

        /// <summary>
        /// A list of different infraction notification times, each entry having a point and day keyvaluepair.
        /// </summary>

        public List<Dictionary<string, int>> InfractionNotifications { get; set; }

        /// <summary>
        /// The infraction notification is the time at which the bot will notify the admin team of a large amount of infractions.
        /// </summary>

        public short InfractionNotification { get; set; }

    }

}
