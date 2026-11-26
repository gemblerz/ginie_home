# Spatial Reality Display Mini (ELF-MN1) Unity Starter

This repository is a Unity starter project to render an example 3D character on **Sony Spatial Reality Display Mini (ELF-MN1)** using the Sony SDK, with a few ready-to-run motion presets.

It contains:
- Unity project scaffolding (`Packages/`, `ProjectSettings/`)
- SRD bootstrap and diagnostics scripts
- Demo character spawning
- Motion presets (Idle bob, Figure-8, Orbit, Hop)
- A practical workflow for replacing the fallback character with your own Unity character

---

## 1) Prerequisites

1. **Hardware**
   - Sony Spatial Reality Display Mini (**ELF-MN1**)
   - Compatible PC and cable connection as listed by Sony
2. **Software**
   - Unity **2022.3 LTS** (recommended in this repo: `2022.3.40f1`)
   - Latest Sony Spatial Reality Display Mini SDK + runtime/driver from the official Sony download page  
     https://xyn.sony.net/en/developer/setup/spatial-reality-display-mini/download-info
   - Official Unity samples reference  
     https://xyn.sony.net/en/developer/setup/spatial-reality-display-mini/unity/samples
3. **Environment**
   - The display should be connected and recognized by the OS before running Unity Play Mode.

> Note: Network access to Sony’s host was unavailable in this coding environment while preparing this repo. The implementation here follows the standard SRD Unity integration pattern; please verify final SDK/version-specific details against Sony’s page above.

---

## 2) Project layout

```text
Assets/
  Scripts/
    SRDMini/
      SrdMiniDisplayBootstrap.cs
      DemoCharacterSpawner.cs
      CharacterMotionController.cs
      MotionPresetCycler.cs
Packages/
  manifest.json
ProjectSettings/
  ProjectVersion.txt
```

---

## 3) Initial setup in Unity

1. Open this repository folder in Unity Hub.
2. Let Unity import packages.
3. Import the official **Sony SRD Mini SDK** package into this project (per Sony download instructions).
4. In a new scene:
   - Keep/add one **Main Camera**
   - Add one empty GameObject: `SRD_Bootstrap`
   - Add component: `SrdMiniDisplayBootstrap`
   - Add one empty GameObject: `DemoSpawner`
   - Add component: `DemoCharacterSpawner`
5. Press **Play**.

Expected result:
- If SDK is present, the script attempts to attach Sony’s SRD camera component to `Main Camera`.
- A fallback demo character is created and animated with motion presets.

---

## 4) How to run on ELF-MN1

1. Power on and connect ELF-MN1.
2. Start the Sony SRD runtime/monitor app if required by your SDK version.
3. Open the Unity scene and press Play.
4. Position your character around Z ~ `0.4` to `0.8` meters in front of camera origin (demo default is `z=0.6`).
5. Use number keys during Play Mode:
   - `1`: IdleBob
   - `2`: Figure8
   - `3`: Orbit
   - `4`: Hop

If no stereoscopic result appears:
- Confirm SDK import succeeded (check Console logs from `SrdMiniDisplayBootstrap`).
- Confirm display runtime is active.
- Confirm camera has an SRD camera component attached at runtime.

---

## 5) Replacing demo with your own character

In `DemoCharacterSpawner`:
- Assign your prefab to **Character Prefab**.
- Keep `CharacterMotionController` for procedural motion, or disable/remove it if your character uses Animator clips only.

Recommended prefab setup:
- Root transform at feet/base center
- Forward direction consistent (+Z in authoring or corrected by `spawnEuler`)
- Reasonable scale (~`1.0` unit = 1 meter)

---

## 6) Motion system in this repo

`CharacterMotionController` exposes four procedural presets:
- **IdleBob**: subtle bob + yaw sway
- **Figure8**: lateral/in-depth figure-8 path
- **Orbit**: circular path + continuous yaw
- **Hop**: repeated vertical hop + slight tilt

`MotionPresetCycler` can:
- Auto-cycle every few seconds
- Switch presets manually via keyboard (`1..4`)

---

## 7) Developing production 3D characters for SRD in Unity

### Asset and rig guidance
- Prefer skinned, rigged characters with clean topology.
- Keep center of mass and pivot predictable.
- Use physically plausible scale to avoid depth discomfort.

### Rendering guidance for SRD
- Keep important elements near the comfortable depth range around focal content.
- Avoid extremely rapid Z-motion toward viewer.
- Use soft lighting and readable silhouettes; heavy post FX can reduce depth clarity.

### Animation guidance
- Start with slower cadence and moderate amplitude.
- Layer idle motion even when using keyframed clips (prevents static look in stereo view).
- Validate animation in device view, not just Game view.

### Runtime/system guidance
- Keep a stable frame rate target (this repo forces 60 FPS by default).
- Minimize spikes (GC allocations, expensive per-frame Instantiate/Destroy).
- Use LODs and balanced shader complexity for sustained performance.

---

## 8) Script details

### `SrdMiniDisplayBootstrap`
- Detects and attaches SRD camera component via reflection.
- Works safely even if SDK assemblies are missing.
- Logs detected SRD/Sony assemblies for diagnosis.

### `DemoCharacterSpawner`
- Spawns your prefab, or a fallback capsule/sphere character if none assigned.
- Auto-adds motion scripts.

### `CharacterMotionController`
- Procedural motion generator with tunable amplitude/speed.

### `MotionPresetCycler`
- Input + auto-cycle controller for quick demos.

---

## 9) Known limitations

- The exact SRD assembly/type names can vary across SDK versions.  
  `SrdMiniDisplayBootstrap` includes multiple candidates and fallback type discovery.
- This repo does not include Sony SDK binaries; you must import them from Sony’s official download channel.

---

## 10) Suggested next steps

1. Import your own humanoid/creature prefab.
2. Add Animator Controller clips for polished motions.
3. Keep `MotionPresetCycler` for debug demos, and replace with gameplay/state logic for production.
4. Cross-check scene/camera configuration against Sony’s official Unity samples and mirror those settings for your SDK version.