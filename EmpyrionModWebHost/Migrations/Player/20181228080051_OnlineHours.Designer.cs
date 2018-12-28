﻿// <auto-generated />
using System;
using EmpyrionModWebHost.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EmpyrionModWebHost.Migrations
{
    [DbContext(typeof(PlayerContext))]
    [Migration("20181228080051_OnlineHours")]
    partial class OnlineHours
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024");

            modelBuilder.Entity("EmpyrionModWebHost.Models.Player", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<float>("BodyTemp");

                    b.Property<float>("BodyTempMax");

                    b.Property<int>("ClientId");

                    b.Property<double>("Credits");

                    b.Property<int>("Died");

                    b.Property<int>("EntityId");

                    b.Property<int>("Exp");

                    b.Property<byte>("FactionGroup");

                    b.Property<int>("FactionId");

                    b.Property<byte>("FactionRole");

                    b.Property<float>("Food");

                    b.Property<float>("FoodMax");

                    b.Property<float>("Health");

                    b.Property<float>("HealthMax");

                    b.Property<int>("Kills");

                    b.Property<DateTime>("LastOnline");

                    b.Property<string>("Note");

                    b.Property<bool>("Online");

                    b.Property<int>("OnlineHours");

                    b.Property<TimeSpan>("OnlineTime");

                    b.Property<byte>("Origin");

                    b.Property<float>("Oxygen");

                    b.Property<float>("OxygenMax");

                    b.Property<int>("Permission");

                    b.Property<int>("Ping");

                    b.Property<string>("PlayerName");

                    b.Property<string>("Playfield");

                    b.Property<float>("PosX");

                    b.Property<float>("PosY");

                    b.Property<float>("PosZ");

                    b.Property<float>("Radiation");

                    b.Property<float>("RadiationMax");

                    b.Property<float>("RotX");

                    b.Property<float>("RotY");

                    b.Property<float>("RotZ");

                    b.Property<float>("Stamina");

                    b.Property<float>("StaminaMax");

                    b.Property<string>("StartPlayfield");

                    b.Property<string>("SteamId");

                    b.Property<string>("SteamOwnerId");

                    b.Property<int>("Upgrade");

                    b.HasKey("Id");

                    b.ToTable("Players");
                });
#pragma warning restore 612, 618
        }
    }
}
