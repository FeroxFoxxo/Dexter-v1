﻿using Dexter.Enums;
using Dexter.Extensions;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace Dexter.Commands {
    public partial class UtilityCommands {

        [Command("changepfp", RunMode = RunMode.Async)]
        [Summary("Changes the profile picture of the bot to a random image from a selection made for him.")]

        public async Task ChangeProfile () {
            await DiscordSocketClient.CurrentUser
                .ModifyAsync(ClientProperties => ClientProperties.Avatar = new Image(ProfileService.GetRandomPFP()));

            await BuildEmbed(EmojiEnum.Love)
                .WithTitle("Profile Changed.")
                .WithDescription($"Haiya! I've successfully changed my profile picture to {ProfileService.CurrentPFP}. " +
                    $"If this has been a delayed message, it may very well be due to ratelimiting! <3")
                .SendEmbed(Context.Channel);
        }

    }
}