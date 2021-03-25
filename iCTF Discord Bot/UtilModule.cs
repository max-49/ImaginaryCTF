﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using iCTF_Shared_Resources;
using Microsoft.Extensions.DependencyInjection;

namespace iCTF_Discord_Bot
{
    public class UtilModule : ModuleBase<SocketCommandContext>
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly DatabaseContext _context;
        private readonly IServiceScope _scope;

        private UtilModule(DiscordSocketClient client, CommandService commands, IServiceScopeFactory scopeFactory)
        {
            _client = client;
            _commands = commands;
            _scope = scopeFactory.CreateScope();
            _context = _scope.ServiceProvider.GetService<DatabaseContext>();
        }

        ~UtilModule() { _scope.Dispose(); }

        [Command("about")]
        public async Task About()
        {
            CustomEmbedBuilder eb = new CustomEmbedBuilder();
            eb.WithThumbnailUrl("https://cdn.discordapp.com/avatars/669798825765896212/1e0a0375779d6a6117c035e7cc7f0665.png?size=128");
            eb.WithTitle("About");
            eb.AddField("Author", "This bot was created by Et3rnos#6556");
            eb.AddField("Support", "If you want to support me you can visit my Patreon:\n<https://www.patreon.com/et3rnos>");
            eb.AddField("Other apps by me", "<https://fdownl.ga> (File Sharing Platform)");
            await ReplyAsync(embed: eb.Build());
        }

        [Command("help")]
        public async Task Help()
        {
            CustomEmbedBuilder embedBuilder = new CustomEmbedBuilder()
            {
                Title = "Available Commands"
            };

            embedBuilder.AddField("`.help`", "Prints this help message.", true);
            embedBuilder.AddField("`.about`", "Prints information about this bot.", true);
            embedBuilder.AddField("`.ping`", "Prints the bot latency.", true);
            embedBuilder.AddField("`.flag <flag>`", "Submits a flag.", true);
            embedBuilder.AddField("`.stats [user]`", "Shows some user statistics.", true);
            embedBuilder.AddField("`.help admin`", "Shows admin-only commands.", true);

            await ReplyAsync(embed: embedBuilder.Build());
        }

        [Command("help")]
        public async Task Help(string category)
        {
            if (category != "admin") { return; }

            CustomEmbedBuilder embedBuilder = new CustomEmbedBuilder()
            {
                Title = "Available Admin Commands",
                Color = Color.Blue
            };

            embedBuilder.AddField("`.link`", "Links this bot to this server.", true);
            embedBuilder.AddField("`.setchallreleasechannel [channel]`", "Sets the released challenges channel.", true);
            embedBuilder.AddField("`.setchallsolveschannel [channel]`", "Sets the solves announcement channel.", true);
            embedBuilder.AddField("`.setleaderboardchannel [channel]`", "Sets the leaderboard channel.", true);
            embedBuilder.AddField("`.settodayschannel [channel]`", "Sets the today's channel.", true);
            embedBuilder.AddField("`.setlogschannel [channel]`", "Sets the logs channel.", true);
            embedBuilder.AddField("`.settoproles <1st-role> <2nd-role> <3rd-role>`", "Sets the top roles.", true);
            embedBuilder.AddField("`.settodaysrole <role>`", "Sets the today's role.", true);
            embedBuilder.AddField("`.setreleasetime <hour> <minutes>`", "Sets the release time (UTC).", true);
            embedBuilder.AddField("`.verify <flag>`", "Verifies a flag without submitting it.", true);

            await ReplyAsync(embed: embedBuilder.Build());
        }

        [Command("ping")]
        public async Task Ping()
        {
            int latency = _client.Latency;

            await ReplyAsync($"The latency is currently {latency}ms.");
        }
    }
}