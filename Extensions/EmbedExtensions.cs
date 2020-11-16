﻿using Dexter.Configurations;
using Dexter.Enums;
using Discord;
using Discord.Commands;
using Discord.Webhook;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;

namespace Dexter.Extensions {
    /// <summary>
    /// The EmbedBuilder Extensions class offers a variety of different extensions that can be applied to an embed to modify or send it.
    /// </summary>
    public static class EmbedExtensions {

        /// <summary>
        /// Builds an embed with the attributes specified by the emoji enum.
        /// </summary>
        /// <param name="EmbedBuilder">The EmbedBuilder which you wish to be built upon.</param>
        /// <param name="Thumbnails">The type of EmbedBuilder you wish it to be, specified by an enum of possibilities.</param>
        /// <returns>The built embed, with the thumbnail and color applied.</returns>
        public static EmbedBuilder BuildEmbed(this EmbedBuilder EmbedBuilder, EmojiEnum Thumbnails) {
            Color Color = Thumbnails switch {
                EmojiEnum.Annoyed => Color.Red,
                EmojiEnum.Love => Color.Green,
                EmojiEnum.Sign => Color.Blue,
                EmojiEnum.Wut => Color.Teal,
                EmojiEnum.Unknown => Color.Orange,
                _ => Color.Magenta
            };

            return EmbedBuilder.WithThumbnailUrl(InitializeDependencies.ServiceProvider.GetRequiredService<BotConfiguration>().ThumbnailURLs[(int)Thumbnails]).WithColor(Color);
        }

        /// <summary>
        /// Builds an EmbedBuilder and sends it to the specified IMessageChannel.
        /// </summary>
        /// <param name="EmbedBuilder">The EmbedBuilder you wish to send.</param>
        /// <param name="MessageChannel">The IMessageChannel you wish to send the embed to.</param>
        /// <returns>A task object, from which we can await until this method completes successfully.</returns>
        public static async Task SendEmbed(this EmbedBuilder EmbedBuilder, IMessageChannel MessageChannel) =>
            await MessageChannel.SendMessageAsync(embed: EmbedBuilder.Build());

        /// <summary>
        /// Builds an EmbedBuilder and sends it to the specified DiscordWebhookClient channel.
        /// </summary>
        /// <param name="EmbedBuilder">The EmbedBuilder you wish to send.</param>
        /// <param name="DiscordWebhookClient">The DiscordWebhookClient you wish to send the embed to.</param>
        /// <returns>A task object, from which we can await until this method completes successfully.</returns>
        public static async Task SendEmbed(this EmbedBuilder EmbedBuilder, DiscordWebhookClient DiscordWebhookClient) =>
            await DiscordWebhookClient.SendMessageAsync(embeds: new Embed[1] { EmbedBuilder.Build() });

        /// <summary>
        /// Builds an EmbedBuilder and sends it to the specified IUser.
        /// </summary>
        /// <param name="EmbedBuilder">The EmbedBuilder you wish to send.</param>
        /// <param name="User">The IUser you wish to send the embed to.</param>
        /// <returns>A task object, from which we can await until this method completes successfully.</returns>
        public static async Task SendEmbed(this EmbedBuilder EmbedBuilder, IUser User) =>
            await User.SendMessageAsync(embed: EmbedBuilder.Build());

        /// <summary>
        /// The AddField method adds a field to an EmbedBuilder if a given condition is true.
        /// </summary>
        /// <param name="EmbedBuilder">The EmbedBuilder you wish to add the field to.</param>
        /// <param name="Condition">The condition which must be true to add the field.</param>
        /// <param name="Name">The name of the field you wish to add.</param>
        /// <param name="Value">The description of the field you wish to add.</param>
        /// <returns>The embed with the field added to it if true.</returns>
        public static EmbedBuilder AddField(this EmbedBuilder EmbedBuilder, bool Condition, string Name, object Value) {
            if (Condition)
                EmbedBuilder.AddField(Name, Value);

            return EmbedBuilder;
        }

        /// <summary>
        /// The GetParametersForCommand adds fields to an embed containing the parameters and summary of the command.
        /// </summary>
        /// <param name="EmbedBuilder">The EmbedBuilder you wish to add the fields to.</param>
        /// <param name="CommandService">An instance of the CommandService, which contains all the currently active commands.</param>
        /// <param name="Command">The command of which you wish to search for.</param>
        /// <returns>The embed with the parameter fields for the command added.</returns>
        public static EmbedBuilder GetParametersForCommand(this EmbedBuilder EmbedBuilder, CommandService CommandService, string Command) {
            SearchResult Result = CommandService.Search(Command);

            foreach (CommandMatch Match in Result.Commands) {
                CommandInfo CommandInfo = Match.Command;

                string CommandDescription = $"Parameters: {string.Join(", ", CommandInfo.Parameters.Select(p => p.Name))}";

                if (CommandInfo.Parameters.Count > 0)
                    CommandDescription = $"Parameters: {string.Join(", ", CommandInfo.Parameters.Select(p => p.Name))}";
                else
                    CommandDescription = "No parameters";

                if (!string.IsNullOrEmpty(CommandInfo.Summary))
                    CommandDescription += $"\nSummary: {CommandInfo.Summary}";

                EmbedBuilder.AddField(string.Join(", ", CommandInfo.Aliases), CommandDescription);
            }

            return EmbedBuilder;
        }

    }
}
