# SSTNO

**Proudly made by AI**

**Made by MickTheDick**

SSTNO is an unofficial, early-development total conversion of [Space Station 14](https://github.com/space-wizards/space-station-14). It is inspired by *The New Order: Last Days of Europe*, a Hearts of Iron IV mod. It is not an official TNO or HOI4 project, it is not finished, and it is not ready for public play.

The intended setting is civilian survival and conflict among human factions in fictional West Russian borderlands around 1962. Separate faction bases, a civilian town, a secondary settlement, and contested supply routes are plans. They are not in the game.

New SSTNO writing and the fork-specific setup were produced with AI assistance under MickTheDick's direction. That credit does not cover Space Station 14. The engine, gameplay code, and assets inherited from upstream were written by their original human authors and stay under their licenses.

## What exists now

The playable SSTNO baseline is a developer test pad, not a region.

- Preset `SSTNO`, map `SSTNODev`, role Civilian, human characters only in the normal round.
- A grass pad with air, gravity, a wooden shed, food, water, a medkit, and hand tools.
- Station presets, non-human species, and antagonist preferences are hidden from the normal round. Those files were not deleted. Shared medical, construction, power, combat, and admin systems are still in the tree.

Start and cleanup notes: [Docs/SSTNO/CONTENT_CLEANUP.md](Docs/SSTNO/CONTENT_CLEANUP.md).

## What is only a plan

[`LORE.md`](LORE.md) is a proposed setting draft. It is not verified TNO canon and it is not implemented.

The draft imagines four human factions around a fictional Kama–Volga border: Vyatka monarchists, the Samara liberation army, Red partisans, and Tatar defenders, plus a neutral town, an outlying works, and the roads between them. None of those places or roles can be played yet. The only civilian role in the build is the developer-pad Civilian.

> "The Front has not fallen while one of us still fights."
>
> SSTNO fictional flavor, written for this fork as if spoken by a Red partisan in the Kama forests in 1962. It is not a historical quotation and it is not a line from TNO.

## Quick start

Pins: .NET SDK `10.0.100` (`global.json`, `rollForward` `latestFeature`) and RobustToolbox `v295.0.2` (`a112cb8368a718c8ef468fb9891349eec12cff4e`).

```shell
python3 RUN_THIS.py
dotnet build Content.Server/Content.Server.csproj --nologo
dotnet run --project Content.Server/Content.Server.csproj --no-build
```

A Tools build loads `Resources/ConfigPresets/Build/development.toml`, skips the lobby, and starts `SSTNO` on `SSTNODev`. The client is `dotnet run --project Content.Client/Content.Client.csproj`. The server uses port 1212 from `Resources/ConfigPresets/server_config.toml` and is not advertised on the hub.

## The New Order image

No TNO image is embedded in this repository.

The TNO logo and other mod art are not free to copy. A Victoria 3 fan project states that the TNO team granted permission before that project used TNO art, which is evidence that permission is required. Attribution by itself is not permission, so the logo is not stored here. Official mod page: [The New Order: Last Days of Europe on Steam Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=2438003901).

A fan flag-map, [File:TNOflagMap.webp](https://web.archive.org/web/20250529201633/https://commons.wikimedia.org/wiki/File:TNOflagMap.webp), was published by AvailableViking on Wikimedia Commons on 15 June 2023 as their own work under [CC BY-SA 4.0](https://creativecommons.org/licenses/by-sa/4.0/). The live Commons file now returns 404, and the archived image file could not be downloaded for this page. That map is also a world flag map, so it would show faction flags, including extremist insignia, as decoration. It is not used.

SSTNO does not claim ownership, a license, or official endorsement of any TNO art. Inherited Space Station 14 assets keep the upstream licenses below. Nothing in this README applies the MIT license to TNO material.

## Credits and upstream license

- Space Station 14 and Robust Toolbox: [space-wizards/space-station-14](https://github.com/space-wizards/space-station-14) and [space-wizards/RobustToolbox](https://github.com/space-wizards/RobustToolbox), by Space Wizards Federation and the other authors named in the upstream history. Code license: [LICENSE.TXT](LICENSE.TXT) (MIT).
- Most upstream assets are [CC BY-SA 3.0](https://creativecommons.org/licenses/by-sa/3.0/) unless a file says otherwise. Some are [CC BY-NC-SA 3.0](https://creativecommons.org/licenses/by-nc-sa/3.0/) and are not cleared for commercial use. Example metadata: [crowbar `meta.json`](https://github.com/space-wizards/space-station-14/blob/master/Resources/Textures/Objects/Tools/crowbar.rsi/meta.json).
- Upstream attribution notes: [Robust Generic Attribution](https://docs.spacestation14.com/en/specifications/robust-generic-attribution.html), [Robust Station Image](https://docs.spacestation14.com/en/specifications/robust-station-image.html). Build guide: [Setting up a development environment](https://docs.spacestation14.com/en/general-development/setup.html).
- Upstream does not accept AI-generated contributions. Do not open a pull request or issue on `space-wizards/space-station-14` from this fork.
- *The New Order: Last Days of Europe* is by the TNO development team. SSTNO is not affiliated with or endorsed by them, by Paradox Interactive, or by Hearts of Iron IV.

**Made by MickTheDick. Proudly made by AI.**
