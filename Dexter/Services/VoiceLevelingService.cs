﻿using Dexter.Abstractions;
using Dexter.Configurations;
using Dexter.Databases.EventTimers;
using Dexter.Databases.Levels;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dexter.Services {

    public class VoiceLevelingService : Service {

        public LevelingConfiguration LevelingConfiguration { get; set; }

        public Random Random { get; set; }

        public LevelDatabase LevelDatabase { get; set; }

        public override async void Initialize() {
            EventTimer Timer = TimerService.EventTimersDB.EventTimers.AsQueryable().Where(Timer => Timer.CallbackClass.Equals(GetType().Name)).FirstOrDefault();

            if (Timer != null)
                TimerService.EventTimersDB.EventTimers.Remove(Timer);

            await CreateEventTimer(AddVoiceLevels, new(), LevelingConfiguration.XPIncrementTime, TimerType.Interval);
        }

        public async Task AddVoiceLevels(Dictionary<string, string> Parameters) {
            DiscordSocketClient.GetGuild(LevelingConfiguration.GuildID).VoiceChannels.ToList().ForEach(
                VoiceChannel => VoiceChannel.Users.ToList().ForEach(
                    UserVC => {
                        if (!(UserVC.IsMuted || UserVC.IsDeafened || UserVC.IsSelfMuted || UserVC.IsSelfDeafened || UserVC.IsBot)) {
                            LevelDatabase.IncrementUserXP(
                                Random.Next(LevelingConfiguration.VCMinXPGiven, LevelingConfiguration.VCMaxXPGiven),
                                UserVC,
                                DiscordSocketClient
                            );
                        }
                    }
                )
            );
        }

    }

}