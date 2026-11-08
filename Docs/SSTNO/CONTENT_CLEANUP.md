# SSTNO content cleanup, pass 1

Made by MickTheDick.

This is the first reversible cleanup of the MickTheDick/SSTNO fork. The fork started from space-wizards/space-station-14 `master`. Upstream license and copyright notices stay in place. Credit in this file does not replace them.

No `LORE.md` was provided, so this pass does not invent factions, history, or a regional map. The only new place a player can start is a small outdoor developer test area.

Upstream does not accept AI-generated contributions. Do not open an upstream pull request from this fork.

## Pins

- .NET SDK: `10.0.100`, from `global.json` (`rollForward` is `latestFeature`, so a later 10.0 SDK also satisfies the pin).
- RobustToolbox: submodule commit `a112cb8368a718c8ef468fb9891349eec12cff4e`, tag `v295.0.2`.

Initialize the engine with `python3 RUN_THIS.py` or `git submodule update --init --recursive RobustToolbox` before building.

## Start the developer baseline

From the repository root, after the submodule is present:

```shell
export PATH="$HOME/.dotnet:$PATH"
export DOTNET_ROOT="$HOME/.dotnet"
dotnet build Content.Server/Content.Server.csproj --nologo
dotnet run --project Content.Server/Content.Server.csproj --no-build
```

A Tools/development build loads `Resources/ConfigPresets/Build/development.toml`. That file turns the lobby off, so the server starts a round immediately. The round preset is `SSTNO` and the map is `SSTNODev`.

What you should see:

- The map name is **SSTNO Developer Test Area**.
- The character is human, with the Civilian role.
- Spawn is on a grass pad with air and gravity, next to a wooden shed, food, water, a medkit, hand tools, wood, steel, and cable. The pad is not a station and not a planet map.
- A paper in the shed says this is not the regional map.
- There is no emergency shuttle, no arrivals shuttle, and no central command.

Connect with the normal client (`dotnet run --project Content.Client/Content.Client.csproj`). The server listens on the default port in `Resources/ConfigPresets/server_config.toml` (1212). This pass does not advertise the server on the hub.

To force the same round from the server console if a local config overrode it:

```
setgamepreset SSTNO 0
forcemap SSTNODev
restartround
```

A packaged server should copy `Resources/ConfigPresets/server_config.toml`, which now carries the same preset, map, species, event, and shuttle defaults. `Resources/ConfigPresets/SSTNO.toml` is the same block and can be selected with `config.presets = "SSTNO"`.

## Deleted

Nothing was deleted from the shared prototype graph or the C# systems.

Every sci-fi leaf that was inspected is still referenced by an integration test that walks all maps, all presets, or all species, or it is still used by a shared system the test area and the admin tools load. Deleting those files would break the build or the existing test suite. They are excluded from play instead.

## Excluded from playable selection

These prototypes remain on disk. A normal round and the character editor do not offer them.

| Item | Change | Why it is not deleted |
| --- | --- | --- |
| Non-human round-start species: Arachnid, Diona, Dwarf, Moth, Reptilian, SlimePerson, Vox, Vulpkanin | `roundStart: false` | Body, marking, and guidebook prototypes still reference them. Gingerbread, Horror, and Skeleton were already `roundStart: false`. |
| Random species table | New `SSTNOSpeciesWeights` is human only. `ic.random_species_weights` points at it. The old `SpeciesWeights` table is unchanged. | Tests and admin tools can still index the old table. |
| Station departments (Cargo, Civilian, Command, Engineering, Medical, Science, Security, Service, Silicon, Specific) | `editorHidden: true` | Job prototypes are still used by station maps and tests. Central Command and Task Force were already hidden. |
| Character preferences for Traitor, Traitor sleeper, Nuclear operatives, Revolutionaries, Thief, Changeling, Zombies, Xenoborgs | `setPreference: false` | Antagonist systems and `AllGamePresetsStartTest` still construct those rules. Wizard preference was already commented out. Ninja, paradox clone, and silicon antagonist preferences were already false. |
| Vote list | `Secret` and `Deathmatch` `showInVote` set to false. `SSTNO` is the only preset with `showInVote: true`. Other presets were already hidden. | Admins can still `setgamepreset` a dormant preset. |
| Default job | New `SSTNOCivilian`. Overflow fallback moved off `Passenger`. New characters request `SSTNOCivilian`. | `Passenger` and the other station jobs still exist. |
| Starting gear | Brown jumpsuit, coat, black shoes, duffel, bread, apple, water, crowbar, wrench, screwdriver, wirecutter, welder, flashlight, knife. | No PDA, headset, ID, hardsuit, emergency oxygen box, or energy weapon. The clothes and the passenger job icon are reused upstream art, not a 1962 costume. |
| Intro text | `Resources/ServerInfo/Intro.txt` no longer welcomes the player to a Nanotrasen shift. | The guidebook XML files are untouched. |

