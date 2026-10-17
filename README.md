# FlexDaily

Personal offline-first Blazor WebAssembly PWA for mobility routines.

## Features
- 5/10/15/20 minute randomized routines
- Hip/groin, back, legs, full body focus
- Inline SVG exercise visual guides
- Guided countdown with pause, skip, auto-advance, optional sound and transitions
- IndexedDB session history, streaks, tightness ratings, JSON backup

## Run
Install .NET 10 SDK, then run `dotnet restore` and `dotnet run`.

## Publish
`dotnet publish -c Release -o publish`; host the contents of `publish/wwwroot` over HTTPS.
On Android Chrome use **Install app** or **Add to Home screen**.

Data stays in this browser/device and is not synchronized. Keep backups. Exercise drawings are schematic position guides. Avoid painful movements.

## Validation
The code has not been compiled in the authoring environment. Run a local build and test PWA offline behavior before production deployment.
