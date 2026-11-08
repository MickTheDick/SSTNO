<div align="center">

# SSTNO

*A grounded human fork of Space Station 14, still early and not for public play.*

### Proudly made by AI

### Made by MickTheDick

[![Status: early development](https://img.shields.io/badge/status-early_development-6b4f3a)](Docs/SSTNO/CONTENT_CLEANUP.md)
[![Round: developer pad](https://img.shields.io/badge/round-developer_pad-3d5c45)](Docs/SSTNO/CONTENT_CLEANUP.md)
[![Project: unofficial fan work](https://img.shields.io/badge/project-unofficial_fan_work-4a4a4a)](https://github.com/space-wizards/space-station-14)

</div>

SSTNO is an unofficial total conversion inspired by *The New Order: Last Days of Europe*. It is not an official TNO or Hearts of Iron IV project. The intended play is civilian survival and human faction conflict in fictional West Russian borderlands around 1962. That region is not built.

New SSTNO writing and fork setup were made with AI assistance under MickTheDick's direction. Space Station 14's code and assets stay the work of their original authors.

## Now and later

| | In the build | Planned, not built |
| --- | --- | --- |
| Place | Grass developer pad, `SSTNODev` | Fictional borderland: town, second site, faction bases, supply routes |
| People | Human Civilian | Four human factions and local civilian jobs |
| Round | Preset `SSTNO`. No shuttle, no station antagonists in the normal round | Faction conflict. Not a mode yet |

Station maps, non-human species, and antagonist files are still in the tree. They are left out of the normal round, not deleted. Detail: [Docs/SSTNO/CONTENT_CLEANUP.md](Docs/SSTNO/CONTENT_CLEANUP.md). Proposed setting: [LORE.md](LORE.md).

## Planned factions

Not selectable. No strongholds exist.

| Faction | Draft base | Motto in the draft |
| --- | --- | --- |
| Vyatka monarchists | Fortified estate, north | For Faith, Tsar and Fatherland. |
| Samara liberation army | Rail depot, south | Neither Hitler nor Stalin. |
| Red partisans | Forest bunkers | The Front has not fallen while one of us still fights. |
| Tatar defenders | Fortified village | This is our land. You are guests on it. |

The partisan line is SSTNO fictional flavor, not a historical quotation and not a line from TNO.

## Quick start

.NET SDK `10.0.100` (`global.json`) and RobustToolbox `v295.0.2` (`a112cb8368a718c8ef468fb9891349eec12cff4e`).

```shell
python3 RUN_THIS.py
dotnet build Content.Server/Content.Server.csproj --nologo
dotnet run --project Content.Server/Content.Server.csproj --no-build
```

A Tools build reads `Resources/ConfigPresets/Build/development.toml`, skips the lobby, and starts `SSTNO` on `SSTNODev`. Client: `dotnet run --project Content.Client/Content.Client.csproj`. Port 1212 comes from `Resources/ConfigPresets/server_config.toml`. The server is not advertised on the hub.

## Credits and licenses

- Code license: [LICENSE.TXT](LICENSE.TXT) (MIT), Copyright (c) 2017-2026 Space Wizards Federation. Upstream: [space-wizards/space-station-14](https://github.com/space-wizards/space-station-14), engine [RobustToolbox](https://github.com/space-wizards/RobustToolbox).
- Most upstream assets are [CC BY-SA 3.0](https://creativecommons.org/licenses/by-sa/3.0/) unless a file says otherwise. Some are [CC BY-NC-SA 3.0](https://creativecommons.org/licenses/by-nc-sa/3.0/) and are not cleared for commercial use. Example: [crowbar metadata](https://github.com/space-wizards/space-station-14/blob/master/Resources/Textures/Objects/Tools/crowbar.rsi/meta.json). Notes: [Robust Generic Attribution](https://docs.spacestation14.com/en/specifications/robust-generic-attribution.html), [Robust Station Image](https://docs.spacestation14.com/en/specifications/robust-station-image.html).
- Upstream does not accept AI-generated contributions. Do not open a pull request or issue on `space-wizards/space-station-14` from this fork.
- TNO artwork is not included; reuse permission has not been established. Workshop page for the mod that inspired the setting: [The New Order: Last Days of Europe](https://steamcommunity.com/sharedfiles/filedetails/?id=2438003901). SSTNO is not affiliated with or endorsed by that project, Paradox Interactive, or Hearts of Iron IV.

<div align="center">

**Proudly made by AI**

**Made by MickTheDick**

</div>
