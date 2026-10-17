# Deminvincibility
A stripped out version of the "Keep 1 Health" section of [Props](https://hub.sp-tarkov.com/user/18746-props/)' [DadGamerMode](https://hub.sp-tarkov.com/files/file/926-dad-gamer-mode) ([click here for repo](https://github.com/dvize/DadGamerMode)).

With explicit permission from Props, this is being released as a standalone BepInEx mod based off of his as a learning experience for me to dive into BepInEx modding.

Built for **SPT 5.0** (EFT 1.2.0.47965), which runs BepInEx 6 on IL2CPP.

## Features
Currently, the following features are implemented:
- Keep 1 Health (on / off)
- Keep 1 Health selection (Head, Thorax, Head & Thorax, All)
- % damage received (means you will only take X% of the actual damage you receive)
- May be more in the near future

## How do I install this?
1. Open `Deminvincibility.zip` (or the built `Deminvincibility.dll`) with 7-Zip
2. Drag and drop the `BepInEx` folder straight into your SPT folder (the one that holds `SPT.Launcher.exe` / `SPT.Server.exe`)
3. Profit

The plugin lands in `BepInEx\plugins\`, and its settings show up under `F12` as `Deminvincibility`.

## Project Setup
1. Install the [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) — SPT 5.0 ships a .NET 10 runtime, so the plugin targets `net10.0`.
2. Point the build at your SPT 5.0 install. The default in `Deminvincibility.csproj` is `C:\SPT-5.0-BE`; either edit the `SPTPath` property or pass it per build:
   ```
   dotnet build -p:SPTPath="D:\SPT 5.0"
   ```
3. All references are taken straight from that install, so there is nothing to copy into `\references`:
   - `\BepInEx\core\` — `BepInEx.Core.dll`, `BepInEx.Unity.IL2CPP.dll`, `0Harmony.dll`, `Il2CppInterop.*`
   - `\BepInEx\interop\` — the IL2CPP interop assemblies (`Assembly-CSharp.dll`, `PlayerEnums.dll`, `UnityEngine.*`, ...)
   - `\BepInEx\plugins\sptushonka\` — `SPTushonka.Reflection.dll` (Harmony patching helpers)

The built DLL is copied to `Build\BepInEx\plugins\` after each successful build; copy that into your SPT install to test.

## Porting notes (4.x -> 5.0)
- Plugin base class moved to `BepInEx.Unity.IL2CPP.BaseUnityPlugin`.
- Patching helpers moved from `SPT.Reflection.Patching` to `SPTushonka.Reflection.Patching` (`ModulePatch`, `PatchPrefix`).
- The game is IL2CPP now, so the assemblies under `BepInEx\interop\` are interop stubs. Il2CppInterop turns instance fields into properties, so Harmony `___Field` injection no longer works — the patches read `__instance.Player` instead of an injected `Player` field.
- `EBodyPart` now lives in the global namespace (assembly `PlayerEnums.dll`) instead of `EFT`.

## Credits
A huge thanks to the following people:
- Props for developing DadGamerMode. Without him and his experience, this wouldn't have been possible!

- [Dirtbikercj](https://hub.sp-tarkov.com/user/37201-dirtbikercj/) and [DrakiaXYZ](https://hub.sp-tarkov.com/user/30839-drakiaxyz/) from the [SPT Pub](https://discord.com/invite/Xn9msqQZan) for their troubleshooting and coding help. I owe everything to these people. <3