## Disabled defaults

These C# defaults and the server config presets stop the station loop from starting. The systems themselves are still compiled.

| Default | Was | Now |
| --- | --- | --- |
| `game.defaultpreset` | `secret` | `SSTNO` |
| `game.fallbackpreset` | `Traitor,Extended` | `SSTNO` |
| `game.map` | empty (rotation) | `SSTNODev` |
| `game.map_pool` | `DefaultMapPool` | `SSTNOMapPool` |
| `game.new_character_jobs` | `Passenger` | `SSTNOCivilian` |
| `ic.random_species_weights` | `SpeciesWeights` | `SSTNOSpeciesWeights` |
| `events.enabled` | true | false |
| `shuttle.emergency` | true | false |
| `shuttle.arrivals` | true | false |
| `shuttle.grid_fill` | true | false |
| `shuttle.auto_call_time` | 90 | 0 |
| Lobby disabled | forced preset `sandbox` | uses `game.defaultpreset` |

`SSTNODeveloperStation` does not parent the Nanotrasen station, cargo, arrivals, shuttle fill, central command, evacuation, salvage, silicon law, station-event eligibility, or delivery components. `StandardNanotrasenStation` is unchanged and is what the old maps still use.

The developer map file `Resources/Maps/_SSTNO/developer_test.yml` is a renamed copy of the upstream empty debug grid, with its spawn points removed. On station setup that same grid is painted with `FloorPlanetGrass` in a 16-tile radius, its shuttle component is removed, and it is set static with gravity. The map gets a breathable oxygen-nitrogen mix and daylight. A wooden shed, food, water, a medkit, and construction supplies are spawned on the pad. This is a developer pad, not a planet and not the regional map. A full outdoor region is follow-up work: the map-load test expects the station grid itself to carry the spawn points, and a separate planet grid failed that test.

## Retained on purpose

- RobustToolbox networking, serialization, and the content client/server entry points.
- Database, authentication, bans, admin tools, admin logs, and ahelp.
- Human body, appearance, movement, inventory, and hands.
- Medical treatment, damage, and the filled medkit placed in the shed.
- Hunger and thirst. The civilian loadout carries bread, an apple, and water.
- Construction graphs, wood and steel sheets, doors, and cable.
- Power, atmos, and door code. The test area does not build a powered grid; cable and hand tools are on the ground so those systems can be tried.
- Melee combat. The loadout carries a knife. Gun and laser prototypes are not equipped.
- Mapping, prototypes, and the admin sandbox preset (`setgamepreset sandbox`). Sandbox is no longer what a lobby-disabled server starts.

## Dormant content that still loads

Still present, not selected by the SSTNO round:

- Station maps under `Resources/Maps/` and their prototypes in `Resources/Prototypes/Maps/`. `DefaultMapPool` still lists Bagel, Box, Elkridge, Marathon, Packed, Plasma, Exo, Snowball, Sushi, and Tram2.
- Shuttle, ruin, salvage, and emergency-shuttle maps.
- Presets Secret, Traitor, Nuclear operatives, Revolutionary, Wizard, Zombie, Xenoborgs, Deathmatch, Extended, Greenshift, Survival, Kessler, Dynamic, and All At Once.
- Station event, meteor, and space-traffic schedulers. They are not in the SSTNO rules, and `events.enabled` is false.
- Antagonist, wizard, revenant, ninja, dragon, changeling, zombie, and xenoborg C# systems.
- Nanotrasen and Syndicate objectives, uplinks, and ghost roles.
- Non-human species prototypes, markings, and organs.
- Station jobs, silicon laws, cargo, research, anomalies, and the guidebook pages that describe them.
- Upstream `Resources/ServerInfo/Sandbox.txt` and the guidebook. Only `Intro.txt` was rewritten.

