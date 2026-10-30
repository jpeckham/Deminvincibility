> Current local branch: EFT **1.3.0.0.48484**, SPT 5.0, `spt-5.0-eft-1.3.0.0.48484`. The 48146 branch and installation remain preserved.

## October 29, 2026: 48484 rebuild

Build projects now default to `C:\sptarkov\SPTarkov 5.0.0-BE-1.3.0.0.48484`. Use .NET 10 and generated references from that installation; existing build commands below accept `-p:SPTPath` to override the target. Build with `-c Release -p:SkipDeploy=true`, then install the staged plugin directory. Do not use old 48146 references for this branch.

No gameplay logic changed for this rebuild. Existing local fixes and mod versions are retained. Installation and validation evidence are recorded in the parent SPT-BE README, section N8.

## Historical 48146 port notes

> Locked baseline: EFT **1.2.5.0.48146**, SPT 5.0. Newer EFT work must use a separate branch and installation. The original `spt-5.0-48146` branch remains available as historical provenance.

# Deminvincibility
A stripped out version of the "Keep 1 Health" section of [Props](https://hub.sp-tarkov.com/user/18746-props/)' [DadGamerMode](https://hub.sp-tarkov.com/files/file/926-dad-gamer-mode) ([click here for repo](https://github.com/dvize/DadGamerMode)).

With explicit permission from Props, this is being released as a standalone BepInEx mod based off of his as a learning experience for me to dive into BepInEx modding.

This branch targets the **local SPT 5.0 / EFT 1.2.5.0.48146** installation running BepInEx 6 on IL2CPP. It is not an official release by the original mod author.

It starts from [y1ngq1ang's SPT 5.0 port](https://github.com/y1ngq1ang/Deminvincibility/commit/156c012800a418a75649c7914825595d50f202c4), originally targeting 47965. That commit is retained in this branch's history. Local version **3.0.1** restores the original English settings/notifications, updates the build path and recorded EFT build, and retains that port's five active patches, including its ChangeHealth fix. The damage slider now permits 100%, so its intended default is no longer clamped to 99%. Patch behavior otherwise remains unchanged.

## Features
Currently, the following features are implemented:
- Keep 1 Health (on / off)
- Keep 1 Health selection (Head, Thorax, Head & Thorax, All)
- % damage received (means you will only take X% of the actual damage you receive)
- May be more in the near future

## How do I install this?
1. Close the SPT client.
2. Copy the built `Deminvincibility.dll` and `LICENSE.txt` into `<game root>/BepInEx/plugins/Deminvincibility/`. Do not extract a DLL with 7-Zip or put it inside `SPT_Runtime`.
3. Start SPT normally. Open F12, select **Deminvincibility**, and enable **1. Health → 1 HP Mode** if desired. The inherited default is **off**, as is second-chance protection.

The plugin lands in `BepInEx\plugins\`, and its settings show up under `F12` as `Deminvincibility`.

## Project Setup
1. Install the [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) — SPT 5.0 ships a .NET 10 runtime, so the plugin targets `net10.0`.
2. Point the build at your SPT 5.0 install. The default in `Deminvincibility.csproj` is `C:\sptarkov\SPTarkov 5.0.0-BE-1.2.5.0.48146`; override it per build if needed:
   ```
   dotnet build Deminvincibility.csproj -c Release -p:SPTPath="C:\sptarkov\SPTarkov 5.0.0-BE-1.2.5.0.48146"
   ```
3. All references are taken straight from that install, so there is nothing to copy into `\references`:
   - `\BepInEx\core\` — `BepInEx.Core.dll`, `BepInEx.Unity.IL2CPP.dll`, `0Harmony.dll`, `Il2CppInterop.*`
   - `\BepInEx\interop\` — the IL2CPP interop assemblies (`Assembly-CSharp.dll`, `PlayerEnums.dll`, `UnityEngine.*`, ...)
   - `\BepInEx\plugins\sptushonka\` — `SPTushonka.Reflection.dll` (Harmony patching helpers)

The built DLL is copied to `Build\BepInEx\plugins\` after each successful build; copy that into your SPT install to test.

## Porting notes (4.x -> 5.0)
- Plugin base class moved to `BepInEx.Unity.IL2CPP.BasePlugin`, using `Load()`.
- Patching helpers moved from `SPT.Reflection.Patching` to `SPTushonka.Reflection.Patching` (`ModulePatch`, `PatchPrefix`).
- The game is IL2CPP now, so the assemblies under `BepInEx\interop\` are interop stubs. Il2CppInterop turns instance fields into properties, so Harmony `___Field` injection no longer works — the patches read `__instance.Player` instead of an injected `Player` field.
- `EBodyPart` now lives in the global namespace (assembly `PlayerEnums.dll`) instead of `EFT`.

## Validation and limits

The community baseline and English local build both compile against the installed 48146 reference assemblies with zero errors and zero warnings. The active targets are `ActiveHealthController.ApplyDamage`, `ChangeHealth`, `DestroyBodyPart`, `DoFracture`, and `Kill`; the unused DoBleed patch remains disabled as in the community port.

A client startup test with LocalBuildTest logged `Loading [Deminvincibility 3.0.1]`, all five `Enabled patch` messages, and `Chainloader startup complete`. This verifies loading and patch installation, not in-raid invincibility or second-chance behavior. The test client and server were stopped afterward. Evidence is retained in the parent workspace's `.local-build-attempt/2026-10-28/deminvincibility-test` directory.

The old optional second-chance implementation is retained, including its process-wide state and asynchronous notification. This port does not claim to fix that feature's inherited behavior. The legacy `VersionChecker/setbuild.ps1` helper is not used by the SDK build and still expects an old AssemblyInfo layout; use the build command above.

## Credits
A huge thanks to the following people:
- Props for developing DadGamerMode. Without him and his experience, this wouldn't have been possible!

- [Dirtbikercj](https://hub.sp-tarkov.com/user/37201-dirtbikercj/) and [DrakiaXYZ](https://hub.sp-tarkov.com/user/30839-drakiaxyz/) from the [SPT Pub](https://discord.com/invite/Xn9msqQZan) for their troubleshooting and coding help. I owe everything to these people. <3
