# Blazor PWA Games

Interactive small games built with Blazor WebAssembly as a Progressive Web App (PWA).

Overview
--------

This repository contains small, self-contained games built using Blazor (WASM) and designed to run as a PWA so they can be installed on desktop and mobile devices. The project is structured so each game lives in the `Frontend/Pages` (or a subfolder) as a Razor component. Over time this repo will grow into a collection of tiny single-file games and educational demos.

Current hosted site
-------------------

The project is hosted on GitHub Pages at:

https://dnawrkshp.github.io/blazor-pwa-games/

Games
-------------------

- War — simple card game of war — `/war`

Planned / Ideas
---------------

- Klondike
- Tic-tac-toe

Quick start (developer)
-----------------------

Prerequisites

- .NET SDK 9.0 or compatible (verify with `dotnet --version`)
- A modern browser (Chrome, Edge, Firefox, Safari)

Run locally (development)

Open a terminal in the repository root and run the Frontend project:

```powershell
cd Frontend
dotnet run
```

After the project starts, open the URL shown in the terminal (usually `http://localhost:5139`).

Build for production

```powershell
dotnet publish Frontend -c Release -o publish
```

This will produce the files under `/publish/wwwroot` (or similar), which can be served by static hosting (GitHub Pages, Netlify, etc.).

How to add a game
------------------

1. Create a new Razor component in `Frontend/Pages` (or a subfolder).
2. Add a `@page "/your-route"` directive to expose the game at a URL.
3. Keep the component small and self-contained — put assets in `wwwroot` or use embedded CSS scoped to the component.
4. Add an entry to this README's game list and consider adding a screenshot to `README/assets` (optional).
