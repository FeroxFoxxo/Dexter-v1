﻿using Dexter.Abstractions;
using Dexter.Configurations;
using Dexter.Databases.Cooldowns;
using Dexter.Enums;
using Dexter.Extensions;
using Discord;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Dexter.Services {

    /// <summary>
    /// The CommissionCooldownService checks to see if a user has posted multiple commissions in the given channel in a quicker period of 
    /// time than otherwise allowed and, if so, attempts to remove the commission posting and warns the user of the event.
    /// </summary>
    
    public class CommissionCooldownService : Service {

        /// <summary>
        /// The CooldownDB contains all the current cooldowns in the database.
        /// </summary>
        
        public CooldownDB CooldownDB { get; set; }

        /// <summary>
        /// The CommissionCooldownConfiguration contains information regarding the channel in which the commission is in, aswell as how long cooldowns should last.
        /// </summary>
        
        public CommissionCooldownConfiguration CommissionCooldownConfiguration { get; set; }

        /// <summary>
        /// The Initialize override hooks into the MessageReceived event to run the related method.
        /// </summary>
        
        public override void Initialize() {
            DiscordSocketClient.MessageReceived += MessageRecieved;
        }

        /// <summary>
        /// The MessageRecieved checks to see if a message was sent in the commissions channel and, if so, runs it on a query to check if the user has sent
        /// a commission on an earlier period of time. If so, it deletes the commission if possible and warns the user of posting too quick commissions.
        /// </summary>
        /// <param name="SocketMessage">The SocketMessage contains information of the message that was sent, including the author, and can be used to remove the message.</param>
        /// <returns>A task object, from which we can await until this method completes successfully.</returns>
        
        public async Task MessageRecieved(SocketMessage SocketMessage) {
            // We first check to see if the channel is in the commissions corner and not from a bot to continue.
            if (SocketMessage.Channel.Id != CommissionCooldownConfiguration.CommissionsCornerID || SocketMessage.Author.IsBot)
                return;

            // We then try pull the cooldown from the database to see if the user and channel ID both exist as a token.
            Cooldown Cooldown = CooldownDB.Cooldowns.AsQueryable()
                .Where(Cooldown => Cooldown.Token.Equals($"{SocketMessage.Author.Id}{SocketMessage.Channel.Id}")).FirstOrDefault();

            if (Cooldown != null) {

                // We then check to see if the cooldown is expired. If so, we set the new time.
                if (Cooldown.TimeOfCooldown + CommissionCooldownConfiguration.CommissionCornerCooldown > DateTimeOffset.UtcNow.ToUnixTimeSeconds()) {

                    // We then check to see if the cooldown is in the grace-period. This is a period of time where the user can send multiple messages. Once this cooldown has ended we warn the user.
                    if (Cooldown.TimeOfCooldown + CommissionCooldownConfiguration.GracePeriod < DateTimeOffset.UtcNow.ToUnixTimeSeconds()) {
                        DateTime CooldownTime = DateTime.UnixEpoch.AddSeconds(Cooldown.TimeOfCooldown);

                        // We first attempt to delete the message. This may throw an error.
                        await SocketMessage.DeleteAsync();

                        // We then warn the user that they are not allowed to post a commission, as they have reached their maximum allotted time.
                        await BuildEmbed(EmojiEnum.Love)
                            .WithTitle($"Haiya, {SocketMessage.Author.Username}.")
                            .WithDescription($"Just a friendly reminder you are only allowed to post commissions every" +
                                $" {TimeSpan.FromSeconds(CommissionCooldownConfiguration.CommissionCornerCooldown).TotalDays} days. " +
                                $"Please take a lookie over the channel pins regarding the regulations of this channel if you haven't already <3")
                            .AddField("Last Commission Sent:", $"{CooldownTime.ToLongTimeString()}, {CooldownTime.ToLongDateString()}")
                            .WithFooter($"Times are in {(TimeZoneInfo.Local.IsDaylightSavingTime(CooldownTime) ? TimeZoneInfo.Local.DaylightName : TimeZoneInfo.Local.StandardName)}.")
                            .WithCurrentTimestamp()
                            .SendEmbed(SocketMessage.Author, SocketMessage.Channel as ITextChannel);
                    }
                } else {
                    // If the commission has expired we set the new cooldown.
                    Cooldown.TimeOfCooldown = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                    CooldownDB.SaveChanges();
                }
            } else {
                // If the user has not posted a commission before we add a new commission cooldown to the database.
                CooldownDB.Cooldowns.Add(
                    new Cooldown() {
                        Token = $"{SocketMessage.Author.Id}{SocketMessage.Channel.Id}",
                        TimeOfCooldown = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    }
                );

                CooldownDB.SaveChanges();
            }
        }

    }

}