An admin who loads `Box` and `setgamepreset traitor` still runs that old round. That is intentional. The files were not stripped, so the cut can be reversed by restoring the CVars and the `roundStart` / `showInVote` / `editorHidden` / `setPreference` flags.

## Follow-up pruning

Do these only after the tests that walk every map, preset, and species are pointed at a kept subset. Do not delete them just to shorten the tree.

1. `Resources/Maps/` station, shuttle, ruin, and salvage maps, plus `Resources/Prototypes/Maps/` entries other than `SSTNODev`.
2. `Resources/Prototypes/game_presets.yml` entries other than `SSTNO`, and `Resources/Prototypes/Roles/Antags/`.
3. `Content.Server/GameTicking/Rules/` antagonist rule systems, `Content.Server/Antag/`, and the wizard, revenant, zombie, xenoborg, and changeling systems.
4. `Resources/Prototypes/Species/` other than human, and the matching mob, marking, and organ prototypes.
5. Station job prototypes under `Resources/Prototypes/Roles/Jobs/` other than `SSTNOCivilian`, then the hidden departments.
6. `Resources/Prototypes/Entities/Stations/nanotrasen.yml` parents that only exist for the station loop: cargo, arrivals, evacuation, central command, salvage, silicon law, deliveries.
7. `Resources/Prototypes/GameRules/` station events, meteors, and space traffic.
8. Guidebook XML that only describes station departments, species, shuttles, and antagonists.
9. Sci-fi clothing, hardsuits, energy weapons, PDAs, and headsets once nothing in the kept loadout or construction graph references them.
10. A real regional map with separate camps, a town, and roads. That map does not exist yet. Do not rename the developer test area into it.

## Verification run on this pass

Commands were run with SDK `10.0.100` and RobustToolbox `v295.0.2` (`a112cb8368a718c8ef468fb9891349eec12cff4e`).

```shell
dotnet build Content.IntegrationTests/Content.IntegrationTests.csproj --nologo
dotnet test Content.IntegrationTests/Content.IntegrationTests.csproj --filter "FullyQualifiedName~SstnoDeveloperRoundTest" --nologo
dotnet test Content.IntegrationTests/Content.IntegrationTests.csproj --filter "FullyQualifiedName~FailAndStartTest" --nologo
dotnet test Content.IntegrationTests/Content.IntegrationTests.csproj --filter "FullyQualifiedName~GameMapsLoadableTest&FullyQualifiedName~SSTNODev" --nologo
timeout --signal=TERM 80s dotnet run --project Content.Server/Content.Server.csproj --no-build --nologo
```

Results:

- The integration-test build succeeded. The first compile failed on a local-name clash in `SstnoDeveloperTestAreaSystem` and was fixed before these runs.
- `SstnoDeveloperRoundTest` passed. A connected client became `MobHuman` on a non-empty tile, on a grid with gravity and without `ShuttleComponent`. The station had no emergency shuttle, arrivals, or central command.
- `FailAndStartTest` passed after the default preset changed from `secret` to `SSTNO`.
- `GameMapsLoadableTest("SSTNODev")` passed. An earlier planet-grid attempt failed this test because the map-load harness requires the station grid itself to own the spawn points.
- Headless server: loaded `development.toml`, logged `Starting round!`, created station `SSTNO Developer Test Area`, attempted preset `SSTNO`, and logged `Server Version 295.0.2.0 -> Ready`. No `[ERRO]` or `[FATL]` lines. The process was stopped by `timeout` (exit 124). It bound port 1212 on the local machine and was not advertised.
- The full integration suite and `Content.YAMLLinter` were not run. Prototype load during the tests and the server start reported no prototype errors. SQLite migration warnings from upstream (`db.ef`, non-transactional `PRAGMA`) appeared and were already in the codebase.

## Tests that still load station content

`Content.IntegrationTests` sets `game.map` to `Empty` and disables the development preset. `Empty` is still `StandardNanotrasenStation`. That is why the shared tests can keep passing while a normal server start does not load a station. `SstnoDeveloperRoundTest` is the check that the SSTNO round itself stays off that station.
