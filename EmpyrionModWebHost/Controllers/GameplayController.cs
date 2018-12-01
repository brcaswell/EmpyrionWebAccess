﻿using AutoMapper;
using Eleon.Modding;
using EmpyrionModWebHost.Configuration;
using EmpyrionModWebHost.Extensions;
using EmpyrionModWebHost.Models;
using EmpyrionModWebHost.Services;
using EmpyrionNetAPIAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace EmpyrionModWebHost.Controllers
{

    public class GameplayManager : EmpyrionModBase, IEWAPlugin
    {
        private const string IdDef = "Id:";
        private const string NameDef = "Name:";

        public ModGameAPI GameAPI { get; private set; }

        public override void Initialize(ModGameAPI dediAPI)
        {
            GameAPI = dediAPI;
        }

        static ItemInfo[] _mItemInfo;

        public IEnumerable<ItemInfo> GetAllItems()
        {
            if (_mItemInfo != null) return _mItemInfo;

            var ItemDef      = File.ReadAllLines(Path.Combine(EmpyrionConfiguration.ProgramPath, @"Content\Configuration\Config_Example.ecf"))
                .Where(L => L.Contains(IdDef));
            var Localisation = File.ReadAllLines(Path.Combine(EmpyrionConfiguration.ProgramPath, @"Content\Extras\Localization.csv"))
                .Where(L => Char.IsLetter(L[0]))
                .ToDictionary(L => L.Substring(0, L.IndexOf(",")), L => L.Substring(L.IndexOf(",") + 1));

            _mItemInfo = ItemDef.Select(L =>
            {
                var IdPos           = L.IndexOf(IdDef);
                var IdDelimiter     = L.IndexOf(",", IdPos);
                var NamePos         = L.IndexOf(NameDef);
                var NameDelimiter   = L.IndexOf(",", NamePos);
                if (NameDelimiter == -1) NameDelimiter = L.Length;

                return IdPos >= 0 && NamePos >= 0 && IdDelimiter >= 0
                    ? new ItemInfo()
                    {
                        Id = int.TryParse(L.Substring(IdPos + IdDef.Length, IdDelimiter - IdPos - IdDef.Length), out int Result) ? Result : 0,
                        Name = L.Substring(NamePos + NameDef.Length, NameDelimiter - NamePos - NameDef.Length).Trim()
                    }
                    : null;
            })
            .Select(I => {
                if (I != null)
                {
                    if (Localisation.TryGetValue(I.Name + ",", out string Value))
                    {
                        var End   = Value.IndexOf(",");
                        I.Name = Value.Substring(0, End);
                    }
                }
                return I;
            })
            .Where(I => I != null)
            .ToArray();

            return _mItemInfo;
        }
    }

    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class GameplayController : ControllerBase
    {
        public GameplayManager GameplayManager { get; }

        public GameplayController()
        {
            GameplayManager = Program.GetManager<GameplayManager>();
        }

        [HttpGet("GetAllPlayfieldNames")]
        public IActionResult GetAllPlayfieldNames()
        {
            return Ok(
                Directory.EnumerateDirectories(Path.Combine(EmpyrionConfiguration.SaveGamePath, "Templates"))
                .Select(D => Path.GetFileName(D))
                );
        }

        [HttpGet("GetAllItems")]
        public IActionResult GetAllItems()
        {
            return Ok(GameplayManager.GetAllItems());
        }

        public class WarpToData
        {
            public string Playfield { get; set; }
            public float PosX { get; set; }
            public float PosY { get; set; }
            public float PosZ { get; set; }
            public float RotX { get; set; }
            public float RotY { get; set; }
            public float RotZ { get; set; }
        }

        [HttpPost("PlayerWarpTo/{aEntityId}")]
        public async System.Threading.Tasks.Task<IActionResult> PlayerWarpToAsync(int aEntityId, [FromBody]WarpToData aWarpToData)
        {
            var playerInfo = await TaskWait.For(5, GameplayManager.Request_Player_Info(new Id(aEntityId)));

            var pos = new PVector3(aWarpToData.PosX, aWarpToData.PosY, aWarpToData.PosZ);
            var rot = new PVector3(aWarpToData.RotX, aWarpToData.RotY, aWarpToData.RotZ);

            await TaskWait.For(5, playerInfo.playfield == aWarpToData.Playfield
                ? GameplayManager.Request_Entity_Teleport         (new IdPositionRotation(aEntityId, pos, rot))
                : GameplayManager.Request_Player_ChangePlayerfield(new IdPlayfieldPositionRotation(aEntityId, aWarpToData.Playfield, pos, rot))
            );

            return Ok();
        }
    }
}
